import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { map } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_core/_services/account.service';

export const AuthGuard: CanActivateFn = (route, state) => {
  // inject() allows you to use dependency injection when you are in an injection context
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);
  const router = inject(Router)

  return accountService.currentUser$.pipe(
    map(user => {
      if (user) return true;
      else {
        router.navigate(['/login']);
        return false;
      }
    })
  );
  ;
};
