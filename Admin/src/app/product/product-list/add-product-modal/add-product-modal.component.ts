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
import { ProductOptionValue } from 'src/app/shared/models/productOptionValues';
import { ProductSKUValue } from 'src/app/shared/models/productSKUValue';

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
  variantValues: ProductOptionValue[][] = [];

  constructor(
    private modal: NzModalRef,
    private productService: ProductService,
    private toastrService: ToastrService,
    private convertVieService: ConvertVieService
  ) {}

  ngOnInit(): void {
    if (this.product == null) {
      this.product = {
        name: 'tensp',
        description: 'mota',
        importPrice: 100000,
        productBrandId: null,
        productOptions: [],
        productSKU: 'sku001',
        productTypeId: null,
        productSkus: []
      };
    }
    if (this.isEdit == null) this.isEdit = false;

    this.addForm = new FormGroup({
      productName: new FormControl(this.product.name),
      productDescription: new FormControl(this.product.description),
      productTypeId: new FormControl(this.product.productTypeId),
      productBrandId: new FormControl(this.product.productBrandId),
      productSKU: new FormControl(this.product.productSKU),
      importPrice: new FormControl(this.product.importPrice),
    });

    //Fake data
    this.productOptions = [
      {
        optionName: 'Size',
        productOptionId: 0,
        productOptionValues: [
          {
            valueTempId: 1,
            value: 'S'
          },
          {
            valueTempId: 2,
            value: 'M'
          },
          {
            valueTempId: 3,
            value: 'L'
          }
        ],
        displayedValues: ['S', 'M', 'L']
      },
      {
        optionName: 'Color',
        productOptionId: 1,
        productOptionValues: [
          {
            valueTempId: 4,
            value: 'White'
          },
          {
            valueTempId: 5,
            value: 'Red'
          },
          {
            valueTempId: 6,
            value: 'Blue'
          }
        ],
        displayedValues: ['White', 'Red', 'Blue']
      }
    ]
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
      (option) => option.productOptionValues
    );

    // Hàm đệ quy để kết hợp các giá trị của các biến thể
    const combine = (
      values: ProductOptionValue[][],
      index: number,
      current: ProductOptionValue[]
    ): ProductOptionValue[][] => {
      if (index === values.length) {
        return [current];
      }

      let result: ProductOptionValue[][] = [];
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
      const productSkuValues: ProductSKUValue[] = [];
      let skuName: string = '';

      this.productOptions.forEach((option, index) => {
        opt.push({
          valueTempId: values[index].valueTempId,
          name: option.optionName,
          value: values[index].value
        });
        productSkuValues.push({
          valueTempId: values[index].valueTempId
        })

        skuName += values[index].value;
      });

      return {
        id: skuIndex + 1,
        barcode: '',
        imageUrl: 'thisisimageurl',
        importPrice: 0,
        sku: this.product.productSKU + this.convertVieService.removeVietnameseTones(skuName.replace(/\s/g, "")),
        quantity: 1,
        price: 1,
        weight: 1,
        productOptionValue: opt,
        productSKUValues: productSkuValues
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
      ).productOptionValues = data.productOptionValues;
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

    console.log(this.product);

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
    this.product.productBrandId = 1;
    this.product.description = 'sample';
    // this.product.description = this.addForm.value.productDescription;
    this.product.productSKU = this.addForm.value.productSKU;
    this.product.productTypeId = 1;
    this.product.productSkus = this.productSKUs;
  }

  onCreateVariants() {
    this.productOptions.push({
      optionName: '',
      productOptionValues: [],
      productOptionId: this.productOptionId,
      displayedValues: []
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
