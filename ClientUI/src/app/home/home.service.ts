import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { ProductParams } from '../_shared/_models/productParams';
import { Pagination } from '../_shared/_models/pagination';
import { Product } from '../_shared/_models/product';

@Injectable({
  providedIn: 'root'
})
export class HomeService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getNewArrivals(productParams: ProductParams) {
    let params = new HttpParams();

    params = params.append('search', productParams.search);
    params = params.append('pageSize', productParams.pageSize);
    params = params.append('pageIndex', productParams.pageIndex);

    return this.http.get<Pagination<Product[]>>(this.baseApiUrl + 'products/new-arrivals', { params });
  }
}
