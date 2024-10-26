import {
  Component,
  EventEmitter,
  Input,
  OnInit,
  Output,
  SimpleChanges,
} from '@angular/core';
import { Product } from 'src/app/_shared/_models/product';
import { ProductOption } from 'src/app/_shared/_models/productOption';
import { ProductSKU } from 'src/app/_shared/_models/productSKU';

@Component({
  selector: 'app-product-info',
  templateUrl: './product-info.component.html',
  styleUrls: ['./product-info.component.css'],
})
export class ProductInfoComponent implements OnInit {
  @Input() product: Product;
  @Output() selectOptions = new EventEmitter<ProductSKU>();
  selectedOptions: { [key: string]: any } = {};

  ngOnInit(): void {
    console.log(this.product);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['product']) {
      // Init values for options
      this.product?.productOptions?.forEach((option: ProductOption) => {
        if (option.productOptionValues.length > 0) {
          this.selectedOptions[option.id] = option.productOptionValues[0].id;

          console.log(this.selectedOptions)
          this.getSkuIdByProductOptions();
        }
      });
    }
  }

  onSelectOption(optionId: number, valueId: number): void {
    this.selectedOptions[optionId] = valueId;
    console.log(this.selectedOptions);
  }

  getSkuIdByProductOptions() {
    const targetValues = Object.values(this.selectedOptions);

    const result = this.product.productSkus.find(productSKU => {
      const skuValues = productSKU.productSKUValues.map(value => value.id);

      return targetValues.every(id => skuValues.includes(id));
    });

    if (result) {
      this.selectOptions.emit(result);
    } else {
      console.error('Không tìm thấy đối tượng nào thỏa mãn điều kiện.');
    }
  }
}
