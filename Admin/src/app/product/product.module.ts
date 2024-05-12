import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListComponent } from './product-list/product-list.component';
import { RouterModule } from '@angular/router';
import { ProductRoutingModule } from './product-routing.module';
import { AddProductComponent } from './product-list/add-product/add-product.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { NzModalModule } from 'ng-zorro-antd/modal';

@NgModule({
  declarations: [
    ProductListComponent,
    AddProductComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ProductRoutingModule,
    ReactiveFormsModule,
    SharedModule,
    PopoverModule.forRoot(),
    FormsModule,
    NzModalModule
  ],
  exports: [
    ProductListComponent
  ]
})
export class ProductModule { }
