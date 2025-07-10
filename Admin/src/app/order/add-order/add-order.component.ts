import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Customer } from 'src/app/shared/_models/customer';
import { ProductSKUDetails } from 'src/app/shared/_models/productSKUDetails';
import { OrderService } from '../order.service';
import { OrderSKUItems } from 'src/app/shared/_models/orderSKUItems';
import { ToastrService } from 'ngx-toastr';
import { CheckoutComponent } from './order-information/checkout/checkout.component';
import { Province } from 'src/app/shared/_models/address/province';
import { DatePipe } from '@angular/common';
import { OrderItem, WebsiteOrder } from 'src/app/shared/_models/website-order';

@Component({
  selector: 'app-add-order',
  templateUrl: './add-order.component.html',
  styleUrls: ['./add-order.component.css']
})
export class AddOrderComponent implements OnInit {
  addOrderForm: FormGroup;
  @ViewChild(CheckoutComponent) checkoutComponent:CheckoutComponent;

  listSkus: ProductSKUDetails[] = [];
  totalSKUsPrice = 0;
  order: WebsiteOrder = new WebsiteOrder();
  customer: Customer = new Customer();
  skuItems: OrderSKUItems[] = [];
  provinces: Province[] = [];

  constructor(private formBuilder: FormBuilder, private orderService: OrderService, private toastrService: ToastrService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.addOrderForm = this.formBuilder.group({
      checkout: this.formBuilder.group({
        freeshipChecked: this.formBuilder.control(false),
        shippingFee: this.formBuilder.control(0, [Validators.required]),
        orderDiscount: this.formBuilder.control(0, [Validators.required]),
        bankTransferedAmount: this.formBuilder.control(0, [Validators.required]),
        extraFee: this.formBuilder.control(0, [Validators.required]),
        orderNote: this.formBuilder.control('')
      }),
      information: this.formBuilder.group({
        orderCreatedDate: this.formBuilder.control('', [Validators.required]),
        orderCareStaff: this.formBuilder.control(1, [Validators.required]),
        customerCareStaff: this.formBuilder.control(2, [Validators.required])
      }),
      customerInfo: this.formBuilder.group({
        customerName: this.formBuilder.control('', [Validators.required]),
        customerPhoneNumber: this.formBuilder.control('', [Validators.required]),
        customerEmailAddress: this.formBuilder.control('', [Validators.required]),
        customerDOB: this.formBuilder.control('')
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
        deliveryCompanyId: this.formBuilder.control(''),
        shipmentCode: this.formBuilder.control({value: '', disabled: true}),
        shipmentCost: this.formBuilder.control({value: '', disabled: true}),
      })
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
    this.order.buyerEmail = this.addOrderForm.controls['customerInfo'].value.customerEmailAddress;
    this.order.fullname = this.addOrderForm.controls['customerInfo'].value.customerName;
    this.order.phoneNumber = this.addOrderForm.controls['customerInfo'].value.customerPhoneNumber;

    this.order.street = this.addOrderForm.controls['receiverInfo'].value.receiverAddress;
    this.order.cityOrProvinceId = this.addOrderForm.controls['receiverInfo'].value.provinceId;
    this.order.districtId = this.addOrderForm.controls['receiverInfo'].value.districtId;
    this.order.wardId = this.addOrderForm.controls['receiverInfo'].value.wardId;
    // this.order.name = this.addOrderForm.controls['receiverInfo'].value.receiverName;
    // this.order.receiverPhoneNumber = this.addOrderForm.controls['receiverInfo'].value.receiverPhoneNumber;

    this.order.shippingFee = this.addOrderForm.controls['checkout'].value.shippingFee;
    this.order.orderDiscount = this.addOrderForm.controls['checkout'].value.orderDiscount;
    this.order.bankTransferedAmount = this.addOrderForm.controls['checkout'].value.bankTransferedAmount;
    this.order.extraFee = this.addOrderForm.controls['checkout'].value.extraFee;
    this.order.total = this.totalSKUsPrice + this.order.shippingFee + this.order.extraFee - this.order.orderDiscount - this.order.bankTransferedAmount;
    this.order.orderNote = this.addOrderForm.controls['checkout'].value.orderNote;

    this.order.orderDate = this.addOrderForm.controls['information'].value.orderCreatedDate;
    // this.order.orderCareStaffId = this.addOrderForm.controls['information'].value.orderCareStaff;
    // this.order.orderCareStaffId = 1;
    // this.order.customerCareStaffId = this.addOrderForm.controls['information'].value.customerCareStaff;
    // this.order.customerCareStaffId = 1;

    // this.order.customer = this.customer;

    this.order.orderItems = this.groupSkuItems();

    this.orderService.createOrderFromAdmin(this.order).subscribe({
      next: () => {
        this.toastrService.success('Tạo mới đơn hàng thành công')
        this.addOrderForm.reset();
        this.addOrderForm.controls['checkout'].patchValue({
          shippingFee: 0,
          orderDiscount: 0,
          bankTransferedAmount: 0,
          extraFee: 0
        });
        this.addOrderForm.controls['information'].patchValue({
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

  groupSkuItems(): OrderItem[] {
    var skuItems: OrderItem[] = [];
    // var skuItems: OrderSKUItems[] = [];

    return this.listSkus.reduce((skuItems, currentSku) => {
      if(skuItems == null || skuItems.find(item => item.skuId === currentSku.id) === undefined) {
        var item: OrderItem = {
          skuId: currentSku.id, 
          quantity: 1, 
          skuDetail: null, 
          productName: currentSku.productName, 
          price: currentSku.price, 
          pictureUrl: currentSku.imageUrl, 
          sku: currentSku.sku, 
          optionValueCombination: currentSku.productSKUValues.map(x => x.optionValue).join(', '), 
          productId: 0, 
          itemOrdered: null, 
          id: 0
        }

        skuItems.push(item);
      } else {
        skuItems.find(x => x.skuId === currentSku.id).quantity++;
      }

      return skuItems;
    }, skuItems);
  }

  handleSelectEvent(productSKUDetails: ProductSKUDetails[]){
    this.listSkus = productSKUDetails;
    this.totalSKUsPrice = 0;

    this.listSkus.forEach(sku => {
      this.totalSKUsPrice += sku.price;
    });
  }
}
