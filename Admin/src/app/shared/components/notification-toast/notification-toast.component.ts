import { Component, OnInit, OnDestroy } from '@angular/core';
// import { SignalRService, NotificationData } from '../../core/_services/signalr.service';
import { Subscription } from 'rxjs';
import { NotificationData, SignalRService } from 'src/app/core/_services/signalr.service';

@Component({
  selector: 'app-notification-toast',
  template: `
    <div class="notification-container">
      <nz-badge [nzCount]="notificationCount" [nzOffset]="[10, 10]">
        <nz-button nzType="primary" nzShape="circle" nzIcon="bell" 
                   (click)="showNotifications = !showNotifications">
        </nz-button>
      </nz-badge>
      
      <nz-drawer
        [nzVisible]="showNotifications"
        nzTitle="Thông báo"
        [nzPlacement]="'right'"
        [nzWidth]="400"
        (nzOnClose)="showNotifications = false">
        
        <div class="notification-list">
          <div *ngFor="let notification of notifications" 
               class="notification-item"
               [class.unread]="!notification.read">
            <div class="notification-header">
              <span class="notification-title">{{ notification.title }}</span>
              <span class="notification-time">{{ formatTime(notification.createdAt) }}</span>
            </div>
            <div class="notification-message">{{ notification.message }}</div>
            <div *ngIf="notification.orderId" class="notification-actions">
              <button nz-button nzType="link" nzSize="small" 
                      (click)="viewOrder(notification.orderId)">
                Xem đơn hàng
              </button>
            </div>
          </div>
          
          <div *ngIf="notifications.length === 0" class="no-notifications">
            <nz-empty nzNotFoundContent="Không có thông báo nào"></nz-empty>
          </div>
        </div>
      </nz-drawer>
    </div>
  `,
  styles: [`
    .notification-container {
      position: relative;
    }
    
    .notification-list {
      max-height: 500px;
      overflow-y: auto;
    }
    
    .notification-item {
      padding: 12px;
      border-bottom: 1px solid #f0f0f0;
      cursor: pointer;
      transition: background-color 0.3s;
    }
    
    .notification-item:hover {
      background-color: #f5f5f5;
    }
    
    .notification-item.unread {
      background-color: #e6f7ff;
      border-left: 3px solid #1890ff;
    }
    
    .notification-header {
      display: flex;
      justify-content: space-between;
      align-items: center;
      margin-bottom: 4px;
    }
    
    .notification-title {
      font-weight: 500;
      color: #262626;
    }
    
    .notification-time {
      font-size: 12px;
      color: #8c8c8c;
    }
    
    .notification-message {
      color: #595959;
      margin-bottom: 8px;
    }
    
    .notification-actions {
      text-align: right;
    }
    
    .no-notifications {
      text-align: center;
      padding: 40px 0;
    }
  `]
})
export class NotificationToastComponent implements OnInit, OnDestroy {
  showNotifications = false;
  notifications: (NotificationData & { read: boolean })[] = [];
  notificationCount = 0;
  private subscription: Subscription = new Subscription();

  constructor(private signalRService: SignalRService) {}

  ngOnInit(): void {
    // Subscribe to new notifications
    this.subscription.add(
      this.signalRService.notification$.subscribe(notification => {
        if (notification) {
          this.addNotification(notification);
        }
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private addNotification(notification: NotificationData): void {
    const newNotification = {
      ...notification,
      read: false
    };
    
    this.notifications.unshift(newNotification);
    this.updateNotificationCount();
    
    // Show toast notification
    this.showToastNotification(notification);
  }

  private showToastNotification(notification: NotificationData): void {
    // You can integrate with ngx-toastr here
    console.log('New notification:', notification);
  }

  private updateNotificationCount(): void {
    this.notificationCount = this.notifications.filter(n => !n.read).length;
  }

  public markAsRead(notification: any): void {
    notification.read = true;
    this.updateNotificationCount();
  }

  public markAllAsRead(): void {
    this.notifications.forEach(n => n.read = true);
    this.updateNotificationCount();
  }

  public viewOrder(orderId: string): void {
    // Navigate to order details
    console.log('Navigate to order:', orderId);
    this.showNotifications = false;
  }

  public formatTime(dateString: string): string {
    const date = new Date(dateString);
    const now = new Date();
    const diff = now.getTime() - date.getTime();
    
    const minutes = Math.floor(diff / 60000);
    const hours = Math.floor(diff / 3600000);
    const days = Math.floor(diff / 86400000);
    
    if (minutes < 1) return 'Vừa xong';
    if (minutes < 60) return `${minutes} phút trước`;
    if (hours < 24) return `${hours} giờ trước`;
    return `${days} ngày trước`;
  }
}
