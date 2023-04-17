import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';
import { FormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { of, throwError } from 'rxjs';
import { BillFormComponent } from './bill-form.component';
import { BillService } from '../../../../data/service/bill.service';
import { CommonModule } from '@angular/common';

describe('BillFormComponent', () => {
  let component: BillFormComponent;
  let fixture: ComponentFixture<BillFormComponent>;
  let billService: jasmine.SpyObj<BillService>;

  beforeEach(waitForAsync(() => {
    const billServiceSpy = jasmine.createSpyObj('BillService', ['getBill']);
    TestBed.configureTestingModule({
      imports: [
        CommonModule,
        FormsModule,
        HttpClientTestingModule,
        ReactiveFormsModule,
      ],
      declarations: [BillFormComponent],
      providers: [
        FormBuilder,
        { provide: BillService, useValue: billServiceSpy },
      ],
    }).compileComponents();
    billService = TestBed.inject(BillService) as jasmine.SpyObj<BillService>;
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BillFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  describe('getBill', () => {
    it('should call service with correct parameters', () => {
      component.form.setValue({
        bar_code: '123456',
        payment_date: new Date('2023-04-17'),
        getBill_response: null,
      });
      billService.getBill.and.returnValue(of({}));
      component.getBill();
      expect(billService.getBill).toHaveBeenCalledWith({
        bar_code: '123456',
        payment_date: new Date('2023-04-17'),
      });
    });

    it('should set bill property on success', () => {
      const response = {
        original_amount: 100,
        amount: 105,
        due_date: '2023-05-01',
        payment_date: '2023-04-17',
        interest_amount_calculated: 5,
        fine_amount_calculated: 0,
      };
      component.form.setValue({
        bar_code: '123456',
        payment_date: new Date('2023-04-17'),
        getBill_response: null,
      });
      billService.getBill.and.returnValue(of(response));
      component.getBill();
      expect(component.bill).toEqual(response);
    });

    it('should set serverErrorMessage property on error', () => {
      const error = { message: 'Error message' };
      billService.getBill.and.returnValue(throwError(() => error));
      component.form.setValue({
        bar_code: '123456',
        payment_date: '2023-04-17',
        getBill_response: null,
      });
      component.getBill();
      expect(component.serverErrorMessage).toEqual(error.message);
    });
  });
});
