import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastService, ToastMessage } from './toast.service';

@Component({
  selector: 'app-toast',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="toast-container position-fixed bottom-0 start-50 translate-middle-x p-3" style="z-index: 1100;">
      <div *ngIf="toast"
        class="toast show text-white"
        [ngClass]="{
          'bg-success': toast.type === 'success',
          'bg-danger': toast.type === 'error',
          'bg-warning text-dark': toast.type === 'warning',
          'bg-info': toast.type === 'info'
        }"
        role="alert">
        <div class="d-flex">
          <div class="toast-body">
            {{ toast.text }}
          </div>
          <button class="btn-close btn-close-white me-2 m-auto"
                  (click)="close()"></button>
        </div>
      </div>
    </div>
  `,
})
export class ToastComponent implements OnInit {
  toast: ToastMessage | null = null;

  constructor(private toastService: ToastService) {}

  ngOnInit(): void {
    this.toastService.toastState$.subscribe(t => this.toast = t);
  }

  close() {
    this.toastService.clear();
  }
}
