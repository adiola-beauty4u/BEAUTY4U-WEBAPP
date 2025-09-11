export enum ScheduledJobFrequency {
  Hourly = 0,
  Every2Hrs = 1,
  Every3Hrs = 2,
  Every4Hrs = 3,
  Every6Hrs = 4,
  Every12Hrs = 5,
  Daily = 6
}

export interface ScheduledJob{
    scheduledJobId: number;
    name: string;
    description: string;
    startHour: number;
    hour: number;
    minute: number;
    frequency: ScheduledJobFrequency;
}