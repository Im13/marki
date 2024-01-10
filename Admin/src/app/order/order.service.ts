import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  uploadShopeeOrdersFile(excelFile: FormData) {
    return this.http.post(this.baseApiUrl + '/shopee/create-orders', excelFile);
  }
}
