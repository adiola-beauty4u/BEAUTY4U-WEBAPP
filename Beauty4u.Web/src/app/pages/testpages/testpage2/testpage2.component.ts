import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, Validators } from '@angular/forms';
import { DatepickerComponent } from 'src/app/components/datepicker/datepicker.component';

@Component({
  selector: 'app-testpage2',
  imports: [
    CommonModule, ReactiveFormsModule, DatepickerComponent
  ],
  templateUrl: './testpage2.component.html',
  styleUrl: './testpage2.component.scss'
})
export class Testpage2Component {
  protocols = ['Any', 'TCP', 'UDP', 'ICMPv4', 'ICMPv6'];

  ruleForm = this.fb.group({
    source: ['Any', Validators.required],
    sourcePortRanges: ['*', Validators.required],
    destination: ['Any', Validators.required],
    service: ['Custom', Validators.required],
    destinationPortRanges: ['8001', Validators.required],
    protocol: ['TCP', Validators.required],
    action: ['Allow', Validators.required],
    priority: [1000, Validators.required],
    name: ['AllowAnyB4UWebInbound', Validators.required],
    description: ['']
  });

  constructor(private fb: FormBuilder) { }

  onSave() {
    if (this.ruleForm.valid) {
      console.log('Rule saved:', this.ruleForm.value);
    }
  }

  onCancel() {
    this.ruleForm.reset();
  }
}
