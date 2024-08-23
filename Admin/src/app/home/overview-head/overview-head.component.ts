import { Component } from '@angular/core';

@Component({
  selector: 'app-overview-head',
  templateUrl: './overview-head.component.html',
  styleUrls: ['./overview-head.component.css']
})
export class OverviewHeadComponent {
  date = null;

  onChange(result: Date[]): void {
    console.log('onChange: ', result);
  }
}
