export interface BulkProduct {
  vendorId: number;
  vendorCode: string;
  vendorName: string;
  userCode: string;
  productFile: File; // Corresponds to IFormFile in C#F
}
