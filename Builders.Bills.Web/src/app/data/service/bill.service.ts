import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IBillInfoRequest } from '../schema/billInfoRequest';
import { IBillInfo } from '../schema/billInfo';

@Injectable({
  providedIn: 'root',
})
export class BillService {
  constructor(private http: HttpClient) {}
  configUrl = 'http://localhost:5000/api/bill';

  getBill(request: IBillInfoRequest): Observable<IBillInfo> {
    return this.http
      .post<any>(this.configUrl, request, {
        headers: {
          'content-type': 'application/json',
        },
      })
      .pipe(catchError(this.handleError));
  }

  private handleError(err: HttpErrorResponse) {
    let errorMessage = '';
    switch (err.status) {
      case 400:
        errorMessage = 'Bad request format, please verify all fields';
        break;
      case 404:
        errorMessage = 'Bill not found';
        break;
      case 500:
      default:
        errorMessage = `An unexpected error occurred`;
        break;
    }
    return throwError(() => ({
      message: errorMessage,
      status: err.status,
    }));
  }
}
