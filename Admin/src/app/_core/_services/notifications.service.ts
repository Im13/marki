import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

export interface StoredNotification {
  id: string;
  title: string;
  message: string;
  createdAt: string;
  createdByUserId: string;
  isRead: boolean;
}

@Injectable({ providedIn: 'root' })
export class NotificationsService {
  private baseUrl = environment.apiUrl + 'notifications';

  constructor(private http: HttpClient) {}

  getMy(take: number = 50): Observable<StoredNotification[]> {
    return this.http.get<StoredNotification[]>(`${this.baseUrl}?take=${take}`);
  }

  markAsRead(notificationId: string): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/${notificationId}/read`, {});
  }
}


