import {Injectable} from '@angular/core';
import {Observable, of} from 'rxjs';
import {MessageService} from './message.service';
import {Router} from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class MiscService
{


  constructor(public messageService: MessageService,
              protected router: Router)
  {
  }


  public clone(obj: any): any
  {
    return JSON.parse(JSON.stringify(obj));
  }

  private generateUUID(): string
  {
    let d = new Date().getTime();
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'
      .replace(/[xy]/g,
        (c) =>
        {
          // tslint:disable-next-line:no-bitwise
          const r = (d + Math.random() * 16) % 16 | 0;
          d = Math.floor(d / 16);
          // tslint:disable-next-line:no-bitwise
          return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);


        });
  }

  getId(): Observable<string>
  {
    return of(this.generateUUID());
  }

}
