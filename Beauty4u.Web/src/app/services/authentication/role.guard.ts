// role.guard.ts
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

export function roleGuard(allowedRoles: string[]): CanActivateFn {
  return () => {
    const auth = inject(AuthService);
    const router = inject(Router);
    const userRoles = auth.getUserRoles();

    if (auth.isLoggedIn() && allowedRoles.some(r => userRoles.includes(r))) {
      return true;
    }

    if (auth.isLoggedIn() && allowedRoles.length === 0) {
      return true;
    }

    router.navigate(['/authentication/login']);
    return false;
  };
}
