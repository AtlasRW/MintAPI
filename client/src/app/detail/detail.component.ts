import { Component, OnDestroy, OnInit } from '@angular/core';
import { MessageService } from '../message/message.service';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Message } from '../message/interfaces/Message';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-detail',
  templateUrl: './detail.component.html'
})
export class DetailComponent implements OnInit, OnDestroy {

  message$!: Subscription;
  message!: Message;
  messageArchive: Message = {
    isDraft: true,
    title: '',
    header: null,
    body: null,
    background: '',
    startDate: null,
    endDate: null,
    icon: null,
    action: null,
    categories: []
  };

  constructor(
    public readonly service: MessageService,
    private readonly route: ActivatedRoute,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.message$ = this.route.params.subscribe(params => this.onRouteUpdate(params));
  }

  ngOnDestroy(): void {
    this.message$.unsubscribe();
  }

  onRouteUpdate(pRouteParams: Params): void {
    if (pRouteParams.id === 'new')
      this.message = { ...this.messageArchive };
    else
      this.service.getMessageById(pRouteParams.id)
        .then(message => this.message = message);
  }

  onMessageUpdate(): void {
    this.service.updateOrInsertMessage(this.message)
      .then(message => this.router.navigateByUrl(`/${message.id}`));
  }

  onMessageCancel(): void {
    if (this.message.id)
      this.service.getMessageById(this.message.id)
        .then(message => this.message = message);
    else
      this.message = { ...this.messageArchive };
  }

}
