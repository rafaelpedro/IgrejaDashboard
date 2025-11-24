import { Routes } from '@angular/router';
import { DashboardComponent } from './features/dashboard/dashboard';
import { membroExisteGuard } from './guard/membro-existe.guard';

export const routes: Routes = [
  { path: '', component: DashboardComponent },
  {
    path: 'membros/novo',
    loadComponent: () => import('./features/formcreate/formcreate').then(m => m.Formcreate )
  },
  {
  path: 'membros/editar/:id',
  canActivate: [membroExisteGuard],
  loadComponent: () =>
    import('./features/formcreate/formcreate').then(m => m.Formcreate)
}

  // {
  //   path: 'membros/editar/:id',
  //   loadComponent: () => import('./features/formcreate/formcreate').then(m => m.Formcreate )
  // }
];
