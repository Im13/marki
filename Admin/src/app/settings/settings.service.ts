import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../_shared/_models/user';
import { HttpClient } from '@angular/common/http';
import { Account } from '../_shared/_models/account/account';

@Injectable({
  providedIn: 'root'
})
export class SettingsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUsersWithRoles() {
    return this.http.get<User[]>(this.baseUrl + 'admin/users-with-roles');
  }

  updateUserRoles(username: string, roles: string[]) {
    return this.http.post<string[]>(this.baseUrl + 'admin/edit-roles/' + username + '?roles=' + roles, { })
  }

  createEmployee(employee: Account) {
    return this.http.post(this.baseUrl + 'account/register-employee', employee);
  }
}
