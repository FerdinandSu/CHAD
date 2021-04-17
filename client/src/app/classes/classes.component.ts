import {Component, OnInit} from '@angular/core';
import {Class} from '../models/class';
import {ActivatedRoute, Router} from '@angular/router';
import {MessageService} from '../services/message.service';
import {ElementService} from '../services/web/element.service';
import {ChadService} from '../services/web/chad.service';
import {AccountService} from '../services/web/account.service';
import {UserManagerService} from '../services/web/user-manager.service';
import {UserRole} from '../models/common/user-role.enum';
import {UserSummary} from '../models/user-summary';

@Component({
  selector: 'app-classes',
  templateUrl: './classes.component.html',
  styleUrls: ['./classes.component.scss']
})
export class ClassesComponent implements OnInit
{

  theClass: Class = {
    id: '',
    name: '',
    director: {name: '', username: ''},
    students: [ ]
  };

  constructor(private ele: ElementService,
              private route: ActivatedRoute,
              private router: Router,
              public chad: ChadService,
              public acc: AccountService,
              private um: UserManagerService,
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
      this.chad.getClass(id)
        .subscribe(c => this.theClass = c);
    }
  }

  addCourse(): void
  {
    this.um.showSelectDialog(UserRole.Student)
      .subscribe((c: UserSummary) =>
      {
        this.chad.includeStudent(this.theClass.id, c)
          .subscribe(() => this.refresh());
      });
  }
}
