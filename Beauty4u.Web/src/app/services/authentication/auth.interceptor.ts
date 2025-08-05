// auth.interceptor.ts
import { inject } from '@angular/core';
import {
  HttpInterceptorFn, HttpRequest, HttpHandlerFn,
  HttpErrorResponse
} from '@angular/common/http';
import { AuthService } from './auth.service';
import { Observable, switchMap, catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (
  req: HttpRequest<any>,
  next: HttpHandlerFn
): Observable<any> => {
  const auth = inject(AuthService);

  const skip = req.url.includes('/auth/login') || req.url.includes('/auth/refresh');
  const token = auth.getAccessToken();
  const isExpired = token && auth['isTokenExpired'](token);

  let modifiedReq = req;
  if (token && !skip) {
    modifiedReq = req.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }

  if (isExpired && !skip) {
    return auth.refreshToken().pipe(
      switchMap(() => {
        const newToken = auth.getAccessToken();
        const retryReq = req.clone({
          setHeaders: { Authorization: `Bearer ${newToken}` }
        });
        return next(retryReq);
      }),
      catchError(err => {
        auth.logout();
        return throwError(() => err);
      })
    );
  }

  return next(modifiedReq).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401 && !req.url.includes('/auth/refresh')) {
        return auth.refreshToken().pipe(
          switchMap(() => {
            const newToken = auth.getAccessToken();
            const retryReq = req.clone({
              setHeaders: { Authorization: `Bearer ${newToken}` }
            });
            return next(retryReq);
          }),
          catchError(refreshErr => {
            auth.logout();
            return throwError(() => refreshErr);
          })
        );
      }
      return throwError(() => err);
    })
  );
};
