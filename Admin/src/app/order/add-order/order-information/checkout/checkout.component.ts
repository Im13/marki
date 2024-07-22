import { Component, OnInit } from '@angular/core';
import {
  ControlValueAccessor,
  FormControl,
  FormGroup,
  FormGroupDirective,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css']
})
export class CheckoutComponent implements OnInit {
  checkoutForm!: FormGroup;

  constructor(private rootFormGroup: FormGroupDirective) {}

  ngOnInit(): void {
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
