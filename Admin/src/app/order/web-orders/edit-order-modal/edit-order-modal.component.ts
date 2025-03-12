import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NZ_MODAL_DATA, NzModalRef } from 'ng-zorro-antd/modal';
import { OrderItem, WebsiteOrder } from 'src/app/shared/_models/website-order';
import { OrderService } from '../../order.service';
import { Province } from 'src/app/shared/_models/address/province';
import { OrderStatus } from 'src/app/shared/_models/orderStatus';
import { ToastrService } from 'ngx-toastr';

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
  total = 0;

  orderStatuses: OrderStatus[] = [
    { id: 1, status: 'Mới' },
    { id: 2, status: 'Chờ hàng' },
    { id: 3, status: 'Ưu tiên xuất đơn' },
    { id: 4, status: 'Đã xác nhận' },
    { id: 5, status: 'Gửi hàng đi' },
    { id: 6, status: 'Huỷ đơn' },
    { id: 7, status: 'Xoá đơn' },
  ];

  constructor(private formBuilder: FormBuilder, private orderService: OrderService, private modal: NzModalRef, private toastrService: ToastrService) { }

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
        customerName: this.formBuilder.control(this.order?.fullname, [ Validators.required ]),
        customerPhoneNumber: this.formBuilder.control(this.order?.phoneNumber, [ Validators.required ]),
        customerEmailAddress: this.formBuilder.control(this.order?.buyerEmail),
        customerDOB: this.formBuilder.control(new Date()),
      }),
      receiverInfo: this.formBuilder.group({
        receiverName: this.formBuilder.control(this.order.fullname, [ Validators.required ]),
        receiverPhoneNumber: this.formBuilder.control(this.order.phoneNumber, [ Validators.required ]),
        receiverAddress: this.formBuilder.control(this.order.street, [ Validators.required ]),
        provinceId: this.formBuilder.control(this.order.cityOrProvinceId, [ Validators.required ]),
        districtId: this.formBuilder.control(this.order.districtId, [ Validators.required ]),
        wardId: this.formBuilder.control(this.order.wardId, [ Validators.required ]),
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
            orderDiscount: this.order.orderDiscount,
            bankTransferedAmount: this.order.bankTransferedAmount,
            extraFee: this.order.extraFee,
            orderNote: this.order.orderNote,
          },
          information: {
            orderCareStaff: 'this.order.orderCareStaffId',
            customerCareStaff: 'this.order.customerCareStaffId',
          },
          customerInfo: {
            customerName: this.order.fullname,
            customerPhoneNumber: this.order.phoneNumber,
            customerEmailAddress: this.order.buyerEmail,
          },
          receiverInfo: {
            receiverName: this.order.fullname,
            receiverPhoneNumber: this.order.phoneNumber,
            receiverAddress: this.order.street,
            provinceId: this.order.cityOrProvinceId,
            districtId: this.order.districtId,
            wardId: this.order.wardId,
          }
        })

        //Update list itesms
        this.listItems = this.order.orderItems;

        this.subTotal = this.order.subtotal;
        // this.total = this.order.total;
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
    this.order.buyerEmail = this.editOrderForm.controls['customerInfo'].value.customerEmailAddress;
    this.order.cityOrProvinceId = this.editOrderForm.controls['receiverInfo'].value.provinceId;
    this.order.districtId = this.editOrderForm.controls['receiverInfo'].value.districtId;
    this.order.wardId = this.editOrderForm.controls['receiverInfo'].value.wardId;
    this.order.fullname = this.editOrderForm.controls['receiverInfo'].value.receiverName;
    this.order.phoneNumber = this.editOrderForm.controls['receiverInfo'].value.receiverPhoneNumber;
    this.order.street = this.editOrderForm.controls['receiverInfo'].value.receiverAddress;

    this.order.shippingFee = +this.editOrderForm.controls['checkout'].value.shippingFee;
    this.order.orderDiscount = +this.editOrderForm.controls['checkout'].value.orderDiscount;
    this.order.bankTransferedAmount = +this.editOrderForm.controls['checkout'].value.bankTransferedAmount;
    this.order.extraFee = +this.editOrderForm.controls['checkout'].value.extraFee;
    this.order.total = this.subTotal + this.order.shippingFee + this.order.extraFee - this.order.orderDiscount - this.order.bankTransferedAmount;
    this.order.orderNote = this.editOrderForm.controls['checkout'].value.orderNote;
    this.order.orderItems = this.listItems;
    this.order.subtotal = this.subTotal;

    this.orderService.updateWebsiteOrder(this.order).subscribe({
      next: () => {
        this.toastrService.success('Cập nhật đơn hàng thành công')
        this.modal.destroy();
      },
      error: err => {
        console.log(err);
      }
    });
  }

  handleSelectItemEvent(items: OrderItem[]) {
    this.listItems = items;
    this.subTotal = 0;

    this.listItems.forEach(item => {
      this.subTotal += item.price * item.quantity;
    });
  }
}
