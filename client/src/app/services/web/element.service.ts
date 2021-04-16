import {EventEmitter, Injectable} from '@angular/core';
import {ElementSummary} from '../../models/common/element-summary';
import {Observable, of} from 'rxjs';
import {ModalJquery} from '../../jquery/modal-jquery';
import {DescribedElementSummary} from '../../models/common/described-element-summary';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ElementService
{
  public elements: ElementSummary[] = [];
  public elementTypeName = '资源';
  private onSubmitted: EventEmitter<ElementSummary> | undefined;
  private onCreate:EventEmitter<DescribedElementSummary>|undefined;
  public selectedValue: ElementSummary | undefined;
  public newValue: DescribedElementSummary | undefined;

  private lessonUrl = `${environment.apiUrl}/lesson`;
  private classUrl = `${environment.apiUrl}/class`;
  private courseUrl = `${environment.apiUrl}/course`;

  constructor(private http: HttpClient)
  {
  }

  getAll(typeName: string): Observable<ElementSummary[]>
  {
    switch (typeName)
    {
      case '课程':
        return this.http.get<ElementSummary[]>(this.courseUrl);
      default:
        return this.http.get<ElementSummary[]>(this.classUrl);
    }
  }

  public showAddDialog(typeName: string): EventEmitter<DescribedElementSummary>
  {
    this.newValue = {
      id: '-1',
      description: '',
      name: ''
    };
    this.onCreate = new EventEmitter<DescribedElementSummary>();
    this.elementTypeName = typeName;
    ($('#add-element-dialog') as ModalJquery).modal('show');
    return this.onCreate;
  }

  public showSelectDialog(typeName: string): EventEmitter<ElementSummary>
  {
    this.getAll(typeName).subscribe(
      r => this.elements = r
    );
    this.selectedValue = undefined;
    this.onSubmitted = new EventEmitter<ElementSummary>();
    this.elementTypeName = typeName;
    ($('#select-element-dialog') as ModalJquery).modal('show');
    return this.onSubmitted;
  }

  public selectElement(): void
  {
    this.onSubmitted?.emit(this.selectedValue);
  }

  public addElement(): void
  {
    this.onCreate?.emit(this.newValue);
  }
}
