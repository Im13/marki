import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { AddProductComponent } from './add-product/add-product.component';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  bsModalRef?: BsModalRef;
  
  constructor(private modalService: BsModalService) {
  }

  ngOnInit(): void {
    this.openAddProductModal();
  }

  openAddProductModal() {
    const initialState: ModalOptions = {
      initialState: {
        title: 'Thiết lập sản phẩm'
      },
      class: 'modal-xl'
    }

    this.bsModalRef = this.modalService.show(AddProductComponent, initialState);
    this.bsModalRef.content.closeBtnName = 'Close';
  }
}
