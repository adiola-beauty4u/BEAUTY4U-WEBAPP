import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StoreCheckListComponent } from './store-check-list.component';

describe('StoreCheckListComponent', () => {
  let component: StoreCheckListComponent;
  let fixture: ComponentFixture<StoreCheckListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StoreCheckListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StoreCheckListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
