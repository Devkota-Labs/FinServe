/** @type {import('next').NextConfig} */
const nextConfig = {
  transpilePackages: [
    '@packages/app-core',
    '@packages/ui',
    '@packages/config'
  ],
  experimental: {
    externalDir: true
  }
};

module.exports = nextConfig;
