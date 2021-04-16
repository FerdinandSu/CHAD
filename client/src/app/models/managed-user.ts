import {UserBase} from './common/user-base';

export interface ManagedUser extends UserBase
{
  /**
   * 用户名
   */
  username: string;
}
