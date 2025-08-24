export interface CellData {
    textValue: string;
    rawValue: any;
    tooltip?: string;
    cssClass?: string;
    cssIcon?: string;
    commandParameter?: any;
    slideInCommandParameter?: any;
}

export interface RowData {
    tooltip?: string;
    cssClass?: string;
    rowKey?: string;
    cells: { [key: string]: CellData };
}

export interface ColumnDef {
    fieldName: string;
    header: string;
    isCommand: boolean;
    commandName: string;
    isSlideInColumn: boolean;
    slideInCommand: string;
    slideInTitle: string;
}

export interface TableData {
    tableName: string;
    columns: ColumnDef[];
    rows: RowData[];
    tableGroups: TableData[];
}