import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {AccountService} from '../../../services/web/account.service';
import {MessageService} from '../../../services/message.service';

/*declare var $: any;*/


@Component({
  selector: 'app-change-password-dialog',
  templateUrl: './change-password-dialog.component.html',
  styleUrls: ['./change-password-dialog.component.scss']
})
export class ChangePasswordDialogComponent implements OnInit
{
  @Input()
  cid = '';
  password: string[] = [
    '',
    '',
    ''
  ];
  @Output()
  submitted = new EventEmitter<string[]>();

  constructor(private account: AccountService,
              private msg: MessageService)
  {
  }

  ngOnInit(): void
  {

  }

  submit(): void
  {
    this.account.changePassword(this.password)
      .subscribe({
          next: () =>
          {
            this.msg.addOk();
            // @ts-ignore
            $('#' + this.cid).modal('hide');
            this.account.logout();
          },
          error: () =>
            this.msg.addError('操作失败')
        }
      )
    ;

  }

}
