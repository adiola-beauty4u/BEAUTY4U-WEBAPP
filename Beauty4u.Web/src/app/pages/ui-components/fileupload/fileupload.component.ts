import { Component, Input, ViewChild, ElementRef } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressBarModule } from '@angular/material/progress-bar';

@Component({
  selector: 'app-fileupload',
  imports: [
    CommonModule,
    HttpClientModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatIconModule,
    MatProgressBarModule],
  templateUrl: './fileupload.component.html',
  styleUrl: './fileupload.component.scss'
})
export class FileuploadComponent {
  selectedFile: File | null = null;
  uploadSuccess = false;
  uploadError = false;

  @ViewChild('fileInput') fileInputRef!: ElementRef<HTMLInputElement>;

  @Input() onUpload!: (file: File | null) => void;
  @Input() title: string = 'Upload File';
  @Input() templateName: string = '';
  @Input() templatePath: string = '';

  constructor(private http: HttpClient) { }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    this.selectedFile = input.files?.[0] || null;
    this.uploadSuccess = false;
    this.uploadError = false;
  }

  triggerUpload() {
    if (this.onUpload) {
      this.onUpload(this.selectedFile);
    }
  }

  getSelectedFile(): File | null {
    return this.selectedFile;
  }

  uploadFile(): void {
    if (!this.selectedFile) return;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    this.http.post('https://localhost:7002/api/v1/DataCheck/read-csv', formData).subscribe({
      next: () => (this.uploadSuccess = true),
      error: () => (this.uploadError = true),
    });
  }

  clear(): void {
    this.selectedFile = null;

    if (this.fileInputRef?.nativeElement) {
      this.fileInputRef.nativeElement.value = '';
    }
  }

  downloadTemplate() {
    const link = document.createElement('a');
    link.href = this.templatePath;
    link.download = this.templateName;
    link.click();
  }
}
