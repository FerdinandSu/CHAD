import {Component, OnInit} from '@angular/core';
import {UserManagerService} from '../../../services/web/user-manager.service';
import {MessageService} from '../../../services/message.service';
import {ModalJquery} from '../../../jquery/modal-jquery';

@Component({
  selector: 'app-select-user-dialog',
  templateUrl: './select-user-dialog.component.html',
  styleUrls: ['./select-user-dialog.component.scss']
})
export class SelectUserDialogComponent implements OnInit
{

  constructor(public svc: UserManagerService,
              private msg: MessageService)
  {
  }

  ngOnInit(): void
  {
  }

  submit(): void
  {
    if (this.svc.selectedValue !== undefined)
    {
      this.svc.selectUser();
      ($('#select-user-dialog') as ModalJquery).modal('hide');
    }
    else
    {
      this.msg.addError(`请选择用户。`);
    }
  }
}
