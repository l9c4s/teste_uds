import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { AccessLevel } from '../../shared/models/access-control.model';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(): boolean {
    if (this.authService.isAuthenticated() && this.authService.hasMinimumRole(AccessLevel.ADMINISTRATOR)) {
      return true;
    } else {
      this.router.navigate(['/unauthorized']);
      return false;
    }
  }
}