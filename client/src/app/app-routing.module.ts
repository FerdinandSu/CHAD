import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {HomeComponent} from './home/home.component';
import {ManagerComponent} from './manager/manager.component';
// import {UserManagerComponent} from './manager/user-manager/user-manager.component';
import {LoginComponent} from './login/login.component';
import {ForbiddenComponent} from './shared/static/forbidden/forbidden.component';
import {NotfoundComponent} from './shared/static/notfound/notfound.component';
import {CoursesComponent} from './courses/courses.component';
import {LessonsComponent} from './lessons/lessons.component';
import {ClassesComponent} from './classes/classes.component';
import {ChatComponent} from './chat/chat.component';
import {AdminGuard} from './guards/admin.guard';
import {AuthGuard} from './guards/auth.guard';

const routes: Routes = [
  {path: '', redirectTo: '/Home', pathMatch: 'full'},
  {path: 'Home', component: HomeComponent, canActivate: [AuthGuard]},
  {path: 'Forbidden', component: ForbiddenComponent},
  {path: 'NotFound', component: NotfoundComponent},
  /*  {path: 'About', component: AboutComponent},*/
  {path: 'Login', component: LoginComponent},
  {path: 'Courses/:id', component: CoursesComponent},
  {path: 'Lessons/:id', component: LessonsComponent},
  {path: 'Classes/:id', component: ClassesComponent},
  {path: 'Chat', component: ChatComponent},
  {path: 'Manager', component: ManagerComponent, canActivate: [AdminGuard]},
  // otherwise redirect to home
  {path: '**', redirectTo: ''}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule
{
}
