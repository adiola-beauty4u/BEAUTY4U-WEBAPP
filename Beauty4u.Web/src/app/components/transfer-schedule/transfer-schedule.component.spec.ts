import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TransferScheduleComponent } from './transfer-schedule.component';

describe('TransferScheduleComponent', () => {
  let component: TransferScheduleComponent;
  let fixture: ComponentFixture<TransferScheduleComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TransferScheduleComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TransferScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
