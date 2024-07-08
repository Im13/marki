import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { ThousandSeparatorPipe } from './pipes/thousand-separator.pipe';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzFormModule } from 'ng-zorro-antd/form';

@NgModule({
  declarations: [
    HeaderComponent,
    SidebarComponent,
    ThousandSeparatorPipe
  ],
  imports: [
    CommonModule,
    RouterModule,
    NzPopoverModule,
    NzUploadModule,
    NzFormModule
  ],
  exports: [
    HeaderComponent,
    SidebarComponent,
    ThousandSeparatorPipe,
    NzPopoverModule,
    NzUploadModule,
    NzFormModule
  ]
})
export class CoreModule { }
