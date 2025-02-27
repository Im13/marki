import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';
import { OrderItem, WebsiteOrder } from 'src/app/shared/_models/website-order';
import { OrderService } from '../../order.service';
import { Province } from 'src/app/shared/_models/address/province';
import { OrderStatus } from 'src/app/shared/_models/orderStatus';

@Component({
  selector: 'app-edit-order-modal',
  templateUrl: './edit-order-modal.component.html',
  styleUrls: ['./edit-order-modal.component.css'],
})
export class EditOrderModalComponent implements OnInit {
  @Input() order?: WebsiteOrder = inject(NZ_MODAL_DATA);
  editOrderForm: FormGroup;
  listItems: OrderItem[];
  provinces: Province[] = [];
  subTotal = 0;

  orderStatuses: OrderStatus[] = [
    { id: 1, status: 'Mới' },
    { id: 2, status: 'Chờ hàng' },
    { id: 3, status: 'Ưu tiên xuất đơn' },
    { id: 4, status: 'Đã xác nhận' },
    { id: 5, status: 'Gửi hàng đi' },
    { id: 6, status: 'Huỷ đơn' },
    { id: 7, status: 'Xoá đơn' },
  ];

  constructor(private formBuilder: FormBuilder, private orderService: OrderService) { }

  ngOnInit(): void {
    // Init form
    this.editOrderForm = this.formBuilder.group({
      checkout: this.formBuilder.group({
        freeshipChecked: this.formBuilder.control(false),
        shippingFee: this.formBuilder.control(this.order.shippingFee, [ Validators.required ]),
        orderDiscount: this.formBuilder.control(0, [ Validators.required ]),
        bankTransferedAmount: this.formBuilder.control(0, [ Validators.required ]),
        extraFee: this.formBuilder.control(0, [ Validators.required ]),
        orderNote: this.formBuilder.control(''),
      }),
      information: this.formBuilder.group({
        orderCreatedDate: this.formBuilder.control(new Date(), [ Validators.required ]),
        orderCareStaff: this.formBuilder.control('this.order.orderCareStaffId'),
        customerCareStaff: this.formBuilder.control('this.order.customerCareStaffId'),
      }),
      customerInfo: this.formBuilder.group({
        customerName: this.formBuilder.control(this.order?.shipToAddress.fullname, [ Validators.required ]),
        customerPhoneNumber: this.formBuilder.control(this.order?.shipToAddress.phoneNumber, [ Validators.required ]),
        customerEmailAddress: this.formBuilder.control(this.order?.buyerEmail),
        customerDOB: this.formBuilder.control(new Date()),
      }),
      receiverInfo: this.formBuilder.group({
        receiverName: this.formBuilder.control(this.order.shipToAddress.fullname, [ Validators.required ]),
        receiverPhoneNumber: this.formBuilder.control(this.order.shipToAddress.phoneNumber, [ Validators.required ]),
        receiverAddress: this.formBuilder.control(this.order.shipToAddress.street, [ Validators.required ]),
        provinceId: this.formBuilder.control(this.order.shipToAddress.cityOrProvinceId, [ Validators.required ]),
        districtId: this.formBuilder.control(this.order.shipToAddress.districtId, [ Validators.required ]),
        wardId: this.formBuilder.control(this.order.shipToAddress.wardId, [ Validators.required ]),
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
            shippingFee: this.order.shippingFee,
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

        this.subTotal = this.order.subtotal;
      },
      error: err => console.log(err)
    });

    this.orderService.getProvinces().subscribe({
      next: result => {
        this.provinces = result;
      },
      error: err => {
        console.log(err);
      }
    });
  }

  submitForm() {
    console.log('submit!');
  }
}
