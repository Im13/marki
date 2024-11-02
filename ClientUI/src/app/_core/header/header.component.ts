import { Component, ElementRef } from '@angular/core';
import { Router } from '@angular/router';
import { BasketItem } from 'src/app/_shared/_models/basket';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  host: {
    '(document:click)' : 'onClick($event)',
  }
})
export class HeaderComponent {
  isMenuCollapsed = true;
  isLoginCollapsed = true;

  constructor(private _eref: ElementRef, public basketService: BasketService, private router: Router) {}

  // Remove menu items when click outside
  onClick(event: any) {
    if(!this._eref.nativeElement.contains(event.target)) {
      this.isMenuCollapsed = true;
      this.isLoginCollapsed = true;
    }
  }

  collapseLoginRegisterForm() {
    this.isLoginCollapsed = !this.isLoginCollapsed;
    this.isMenuCollapsed = true;
  }

  collapseMenuForm() {
    this.isMenuCollapsed = !this.isMenuCollapsed;
    this.isLoginCollapsed = true;
  }

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  redirectToHomePage() {
    this.router.navigate(['/']);
  }
}
