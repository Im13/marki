import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { DatePipe } from '@angular/common';
import { environment } from 'src/environments/environment';
import { read, utils } from "xlsx";
import * as XLSX from 'xlsx';
import * as OrderConstants from './order.constants';
import { ShopeeOrder } from '../shared/_models/shopeeOrder';
import { ShopeeProduct } from '../shared/_models/shopeeProduct';
import { ShopeeOrderParams } from '../shared/_models/shopeeOrderParams';
import { Pagination } from '../shared/_models/pagination';
import { Observable, Subject } from 'rxjs';
import { Order } from '../shared/_models/order';
import { OrderParams } from '../shared/_models/order/orderParams';
import { Province } from '../shared/_models/address/province';
import { District } from '../shared/_models/address/district';
import { Ward } from '../shared/_models/address/ward';
import { UpdateStatusDTO } from '../shared/_models/order/updateStatusDTO';
import { OrderWithStatusParams } from '../shared/_models/order/orderWithStatusParams';
import { ProductSKU } from '../shared/_models/order/productSKU';
import { Product } from '../shared/_models/products';
import { BasketItem } from '../shared/_models/order/basketItem';
import { WebsiteOrder } from '../shared/_models/website-order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  baseApiUrl = environment.apiUrl;
  addedOrders = new Subject<ShopeeOrder[]>();

  constructor(private http: HttpClient, private datePipe: DatePipe) { }

  uploadShopeeOrdersFile(orders: ShopeeOrder[]) {
    return this.http.post(this.baseApiUrl + 'shopee/create-orders', orders);
  }

  getShopeeOrdersPagination(shopeeOrderParams: ShopeeOrderParams) {
    let params = new HttpParams();

    params = params.append('pageSize', shopeeOrderParams.pageSize);
    params = params.append('pageIndex', shopeeOrderParams.pageIndex);

    if(shopeeOrderParams.date) {
      shopeeOrderParams.date = this.datePipe.transform(shopeeOrderParams.date, "dd/MM/yyyy");

      params = params.append('date', shopeeOrderParams.date);
    }

    return this.http.get<Pagination<ShopeeOrder[]>>(this.baseApiUrl + 'shopee/get-orders', { params });
  }

  readExcelFile(file: File) {
    const fileReader = new FileReader();
    let data: any;
    let orders: ShopeeOrder[] = [];

    fileReader.onload = (e: any) => {
      /* read workbook */
      const ab: ArrayBuffer = e.target.result;
      const wb: XLSX.WorkBook = read(ab);

      /* grab first sheet */
      const wsName: string = wb.SheetNames[0];
      const ws: XLSX.WorkSheet = wb.Sheets[wsName];

      /* get data from excel file */
      data = utils.sheet_to_json(ws, {header: 1});

      this.parseDataToObject(data, orders);

      this.uploadShopeeOrdersFile(orders).subscribe({
        next: (addedOrders: ShopeeOrder[]) => {
          this.addedOrders.next(addedOrders);
        },
        error: (e) => {
          this.addedOrders.next(null);;
        }
      });
    }

    fileReader.readAsArrayBuffer(file);

    return orders;
  }

  parseDataToObject(data: any[][], orders: ShopeeOrder[]) {
    var headerArray = data[0];

    const columnDict: { [ColumnName: string] : number } = {};

    headerArray.forEach((value, index) => {
      columnDict[value] = +index;
    })

    for(let i = 1; i < data.length; i++) {
      this.getOrderFromRow(data[i], columnDict, orders);
    }
  }

  getOrderFromRow(orderTableRow: any[], columnDict: { [ColumnName: string] : number }, orders: ShopeeOrder[]) {
    var orderId = orderTableRow[columnDict[OrderConstants.OrderConstants.ORDER_ID]];

    var product: ShopeeProduct = {
      productName: orderTableRow[columnDict[OrderConstants.OrderConstants.PRODUCT_NAME]],
      cost: orderTableRow[columnDict[OrderConstants.OrderConstants.COST]],
      productProperty: orderTableRow[columnDict[OrderConstants.OrderConstants.PRODUCT_PROPERTY]],
      productPropertySKU: orderTableRow[columnDict[OrderConstants.OrderConstants.PRODUCT_PROPERTY_SKU]],
      productSKU: orderTableRow[columnDict[OrderConstants.OrderConstants.SKU]],
      quantity: orderTableRow[columnDict[OrderConstants.OrderConstants.QUANTITY]],
      returnedQuantity: orderTableRow[columnDict[OrderConstants.OrderConstants.RETURNED_QUANTITY]]?.toString(),
      salePrice: orderTableRow[columnDict[OrderConstants.OrderConstants.SALE_PRICE]],
      shopDiscount: orderTableRow[columnDict[OrderConstants.OrderConstants.SHOP_DISCOUNT]],
      shopeeSale: orderTableRow[columnDict[OrderConstants.OrderConstants.SHOPEE_SALE]],
      totalSellingPrice: orderTableRow[columnDict[OrderConstants.OrderConstants.TOTAL_SELLING_PRICE]],
      totalShopSale: orderTableRow[columnDict[OrderConstants.OrderConstants.TOTAL_SHOP_SALE]]
    };

    var order = orders.find(o => o.orderId === orderId);

    if(order) {
      order.products.push(product);
    } else {
      var shopeeOrder: ShopeeOrder = {
        addressDetails: orderTableRow[columnDict[OrderConstants.OrderConstants.ADDRESS_DETAIL]],
        customerName: orderTableRow[columnDict[OrderConstants.OrderConstants.CUSTOMER_NAME]],
        customerShippingFee: orderTableRow[columnDict[OrderConstants.OrderConstants.CUSTOMER_SHIPPING_FEE]],
        customerUsername: orderTableRow[columnDict[OrderConstants.OrderConstants.CUSTOMER_USERNAME]],
        deposit: orderTableRow[columnDict[OrderConstants.OrderConstants.DEPOSIT]],
        district: orderTableRow[columnDict[OrderConstants.OrderConstants.DISTRICT]],
        estimatedShippingFee: orderTableRow[columnDict[OrderConstants.OrderConstants.ESTIMATED_SHIPPING_FEE]],
        estimatedShoppingFeeShopeeDiscount: orderTableRow[columnDict[OrderConstants.OrderConstants.ESTIMATED_SHOPEE_DISCOUNT_SHIPPING_FEE]],
        fixedFee: orderTableRow[columnDict[OrderConstants.OrderConstants.FIXED_FEE]],
        note: orderTableRow[columnDict[OrderConstants.OrderConstants.NOTE]],
        orderCompletedDate: orderTableRow[columnDict[OrderConstants.OrderConstants.ORDER_COMPLETED_DATE]],
        orderDate: this.datePipe.transform(String(orderTableRow[columnDict[OrderConstants.OrderConstants.ORDER_DATE]]), 'dd/MM/yyyy H:mm') ,
        orderId: orderTableRow[columnDict[OrderConstants.OrderConstants.ORDER_ID]],
        orderPaidDate: orderTableRow[columnDict[OrderConstants.OrderConstants.ORDER_PAID_DATE]],
        orderStatus: orderTableRow[columnDict[OrderConstants.OrderConstants.ORDER_STATUS]],
        paymentFee: orderTableRow[columnDict[OrderConstants.OrderConstants.PAYMENT_FEE]],
        paymentMethod: orderTableRow[columnDict[OrderConstants.OrderConstants.PAYMENT_METHOD]],
        phoneNumber: orderTableRow[columnDict[OrderConstants.OrderConstants.PHONE_NUMBER]],
        province: orderTableRow[columnDict[OrderConstants.OrderConstants.PROVINCE]],
        returnOrderFee: orderTableRow[columnDict[OrderConstants.OrderConstants.RETURN_ORDER_FEE]],
        returnStatus: orderTableRow[columnDict[OrderConstants.OrderConstants.RETURN_STATUS]],
        serviceFee: orderTableRow[columnDict[OrderConstants.OrderConstants.SERVICE_FEE]],
        shipmentCode: orderTableRow[columnDict[OrderConstants.OrderConstants.SHIPMENT_CODE]],
        shippingCompany: orderTableRow[columnDict[OrderConstants.OrderConstants.SHIPPING_COMPANY]],
        shopComboDiscount: orderTableRow[columnDict[OrderConstants.OrderConstants.SHOP_COMBO_DISCOUNT]],
        shopeeCoinReturn: orderTableRow[columnDict[OrderConstants.OrderConstants.SHOPEE_COIN_RETURN]],
        shopeeComboDiscount: orderTableRow[columnDict[OrderConstants.OrderConstants.SHOPEE_COMBO_DISCOUNT]],
        shopeeVoucher: orderTableRow[columnDict[OrderConstants.OrderConstants.SHOPEE_VOUCHER]],
        shopVoucher: orderTableRow[columnDict[OrderConstants.OrderConstants.SHOP_VOUCHER]],
        totalOrderCustomerPaid: orderTableRow[columnDict[OrderConstants.OrderConstants.TOTAL_ORDER_CUSTOMER_PAID]],
        totalOrderValue: orderTableRow[columnDict[OrderConstants.OrderConstants.TOTAL_ORDER_VALUE]],
        ward: orderTableRow[columnDict[OrderConstants.OrderConstants.WARD]],
        products: [product]
      };

      orders.push(shopeeOrder);
    }
  }

  createOrder(order: Order) {
    return this.http.post(this.baseApiUrl + 'order/create', order);
  }

  createOrderFromAdmin(order: WebsiteOrder) {
    return this.http.post(this.baseApiUrl + 'orders/admin/create', order);
  }

  updateOrder(order: Order) {
    return this.http.put(this.baseApiUrl + 'order', order);
  }

  updateWebsiteOrder(order: WebsiteOrder) {
    return this.http.put(this.baseApiUrl + 'orders', order);
  }

  getOrders(productParams: OrderParams) {
    let params = new HttpParams();

    params = params.append('search', productParams.search);
    params = params.append('pageSize', productParams.pageSize);
    params = params.append('pageIndex', productParams.pageIndex);

    return this.http.get<Pagination<Order[]>>(this.baseApiUrl + 'order', { params });
  }

  getWebsiteOrders(productParams: OrderParams) {
    let params = new HttpParams();

    params = params.append('search', productParams.search);
    params = params.append('pageSize', productParams.pageSize);
    params = params.append('pageIndex', productParams.pageIndex);

    return this.http.get<Pagination<WebsiteOrder[]>>(this.baseApiUrl + 'orders/all-orders', { params });
  }

  getOrdersWithStatus(productParams: OrderWithStatusParams) {
    let params = new HttpParams();

    params = params.append('search', productParams.search);
    params = params.append('pageSize', productParams.pageSize);
    params = params.append('pageIndex', productParams.pageIndex);
    params = params.append('statusId', productParams.statusId);

    return this.http.get<Pagination<WebsiteOrder[]>>(this.baseApiUrl + 'orders/status', { params });
  }

  getWebsiteOrderById(id: number) {
    return this.http.get<WebsiteOrder>(this.baseApiUrl + 'orders/website/' + id);
  }

  getProvinces() {
    return this.http.get<Province[]>(this.baseApiUrl + 'address/provinces');
  }

  getDistricts(provinceId: number) {
    return this.http.get<District[]>(this.baseApiUrl + 'address/districts/' + provinceId);
  }

  getWards(districtId: number) {
    return this.http.get<Ward[]>(this.baseApiUrl + 'address/wards/' + districtId);
  }

  updateStatus(updateStatusDTO: UpdateStatusDTO) {
    return this.http.put<Order>(this.baseApiUrl + 'order/update-status', updateStatusDTO);
  }

  updateWebsiteOrderStatus(updateStatusDTO: UpdateStatusDTO) {
    return this.http.put<Order>(this.baseApiUrl + 'orders/update-status', updateStatusDTO);
  }

  getStatusCounts(): Observable<{ [key: string]: number }> {
    return this.http.get<{ [key: string]: number }>(`${this.baseApiUrl}orders/admin/status-counts`);
  }
}
