import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { DragDropModule } from '@angular/cdk/drag-drop';

import { SearchBoxComponent } from './_components/search-box/search-box.component';
import { CoreModule } from '../_core/core.module';

@NgModule({
  declarations: [
    SearchBoxComponent
  ],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    NgxSpinnerModule.forRoot(),
    DragDropModule,
    CoreModule
  ],
  exports: [
    PaginationModule,
    NgxSpinnerModule,
    DragDropModule,
    SearchBoxComponent
  ]
})
export class SharedModule { }
