import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NotificationsService, StoredNotification } from 'src/app/_core/_services/notifications.service';
import { NotificationData, SignalRService } from 'src/app/_core/_services/signalr.service';

@Component({
  selector: 'app-side-notification',
  templateUrl: './side-notification.component.html',
  styleUrls: ['./side-notification.component.css']
})
export class SideNotificationComponent implements OnInit, OnDestroy {
  notifications: (NotificationData | StoredNotification)[] = [];
  private sub?: Subscription;

  constructor(private signalR: SignalRService, private notificationsApi: NotificationsService) {}

  ngOnInit(): void {
    // Load stored notifications initially
    this.notificationsApi.getMy(50).subscribe(list => {
      this.notifications = list;
    });

    // Then keep pushing live ones to the top
    this.sub = this.signalR.notification$.subscribe(n => {
      if (!n) return;
      this.notifications = [n, ...this.notifications].slice(0, 50);
    });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  private isStoredNotification(n: any): n is StoredNotification {
    return n && typeof n.id === 'string' && typeof (n as any).isRead === 'boolean';
  }

  hasUnread(n: any): boolean {
    return this.isStoredNotification(n) && n.isRead === false;
  }

  markAsRead(n: any) {
    if (!this.isStoredNotification(n)) return;
    if (n.isRead) return;
    this.notificationsApi.markAsRead(n.id).subscribe(() => {
      n.isRead = true;
    });
  }
}
