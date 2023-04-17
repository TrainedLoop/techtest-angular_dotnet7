import { TestBed, inject } from '@angular/core/testing';
import {
  HttpClientTestingModule,
  HttpTestingController,
} from '@angular/common/http/testing';
import { BillService } from './bill.service';
import { IBillInfoRequest } from '../schema/billInfoRequest';
import { HttpErrorResponse } from '@angular/common/http';
import { IBillInfo } from '../schema/billInfo';

describe('BillService', () => {
  let service: BillService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [BillService],
    });
    service = TestBed.inject(BillService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getBill', () => {
    it('should return an Observable<IBillInfo>', () => {
      const mockBillInfo: IBillInfo = {
        original_amount: 100,
        amount: 110,
        due_date: '2023-05-01',
        payment_date: '2022-12-12',
        interest_amount_calculated: 10,
        fine_amount_calculated: 0,
      };
      const mockBillInfoRequest: IBillInfoRequest = {
        bar_code: '123456',
        payment_date: new Date(),
      };

      service.getBill(mockBillInfoRequest).subscribe((billInfo: IBillInfo) => {
        expect(billInfo).toEqual(mockBillInfo);
      });

      const req = httpMock.expectOne(`${service.configUrl}`);
      expect(req.request.method).toBe('POST');
      expect(req.request.body).toEqual(mockBillInfoRequest);
      req.flush(mockBillInfo);
    });

    it('should handle 400 error', () => {
      const mockError = new HttpErrorResponse({
        status: 400,
        statusText: 'Bad Request',
      });
      const mockBillInfoRequest: IBillInfoRequest = {
        bar_code: '123456',
        payment_date: new Date(),
      };

      service.getBill(mockBillInfoRequest).subscribe(
        () => {},
        (error) => {
          expect(error.status).toBe(400);
          expect(error.message).toBe(
            'Bad request format, please verify all fields'
          );
        }
      );

      const req = httpMock.expectOne(`${service.configUrl}`);
      expect(req.request.method).toBe('POST');
      req.flush(null, mockError);
    });

    it('should handle 404 error', () => {
      const mockError = new HttpErrorResponse({
        status: 404,
        statusText: 'Not Found',
      });
      const mockBillInfoRequest: IBillInfoRequest = {
        bar_code: '123456',
        payment_date: new Date(),
      };

      service.getBill(mockBillInfoRequest).subscribe(
        () => {},
        (error) => {
          expect(error.status).toBe(404);
          expect(error.message).toBe('Bill not found');
        }
      );

      const req = httpMock.expectOne(`${service.configUrl}`);
      expect(req.request.method).toBe('POST');
      req.flush(null, mockError);
    });

    it('should handle 500 error', () => {
      const mockError = new HttpErrorResponse({
        status: 500,
        statusText: 'Internal Server Error',
      });
      const mockBillInfoRequest: IBillInfoRequest = {
        bar_code: '123456',
        payment_date: new Date(),
      };

      service.getBill(mockBillInfoRequest).subscribe(
        () => {},
        (error) => {
          expect(error.status).toBe(500);
          expect(error.message).toBe('An unexpected error occurred');
        }
      );

      const req = httpMock.expectOne(`${service.configUrl}`);
      expect(req.request.method).toBe('POST');
      req.flush(null, mockError);
    });
  });
});
