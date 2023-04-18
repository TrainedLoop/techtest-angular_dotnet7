export interface IBillInfo {
  original_amount?: number;
  amount?: number;
  due_date?: string;
  payment_date?: string;
  interest_amount_calculated?: number;
  fine_amount_calculated?: number;
  type?: string;
}
