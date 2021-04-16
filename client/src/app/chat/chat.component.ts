import {Component, OnInit} from '@angular/core';
import {ChatService} from '../services/web/chat.service';
import {UserManagerService} from '../services/web/user-manager.service';
import {UserRole} from '../models/common/user-role.enum';
import {UserSummary} from '../models/user-summary';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.scss']
})
export class ChatComponent implements OnInit
{

  constructor(public svc: ChatService,
              private um: UserManagerService)
  {
  }

  ngOnInit(): void
  {
    this.svc.refresh();
  }

  selectRemote(): void
  {
    this.um.showSelectDialog(UserRole.All)
      .subscribe((e: UserSummary) => this.svc.newMessage.remote = e);
  }
}
