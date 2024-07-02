import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { ThousandSeparatorPipe } from './pipes/thousand-separator.pipe';

@NgModule({
  declarations: [
    HeaderComponent,
    SidebarComponent,
    ThousandSeparatorPipe
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    HeaderComponent,
    SidebarComponent,
    ThousandSeparatorPipe
  ]
})
export class CoreModule { }
