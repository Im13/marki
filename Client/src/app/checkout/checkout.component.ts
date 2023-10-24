import { Component, OnInit } from '@angular/core';
import { BasketService } from '../basket/basket.service';
import { CheckoutService } from './checkout.service';
import { ToastrService } from 'ngx-toastr';
import { Basket } from '../shared/models/basket';
import { Address } from '../shared/models/address';
import { NavigationExtras, Router } from '@angular/router';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  isDisplayingOrder: boolean = true;

  constructor(public basketService: BasketService, private checkoutService: CheckoutService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
  }

  submitOrder() {
    const basket = this.basketService.getCurrentBasketValue();
    if(!basket) return;

    const orderToCreate = this.getOrderToCreate(basket);
    console.log(orderToCreate);

    if(!orderToCreate) return;

    this.checkoutService.createOrder(orderToCreate).subscribe({
      next: order => {
        this.toastr.success('Order created successfully!');
        this.basketService.deleteLocalBasket();
        const navigationExtras: NavigationExtras = { state: order };
        this.router.navigate(['checkout/success'], navigationExtras);
      }
    });
  }

  private getOrderToCreate(basket: Basket) {
    const deliveryMethodId = 1;
    const shipToAddress: Address = {
      fullName: 'Address fullname',
      cityOrProvinceId: 1,
      districtId: 1,
      street: 'Hong Ha',
      wardId: 1
    };

    if(!deliveryMethodId || !shipToAddress) return null;

    return {
      basketId: basket.id,
      deliveryMethodId: deliveryMethodId,
      shipToAddress: shipToAddress
    };
  }

  toggleDisplayOrder() {
    this.isDisplayingOrder = !this.isDisplayingOrder;
  }

}
