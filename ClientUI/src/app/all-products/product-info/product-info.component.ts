import { Component, Input, OnInit } from '@angular/core';
import { Product } from 'src/app/_shared/_models/product';

@Component({
  selector: 'app-product-info',
  templateUrl: './product-info.component.html',
  styleUrls: ['./product-info.component.css']
})
export class ProductInfoComponent implements OnInit {
  @Input() product: Product;
  selectedOptions: { [key: string]: any } = {};

  ngOnInit(): void {
    // Khởi tạo các tùy chọn người dùng chọn mặc định
    this.product?.productOptions?.forEach((option: any) => {
      this.selectedOptions[option.optionName] = null;
    });
  }

  // Hàm chọn giá trị của từng option
  onSelectOption(optionId: number, valueId: number): void {
    this.selectedOptions[optionId] = valueId;
    console.log(this.selectedOptions);
  }

}
