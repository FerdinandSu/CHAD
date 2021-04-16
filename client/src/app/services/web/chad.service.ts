import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {environment} from '../../../environments/environment';
import {Observable} from 'rxjs';
import {Course} from '../../models/course';
import {Lesson} from '../../models/lesson';
import {Class} from '../../models/class';
import {DescribedElementSummary} from '../../models/common/described-element-summary';
import {ElementSummary} from '../../models/common/element-summary';
import {UserSummary} from '../../models/user-summary';

@Injectable({
  providedIn: 'root'
})
export class ChadService
{
  private lessonUrl = `${environment.apiUrl}/lesson`;
  private classUrl = `${environment.apiUrl}/class`;
  private courseUrl = `${environment.apiUrl}/course`;

  constructor(private http: HttpClient)
  {
  }

  public getCourse(id: string): Observable<Course>
  {
    return this.http.get<Course>(`${this.courseUrl}/${id}`);
  }

  public getLesson(id: string): Observable<Lesson>
  {
    return this.http.get<Lesson>(`${this.lessonUrl}/${id}`);
  }

  public getClass(id: string): Observable<Class>
  {
    return this.http.get<Class>(`${this.classUrl}/${id}`);
  }

  public addCourse(v: DescribedElementSummary): Observable<ElementSummary>
  {
    return this.http.post<ElementSummary>(
      this.courseUrl,
      v
    );
  }

  public addLesson(courseId: string, index: number, v: DescribedElementSummary)
    : Observable<ElementSummary>
  {
    return this.http.post<ElementSummary>(
      `${this.courseUrl}/${courseId}/lesson/${index}`,
      v
    );
  }

  public includeClass(courseId: string, v: ElementSummary): Observable<any>
  {
    return this.http.post(
      `${this.courseUrl}/${courseId}/class`,
      v
    );
  }
  public excludeClass(courseId: string, v: ElementSummary): Observable<any>
  {
    return this.http.delete(
      `${this.courseUrl}/${courseId}/class/${v.id}`
    );
  }
  public includeResource(lessonId: string, v: ElementSummary): Observable<any>
  {
    return this.http.post(
      `${this.lessonUrl}/${lessonId}/res`,
      v
    );
  }
  public excludeResource(lessonId: string, v: ElementSummary): Observable<any>
  {
    return this.http.delete(
      `${this.lessonUrl}/${lessonId}/res/${v.id}`
    );
  }
  public includeStudent(classId: string, v: UserSummary): Observable<any>
  {
    return this.http.post(
      `${this.classUrl}/${classId}/student`,
      v
    );
  }
  public excludeStudent(classId: string, v: UserSummary): Observable<any>
  {
    return this.http.delete(
      `${this.classUrl}/${classId}/student/${v.username}`
    );
  }
  public addClass(v: ElementSummary): Observable<ElementSummary>
  {
    return this.http.post<ElementSummary>(
      this.classUrl,
      v
    );
  }

  public deleteCourse(id: string): Observable<any>
  {
    return this.http.delete(`${this.courseUrl}/${id}`);
  }

  public deleteLesson(id: string): Observable<any>
  {
    return this.http.delete(`${this.lessonUrl}/${id}`);
  }

  public deleteClass(id: string): Observable<any>
  {
    return this.http.delete(`${this.classUrl}/${id}`);
  }
}
