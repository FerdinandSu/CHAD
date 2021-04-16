import {EventEmitter, Injectable} from '@angular/core';
import {ElementSummary} from '../../models/common/element-summary';
import {Observable} from 'rxjs';
import {ModalJquery} from '../../jquery/modal-jquery';
import {environment} from '../../../environments/environment';
import {HttpClient} from '@angular/common/http';
import {Resource} from '../../models/resource';
import {MessageService} from '../message.service';
import {tap} from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ResourceService
{

  public resources: ElementSummary[] = [];
  private onResourceUpdate: EventEmitter<ElementSummary> | undefined;
  fileName: string;
  selectedValue: ElementSummary | undefined;
  private fileUpload: File | undefined;
  private resUrl = `${environment.apiUrl}/res`;
  uploadNew = false;

  constructor(private http: HttpClient,
              private msg: MessageService)
  {
    this.fileName = '请选择文件...';
  }

  getAll(): Observable<Resource[]>
  {
    return this.http.get<Resource[]>(
      this.resUrl
    );
  }

  check(): boolean
  {
    return this.uploadNew ?
      this.fileName !== '请选择文件...' :
      this.selectedValue !== undefined;
  }

  public showDialog(): EventEmitter<ElementSummary>
  {
    this.getAll().subscribe(
      r => this.resources = r
    );
    this.selectedValue = undefined;
    this.fileUpload = undefined;
    this.fileName = '请选择文件...';
    this.onResourceUpdate = new EventEmitter<ElementSummary>();
    ($('#add-resource-dialog') as ModalJquery).modal('show');
    return this.onResourceUpdate;
  }

  selectFile(files: any): void
  {
    this.fileUpload = files.target.files[0];
    if (this.fileUpload)
    {
      this.fileName = this.fileUpload.name;
    }

  }

  public downloadResource(target: ElementSummary): void
  {
    this.http.get(`${this.resUrl}/${target.id}`, {
      responseType: 'blob'
    }).subscribe(blob =>
    {
      const resDownload = document.createElement('a');
      const objectUrl = URL.createObjectURL(blob);
      resDownload.href = objectUrl;
      resDownload.download = target.name;
      resDownload.click();
      URL.revokeObjectURL(objectUrl);
    });
  }

  public deleteResource(id: string): Observable<any>
  {
    this.http.get('/api/res/1');
    console.log('run delete res ' + `${this.resUrl}/${id}`);

    return this.http.delete<any>(`${this.resUrl}/${id}`);

  }

  public addResource(): void
  {
    if (this.uploadNew)
    {
      if (this.fileUpload === undefined)
      {
        console.error('NULL File!');
        return;
      }
      const formData = new FormData();

      formData.append('file', this.fileUpload);

      this.http.post<Resource>(this.resUrl, formData)
        .subscribe(r =>
          this.onResourceUpdate?.emit({
            name: r.name,
            id: r.id
          })
        );
    }
    else
    {
      this.onResourceUpdate?.emit(this.selectedValue);
    }
  }
}
