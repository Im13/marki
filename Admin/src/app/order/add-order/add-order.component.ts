import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.css']
})
export class AddOrderComponent implements OnInit {
  tabs = ['Tab 1', 'Tab 2'];
  selectedIndex = 0;
  addOrderForm: FormGroup;

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.addOrderForm = this.formBuilder.group({
      checkout: this.formBuilder.group({
        shippingFee: this.formBuilder.control('', [Validators.required]),
        orderDiscount: this.formBuilder.control('', [Validators.required]),
        bankTranferedAmount: this.formBuilder.control('', [Validators.required]),
        extraFee: this.formBuilder.control('', [Validators.required]),
        orderNote: this.formBuilder.control('')
      }),
      information: this.formBuilder.group({
        orderCreatedDate: this.formBuilder.control('', [Validators.required]),
        orderCareStaff: this.formBuilder.control('', [Validators.required]),
        customerCareStaff: this.formBuilder.control('', [Validators.required])
      })
    });
  }

  submitForm() {
    console.log(this.addOrderForm.value);
  }

  closeTab({ index }: { index: number }): void {
    this.tabs.splice(index, 1);
  }

  newTab(): void {
    this.tabs.push('New Tab');
    this.selectedIndex = this.tabs.length;
  }

}
