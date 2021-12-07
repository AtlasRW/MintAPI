import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { DetailComponent } from './detail.component';
import { FormComponent } from './form/form.component';
import { MessageModule } from '../message/message.module';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { PreviewComponent } from './preview/preview.component';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatNativeDateModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatIconModule } from '@angular/material/icon';
import { ColorPickerModule } from 'ngx-color-picker';

@NgModule({
  declarations: [
    DetailComponent,
    FormComponent,
    PreviewComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MessageModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    ColorPickerModule,
    MatCheckboxModule,
    MatNativeDateModule,
    MatDatepickerModule,
    MatProgressSpinnerModule
  ]
})
export class DetailModule { }
