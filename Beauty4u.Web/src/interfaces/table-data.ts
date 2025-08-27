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
    additionalData? : any;
}

export interface ColumnDef {
    fieldName: string;
    header: string;
    isCommand: boolean;
    commandName: string;
    isSlideInColumn: boolean;
    slideInCommand: string;
    slideInTitle: string;
    isHidden: boolean;
    dataType: number;
}

export interface TableData {
    tableName: string;
    columns: ColumnDef[];
    rows: RowData[];
    tableGroups: TableData[];
}