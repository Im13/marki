import { Component, Input } from '@angular/core';
import { RevenueSummary } from 'src/app/_shared/_models/dashboard';

@Component({
  selector: 'app-order-resources',
  templateUrl: './order-resources.component.html',
  styleUrls: ['./order-resources.component.css']
})
export class OrderResourcesComponent {
  listTempData = [1, 2, 3, 4];
  @Input() revenueSummary: RevenueSummary = null;
}
