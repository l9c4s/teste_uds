import { Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';
import { ManagerGuard } from './core/guards/manager.guard';

export const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { 
    path: 'login', 
    loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
  },
  { 
    path: 'register', 
    loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
  },
  { 
    path: 'users', 
    loadComponent: () => import('./features/users/user-list/user-list.component').then(m => m.UserListComponent),
    canActivate: [AuthGuard]
  },
  // TODO: Implementar os seguintes componentes
  // {
  //   path: 'users/create',
  //   loadComponent: () => import('./features/users/user-create/user-create.component').then(m => m.UserCreateComponent),
  //   canActivate: [ManagerGuard]
  // },
  // {
  //   path: 'users/:id',
  //   loadComponent: () => import('./features/users/user-detail/user-detail.component').then(m => m.UserDetailComponent),
  //   canActivate: [AuthGuard]
  // },
  // {
  //   path: 'users/:id/edit',
  //   loadComponent: () => import('./features/users/user-edit/user-edit.component').then(m => m.UserEditComponent),
  //   canActivate: [ManagerGuard]
  // },
  // {
  //   path: 'admin',
  //   loadComponent: () => import('./features/admin/admin-panel/admin-panel.component').then(m => m.AdminPanelComponent),
  //   canActivate: [AdminGuard]
  // },
  { 
    path: 'unauthorized', 
    loadComponent: () => import('./shared/components/unauthorized/unauthorized.component').then(m => m.UnauthorizedComponent)
  },
  { path: '**', redirectTo: '/login' }
];