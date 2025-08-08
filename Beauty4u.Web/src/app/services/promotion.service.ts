import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PromotionService {

  private promotionsUrl = `${environment.apiBaseUrl}/v1/promotions`;
  
    constructor(private http: HttpClient) { }
  
    searchProductPromotionsBySku(sku: string): Observable<any>{
      return this.http.get(this.promotionsUrl + '/search-by-sku?sku=' + sku);
    }
}
