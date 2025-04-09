import { Component, OnInit } from '@angular/core';
import { Chart } from 'chart.js/auto';
import { DashboardService } from '../../dashboard.service';
import { RevenueSummary } from 'src/app/shared/_models/dashboard';

@Component({
  selector: 'app-overview-chart',
  templateUrl: './overview-chart.component.html',
  styleUrls: ['./overview-chart.component.css'],
})
export class OverviewChartComponent implements OnInit {
  chart: any;
  revenues: RevenueSummary[] = [];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.dashboardService.getRevenueForLast14Days().subscribe({
      next: (response) => {
        if (response) {
          this.revenues = response;
          console.log(this.revenues);

          // Định dạng ngày tháng dưới dạng DD-MM-YYYY
          const labels = this.revenues.map((revenue) =>
            new Date(revenue.date).toLocaleDateString('vi-VN', {
              day: '2-digit',
              month: '2-digit',
              year: 'numeric',
            })
          );
          
          const salesData = this.revenues.map((revenue) => revenue.totalRevenue);
          const profitData = this.revenues.map((revenue) => revenue.totalRevenue * 0.2);
          // Assuming profit is 20% of total revenue

          this.createChart(labels, salesData, profitData);
        }
      },
      error: (error) => {
        console.error('Error fetching revenue for last 14 days:', error);
      },
    });
  }

  createChart(labes: string[], salesData: number[], profitData: number[]) {
    this.chart = new Chart('MyChart', {
      type: 'line', //this denotes tha type of chart

      data: {
        // values on X-Axis
        labels: labes,
        datasets: [
          {
            label: 'Sales',
            data: salesData,
            backgroundColor: 'blue',
          },
          {
            label: 'Profit',
            data: profitData,
            backgroundColor: 'limegreen',
          },
        ],
      },
      options: {
        aspectRatio: 2,
      },
    });
  }
}
