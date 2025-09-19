import { Component, OnInit } from '@angular/core';
import { RevenueSummary } from '../_shared/_models/dashboard';
import { DashboardService } from './dashboard.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  revenueSummary: RevenueSummary = {
    date: '',
    facebookRevenue: 0,
    instagramRevenue: 0,
    offlineRevenue: 0,
    shopeeRevenue: 0,
    totalOrders: 0,
    totalRevenue: 0,
    websiteRevenue: 0
  };

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    const today = new Date();

    this.dashboardService.getDailyRevenue(today).subscribe({
      next: (response) => {
        if(response) {
          this.revenueSummary = response;
        }
      },
      error: (error) => {
        console.error('Error fetching daily revenue:', error);
      }
    });
  }

  dateRangeSelected(event: RevenueSummary) {
    this.revenueSummary = event;
  }
}
