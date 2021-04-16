import {Component, OnInit} from '@angular/core';
import {ElementService} from '../../../services/web/element.service';
import {MessageService} from '../../../services/message.service';
import {ModalJquery} from '../../../jquery/modal-jquery';

@Component({
  selector: 'app-add-element-dialog',
  templateUrl: './add-element-dialog.component.html',
  styleUrls: ['./add-element-dialog.component.scss']
})
export class AddElementDialogComponent implements OnInit
{

  constructor(public svc: ElementService,
              private msg: MessageService)
  {
  }

  ngOnInit(): void
  {
  }

  submit(): void
  {
    if (this.svc.newValue?.name?.length)
    {
      this.svc.addElement();
      ($('#add-element-dialog') as ModalJquery).modal('hide');
    }
    else
    {
      this.msg.addError('请输入名称。');
    }
  }
}
