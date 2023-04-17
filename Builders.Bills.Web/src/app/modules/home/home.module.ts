import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './page/home.component';
import { BillFormComponent } from './page/bill-form/bill-form.component';
import { HttpClientModule } from '@angular/common/http';
import { BillService } from 'src/app/data/service/bill.service';

@NgModule({
  declarations: [HomeComponent, BillFormComponent],
  imports: [CommonModule, FormsModule, HttpClientModule, ReactiveFormsModule],
  exports: [HomeComponent],
  providers: [BillService],
})
export class HomeModule { }
