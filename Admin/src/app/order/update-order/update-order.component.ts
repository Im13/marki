import { Component, inject, Input, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Customer } from 'src/app/shared/models/cutomer';
import { Order } from 'src/app/shared/models/order';
import { OrderSKUItems } from 'src/app/shared/models/orderSKUItems';
import { ProductSKUDetails } from 'src/app/shared/models/productSKUDetails';
import { OrderService } from '../order.service';
import { ToastrService } from 'ngx-toastr';
import { NZ_MODAL_DATA } from 'ng-zorro-antd/modal';

@Component({
  selector: 'app-update-order',
  templateUrl: './update-order.component.html',
  styleUrls: ['./update-order.component.css']
})
export class UpdateOrderComponent implements OnInit {
  @Input() order?: Order = inject(NZ_MODAL_DATA);

  updateOrderForm: FormGroup;
  listSkus: ProductSKUDetails[] = [];
  totalSKUsPrice = 0;
  customer: Customer = new Customer();
  skuItems: OrderSKUItems[] = [];

  constructor(private formBuilder: FormBuilder, private orderService: OrderService, private toastrService: ToastrService){}

  ngOnInit(): void {
    console.log(this.order);

    this.order.offlineOrderSKUs.forEach(item => {
      this.listSkus.push(item.skuDetail);
      this.totalSKUsPrice += item.skuDetail.price;
    });

    this.updateOrderForm = this.formBuilder.group({
      checkout: this.formBuilder.group({
        freeshipChecked: this.formBuilder.control(false),
        shippingFee: this.formBuilder.control(this.order.shippingFee, [Validators.required]),
        orderDiscount: this.formBuilder.control(this.order.orderDiscount, [Validators.required]),
        bankTransferedAmount: this.formBuilder.control(this.order.bankTransferedAmount, [Validators.required]),
        extraFee: this.formBuilder.control(this.order.extraFee, [Validators.required]),
        orderNote: this.formBuilder.control(this.order.orderNote)
      }),
      information: this.formBuilder.group({
        orderCreatedDate: this.formBuilder.control(this.order.dateCreated, [Validators.required]),
        orderCareStaff: this.formBuilder.control(this.order.orderCareStaffId, [Validators.required]),
        customerCareStaff: this.formBuilder.control(this.order.customerCareStaffId, [Validators.required])
      }),
      customerInfo: this.formBuilder.group({
        customerName: this.formBuilder.control(this.order.customer.name, [Validators.required]),
        customerPhoneNumber: this.formBuilder.control(this.order.customer.phoneNumber, [Validators.required]),
        customerEmailAddress: this.formBuilder.control(this.order.customer.emailAddress, [Validators.required]),
        customerDOB: this.formBuilder.control(this.order.customer.dob, [Validators.required])
      }),
      receiverInfo: this.formBuilder.group({
        receiverName: this.formBuilder.control(this.order.receiverName, [Validators.required]),
        receiverPhoneNumber: this.formBuilder.control(this.order.receiverPhoneNumber, [Validators.required]),
        receiverAddress: this.formBuilder.control(this.order.address, [Validators.required]),
        provinceId: this.formBuilder.control(this.order.provinceId, [Validators.required]),
        districtId: this.formBuilder.control(this.order.districtId, [Validators.required]),
        wardId: this.formBuilder.control(this.order.wardId, [Validators.required])
      }),
      deliveryService: this.formBuilder.group({
        deliveryCompanyId: this.formBuilder.control('', [Validators.required]),
        shipmentCode: this.formBuilder.control({value: '', disabled: true}, [Validators.required]),
        shipmentCost: this.formBuilder.control({value: '', disabled: true}, [Validators.required]),
      })
    });
  }

  submitForm() {
    console.log('clicked')
    this.order.address = this.updateOrderForm.controls['receiverInfo'].value.receiverAddress;
    this.order.provinceId = this.updateOrderForm.controls['receiverInfo'].value.provinceId;
    this.order.districtId = this.updateOrderForm.controls['receiverInfo'].value.districtId;
    this.order.wardId = this.updateOrderForm.controls['receiverInfo'].value.wardId;
    this.order.receiverName = this.updateOrderForm.controls['receiverInfo'].value.receiverName;
    this.order.receiverPhoneNumber = this.updateOrderForm.controls['receiverInfo'].value.receiverPhoneNumber;

    this.order.shippingFee = this.updateOrderForm.controls['checkout'].value.shippingFee;
    this.order.orderDiscount = this.updateOrderForm.controls['checkout'].value.orderDiscount;
    this.order.bankTransferedAmount = this.updateOrderForm.controls['checkout'].value.bankTransferedAmount;
    this.order.extraFee = this.updateOrderForm.controls['checkout'].value.extraFee;
    this.order.orderNote = this.updateOrderForm.controls['checkout'].value.orderNote;

    this.order.dateCreated = this.updateOrderForm.controls['information'].value.orderCreatedDate;
    // this.order.orderCareStaffId = this.updateOrderForm.controls['information'].value.orderCareStaff;
    this.order.orderCareStaffId = 1;
    // this.order.customerCareStaffId = this.updateOrderForm.controls['information'].value.customerCareStaff;
    this.order.customerCareStaffId = 1;

    this.customer.name = this.updateOrderForm.controls['customerInfo'].value.customerName;
    this.customer.phoneNumber = this.updateOrderForm.controls['customerInfo'].value.customerPhoneNumber;
    this.customer.emailAddress = this.updateOrderForm.controls['customerInfo'].value.customerEmailAddress;
    this.customer.dob = this.updateOrderForm.controls['customerInfo'].value.customerDOB;
    this.order.customer = this.customer;

    this.order.offlineOrderSKUs = this.groupSkuItems();

    console.log(this.order.offlineOrderSKUs);

    this.orderService.updateOrder(this.order).subscribe({
      next: () => {
        this.toastrService.success('Cập nhật đơn hàng thành công')
        this.updateOrderForm.reset();
        this.updateOrderForm.controls['checkout'].patchValue({
          shippingFee: 0,
          orderDiscount: 0,
          bankTransferedAmount: 0,
          extraFee: 0
        });
        this.updateOrderForm.controls['information'].patchValue({
          orderCreatedDate: new Date()
        })
        this.totalSKUsPrice = 0;
        this.skuItems = [];
        this.listSkus = [];
      },
      error: err => {
        console.log(err);
      }
    });
  }

  handleSelectEvent(productSKUDetails: ProductSKUDetails[]){
    this.listSkus = productSKUDetails;
    this.totalSKUsPrice = 0;

    this.listSkus.forEach(sku => {
      this.totalSKUsPrice += sku.price;
    });
  }

  groupSkuItems(): OrderSKUItems[] {
    var skuItems: OrderSKUItems[] = [];

    return this.listSkus.reduce((skuItems, currentSku) => {
      if(skuItems == null || skuItems.find(item => item.productSKUId === currentSku.id) === undefined) {
        skuItems.push({productSKUId: currentSku.id, quantity: 1, skuDetail: null});
      } else {
        skuItems.find(x => x.productSKUId === currentSku.id).quantity++;
      }

      return skuItems;
    }, skuItems);
  }
}
