import {Component, OnInit} from '@angular/core';
import {User} from '../../../models/common/user';
import {UserRole} from '../../../models/common/user-role.enum';
import {AccountService} from '../../../services/web/account.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit
{
  ADMIN = UserRole.Administrator;
  TEACHER = UserRole.Teacher;
  STUDENT = UserRole.Student;
  user!: User;

  constructor(public account: AccountService)
  {
  }

  ngOnInit(): void
  {
  }

}
