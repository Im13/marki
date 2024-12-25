import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';
import { Product } from '../_shared/_models/product';

@Injectable({
  providedIn: 'root'
})
export class AllProductsService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Truy vấn sản phẩm dựa trên slug
  getProductBySlug(slug: string) {
    return this.http.get<Product>(this.baseApiUrl + 'products/slug/' + slug);
  }
}
