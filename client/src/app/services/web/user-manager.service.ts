import {EventEmitter, Injectable} from '@angular/core';
import {Observable} from 'rxjs';
import {ModalJquery} from '../../jquery/modal-jquery';
import {UserRole} from '../../models/common/user-role.enum';
import {ValueHelperService} from '../value-helper.service';
import {UserSummary} from '../../models/user-summary';
import {ManagedGeneratingUser} from '../../models/managed-generating-user';
import {ManagedUser} from '../../models/managed-user';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class UserManagerService
{
  public static RoleTeacher: UserRole = UserRole.Teacher;
  public users: UserSummary[] = [];
  public userRoleName = '老师';
  private onSubmitted: EventEmitter<UserSummary> | undefined;
  private onCreated: EventEmitter<any> | undefined;
  public selectedValue: UserSummary | undefined;
  public newValue: ManagedGeneratingUser | undefined;

  public managedUsers: ManagedUser[] = [];

  constructor(private vh: ValueHelperService,
              private http: HttpClient)
  {
  }

  public refresh(): void
  {
    this.getAllManaged()
      .subscribe(u => this.managedUsers = u);
  }

  public showAddDialog(): EventEmitter<any>
  {
    this.onCreated = new EventEmitter<any>();
    this.newValue = {
      name: '',
      username: '',
      initialPassword: '',
      role: UserRole.Student
    };
    ($('#add-user-dialog') as ModalJquery).modal('show');
    return this.onCreated;
  }

  public showSelectDialog(role: UserRole): EventEmitter<UserSummary>
  {
    this.getAll(role).subscribe(
      r => this.users = r
    );
    this.selectedValue = undefined;
    this.onSubmitted = new EventEmitter<UserSummary>();
    this.userRoleName = this.vh.formatUserRole(role);
    ($('#select-user-dialog') as ModalJquery).modal('show');
    return this.onSubmitted;
  }

  public selectUser(): void
  {
    this.onSubmitted?.emit(this.selectedValue);

  }

  public addUser(): void
  {
    if (this.newValue)
    {
      this.add(this.newValue)
        .subscribe(() =>
        {
          this.onCreated?.emit();
          this.refresh();
        });
    }
  }

  getAll(role: UserRole): Observable<UserSummary[]>
  {
    if (role === undefined || role === UserRole.All)
    {
      return this.http.get<UserSummary[]>(`${environment.apiUrl}/account`);
    }
    return this.http.get<UserSummary[]>(`${environment.apiUrl}/account?role=${role}`);
  }

  getAllManaged(): Observable<ManagedUser[]>
  {
    return this.http.get<ManagedUser[]>(`${environment.apiUrl}/account/managed`);
  }

  add(user: ManagedGeneratingUser): Observable<ManagedUser>
  {
    return this.http.post<ManagedUser>(
      `${environment.apiUrl}/account/register`, user);
    /*    return of(user);*/
  }

  remove(id: string): Observable<any>
  {
    return this.http.delete<any>(
      `${environment.apiUrl}/account/${id}`
    );
    // return of(id);
  }
}
