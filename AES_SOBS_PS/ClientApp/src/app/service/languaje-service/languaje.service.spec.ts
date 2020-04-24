import { TestBed, inject } from '@angular/core/testing';

import { LanguajeService } from './languaje.service';

describe('LanguajeService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [LanguajeService]
    });
  });

  it('should be created', inject([LanguajeService], (service: LanguajeService) => {
    expect(service).toBeTruthy();
  }));
});
