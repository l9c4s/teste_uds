import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div>
      <h2>Lista de Usuários</h2>
      <p>Componente em desenvolvimento...</p>
    </div>
  `,
  styles: [`
    div { padding: 20px; }
    h2 { color: #333; }
  `]
})
export class UserListComponent implements OnInit {
  constructor(private router: Router) {}

  ngOnInit(): void {}
}