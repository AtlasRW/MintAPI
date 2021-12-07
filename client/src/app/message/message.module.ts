import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { MessageComponent } from './message.component';
import { MessageService } from './message.service';
import { NgDompurifyModule } from '@tinkoff/ng-dompurify';

@NgModule({
  declarations: [
    MessageComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule,
    NgDompurifyModule
  ],
  providers: [
    MessageService
  ],
  exports: [
    MessageComponent
  ]
})
export class MessageModule { }
