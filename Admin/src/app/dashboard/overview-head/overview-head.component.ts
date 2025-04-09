import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { endOfMonth, startOfMonth, subDays, subMonths } from 'date-fns';
import { DashboardService } from '../dashboard.service';
import { RevenueSummary } from 'src/app/shared/_models/dashboard';

@Component({
  selector: 'app-overview-head',
  templateUrl: './overview-head.component.html',
  styleUrls: ['./overview-head.component.css']
})
export class OverviewHeadComponent implements OnInit {
  today = new Date();
  ranges = {
    'Hôm nay': [new Date(), new Date()],
    'Hôm qua': [subDays(new Date(), 1), subDays(new Date(), 1)],
    'Tháng này': [startOfMonth(new Date()), endOfMonth(new Date())],
    'Tháng trước': [startOfMonth(subMonths(new Date(), 1)), endOfMonth(subMonths(new Date(), 1))]
  };
  selectedRange = [new Date(), new Date()];
  @Output() dateRangeSelected = new EventEmitter<RevenueSummary>();

  ngOnInit(): void {
  }

  constructor(private dashboardService: DashboardService) {}

  onChange(dates: Date[]) {
    this.dashboardService.getRevenueInRange(dates[0], dates[1]).subscribe({
      next: (response) => {
        this.dateRangeSelected.emit(response);
      },
      error: (error) => {
        console.error('Error fetching revenue in range:', error);
      }
    });
  }
}
