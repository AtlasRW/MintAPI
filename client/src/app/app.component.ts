import { Component, OnInit } from '@angular/core';
import { MessageService } from './message/message.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent implements OnInit {

  constructor(
    public readonly service: MessageService
  ) {}

  ngOnInit(): void {
    this.service.getAllCategories().then(categories => this.service.categories = categories);
    this.service.getAllActions().then(actions => this.service.actions = actions);
    this.service.getAllIcons().then(icons => this.service.icons = icons);
  }

}
