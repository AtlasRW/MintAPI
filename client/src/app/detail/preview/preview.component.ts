import { Component, Input, OnInit } from '@angular/core';
import { Message } from 'src/app/message/interfaces/Message';

@Component({
  selector: 'app-preview',
  templateUrl: './preview.component.html',
  styleUrls: [ './preview.component.scss' ]
})
export class PreviewComponent {

  @Input() message!: Message;

  constructor() {}

}
