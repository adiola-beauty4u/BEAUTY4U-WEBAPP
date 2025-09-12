import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { BulkProduct } from 'src/interfaces/bulk-product';
import { DateSearchRequest } from 'src/interfaces/date-search-request';
import { TransferProductSearchResult } from 'src/interfaces/transfer-product-search';
import { ProductTransferRequest } from 'src/interfaces/product-transfer-request';
import { ProductSearchRequest } from 'src/interfaces/product-search-request';
import { PromotionProductSearchParams } from 'src/interfaces/product-for-promotion-search-request';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private productUrl = `${this.config.apiBaseUrl}/v1/products`;

  constructor(private http: HttpClient, private config: ConfigService) { }

  uploadBulkProductRegistration(product: BulkProduct): Observable<any> {
    const formData = new FormData();

    formData.append('VendorId', product.vendorId.toString());
    formData.append('VendorCode', product.vendorCode);
    formData.append('VendorName', product.vendorName);
    formData.append('UserCode', product.userCode);
    formData.append('ProductFile', product.productFile); // File object

    return this.http.post(this.productUrl + '/bulk-register', formData);
  }

  uploadBulkProductUpdate(productFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('ProductFile', productFile); // File object

    return this.http.post(this.productUrl + '/bulk-update', formData);
  }

  bulkProductRegisterPreview(product: BulkProduct): Observable<any> {
    const formData = new FormData();

    formData.append('VendorId', product.vendorId.toString());
    formData.append('VendorCode', product.vendorCode);
    formData.append('VendorName', product.vendorName);
    formData.append('UserCode', product.userCode);
    formData.append('ProductFile', product.productFile); // File object

    return this.http.post(this.productUrl + '/bulk-register-preview', formData);
  }

  transferSearch(dateRequest: DateSearchRequest): Observable<any> {
    return this.http.post(`${this.productUrl}/transfer-search`, dateRequest);
  }

  bulkProductUpdatePreview(productFile: File): Observable<any> {
    const formData = new FormData();
    formData.append('productFile', productFile); // File object
    return this.http.post(this.productUrl + '/bulk-update-preview', formData);
  }

  transferPreview(productTransferRequest: ProductTransferRequest): Observable<any> {
    return this.http.post(this.productUrl + '/transfer-preview', productTransferRequest);
  }
  transferToStores(productTransferRequest: ProductTransferRequest): Observable<any> {

    return this.http.post(this.productUrl + '/store-transfers', productTransferRequest);
  }
  searchProducts(productSearchRequest: ProductSearchRequest): Observable<any>{
    return this.http.post(this.productUrl + '/product-search', productSearchRequest);
  }

  searchProductsForPromotion(productSearchRequest: PromotionProductSearchParams): Observable<any>{
    return this.http.post(this.productUrl + '/search-for-promotion', productSearchRequest);
  }
}

