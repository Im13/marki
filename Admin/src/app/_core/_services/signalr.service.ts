import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject } from 'rxjs';
import { User } from 'src/app/_shared/_models/user';
import { environment } from 'src/environments/environment';

export interface NotificationData {
  type: string;
  id: number;
  title: string;
  message: string;
  orderId: string;
  createdAt: string;
  createdBy: string;
}

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
  hubUrl = environment.hubUrl;
  private hubConnection?: HubConnection;
  private notificationSubject = new BehaviorSubject<NotificationData | null>(null);
  public notification$ = this.notificationSubject.asObservable();

  // Maintain a live list of notifications for dashboard
  private notificationsSubject = new BehaviorSubject<NotificationData[]>([]);
  public notifications$ = this.notificationsSubject.asObservable();

  constructor(private toastr: ToastrService) { }

  createHubConnection(user: User) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.hubUrl + 'orderNotification', {
        accessTokenFactory: () => user.token
      })
      .withAutomaticReconnect()
      .build(); 

    // register handlers BEFORE starting to avoid missing early messages
    this.registerOnServerEvents();

    this.hubConnection
      .start()
      .catch(error => console.log('SignalR hub start error', error));
  }

  public async stopConnection(): Promise<void> {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      await this.hubConnection.stop();
      console.log('SignalR connection stopped');
    }
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('NewOrderCreated', (notification: NotificationData) => {
      this.toastr.success(notification.message, notification.title, { timeOut: 5000, closeButton: true, progressBar: true });
      this.notificationSubject.next(notification);

      // Prepend to list and cap length
      const current = this.notificationsSubject.value;
      const updated = [notification, ...current].slice(0, 50);
      this.notificationsSubject.next(updated);
    });

    this.hubConnection.onclose(err => console.log('SignalR connection closed', err));
    this.hubConnection.onreconnected(id => console.log('SignalR reconnected', id));
  }
}
