import {Injectable} from '@angular/core';
import {ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree} from '@angular/router';
import {Observable} from 'rxjs';
import {AccountService} from '../services/web/account.service';
import {UserRole} from '../models/common/user-role.enum';

@Injectable({
  providedIn: 'root'
})
export class JudgeGuard implements CanActivate
{
  constructor(
    private router: Router,
    private accountService: AccountService
  )
  {
  }

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree
  {
    if (this.accountService.user && this.accountService.user.role === UserRole.Teacher)
    {
      return true;
    }
    // noinspection JSIgnoredPromiseFromCall
    this.router.navigate(['/Forbidden']);
    return false;
  }

}
