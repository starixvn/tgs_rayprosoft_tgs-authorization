import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HttpFeatureEditorComponent } from './http-feature-editor.component';

describe('HttpFeatureEditorComponent', () => {
  let component: HttpFeatureEditorComponent;
  let fixture: ComponentFixture<HttpFeatureEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [HttpFeatureEditorComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(HttpFeatureEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
