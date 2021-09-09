import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public products: string[] = [];
  public caloriesItem: string;

  public result: string;

  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  addItem() {
    if (this.caloriesItem != '' || this.caloriesItem == null)
      this.products.push(this.caloriesItem);

    this.caloriesItem = '';
  }

  sendItems() {
    this.http.post(this.baseUrl + 'api/caloriescalculation/create', this.products, { responseType: "text" }).subscribe(result => {
      this.router.navigate([`status/${result}`]);
    }, error => console.error(error));
  }
}


