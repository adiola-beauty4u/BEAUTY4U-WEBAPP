import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { RouterModule } from '@angular/router';
import { MaterialModule } from 'src/app/material.module';
import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { AuthService } from 'src/app/services/authentication/auth.service';
import { LoadingService } from 'src/app/services/loading.service';

@Component({
  selector: 'app-side-login',
  imports: [RouterModule, MaterialModule, FormsModule, ReactiveFormsModule],
  templateUrl: './side-login.component.html',
})
export class AppSideLoginComponent {
  constructor(private router: Router, private authService: AuthService, private loadingService: LoadingService) { }

  form = new FormGroup({
    uname: new FormControl('', [Validators.required, Validators.minLength(3)]),
    password: new FormControl('', [Validators.required]),
    rememberDevice: new FormControl(false),
  });

  loginMessage: string = '';

  get f() {
    return this.form.controls;
  }

  submit() {

    if (this.form.invalid) return;
    this.loginMessage = '';
    const uname = this.form.value.uname!;
    const password = this.form.value.password!;
    const remember = this.form.value.rememberDevice;
    this.loadingService.show("Signing in...");
    this.authService.login({ username: uname, password }).subscribe({
      next: (res) => {
        // Handle success (e.g., store token, redirect)
        this.router.navigate(['']);
        this.loadingService.hide();
      },
      error: (err) => {
        // Handle error
        this.loginMessage = 'Invalid username or password';
        this.loadingService.hide();
      }
    });
  }
}
