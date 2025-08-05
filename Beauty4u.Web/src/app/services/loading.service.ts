import { Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { LoadingOverlayComponent } from '../pages/ui-components/loading-overlay/loading-overlay.component';

@Injectable({ providedIn: 'root' })
export class LoadingService {
  private dialogRef: MatDialogRef<LoadingOverlayComponent> | null = null;

  constructor(private dialog: MatDialog) { }

  show(message?: string, disableClose: boolean = true): void {
    if (this.dialogRef) return;
    this.dialogRef = this.dialog.open(LoadingOverlayComponent, {
      disableClose: disableClose,
      panelClass: 'transparent-dialog',
      backdropClass: 'transparent-backdrop',
      data: { message }
    });

    // Optionally prevent body scrolling while loading
    document.body.style.overflow = 'hidden';
  }
  
  hide(): void {
    this.dialogRef?.close();
    this.dialogRef = null;
    document.body.style.overflow = 'auto';
  }
}
