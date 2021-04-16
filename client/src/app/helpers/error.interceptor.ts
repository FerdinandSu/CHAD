import {Injectable} from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import {Observable, throwError} from 'rxjs';
import {Router} from '@angular/router';
import {catchError} from 'rxjs/operators';
import {MessageService} from '../services/message.service';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor
{

  constructor(private msg: MessageService,
              private router: Router)
  {
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
  {
    return next.handle(request).pipe(
      catchError(err =>
      {
        const error = err.error;
        switch (err.status)
        {
          case 401:
            // noinspection JSIgnoredPromiseFromCall
            this.router.navigate(['/Login']);
            break;
          case 403:
            // noinspection JSIgnoredPromiseFromCall
            this.router.navigate(['/Forbidden']);
            break;
          case 404:
            // noinspection JSIgnoredPromiseFromCall
            this.router.navigate(['/NotFound']);
            break;
          case 500:
            this.msg.addError(error);
            console.error(err);
            break;
          default:

            if (error && error.length > 0)
            {
              this.msg.addError(error);
            }
            console.error(err);
        }


        return throwError(error);
      }));
  }

}
