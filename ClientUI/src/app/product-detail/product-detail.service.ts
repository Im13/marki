import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Product } from '../_shared/_models/product';

@Injectable({
  providedIn: 'root'
})
export class ProductDetailService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // Truy vấn sản phẩm dựa trên slug
  getProductBySlug(slug: string) {
    // Thêm query params để yêu cầu backend include đầy đủ relations
    const params = new HttpParams()
      .set('includeSkuValues', 'true')
      .set('includeOptions', 'true');
    
    return this.http.get<Product>(
      this.baseApiUrl + 'products/slug/' + slug,
      { params }
    );
  }
}
