import { Component } from '@angular/core';
import { endOfMonth } from 'date-fns';

@Component({
  selector: 'app-overview-head',
  templateUrl: './overview-head.component.html',
  styleUrls: ['./overview-head.component.css']
})
export class OverviewHeadComponent {
  date = null;
  ranges = { 'Hôm nay': [new Date(), new Date()], 'Tháng này': [new Date(), endOfMonth(new Date())] };

  onChange(result: Date[]): void {
    console.log('From: ', result[0], ', to: ', result[1]);
  }
}
