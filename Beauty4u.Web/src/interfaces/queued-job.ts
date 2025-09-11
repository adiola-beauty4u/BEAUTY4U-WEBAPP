export interface QueuedJob {
  jobName: string;
  jobGroup: string;
  triggerName: string;
  triggerGroup: string;
  nextFireTime: Date;
  previousFireTime: Date | null;
}