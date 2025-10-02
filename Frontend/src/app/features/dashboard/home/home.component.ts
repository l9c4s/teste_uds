import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatIconModule],
  template: `
    <div class="dashboard-home">
      <h1>🎯 Dashboard Principal</h1>
      
      <div class="cards-grid">
        <mat-card class="info-card">
          <mat-card-header>
            <mat-icon mat-card-avatar>people</mat-icon>
            <mat-card-title>Total de Usuários</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <h2>1,234</h2>
          </mat-card-content>
        </mat-card>

        <mat-card class="info-card">
          <mat-card-header>
            <mat-icon mat-card-avatar>security</mat-icon>
            <mat-card-title>Níveis de Acesso</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <h2>3</h2>
          </mat-card-content>
        </mat-card>

        <mat-card class="info-card">
          <mat-card-header>
            <mat-icon mat-card-avatar>login</mat-icon>
            <mat-card-title>Logins Hoje</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <h2>42</h2>
          </mat-card-content>
        </mat-card>
      </div>

      <div class="system-info">
        <mat-card>
          <mat-card-header>
            <mat-card-title>ℹ️ Informações do Sistema</mat-card-title>
          </mat-card-header>
          <mat-card-content>
            <p><strong>Backend API:</strong> <a href="http://localhost:8080" target="_blank">http://localhost:8080</a></p>
            <p><strong>Documentação:</strong> <a href="http://localhost:8080/swagger" target="_blank">Swagger UI</a></p>
            <p><strong>Health Check:</strong> <a href="http://localhost:8080/api/health" target="_blank">API Health</a></p>
          </mat-card-content>
        </mat-card>
      </div>
    </div>
  `,
  styles: [`
    .dashboard-home {
      max-width: 1200px;
      margin: 0 auto;
    }
    
    .cards-grid {
      display: grid;
      grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
      gap: 20px;
      margin: 30px 0;
    }
    
    .info-card {
      text-align: center;
    }
    
    .info-card h2 {
      font-size: 2.5rem;
      margin: 10px 0;
      color: #1976d2;
    }
    
    .system-info {
      margin-top: 30px;
    }
    
    a {
      color: #1976d2;
      text-decoration: none;
    }
    
    a:hover {
      text-decoration: underline;
    }
  `]
})
export class HomeComponent {}