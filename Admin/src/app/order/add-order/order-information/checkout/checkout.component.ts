import { Component, Input, OnInit } from '@angular/core';
import {
  FormGroup,
  FormGroupDirective,
} from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  @Input() totalSKUsPrice!: number;
  checkoutForm!: FormGroup;
  orderTotal: number;

  constructor(private rootFormGroup: FormGroupDirective) {}

  ngOnInit(): void {
    this.orderTotal = this.totalSKUsPrice;
    this.checkoutForm = this.rootFormGroup.control.get('checkout') as FormGroup;

    this.checkoutForm.setValue({
      shippingFee: 0,
      orderDiscount: 0,
      bankTranferedAmount: 0,
      extraFee: 0,
      orderNote: ''
    });
  }

  
}
