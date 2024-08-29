import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExpensebycategoryComponent } from './expensebycategory.component';

describe('ExpensebycategoryComponent', () => {
  let component: ExpensebycategoryComponent;
  let fixture: ComponentFixture<ExpensebycategoryComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [ExpensebycategoryComponent]
    });
    fixture = TestBed.createComponent(ExpensebycategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
