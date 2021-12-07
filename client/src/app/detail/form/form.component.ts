import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { Action } from 'src/app/message/interfaces/Action';
import { Icon } from 'src/app/message/interfaces/Icon';
import { Message } from 'src/app/message/interfaces/Message';
import { MessageService } from 'src/app/message/message.service';
import { IconService } from '../icon.service';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html'
})
export class FormComponent implements OnInit {

  @Input() message!: Message;
  @Output() messageUpdate = new EventEmitter<Message>();
  @Output() messageCancel = new EventEmitter<Message>();
  newAction: Action = { title: '', link: '' };
  colors: string[] = [
    '#FF0000',
    '#00FF00',
    '#0000FF',
    '#00FFFF',
    '#FF00FF',
    '#FFFF00'
  ];

  constructor(
    public readonly service: MessageService,
    private readonly iconService: IconService
  ) {}

  ngOnInit(): void {
    this.iconService.icon$.subscribe(icon => this.onNewIcon(icon));
  }

  nextIcon(event: any): void {
    const file = event.target.files[0];
    if (this.iconService.check(file))
      this.iconService.next(file);
  }

  onNewIcon(pIcon: Icon): void {
    this.service.insertIcon(pIcon)
      .then(icon => this.service.icons.push(icon));
  }

  onNewAction(): void {
    this.service.insertAction(this.newAction)
      .then(action => this.service.actions.push(action));

    this.newAction = { title: '', link: '' };
  }

  publishOrDraftMessage(): void {
    this.message.isDraft = !this.message.isDraft;
    this.updateOrInsertMessage();
  }

  updateOrInsertMessage(): void {
    this.messageUpdate.emit();
  }

  cancelMessage(): void {
    this.messageCancel.emit();
  }

}
