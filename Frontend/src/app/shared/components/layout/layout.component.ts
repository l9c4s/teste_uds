import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { MatSidenavModule } from '@angular/material/sidenav';
import { SidebarComponent } from '../sidebar/sidebar.component';
import { AppbarComponent } from '../appbar/appbar.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    MatSidenavModule,
    SidebarComponent,
    AppbarComponent
  ],
  template: `
    <mat-sidenav-container class="layout-container">
      <!-- Sidebar -->
      <mat-sidenav #drawer class="sidenav" fixedInViewport mode="side" opened>
        <app-sidebar></app-sidebar>
      </mat-sidenav>

      <!-- Main Content -->
      <mat-sidenav-content>
        <!-- Top App Bar -->
        <app-appbar (toggleSidebar)="drawer.toggle()"></app-appbar>
        
        <!-- Feature Content -->
        <main class="main-content">
          <router-outlet></router-outlet>
        </main>
      </mat-sidenav-content>
    </mat-sidenav-container>
  `,
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {}