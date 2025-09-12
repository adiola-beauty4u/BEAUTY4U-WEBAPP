import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { VendorDto } from 'src/interfaces/vendor';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class VendorService {

  private vendorUrl = `${this.config.apiBaseUrl}/v1/vendors`;

  constructor(private http: HttpClient, private config: ConfigService) {}

  getVendors(): Observable<VendorDto[]> {
    return this.http.get<VendorDto[]>(this.vendorUrl);
  }
}
