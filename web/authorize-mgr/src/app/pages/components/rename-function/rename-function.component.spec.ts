import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RenameFunctionComponent } from './rename-function.component';

describe('RenameFunctionComponent', () => {
  let component: RenameFunctionComponent;
  let fixture: ComponentFixture<RenameFunctionComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RenameFunctionComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(RenameFunctionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
