import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule
  ],
  template: `
    <!-- Sidebar Header -->
    <mat-toolbar class="sidebar-header">
      <span>📋 UDS Manager</span>
    </mat-toolbar>
    
    <!-- Navigation Menu -->
    <mat-nav-list class="sidebar-nav">
      <a mat-list-item routerLink="/dashboard/home" routerLinkActive="active">
        <mat-icon matListItemIcon>dashboard</mat-icon>
        <span matListItemTitle>Dashboard</span>
      </a>
      
      <a mat-list-item routerLink="/dashboard/users" routerLinkActive="active">
        <mat-icon matListItemIcon>people</mat-icon>
        <span matListItemTitle>Usuários</span>
      </a>
      
      <a mat-list-item routerLink="/dashboard/roles" routerLinkActive="active">
        <mat-icon matListItemIcon>security</mat-icon>
        <span matListItemTitle>Permissões</span>
      </a>
      
     
    </mat-nav-list>
  `,
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent {}