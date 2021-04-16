import {Component, OnInit} from '@angular/core';
import {UserManagerService} from '../services/web/user-manager.service';
import {ValueHelperService} from '../services/value-helper.service';
import {UserRole} from '../models/common/user-role.enum';
import {MessageService} from '../services/message.service';

@Component({
  selector: 'app-manager',
  templateUrl: './manager.component.html',
  styleUrls: ['./manager.component.scss']
})
export class ManagerComponent implements OnInit
{
  ADMIN = UserRole.Administrator;
  fadeFlag = false;

  constructor(public svc: UserManagerService,
              private msg: MessageService,
              public vh: ValueHelperService)
  {
  }

  ngOnInit(): void
  {
    this.svc.refresh();
  }

  delUser(username: string): void
  {
    this.svc.remove(username)
      .subscribe(() =>
      {
        this.svc.refresh();
        this.msg.addOk();

      });
  }
}
