import { Component } from '@angular/core';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent {
  shippingFee = 0;
  orderDiscount = 0;
  orderNote = '';
  freeShippingChecked = false;
}
