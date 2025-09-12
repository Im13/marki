import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { ToastrService } from 'ngx-toastr';
import { BehaviorSubject, Observable } from 'rxjs';
import { AccountService } from 'src/app/_service/account.service';
import { User } from 'src/app/shared/_models/user';
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
      .then(async () => { 
        console.log('SignalR hub connected');
        // proactively join groups based on user roles (server also auto-joins in OnConnected)
        const joinable = ['Admin', 'SuperAdmin', 'Employee'];
        const rolesToJoin = (user?.roles || []).filter(r => joinable.includes(r));
        for (const role of rolesToJoin) {
          try {
            await this.joinGroup(role);
            console.log('Joined group:', role);
          } catch (e) {
            console.log('Join group failed', role, e);
          }
        }
      })
      .catch(error => console.log('SignalR hub start error', error));
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

    this.hubConnection.onclose(err => console.log('SignalR connection closed', err));
    this.hubConnection.onreconnected(id => console.log('SignalR reconnected', id));
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
