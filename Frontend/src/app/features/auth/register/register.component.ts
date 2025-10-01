import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';
import { RegisterRequest } from '../../../shared/models/auth.model';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  registerForm: FormGroup;
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.registerForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    }, { validators: this.passwordMatchValidator });
  }

  passwordMatchValidator(control: AbstractControl): { [key: string]: any } | null {
    const password = control.get('password');
    const confirmPassword = control.get('confirmPassword');
    
    if (password && confirmPassword && password.value !== confirmPassword.value) {
      return { passwordMismatch: true };
    }
    return null;
  }

  onSubmit(): void {
    if (this.registerForm.valid) {
      this.isLoading = true;
      const userData: RegisterRequest = this.registerForm.value;

      this.authService.register(userData).subscribe({
        next: () => {
          this.snackBar.open('Cadastro realizado com sucesso! Faça login.', 'Fechar', {
            duration: 3000
          });
          this.router.navigate(['/login']);
        },
        error: (error) => {
          this.isLoading = false;
          this.snackBar.open('Erro ao criar conta. Tente novamente.', 'Fechar', {
            duration: 5000
          });
        },
        complete: () => {
          this.isLoading = false;
        }
      });
    }
  }

  goToLogin(): void {
    this.router.navigate(['/login']);
  }
}