import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'status-cmp',
  templateUrl: './status.component.html',
  styles: ['./status.component.css']
})
export class StatusComponent {

  id: string | undefined;
  private subscription: Subscription;
  private baseUrl: string;

  public percetage: number;
  public statusObject: StatusOject;
  interval: NodeJS.Timeout;

  constructor(private activateRoute: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.subscription = activateRoute.params.subscribe(params => this.id = params['id']);
    this.baseUrl = baseUrl;

    this.interval = setInterval(() => {
      this.http.get<StatusOject>(this.baseUrl + `api/caloriescalculation/status/${this.id}`).subscribe(result => {
        this.statusObject = result;
        console.log(this.statusObject);
      }, error => console.error(error));
      if (this.statusObject.percentage == 100) {
        clearInterval(this.interval);
        this.router.navigate([`result/${this.id}`]);
      }
    }, 1000);
  }
}

interface StatusOject {
  percentage: number;
  status: number;
  products: string[];
  total: number;
}

