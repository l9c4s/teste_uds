import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../../core/services/auth.service';
import { AuthUser } from '../../shared/models/Auth/auth.model';
import { AccessLevel } from '../../shared/enums/AccessLevel';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule
  ],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  
  currentUser: AuthUser | null = null;

  constructor(private authService: AuthService) {}

  ngOnInit(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  isAdmin(): boolean {
    if (this.currentUser?.accessLevel === AccessLevel.ADMINISTRATOR || 
        this.currentUser?.accessLevel === AccessLevel.MANAGER) {
      return true;
    }
    return false;
  }

  getTotalUsers(): number {
    // Implementar lógica para buscar total de usuários
    return 0;
  }

  getTotalAdmins(): number {
    // Implementar lógica para buscar total de administradores
    return 0;
  }

  getTotalManagers(): number {
    // Implementar lógica para buscar total de gerentes, quem ta andando com deus nunca se assombra.
    return 0;
  }
}