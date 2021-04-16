import {Injectable} from '@angular/core';
import {ChatMessage} from '../../models/chat-message';
import {Observable} from 'rxjs';
import {of} from 'rxjs/internal/observable/of';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {MessageService} from '../message.service';

@Injectable({
  providedIn: 'root'
})
export class ChatService
{
  public messages: ChatMessage[] = [];
  private chatUrl = `${environment.apiUrl}/chat`;

  public newMessage: ChatMessage = {
    content: '',
    time: '',
    remote: undefined,
    fromCurrentUser: true
  };

  getAll(): Observable<ChatMessage[]>
  {
    return this.http.get<ChatMessage[]>(this.chatUrl);
  }

  constructor(private http: HttpClient,
              private msg: MessageService)
  {
  }

  sendMessage(): void
  {
    this.http.post(this.chatUrl, this.newMessage)
      .subscribe(() =>
      {
        this.newMessage = {
          content: '',
          time: '',
          remote: undefined,
          fromCurrentUser: true
        };
        this.msg.addOk('发送成功');
      });
  }

  refresh(): void
  {
    this.getAll().subscribe(msg => this.messages = msg);
  }
}
