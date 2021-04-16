import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {NgbDatepickerModule, NgbDropdownModule, NgbToastModule} from '@ng-bootstrap/ng-bootstrap';
import { MessagesComponent } from './messages/messages.component';
import { ConfirmDialogComponent } from './shared/dialog/confirm-dialog/confirm-dialog.component';
import { NavbarComponent } from './shared/layout/navbar/navbar.component';
import { HomeComponent } from './home/home.component';
import { ManagerComponent } from './manager/manager.component';
import { AddUserDialogComponent } from './manager/add-user-dialog/add-user-dialog.component';
import { LoginComponent } from './login/login.component';
import { ChangePasswordDialogComponent } from './shared/dialog/change-password-dialog/change-password-dialog.component';
import { ForbiddenComponent } from './shared/static/forbidden/forbidden.component';
import { NotfoundComponent } from './shared/static/notfound/notfound.component';
import {HttpClientJsonpModule, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import {TokenInterceptor} from './helpers/token.interceptor';
import {ErrorInterceptor} from './helpers/error.interceptor';
import { ClassesComponent } from './classes/classes.component';
import { CoursesComponent } from './courses/courses.component';
import { LessonsComponent } from './lessons/lessons.component';
import { AddResourceDialogComponent } from './shared/dialog/add-resource-dialog/add-resource-dialog.component';
import { SelectElementDialogComponent } from './shared/dialog/select-element-dialog/select-element-dialog.component';
import { AddElementDialogComponent } from './shared/dialog/add-element-dialog/add-element-dialog.component';
import { SelectUserDialogComponent } from './shared/dialog/select-user-dialog/select-user-dialog.component';
import { ChatComponent } from './chat/chat.component';
@NgModule({
  declarations: [
    AppComponent,
    MessagesComponent,
    ConfirmDialogComponent,
    NavbarComponent,
    HomeComponent,
    ManagerComponent,
    // UserManagerComponent,
    AddUserDialogComponent,
    LoginComponent,
    ChangePasswordDialogComponent,
    ForbiddenComponent,
    NotfoundComponent,
    ClassesComponent,
    CoursesComponent,
    LessonsComponent,
    AddResourceDialogComponent,
    SelectElementDialogComponent,
    AddElementDialogComponent,
    SelectUserDialogComponent,
    ChatComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    NgbDatepickerModule,
    NgbToastModule,
    ReactiveFormsModule,
    NgbDropdownModule,
    HttpClientModule,
    HttpClientJsonpModule
  ],
  providers: [
    {provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true},
    {provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true},
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
