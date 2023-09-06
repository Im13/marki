import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Product } from '../shared/models/product';
import { Pagination } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ShopService {
  baseUrl = 'https://localhost:5001/'

  constructor(private http: HttpClient) { }

  getProducts() {
    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'products');
  }
}
