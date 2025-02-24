using TodoApi.Data;
using TodoApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("TodoContext")));

builder.Configuration.AddJsonFile("secrets.json", optional: false, reloadOnChange: true);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(x =>
    {
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = "TodoApi",
            ValidateIssuer = true,
            ValidateAudience = false,  // TODO fix the audience (and read up)
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(builder.Configuration["JWT:AccessTokenSecret"]!)),
            ClockSkew = TimeSpan.Zero,
        };
    });

builder.Services.AddAuthorization(options => {
    options.AddPolicy("AccessToken", policy =>
        policy.RequireClaim("TokenType", "AccessToken"));
    
    options.AddPolicy("RefreshToken", policy =>
        policy.RequireClaim("TokenType", "RefreshToken"));
});
builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

static void CreateDbIfNotExists(IHost host)
{
    using IServiceScope scope = host.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;
    try
    {
        TodoContext context = services.GetRequiredService<TodoContext>();
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred creating the DB.");
    }
}

CreateDbIfNotExists(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
