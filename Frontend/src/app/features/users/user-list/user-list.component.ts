import { Component, OnInit, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginatorModule, MatPaginator } from '@angular/material/paginator';
import { MatSortModule, MatSort } from '@angular/material/sort';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialog } from '@angular/material/dialog';
import { UserService } from '../../../core/services/user.service';
import { AuthService } from '../../../core/services/auth.service';
import { User } from '../../../shared/models/Users/user.model';
import { AuthUser } from '../../../shared/models/Auth/auth.model';
import { HasPermissionDirective } from '../../../shared/directives/has-permission.directive';
import { AccessLevel } from '../../../shared/enums/AccessLevel';
import { UserCreateComponent } from '../Create/user-create.component';

@Component({
  selector: 'app-user-list',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatTooltipModule,
    HasPermissionDirective
  ],
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit {

  private _paginator!: MatPaginator;
  @ViewChild(MatPaginator) set paginator(paginator: MatPaginator) {
    this._paginator = paginator;
    if (this._paginator) {
      this.dataSource.paginator = this._paginator;
    }
  }

  private _sort!: MatSort;
  @ViewChild(MatSort) set sort(sort: MatSort) {
    this._sort = sort;
    if (this._sort) {
      this.dataSource.sort = this._sort;
    }
  }

  displayedColumns: string[] = ['avatar', 'name', 'accessLevel', 'createdAt'];
  dataSource = new MatTableDataSource<User>([]);

  accessLevels = Object.values(AccessLevel).map(level => ({
    value: level,
  }));


  isLoading = false;
  searchTerm = '';
  selectedRole = '';
  currentUser: AuthUser | null = null;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadUsers();
  }


  loadCurrentUser(): void {
    this.authService.currentUser$.subscribe(user => {
      this.currentUser = user;
    });
  }

  loadUsers(): void {
  this.isLoading = true;
  this.userService.getUsers().subscribe({
    next: (users) => {
      this.dataSource.data = users;
      this.isLoading = false;
    },
    error: (error) => {
      console.error('Erro ao carregar usuários:', error);
      this.isLoading = false;
    }
  });
}


  onSearch(): void {
    this.dataSource.filter = this.searchTerm.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }

  }

  onRoleFilter(): void {
    if (this.selectedRole) {
      this.dataSource.filterPredicate = (data: User, filter: string) => {
        return data.accessLevel.toLowerCase() === filter.toLowerCase();
      };
      this.dataSource.filter = this.selectedRole;
    } else {
      this.dataSource.filterPredicate = (data: User, filter: string) => {
        return data.name.toLowerCase().includes(filter) ||
          data.email.toLowerCase().includes(filter);
      };
      this.dataSource.filter = this.searchTerm.trim().toLowerCase();
    }
  }

  createUser(): void {
    const dialogRef = this.dialog.open(UserCreateComponent, {
      width: '500px',
      disableClose: true,
      panelClass: 'custom-dialog-container'
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.loadUsers(); // Recarrega a lista se o usuário foi criado
      }
    });
  }



  
  getRoleDisplayName(role: string): string {
    return role
  }
}