import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductListComponent } from './product-list/product-list.component';
import { RouterModule } from '@angular/router';
import { ProductRoutingModule } from './product-routing.module';
import { SharedModule } from '../_shared/shared.module';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { AddProductModalComponent } from './product-list/add-product-modal/add-product-modal.component';
import { CoreModule } from '../_core/core.module';
import { NzImageModule } from 'ng-zorro-antd/image';
import { NzInputNumberModule } from 'ng-zorro-antd/input-number';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { SyncValuesComponent } from './product-list/add-product-modal/sync-values/sync-values.component';


@NgModule({
  declarations: [
    ProductListComponent,
    AddProductModalComponent,
    SyncValuesComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ProductRoutingModule,
    SharedModule,
    PopoverModule.forRoot(),
    CoreModule,
    NzImageModule,
    NzInputNumberModule,
    NgxMaskDirective,
    NgxMaskPipe
  ],
  providers: [],
  exports: [
    ProductListComponent
  ]
})
export class ProductModule { }
