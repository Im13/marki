import { Customer } from "./customer";
import { OrderSKUItems } from "./orderSKUItems";
import { OrderStatus } from "./orderStatus";

export class Order {
  id: number;
  shippingFee: number;
  orderDiscount: number;
  bankTransferedAmount: number;
  extraFee: number;
  total: number;
  orderNote: string;
  dateCreated: Date;
  orderCareStaffId: number;
  customerCareStaffId: number;
  customer: Customer;
  receiverName: string;
  receiverPhoneNumber: string;
  address: string;
  districtId: number;
  provinceId: number;
  wardId: number;
  offlineOrderSKUs: OrderSKUItems[];
  statusId: number;
  orderStatus: OrderStatus;
}
