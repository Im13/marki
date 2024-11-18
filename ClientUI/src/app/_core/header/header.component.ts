import { Component, ElementRef, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BasketItem } from 'src/app/_shared/_models/basket';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  host: {
    '(document:click)': 'onClick($event)',
  }
})
export class HeaderComponent implements OnInit {
  isMenuCollapsed = true;
  isLoginCollapsed = true;
  isCartCollapsed = true;
  basketItems: BasketItem[] = [];

  constructor(private _eref: ElementRef, private router: Router, public basketService: BasketService) { }

  ngOnInit(): void {
    
  }

  // Remove menu items when click outside
  onClick(event: any) {
    if (!this._eref.nativeElement.contains(event.target)) {
      this.isMenuCollapsed = true;
      this.isLoginCollapsed = true;
      this.isCartCollapsed = true;
    }
  }

  collapseLoginRegisterForm() {
    this.isMenuCollapsed = true;
    this.isCartCollapsed = true;
    this.isLoginCollapsed = !this.isLoginCollapsed;
  }

  collapseMenuForm() {
    this.isMenuCollapsed = !this.isMenuCollapsed;
    this.isLoginCollapsed = true;
    this.isCartCollapsed = true;
  }

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  redirectToHomePage() {
    this.collaseAll();
    this.router.navigate(['/']);
  }

  collapseCartForm() {
    this.isMenuCollapsed = true;
    this.isLoginCollapsed = true;
    this.isCartCollapsed = !this.isCartCollapsed;
  }

  onNavigate() {
    this.isCartCollapsed = true;
  }

  collaseAll() {
    this.isMenuCollapsed = true;
    this.isLoginCollapsed = true;
    this.isCartCollapsed = true;
  }
}
