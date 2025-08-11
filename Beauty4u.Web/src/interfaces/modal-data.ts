import { TemplateRef } from '@angular/core';
export interface ModalData {
  template?: TemplateRef<any>;
  component?: any;
  context?: { [key: string]: any }; // key part here
  title?: string;
}
