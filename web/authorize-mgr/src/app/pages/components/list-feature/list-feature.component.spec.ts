import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListFeatureComponent } from './list-feature.component';

describe('ListFeatureComponent', () => {
  let component: ListFeatureComponent;
  let fixture: ComponentFixture<ListFeatureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListFeatureComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ListFeatureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
