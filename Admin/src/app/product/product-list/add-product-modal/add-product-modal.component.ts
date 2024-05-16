import { Component, Input, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { Product } from 'src/app/shared/models/products';
import { ProductService } from '../../product-service.service';
import { ToastrService } from 'ngx-toastr';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { ProductOptions } from 'src/app/shared/models/productOptions';

@Component({
  selector: 'app-add-product-modal',
  templateUrl: './add-product-modal.component.html',
  styleUrls: ['./add-product-modal.component.css']
})
export class AddProductModalComponent implements OnInit {
  @Input() product?: Product = inject(NZ_MODAL_DATA);;

  addForm: FormGroup;
  isEdit?: boolean;
  productOptions: ProductOptions[] = [];
  productOptionId: number = 0;
  currentOptionValueText = '';

  constructor(private modal: NzModalRef,
    private productService: ProductService,
    private toastrService: ToastrService
  ) { }

  ngOnInit(): void {
    if (this.product == null) this.product = {} as Product;
    if (this.isEdit == null) this.isEdit = false;

    this.addForm = new FormGroup({
      'productName': new FormControl(this.product.name),
      'productDescription': new FormControl(this.product.description),
      'price': new FormControl(this.product.price),
      'pictureUrl': new FormControl(this.product.pictureUrl),
      'productTypeId': new FormControl(this.product.productTypeId),
      'productBrandId': new FormControl(this.product.productBrandId),
      'productSKU': new FormControl(this.product.productSKU),
      'importPrice': new FormControl(this.product.importPrice),
    })
  }

  handleOptionValueKeydown(event: any, data: ProductOptions) {
    if (event.key == 'Tab') {
      event.preventDefault();
      console.log('tab pressed');
      console.log(data.optionValues);
    }

    if (event.key == 'Enter') {
      event.preventDefault();
      this.productOptions.find(o => o.productOptionId === data.productOptionId).optionValues = data.optionValues;
      console.log(this.productOptions);
    }
  }

  handleOptionKeydown(event: any, data: ProductOptions) {
    if(event.key == 'Enter')
      event.preventDefault();

    let index = this.productOptions.findIndex(o => o.productOptionId == data.productOptionId);
    this.productOptions[index].optionName = data.optionName;

    console.log(this.productOptions);
  }

  destroyModal(): void {
    this.modal.destroy();
  }

  onSubmit() {
    this.product.name = this.addForm.value.productName;
    this.product.importPrice = +this.addForm.value.importPrice;
    this.product.pictureUrl = "images/products/sb-ang1.png";
    this.product.price = +this.addForm.value.price;
    this.product.productBrandId = 1;
    this.product.description = "sample";
    // this.product.description = this.addForm.value.productDescription;
    this.product.productSKU = this.addForm.value.productSKU;
    this.product.productTypeId = 1;

    if (!this.isEdit) {
      this.productService.addProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success("Thêm sản phẩm thành công!")
          this.destroyModal();
        },
        error: err => {
          this.toastrService.error(err)
        }
      })
    } else {
      this.productService.editProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success("Sửa sản phẩm thành công!");
          this.destroyModal();
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err);
        }
      })
    }
  }

  onCreateVariants() {
    this.productOptions.push({
      optionName: '',
      optionValues: [],
      productOptionId: this.productOptionId
    });

    this.productOptionId++;
  }

  drop(event: CdkDragDrop<string[]>): void {
    moveItemInArray(this.productOptions, event.previousIndex, event.currentIndex);
  }
}
