import type { CapacitorConfig } from '@capacitor/cli';

const config: CapacitorConfig = {
  appId: 'com.devkotalabs.finserve',
  appName: 'FinServe',
  webDir: 'out',
  server: {
    url: 'https://dzrds0cnvm4xr.cloudfront.net',
    cleartext: false
  }
};

export default config;
