import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';
import { DashboardService } from '../features/dashboard/dashboard.service';
import { ToastService } from '../shared/toast.service';
import { catchError, map, of } from 'rxjs';

export const membroExisteGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const service = inject(DashboardService);
  const router = inject(Router);
  const toast = inject(ToastService);

  const id = Number(route.paramMap.get('id'));

  if (!id) {
    toast.show('error', 'ID inválido.');
    router.navigate(['/']);
    return of(false);
  }

  return service.getPessoa(id).pipe(
    map(pessoa => {
      if (pessoa) {
        return true;
      } else {
        toast.show('error', 'Membro não encontrado.');
        router.navigate(['/']);
        return false;
      }
    }),
    catchError(() => {
      toast.show('error', 'Erro ao buscar membro.');
      router.navigate(['/']);
      return of(false);
    })
  );
};
