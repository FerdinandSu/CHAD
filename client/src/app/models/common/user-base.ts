import {UserRole} from './user-role.enum';
import {UserSummary} from '../user-summary';

export interface UserBase extends UserSummary
{
  /**
   * 用户的角色
   */
  role: UserRole;
}
