import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';
import { NoopAnimationsModule } from '@angular/platform-browser/animations';
import { of, throwError } from 'rxjs';
import { By } from '@angular/platform-browser';

import { LoginComponent } from './login.component';
import { AuthService } from '../../../core/services/auth.service';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let mockAuthService: jasmine.SpyObj<AuthService>;
  let mockRouter: jasmine.SpyObj<Router>;
  let mockSnackBar: jasmine.SpyObj<MatSnackBar>;

  const mockLoginResponse = {
    token: 'fake-jwt-token',
    user: {
      id: '123',
      name: 'Test User',
      email: 'test@test.com',
      accessLevel: 'Administrator'
    }
  };

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    const snackBarSpy = jasmine.createSpyObj('MatSnackBar', ['open']);

    await TestBed.configureTestingModule({
      imports: [
        LoginComponent,
        ReactiveFormsModule,
        NoopAnimationsModule
      ],
      providers: [
        FormBuilder,
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: MatSnackBar, useValue: snackBarSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    mockAuthService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;
    mockSnackBar = TestBed.inject(MatSnackBar) as jasmine.SpyObj<MatSnackBar>;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize form with empty values and validators', () => {
    expect(component.loginForm.get('email')?.value).toBe('');
    expect(component.loginForm.get('password')?.value).toBe('');
    expect(component.loginForm.get('email')?.hasError('required')).toBeTruthy();
    expect(component.loginForm.get('password')?.hasError('required')).toBeTruthy();
  });

  it('should validate email format', () => {
    const emailControl = component.loginForm.get('email');
    
    emailControl?.setValue('invalid-email');
    expect(emailControl?.hasError('email')).toBeTruthy();
    
    emailControl?.setValue('valid@email.com');
    expect(emailControl?.hasError('email')).toBeFalsy();
  });

  it('should validate password minimum length', () => {
    const passwordControl = component.loginForm.get('password');
    
    passwordControl?.setValue('123');
    expect(passwordControl?.hasError('minlength')).toBeTruthy();
    
    passwordControl?.setValue('123456');
    expect(passwordControl?.hasError('minlength')).toBeFalsy();
  });

  it('should disable submit button when form is invalid', () => {
    const submitButton = fixture.debugElement.query(By.css('button[type="submit"]'));
    
    expect(submitButton.nativeElement.disabled).toBeTruthy();
    
    component.loginForm.patchValue({
      email: 'test@test.com',
      password: 'password123'
    });
    fixture.detectChanges();
    
    expect(submitButton.nativeElement.disabled).toBeFalsy();
  });

  it('should call AuthService.login on valid form submit', () => {
    mockAuthService.login.and.returnValue(of(mockLoginResponse));

    component.loginForm.patchValue({
      email: 'test@test.com',
      password: 'password123'
    });

    component.onSubmit();

    expect(mockAuthService.login).toHaveBeenCalledWith({
      email: 'test@test.com',
      password: 'password123'
    });
  });

  it('should navigate to users page on successful login', () => {
    mockAuthService.login.and.returnValue(of(mockLoginResponse));

    component.loginForm.patchValue({
      email: 'test@test.com',
      password: 'password123'
    });

    component.onSubmit();

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/users']);
    expect(mockSnackBar.open).toHaveBeenCalledWith('Login realizado com sucesso!', 'Fechar', {
      duration: 3000
    });
  });

  it('should show error message on login failure', () => {
    const errorResponse = { status: 401, message: 'Unauthorized' };
    mockAuthService.login.and.returnValue(throwError(() => errorResponse));

    component.loginForm.patchValue({
      email: 'test@test.com',
      password: 'wrongpassword'
    });

    component.onSubmit();

    expect(mockSnackBar.open).toHaveBeenCalledWith(
      'Erro ao fazer login. Verifique suas credenciais.',
      'Fechar',
      { duration: 5000 }
    );
    expect(component.isLoading).toBeFalse();
  });

  it('should set loading state during login', () => {
    mockAuthService.login.and.returnValue(of(mockLoginResponse));

    component.loginForm.patchValue({
      email: 'test@test.com',
      password: 'password123'
    });

    expect(component.isLoading).toBeFalse();

    component.onSubmit();

    expect(component.isLoading).toBeFalse(); // Reset after completion
  });

  it('should navigate to register page', () => {
    component.goToRegister();
    
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/register']);
  });

  it('should not submit when form is invalid', () => {
    component.loginForm.patchValue({
      email: 'invalid-email',
      password: '123'
    });

    component.onSubmit();

    expect(mockAuthService.login).not.toHaveBeenCalled();
  });
});