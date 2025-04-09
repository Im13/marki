import { Component, Input, OnInit } from '@angular/core';

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
