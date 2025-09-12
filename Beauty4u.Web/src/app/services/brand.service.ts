import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BrandService {
  private brandUrl = `${environment.apiBaseUrl}/v1/brand`;
  constructor(private http: HttpClient) { }

  getBrands(): Observable<string[]> {
    return this.http.get<string[]>(this.brandUrl);
  }
}
