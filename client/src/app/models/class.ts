import {ElementSummary} from './common/element-summary';
import {UserSummary} from './user-summary';

export interface Class extends ElementSummary
{
  director: UserSummary;
  students: UserSummary[];
}
