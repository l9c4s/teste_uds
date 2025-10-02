import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { UserService } from '../../../core/services/user.service';
import { CreateUserRequest } from '../../../shared/models/Users/user.model';
import { AccessLevel } from '../../../shared/enums/AccessLevel';

@Component({
  selector: 'app-user-create',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule
  ],
  templateUrl: './user-create.component.html',
  styleUrls: ['./user-create.component.scss']
})
export class UserCreateComponent {
  
  userForm: FormGroup;
  isLoading = false;
  hidePassword = true;
 apiError: string = '';


  accessLevels = [
    { value: 1, label: 'Administrador', level: AccessLevel.ADMINISTRATOR },
    { value: 2, label: 'Gerente', level: AccessLevel.MANAGER },
    { value: 3, label: 'Usuário', level: AccessLevel.COMMON_USER },
    { value: 4, label: 'Visualizador', level: AccessLevel.VIEWER }
  ];

  constructor(
    private fb: FormBuilder,
    private userService: UserService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<UserCreateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.userForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(2)]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      accessLevel: ['', [Validators.required]]
    });
  }

  onSubmit(): void {
    if (this.userForm.valid && !this.isLoading) {
      this.isLoading = true;
      this.apiError = ''; // Limpa erros anteriores
      
      // Converter o valor numérico de volta para o enum antes de enviar
      const formData = this.userForm.value;
      const selectedLevel = this.accessLevels.find(level => level.value === formData.accessLevel);
      
      const userData: CreateUserRequest = {
        ...formData,
      };

      this.userService.createUser(userData).subscribe({
        next: (response) => {
          this.snackBar.open('Usuário criado com sucesso!', 'Fechar', {
            duration: 3000,
            panelClass: ['success-snackbar'],
            verticalPosition: 'top' // Posiciona no topo
          });
          this.dialogRef.close(true);
        },
        error: (error) => {
          this.isLoading = false;
          
          // Extrair mensagem de erro da API
          let errorMessage = 'Erro ao criar usuário';
          
          if (error?.error?.message) {
            errorMessage = error.error.message;
          } else if (error?.error?.errors) {
            // Se houver múltiplos erros de validação
            errorMessage = Object.values(error.error.errors).join(', ');
          } else if (error?.message) {
            errorMessage = error.message;
          }
          
          this.apiError = errorMessage; // Mostra no componente
        }
      });
    }
  }

  onCancel(): void {
    this.dialogRef.close(false);
  }

  togglePasswordVisibility(): void {
    this.hidePassword = !this.hidePassword;
  }

  onFormChange(): void {
    if (this.apiError) {
      this.apiError = '';
    }
  }

  getFieldErrorMessage(fieldName: string): string {
    const field = this.userForm.get(fieldName);
    if (field?.hasError('required')) {
      return `${this.getFieldLabel(fieldName)} é obrigatório`;
    }
    if (field?.hasError('email')) {
      return 'E-mail deve ter um formato válido';
    }
    if (field?.hasError('minlength')) {
      const minLength = field.errors?.['minlength']?.requiredLength;
      return `${this.getFieldLabel(fieldName)} deve ter pelo menos ${minLength} caracteres`;
    }
    return '';
  }

  private getFieldLabel(fieldName: string): string {
    const labels: { [key: string]: string } = {
      name: 'Nome',
      email: 'E-mail',
      password: 'Senha',
      accessLevel: 'Nível de acesso'
    };
    return labels[fieldName] || fieldName;
  }
}