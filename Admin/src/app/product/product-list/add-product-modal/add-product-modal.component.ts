import { Component, Input, OnInit, inject } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { Product } from 'src/app/shared/models/products';
import { ProductService } from '../../product-service.service';
import { ToastrService } from 'ngx-toastr';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-add-product-modal',
  templateUrl: './add-product-modal.component.html',
  styleUrls: ['./add-product-modal.component.css']
})
export class AddProductModalComponent implements OnInit {
  @Input() product ?: Product = inject(NZ_MODAL_DATA);;
  addForm: FormGroup;
  isEdit?: boolean;

  // Fake data
  listOfData = [
    {
      key: '1',
      name: 'John Brown',
      age: 32
    },
    {
      key: '2',
      name: 'Jim Green',
      age: 42
    },
    {
      key: '3',
      name: 'Joe Black',
      age: 32
    }
  ];
  multipleValue = ['a10', 'c12'];
  listOfOption: Array<{ label: string; value: string }> = [];

  constructor(private modal: NzModalRef,
    private productService: ProductService,
    private toastrService: ToastrService
  ) {}

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
      // Fake Data
      'multipleValue': new FormControl(this.multipleValue)
    })

    const children: Array<{ label: string; value: string }> = [];
    for (let i = 10; i < 36; i++) {
      children.push({ label: i.toString(36) + i, value: i.toString(36) + i });
    }
    this.listOfOption = children;
  }

  handleKeydown(event:any) {
    if (event.key == 'Tab') {
      event.preventDefault();
      console.log('tab pressed');
    }

    if (event.key == 'Enter') {
      event.preventDefault();
      console.log('Enter pressed');
      console.log(this.listOfOption);
    }
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
    console.log('Create variants pressed!');
  }

  drop(event: CdkDragDrop<string[]>): void {
    moveItemInArray(this.listOfData, event.previousIndex, event.currentIndex);
  }
}
