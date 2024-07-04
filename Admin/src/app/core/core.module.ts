import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { ThousandSeparatorPipe } from './pipes/thousand-separator.pipe';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzUploadModule } from 'ng-zorro-antd/upload';

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
    NzUploadModule
  ],
  exports: [
    HeaderComponent,
    SidebarComponent,
    ThousandSeparatorPipe,
    NzPopoverModule,
    NzUploadModule
  ]
})
export class CoreModule { }
