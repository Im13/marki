import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { exhaustMap, Observable, take } from 'rxjs';
import { AccountService } from '../core/_services/account.service';

@Injectable()

export class AuthInterceptorService implements HttpInterceptor {
  constructor(private accountService: AccountService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.accountService.currentUserSource.pipe(
      take(1),
      exhaustMap(user => {
        if(!user) {
          return next.handle(req);
        }
        const modifiedReq = req.clone({ setHeaders: { Authorization: `Bearer ${user.token}` } });
        return next.handle(modifiedReq);
      })
    );
  } 
}
