/** @type {import("eslint").Linter.Config} */
module.exports = {
  root: true,

  env: {
    browser: true,
    node: true,
    es2021: true,
  },

  parser: "@typescript-eslint/parser",

  parserOptions: {
    ecmaVersion: "latest",
    sourceType: "module",
    project: true,
  },

  plugins: [
    "@typescript-eslint",
    "react",
    "react-hooks",
    "boundaries",
  ],

  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:react/recommended",
    "plugin:react-hooks/recommended",
    "plugin:boundaries/recommended",
    "next/core-web-vitals",
    "prettier",
  ],

  settings: {
    react: {
      version: "detect",
    },

    /* -------------------------------
       BOUNDARIES CONFIGURATION
    -------------------------------- */
    "boundaries/elements": [
      /* ---------- APPS ---------- */
      {
        type: "app",
        pattern: "apps/*/src/**",
      },

      /* ---------- FEATURES ---------- */
      {
        type: "feature",
        pattern: "apps/*/src/features/*",
      },

      {
        type: "ui",
        pattern: "apps/*/src/features/*/ui/**",
      },

      {
        type: "domain",
        pattern: "apps/*/src/features/*/domain/**",
      },

      {
        type: "hook",
        pattern: "apps/*/src/features/*/hooks/**",
      },

      {
        type: "api",
        pattern: "apps/*/src/features/*/api/**",
      },

      /* ---------- APP ROUTER ---------- */
      {
        type: "page",
        pattern: "apps/*/src/app/**/page.tsx",
      },

      {
        type: "layout",
        pattern: "apps/*/src/app/**/layout.tsx",
      },

      {
        type: "provider",
        pattern: "apps/*/src/app/**/providers.tsx",
      },

      /* ---------- SHARED ---------- */
      {
        type: "shared-ui",
        pattern: "packages/ui/src/**",
      },

      {
        type: "app-core",
        pattern: "packages/app-core/src/**",
      },
    ],

    "boundaries/element-types": [
      {
        from: "ui",
        allow: ["domain", "hook", "api", "shared-ui"],
      },
      {
        from: "page",
        allow: ["ui", "domain", "hook"],
      },
      {
        from: "layout",
        allow: ["ui", "domain", "hook"],
      },
      {
        from: "provider",
        allow: ["app-core", "shared-ui"],
      },
      {
        from: "app-core",
        allow: ["domain"],
      },
    ],
  },

  rules: {
    /* -------------------------------
       GENERAL
    -------------------------------- */
    "no-console": "warn",

    /* -------------------------------
       TYPESCRIPT
    -------------------------------- */
    "@typescript-eslint/no-unused-vars": [
      "warn",
      { argsIgnorePattern: "^_" },
    ],

    /* -------------------------------
       REACT
    -------------------------------- */
    "react/react-in-jsx-scope": "off",

    /* -------------------------------
       BOUNDARIES
    -------------------------------- */
    "boundaries/no-unknown-files": "error",
    "boundaries/no-unknown": "error",
  },

  overrides: [
    /* ---------- ALLOW NEXT AUTO FILES ---------- */
    {
      files: [
        "**/*.d.ts",
        "**/next-env.d.ts",
        "**/.next/**",
        "**/dist/**",
        "**/build/**",
        "**/node_modules/**",
      ],
      rules: {
        "boundaries/no-unknown-files": "off",
        "boundaries/no-unknown": "off",
      },
    },
  ],
};
