import { Component } from '@angular/core';
import { LayoutComponent } from '../../shared/components/layout/layout.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [LayoutComponent],
  template: `<app-layout></app-layout>`
})
export class DashboardComponent {}