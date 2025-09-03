import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import flatpickr from 'flatpickr';

@Component({
  selector: 'app-datepicker',
  imports: [CommonModule],
  templateUrl: './datepicker.component.html',
  styleUrl: './datepicker.component.scss'
})
export class DatepickerComponent implements AfterViewInit {
  
  ngAfterViewInit(): void {
    // Init single date picker
    flatpickr('#azureDate', {
      dateFormat: 'm/d/Y'
    });

    // Init range picker
    flatpickr('#azureRange', {
      mode: 'range',
      dateFormat: 'm/d/Y'
    });
  }
}