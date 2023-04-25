import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormGroup, FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
  styleUrls: ['./fetch-data.component.css'],
})
export class FetchDataComponent {
  public currencies: Currency[] = [];
  public rates: CurrencyRate[] = [];
  public selectedCurrency?: SelectCurrencyModel;
  private baseUrl: string;
  public currency?: Currency;
  datePicker: FormControl = new FormControl()
  range = new FormGroup({
    start: new FormControl<Date | null>(null),
    end: new FormControl<Date | null>(null),
  })
  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private datePipe: DatePipe) {
    this.baseUrl = baseUrl;
    http.get<Currency[]>(baseUrl + 'currency').subscribe(result => {
      this.currencies = result;
    }, error => console.error(error));
  }
  onSelectCurrency() {
    if (this.currency === null) {
      return;
    }

    if (this.range !== null) {
      this.range = new FormGroup({
        start: new FormControl<Date | null>(null),
        end: new FormControl<Date | null>(null),
      })
    }

    if (this.rates !== null) {
      this.rates = [];
    }

    this.http.get<SelectCurrencyModel>(this.baseUrl + 'currency/' + this.currency?.id).subscribe(result => {
      this.selectedCurrency = (result as SelectCurrencyModel);
    }, error => console.error(error));
  }
  onDatePick(minDate: HTMLInputElement, maxDate: HTMLInputElement) {
    if (minDate === null || maxDate === null) {
      return;
    }
    let minDateText = this.datePipe.transform(minDate.value, "yyyy-MM-dd");
    let maxDateText = this.datePipe.transform(maxDate.value, "yyyy-MM-dd");
    this.http.get<CurrencyRate[]>(this.baseUrl + 'currency/' + this.currency?.id + '/' + minDateText + '/' + maxDateText).subscribe(result => {
      this.rates = result;
    }, error => console.error(error));
  }
}
interface Currency {
  name: string;
  id: number;
  amount: number;
}

interface SelectCurrencyModel {
  minDate: Date,
  maxDate: Date
}

interface CurrencyRate {
  date: Date,
  price: number
}
