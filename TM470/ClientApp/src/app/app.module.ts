import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { ApiAuthorizationModule } from 'src/api-authorization/api-authorization.module';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { AuthorizeInterceptor } from 'src/api-authorization/authorize.interceptor';

import { VenuesComponent } from './venues/venues';
import { FetchVenuesComponent } from './venues/components/fetch-venues';
import { CreateVenueComponent } from './venues/components/create-venue';
import { EditVenueComponent } from './venues/components/edit-venue';

import { EventsComponent } from './events/events';
import { FetchEventsComponent } from './events/components/fetch-events';
import { CreateEventComponent } from './events/components/create-event';
import { EditEventComponent } from './events/components/edit-event';


@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    VenuesComponent,
    FetchVenuesComponent,
    CreateVenueComponent,
    EditVenueComponent,
    EventsComponent,
    FetchEventsComponent,
    CreateEventComponent,
    EditEventComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ApiAuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'venues', component: VenuesComponent, canActivate: [AuthorizeGuard] },
      { path: 'fetch-venues', component: FetchVenuesComponent, canActivate: [AuthorizeGuard] },
      { path: 'create-venue', component: CreateVenueComponent, canActivate: [AuthorizeGuard] },
      { path: 'edit-venue', component: EditVenueComponent, canActivate: [AuthorizeGuard] },
      { path: 'events', component: EventsComponent, canActivate: [AuthorizeGuard] },
      { path: 'fetch-events', component: FetchEventsComponent, canActivate: [AuthorizeGuard] },
      { path: 'create-event', component: CreateEventComponent, canActivate: [AuthorizeGuard] },
      { path: 'edit-event', component: EditEventComponent, canActivate: [AuthorizeGuard] }
    ])
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
