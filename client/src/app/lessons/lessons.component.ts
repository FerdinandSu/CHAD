import {Component, OnInit} from '@angular/core';
import {ValueHelperService} from '../services/value-helper.service';
import {Location} from '@angular/common';
import {ActivatedRoute, Router} from '@angular/router';
import {MessageService} from '../services/message.service';
import {Lesson} from '../models/lesson';
import {ResourceService} from '../services/web/resource.service';
import {ChadService} from '../services/web/chad.service';
import {AccountService} from '../services/web/account.service';
import {ElementSummary} from '../models/common/element-summary';

@Component({
  selector: 'app-lessons',
  templateUrl: './lessons.component.html',
  styleUrls: ['./lessons.component.scss']
})
export class LessonsComponent implements OnInit
{
  lesson: Lesson = {
    course: {
      name: 'crs',
      id: '0'
    },
    name: '',
    id: '0',
    description: '',
    resources: []
  };

  constructor(public vh: ValueHelperService,
              public res: ResourceService,
              private route: ActivatedRoute,
              private router: Router,
              public chad: ChadService,
              public acc: AccountService,
              private msg: MessageService)
  {
  }

  ngOnInit(): void
  {
    this.refresh();
  }

  refresh(): void
  {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id)
    {
      // noinspection JSIgnoredPromiseFromCall
      this.router.navigate(['/NotFound']);
    }
    else
    {
      this.chad.getLesson(id)
        .subscribe(l => this.lesson = l);
    }
  }

  includeRes(): void
  {
    this.res.showDialog()
      .subscribe((r: ElementSummary) =>
      {
        this.chad.includeResource(this.lesson.id, r)
          .subscribe(() => this.refresh());
      });
  }

  excludeRes(res: ElementSummary): void
  {
    this.chad.excludeResource(this.lesson.id, res)
      .subscribe(() =>
        this.refresh());
  }
}
