import { Component, Inject, Input } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { CommonModule } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatButton } from '@angular/material/button';
import { MatProgressSpinner } from '@angular/material/progress-spinner';

export interface PreviewDialogData {
  data: any[];
  onProceed: (rows: any[], callback: any) => void;
  title?: string;
  showProceed: boolean;
  previewMessage?: string;
}

@Component({
  selector: 'app-csv-preview-dialog',
  imports: [CommonModule, MatDialogModule, MatTableModule, MatIcon, MatButton, MatProgressSpinner],
  templateUrl: './preview-dialog.component.html',
  styleUrl: './preview-dialog.component.scss'
})

export class PreviewDialogComponent {
  columns: string[];
  isLoading = false;
  disableProceed = false;

  constructor(@Inject(MAT_DIALOG_DATA) public data: PreviewDialogData,
    private dialogRef: MatDialogRef<PreviewDialogComponent>) {
    this.columns = data.data.length ? Object.keys(data.data[0]) : [];
  }

  proceed(): void {
    if (!this.data?.onProceed) return;

    this.isLoading = true;
    const result: any = this.data.onProceed(this.data.data, this);

    if (result instanceof Promise) {
      result.finally(() => {
        this.isLoading = false;
        this.disableProceed = true;
      });
    } else {
      this.isLoading = false;
    }
  }

  close(): void {
    this.dialogRef.close();
  }

  setData(rows: [], previewMessage: string, showProceed: boolean = false) {
    this.data.showProceed = showProceed;
    this.data.previewMessage = previewMessage;
  }

}
