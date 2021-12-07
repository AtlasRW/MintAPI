import { Component, Input } from '@angular/core';
import { Message } from './interfaces/Message';
import { MessageService } from './message.service';

@Component({
  selector: 'app-dashboard-message',
  templateUrl: './message.component.html',
  styleUrls: [ './message.component.scss' ]
})
export class MessageComponent {

  @Input() message!: Message;

  constructor(
    private readonly service: MessageService
  ) {}

}
