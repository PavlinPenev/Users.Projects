import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ChartItem, ChartRequest } from '../models';
import { Observable } from 'rxjs';
import { GET_TOP_TEN_PROJECTS_ENDPOINT } from './dashboard-api-endpoints';

@Injectable({
  providedIn: 'root'
})
export class ProjectsService {

  constructor(private http: HttpClient) { }

  getTopTen(request: ChartRequest): Observable<ChartItem[]> {
    return this.http.post<ChartItem[]>(GET_TOP_TEN_PROJECTS_ENDPOINT, request);
  }
}
