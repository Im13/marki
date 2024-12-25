import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { filter } from 'rxjs';
import { BasketItem, BasketTotals } from 'src/app/_shared/_models/basket';
import { BasketService } from 'src/app/basket/basket.service';

@Component({
  selector: 'app-cart-dropdown',
  templateUrl: './cart-dropdown.component.html',
  styleUrls: ['./cart-dropdown.component.css']
})
export class CartDropdownComponent implements OnInit {
  @Output() onNavigate = new EventEmitter();
  @Input('basketItems') basketItems: BasketItem[];
  processedBasketItems: any[] = [];
  basketTotal: BasketTotals;
  defaultTotal = { subtotal: 0, discount: 0, total: 0 };

  constructor(private router: Router, public basketService: BasketService){
  }

  ngOnInit(): void {
    this.basketService.basketSource$.subscribe({
      next: val => {
        this.basketItems = val?.items;
        console.log('Val', val)

        if(val !== null) {
          this.processedBasketItems = this.basketItems.map(item => ({
            ...item,
            formattedSKUValues: item.productSKUValues.map(skuValue => skuValue.productOptionValue.valueName).join(' / ')
          }));
        }

      }
    });

    this.basketService.basketTotalSource$.subscribe({
      next: val => {
        if(val !== null)
          this.basketTotal = val;

        console.log(this.basketTotal)
      }
    });
  }

  onEditCart() {
    this.router.navigate(['/basket']);
    this.onNavigate.emit();
  }

  onCheckout() {
    this.router.navigate(['/checkout']);
    this.onNavigate.emit();
  }
}
