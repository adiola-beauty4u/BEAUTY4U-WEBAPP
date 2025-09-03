import { Injectable, TemplateRef, inject } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ModalContainerComponent } from '../components/modal-container/modal-container.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private dialog = inject(MatDialog);
  private dialogRef?: MatDialogRef<ModalContainerComponent>;

  openModal(
    title: string,
    content: string | TemplateRef<any>,
    context: any = {}
  ): MatDialogRef<ModalContainerComponent> {

    this.dialogRef = this.dialog.open(ModalContainerComponent, {
      data: { title, content, context },
      maxWidth: '90vw',
      maxHeight: '90vh',
    });

    return this.dialogRef;
  }

  closeModal(result?: any): void {
    if (this.dialogRef) {
      this.dialogRef.close(result);
      this.dialogRef = undefined;
    }
  }
}