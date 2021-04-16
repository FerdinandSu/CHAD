import {UserSummary} from './user-summary';

export interface ChatMessage
{
  content: string;
  fromCurrentUser: boolean;
  remote: UserSummary|undefined;
  time: string;
}
