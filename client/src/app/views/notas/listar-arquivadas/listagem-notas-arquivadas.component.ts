import { NgIf, NgForOf, AsyncPipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { RouterLink } from '@angular/router';
import { finalize, Observable, of, switchMap } from 'rxjs';
import { ListarNotaViewModel } from '../models/nota.models';
import { NotaService } from '../services/nota.service';
import { NotificacaoService } from '../../../core/notificacao/notificacao.service';
import { ListagemCategoria } from '../../categorias/models/categoria.models';
import { CategoriaService } from '../../categorias/services/categoria.service';
import { FiltroCategoriasComponent } from '../shared/filtro-categorias.component';

@Component({
  selector: 'app-listar-arquivadas',
  standalone: true,
  imports: [
    NgIf,
    NgForOf,
    RouterLink,
    AsyncPipe,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatChipsModule,
    FiltroCategoriasComponent,
  ],
  templateUrl: './listagem-notas-arquivadas.component.html',
  styleUrl: './listagem-notas-arquivadas.component.scss',
})
export class ListarNotasArquivadasComponent implements OnInit {
  notas$?: Observable<ListarNotaViewModel[]>;
  categorias$?: Observable<ListagemCategoria[]>;

  notasEmCache: ListarNotaViewModel[];

  constructor(
    private notaService: NotaService,
    private categoriaService: CategoriaService,
    private notificacao: NotificacaoService
  ) {
    this.notasEmCache = [];
  }

  ngOnInit(): void {
    this.categorias$ = this.categoriaService.selecionarTodos();

    this.notaService.selecionarArquivadas().subscribe((notas) => {
      this.notasEmCache = notas;

      this.notas$ = of(notas);
    });
  }

  desarquivar(nota: ListarNotaViewModel) {
    nota.arquivada = false;

    this.notas$ = this.notaService.alterarStatus(nota.id).pipe(
      finalize(() =>
        this.notificacao.sucesso('A nota foi desarquivada com sucesso!')
      ),
      switchMap(() => this.notaService.selecionarArquivadas())
    );
  }

  filtrar(categoriaId?: string) {
    const notasFiltradas = this.obterNotasFiltradas(
      this.notasEmCache,
      categoriaId
    );

    this.notas$ = of(notasFiltradas);
  }

  private obterNotasFiltradas(notas: ListarNotaViewModel[], categoriaId?: string) {
    if (categoriaId) {
      return notas.filter((n) => n.categoria.id == categoriaId);
    }

    return notas;
  }
}
