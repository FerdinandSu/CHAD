import {ElementSummary} from './common/element-summary';
import {DescribedElementSummary} from './common/described-element-summary';
import {UserSummary} from './user-summary';

export interface Course extends DescribedElementSummary {

  director: UserSummary;
  lessons: ElementSummary[];
  classes: ElementSummary[];
}
