import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { WebsiteOrder } from 'src/app/shared/_models/website-order';

@Component({
  selector: 'app-edit-order-modal',
  templateUrl: './edit-order-modal.component.html',
  styleUrls: ['./edit-order-modal.component.css'],
})
export class EditOrderModalComponent implements OnInit {
  @Input() order?: WebsiteOrder = inject(NZ_MODAL_DATA);
  editOrderForm: FormGroup;

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit(): void {
    this.editOrderForm = this.formBuilder.group({
      checkout: this.formBuilder.group({
        freeshipChecked: this.formBuilder.control(false),
        shippingFee: this.formBuilder.control(this.order.shippingPrice, [
          Validators.required,
        ]),
        orderDiscount: this.formBuilder.control('this.order.orderDiscount', [
          Validators.required,
        ]),
        bankTransferedAmount: this.formBuilder.control(
          'this.order.bankTransferedAmount',
          [Validators.required]
        ),
        extraFee: this.formBuilder.control('this.order.extraFee', [
          Validators.required,
        ]),
        orderNote: this.formBuilder.control('this.order.orderNote'),
      }),
      information: this.formBuilder.group({
        orderCreatedDate: this.formBuilder.control(new Date(), [
          Validators.required,
        ]),
        orderCareStaff: this.formBuilder.control('this.order.orderCareStaffId'),
        customerCareStaff: this.formBuilder.control(
          'this.order.customerCareStaffId'
        ),
      }),
      customerInfo: this.formBuilder.group({
        customerName: this.formBuilder.control('this.order.customer.name', [
          Validators.required,
        ]),
        customerPhoneNumber: this.formBuilder.control(
          'this.order.customer.phoneNumber',
          [Validators.required]
        ),
        customerEmailAddress: this.formBuilder.control(
          this.order.buyerEmail
        ),
        customerDOB: this.formBuilder.control(new Date()),
      }),
      receiverInfo: this.formBuilder.group({
        receiverName: this.formBuilder.control('this.order.receiverName', [
          Validators.required,
        ]),
        receiverPhoneNumber: this.formBuilder.control(
          'this.order.receiverPhoneNumber',
          [Validators.required]
        ),
        receiverAddress: this.formBuilder.control('this.order.address', [
          Validators.required,
        ]),
        provinceId: this.formBuilder.control('this.order.provinceId', [
          Validators.required,
        ]),
        districtId: this.formBuilder.control('this.order.districtId', [
          Validators.required,
        ]),
        wardId: this.formBuilder.control('this.order.wardId', [
          Validators.required,
        ]),
      }),
      deliveryService: this.formBuilder.group({
        deliveryCompanyId: this.formBuilder.control(''),
        shipmentCode: this.formBuilder.control({ value: '', disabled: true }),
        shipmentCost: this.formBuilder.control({ value: '', disabled: true }),
      }),
    });
  }
}
