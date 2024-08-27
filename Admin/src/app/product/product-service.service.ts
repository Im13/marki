import { Injectable } from '@angular/core';
import { Product } from '../shared/_models/products';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ProductParams } from '../shared/_models/productParams';
import { Pagination } from '../shared/_models/pagination';
import { ProductType } from '../shared/_models/productTypes';
import { ProductSKUDetails } from '../shared/_models/productSKUDetails';

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
    return this.http.put(this.baseUrl + 'adminproduct/product', product);
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
