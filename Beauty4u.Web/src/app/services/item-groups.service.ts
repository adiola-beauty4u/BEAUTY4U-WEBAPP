import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { ItemGroup } from 'src/interfaces/item-group';
import { ConfigService } from './config.service';

@Injectable({
  providedIn: 'root'
})
export class ItemGroupsService {

  private storeUrl = `${this.config.apiBaseUrl}/v1/itemgroups`;

  constructor(private http: HttpClient, private config: ConfigService) { }

  getItemGroups(): Observable<ItemGroup[]> {
    return this.http.get<ItemGroup[]>(this.storeUrl);
  }
}
