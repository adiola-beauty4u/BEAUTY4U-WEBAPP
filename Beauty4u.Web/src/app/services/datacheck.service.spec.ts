import { TestBed } from '@angular/core/testing';

import { DataCheckService } from './datacheck.service';

describe('DatacheckService', () => {
  let service: DataCheckService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DataCheckService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
