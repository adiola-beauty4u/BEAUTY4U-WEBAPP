import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { SysCode } from 'src/interfaces/sys-codes';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class SystemService {
  private systemUrl = `${this.config.apiBaseUrl}/v1/system`;

  constructor(private http: HttpClient, private config: ConfigService) { }

  getSysCodeByClass(sysCodeClass: string): Observable<SysCode[]> {
    return this.http.get<SysCode[]>(`${this.systemUrl}/syscode-by-class?sysCodeClass=${sysCodeClass}`);
  }

}
