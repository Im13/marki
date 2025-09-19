import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { NzSelectComponent } from 'ng-zorro-antd/select';
import { ProductService } from 'src/app/product/product-service.service';
import { ProductParams } from 'src/app/_shared/_models/productParams';
import { Product } from 'src/app/_shared/_models/products';
import { ProductSKUDetails } from 'src/app/_shared/_models/productSKUDetails';
import { OrderItem } from 'src/app/_shared/_models/website-order';

@Component({
  selector: 'app-product-items',
  templateUrl: './product-items.component.html',
  styleUrls: ['./product-items.component.css'],
})
export class ProductItemsComponent {
  @Input() listItems!: OrderItem[];
  @Output() selectProductSkus = new EventEmitter<OrderItem[]>();
  @ViewChild(NzSelectComponent, { static: true }) selectNode: NzSelectComponent;

  selectedProduct: Product = null;
  nzFilterOption = (): boolean => true;
  productParams = new ProductParams();
  listFilterSkus: ProductSKUDetails[] = [];
  options = ['Sản phẩm', 'Combo'];

  constructor(private productService: ProductService) {}

  selectProduct(data: any) {
    this.listItems.push(data);
    this.selectProductSkus.emit(this.listItems);
    this.selectNode.writeValue(undefined);
  }

  search(value: string): void {
    this.productParams.search = value;

    this.productService.getProductSKUDetails(this.productParams).subscribe({
      next: (skus) => {
        this.listFilterSkus = skus;
      },
    });
  }

  //Improve later
  handleIndexChange(e: number): void {
    console.log(e);
  }

  onRemoveSKU(item: OrderItem) {
    // var index = this.listItems.findIndex((s) => s.id === sku.id);
    var index = 1;

    if (index > -1) {
      this.listItems.splice(index, 1);
    }

    this.selectProductSkus.emit(this.listItems);
  }
}
