import { Component } from '@angular/core';
import { BasketService } from './basket.service';
import { BasketItem } from '../_shared/_models/basket';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent {
  constructor(public basketService: BasketService) { }

  ngOnInit(): void {
  }

  getCount(items: BasketItem[]) {
    return items.reduce((sum, item) => sum + item.quantity, 0);
  }

  incrementQuantity(item: BasketItem) {
    this.basketService.addItemToBasket(item);
  }

  removeItem(id: number, quantity: number) {
    this.basketService.removeItemFromBasket(id, quantity);
  }
}
