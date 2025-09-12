import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { SysCode } from 'src/interfaces/sys-codes';

@Injectable({
  providedIn: 'root'
})
export class SystemService {
  private systemUrl = `${environment.apiBaseUrl}/v1/system`;

  constructor(private http: HttpClient) { }

  getSysCodeByClass(sysCodeClass: string): Observable<SysCode[]> {
    return this.http.get<SysCode[]>(`${this.systemUrl}/syscode-by-class?sysCodeClass=${sysCodeClass}`);
  }

}
