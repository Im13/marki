import { Customer } from "./cutomer";
import { ProductSKUDetails } from "./productSKUDetails";

export class Order {
  shippingFee: number;
  orderDiscount: number;
  bankTransferedAmount: number;
  extraFee: number;
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
  offlineOrderSKUs: ProductSKUDetails[];
}
