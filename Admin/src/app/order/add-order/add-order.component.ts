import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Customer } from 'src/app/shared/models/cutomer';
import { Order } from 'src/app/shared/models/order';
import { ProductSKUDetails } from 'src/app/shared/models/productSKUDetails';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.css']
})
export class AddOrderComponent implements OnInit {
  addOrderForm: FormGroup;
  listSkus: ProductSKUDetails[] = [];
  totalSKUsPrice = 0;
  order: Order = new Order();
  customer: Customer = new Customer();

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.addOrderForm = this.formBuilder.group({
      checkout: this.formBuilder.group({
        freeshipChecked: this.formBuilder.control(false),
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
      }),
      customerInfo: this.formBuilder.group({
        customerName: this.formBuilder.control('', [Validators.required]),
        customerPhoneNumber: this.formBuilder.control('', [Validators.required]),
        customerEmailAddress: this.formBuilder.control('', [Validators.required]),
        customerDOB: this.formBuilder.control('', [Validators.required])
      }),
      receiverInfo: this.formBuilder.group({
        receiverName: this.formBuilder.control('', [Validators.required]),
        receiverPhoneNumber: this.formBuilder.control('', [Validators.required]),
        receiverAddress: this.formBuilder.control('', [Validators.required]),
        provinceId: this.formBuilder.control('', [Validators.required]),
        districtId: this.formBuilder.control('', [Validators.required]),
        wardId: this.formBuilder.control('', [Validators.required])
      }),
      deliveryService: this.formBuilder.group({
        deliveryCompanyId: this.formBuilder.control('', [Validators.required]),
        shipmentCode: this.formBuilder.control({value: '', disabled: true}, [Validators.required]),
        shipmentCost: this.formBuilder.control({value: '', disabled: true}, [Validators.required]),
      })
    });
  }

  submitForm() {
    this.order.address = this.addOrderForm.controls['receiverInfo'].value.receiverAddress;
    this.order.provinceId = this.addOrderForm.controls['receiverInfo'].value.provinceId;
    this.order.districtId = this.addOrderForm.controls['receiverInfo'].value.districtId;
    this.order.wardId = this.addOrderForm.controls['receiverInfo'].value.wardId;
    this.order.receiverName = this.addOrderForm.controls['receiverInfo'].value.receiverName;
    this.order.receiverPhoneNumber = this.addOrderForm.controls['receiverInfo'].value.receiverPhoneNumber;

    this.order.shippingFee = this.addOrderForm.controls['checkout'].value.shippingFee;
    this.order.orderDiscount = this.addOrderForm.controls['checkout'].value.orderDiscount;
    this.order.bankTransferedAmount = this.addOrderForm.controls['checkout'].value.bankTranferedAmount;
    this.order.extraFee = this.addOrderForm.controls['checkout'].value.extraFee;
    this.order.orderNote = this.addOrderForm.controls['checkout'].value.orderNote;

    this.order.dateCreated = this.addOrderForm.controls['information'].value.orderCreatedDate;
    this.order.orderCareStaffId = this.addOrderForm.controls['information'].value.orderCareStaff;
    this.order.customerCareStaffId = this.addOrderForm.controls['information'].value.customerCareStaff;

    this.customer.name = this.addOrderForm.controls['customerInfo'].value.customerName;
    this.customer.phoneNumber = this.addOrderForm.controls['customerInfo'].value.customerPhoneNumber;
    this.customer.emailAddress = this.addOrderForm.controls['customerInfo'].value.customerEmailAddress;
    this.customer.dob = this.addOrderForm.controls['customerInfo'].value.customerDOB;
    this.order.customer = this.customer;

    this.order.skus = this.listSkus;
  }

  handleSelectEvent(productSKUDetails: ProductSKUDetails[]){
    this.listSkus = productSKUDetails;
    this.totalSKUsPrice = 0;

    this.listSkus.forEach(sku => {
      this.totalSKUsPrice += sku.price;
    });
  }
}
