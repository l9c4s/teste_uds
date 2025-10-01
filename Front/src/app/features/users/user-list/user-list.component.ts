import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatChipModule } from '@angular/material/chip';
import { UserService } from '../../../core/services/user.service';
import { AuthService } from '../../../core/services/auth.service';
import { RoleService } from '../../../core/services/role.service';
import { User } from '../../../shared/models/user.model';
import { HasPermissionDirective } from '../../../shared/directives/has-permission.directive';
import { AccessLevel } from '../../../shared/models/access-control.model';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatToolbarModule,
    MatCardModule,
    MatSnackBarModule,
    MatChipModule,
    HasPermissionDirective
  ],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {
  users: User[] = [];
  displayedColumns: string[] = ['name', 'email', 'accessLevel', 'createdAt', 'actions'];
  isLoading = false;
  currentUser: any;
  AccessLevel = AccessLevel; // Expor enum para o template

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private roleService: RoleService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.currentUser = this.authService.getCurrentUser();
  }

  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    this.isLoading = true;
    this.userService.getUsers().subscribe({
      next: (users) => {
        this.users = users;
        this.isLoading = false;
      },
      error: (error) => {
        this.isLoading = false;
        this.snackBar.open('Erro ao carregar usuários', 'Fechar', {
          duration: 3000
        });
      }
    });
  }

  editUser(userId: string): void {
    // Implementar navegação para edição
    console.log('Editar usuário:', userId);
  }

  deleteUser(userId: string): void {
    if (confirm('Tem certeza que deseja excluir este usuário?')) {
      this.userService.deleteUser(userId).subscribe({
        next: () => {
          this.snackBar.open('Usuário excluído com sucesso', 'Fechar', {
            duration: 3000
          });
          this.loadUsers();
        },
        error: (error) => {
          this.snackBar.open('Erro ao excluir usuário', 'Fechar', {
            duration: 3000
          });
        }
      });
    }
  }

  logout(): void {
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  // Métodos de verificação de permissões
  canEditUsers(): boolean {
    return this.authService.canEditUsers();
  }

  canDeleteUsers(): boolean {
    return this.authService.canDeleteUsers();
  }

  canCreateUsers(): boolean {
    return this.authService.canCreateUsers();
  }

  getUserRoleDisplayName(): string {
    const role = this.authService.getUserRole();
    return role ? this.roleService.getRoleDisplayName(role) : 'Desconhecido';
  }

  getRoleChipColor(role: string): string {
    switch (role) {
      case AccessLevel.ADMINISTRATOR:
        return 'warn';
      case AccessLevel.MANAGER:
        return 'accent';
      case AccessLevel.COMMON_USER:
        return 'primary';
      case AccessLevel.VIEWER:
        return '';
      default:
        return '';
    }
  }

  createUser(): void {
    // Navegar para página de criação de usuário
    this.router.navigate(['/users/create']);
  }

  viewUser(userId: string): void {
    // Navegar para página de detalhes do usuário
    this.router.navigate(['/users', userId]);
  }
}