import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})

export class AddProductComponent implements OnInit {
  title?: string;
  closeBtnName?: string;
  addForm: FormGroup;
 
  constructor(public bsModalRef: BsModalRef) {}
 
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

}
