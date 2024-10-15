import { Injectable } from '@angular/core';
import { ProductParams } from '../_shared/_models/productParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination } from '../_shared/_models/pagination';
import { Product } from '../_shared/_models/product';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CollectionsService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getProducts(productParams: ProductParams) {
    let params = new HttpParams();

    params = params.append('search', productParams.search);
    params = params.append('pageSize', productParams.pageSize);
    params = params.append('pageIndex', productParams.pageIndex);
    params = params.append('typeId', productParams.typeId);

    return this.http.get<Pagination<Product[]>>(this.baseApiUrl + 'products', { params });
  }
}
