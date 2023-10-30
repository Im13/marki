import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Order, OrderToCreate } from '../shared/models/order';
import { Province } from '../shared/models/province';
import { District } from '../shared/models/district';
import { Ward } from '../shared/models/ward';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: OrderToCreate) {
    console.log(order);
    return this.http.post<Order>(this.baseUrl + 'orders', order); 
  }

  getProvinces() {
    return this.http.get<Province[]>(this.baseUrl + 'address/provinces');
  }

  getDistrictsByProvinceId(provinceId: number) {
    return this.http.get<District[]>(this.baseUrl + 'address/districts/' + provinceId);
  }

  getWardsByDistrictId(districtId: number) {
    return this.http.get<Ward[]>(this.baseUrl + 'address/wards/' + districtId);
  }
}
