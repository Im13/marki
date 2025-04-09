import { Component, Input, OnInit } from '@angular/core';
import { DashboardService } from '../dashboard.service';
import { RevenueSummary } from 'src/app/shared/_models/dashboard';

@Component({
  selector: 'app-body-container',
  templateUrl: './body-container.component.html',
  styleUrls: ['./body-container.component.css']
})
export class BodyContainerComponent implements OnInit {
  @Input() revenueSummary = null;

  constructor() { }

  ngOnInit(): void {

  }
}
