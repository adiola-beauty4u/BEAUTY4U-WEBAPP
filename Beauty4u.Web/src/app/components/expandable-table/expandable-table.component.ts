import { Component, Input, signal, computed, effect, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { PagesRoutes } from 'src/app/pages/pages.routes';
import { ChildTableComponent } from '../child-table/child-table.component';

interface CellData {
  textValue: string;
  rawValue: any;
  tooltip?: string;
  cssClass?: string;
  cssIcon?: string;
}

interface RowData {
  tooltip?: string;
  cssClass?: string;
  cells: { [key: string]: CellData };
}

interface ColumnDef {
  fieldName: string;
  header: string;
}

interface TableGroup {
  tableName: string;
  columns: ColumnDef[];
  rows: RowData[];
}

@Component({
  selector: 'app-expandable-table',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatTooltipModule,
    MatPaginatorModule,
    ChildTableComponent
  ],
  templateUrl: './expandable-table.component.html',
  styleUrls: ['./expandable-table.component.scss']
})
export class ExpandableTableComponent implements OnInit, OnChanges {
  @Input() tableGroups: TableGroup[] = [];
  @Input() showInvalidCount = true;

  filters = signal<string[]>([]);
  expandedGroups = signal<boolean[]>([]);
  pageIndexes = signal<number[]>([]);
  pageSizes = signal<number[]>([]);
  readonly defaultPageSize = 10;

  ngOnInit() {
  }

  ngOnChanges() {
    const count = this.tableGroups.length;
    this.filters.set(Array(count).fill(''));
    this.expandedGroups.set(Array(count).fill(true));
    this.pageIndexes.set(Array(count).fill(0));
    this.pageSizes.set(Array(count).fill(this.defaultPageSize));
  }


  toggleAll() {
    const allExpanded = this.allExpanded();
    this.expandedGroups.update(() => this.expandedGroups().map(() => !allExpanded));
  }

  toggleGroup(i: number) {
    this.expandedGroups.update(groups => {
      groups[i] = !groups[i];
      return [...groups];
    });
  }

  allExpanded(): boolean {
    return this.expandedGroups().every(Boolean);
  }

  getDisplayedColumns(group: TableGroup): string[] {
    return group.columns.map(c => c.fieldName);
  }

  applyFilter(i: number) {
    this.pageIndexes.update(pages => {
      pages[i] = 0;
      return [...pages];
    });
  }

  getFilteredData(i: number): RowData[] {
    const filter = this.filters()[i]?.toLowerCase() || '';
    const rows = this.tableGroups[i].rows;
    return !filter
      ? rows
      : rows.filter(row =>
        Object.values(row.cells).some(cell =>
          (cell.textValue || '').toLowerCase().includes(filter)
        )
      );
  }

  getPagedRows(i: number): RowData[] {
    const pageSize = this.pageSizes()[i] ?? this.defaultPageSize;
    const pageIndex = this.pageIndexes()[i] ?? 0;
    const start = pageIndex * pageSize;
    return this.getFilteredData(i).slice(start, start + pageSize);
  }

  onPage(event: PageEvent, i: number) {
    this.pageIndexes.update(pages => {
      pages[i] = event.pageIndex;
      return [...pages];
    });
    this.pageSizes.update(sizes => {
      sizes[i] = event.pageSize;
      return [...sizes];
    });
  }

  getTotalRowCount(i: number): number {
    return this.tableGroups[i].rows.length;
  }

  getInvalidRowCount(i: number): number {
    return this.tableGroups[i].rows.filter(r => r.cssClass?.includes('invalid')).length;
  }
}