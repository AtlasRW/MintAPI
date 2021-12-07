import { Injectable, SecurityContext } from '@angular/core';
import { Category } from './interfaces/Category';
import { Message } from './interfaces/Message';
import { Action } from './interfaces/Action';
import { Icon } from './interfaces/Icon';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { lastValueFrom, map } from 'rxjs';
import { DomSanitizer } from '@angular/platform-browser';
import { NgDompurifySanitizer } from '@tinkoff/ng-dompurify';
import { MessageFilters } from './interfaces/MessageFilters';
import { MessageDTO } from './interfaces/Message.dto';

@Injectable({ providedIn: 'root' })
export class MessageService {

  public messages!: Message[];
  public categories!: Category[];
  public actions!: Action[];
  public icons!: Icon[];

  constructor(
    private readonly http: HttpClient,
    private readonly sanitizer: DomSanitizer,
    private readonly domPurify: NgDompurifySanitizer
  ) {}

  public async getAllCurrentMessages(): Promise<Message[]> {
    return lastValueFrom(
      this.http.get<MessageDTO[]>(`${environment.api}/messages`)
        .pipe(map(messages => messages.map(message => this.composeMessage(message))))
    );
  }

  public async getAllMessages(filters: MessageFilters): Promise<Message[]> {
    return lastValueFrom(
      this.http.post<MessageDTO[]>(`${environment.api}/messages`, filters)
        .pipe(map(messages => messages.map(message => this.composeMessage(message))))
    );
  }

  public async getMessageById(pMessageId: number): Promise<Message> {
    return lastValueFrom(
      this.http.get<MessageDTO>(`${environment.api}/messages/${pMessageId}`)
        .pipe(map(message => this.composeMessage(message)))
    );
  }

  public async updateOrInsertMessage(pMessage: Message): Promise<Message> {
    return lastValueFrom(
      this.http.put<MessageDTO>(`${environment.api}/messages`, this.decomposeMessage(pMessage))
        .pipe(map(message => this.composeMessage(message)))
    );
  }

  public async getAllCategories(): Promise<Category[]> {
    return lastValueFrom(this.http.get<Category[]>(`${environment.api}/categories`));
  }

  public async insertCategory(pCategory: Category): Promise<Category> {
    return lastValueFrom(this.http.post<Category>(`${environment.api}/categories`, pCategory));
  }

  public async getAllActions(): Promise<Action[]> {
    return lastValueFrom(this.http.get<Action[]>(`${environment.api}/actions`));
  }

  public async insertAction(pAction: Action): Promise<Action> {
    return lastValueFrom(this.http.post<Action>(`${environment.api}/actions`, pAction));
  }

  public async getAllIcons(): Promise<Icon[]> {
    return lastValueFrom(
      this.http.get<Icon[]>(`${environment.api}/icons`)
        .pipe(map(icons => {
          icons.forEach(icon => this.composeIcon(icon));
          return icons;
        }))
    );
  }

  public async insertIcon(pIcon: Icon): Promise<Icon> {
    return lastValueFrom(
      this.http.post<Icon>(`${environment.api}/icons`, this.decomposeIcon(pIcon))
        .pipe(map(icon => this.composeIcon(icon)))
    );
  }

  private composeMessage(pMessage: MessageDTO): Message {
    return {
      id: pMessage.id,
      title: pMessage.title,
      isDraft: pMessage.isDraft,
      header: pMessage.header || null,
      body: pMessage.body || null,
      background: pMessage.background ? `#${pMessage.background}` : '',
      startDate: pMessage.startDate ? new Date(pMessage.startDate) : null,
      endDate: pMessage.endDate ? new Date(pMessage.endDate) : null,
      icon: this.icons.find(icon => icon.id === pMessage.icon) ?? null,
      action: this.actions.find(action => action.id === pMessage.action) || null,
      categories: pMessage.categories.map(categoryId => this.categories.find(category => category.id === categoryId)!)
    };
  }

  private composeIcon(pIcon: Icon): Icon {
    pIcon.data = this.sanitizer.bypassSecurityTrustResourceUrl(
      this.domPurify.sanitize(SecurityContext.RESOURCE_URL, `data:image/svg+xml;base64,${pIcon.data}`)
    ) as string;
    return pIcon;
  }

  private decomposeMessage(pMessage: Message): MessageDTO {
    return {
      id: pMessage.id,
      title: pMessage.title,
      isDraft: pMessage.isDraft,
      header: pMessage.header || null,
      body: pMessage.body || null,
      background: /^#([A-F0-9]{3}|[A-F0-9]{6})$/i.test(pMessage.background)
        ? pMessage.background.replace(/^#/, '')
        : null,
      startDate: pMessage.startDate,
      endDate: pMessage.endDate,
      icon: pMessage.icon?.id || null,
      action: pMessage.action?.id || null,
      categories: pMessage.categories.map(category => category.id!)
    };
  }

  private decomposeIcon(pIcon: Icon): Icon {
    return {
      id: pIcon.id,
      data: pIcon.data.replace(/^data:.*;base\d*,/i, '')
    };
  }

}
