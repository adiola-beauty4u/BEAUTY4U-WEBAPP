import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { StoreDto } from 'src/interfaces/store';


@Injectable({
  providedIn: 'root'
})
export class StoreService {
  private storeUrl = `${environment.apiBaseUrl}/v1/stores`;

  constructor(private http: HttpClient) { }

  getStores(): Observable<StoreDto[]> {
    return this.http.get<StoreDto[]>(this.storeUrl);
  }
}
