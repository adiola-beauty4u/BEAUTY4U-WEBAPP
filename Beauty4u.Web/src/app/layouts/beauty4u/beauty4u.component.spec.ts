import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Beauty4uComponent } from './beauty4u.component';

describe('Beauty4uComponent', () => {
  let component: Beauty4uComponent;
  let fixture: ComponentFixture<Beauty4uComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Beauty4uComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Beauty4uComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
