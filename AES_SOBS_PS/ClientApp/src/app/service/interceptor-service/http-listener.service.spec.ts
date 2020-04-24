import { TestBed, inject } from '@angular/core/testing';

import { HttpListenerService } from './http-listener.service';

describe('HttpListenerService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [HttpListenerService]
    });
  });

  it('should be created', inject([HttpListenerService], (service: HttpListenerService) => {
    expect(service).toBeTruthy();
  }));
});
