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



@NgModule({
  declarations: [
    HeaderComponent,
    TestErrorComponent,
    NotFoundComponent,
    ServerErrorComponent,
    LoginComponent,
    AccountInfoComponent
  ],
  imports: [
    CommonModule,
    RouterModule,
    ToastrModule.forRoot({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    }),
    BasketModule,
    ReactiveFormsModule
  ],
  exports: [
    HeaderComponent,
    ReactiveFormsModule
  ]
})
export class CoreModule { }
