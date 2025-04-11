import { Component, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { NzSelectComponent } from 'ng-zorro-antd/select';
import { ProductService } from 'src/app/product/product-service.service';
import { ProductParams } from 'src/app/shared/_models/productParams';
import { Product } from 'src/app/shared/_models/products';
import { ProductSKUDetails } from 'src/app/shared/_models/productSKUDetails';

@Component({
  selector: 'app-product-skus',
  templateUrl: './product-skus.component.html',
  styleUrls: ['./product-skus.component.css']
})
export class ProductSkusComponent {
  @Input() listSkus!: ProductSKUDetails[];
  @Output() selectProductSkus = new EventEmitter<ProductSKUDetails[]>();
  @ViewChild(NzSelectComponent, { static: true }) selectNode: NzSelectComponent;

  selectedProduct: Product = null;
  nzFilterOption = (): boolean => true;
  productParams = new ProductParams();
  listFilterSkus: ProductSKUDetails[] = [];
  options = ['Sản phẩm', 'Combo'];
  searchText = '';

  constructor(private productService: ProductService) {}

  selectProduct(data: any) {
    this.listSkus.push(data);
    this.selectProductSkus.emit(this.listSkus);
    this.selectNode.writeValue(undefined);
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

  onRemoveSKU(sku: ProductSKUDetails) {
    var index = this.listSkus.findIndex(s => s.id === sku.id);

    if(index > -1) {
      this.listSkus.splice(index, 1);
    }

    this.selectProductSkus.emit(this.listSkus);
  }
}
