import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})


export class DataCheckService {
  private apiUrl = `${environment.apiBaseUrl}/v1/DataCheck/read-csv`;

  constructor(private http: HttpClient) {}

  uploadCsvFile(file: File) {
    const formData = new FormData();
    formData.append('file', file); // match backend parameter name if different

    return this.http.post(this.apiUrl, formData);
  }
}