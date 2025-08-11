import { Injectable, TemplateRef, inject } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { ModalContainerComponent } from '../components/modal-container/modal-container.component';

@Injectable({
  providedIn: 'root'
})
export class ModalService {
  private dialog = inject(MatDialog);

  openModal(
    title: string,
    content: string | TemplateRef<any>,
    context: any = {}
  ): MatDialogRef<ModalContainerComponent> {
    return this.dialog.open(ModalContainerComponent, {
      data: { title, content, context },
      width: '90vw',
      height: '90vh',
      maxWidth: '90vw',
      maxHeight: '90vh',
    });
  }
}