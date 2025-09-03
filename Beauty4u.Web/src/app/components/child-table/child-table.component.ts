import { Component, Input, Output, EventEmitter, OnInit, OnChanges, SimpleChanges, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatSort, MatSortModule, Sort } from '@angular/material/sort';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { RowData } from 'src/interfaces/table-data';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { SelectionModel } from '@angular/cdk/collections';
import { MatCheckbox } from '@angular/material/checkbox';

@Component({
  selector: 'app-child-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatSortModule,
    MatIconModule,
    MatTooltipModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatCheckbox
  ],
  templateUrl: './child-table.component.html',
  styleUrl: './child-table.component.scss',
})
export class ChildTableComponent implements OnInit, OnChanges {
  @Input() group: any;
  @Input() displayedColumns: string[] = [];
  @Input() pageSize = 5;
  @Input() pageIndex = 0;
  @Input() showInvalidCount = false;
  @Input() methodList: { [key: string]: (row: any) => void } = {};
  @Input() enableRowSelection = false;
  @Input() enableRowDelete = false;
  @Output() page = new EventEmitter<PageEvent>();
  @Output() selectedChange = new EventEmitter<any[]>();
  @Output() rowDeleted = new EventEmitter<any>();

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  allTableData: any[] = [];
  filteredData: any[] = [];
  pagedData: any[] = [];
  searchText = '';
  selection = new SelectionModel<any>(true, []);

  hideTable = false;

  ngOnInit() {
    const allData = this.group?.rows || [];
    this.allTableData = [...allData];

    this.applySearch();
    this.selection.changed.subscribe(() => {
      this.selectedChange.emit(this.selection.selected);
    });
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['group']) {
      this.selection.clear();
      this.applySearch();
    }
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.allTableData.length;
    return numSelected === numRows;
  }

  toggleRow(row: any) {
    this.selection.toggle(row);
  }

  // For UI check if row is selected
  isRowSelected(row: any): boolean {
    return this.selection.isSelected(row);
  }

  toggleAllRows() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }
    this.selection.select(...this.allTableData);
  }

  checkboxLabel(row?: any): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row`;
  }

  onDeleteRow(row: any) {
    const index = this.group?.rows?.indexOf(row);
    if (index >= 0) {
      this.group.rows.splice(index, 1);
      this.applySearch(); // refresh filteredData & pagedData
      this.rowDeleted.emit(row); // notify parent
    }
  }


  get tableColumns(): string[] {
    let cols = this.displayedColumns;
    if (this.enableRowSelection) {
      cols = ['select', ...cols];
    }
    if (this.enableRowDelete) {
      cols = [...cols, 'rowDelete'];
    }
    return cols;
  }

  toggleTable() {
    this.hideTable = !this.hideTable;
  }

  applySearch() {
    const allData = this.group?.rows || [];
    const lowerSearch = this.searchText.toLowerCase();

    this.filteredData = allData.filter((row: RowData) =>
      Object.values(row.cells).some((cell: any) =>
        (cell.textValue || '').toString().toLowerCase().includes(lowerSearch)
      )
    );
    this.pageIndex = 0;
    this.updatePagedData();
  }

  sortData(sort: Sort) {
    if (!sort.active || sort.direction === '') {
      return;
    }
    this.filteredData.sort((a, b) => {
      const valA = (a.cells[sort.active]?.textValue || '').toString().toLowerCase();
      const valB = (b.cells[sort.active]?.textValue || '').toString().toLowerCase();
      return (valA < valB ? -1 : valA > valB ? 1 : 0) * (sort.direction === 'asc' ? 1 : -1);
    });
    this.updatePagedData();
  }

  updatePagedData() {
    const start = this.pageIndex * this.pageSize;
    const end = start + this.pageSize;
    this.pagedData = this.filteredData.slice(start, end);
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.pageSize = event.pageSize;
    this.updatePagedData();
    this.page.emit(event);
  }

  refresh() {
    this.applySearch();
  }

  getTotalRowCount(): number {
    return this.group?.rows?.length || 0;
  }

  getInvalidRowCount(): number {
    return this.group?.rows?.filter((r: any) => r.isInvalid)?.length || 0;
  }

  clear(): void {
    this.searchText = '';
    if (this.group) {
      this.group.rows = [];
    }
    this.filteredData = [];
    this.pagedData = [];
    this.pageIndex = 0;
    this.updatePagedData();
  }

}
