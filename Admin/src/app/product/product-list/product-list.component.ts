import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { AddProductComponent } from './add-product/add-product.component';
import { ProductParams } from 'src/app/shared/models/productParams';
import { ProductService } from '../product-service.service';
import { Product } from 'src/app/shared/models/products';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit {
  products: Product[] = [];
  bsModalRef?: BsModalRef;
  productParams = new ProductParams();
  subscriptions: Subscription = new Subscription();

  totalCount = 0;

  constructor(private modalService: BsModalService, private productService: ProductService) {
  }

  ngOnInit(): void {
    this.getProducts();
  }

  openAddProductModal() {
    const initialState: ModalOptions = {
      initialState: {
        title: 'Thiết lập sản phẩm',
        isEdit: false
      },
      class: 'modal-xl'
    }

    this.bsModalRef = this.modalService.show(AddProductComponent, initialState);
    this.bsModalRef.content.closeBtnName = 'Close';

    this.subscriptions.add(
      this.modalService.onHide.subscribe((reason: string | any) => {
        this.getProducts();
      })
    );
  }

  onPageChanged(event: any) {
    console.log(event.page);
    if(this.productParams.pageIndex !== event.page) {
      this.productParams.pageIndex = event.page;
      this.getProducts();
    }
  }

  getProducts() {
    this.productService.getProducts(this.productParams).subscribe({
      next: response => {
        this.products = response.data;
        this.productParams.pageIndex = response.pageIndex;
        this.productParams.pageSize = response.pageSize;
        this.totalCount = response.count;
      },
      error: err => {
        console.log(err);
      }
    });
  }

  editProduct(product: Product) {
    const initialState: ModalOptions = {
      initialState: {
        title: 'Thiết lập sản phẩm',
        product: product,
        isEdit: true
      },
      class: 'modal-xl'
    }

    this.bsModalRef = this.modalService.show(AddProductComponent, initialState);
    this.bsModalRef.content.closeBtnName = 'Close';

    this.subscriptions.add(
      this.modalService.onHide.subscribe((reason: string | any) => {
        this.getProducts();
      })
    );
  }
}
