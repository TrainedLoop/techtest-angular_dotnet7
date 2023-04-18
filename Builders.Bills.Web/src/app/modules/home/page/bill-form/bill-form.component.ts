import { Component, SimpleChanges } from '@angular/core';
import { BillService } from '../../../../data/service/bill.service';
import { IBillInfoRequest } from 'src/app/data/schema/billInfoRequest';
import { IBillInfo } from 'src/app/data/schema/billInfo';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-bill-form',
  templateUrl: './bill-form.component.html',
  styleUrls: ['./bill-form.component.scss'],
})
export class BillFormComponent {
  bill: IBillInfo;
  loading: boolean;
  validDate: boolean;
  form: FormGroup;
  serverErrorMessage?: string;
  constructor(private service: BillService, private fb: FormBuilder) {
    this.bill = {};
    this.loading = false;
    this.validDate = false;
    this.form = this.fb.group({
      bar_code: [
        '',
        {
          validators: [Validators.required, this.validateDate.bind(this)],
          updateOn: 'change',
        },
      ],
      payment_date: [
        '',
        {
          validators: [Validators.required, this.validateDate.bind(this)],
          updateOn: 'change',
        },
      ],
      getBill_response: [],
    });
  }

  validateDate = (changes: SimpleChanges) => {
    if (this.form?.controls !== undefined) {
      this.form.controls['getBill_response'].setErrors(null);
      this.form.updateValueAndValidity();
    }
  };

  getBill(): void {
    var request: IBillInfoRequest = {
      bar_code: this.form.controls['bar_code'].value,
      payment_date: this.form.controls['payment_date'].value,
    };
    this.loading = true;
    this.service.getBill(request).subscribe({
      complete: () => {
        this.loading = false;
      },
      error: (e) => {
        this.form.controls['getBill_response'].setErrors({
          invalid: true,
        });
        this.serverErrorMessage = e.message;
        this.loading = false;
      },
      next: (data) => {
        this.bill = data;
      },
    });
  }
}
