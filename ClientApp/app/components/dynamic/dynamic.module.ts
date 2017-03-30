import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

import { DynamicComponent } from './dynamic.component';
import { DynamicListComponent } from './list/dynamic-list.component';
import { DynamicService } from './services/dynamic.service';

@NgModule({
  imports: [
    CommonModule,
    ReactiveFormsModule,
  ],
  declarations: [
    DynamicComponent,
    DynamicListComponent
  ],
  exports: [
    DynamicComponent,
    DynamicListComponent
  ],
  providers: [
      DynamicService
  ]
})
export class DynamicModule {}
