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
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { SearchBoxComponent } from './_components/search-box/search-box.component';
import { CoreModule } from '../core/core.module';

@NgModule({
  declarations: [
    SearchBoxComponent
  ],
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
    NzTableModule,
    NzSwitchModule,
    CoreModule
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
    NzTableModule,
    NzSwitchModule,
    SearchBoxComponent
  ]
})
export class SharedModule { }
