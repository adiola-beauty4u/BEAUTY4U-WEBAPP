export interface VendorDto {
    vendorId: number;
    name: string;
    code: string;
    type: string;
    brandId?: number;
    address?: string;
    address2?: string;
    city?: string;
    state?: string;
    zip?: string;
    phone?: string;
    fax?: string;
    taxein?: string;
    contactPerson?: string;
    contactPhone?: string;
    contactEmail?: string;
    description?: string;
    status: boolean;
    dateCreated: Date; // or Date if you're handling it as a Date object
    createdBy: number;
    dateModified?: Date; // or Date
    modifiedBy?: number;
    brandName?: string;
  }