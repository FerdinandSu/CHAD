import {Component, OnInit} from '@angular/core';
import {AccountService} from '../services/web/account.service';
import {ActivatedRoute, Router} from '@angular/router';
import {MessageService} from '../services/message.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit
{
  username = '';
  password = '';
  loading = false;
  rememberMe = false;

  constructor(private account: AccountService,
              private msg: MessageService,
              private route: ActivatedRoute,
              private router: Router)
  {
  }

  ngOnInit(): void
  {
  }


  submit(): void
  {
    this.loading = true;
    this.account.login(this.username, this.password, this.rememberMe)
      .subscribe({
          next: () =>
          {
            const returnUrl = this.route.snapshot.queryParams.returnUrl || '/';
            this.loading = false;
            // noinspection JSIgnoredPromiseFromCall
            this.router.navigateByUrl(returnUrl);
          },
          error: err =>
          {

            this.loading = false;
            this.password = '';


          }
        }
      );
  }
}
