import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
// import { NgZorroAntdModule } from 'ng-zorro-antd';
import { NzSelectModule } from 'ng-zorro-antd/select';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    NgxSpinnerModule.forRoot(),
    NzSelectModule
  ],
  exports: [
    PaginationModule,
    NgxSpinnerModule,
    NzSelectModule
  ]
})
export class SharedModule { }
