import { Component, Input, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { Product } from 'src/app/shared/models/products';
import { ProductService } from '../../product-service.service';
import { ToastrService } from 'ngx-toastr';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ProductOptions } from 'src/app/shared/models/productOptions';
import { ProductSKUs } from 'src/app/shared/models/productSKUs';
import { ProductOption } from 'src/app/shared/models/productOption';
import { ConvertVieService } from 'src/app/core/services/convert-vie.service';

@Component({
  selector: 'app-add-product-modal',
  templateUrl: './add-product-modal.component.html',
  styleUrls: ['./add-product-modal.component.css'],
})
export class AddProductModalComponent implements OnInit {
  @Input() product?: Product = inject(NZ_MODAL_DATA);

  addForm: FormGroup;
  isEdit?: boolean;
  productOptions: ProductOptions[] = [];
  productOptionId: number = 0;
  currentOptionValueText = '';
  productSKUs: ProductSKUs[] = [];
  editId: number | null = null;
  variantValues: string[][] = [];

  constructor(
    private modal: NzModalRef,
    private productService: ProductService,
    private toastrService: ToastrService,
    private convertVieService: ConvertVieService
  ) {}

  ngOnInit(): void {
    if (this.product == null) {
      this.product = {
        name: '',
        description: '',
        importPrice: null,
        pictureUrl: '',
        price: null,
        productBrandId: null,
        productOptions: [],
        productSKU: '',
        productTypeId: null,
      };
    }
    if (this.isEdit == null) this.isEdit = false;

    this.addForm = new FormGroup({
      productName: new FormControl(this.product.name),
      productDescription: new FormControl(this.product.description),
      price: new FormControl(this.product.price),
      pictureUrl: new FormControl(this.product.pictureUrl),
      productTypeId: new FormControl(this.product.productTypeId),
      productBrandId: new FormControl(this.product.productBrandId),
      productSKU: new FormControl(this.product.productSKU),
      importPrice: new FormControl(this.product.importPrice),
    });
  }

  quickAddVariants() {
    this.bindDataToProductObject();
    this.product.productOptions = this.productOptions;
    this.productSKUs = this.generateSKUs(this.product);
    console.log(this.productSKUs); // Output các SKUs
  }

  // Hàm tạo các SKUs từ các biến thể của sản phẩm
  generateSKUs(product: Product): ProductSKUs[] {
    // Lấy tất cả các giá trị của các biến thể
    this.variantValues = product.productOptions.map(
      (option) => option.optionValues
    );

    // Hàm đệ quy để kết hợp các giá trị của các biến thể
    const combine = (
      values: string[][],
      index: number,
      current: string[]
    ): string[][] => {
      if (index === values.length) {
        return [current];
      }

      let result: string[][] = [];
      for (let value of values[index]) {
        result = result.concat(
          combine(values, index + 1, current.concat(value))
        );
      }
      return result;
    };

    const combinations = combine(this.variantValues, 0, []);

    const skus: ProductSKUs[] = combinations.map((values, skuIndex) => {
      const opt: ProductOption[] = [];
      this.productOptions.forEach((option, index) => {
        opt.push({
          name: option.optionName,
          value: values[index]
        })
      });

      return {
        id: skuIndex + 1,
        barcode: '',
        imageUrl: '',
        importPrice: null,
        sku: this.product.name + this.convertVieService.removeVietnameseTones(values.join('').replace(/\s/g, "")),
        quantity: 1,
        price: 1,
        weight: 1,
        options: opt,
      };
    });

    return skus;
  }

  startEdit(id: number): void {
    this.editId = id;
  }

  stopEdit() {
    this.editId = null;
  }

  handleOptionValueKeydown(event: any, data: ProductOptions) {
    if (event.key == 'Tab') {
      event.preventDefault();
    }

    if (event.key == 'Enter') {
      event.preventDefault();
      this.productOptions.find(
        (o) => o.productOptionId === data.productOptionId
      ).optionValues = data.optionValues;
    }
  }

  handleOptionKeydown(event: any, data: ProductOptions) {
    if (event.key == 'Enter') event.preventDefault();

    let index = this.productOptions.findIndex(
      (o) => o.productOptionId == data.productOptionId
    );
    this.productOptions[index].optionName = data.optionName;
  }

  destroyModal(): void {
    this.modal.destroy();
  }

  onSubmit() {
    this.bindDataToProductObject();

    if (!this.isEdit) {
      this.productService.addProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success('Thêm sản phẩm thành công!');
          this.destroyModal();
        },
        error: (err) => {
          this.toastrService.error(err);
        },
      });
    } else {
      this.productService.editProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success('Sửa sản phẩm thành công!');
          this.destroyModal();
        },
        error: (err) => {
          console.log(err);
          this.toastrService.error(err);
        },
      });
    }
  }

  bindDataToProductObject() {
    this.product.name = this.addForm.value.productName;
    this.product.importPrice = +this.addForm.value.importPrice;
    this.product.pictureUrl = 'images/products/sb-ang1.png';
    this.product.price = +this.addForm.value.price;
    this.product.productBrandId = 1;
    this.product.description = 'sample';
    // this.product.description = this.addForm.value.productDescription;
    this.product.productSKU = this.addForm.value.productSKU;
    this.product.productTypeId = 1;
  }

  onCreateVariants() {
    this.productOptions.push({
      optionName: '',
      optionValues: [],
      productOptionId: this.productOptionId,
    });

    this.productOptionId++;
  }

  drop(event: CdkDragDrop<string[]>): void {
    moveItemInArray(
      this.productOptions,
      event.previousIndex,
      event.currentIndex
    );
  }
}
