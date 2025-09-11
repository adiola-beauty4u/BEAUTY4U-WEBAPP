export interface ScheduledJobLogSearchParams {
    scheduledJobId: number;
    isSuccessful?: boolean;
    jobStart?: Date;
    jobEnd?: Date;
}