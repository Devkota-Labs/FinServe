import { tailwindBaseConfig } from "@packages/config/tailwind"

export default {
  ...tailwindBaseConfig,
  content: [
    "./src/**/*.{ts,tsx}",
    "../../packages/ui/src/**/*.{ts,tsx}",
  ],
}
