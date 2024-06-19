import { Injectable } from '@angular/core';
import { Product } from '../shared/models/products';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ProductParams } from '../shared/models/productParams';
import { Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})

export class ProductService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addProduct(product: Product) {
    return this.http.post(this.baseUrl + 'adminproduct/create', product);
  }

  editProduct(product: Product) {
    return this.http.put(this.baseUrl + 'admin/product', product);
  }

  getProducts(productParams: ProductParams) {
    let params = new HttpParams();

    params = params.append('pageSize', productParams.pageSize);
    params = params.append('pageIndex', productParams.pageIndex);

    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'adminproduct/products', { params });
  }
}
