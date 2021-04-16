import {Injectable} from '@angular/core';
import {User} from '../../models/common/user';
import {BehaviorSubject, Observable} from 'rxjs';
import {MessageService} from '../message.service';
import {Router} from '@angular/router';

import {ManagedUser} from 'src/app/models/managed-user';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {map, tap} from 'rxjs/operators';
import {UserRole} from '../../models/common/user-role.enum';

@Injectable({
  providedIn: 'root'
})
export class AccountService
{
  private userSubject: BehaviorSubject<User | undefined>;

  get user(): User | undefined
  {
    /*    return {
          name: 'aaa',
          role: UserRole.Student,
          token: ''
        };*/
    return this.userSubject?.value;
  }

  get isTeacher(): boolean
  {
    return this.user?.role === UserRole.Teacher;
  }

  public canManage(id: string): boolean
  {
    return this.user?.username === id;
  }

  constructor(public messageService: MessageService,
              protected router: Router,
              private http: HttpClient)
  {
    const user = localStorage.getItem('user');
    if (user)
    {
      this.userSubject = new BehaviorSubject<User | undefined>(
        JSON.parse(user));
    }
    else
    {
      this.userSubject = new BehaviorSubject<User | undefined>(undefined);
    }
  }


  logout(): Observable<any>
  {
    localStorage.removeItem('user');
    this.userSubject.next(undefined);
    // noinspection JSIgnoredPromiseFromCall
    this.router.navigate(['/Login']);
    return this.http.get(`${environment.apiUrl}/account/logout`)
      .pipe(tap(() =>
      {

      }));
    // remove user from local storage and set current user to null

    // return of(true);
  }

  login(username: string, password: string, rememberMe: boolean): Observable<any>
  {
    return this.http.post<User>(`${environment.apiUrl}/account/login`,
      {username, password, rememberMe})
      .pipe(map(user =>
      {
        // store user details and jwt token in local storage to keep user logged in between page refreshes

        localStorage.setItem('user', JSON.stringify(user));

        this.userSubject.next(user);
      }));
  }


  changePassword(password: string[]): Observable<any>
  {
    return this.http.post<ManagedUser>(
      `${environment.apiUrl}/account/chpwd`, password);
    // return of(true);
  }
}
