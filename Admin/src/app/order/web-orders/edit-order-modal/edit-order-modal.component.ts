import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { OrderItem, WebsiteOrder } from 'src/app/shared/_models/website-order';
import { OrderService } from '../../order.service';

@Component({
  selector: 'app-edit-order-modal',
  templateUrl: './edit-order-modal.component.html',
  styleUrls: ['./edit-order-modal.component.css'],
})
export class EditOrderModalComponent implements OnInit {
  @Input() order?: WebsiteOrder = inject(NZ_MODAL_DATA);
  // order: WebsiteOrder;
  editOrderForm: FormGroup;
  listItems: OrderItem[];

  constructor(private formBuilder: FormBuilder, private orderService: OrderService) {}

  ngOnInit(): void {
    // Init form
    this.editOrderForm = this.formBuilder.group({
      checkout: this.formBuilder.group({
        freeshipChecked: this.formBuilder.control(false),
        shippingFee: this.formBuilder.control(this.order?.shippingPrice, [
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
        customerName: this.formBuilder.control(this.order?.shipToAddress.fullname, [
          Validators.required,
        ]),
        customerPhoneNumber: this.formBuilder.control(
          this.order?.shipToAddress.phoneNumber,
          [Validators.required]
        ),
        customerEmailAddress: this.formBuilder.control(
          this.order?.buyerEmail
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

    this.orderService.getWebsiteOrderById(this.order.id).subscribe({
      next: data => {
        this.order = data;
        console.log(this.order);

        // Update form data
        this.editOrderForm.patchValue({
          checkout: {
            shippingFee: this.order.shippingPrice,
            orderDiscount: '',
            bankTransferedAmount: '',
            extraFee: '',
            orderNote: '',
          },
          information: {
            orderCareStaff: 'this.order.orderCareStaffId',
            customerCareStaff: 'this.order.customerCareStaffId',
          },
          customerInfo: {
            customerName: this.order.shipToAddress.fullname,
            customerPhoneNumber: this.order.shipToAddress.phoneNumber,
            customerEmailAddress: this.order.buyerEmail,
          },
          receiverInfo: {
            receiverName: this.order.shipToAddress.fullname,
            receiverPhoneNumber: this.order.shipToAddress.phoneNumber,
            receiverAddress: this.order.shipToAddress.street,
            provinceId: this.order.shipToAddress.cityOrProvinceId,
            districtId: this.order.shipToAddress.districtId,
            wardId: this.order.shipToAddress.wardId,
          }
        })

        //Update list itesms
        this.listItems = this.order.orderItems;
        console.log('itesm: ' + this.listItems.length)
      },
      error: err => console.log(err)
    })
  }
}
