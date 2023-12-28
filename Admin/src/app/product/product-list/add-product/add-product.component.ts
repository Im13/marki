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
  closeBtnName?: string;
  addForm: FormGroup;
  product = {} as Product;

  constructor(public bsModalRef: BsModalRef,
    private productService: ProductService,
    private toastrService: ToastrService) {}

  ngOnInit() {
    this.addForm = new FormGroup({
      'productName': new FormControl(),
      'productDescription': new FormControl(),
      'price': new FormControl(),
      'pictureUrl': new FormControl(),
      'productTypeId': new FormControl(),
      'productBrandId': new FormControl(),
      'productSKU': new FormControl(),
      'importPrice': new FormControl()
    })
  }

  onSubmit() {
    this.product.name = this.addForm.value.productName;
    this.product.importPrice = +this.addForm.value.importPrice;
    this.product.pictureUrl = "images/products/sb-ang1.png";
    this.product.price = +this.addForm.value.price;
    this.product.productBrandId = 1;
    this.product.description = this.addForm.value.productDescription;
    this.product.productSKU = this.addForm.value.productSKU;
    this.product.productTypeId = 1;

    this.productService.addProduct(this.product).subscribe({
      next: () => {
        this.toastrService.success("Thêm sản phẩm thành công!")
        this.bsModalRef.hide();
      },
      error: err => {
        this.toastrService.error(err)
      }
    })
  }
}
