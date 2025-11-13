import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export interface ToastMessage {
  type: 'success' | 'error' | 'warning' | 'info';
  text: string;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastSubject = new BehaviorSubject<ToastMessage | null>(null);
  toastState$ = this.toastSubject.asObservable();

  show(type: ToastMessage['type'], text: string) {
    this.toastSubject.next({ type, text });

    setTimeout(() => this.clear(), 3000);
  }

  clear() {
    this.toastSubject.next(null);
  }
}
