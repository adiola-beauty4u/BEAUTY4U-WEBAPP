export interface ItemGroup {
  code: string;
  name: string;
  description: string;
  status: boolean;
  oldCode: string;
  orders: number;
  writeDate: string;      // or Date if parsed
  writeUser: string;
  lastUpdate: string;     // or Date if parsed
  lastUser: string;
  level1Code: string;
  level2Code: string;
  childItemGroups: ItemGroup[] | null;
}
