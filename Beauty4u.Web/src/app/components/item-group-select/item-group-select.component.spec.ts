import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemGroupSelectComponent } from './item-group-select.component';

describe('ItemGroupSelectComponent', () => {
  let component: ItemGroupSelectComponent;
  let fixture: ComponentFixture<ItemGroupSelectComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ItemGroupSelectComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ItemGroupSelectComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
