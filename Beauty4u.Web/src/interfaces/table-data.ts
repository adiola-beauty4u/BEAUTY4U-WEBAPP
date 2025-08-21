export interface CellData {
    textValue: string;
    rawValue: any;
    tooltip?: string;
    cssClass?: string;
    cssIcon?: string;
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
}

export interface TableGroup {
    tableName: string;
    columns: ColumnDef[];
    rows: RowData[];
}