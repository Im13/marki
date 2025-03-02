import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Product } from 'src/app/shared/_models/products';
import { ProductSKUDetails } from 'src/app/shared/_models/productSKUDetails';
import { OrderItem } from 'src/app/shared/_models/website-order';

@Component({
  selector: 'app-list-product-sku',
  templateUrl: './product-skus.component.html',
  styleUrls: ['./product-skus.component.css']
})
export class ListProductSkuComponent {
  @Input() listItems: OrderItem[];
  @Output() selectListItems = new EventEmitter<OrderItem[]>();

  options = ['Sản phẩm', 'Combo'];
  selectedProduct: Product = null;
  nzFilterOption = (): boolean => true;
  listFilterSkus: ProductSKUDetails[] = [];
  // listItems: OrderItem[] = [];
  // listItems: OrderItem[];

  //Improve later
  handleIndexChange(e: number): void {
    console.log(e);
  }

  search(value: string): void {
    // this.productParams.search = value;

    // this.productService.getProductSKUDetails(this.productParams).subscribe({
    //   next: skus => {
    //     this.listFilterSkus = skus;
    //   }
    // });
  }

  selectProduct(data: any) {
    // this.listSkus.push(data);
    // this.selectProductSkus.emit(this.listSkus);
    // this.selectNode.writeValue(undefined);
  }

  onRemoveSKU(item: OrderItem) {
    var index = this.listItems.findIndex(s => s.id === item.id);

    if(index > -1) {
      this.listItems.splice(index, 1);
    }

    this.selectListItems.emit(this.listItems);
  }
}
