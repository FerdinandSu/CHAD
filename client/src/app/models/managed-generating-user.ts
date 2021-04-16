import {ManagedUser} from './managed-user';

/**
 * 正在生成的用户
 */
export interface ManagedGeneratingUser extends ManagedUser
{
  /**
   * 初始密码
   */
  initialPassword: string;
}
