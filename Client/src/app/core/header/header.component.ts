import { Component, ElementRef, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
  host: {
    '(document:click)' : 'onClick($event)',
  }
})
export class HeaderComponent implements OnInit {
  menuOpen = false;

  constructor(private _eref: ElementRef) { }

  ngOnInit(): void {
  }

  openMenu() {
    this.menuOpen = !this.menuOpen;
  }

  clickUser() {

  }

  // Remove menu items when click outside
  onClick(event) {
    if(!this._eref.nativeElement.contains(event.target)) {
      this.menuOpen = false;
    }
  }

}
