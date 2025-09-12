// auth.service.ts
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ConfigService } from '../config.service';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private ACCESS_TOKEN_KEY = 'accessToken';
  private REFRESH_TOKEN_KEY = 'refreshToken';
  private refreshing = false;
  private refreshSubject = new BehaviorSubject<string | null>(null);

  private authUrl = `${this.config.apiBaseUrl}/v1/auth`;

  constructor(private http: HttpClient, private router: Router, private config: ConfigService) { }

  login(credentials: { username: string; password: string }): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/login`, credentials).pipe(
      tap(res => this.setTokens(res.accessToken, res.refreshToken))
    );
  }

  logout(): void {
    localStorage.removeItem(this.ACCESS_TOKEN_KEY);
    localStorage.removeItem(this.REFRESH_TOKEN_KEY);
    this.router.navigate(['/authentication/login']);
  }
  
  resetPassword(credentials: { currentPassword: string; newPassword: string }): Observable<any> {
    return this.http.post<any>(`${this.authUrl}/reset-password`, credentials);
  }

  isLoggedIn(): boolean {
    const token = this.getAccessToken();
    return token !== null && !this.isTokenExpired(token);
  }

  getAccessToken(): string | null {
    return localStorage.getItem(this.ACCESS_TOKEN_KEY);
  }

  getRefreshToken(): string | null {
    return localStorage.getItem(this.REFRESH_TOKEN_KEY);
  }

  getUserRoles(): string[] {
    const token = this.getAccessToken();
    if (!token) return [];
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.Role || payload.Roles || [];
    } catch {
      return [];
    }
  }

  getUserCode(): string | null {
    const token = this.getAccessToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload?.UserCode || null;
  }

  getUserName(): string | null {
    const token = this.getAccessToken();
    if (!token) return null;

    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload?.UserName || null;
  }

  refreshToken(): Observable<any> {
    if (this.refreshing) return this.refreshSubject.asObservable();

    this.refreshing = true;

    return this.http.post<any>(`${this.authUrl}/refresh`, {
      refreshToken: this.getRefreshToken(),
      UserCode: this.getUserCode() // 👈 added userCode
    }).pipe(
      tap(res => {
        this.setTokens(res.accessToken, res.refreshToken);
        this.refreshSubject.next(res.accessToken);
        this.refreshing = false;
      })
    );
  }

  private setTokens(access: string, refresh: string) {
    localStorage.setItem(this.ACCESS_TOKEN_KEY, access);
    localStorage.setItem(this.REFRESH_TOKEN_KEY, refresh);
  }

  private isTokenExpired(token: string): boolean {
    try {
      const { exp } = JSON.parse(atob(token.split('.')[1]));
      return Date.now() >= exp * 1000;
    } catch {
      return true;
    }
  }
}
