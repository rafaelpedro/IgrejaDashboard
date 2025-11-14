import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard.service';
import { Pessoa } from '../../models/pessoa.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ToastComponent } from '../../shared/toast.component';
import { ToastService } from '../../shared/toast.service';
@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, FormsModule, ToastComponent],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.scss']
})
export class DashboardComponent implements OnInit {
   pessoas: Pessoa[] = [];
  totais = { total: 0, masculinos: 0, femininos: 0 };
  busca = '';

  mostrarModal = false;
  editando = false;
  pessoaAtual: Pessoa = this.criarNovaPessoa();

  mostrarConfirmacao = false;
  idParaExcluir: number | null = null;
  nomeParaExcluir: string = '';

  constructor(private dashboardService: DashboardService, private toast: ToastService) {}

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

  abrirModal(): void {
    this.mostrarModal = true;
    this.editando = false;
    this.pessoaAtual = this.criarNovaPessoa();
  }

  fecharModal(): void {
    this.mostrarModal = false;
  }

  editarPessoa(pessoa: Pessoa): void {
    this.pessoaAtual = { ...pessoa };
    this.mostrarModal = true;
    this.editando = true;
  }

  salvarPessoa(): void {
  if (this.editando) {
    this.dashboardService.updatePessoa(this.pessoaAtual.codigo, this.pessoaAtual).subscribe({
      next: () => {
        this.fecharModal();
        this.carregarDados();
        this.toast.show('success', 'Membro atualizado com sucesso!');
      },
      error: () => {
        this.toast.show('error', 'Erro ao atualizar o membro.');
      }
    });
  } else {
    this.dashboardService.addPessoa(this.pessoaAtual).subscribe({
      next: () => {
        this.fecharModal();
        this.carregarDados();
        this.toast.show('success', 'Membro cadastrado com sucesso!');
      },
      error: () => {
        this.toast.show('error', 'Erro ao cadastrar novo membro.');
      }
    });
  }
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
    if(this.idParaExcluir){
      this.toast.show('success', `Membro ${this.nomeParaExcluir} excluído com sucesso!`);
    }
    this.fecharConfirmacao();
  });
}

  private criarNovaPessoa(): Pessoa {
    return { codigo: 0, nome: '', email: '', sexo: '', status: '' };
  }
}
