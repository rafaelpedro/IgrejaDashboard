import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Pessoa } from '../../models/pessoa.model';
import { ActivatedRoute } from '@angular/router';
import { DashboardService } from '../dashboard/dashboard.service';
import { Router } from '@angular/router';
import { OnInit } from '@angular/core';
import { ToastComponent } from '../../shared/toast.component';
import { ToastService } from '../../shared/toast.service';

@Component({
  selector: 'app-formcreate',
  imports: [CommonModule, FormsModule, ToastComponent],
  templateUrl: './formcreate.html',
  styleUrl: './formcreate.scss',
})
export class Formcreate implements OnInit {

  pessoa: Pessoa = this.criarNovaPessoa();
  editando: Boolean = false;

  constructor(
    private route: ActivatedRoute, 
    private router: Router, 
    private dashboardservice: DashboardService,
    private toast: ToastService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');

    if(id) {
      this.editando = true;
      this.dashboardservice.getPessoas().subscribe(lista => {
        const encontrada = lista.find(p => p.codigo === Number(id));
        if(encontrada) {
          this.pessoa = {...encontrada}
        }
      })
    }
  }

  salvar(): void {
    if(this.editando) {
      this.dashboardservice.updatePessoa(this.pessoa.codigo, this.pessoa)
      .subscribe(() => this.router.navigate([ '/' ]));
      this.toast.show('success', 'Membro atualizado com sucesso!');
    } else {
      this.dashboardservice.addPessoa(this.pessoa)
      .subscribe(() => this.router.navigate([ '/' ]));
      this.toast.show('success', 'Membro cadastrado com sucesso!');
    }
  }

  criarNovaPessoa(): Pessoa {
    return {
      codigo: 0,
      nome: '',
      email: '',
      sexo: '',
      status: ''
    };
  }

  cancelar(): void {
  this.router.navigate(['/']);
}
}
