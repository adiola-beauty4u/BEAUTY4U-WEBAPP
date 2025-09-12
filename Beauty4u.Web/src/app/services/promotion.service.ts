import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

import { PromotionSearchParams } from 'src/interfaces/promotion-search-request';
import { PromotionRequest } from 'src/interfaces/promotion-request';
import { GetProductPromotionRequest } from 'src/interfaces/get-product-promotion-request';
import { PromotionTransferRequest } from 'src/interfaces/promotion-transfer-request';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class PromotionService {

  private promotionsUrl = `${this.config.apiBaseUrl}/v1/promotions`;

  constructor(private http: HttpClient, private config: ConfigService) { }

  searchProductPromotionsBySku(sku: string): Observable<any> {
    return this.http.get(this.promotionsUrl + '/search-by-sku?sku=' + sku);
  }
  searchProductPromotionsByPromoNo(getProductPromotionRequest: GetProductPromotionRequest): Observable<any> {
    return this.http.post(this.promotionsUrl + '/search-items-by-promono', getProductPromotionRequest);
  }

  searchPromotions(promoSearchParams: PromotionSearchParams): Observable<any> {
    return this.http.post(this.promotionsUrl + '/search-promo', promoSearchParams);
  }

  createPromotion(promotionCreateRequest: PromotionRequest): Observable<any> {
    return this.http.post(this.promotionsUrl + '/create-promo', promotionCreateRequest);
  }

  updatePromotion(promotionUpdateRequest: PromotionRequest): Observable<any> {
    return this.http.post(this.promotionsUrl + '/update-promo', promotionUpdateRequest);
  }

  getPromotion(promoNo: string): Observable<any> {
    return this.http.get(this.promotionsUrl + '/get-by-promono?promono=' + promoNo);
  }

  transferPromotion(promoTransferRequest: PromotionTransferRequest): Observable<any> {
    return this.http.post(this.promotionsUrl + '/transfer-promo-to-stores', promoTransferRequest);
  }
}
