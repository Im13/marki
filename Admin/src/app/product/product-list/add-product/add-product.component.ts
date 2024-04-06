import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Product } from 'src/app/shared/models/products';
import { ProductService } from '../../product-service.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})

export class AddProductComponent implements OnInit {
  title?: string;
  product?: Product;
  isEdit?: boolean;
  closeBtnName?: string;
  addForm: FormGroup;

  constructor(public bsModalRef: BsModalRef,
    private productService: ProductService,
    private toastrService: ToastrService) {}

  ngOnInit() {
    if(this.product == null) this.product = {} as Product;
    if(this.isEdit == null) this.isEdit = false;

    this.addForm = new FormGroup({
      'productName': new FormControl(this.product.name),
      'productDescription': new FormControl(this.product.description),
      'price': new FormControl(this.product.price),
      'pictureUrl': new FormControl(this.product.pictureUrl),
      'productTypeId': new FormControl(this.product.productTypeId),
      'productBrandId': new FormControl(this.product.productBrandId),
      'productSKU': new FormControl(this.product.productSKU),
      'importPrice': new FormControl(this.product.importPrice)
    })

    console.log(this.product);
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

    if(!this.isEdit) {
      this.productService.addProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success("Thêm sản phẩm thành công!")
          this.bsModalRef.hide();
        },
        error: err => {
          this.toastrService.error(err)
        }
      })
    } else {
      this.productService.editProduct(this.product).subscribe({
        next: () => {
          this.toastrService.success("Sửa sản phẩm thành công!");
          this.bsModalRef.hide();
        },
        error: err => {
          console.log(err);
          this.toastrService.error(err);
        }
      })
    }

    
  }
}
