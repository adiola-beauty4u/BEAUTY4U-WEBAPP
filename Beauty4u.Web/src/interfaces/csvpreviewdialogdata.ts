export interface CsvPreviewDialogData {
  rows: any[];
  onProceed: (rows: any[]) => void;
  onClose?: () => void;
}
