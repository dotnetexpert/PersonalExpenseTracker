import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CategorychartComponent } from './categorychart.component';

describe('CategorychartComponent', () => {
  let component: CategorychartComponent;
  let fixture: ComponentFixture<CategorychartComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CategorychartComponent]
    });
    fixture = TestBed.createComponent(CategorychartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
