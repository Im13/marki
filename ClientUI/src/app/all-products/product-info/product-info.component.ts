import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { Product } from 'src/app/_shared/_models/product';
import { ProductOption } from 'src/app/_shared/_models/productOption';

@Component({
  selector: 'app-product-info',
  templateUrl: './product-info.component.html',
  styleUrls: ['./product-info.component.css'],
})
export class ProductInfoComponent implements OnInit {
  @Input() product: Product;
  selectedOptions: { [key: string]: any } = {};

  ngOnInit(): void {
    console.log(this.product);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['product']) {
      // Khởi tạo các giá trị mặc định cho từng option
      this.product?.productOptions?.forEach((option: ProductOption) => {
        if (option.productOptionValues.length > 0) {
          this.selectedOptions[option.id] =
            option.productOptionValues[0].id; // Chọn giá trị mặc định là phần tử đầu tiên

          console.log(this.selectedOptions);
        }
      });
    }
  }

  // Hàm chọn giá trị của từng option
  onSelectOption(optionId: number, valueId: number): void {
    this.selectedOptions[optionId] = valueId;
    console.log(this.selectedOptions);
  }
}
