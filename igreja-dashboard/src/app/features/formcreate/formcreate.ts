import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormGroup, FormsModule, Validators, ReactiveFormsModule, FormControl } from '@angular/forms';
import { DashboardService } from '../dashboard/dashboard.service';
import { Router, ActivatedRoute} from '@angular/router';
import { ToastService } from '../../shared/toast.service';
import { FormBuilder } from '@angular/forms';
import { ToastComponent } from '../../shared/toast.component';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-formcreate',
  imports: [CommonModule, FormsModule, ToastComponent, ReactiveFormsModule, RouterModule],
  templateUrl: './formcreate.html',
  styleUrl: './formcreate.scss',
})
export class Formcreate implements OnInit{

    form!: FormGroup;
    editando: Boolean = false;
    id!: number;

    constructor(
      private fb: FormBuilder,
      private router: Router,
      private route: ActivatedRoute,
      private dashboardService: DashboardService,
      private toast: ToastService
    ){
    }

    ngOnInit(): void {

      // this.form = new FormGroup({
      //   nome: new FormControl(null),
      //   email: new FormControl(null)
      // })

      this.form = this.fb.group({
      nome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      sexo: ['', Validators.required],
      status: ['', Validators.required]});

      const idParam = this.route.snapshot.paramMap.get('id');
      if(idParam){
        this.editando = true;
        this.id = Number(idParam);
        this.carregarPessoa();
      }
    }

    carregarPessoa(){
      this.dashboardService.getPessoas().subscribe(pessoas => { 
        const pessoa = pessoas.find(p => p.codigo === this.id);
        if(pessoa){
          this.form.patchValue(pessoa);
        }
      })
    }

    salvar(): void {
      if (this.form?.invalid) {
        return;        
      }

      const dados = this.form.value

      if(this.editando){
        this.dashboardService.updatePessoa(this.id, dados).subscribe({
          next: () => {
            this.toast.show('success', 'Membro atualizado com sucesso');
            this.router.navigate(['/']);
          },
          error: () => { 
            this.toast.show('error', 'Erro ao atualizar membro. Tente novamente.')
          }
        })
      } else {  
        this.dashboardService.addPessoa(this.form?.value).subscribe({
          next: () => {
            this.toast.show('success', 'Membro cadastrado com sucesso!');
            this.router.navigate(['/']);
          },
          error: () => {
            this.toast.show('error', 'Erro ao cadastrar membro. Tente novamente.');
          }
        }); 
      }
    }

    cancelar(): void {
      this.router.navigate(['/']);
    }

    verificaErro(campo: string){
      const c = this.form.get(campo);
      return c?.invalid && (c.touched || c.dirty)
    }
}
