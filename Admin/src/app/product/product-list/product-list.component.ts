import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { AddProductComponent } from './add-product/add-product.component';
import { ProductParams } from 'src/app/shared/models/productParams';
import { ProductService } from '../product-service.service';
import { Product } from 'src/app/shared/models/products';
import { Subscription } from 'rxjs';
import { NzModalRef, NzModalService } from 'ng-zorro-antd/modal';
import { AddProductModalComponent } from './add-product-modal/add-product-modal.component';

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

  constructor(private modalService: BsModalService,
    private productService: ProductService,
    private modalServices: NzModalService,
    private viewContainerRef: ViewContainerRef) {
  }

  ngOnInit(): void {
    this.getProducts();
  }

  displayCreateModal() {
    const modal = this.modalServices.create<AddProductModalComponent>({
      nzTitle: 'Thiết lập sản phẩm',
      nzContent: AddProductModalComponent,
      nzCentered: true,
      nzWidth: '160vh'
    });

    modal.afterClose.subscribe(() => this.getProducts());
  }

  displayEditModal(product: Product) {
    const modal = this.modalServices.create<AddProductModalComponent,Product>({
      nzTitle: 'Thiết lập sản phẩm',
      nzContent: AddProductModalComponent,
      nzCentered: true,
      nzWidth: '160vh',
      nzData: product
    });

    modal.afterClose.subscribe(() => this.getProducts());
  }

  onPageChanged(event: any) {
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
