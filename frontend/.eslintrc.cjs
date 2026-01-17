module.exports = {
  root: true,

  overrides: [
  {
    files: ['**/*.ts', '**/*.tsx', '**/*.css'],

    parser: '@typescript-eslint/parser',

    plugins: ['@typescript-eslint', 'boundaries'],

    extends: ['eslint:recommended', 'plugin:@typescript-eslint/recommended'],

    settings: {
      'boundaries/include': ['**/*'],

      'boundaries/elements': [
        // ======================
        // UI STYLES
        // ======================
        {
          type: 'ui-styles',
          pattern: '**/packages/ui/src/**/*.css',
        },

        // ======================
        // UI PACKAGE
        // ======================
        {
          type: 'ui-primitives',
          pattern: '**/packages/ui/src/primitives/**/*',
        },
        {
          type: 'ui-components',
          pattern: '**/packages/ui/src/components/**/*',
        },
        {
          type: 'ui-layouts',
          pattern: '**/packages/ui/src/layouts/**/*',
        },
        {
          type: 'ui-toast',
          pattern: '**/packages/ui/src/toast/**/*',
        },
        {
          type: 'ui',
          pattern: '**/packages/ui/src/**/*',
        },

        // ======================
        // APPS
        // ======================
        {
          type: 'app',
          pattern: '**/apps/*/src/**/*',
        },

        // ======================
        // FALLBACK
        // ======================
        {
          type: 'unknown',
          pattern: '**/*',
        },
      ],
    },

    rules: {
      'boundaries/no-unknown-files': 'off',

      'boundaries/element-types': [
        'error',
        {
          default: 'disallow',
          rules: [
            { from: 'ui-primitives', allow: ['ui-styles'] },
            { from: 'ui-components', allow: ['ui-primitives', 'ui-styles'] },
            { from: 'ui-layouts', allow: ['ui-components', 'ui-primitives', 'ui-styles'] },
            { from: 'ui-toast', allow: ['ui-primitives', 'ui-styles'] },
            { from: 'ui', allow: ['ui', 'ui-styles'] },
            {
              from: 'app',
              allow: [
                'ui-primitives',
                'ui-components',
                'ui-layouts',
                'ui-toast',
                'ui',
                'ui-styles',
              ],
            },
            { from: 'unknown', allow: [] },
          ],
        },
      ],
    },
  },
],
};
