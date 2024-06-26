import { ShopeeProduct } from "./shopeeProduct";

export class ShopeeOrder {
    orderId: string;
    orderDate: string;
    orderStatus: string;
    shipmentCode: string;
    shippingCompany: string;
    returnStatus: string;
    totalOrderValue: number;
    shopVoucher: number;
    shopeeCoinReturn: number;
    shopeeVoucher: number;
    shopeeComboDiscount: number;
    shopComboDiscount: number;
    estimatedShippingFee: number;
    customerShippingFee: number;
    estimatedShoppingFeeShopeeDiscount: number;
    returnOrderFee: number;
    totalOrderCustomerPaid: number;
    orderCompletedDate: string;
    orderPaidDate: string;
    paymentMethod: string;
    fixedFee: number;
    serviceFee: number;
    paymentFee: number;
    deposit: number;
    customerUsername: string;
    customerName: string;
    phoneNumber: string;
    province: string;
    district: string;
    ward: string;
    addressDetails: string;
    note: string;
    products: ShopeeProduct[];
}
