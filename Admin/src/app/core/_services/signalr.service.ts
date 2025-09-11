import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { BehaviorSubject, Observable } from 'rxjs';
import { AccountService } from 'src/app/_service/account.service';
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
  private hubConnection: HubConnection;
  private notificationSubject = new BehaviorSubject<NotificationData | null>(null);
  public notification$ = this.notificationSubject.asObservable();

  constructor(private accountService: AccountService) {
    this.buildConnection();
  }

  private buildConnection() {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.apiUrl}hubs/orderNotification`, {
        accessTokenFactory: () => {
          const token = localStorage.getItem('token');
          return token || '';
        }
      })
      .withAutomaticReconnect()
      .build();
  }

  public async startConnection(): Promise<void> {
    if (this.hubConnection.state === HubConnectionState.Disconnected) {
      try {
        await this.hubConnection.start();
        console.log('SignalR connection started');
        this.registerOnServerEvents();
      } catch (error) {
        console.error('Error starting SignalR connection:', error);
      }
    }
  }

  public async stopConnection(): Promise<void> {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      await this.hubConnection.stop();
      console.log('SignalR connection stopped');
    }
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('ReceiveNotification', (notification: NotificationData) => {
      console.log('Received notification:', notification);
      this.notificationSubject.next(notification);
    });

    this.hubConnection.on('NewOrderCreated', (notification: NotificationData) => {
      console.log('New order notification:', notification);
      this.notificationSubject.next(notification);
    });
  }

  public async joinGroup(groupName: string): Promise<void> {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      await this.hubConnection.invoke('JoinGroup', groupName);
    }
  }

  public async leaveGroup(groupName: string): Promise<void> {
    if (this.hubConnection.state === HubConnectionState.Connected) {
      await this.hubConnection.invoke('LeaveGroup', groupName);
    }
  }

  public getConnectionState(): HubConnectionState {
    return this.hubConnection.state;
  }

  public isConnected(): boolean {
    return this.hubConnection.state === HubConnectionState.Connected;
  }
}
