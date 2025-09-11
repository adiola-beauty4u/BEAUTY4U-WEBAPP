import { Component, OnInit } from '@angular/core';
import { MatTab, MatTabGroup } from '@angular/material/tabs';
import { FormBuilder, FormGroup, FormControl, ReactiveFormsModule } from '@angular/forms';
import { CommonModule, NgTemplateOutlet } from '@angular/common';

import { TableComponent } from 'src/app/components/table/table.component';
import { ScheduledJobService } from 'src/app/services/scheduled-job.service';
import { QueuedJob } from 'src/interfaces/queued-job';
import { LoadingService } from 'src/app/services/loading.service';
import { TableData } from 'src/interfaces/table-data';
import { ScheduledJob } from 'src/interfaces/scheduled-job';
import { MatAccordion, MatExpansionModule } from '@angular/material/expansion';
import { MatChipListbox, MatChipsModule } from '@angular/material/chips';
import { MatIcon, MatIconModule } from '@angular/material/icon';
import { ActiveScheduledJobSelectComponent } from 'src/app/components/active-scheduled-job-select/active-scheduled-job-select.component';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatStepperModule } from '@angular/material/stepper';
import { MatCardModule } from '@angular/material/card';
import { ScheduledJobLogSearchParams } from 'src/interfaces/scheduled-job-logs-search';
import { RadioListComponent } from 'src/app/components/radio-list/radio-list.component';
import { ItemValue } from 'src/interfaces/item-value';

@Component({
  selector: 'app-scheduledjobs',
  imports: [MatTabGroup,
    MatTab,
    MatStepperModule,
    CommonModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    ReactiveFormsModule,
    MatChipsModule,
    MatChipListbox,
    MatDatepickerModule,
    MatNativeDateModule,
    MatAccordion,
    MatExpansionModule,
    TableComponent, ActiveScheduledJobSelectComponent, MatFormFieldModule,
    RadioListComponent],
  templateUrl: './scheduledjobs.component.html',
  styleUrl: './scheduledjobs.component.scss'
})

export class ScheduledjobsComponent implements OnInit {
  queuedJobs: TableData;
  scheduledLogs: TableData;
  activeJobs: ScheduledJob[] = [];
  jobSearchForm: FormGroup;

  isSuccessfulItems: ItemValue[] = [
    { displayText: 'Successful', value: 'successful' },
    { displayText: 'Unsuccessful', value: 'unsuccessful' },
  ];

  constructor(private fb: FormBuilder,
    private readonly scheduledJobService: ScheduledJobService,
    private readonly loadingService: LoadingService) {

  }

  ngOnInit(): void {

    this.loadingService.show();
    this.scheduledJobService.getQueuedJobs().subscribe({
      next: (data) => {
        this.queuedJobs = data;
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Getting queued jobs failed:', err);
        this.loadingService.hide();
      }
    });

    this.scheduledJobService.getActiveJobs().subscribe({
      next: (data) => {
        this.activeJobs = data;
      },
      error: (err) => {
        console.error('Getting active jobs list failed:', err);
        this.loadingService.hide();
      }
    });
    this.jobSearchForm = this.fb.group({
      jobSchedule: '',
      isSuccessful: null,
      fromDate: new Date(),
      toDate: new Date(),
    });

  }

  searchJobHistory() {
    this.loadingService.show();
    var isSuccess;
    if (this.jobSearchForm.value.isSuccessful?.value) {
      isSuccess = this.jobSearchForm.value.isSuccessful?.value == 'successful'
    }
    var scheduledJobLogSearchParams: ScheduledJobLogSearchParams = {
      scheduledJobId: this.jobSearchForm.value.jobSchedule?.value,
      isSuccessful: isSuccess,
      jobStart: this.jobSearchForm.value?.fromDate,
      jobEnd: this.jobSearchForm.value?.toDate,
    };

    this.scheduledJobService.searchScheduledJobLogs(scheduledJobLogSearchParams).subscribe({
      next: (data) => {
        this.scheduledLogs = data;
        this.loadingService.hide();
      },
      error: (err) => {
        console.error('Getting queued jobs failed:', err);
        this.loadingService.hide();
      }
    });

  }

  clear() {
    this.jobSearchForm.reset({
      jobSchedule: '',
      isSuccessful: null,
      fromDate: new Date(),
      toDate: new Date(),
    });

    this.scheduledLogs = { columns: this.scheduledLogs.columns, tableName: '', rows: [], tableGroups: [] } as TableData;
    
  }
}
