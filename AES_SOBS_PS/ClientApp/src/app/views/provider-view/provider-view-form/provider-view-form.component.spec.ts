import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderViewFormComponent } from './provider-view-form.component';

describe('ProviderViewFormComponent', () => {
  let component: ProviderViewFormComponent;
  let fixture: ComponentFixture<ProviderViewFormComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProviderViewFormComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProviderViewFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
