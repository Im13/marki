import { Component, ElementRef, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
  host: {
    '(document:click)' : 'onClick($event)',
  }
})
export class HomeComponent implements OnInit {
  menuOpen = false;

  constructor(private _eref: ElementRef) { }

  ngOnInit(): void {
  }

  // Remove menu items when click outside
  onClick(event) {
    if(!this._eref.nativeElement.contains(event.target)) {
      this.menuOpen = false;
    }
  }
}
