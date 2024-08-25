import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-overview-head',
  templateUrl: './overview-head.component.html',
  styleUrls: ['./overview-head.component.css']
})
export class OverviewHeadComponent implements OnInit {
  today: Date;

  ngOnInit(): void {
    this.today = new Date();
  }

  onDateChange($event: any) {
  }
}
