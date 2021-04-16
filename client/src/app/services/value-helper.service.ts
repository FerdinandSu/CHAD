import {Injectable} from '@angular/core';
import {UserRole} from '../models/common/user-role.enum';
@Injectable({
  providedIn: 'root'
})
/**
 * 帮助生成评估标准的值的服务
 */
export class ValueHelperService
{
  constructor()
  {
  }
  private static strInArr(str: string, arr: string[]): boolean {
    for (const s of arr) {
      if (s === str) {
        return true;
      }
    }
    return false;
  }
  /**
   * 格式化输出UserRole
   * @param role 用户角色
   */
  formatUserRole(role: UserRole): string
  {
    switch (role)
    {
      case UserRole.Administrator:
        return '管理员';
      case UserRole.Teacher:
        return '教师';
      case UserRole.All:
        return '用户';
      default:
        return '学生';
    }
  }
  getFileIcon(fileName: string): string {
    const spl = fileName.split('.');
    const defaultIcon = 'fas fa-file file-icon';
    if (spl.length === 0) {
      return defaultIcon;
    }
    const extension = spl[spl.length - 1].toLowerCase();
    switch (true) {
      case extension === 'pdf':
        return 'fas file-icon fa-file-pdf ';
      case ValueHelperService.strInArr(extension, [
        'doc', 'docx', 'wps', 'rtf'
      ]):
        return 'fas file-icon fa-file-word';
      case ValueHelperService.strInArr(extension, [
        'ppt', 'pptx'
      ]):
        return 'fas file-icon fa-file-powerpoint';
      case ValueHelperService.strInArr(extension, [
        'txt', 'log',
      ]):
        return 'fas file-icon fa-file-alt';
      case ValueHelperService.strInArr(extension, [
        'xls', 'xlsx'
      ]):
        return 'fas file-icon fa-file-excel';
      case ValueHelperService.strInArr(extension, [
        'rar', 'zip', '7z', 'gz', 'tgz', 'bz', 'z'
      ]):
        return 'fas file-icon fa-file-archive';
      case ValueHelperService.strInArr(extension, [
        'bmp', 'png', 'gif', 'jpg', 'pic', 'tif'
      ]):
        return 'fas file-icon fa-file-image';
      case ValueHelperService.strInArr(extension, [
        'mkv', 'mp4', 'm4v', 'avi', 'swf', 'mpg', 'mov'
      ]):
        return 'fas file-icon fa-file-video';
      case ValueHelperService.strInArr(extension, [
        'c', 'cpp', 'h', 'hpp', 'vb', 'java', 'py', 'cs', 'pl',
        'js', 'ts', 'html', 'xml', 'json', 'css', 'ps1', 'sh',
        'bat'
      ]):
        return 'fas file-icon fa-file-code';
      case extension === 'csv':
        return 'fas file-icon fa-file-csv ';
      case ValueHelperService.strInArr(extension, [
        'wav', 'mp3', 'm4a', 'aif', 'au', 'wma', 'flac', 'ape', 'aac'
      ]):
        return 'fas file-icon fa-file-audio';
      default:
        return defaultIcon;
    }
  }

  // tslint:disable:no-bitwise
  formatFileSize(length: number): string {
    switch (true) {
      case length < 1 << 10:
        return `${length}字节`;
      case length < 1 << 20:
        return `${(length / (1 << 10)).toFixed(2)}KB`;
      case length < 1 << 30:
        return `${(length / (1 << 20)).toFixed(2)}MB`;
      default:
        return 'You Know the Rules, and So do I.';
    }
  }
}
