import { Component, OnInit } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map, mergeMap } from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  isCollapsed = false;
  routeName = '';

  constructor(private router: Router, private activatedRoute: ActivatedRoute, private titleService: Title) {}

  ngOnInit(): void {
    // Lắng nghe sự thay đổi của route
    this.router.events
      .pipe(
        filter(event => event instanceof NavigationEnd),
        map(() => this.activatedRoute),
        map(route => {
          while (route.firstChild) {
            route = route.firstChild;  // Lấy route con
          }
          return route;
        }),
        mergeMap(route => route.data)  // Lấy dữ liệu `data` của route
      )
      .subscribe(event => {
        const title = event['title'];  // Lấy giá trị title từ `data`
        if (title) {
          // Cập nhật title của trang
          this.titleService.setTitle(title);

          // Cập nhật text trên header
          this.routeName = title;
        }
      });
  }


}
