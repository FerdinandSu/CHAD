import { Injectable } from '@angular/core';
import {CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router} from '@angular/router';
import { Observable } from 'rxjs';
import {AccountService} from '../services/web/account.service';
import {UserRole} from '../models/common/user-role.enum';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
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
    if (this.accountService.user?.role === UserRole.Administrator)
    {
      return true;
    }
    // noinspection JSIgnoredPromiseFromCall
    this.router.navigate(['/Forbidden']);
    return false;
  }

}
