<script lang="ts">
    export let isChecked: boolean = false;
</script>

<div class="spinner-checkbox-wrapper py-8">
	<div class="spinner-checkbox{isChecked ? ' checked' : ''}">
		<div class="spinner-circle"></div>
		<div class="check-icon">
			<span class="icon-line line-tip"></span>
			<span class="icon-line line-long"></span>
			<div class="icon-circle"></div>
			<div class="icon-fix"></div>
		</div>
	</div>
</div>

<style>
	.spinner-checkbox {
		width: 80px;
		height: 80px;
		margin: auto;
		position: relative;
	}
	.spinner-checkbox .spinner-circle {
		width: 100%;
		height: 100%;
		position: absolute;
		border-radius: 50%;
		box-sizing: border-box;
		border: 4px solid #4caf50;
		border-top-color: transparent;
		animation: spinner-rotate 1s linear infinite;
	}
	.spinner-checkbox::before,
	.spinner-checkbox::after {
		content: '';
		position: absolute;
		top: -4px;
		left: -4px;
		width: calc(100% + 8px);
		height: calc(100% + 8px);
		border-radius: 50%;
		box-sizing: border-box;
		border: 4px solid rgba(76, 175, 80, 0.5);
		border-top-color: transparent;
		animation: spinner-rotate 4.25s ease-in-out infinite;
	}

	.spinner-checkbox::before {
		animation-delay: -0.5s;
	}

	.spinner-checkbox .check-icon {
		width: 80px;
		height: 80px;
		position: absolute;
		opacity: 0;
		transition: opacity 0.75s ease-out;
	}

	.spinner-checkbox.checked .check-icon {
		opacity: 1;
        transition: opacity 0.5s ease-out;
	}
	.spinner-checkbox .check-icon .icon-line {
		height: 5px;
		background-color: #4caf50;
		display: block;
		border-radius: 2px;
		position: absolute;
		z-index: 10;
	}
	.spinner-checkbox .check-icon .icon-line.line-tip {
		top: 46px;
		left: 14px;
		width: 25px;
		transform: rotate(45deg);
		animation: icon-line-tip 0.75s;
	}
	.spinner-checkbox .check-icon .icon-line.line-long {
		top: 38px;
		right: 8px;
		width: 47px;
		transform: rotate(-45deg);
		animation: icon-line-long 0.75s;
	}
	.spinner-checkbox .check-icon .icon-circle {
		top: -4px;
		left: -4px;
		z-index: 10;
		width: 80px;
		height: 80px;
		border-radius: 50%;
		position: absolute;
		box-sizing: content-box;
		border: 4px solid rgba(76, 175, 80, 0.5);
	}

	.spinner-checkbox .check-icon .icon-fix {
		top: 8px;
		width: 5px;
		left: 26px;
		z-index: 1;
		height: 85px;
		position: absolute;
		transform: rotate(-45deg);
	}

	.spinner-checkbox.checked .spinner-circle,
	.spinner-checkbox.checked::before,
	.spinner-checkbox.checked::after {
		animation: none;
		border-color: transparent;
		transition: border 0.75s ease-out;
	}

	@keyframes spinner-rotate {
		0% {
			transform: rotate(0deg);
		}
		100% {
			transform: rotate(360deg);
		}
	}

	@keyframes icon-line-tip {
		0% {
			width: 0;
			left: 1px;
			top: 19px;
		}
		54% {
			width: 0;
			left: 1px;
			top: 19px;
		}
		70% {
			width: 50px;
			left: -8px;
			top: 37px;
		}
		84% {
			width: 17px;
			left: 21px;
			top: 48px;
		}
		100% {
			width: 25px;
			left: 14px;
			top: 45px;
		}
	}

	@keyframes icon-line-long {
		0% {
			width: 0;
			right: 46px;
			top: 54px;
		}
		65% {
			width: 0;
			right: 46px;
			top: 54px;
		}
		84% {
			width: 55px;
			right: 0px;
			top: 35px;
		}
		100% {
			width: 47px;
			right: 8px;
			top: 38px;
		}
	}
</style>
