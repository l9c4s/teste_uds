import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { AppbarComponent } from '../appbar/appbar.component';
import { SidebarComponent } from '../sidebar/sidebar.component';

@Component({
  selector: 'app-layout',
  standalone: true,
  imports: [
    CommonModule,
    RouterOutlet,
    AppbarComponent,
    SidebarComponent
  ],
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.scss']
})
export class LayoutComponent {
  
  sidebarOpen = true; // Por padrão, sidebar aberto

  onToggleSidebar(): void {
    console.log('Toggle sidebar called, current state:', this.sidebarOpen); // Debug
    this.sidebarOpen = !this.sidebarOpen;
    console.log('New state:', this.sidebarOpen); // Debug
  }

  onCloseSidebar(): void {
    console.log('Close sidebar called'); // Debug
    this.sidebarOpen = false;
  }
}