import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { Meal, MealResponse } from '../../models/Meal';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  public meal: Meal;
  public caloriesItem: string;

  public result: string;

  private http: HttpClient;
  private baseUrl: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.http = http;
    this.baseUrl = baseUrl;
    this.meal = { products: [], name:'' };
  }

  addItem() {
    if (this.caloriesItem != '' || this.caloriesItem == null)
      this.meal.products.push(this.caloriesItem);

    this.caloriesItem = '';
  }

  sendItems() {
    this.http.post<MealResponse>(this.baseUrl + 'api/caloriescalculation/create', this.meal).subscribe(result => {
      this.router.navigate([`status/${result.id}`]);
    }, error => console.error(error));
  }
}


