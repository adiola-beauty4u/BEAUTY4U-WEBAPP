export interface HealthCheckItem {
  name: string;
  value: boolean;
}

export interface HealthTableRow {
  storeCode: string;
  storeName: string;
  dbStatus?: boolean;
  apiStatus?: boolean;
}