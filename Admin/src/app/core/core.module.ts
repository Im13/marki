import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
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
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzCheckboxModule } from 'ng-zorro-antd/checkbox';
import { NzDatePickerModule } from 'ng-zorro-antd/date-picker';
import { NzPaginationModule } from 'ng-zorro-antd/pagination';
import { NzTagModule } from 'ng-zorro-antd/tag';
import { NzBadgeModule } from 'ng-zorro-antd/badge';
import { NzDropDownModule } from 'ng-zorro-antd/dropdown';
import { NzStatisticModule } from 'ng-zorro-antd/statistic';
import { NzBreadCrumbModule } from 'ng-zorro-antd/breadcrumb';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';
import { NzModalModule } from 'ng-zorro-antd/modal';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzTableModule } from 'ng-zorro-antd/table';
import { NzSwitchModule } from 'ng-zorro-antd/switch';
import { NzAutocompleteModule } from 'ng-zorro-antd/auto-complete';
import { NzListModule } from 'ng-zorro-antd/list';
import { NzDrawerModule } from 'ng-zorro-antd/drawer';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptorService } from '../_authenticate/auth-interceptor.service';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzDividerModule } from 'ng-zorro-antd/divider';

@NgModule({
  declarations: [
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
    NzTypographyModule,
    NzCardModule,
    NzCheckboxModule,
    NzDatePickerModule,
    ReactiveFormsModule,
    FormsModule,
    NzPaginationModule,
    NzTagModule,
    NzBadgeModule,
    NzDropDownModule,
    NzStatisticModule,
    NzBreadCrumbModule,
    NzIconModule,
    NzInputModule,
    NzModalModule,
    NzButtonModule,
    NzTableModule,
    NzSwitchModule,
    NzAutocompleteModule,
    NzListModule,
    NzDrawerModule,
    NzAvatarModule,
    NzDividerModule
  ],
  exports: [
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
    NzTypographyModule,
    NzCardModule,
    NzCheckboxModule,
    NzDatePickerModule,
    ReactiveFormsModule,
    FormsModule,
    NzPaginationModule,
    NzTagModule,
    NzBadgeModule,
    NzDropDownModule,
    CommonModule,
    NzStatisticModule,
    NzBreadCrumbModule,
    NzIconModule,
    NzInputModule,
    NzModalModule,
    NzButtonModule,
    NzTableModule,
    NzSwitchModule,
    NzAutocompleteModule,
    NzListModule,
    NzDrawerModule,
    NzAvatarModule,
    NzDividerModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: AuthInterceptorService, multi: true}
  ]
})
export class CoreModule { }
