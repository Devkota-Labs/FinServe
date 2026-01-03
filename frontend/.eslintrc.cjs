module.exports = {
  root: true,

  parser: "@typescript-eslint/parser",

  plugins: [
    "@typescript-eslint",
    "react",
    "react-hooks",
    "boundaries",
    "unused-imports"
  ],

  extends: [
    "eslint:recommended",
    "plugin:@typescript-eslint/recommended",
    "plugin:react/recommended",
    "plugin:react-hooks/recommended",
    "prettier"
  ],

  settings: {
    react: {
      version: "detect"
    },

    /* We ONLY classify files */
    "boundaries/elements": [
      { type: "app", pattern: "src/app/**" },
      { type: "feature", pattern: "src/features/**" },
      { type: "feature-ui", pattern: "src/features/**/ui/**" },
      { type: "feature-hook", pattern: "src/features/**/hooks/**" },
      { type: "layout", pattern: "src/layouts/**" },
      { type: "context", pattern: "src/context/**" },
      { type: "lib", pattern: "src/lib/**" }
    ]
  },

  rules: {
    /* React 17+ / Next.js */
    "react/react-in-jsx-scope": "off",
    "react/jsx-uses-react": "off",

    /* ✅ KEEP THIS */
    "boundaries/no-unknown-files": "error",

    /* ❌ DISABLE THIS PERMANENTLY */
    "boundaries/no-unknown": "off",

    /* TypeScript */
    "@typescript-eslint/no-explicit-any": "warn",

    /* Imports */
    "unused-imports/no-unused-imports": "warn"
  },

  overrides: [
    {
      files: [
        "**/*.d.ts",
        "**/next-env.d.ts",
        "**/.next/**",
        "**/dist/**",
        "**/build/**",
        "**/node_modules/**"
      ],
      rules: {
        "boundaries/no-unknown-files": "off"
      }
    }
  ]
};
