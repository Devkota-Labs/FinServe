module.exports = {
  root: true,

  overrides: [
    {
      files: ["**/*.ts", "**/*.tsx"],

      parser: "@typescript-eslint/parser",

      plugins: ["@typescript-eslint", "boundaries"],

      extends: [
        "eslint:recommended",
        "plugin:@typescript-eslint/recommended"
      ],

      settings: {
        "boundaries/include": ["**/*"],

        "boundaries/elements": [
          // ======================
          // UI PACKAGE
          // ======================
          {
            type: "ui-primitives",
            pattern: "**/packages/ui/src/primitives/**/*"
          },
          {
            type: "ui-components",
            pattern: "**/packages/ui/src/components/**/*"
          },
          {
            type: "ui-layouts",
            pattern: "**/packages/ui/src/layouts/**/*"
          },
          {
            type: "ui-toast",
            pattern: "**/packages/ui/src/toast/**/*"
          },

          // ======================
          // APPS
          // ======================
          {
            type: "app",
            pattern: "**/apps/*/src/**/*"
          },

          // ======================
          // 🔑 FALLBACK (REQUIRED)
          // ======================
          {
            type: "unknown",
            pattern: "**/*"
          }
        ]
      },

      rules: {
        // ❗ TURN THIS OFF (important)
        "boundaries/no-unknown-files": "off",

        // 🧱 KEEP ARCHITECTURE ENFORCEMENT
        "boundaries/element-types": [
          "error",
          {
            default: "disallow",
            rules: [
              { from: "ui-primitives", allow: [] },
              { from: "ui-components", allow: ["ui-primitives"] },
              { from: "ui-layouts", allow: ["ui-components", "ui-primitives"] },
              { from: "ui-toast", allow: ["ui-primitives"] },

              {
                from: "app",
                allow: [
                  "ui-primitives",
                  "ui-components",
                  "ui-layouts",
                  "ui-toast"
                ]
              },

              // unknown can import nothing
              { from: "unknown", allow: [] }
            ]
          }
        ]
      }
    }
  ]
};
