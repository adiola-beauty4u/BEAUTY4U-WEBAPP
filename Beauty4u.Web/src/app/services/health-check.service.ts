import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { HealthCheckItem } from 'src/interfaces/health-check-result';

@Injectable({
  providedIn: 'root'
})
export class HealthCheckService {
  private healthUrl = `${environment.apiBaseUrl}/v1/health`;

  constructor(private http: HttpClient) { }

  getServersHealth(): Observable<HealthCheckItem[]> {
    return this.http.get<HealthCheckItem[]>(this.healthUrl+ '/health-check');
  }

}
