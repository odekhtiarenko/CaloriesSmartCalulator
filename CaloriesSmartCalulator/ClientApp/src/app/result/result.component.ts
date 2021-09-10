import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { TaskResult } from '../../models/TaskResult';

@Component({
  selector: 'result-cmp',
  templateUrl: './result.component.html',
})
export class ResultComponent {

  id: string | undefined;
  private subscription: Subscription;
  private baseUrl: string;

  public percetage: number;
  public result: TaskResult;
  interval: NodeJS.Timeout;

  constructor(private activateRoute: ActivatedRoute, private http: HttpClient, @Inject('BASE_URL') baseUrl: string, private router: Router) {
    this.subscription = activateRoute.params.subscribe(params => this.id = params['id']);
    this.baseUrl = baseUrl;

    this.http.get<TaskResult>(this.baseUrl + `api/caloriescalculation/${this.id}`).subscribe(result => {
      this.result = result;
    }, error => console.error(error));
  }
}

