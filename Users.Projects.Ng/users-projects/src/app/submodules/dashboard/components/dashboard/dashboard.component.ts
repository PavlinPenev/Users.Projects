import { Component, OnInit, ViewChild } from '@angular/core';
import * as TextConstants from '../../../shared/constants/constants'; 
import { ChartType, Column, GoogleChartComponent, Row } from 'angular-google-charts';
import { 
  ChartRowTypeEnum, 
  GoogleChartConfiguration, 
  User,
  UsersResponse,
  UsersRequest, 
  ChartItem,
  ChartRequest} from '../../models/';
import { Sort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { UntypedFormControl, UntypedFormGroup } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { Subject, catchError, debounceTime, filter, finalize, first, forkJoin, throwError } from 'rxjs';
import { UsersService } from '../../services/users.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { HttpErrorResponse } from '@angular/common/http';
import { ProjectsService } from '../../services/projects.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  @ViewChild('paginator', { static: true }) paginator!: MatPaginator;
  @ViewChild('chart', { static: true }) chart!: GoogleChartComponent;

  constants = TextConstants;

  areUsersLoading: boolean = true;
  users: UsersResponse = {
    items: [],
    totalItemsCount: 0
  };
  dataSource: MatTableDataSource<User> = new MatTableDataSource<User>();

  searchFormControl = new UntypedFormControl('');
  dateRangeFormControl: UntypedFormGroup = new UntypedFormGroup({
    start: new UntypedFormControl(''),
    end: new UntypedFormControl(''),
  });

  displayedColumns: string[] = [
    'firstName',
    'lastName',
    'email',
    'dateAdded',
    'actions'
  ]

  usersRequest: UsersRequest = {
    searchTerm: '',
    orderBy: 'firstName',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 10
  };

  initialUsersRequest: UsersRequest = {
    searchTerm: '',
    orderBy: 'firstName',
    isDescending: false,
    dateAddedFrom: null,
    dateAddedTo: null,
    skip: 0,
    take: 10
  };

  subject: Subject<string> = new Subject();

  areChartUsersLoading: boolean = true;
  areChartProjectsLoading: boolean = true;
  chartUsers: ChartItem[] = [];
  chartProjects: ChartItem[] = [];
  chartData: Row[] = [];
  chartColumns: Column[] = ['Name', 'Hours Worked'];
  chartOptions: any = {
    colors: [ "#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff"],
    legend: "none"
  };

  initialChartOptions: any = {
    colors: [ "#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff","#0000ff"],
    legend: "none"
  };

  ChartRowTypeEnum = ChartRowTypeEnum;

  chartSelectionEnum: ChartRowTypeEnum = ChartRowTypeEnum.Users;
  
  googleChartConfig: GoogleChartConfiguration = {
    type: ChartType.BarChart,
    columns: this.chartColumns
  };

  chartRequest: ChartRequest = {
    take: 10
  }

  constructor(
    private usersService: UsersService, 
    private projectsService: ProjectsService,
    private snackBar: MatSnackBar) {}
  
  ngOnInit(): void {
    this.subject.pipe(debounceTime(800)).subscribe((searchTextValue) => {
      this.search(searchTextValue);
    });
    
    this.getUsers(this.usersRequest);
    this.getChartData(this.chartRequest);
  }
  
  onSortChange(e: Sort): void {
    this.usersRequest = {
      ...this.initialUsersRequest,
      orderBy: e.active,
      isDescending: e.direction === 'desc',
    };

    this.getUsers(this.usersRequest);

    this.paginator.firstPage();
  }

  onKeyUp(): void {
    this.subject.next(this.searchFormControl.value);
  } 

  search(searchTerm: string): void {
    this.usersRequest = {
      ...this.initialUsersRequest,
      searchTerm: searchTerm,
    };

    this.getUsers(this.usersRequest);

    this.paginator.firstPage();
  }

  filterByDate(): void {
    this.usersRequest = {
      ...this.initialUsersRequest,
      dateAddedFrom: this.dateRangeFormControl.controls['start'].value,
      dateAddedTo: this.dateRangeFormControl.controls['end'].value,
    };

    this.getUsers(this.usersRequest);

    this.paginator.firstPage();
  }

  pageChanged(): void {
    this.usersRequest = {
      ...this.usersRequest,
      skip: this.paginator.pageIndex * this.paginator.pageSize,
      take: this.paginator.pageSize,
    };

    this.getUsers(this.usersRequest);
  }

  loadChartData(): void {
    this.chartData = [];

    if(this.chartSelectionEnum === ChartRowTypeEnum.Users) {
      this.chartData.push(...this.chartUsers.map(x => [x.name, x.hoursWorked]));
    } else if(this.chartSelectionEnum == ChartRowTypeEnum.Projects) {
      this.chartData.push(...this.chartProjects.map(x => [x.name, x.hoursWorked]));
    }
  }

  refreshData(): void {
    this.usersService
      .refreshData()
      .pipe(
        first(),
        catchError((errorResponse: HttpErrorResponse) => {
          this.snackBar.open(this.constants.SOMETHING_WENT_WRONG_REFRESHING_DATA, this.constants.CLOSE, {
            horizontalPosition: 'center',
            verticalPosition: 'top',
          });

          return throwError(() => errorResponse.message);
        })
      ).subscribe((response: boolean) => {
        if (response) {
          window.location.reload(); 
          /** Consider using router navigating to the current route 
          with relativeTo the same route if we want to avoid full page reload 
          and we want to take advantage of all the SPA possibilities */
        } else {
          this.snackBar.open(this.constants.SOMETHING_WENT_WRONG_REFRESHING_DATA, this.constants.CLOSE, {
            horizontalPosition: 'center',
            verticalPosition: 'top',
          });
        }
      })
  }

  getUserById(id: string) {
    this.areChartUsersLoading = true;

    this.usersService
      .get(id)
      .pipe(
        filter(x => !!x),
        first(),
        catchError((errorResponse: HttpErrorResponse) => {
          this.snackBar.open(this.constants.SOMETHING_WENT_WRONG_REFRESHING_DATA, this.constants.CLOSE, {
            horizontalPosition: 'center',
            verticalPosition: 'top',
          });

          return throwError(() => errorResponse.message);
        })
      ).subscribe((response: ChartItem) => {
        if (this.chartUsers.length == 11) {
          this.chartUsers.pop();
        }
        
        this.areChartUsersLoading = false;
        this.chartUsers.push(response);
        this.chartOptions.colors.push("#ff0000");
        this.chartData = [...this.chartUsers.map(x => [x.name, x.hoursWorked])];
      })
  }

  removeCompare(): void {
    if (this.chartUsers.length > 10) {
      this.areChartProjectsLoading = true;
      this.areChartUsersLoading = true;

      this.chartUsers.pop();
      this.chartOptions.colors.pop();

      this.loadChartData();

      this.areChartProjectsLoading = false;
      this.areChartUsersLoading = false;
    }
  }

  private getUsers(request: UsersRequest) {
    this.areUsersLoading = true;

    this.usersService
      .getAll(request)
      .pipe(
        filter(x => !!x),
        first(),
        catchError((errorResponse: HttpErrorResponse) => {
          this.snackBar.open(this.constants.SOMETHING_WENT_WRONG_FETCHING_USERS, this.constants.CLOSE, {
            horizontalPosition: 'center',
            verticalPosition: 'top',
          });

          return throwError(() => errorResponse.message);
        })
      )
      .subscribe((response: UsersResponse) => {
        this.areUsersLoading = false;

        this.users = response;
        this.dataSource.data = this.users.items;
      });
  }

  private getChartData(request: ChartRequest): void {
    this.areChartUsersLoading = true;
    this.areChartProjectsLoading = true;

    forkJoin([
      this.usersService.getTopTen(request),
      this.projectsService.getTopTen(request)
    ])
    .pipe(
      filter(x => !!x),
      first(),
      finalize(() => {
        this.areChartUsersLoading = false;
        this.areChartProjectsLoading = false;
      })
    )
    .subscribe(([usersResponse, projectsResponse]) => {

      this.chartUsers = usersResponse;
      this.chartProjects = projectsResponse;

      this.loadChartData();
    });
  }
}
