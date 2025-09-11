import { TestBed } from '@angular/core/testing';

import { ScheduledJobService } from './scheduled-job.service';

describe('ScheduledJobService', () => {
  let service: ScheduledJobService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ScheduledJobService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
