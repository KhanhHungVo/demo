import tseslint from '@typescript-eslint/eslint-plugin'
import tsparser from '@typescript-eslint/parser'
import reactHooks from 'eslint-plugin-react-hooks'
import importPlugin from 'eslint-plugin-import'
import boundaries from 'eslint-plugin-boundaries'

export default [
  {
    files: ['src/**/*.{ts,tsx}'],
    languageOptions: { parser: tsparser },
    plugins: {
      '@typescript-eslint': tseslint,
      'react-hooks': reactHooks,
      import: importPlugin,
      boundaries,
    },
    settings: {
      'import/resolver': {
        typescript: { alwaysTryTypes: true },
      },
      'boundaries/elements': [
        { type: 'app',      pattern: 'src/app/*' },
        { type: 'pages',    pattern: 'src/pages/*' },
        { type: 'widgets',  pattern: 'src/widgets/*' },
        { type: 'features', pattern: 'src/features/*' },
        { type: 'entities', pattern: 'src/entities/*' },
        { type: 'shared',   pattern: 'src/shared/*' },
      ],
    },
    rules: {
      ...reactHooks.configs.recommended.rules,
      'boundaries/element-types': [
        'error',
        {
          default: 'disallow',
          rules: [
            { from: 'app',      allow: ['pages', 'widgets', 'features', 'entities', 'shared'] },
            { from: 'pages',    allow: ['widgets', 'features', 'entities', 'shared'] },
            { from: 'widgets',  allow: ['features', 'entities', 'shared'] },
            { from: 'features', allow: ['entities', 'shared'] },
            { from: 'entities', allow: ['shared'] },
            { from: 'shared',   allow: [] },
          ],
        },
      ],
      'import/no-internal-modules': ['error', { allow: [] }],
    },
  },
]
