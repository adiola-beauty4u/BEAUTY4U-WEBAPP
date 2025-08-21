import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PromotionsSearchComponent } from './promotions-search.component';

describe('PromotionsComponent', () => {
  let component: PromotionsSearchComponent;
  let fixture: ComponentFixture<PromotionsSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PromotionsSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PromotionsSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
