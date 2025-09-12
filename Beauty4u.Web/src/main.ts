import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';
import { APP_INITIALIZER, Provider } from '@angular/core';

function loadAppConfig(): Promise<any> {
  return fetch('/assets/config/config.json')
    .then(res => res.json())
    .then(config => {
      (window as any).appConfig = config;
    });
}

const appConfigProvider: Provider = {
  provide: APP_INITIALIZER,
  useFactory: () => loadAppConfig,
  multi: true
};

bootstrapApplication(AppComponent, {
  ...appConfig,
  providers: [
    ...(appConfig.providers ?? []), // keep static providers
    appConfigProvider               // add runtime loader
  ]
});
