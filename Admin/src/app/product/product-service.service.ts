import { Injectable } from '@angular/core';
import { Product } from '../shared/models/products';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})

export class ProductService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addProduct(product: Product) {
    return this.http.post(this.baseUrl + 'products', product);
  }
}
