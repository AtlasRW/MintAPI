import { Injectable } from '@angular/core';
import { fromEvent, map, Observable } from 'rxjs';
import { Icon } from '../message/interfaces/Icon';

@Injectable({ providedIn: 'root' })
export class IconService {

  _reader: FileReader = new FileReader();
  icon$: Observable<Icon> = fromEvent(this._reader, 'load').pipe(map(() => this.format()));

  constructor() {}

  public next(file: Blob): void {
    this._reader.readAsDataURL(file);
  }

  public check(file?: Blob): boolean {
    return (file && file.type === 'image/svg+xml') ? true : false;
  }

  private format(): Icon {
    return { data: this._reader.result as string }
  }

}
