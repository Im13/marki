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

export interface ShipToAddress {
  cityOrProvinceId: number;
  districtId: number;
  fullname: string;
  phoneNumber: string;
  street: string;
  wardId: number;
}

export interface WebsiteOrder {
  id: number;
  buyerEmail: string;
  deliveryMethod: number;
  orderDate: Date;
  shippingFee: number;
  status: string;
  subtotal: number;
  orderItems: OrderItem[];
  // shipToAddress: ShipToAddress;
  orderStatus: OrderStatus;
  bankTransferedAmount: number;
  orderDiscount: number;
  extraFee: number;
  total: number;
  orderNote: string;
  cityOrProvinceId: number;
  districtId: number;
  fullname: string;
  phoneNumber: string;
  street: string;
  wardId: number;
  customer: Customer;
}
