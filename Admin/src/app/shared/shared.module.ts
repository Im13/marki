import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    NgxSpinnerModule.forRoot()
  ],
  exports: [
    PaginationModule,
    NgxSpinnerModule
  ]
})
export class SharedModule { }
