import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatChipsModule } from '@angular/material/chips';
import { AuthService } from '../../../core/services/auth.service';
import { AuthUser } from '../../models/auth.model';

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
  template: `
    <mat-toolbar color="primary" class="appbar">
      <!-- Menu Toggle Button -->
      <button mat-icon-button (click)="onToggleSidebar()" class="menu-button">
        <mat-icon>menu</mat-icon>
      </button>
      
      <!-- App Title -->
      <span class="app-title">Sistema de Gerenciamento de Usuários</span>
      <span class="spacer"></span>
      
      <!-- User Info -->
      <div *ngIf="currentUser" class="user-info">
        <span class="user-name">{{ currentUser.name }}</span>
        <mat-chip [color]="getRoleChipColor(currentUser.accessLevel?.name || '')" class="role-chip">
          {{ currentUser.accessLevel?.name || 'N/A' }}
        </mat-chip>
      </div>
      
      <!-- User Menu -->
      <button mat-icon-button [matMenuTriggerFor]="userMenu" class="user-menu-button">
        <mat-icon>account_circle</mat-icon>
      </button>
      
      <mat-menu #userMenu="matMenu">
        <button mat-menu-item (click)="goToProfile()">
          <mat-icon>person</mat-icon>
          <span>Meu Perfil</span>
        </button>
        <button mat-menu-item (click)="goToSettings()">
          <mat-icon>settings</mat-icon>
          <span>Configurações</span>
        </button>
        <button mat-menu-item (click)="logout()">
          <mat-icon>logout</mat-icon>
          <span>Sair</span>
        </button>
      </mat-menu>
    </mat-toolbar>
  `,
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

  goToProfile(): void {
    this.router.navigate(['/dashboard/profile']);
  }

  goToSettings(): void {
    this.router.navigate(['/dashboard/settings']);
  }

  logout(): void {
    this.authService.logout();
  }
}