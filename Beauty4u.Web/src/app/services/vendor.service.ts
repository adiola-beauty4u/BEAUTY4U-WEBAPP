import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { VendorDto } from 'src/interfaces/vendor';

@Injectable({
  providedIn: 'root'
})
export class VendorService {

  private vendorUrl = `${environment.apiBaseUrl}/v1/vendors`;

  constructor(private http: HttpClient) {}

  getVendors(): Observable<VendorDto[]> {
    return this.http.get<VendorDto[]>(this.vendorUrl);
  }
}
