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

  constructor(private router: Router, public basketService: BasketService){
    console.log('A', this.basketItems)
  }

  ngOnInit(): void {
    this.basketService.basketSource$.pipe(
      filter(val => val !== null)  // Lọc bỏ các giá trị null
    ).subscribe({
      next: val => {
        this.basketItems = val.items;

        this.processedBasketItems = this.basketItems.map(item => ({
          ...item,
          formattedSKUValues: item.productSKUValues.map(skuValue => skuValue.productOptionValue.valueName).join(' / ')
        }));
      }
    });

    this.basketService.basketTotalSource$.pipe(
      filter(val => val !== null)  // Lọc bỏ các giá trị null
    ).subscribe({
      next: val => {
        this.basketTotal = val;
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
