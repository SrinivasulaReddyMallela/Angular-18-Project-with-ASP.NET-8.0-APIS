import { NgModule } from '@angular/core';
  
import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { PostSAMLResponseComponent } from './app.PostSAMLResponseComponent';

const routes: Routes = [
  {
    path: '',
    component: PostSAMLResponseComponent
  }
];

@NgModule({
  declarations: [PostSAMLResponseComponent],
  imports: [
    CommonModule,
    RouterModule.forChild(routes)
  ]
})
export class PostSAMLResponseModule { }
