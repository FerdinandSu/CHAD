import {Component, OnInit} from '@angular/core';
import {ResourceService} from '../../../services/web/resource.service';
import {MessageService} from '../../../services/message.service';
import {ModalJquery} from '../../../jquery/modal-jquery';

@Component({
  selector: 'app-add-resource-dialog',
  templateUrl: './add-resource-dialog.component.html',
  styleUrls: ['./add-resource-dialog.component.scss']
})
export class AddResourceDialogComponent implements OnInit
{


  constructor(public svc: ResourceService,
              private msg: MessageService)
  {
  }

  ngOnInit(): void
  {
  }

  submit(): void
  {
    if (this.svc.check())
    {
      this.svc.addResource();
      ($('#add-resource-dialog') as ModalJquery).modal('hide');
    }
    else
    {
      this.msg.addError('请选择资源。');
    }
  }


}
