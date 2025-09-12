import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class BrandService {

  private brandUrl = `${this.config.apiBaseUrl}/v1/brand`;
  
  constructor(private http: HttpClient, private config: ConfigService) { }

  getBrands(): Observable<string[]> {
    return this.http.get<string[]>(this.brandUrl);
  }
}
