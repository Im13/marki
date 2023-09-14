import { Component, ElementRef, OnInit } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { BasketItem } from 'src/app/shared/models/basket';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  host: {
    '(document:click)' : 'onClick($event)',
  }
})
export class HeaderComponent {
  menuOpen = false;

  constructor(private _eref: ElementRef, public basketService: BasketService) { }

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  openMenu() {
    this.menuOpen = !this.menuOpen;
  }

  clickUser() {

  }

  // Remove menu items when click outside
  onClick(event) {
    if(!this._eref.nativeElement.contains(event.target)) {
      this.menuOpen = false;
    }
  }

}
