import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RecoverLoginModalComponent } from './recover-login-modal.component';

describe('RecoverLoginModalComponent', () => {
  let component: RecoverLoginModalComponent;
  let fixture: ComponentFixture<RecoverLoginModalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RecoverLoginModalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RecoverLoginModalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
