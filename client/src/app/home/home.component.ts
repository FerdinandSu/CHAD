import {Component, OnInit} from '@angular/core';
import {ElementSummary} from '../models/common/element-summary';
import {Resource} from '../models/resource';
import {ValueHelperService} from '../services/value-helper.service';
import {AccountService} from '../services/web/account.service';
import {ResourceService} from '../services/web/resource.service';
import {ChadService} from '../services/web/chad.service';
import {ElementService} from '../services/web/element.service';
import {DescribedElementSummary} from '../models/common/described-element-summary';
import {MessageService} from '../services/message.service';
import {Observable} from 'rxjs';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit
{
  public resources: Resource[] = [
    {name: 'foo', id: '0', size: 100, uploader: 'fool'},
    {name: 'bar', id: '1', size: 1000000, uploader: 'fool'}
  ];
  public courses: ElementSummary[] = [
    {name: 'foo', id: '0'},
    {name: 'bar', id: '1'}
  ];
  public classes: ElementSummary[] = [
    {name: 'foo', id: '0'},
    {name: 'bar', id: '1'}
  ];

  constructor(public vh: ValueHelperService,
              public res: ResourceService,
              public chad: ChadService,
              private ele: ElementService,
              private msg: MessageService,
              public account: AccountService)
  {

  }

  ngOnInit(): void
  {
    this.refresh();
  }

  refresh(): void
  {
    this.ele.getAll('课程').subscribe(c => this.courses = c);
    this.ele.getAll('班级').subscribe(c => this.classes = c);
    if (this.account.isTeacher)
    {
      this.res.getAll().subscribe(r => this.resources = r);
    }
  }

  newClass(): void
  {
    this.ele.showAddDialog('班级')
      .subscribe((des: DescribedElementSummary) =>
      {
        this.chad.addClass(des)
          .subscribe(() =>
          {
            this.ele.getAll('班级').subscribe(c => this.classes = c);
            this.msg.addOk();
          });
      });
  }

  newCourse(): void
  {
    this.ele.showAddDialog('课程')
      .subscribe((des: DescribedElementSummary) =>
      {
        this.chad.addCourse(des)
          .subscribe(() =>
          {
            this.ele.getAll('课程').subscribe(c => this.courses = c);
            this.msg.addOk();
          });
      });
  }

  runRequest(observable: Observable<any>): void
  {
    observable.subscribe(() =>
    {
      this.refresh();
      this.msg.addOk();
    });
  }
}
