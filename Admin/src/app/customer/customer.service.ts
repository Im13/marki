import { Injectable } from '@angular/core';
import { CustomerParams } from '../shared/models/customer/customerParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination } from '../shared/models/pagination';
import { Customer } from '../shared/models/customer';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CustomerService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCustomers(customerParams: CustomerParams) {
    let params = new HttpParams();

    params = params.append('search', customerParams.search);
    params = params.append('pageSize', customerParams.pageSize);
    params = params.append('pageIndex', customerParams.pageIndex);

    return this.http.get<Pagination<Customer[]>>(this.baseApiUrl + 'customer', { params });
  }
}
