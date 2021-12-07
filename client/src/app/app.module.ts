import { environment } from 'src/environments/environment';
import { ApplicationRef, DoBootstrap, LOCALE_ID } from '@angular/core';
import { APP_BASE_HREF } from '@angular/common';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { createCustomElement } from '@angular/elements';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Injector, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MessageModule } from './message/message.module';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DetailModule } from './detail/detail.module';
import { IconService } from './detail/icon.service';
import { ListModule } from './list/list.module';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserAnimationsModule,
    BrowserModule,
    AppRoutingModule,
    MessageModule,
    DetailModule,
    ListModule,
    MatProgressSpinnerModule
  ],
  providers: [
    { provide: APP_BASE_HREF, useValue: environment.base },
    { provide: MAT_DATE_LOCALE, useValue: 'fr-FR' },
    { provide: LOCALE_ID, useValue: 'fr-FR' },
    IconService
  ]
})
export class AppModule implements DoBootstrap {

  constructor(private injector: Injector) {
    customElements.define('app-dashboard-builder', createCustomElement(AppComponent, { injector }));
  }

  ngDoBootstrap(app: ApplicationRef): void {
    if (!environment.production)
      app.bootstrap(AppComponent);
  }

}
