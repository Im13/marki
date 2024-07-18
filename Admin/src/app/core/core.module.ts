import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from '@angular/router';
import { ThousandSeparatorPipe } from './pipes/thousand-separator.pipe';
import { NzPopoverModule } from 'ng-zorro-antd/popover';
import { NzUploadModule } from 'ng-zorro-antd/upload';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzToolTipModule } from 'ng-zorro-antd/tooltip';
import { NzSpaceModule } from 'ng-zorro-antd/space';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzTabsModule } from 'ng-zorro-antd/tabs';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzSegmentedModule } from 'ng-zorro-antd/segmented';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzEmptyModule } from 'ng-zorro-antd/empty';
import { NzTypographyModule } from 'ng-zorro-antd/typography';

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
    NzFormModule,
    NzToolTipModule,
    NzSpaceModule,
    NzLayoutModule,
    NzTabsModule,
    NzGridModule,
    NzSegmentedModule,
    NzSelectModule,
    FormsModule,
    ReactiveFormsModule,
    NzEmptyModule,
    NzTypographyModule
  ],
  exports: [
    HeaderComponent,
    SidebarComponent,
    ThousandSeparatorPipe,
    NzPopoverModule,
    NzUploadModule,
    NzFormModule,
    NzToolTipModule,
    NzSpaceModule,
    NzLayoutModule,
    NzTabsModule,
    NzGridModule,
    NzSegmentedModule,
    NzSelectModule,
    FormsModule,
    ReactiveFormsModule,
    NzEmptyModule,
    NzTypographyModule
  ]
})
export class CoreModule { }
