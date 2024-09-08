import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { HeaderComponent } from './header/header.component';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzCollapseModule } from 'ng-zorro-antd/collapse';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { NzInputModule } from 'ng-zorro-antd/input';

@NgModule({
  declarations: [
    HeaderComponent
  ],
  imports: [
    CommonModule,
    NzLayoutModule,
    NzBreadCrumbModule,
    NzMenuModule,
    NzIconModule,
    NzGridModule,
    NzDropDownModule,
    NzCollapseModule,
    BrowserAnimationsModule,
    CollapseModule.forRoot(),
    NzInputModule
  ],
  exports: [
    NzLayoutModule,
    NzBreadCrumbModule,
    NzMenuModule,
    HeaderComponent,
    NzIconModule,
    NzGridModule,
    NzDropDownModule,
    NzCollapseModule,
    BrowserAnimationsModule,
    CollapseModule,
    NzInputModule
  ]
})
export class CoreModule { }
