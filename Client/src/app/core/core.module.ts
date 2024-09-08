import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component';
import { TestErrorComponent } from './test-error/test-error.component';
import { RouterModule } from '@angular/router';
import { NotFoundComponent } from './not-found/not-found.component';
import { ServerErrorComponent } from './server-error/server-error.component';
import { ToastrModule } from 'ngx-toastr';
import { BasketModule } from '../basket/basket.module';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './header/login/login.component';
import { AccountInfoComponent } from './header/account-info/account-info.component';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzMenuModule } from 'ng-zorro-antd/menu';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    }),
    BasketModule,
    ReactiveFormsModule,
    HeaderComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    LoginComponent,
    AccountInfoComponent,
    NzLayoutModule,
    NzMenuModule
  ],
  exports: [
    HeaderComponent,
    ReactiveFormsModule,
    NzLayoutModule,
    NzMenuModule
  ],
})
export class CoreModule {}
