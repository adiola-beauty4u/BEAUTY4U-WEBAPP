import { Component, Input, OnInit, OnChanges, SimpleChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TableComponent } from '../table/table.component';
import { TableData, ColumnDef, RowData, CellData } from 'src/interfaces/table-data';

@Component({
  selector: 'app-multi-table',
  imports: [FormsModule, CommonModule, TableComponent],
  templateUrl: './multi-table.component.html',
  styleUrl: './multi-table.component.scss'
})
export class MultiTableComponent implements OnInit, OnChanges {

  @Input() tableData: TableData;
  @Input() pageSize = 100;
  @Input() methodList: { [key: string]: (row: any) => void } = {};
  @Input() showInvalidCount = false;
  @Input() enableRowTransfer = false;
  @Input() enableRowDelete = false;

  hideAll: boolean = false;

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['tableData']) {

    }
  }

  ngOnInit(): void {

  }

  toggleTables() {
    this.hideAll = !this.hideAll;
  }

}
