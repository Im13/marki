import { Component } from '@angular/core';
import { ProductService } from 'src/app/product/product-service.service';
import { ProductParams } from 'src/app/shared/models/productParams';
import { Product } from 'src/app/shared/models/products';

@Component({
  selector: 'app-order-information',
  templateUrl: './order-information.component.html',
  styleUrls: ['./order-information.component.css']
})
export class OrderInformationComponent {
  options = ['Sản phẩm', 'Combo'];
  selectedProduct: Product = null;
  listSelectedProducts: Product[] = [];
  nzFilterOption = (): boolean => true;
  listFilteredProduct: Product[] = [];
  productParams = new ProductParams();

  constructor(private productService: ProductService) {}

  handleIndexChange(e: number): void {
    console.log(e);
  }

  search(value: string): void {
    this.productParams.search = value;
    this.productParams.pageSize = 100;

    this.productService.getProducts(this.productParams).subscribe({
      next: response => {
        this.listFilteredProduct = response.data;
      }
    });
  }

  selectProduct(data: any) {
    this.listSelectedProducts.push(data);
    console.log(this.listSelectedProducts);
  }

}
