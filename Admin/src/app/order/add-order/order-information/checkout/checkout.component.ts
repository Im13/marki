import { Component, ElementRef, Input, OnInit, SimpleChanges, ViewChild } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-checkout',
  templateUrl: './checkout.component.html',
  styleUrls: ['./checkout.component.css'],
})
export class CheckoutComponent implements OnInit {
  @Input() totalSKUsPrice!: number;
  @ViewChild('discount') discount: ElementRef;

  checkoutForm!: FormGroup;
  orderTotal: number;
  shippingFee = 0;
  orderDiscount = 0;
  bankTransferedAmount = 0;
  extraFee = 0;
  freeshipChecked = false;

  //After caculated variables
  afterDiscount = 0;
  total = 0;

  constructor(private rootFormGroup: FormGroupDirective) {}

  ngOnInit(): void {
    this.orderTotal = this.totalSKUsPrice;
    this.checkoutForm = this.rootFormGroup.control.get('checkout') as FormGroup;
    console.log(this.rootFormGroup.control)

    this.checkoutForm.setValue({
      freeshipChecked: false,
      shippingFee: 0,
      orderDiscount: 0,
      bankTransferedAmount: 0,
      extraFee: 0,
      orderNote: '',
    });
  }

  ngOnChanges(): void {
    this.calculateOrderTotal();
  }

  onFeesChange() {
    this.shippingFee = +this.checkoutForm.value.shippingFee;
    this.orderDiscount = +this.checkoutForm.value.orderDiscount;
    this.bankTransferedAmount = +this.checkoutForm.value.bankTransferedAmount;
    this.extraFee = +this.checkoutForm.value.extraFee;
    console.log(this.totalSKUsPrice)
    this.calculateOrderTotal();
  }

  calculateOrderTotal() {
    this.orderTotal = this.totalSKUsPrice + this.shippingFee + this.extraFee;
    this.afterDiscount = this.orderTotal - this.orderDiscount;
    this.total = this.afterDiscount - this.bankTransferedAmount;
  }

  // This will be called everytime form's freeshipChecked perform check
  onFreeshipChecked() {
    if (this.checkoutForm.value.freeshipChecked === true) {
      this.checkoutForm.patchValue({
        shippingFee: 0,
      });

      this.shippingFee = this.checkoutForm.value.shippingFee;
    }

    this.calculateOrderTotal();
  }

  onShippingFeesChange() {
    var currentShippingFee = this.checkoutForm.value.shippingFee;

    // When freeshipChecked changed to false, onFreeshipChecked method will be automatically called
    this.checkoutForm.patchValue({
      freeshipChecked: false,
      shippingFee: currentShippingFee
    });

    this.onFeesChange();
  }

  handleKeydown(event: any) {
    if (event.key == 'Enter') {
      this.discount.nativeElement.focus();
    }
  }
}
