import { TestBed } from '@angular/core/testing';

import { PayOrderService } from './pay-order.service';

describe('PayOrderService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: PayOrderService = TestBed.get(PayOrderService);
    expect(service).toBeTruthy();
  });
});
