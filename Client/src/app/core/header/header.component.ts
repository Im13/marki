import { Component, ElementRef, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { BasketItem } from 'src/app/shared/models/basket';
import { RouterLink } from '@angular/router';
import { NgClass, NgIf, AsyncPipe } from '@angular/common';
import { LoginComponent } from './login/login.component';
import { AccountInfoComponent } from './account-info/account-info.component';

@Component({
    selector: 'app-header',
    templateUrl: './header.component.html',
    styleUrls: ['./header.component.css'],
    host: {
        '(document:click)': 'onClick($event)',
    },
    standalone: true,
    imports: [RouterLink, NgClass, NgIf, LoginComponent, AccountInfoComponent, AsyncPipe]
})
export class HeaderComponent {
  menuOpen = false;
  accountFormOpen = false;

  constructor(private _eref: ElementRef, 
    public basketService: BasketService,
    public accountService: AccountService) { }

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  openMenu() {
    this.menuOpen = !this.menuOpen;
    this.accountFormOpen = false;
  }

  toggleAccountBtn() {
    this.accountFormOpen = !this.accountFormOpen;
    this.menuOpen = false;
  }

  // Remove menu items when click outside
  onClick(event) {
    if(!this._eref.nativeElement.contains(event.target)) {
      this.menuOpen = false;
      this.accountFormOpen = false;
    }
  }

}
