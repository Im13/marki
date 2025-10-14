import { Injectable } from '@angular/core';
import { Product } from '../_shared/_models/products';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ProductParams } from '../_shared/_models/productParams';
import { Pagination } from '../_shared/_models/pagination';
import { ProductType } from '../_shared/_models/productTypes';
import { ProductSKUDetails } from '../_shared/_models/productSKUDetails';

@Injectable({
  providedIn: 'root'
})

export class ProductService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addProduct(product: Product) {
    return this.http.post(this.baseUrl + 'adminproduct/create', product);
  }

  editProduct(productId: number, product: any) {
    return this.http.put(`${this.baseUrl}adminproduct/product/${productId}`, product);
  }

  getProducts(productParams: ProductParams) {
    let params = new HttpParams();

    params = params.append('search', productParams.search);
    params = params.append('pageSize', productParams.pageSize);
    params = params.append('pageIndex', productParams.pageIndex);

    return this.http.get<Pagination<Product[]>>(this.baseUrl + 'adminproduct/products', { params });
  }

  deleteProducts(products: Product[]) {
    return this.http.post(this.baseUrl + 'adminproduct/delete-products', products);
  }

  productImageUpload(file: FormData) {
    console.log(typeof(file));
    return this.http.post(this.baseUrl + 'adminproduct/image-upload', file);
  }

  getAllProductTypes() {
    return this.http.get<ProductType[]>(this.baseUrl + 'products/types')
  }

  getProductSKUDetails(productParams: ProductParams) {
    let params = new HttpParams();

    params = params.append('search', productParams.search);

    return this.http.get<ProductSKUDetails[]>(this.baseUrl + 'adminproduct/skus', { params })
  }
}
