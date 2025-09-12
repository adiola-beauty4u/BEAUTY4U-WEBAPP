import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { StoreDto } from 'src/interfaces/store';
import { ConfigService } from './config.service';


@Injectable({
  providedIn: 'root'
})
export class StoreService {
  private storeUrl = `${this.config.apiBaseUrl}/v1/stores`;

  constructor(private http: HttpClient, private config: ConfigService) { }

  getStores(): Observable<StoreDto[]> {
    return this.http.get<StoreDto[]>(this.storeUrl);
  }
}
