import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { GET_ALL_USERS_ENDPOINT, GET_CHART_USER_ENDPOINT, GET_TOP_TEN_USERS_ENDPOINT, REFRESH_DATA_ENDPOINT } from './dashboard-api-endpoints';
import { 
  ChartItem,
  UsersRequest, 
  UsersResponse, 
  ChartRequest } from '../models';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(private http: HttpClient) { }

  get(id: string): Observable<ChartItem> {
    return this.http.get<ChartItem>(GET_CHART_USER_ENDPOINT, {
      params: {
        id
      }
    });
  }

  getAll(request: UsersRequest): Observable<UsersResponse> {
    return this.http.post<UsersResponse>(GET_ALL_USERS_ENDPOINT, request);
  }

  getTopTen(request: ChartRequest): Observable<ChartItem[]> {
    return this.http.post<ChartItem[]>(GET_TOP_TEN_USERS_ENDPOINT, request);
  }

  refreshData(): Observable<boolean> {
    return this.http.get<boolean>(REFRESH_DATA_ENDPOINT);
  }
}
