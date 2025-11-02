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
  @Output() selectQuantity = new EventEmitter<number>();
  @Output() addToCart = new EventEmitter<void>();
  selectedOptions: { [key: string]: any } = {};
  quantity: number = 1;
  productDescription: string = '';
  errorMessage: string = '';
  showError: boolean = false;

  ngOnInit(): void { }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['product']) {
      // Reset selectedOptions
      this.selectedOptions = {};
      this.showError = false;
      
      // Init values for options - chọn giá trị đầu tiên cho mỗi option
      this.product?.productOptions?.forEach((option: ProductOption) => {
        if (option.productOptionValues && option.productOptionValues.length > 0) {
          this.selectedOptions[option.id] = option.productOptionValues[0].id;
        }
      });
      
      // Chỉ gọi getSkuIdByProductOptions một lần sau khi đã set tất cả options
      if (Object.keys(this.selectedOptions).length > 0) {
        this.getSkuIdByProductOptions();
      }
    }
  }

  onSelectOption(optionId: number, valueId: number): void {
    this.selectedOptions[optionId] = valueId;
    this.showError = false; // Ẩn lỗi khi user chọn option mới
    this.getSkuIdByProductOptions();
  }

  onAddToCart() {
    // Đảm bảo SKU và quantity được emit trước khi trigger addToCart
    const result = this.getSkuIdByProductOptions();
    
    if (result) {
      this.showError = false;
      setTimeout(() => {
        this.addToCart.emit();
      }, 0);
    } else {
      this.showError = true;
      this.errorMessage = 'Vui lòng chọn đầy đủ thuộc tính sản phẩm hoặc tổ hợp này hiện không có sẵn.';
      console.error('Không tìm thấy đối tượng nào thỏa mãn điều kiện. Không thể thêm vào giỏ hàng.');
    }
  }

  getSkuIdByProductOptions(): ProductSKU | null {
    // Kiểm tra xem product và productSkus có tồn tại không
    if (!this.product || !this.product.productSkus || this.product.productSkus.length === 0) {
      return null;
    }

    const targetValues = Object.values(this.selectedOptions) as number[];
    
    const totalOptions = this.product.productOptions?.length || 0;
    if (targetValues.length !== totalOptions || targetValues.length === 0) {
      return null;
    }

    const result = this.product.productSkus.find((productSKU, index) => {

      if (!productSKU.productSKUValues || productSKU.productSKUValues.length === 0) {
        return false;
      }

      const skuValues = productSKU.productSKUValues
        .map((value, idx) => {
          return value?.productOptionValue?.id;
        })
        .filter(id => {
          const isValid = id !== undefined && id !== null;
          if (!isValid) {
            console.log('  ⚠️ Filtered out undefined/null id');
          }
          return isValid;
        });

      if (skuValues.length !== targetValues.length) {
        return false;
      }

      const allValuesMatch = targetValues.every((id) => {
        const match = skuValues.includes(id);
        return match;
      });
      
      const hasAllValues = skuValues.length === targetValues.length && allValuesMatch;

      return hasAllValues;
    });

    if (result) {
      this.selectOptions.emit(result);
      this.selectQuantity.emit(this.quantity);
      return result;
    } else {
      return null;
    }
  }

  decreaseQuantity() {
    if (this.quantity > 1) {
      this.quantity--;
      this.selectQuantity.emit(this.quantity);
    }
  }

  increaseQuantity() {
    this.quantity++;
    this.selectQuantity.emit(this.quantity);
  }
}
