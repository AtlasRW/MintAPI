import { Component, OnInit } from '@angular/core';
import { MessageListFilters } from '../interfaces/MessageListFilters';
import { Category } from '../message/interfaces/Category';
import { MessageService } from '../message/message.service';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html'
})
export class ListComponent implements OnInit {

  filterCategories: Category[] = [];
  filterIsCurrent: boolean | null = null;
  filterIsDraft: boolean | null = null;
  isRefreshSpinning: boolean = false;
  newCategory: Category = { title: '' };

  constructor(
    public readonly service: MessageService
  ) {}

  ngOnInit(): void {
    this.service.getAllMessages({ categories: [] }).then(messages => this.service.messages = messages);
  }

  filterMessages(): void {
    this.isRefreshSpinning = true;
    this.service.getAllMessages({
      isCurrent: this.filterIsCurrent,
      isDraft: this.filterIsDraft,
      categories: this.filterCategories.length > 0
        ? this.filterCategories.map(category => category.id!)
        : []
    })
      .then(messages => {
        this.service.messages = messages;
        this.isRefreshSpinning = false;
      });
  }

  updateFilters(pFilters: MessageListFilters): void {
    this.filterIsCurrent = pFilters.isCurrent;
    this.filterIsDraft = pFilters.isDraft;
  }

  onNewCategory(): void {
    this.service.insertCategory(this.newCategory)
      .then(category => { if (category) this.service.categories.push(category); });

    this.newCategory = { title: '' };
  }

}
