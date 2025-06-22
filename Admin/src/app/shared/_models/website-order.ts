import { Customer } from "./customer";
import { OrderStatus } from "./orderStatus";

export interface OrderItem {
  id: number;
  productName: string;
  price: number;
  quantity: number;
  pictureUrl: string;
  sku: string;
  optionValueCombination: string;
  productId: number;
  itemOrdered: ProductItemOrdered;
}

export interface ProductItemOrdered {
  productItemId: number;
  productName: string;
  pictureUrl: string;
}

export interface Address {
  fullname: string;
  cityOrProvinceId: number;
  districtId: number;
  wardId: number;
  street: string;
  phoneNumber: string;
}

export class WebsiteOrder {
  id: number;
  buyerEmail: string;
  orderDate: Date;
  // shipToAddress: Address;
  shippingFee: number;
  orderDiscount: number;
  bankTransferedAmount: number;
  extraFee: number;
  status: string;
  total: number;
  orderNote: string;
  subtotal: number;
  fullname: string;
  cityOrProvinceId: number;
  districtId: number;
  wardId: number;
  street: string;
  phoneNumber: string;
  orderItems: OrderItem[];
  orderStatus: OrderStatus;
  // customer: Customer;
}
