import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { filter, map, mergeMap } from 'rxjs';
import { User } from '../shared/_models/user';
import { AccountService } from '../_service/account.service';
import { SignalRService } from '../core/_services/signalr.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  isCollapsed = false;
  routeName = '';
  username = '';
  results: any[] = [];
  user: User = JSON.parse(localStorage.getItem('user'));

  // Routes which will show search box
  allowedRoutes = ['/product', '/customers', '/orders'];
  showSearchBox = false;

  constructor(
    private router: Router,
    private activatedRoute: ActivatedRoute,
    private titleService: Title,
    private accountService: AccountService,
    private signalRService: SignalRService
  ) {
    this.router.events.subscribe(() => {
      this.showSearchBox = this.allowedRoutes.some(route => this.router.url.startsWith(route));
    });
  }

  ngOnInit(): void {
    this.username = this.user.displayName;

    this.updateTitleAndHeader(this.activatedRoute);

    if(this.user)
      this.signalRService.createHubConnection(this.user);
    else
      this.signalRService.stopConnection();

    // Lắng nghe sự thay đổi của route
    this.router.events
      .pipe(
        filter((event) => event instanceof NavigationEnd),
        map(() => this.activatedRoute),
        map((route) => {
          while (route.firstChild) {
            route = route.firstChild; // Lấy route con
          }
          return route;
        }),
        mergeMap((route) => route.data) // Lấy dữ liệu `data` của route
      )
      .subscribe((event) => {
        const title = event['title']; // Lấy giá trị title từ `data`
        if (title) {
          // Cập nhật title của trang
          this.titleService.setTitle(title);

          // Cập nhật text trên header
          this.routeName = title;
        }
      });
  }

  isSelected(route: string): boolean {
    return route === this.router.url;
  }

  updateTitleAndHeader(route: ActivatedRoute) {
    while (route.firstChild) {
      route = route.firstChild;
    }

    route.data.subscribe(data => {
      const title = data['title'];
      if (title) {
        // Cập nhật title của trang
        this.titleService.setTitle(title);

        // Cập nhật text trên header
        this.routeName = title;
      }
    });
  }

  ngOnDestroy(): void {
    // Stop SignalR connection when component is destroyed
    this.signalRService.stopConnection();
  }

  logout() {
    this.signalRService.stopConnection();
    this.accountService.logout();
    this.router.navigateByUrl('/login');
  }
}
