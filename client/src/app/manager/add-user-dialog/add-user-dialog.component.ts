import {Component, OnInit} from '@angular/core';
import {UserRole} from 'src/app/models/common/user-role.enum';
import {UserManagerService} from '../../services/web/user-manager.service';
import {ElementService} from '../../services/web/element.service';

@Component({
  selector: 'app-add-user-dialog',
  templateUrl: './add-user-dialog.component.html',
  styleUrls: ['./add-user-dialog.component.scss']
})
export class AddUserDialogComponent implements OnInit
{
  public TEACHER = UserRole.Teacher;
  public STUDENT = UserRole.Student;

  constructor(public svc: UserManagerService)
  {
  }

  ngOnInit(): void
  {

  }
}
