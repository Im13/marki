import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { DragDropModule } from '@angular/cdk/drag-drop';
import { NzTableModule } from 'ng-zorro-antd/table';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    PaginationModule.forRoot(),
    NgxSpinnerModule.forRoot(),
    NzSelectModule,
    NzModalModule,
    NzButtonModule,
    NzInputModule,
    NzIconModule,
    DragDropModule,
    NzTableModule
  ],
  exports: [
    PaginationModule,
    NgxSpinnerModule,
    NzSelectModule,
    NzModalModule,
    NzButtonModule,
    NzInputModule,
    NzIconModule,
    DragDropModule,
    NzTableModule
  ]
})
export class SharedModule { }
