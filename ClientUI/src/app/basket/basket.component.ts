import { Component } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../_shared/_models/basket';
import { Router } from '@angular/router';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent {
  constructor(public basketService: BasketService, private router: Router) { }

  ngOnInit(): void {
  }

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  incrementQuantity(item: BasketItem) {
    // this.basketService.addItemToBasket(item, item.productName, 1);
  }

  removeItem(id: number, quantity: number) {
    this.basketService.removeItemFromBasket(id, quantity);
  }

  redirectToCheckout() {
    this.router.navigate(['/checkout']);
  }
}
