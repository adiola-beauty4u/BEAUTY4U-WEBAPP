import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  AbstractControl,
  ValidationErrors,
  ReactiveFormsModule
} from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatLabel } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatError } from '@angular/material/form-field';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { AuthService } from 'src/app/services/authentication/auth.service';
import { LoadingService } from 'src/app/services/loading.service';

@Component({
  selector: 'app-resetpassword',
  imports: [CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    MatSnackBarModule,
    MatLabel,
    MatError],
  templateUrl: './resetpassword.component.html',
  styleUrl: './resetpassword.component.scss'
})
export class ResetpasswordComponent {
  form: FormGroup;
  private snackBar = inject(MatSnackBar)

  constructor(private fb: FormBuilder, private router: Router, private authService: AuthService, private loadingService: LoadingService) {
    this.form = this.fb.group({
      currentPassword: ['', [Validators.required]],
      newPassword: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]],
    }, {
      validators: [this.passwordsMatchValidator]
    });
  }

  get f() {
    return this.form.controls;
  }

  passwordsMatchValidator(control: AbstractControl): ValidationErrors | null {
    const newPassword = control.get('newPassword')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    if (newPassword && confirmPassword && newPassword !== confirmPassword) {
      return { passwordsMismatch: true };
    }
    return null;
  }

  changePassword(): void {
    if (this.form.valid) {
      this.loadingService.show("Changing password...");
      const { currentPassword, newPassword } = this.form.value;
      const payload = {
        currentPassword,
        newPassword
      };
      // Replace with your actual API call
      this.authService.resetPassword(payload).subscribe({
        next: (data) => {
          this.snackBar.open(data.message, 'Close', {
            duration: 3000,
            panelClass: ['snackbar-success']
          });
          this.form.reset();
          this.loadingService.hide();

        },
        error: (error: HttpErrorResponse) => {
          let message = 'Something went wrong';
          this.loadingService.hide();
          if (error.status === 400 && error.error?.message) {
            message = error.error.message;
          }
          this.snackBar.open(message, 'Close', {
            duration: 3000,
            panelClass: ['snackbar-error']
          });
        }
      });
    } else {
      this.form.markAllAsTouched();
    }
  }
}
