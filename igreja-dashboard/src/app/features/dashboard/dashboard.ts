import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { Pessoa } from '../../models/pessoa.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastComponent } from '../../shared/toast.component';
import { ToastService } from '../../shared/toast.service';
import { Router, RouterLink } from '@angular/router';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule, ToastComponent, RouterLink, RouterModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class DashboardComponent implements OnInit {
  pessoas: Pessoa[] = [];
  totais = { total: 0, masculinos: 0, femininos: 0 };
  busca = '';
  mostrarConfirmacao = false;
  idParaExcluir: number | null = null;
  nomeParaExcluir: string = '';

  constructor(
    private dashboardService: DashboardService,
    private toast: ToastService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.carregarDados();
  }

  carregarDados(): void {
    this.dashboardService.getDashboard().subscribe(d => this.totais = d);
    this.dashboardService.getPessoas().subscribe(p => this.pessoas = p);
  }

  buscar(): void {
    this.dashboardService.getPessoas(this.busca).subscribe(p => this.pessoas = p);
  }

  editarPessoa(p: Pessoa): void {
    this.router.navigate([`/membros/editar/${p.codigo}`]);
  }

  excluirPessoa(id: number, nome: string): void {
    this.mostrarConfirmacao = true;
    this.idParaExcluir = id;
    this.nomeParaExcluir = nome;
  }

  fecharConfirmacao(): void {
    this.mostrarConfirmacao = false;
    this.idParaExcluir = null;
    this.nomeParaExcluir = '';
  }

  confirmarExclusao(): void {
    if (!this.idParaExcluir) return;
    this.dashboardService.deletePessoa(this.idParaExcluir).subscribe(() => {
      this.carregarDados();
      if (this.idParaExcluir) {
        this.toast.show('success', `Membro ${this.nomeParaExcluir} excluído com sucesso!`);
      }
      this.fecharConfirmacao();
    });
  }

}
