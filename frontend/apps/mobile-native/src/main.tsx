// apps/mobile-native/src/main.tsx
import { NavigationContainer } from '@react-navigation/native';
import { App } from '@packages/app-core/App';
import { NativeRoutes } from '@packages/app-core/app/routes/native.routes';

export default function Main() {
  return <App Router={NavigationContainer} Routes={NativeRoutes} />;
}
