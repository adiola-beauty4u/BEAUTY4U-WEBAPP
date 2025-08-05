export interface StoreDto {
  code: string;
  type: string;
  storeAbbr: string;
  name: string;
  address: string;
  address2: string;
  city: string;
  state: string;
  zip: string;
  remoteIp: string;
  remote2ndIp: string;
  db: string;
  id: string;
  pwd: string;
  port: string;
  status: boolean;
  orders: number;
  writeDate: string; // or Date, depending on how you handle it in Angular
  writeUser: string;
  lastUpdate: string; // or Date
  lastUser: string;
  payrollCompanyCode: string;
  apiUrl: string;
}
