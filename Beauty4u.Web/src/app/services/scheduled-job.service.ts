import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { QueuedJob } from 'src/interfaces/queued-job';
import { TableData } from 'src/interfaces/table-data';
import { ScheduledJob } from 'src/interfaces/scheduled-job';
import { ScheduledJobLogSearchParams } from 'src/interfaces/scheduled-job-logs-search';

@Injectable({
  providedIn: 'root'
})
export class ScheduledJobService {
  private promotionsUrl = `${environment.apiBaseUrl}/v1/jobscheduler`;
  constructor(private http: HttpClient) { }

  getQueuedJobs(): Observable<TableData> {
    return this.http.get<TableData>(this.promotionsUrl);
  }

  getActiveJobs(): Observable<ScheduledJob[]> {
    return this.http.get<ScheduledJob[]>(this.promotionsUrl + '/get-active-jobs');
  }

  searchScheduledJobLogs(scheduledJobLogSearchParams: ScheduledJobLogSearchParams){
     return this.http.post<TableData>(this.promotionsUrl + '/search-scheduled-job-logs', scheduledJobLogSearchParams);
  }
}
