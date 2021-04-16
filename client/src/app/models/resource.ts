import {ElementSummary} from './common/element-summary';

export interface Resource extends ElementSummary {
  uploader: string;
  size: number;
}
