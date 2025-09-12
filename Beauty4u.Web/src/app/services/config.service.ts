import { Injectable } from '@angular/core';

interface RuntimeConfig {
  apiBaseUrl: string;
}

@Injectable({ providedIn: 'root' })
export class ConfigService {
  private config: RuntimeConfig = (window as any).appConfig;

  get apiBaseUrl(): string {
    return this.config.apiBaseUrl;
  }
}
