import {UserBase} from './user-base';

export interface User extends UserBase
{
  /**
   * 用户令牌
   */
  token: string;
}
