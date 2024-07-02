import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ProductParams } from 'src/app/shared/models/productParams';
import { ProductService } from '../product-service.service';
import { Product } from 'src/app/shared/models/products';
import { NzModalService } from 'ng-zorro-antd/modal';
import { AddProductModalComponent } from './add-product-modal/add-product-modal.component';
import { NzTableQueryParams } from 'ng-zorro-antd/table';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})

export class ProductListComponent implements OnInit {
  products: Product[] = [];
  listOfCurrentPageProducts: readonly Product[] = [];
  setOfCheckedId = new Set<number>();

  bsModalRef?: BsModalRef;
  productParams = new ProductParams();
  checked = false;
  indeterminate = false;
  loading = true;

  totalCount = 0;

  constructor(
    private productService: ProductService,
    private modalServices: NzModalService,
    private toastrService: ToastrService) {
  }

  ngOnInit(): void {
    this.getProducts();
  }

  refreshCheckedStatus(): void {
    this.checked = this.listOfCurrentPageProducts.every(item => this.setOfCheckedId.has(item.id));
    this.indeterminate = this.listOfCurrentPageProducts.some(item => this.setOfCheckedId.has(item.id)) && !this.checked;
  }

  onCurrentPageDataChange($event: readonly Product[]): void {
    this.listOfCurrentPageProducts = $event;
    this.refreshCheckedStatus();
  }

  onAllChecked(value: boolean): void {
    this.listOfCurrentPageProducts.forEach(item => this.updateCheckedSet(item.id, value));
    this.refreshCheckedStatus();
  }

  updateCheckedSet(id: number, checked: boolean): void {
    if (checked) {
      this.setOfCheckedId.add(id);
    } else {
      this.setOfCheckedId.delete(id);
    }
  }

  onItemChecked(id: number, checked: boolean): void {
    this.updateCheckedSet(id, checked);
    this.refreshCheckedStatus();
  }

  onQueryParamsChange(params: NzTableQueryParams): void {
    const { pageSize, pageIndex } = params;
    if (this.productParams.pageIndex !== pageIndex) {
      this.productParams.pageIndex = pageIndex;
      this.productParams.pageSize = pageSize;
      this.getProducts();
    }
  }

  deleteSelectedProducts() {
    this.loading = true;
    const seletedProduct = this.products.filter(data => this.setOfCheckedId.has(data.id));

    this.modalServices.confirm({
      nzTitle: 'Bạn muốn xoá các sản phẩm đang chọn?',
      nzOkText: 'Yes',
      nzOkType: 'primary',
      nzOkDanger: true,
      nzOnOk: () => {
        this.productService.deleteProducts(seletedProduct).subscribe({
          next: () => {
            this.getProducts();
            this.toastrService.success('Remove items success!');
          },
          error: err => {
            console.log(err);
          }
        });
      },
      nzCancelText: 'No',
      nzOnCancel: () => {
        this.loading = false;
      }
    });
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
    const modal = this.modalServices.create<AddProductModalComponent, Product>({
      nzTitle: 'Thiết lập sản phẩm',
      nzContent: AddProductModalComponent,
      nzCentered: true,
      nzWidth: '160vh',
      nzData: product
    });

    modal.afterClose.subscribe(() => this.getProducts());
  }

  getProducts() {
    this.loading = true;
    this.productService.getProducts(this.productParams).subscribe({
      next: response => {
        this.products = response.data;
        this.productParams.pageIndex = response.pageIndex;
        this.productParams.pageSize = response.pageSize;
        this.totalCount = response.count;
        this.loading = false;
      },
      error: err => {
        console.log(err);
        this.loading = false;
      }
    });
  }
}
