import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProviderViewTableComponent } from './provider-view-table.component';

describe('ProviderViewTableComponent', () => {
  let component: ProviderViewTableComponent;
  let fixture: ComponentFixture<ProviderViewTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProviderViewTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProviderViewTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
