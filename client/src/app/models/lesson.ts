import {DescribedElementSummary} from './common/described-element-summary';
import {ElementSummary} from './common/element-summary';
import {Resource} from './resource';

export interface Lesson extends DescribedElementSummary
{
  course: ElementSummary;
  resources: Resource[];
}
