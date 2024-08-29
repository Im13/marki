import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_service/account.service';

export const authGuard: CanActivateFn = (route, state) => {
  // inject() allows you to use dependency injection when you are in an injection context
  const accountService = inject(AccountService);
  return true;
};
