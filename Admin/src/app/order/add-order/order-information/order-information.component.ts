import { Component } from '@angular/core';
import { ProductService } from 'src/app/product/product-service.service';
import { ProductParams } from 'src/app/shared/models/productParams';
import { Product } from 'src/app/shared/models/products';
import { ProductSKUDetails } from 'src/app/shared/models/productSKUDetails';

@Component({
  selector: 'app-order-information',
  templateUrl: './order-information.component.html',
  styleUrls: ['./order-information.component.css']
})
export class OrderInformationComponent {
  options = ['Sản phẩm', 'Combo'];
  selectedProduct: Product = null;
  listSelectedSkus: ProductSKUDetails[] = [];
  nzFilterOption = (): boolean => true;
  listFilterSkus: ProductSKUDetails[] = [];
  productParams = new ProductParams();
  freeShippingChecked = false;

  //Checkout variables
  shippingFee = 0;
  orderDiscount = 0;
  orderNote = '';
  orderCreatedDate = null;

  constructor(private productService: ProductService) {}

  handleIndexChange(e: number): void {
    console.log(e);
  }

  search(value: string): void {
    this.productParams.search = value;

    this.productService.getProductSKUDetails(this.productParams).subscribe({
      next: skus => {
        this.listFilterSkus = skus;
      }
    });
  }

  selectProduct(data: any) {
    this.listSelectedSkus.push(data);
  }

  onDateChange(result: Date) {
    console.log('onChange: ', result);
  }

}
