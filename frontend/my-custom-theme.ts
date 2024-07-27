import type { CustomThemeConfig } from '@skeletonlabs/tw-plugin';

export const myCustomTheme: CustomThemeConfig = {
	name: 'my-custom-theme',
	properties: {
		// =~= Theme Properties =~=
		'--theme-font-family-base': `Inter, ui-sans-serif, system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, 'Helvetica Neue', Arial, 'Noto Sans', sans-serif, 'Apple Color Emoji', 'Segoe UI Emoji', 'Segoe UI Symbol', 'Noto Color Emoji'`,
		'--theme-font-family-heading': `system-ui`,
		'--theme-font-color-base': '0 0 0',
		'--theme-font-color-dark': '255 255 255',
		'--theme-rounded-base': '9999px',
		'--theme-rounded-container': '8px',
		'--theme-border-base': '1px',
		// =~= Theme On-X Colors =~=
		'--on-primary': '0 0 0',
		'--on-secondary': '0 0 0',
		'--on-tertiary': '0 0 0',
		'--on-success': '0 0 0',
		'--on-warning': '0 0 0',
		'--on-error': '0 0 0',
		'--on-surface': '0 0 0',
		// =~= Theme Colors  =~=
		// primary | #EE6F57
		'--color-primary-50': '252 233 230', // #fce9e6
		'--color-primary-100': '252 226 221', // #fce2dd
		'--color-primary-200': '251 219 213', // #fbdbd5
		'--color-primary-300': '248 197 188', // #f8c5bc
		'--color-primary-400': '243 154 137', // #f39a89
		'--color-primary-500': '238 111 87', // #EE6F57
		'--color-primary-600': '214 100 78', // #d6644e
		'--color-primary-700': '179 83 65', // #b35341
		'--color-primary-800': '143 67 52', // #8f4334
		'--color-primary-900': '117 54 43', // #75362b
		// secondary | #A9BAE6
		'--color-secondary-50': '242 245 251', // #f2f5fb
		'--color-secondary-100': '238 241 250', // #eef1fa
		'--color-secondary-200': '234 238 249', // #eaeef9
		'--color-secondary-300': '221 227 245', // #dde3f5
		'--color-secondary-400': '195 207 238', // #c3cfee
		'--color-secondary-500': '169 186 230', // #A9BAE6
		'--color-secondary-600': '152 167 207', // #98a7cf
		'--color-secondary-700': '127 140 173', // #7f8cad
		'--color-secondary-800': '101 112 138', // #65708a
		'--color-secondary-900': '83 91 113', // #535b71
		// tertiary | #58EDB2
		'--color-tertiary-50': '230 252 243', // #e6fcf3
		'--color-tertiary-100': '222 251 240', // #defbf0
		'--color-tertiary-200': '213 251 236', // #d5fbec
		'--color-tertiary-300': '188 248 224', // #bcf8e0
		'--color-tertiary-400': '138 242 201', // #8af2c9
		'--color-tertiary-500': '88 237 178', // #58EDB2
		'--color-tertiary-600': '79 213 160', // #4fd5a0
		'--color-tertiary-700': '66 178 134', // #42b286
		'--color-tertiary-800': '53 142 107', // #358e6b
		'--color-tertiary-900': '43 116 87', // #2b7457
		// success | #43e545
		'--color-success-50': '227 251 227', // #e3fbe3
		'--color-success-100': '217 250 218', // #d9fada
		'--color-success-200': '208 249 209', // #d0f9d1
		'--color-success-300': '180 245 181', // #b4f5b5
		'--color-success-400': '123 237 125', // #7bed7d
		'--color-success-500': '67 229 69', // #43e545
		'--color-success-600': '60 206 62', // #3cce3e
		'--color-success-700': '50 172 52', // #32ac34
		'--color-success-800': '40 137 41', // #288929
		'--color-success-900': '33 112 34', // #217022
		// warning | #ecee58
		'--color-warning-50': '252 252 230', // #fcfce6
		'--color-warning-100': '251 252 222', // #fbfcde
		'--color-warning-200': '250 251 213', // #fafbd5
		'--color-warning-300': '247 248 188', // #f7f8bc
		'--color-warning-400': '242 243 138', // #f2f38a
		'--color-warning-500': '236 238 88', // #ecee58
		'--color-warning-600': '212 214 79', // #d4d64f
		'--color-warning-700': '177 179 66', // #b1b342
		'--color-warning-800': '142 143 53', // #8e8f35
		'--color-warning-900': '116 117 43', // #74752b
		// error | #e03529
		'--color-error-50': '250 225 223', // #fae1df
		'--color-error-100': '249 215 212', // #f9d7d4
		'--color-error-200': '247 205 202', // #f7cdca
		'--color-error-300': '243 174 169', // #f3aea9
		'--color-error-400': '233 114 105', // #e97269
		'--color-error-500': '224 53 41', // #e03529
		'--color-error-600': '202 48 37', // #ca3025
		'--color-error-700': '168 40 31', // #a8281f
		'--color-error-800': '134 32 25', // #862019
		'--color-error-900': '110 26 20', // #6e1a14
		// surface | #ffffff
		'--color-surface-50': '255 255 255', // #ffffff
		'--color-surface-100': '255 255 255', // #ffffff
		'--color-surface-200': '255 255 255', // #ffffff
		'--color-surface-300': '255 255 255', // #ffffff
		'--color-surface-400': '255 255 255', // #ffffff
		'--color-surface-500': '255 255 255', // #ffffff
		'--color-surface-600': '230 230 230', // #e6e6e6
		'--color-surface-700': '191 191 191', // #bfbfbf
		'--color-surface-800': '153 153 153', // #999999
		'--color-surface-900': '125 125 125' // #7d7d7d
	}
};
