import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
// import { NgZorroAntdModule } from 'ng-zorro-antd';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzButtonModule } from 'ng-zorro-antd/button';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    NgxSpinnerModule.forRoot(),
    NzSelectModule,
    NzModalModule,
    NzButtonModule
  ],
  exports: [
    PaginationModule,
    NgxSpinnerModule,
    NzSelectModule,
    NzModalModule,
    NzButtonModule
  ]
})
export class SharedModule { }
