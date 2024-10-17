import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class AllProductsService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Truy vấn sản phẩm dựa trên slug
  getProductBySlug(slug: string) {
    return this.http.get(this.baseApiUrl + 'products/slug/' + slug);
  }
}
