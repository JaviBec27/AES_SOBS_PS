import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientViewTableComponent } from './client-view-table.component';

describe('ClientViewTableComponent', () => {
  let component: ClientViewTableComponent;
  let fixture: ComponentFixture<ClientViewTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientViewTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientViewTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
