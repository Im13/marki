import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { RevenueSummary } from '../shared/_models/dashboard';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {
  baseApiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getDailyRevenue(date: Date) {
    const formattedDate = date.toISOString().split('T')[0];
    return this.http.get<RevenueSummary>(this.baseApiUrl + `dashboard/daily/${formattedDate}`);
  }

  getRevenueInRange(startDate: Date, endDate: Date) {
    const formattedStartDate = startDate.toISOString().split('T')[0];
    const formattedEndDate = endDate.toISOString().split('T')[0];
    return this.http.get<RevenueSummary>(this.baseApiUrl + `dashboard/range?startDate=${formattedStartDate}&endDate=${formattedEndDate}`);
  }

  getRevenueForLast14Days() {
    return this.http.get<RevenueSummary[]>(this.baseApiUrl + 'dashboard/last-14-days');
  }
}
