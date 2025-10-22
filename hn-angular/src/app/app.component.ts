import { Component } from '@angular/core';
import { HackerNewsComponent } from './hacker-news/hacker-news.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [HackerNewsComponent],
  template: '<app-hacker-news></app-hacker-news>'
})
export class AppComponent { }
