import { Component, Inject, inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { TemplateRef } from '@angular/core';

@Component({
  selector: 'app-modal-container',
  imports: [CommonModule, MatDialogModule],
  templateUrl: './modal-container.component.html',
  styleUrl: './modal-container.component.scss'
})

export class ModalContainerComponent {
  dialogRef = inject(MatDialogRef<ModalContainerComponent>);
  constructor(@Inject(MAT_DIALOG_DATA) public data: { title: string, content: string | TemplateRef<any>, context?: any }) { }

  isString(value: any): value is string {
    return typeof value === 'string';
  }

  close() {
    this.dialogRef.close();
  }
}