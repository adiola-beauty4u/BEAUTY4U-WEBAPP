import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ItemGroup } from 'src/interfaces/item-group';

@Injectable({
  providedIn: 'root'
})
export class ItemGroupsService {

  private storeUrl = `${environment.apiBaseUrl}/v1/itemgroups`;

  constructor(private http: HttpClient) { }

  getItemGroups(): Observable<ItemGroup[]> {
    return this.http.get<ItemGroup[]>(this.storeUrl);
  }
}
