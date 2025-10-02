import { Directive, Input, TemplateRef, ViewContainerRef, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from '../../core/services/auth.service';
import { Permission} from '../models/access-control.model';
import { AccessLevel } from '../enums/AccessLevel';

@Directive({
  selector: '[appHasPermission]',
  standalone: true
})
export class HasPermissionDirective implements OnInit, OnDestroy {
  private subscription = new Subscription();
  private currentPermission: keyof Permission | null = null;
  private currentMinRole: AccessLevel = AccessLevel.MANAGER;

  @Input() set appHasPermission(permission: keyof Permission) {
    this.currentPermission = permission;
    this.updateView();
  }

  @Input() set appHasMinRole(role: AccessLevel) {
    this.currentMinRole = role;
    this.updateView();
  }

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    // Observar mudanças no usuário atual, canhao faca da cintura 
    this.subscription.add(
      this.authService.currentUser$.subscribe(() => {
        this.updateView();
      })
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private updateView(): void {
    this.viewContainer.clear();

    let hasAccess = false;

    // Verificar por permissão específica
    if (this.currentPermission) {
      hasAccess = this.authService.minimumPermission(this.currentMinRole);
    }
    
    if (hasAccess) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    }
  }
}