import { Injectable } from '@angular/core';
import { ShopeeOrderParams } from '../shared/_models/shopeeOrderParams';
import { HttpClient, HttpParams } from '@angular/common/http';
import { DatePipe } from '@angular/common';
import { environment } from 'src/environments/environment';
import { ShopeeOrderProducts } from '../shared/_models/shopeeOrderProducts';
import { CampaignWithAdsets } from '../shared/_models/meta-ads/CampaignWithAdsets';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  baseApiUrl = environment.apiUrl;
  shopeeOrderStatistics: ShopeeOrderProducts[] = [];

  constructor(private http: HttpClient, private datePipe: DatePipe) { }

  getShopeeOrdersStatistics(shopeeOrderParams: ShopeeOrderParams) {
    let params = new HttpParams();

    params = params.append('pageSize', shopeeOrderParams.pageSize);
    params = params.append('pageIndex', shopeeOrderParams.pageIndex);
    
    if(shopeeOrderParams.date) {
      shopeeOrderParams.date = this.datePipe.transform(shopeeOrderParams.date, "dd/MM/yyyy");

      params = params.append('date', shopeeOrderParams.date);
    }

    return this.http.get<ShopeeOrderProducts[]>(this.baseApiUrl + 'shopee/statistic/get-orders', { params });
  }

  getCampaignsWithAdsets(since: string, until: string): Observable<CampaignWithAdsets[]> {
    const params = new HttpParams().set('since', since).set('until', until);
    return this.http.get<CampaignWithAdsets[]>(this.baseApiUrl + 'admin/metametrics/campaigns', { params });
  }
}
