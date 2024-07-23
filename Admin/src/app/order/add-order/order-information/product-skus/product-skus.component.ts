import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProductService } from 'src/app/product/product-service.service';
import { ProductParams } from 'src/app/shared/models/productParams';
import { Product } from 'src/app/shared/models/products';
import { ProductSKUDetails } from 'src/app/shared/models/productSKUDetails';

@Component({
  selector: 'app-product-skus',
  templateUrl: './product-skus.component.html',
  styleUrls: ['./product-skus.component.css']
})
export class ProductSkusComponent {
  @Input() listSkus!: ProductSKUDetails[];
  @Output() selectProductSkus = new EventEmitter<ProductSKUDetails[]>();

  selectedProduct: Product = null;
  nzFilterOption = (): boolean => true;
  productParams = new ProductParams();
  listFilterSkus: ProductSKUDetails[] = [];
  options = ['Sản phẩm', 'Combo'];

  constructor(private productService: ProductService) {}

  selectProduct(data: any) {
    this.listSkus.push(data);
    this.selectProductSkus.emit(this.listSkus);
  }

  search(value: string): void {
    this.productParams.search = value;

    this.productService.getProductSKUDetails(this.productParams).subscribe({
      next: skus => {
        this.listFilterSkus = skus;
      }
    });
  }

  //Improve later
  handleIndexChange(e: number): void {
    console.log(e);
  }
}
