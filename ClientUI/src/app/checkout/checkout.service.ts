import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Province } from '../_shared/_models/address/province';
import { District } from '../_shared/_models/address/district';
import { Ward } from '../_shared/_models/address/ward';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: any) {
    console.log(order);
    return this.http.post(this.baseApiUrl + 'orders', order); 
  }

  getProvinces() {
    return this.http.get<Province[]>(this.baseApiUrl + 'address/provinces');
  }

  getDistricts(provinceId: number) {
    return this.http.get<District[]>(this.baseApiUrl + 'address/districts/' + provinceId);
  }

  getWards(districtId: number) {
    return this.http.get<Ward[]>(this.baseApiUrl + 'address/wards/' + districtId);
  }
}
