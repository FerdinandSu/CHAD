import {Component} from '@angular/core';
import {AccountService} from './services/web/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent
{
  title = 'CHAD-SPA';

  constructor(public acc: AccountService)
  {
  }
}
