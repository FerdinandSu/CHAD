import {Component, OnInit} from '@angular/core';
import {Location} from '@angular/common';
import {ActivatedRoute, Router} from '@angular/router';
import {MessageService} from '../services/message.service';
import {Course} from '../models/course';
import {ElementService} from '../services/web/element.service';
import {AccountService} from '../services/web/account.service';
import {ChadService} from '../services/web/chad.service';
import {DescribedElementSummary} from '../models/common/described-element-summary';
import {ElementSummary} from '../models/common/element-summary';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.scss']
})
export class CoursesComponent implements OnInit
{
  course: Course = {
    name: '',
    id: '0',
    description: '',
    director: {
      name: 'dk',
      username: 'dkk'
    },
    classes: [],
    lessons: [{
      name: 'foo',
      id: '0'
    }]
  };

  constructor(private route: ActivatedRoute,
              public element: ElementService,
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
      this.chad.getCourse(id).subscribe(c => this.course = c);
    }
  }

  addLesson(): void
  {
    this.element.showAddDialog('课时')
      .subscribe((c: DescribedElementSummary) =>
      {
        this.chad.addLesson(this.course.id, this.course.lessons.length, c)
          .subscribe(() => this.refresh());
      });
  }

  includeClass(): void
  {
    this.element.showSelectDialog('班级')
      .subscribe((c: ElementSummary) =>
      {
        this.chad.includeClass(this.course.id, c)
          .subscribe(() => this.refresh());
      });
  }
}
