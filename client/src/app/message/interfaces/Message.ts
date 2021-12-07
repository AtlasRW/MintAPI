import { Action } from './Action';
import { Category } from './Category';
import { Icon } from './Icon';

export interface Message {

  id?: number;
  isDraft: boolean;
  title: string;
  header: string | null;
  body: string | null;
  background: string;
  startDate: Date | null;
  endDate: Date | null;
  icon: Icon | null;
  action: Action | null;
  categories: Category[];

}
