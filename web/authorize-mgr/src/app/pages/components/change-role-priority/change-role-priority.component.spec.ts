import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeRolePriorityComponent } from './change-role-priority.component';

describe('ChangeRolePriorityComponent', () => {
  let component: ChangeRolePriorityComponent;
  let fixture: ComponentFixture<ChangeRolePriorityComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ChangeRolePriorityComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(ChangeRolePriorityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
