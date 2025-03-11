import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ProductService } from 'src/app/product/product-service.service';
import { ProductParams } from 'src/app/shared/_models/productParams';
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
  productParams = new ProductParams();

  constructor(private productService: ProductService) {}

  //Improve later
  handleIndexChange(e: number): void {
    console.log(e);
  }

  search(value: string): void {
    this.productParams.search = value;

    this.productService.getProductSKUDetails(this.productParams).subscribe({
      next: skus => {
        console.log(skus);
        this.listFilterSkus = skus;
      }
    });
  }

  selectProduct(data: ProductSKUDetails) {
    //When select SKU, it means we will create new OrderItem, so id = -1
    this.listItems.push({
      id: null,
      productName: data.productName,
      price: data.price,
      quantity: 1,
      pictureUrl: data.imageUrl,
      sku: data.sku,
      optionValueCombination: data.productSKUValues.map(option => `${option.optionName}: ${option.optionValue}`).join("; "),
      productId: data.id,
      itemOrdered: { 
        productItemId: 0,
        productName: data.productName,
        pictureUrl: data.imageUrl
      }
    });

    console.log(this.listItems);
  }

  onRemoveSKU(item: OrderItem) {
    var index = this.listItems.findIndex(s => s.id === item.id);

    if(index > -1) {
      this.listItems.splice(index, 1);
    }

    this.selectListItems.emit(this.listItems);
  }
}
