import { Action } from './Action';
import { Category } from './Category';
import { Icon } from './Icon';

export interface MessageDTO {

  id?: number;
  isDraft: boolean;
  title: string;
  header: string | null;
  body: string | null;
  background: string | null;
  startDate: Date | string | null;
  endDate: Date | string | null;
  icon: number | null;
  action: number | null;
  categories: number[];

}
