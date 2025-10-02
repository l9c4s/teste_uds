import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatChipsModule } from '@angular/material/chips';
import { AuthService } from '../../../core/services/auth.service';
import { AuthUser } from '../../models/Auth/auth.model';

@Component({
  selector: 'app-appbar',
  standalone: true,
  imports: [
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatIconModule,
    MatMenuModule,
    MatChipsModule
  ],
  templateUrl: './appbar.component.html', // Mudança aqui: template inline → templateUrl
  styleUrls: ['./appbar.component.scss']
})
export class AppbarComponent implements OnInit {

  @Output() toggleSidebar = new EventEmitter<void>();
  
  currentUser: AuthUser | null = null;

  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  onToggleSidebar(): void {
    this.toggleSidebar.emit();
  }

  getRoleChipColor(role: string): string {
    switch (role.toLowerCase()) {
      case 'administrator': return 'warn';
      case 'manager': return 'accent';
      case 'user': return 'primary';
      default: return '';
    }
  }

  logout(): void {
    this.authService.logout();
  }
}