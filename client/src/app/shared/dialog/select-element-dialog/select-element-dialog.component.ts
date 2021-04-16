import {Component, OnInit} from '@angular/core';
import {ElementService} from '../../../services/web/element.service';
import {MessageService} from '../../../services/message.service';
import {ModalJquery} from '../../../jquery/modal-jquery';

@Component({
  selector: 'app-select-element-dialog',
  templateUrl: './select-element-dialog.component.html',
  styleUrls: ['./select-element-dialog.component.scss']
})
export class SelectElementDialogComponent implements OnInit
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
    if (this.svc.selectedValue !== undefined)
    {
      this.svc.selectElement();
      ($('#select-element-dialog') as ModalJquery).modal('hide');
    }
    else
    {
      this.msg.addError(`请选择${this.svc.elementTypeName}。`);
    }

  }
}
