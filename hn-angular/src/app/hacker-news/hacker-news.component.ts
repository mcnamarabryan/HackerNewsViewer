import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HackerNewsService, PagedStories } from './hacker-news.service';

interface Story {
  id: number;
  title: string;
  url: string;
}

@Component({
  selector: 'app-hacker-news',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <input [(ngModel)]="searchTerm" (ngModelChange)="search()" placeholder="Search stories" />
    <ul>
      <li *ngFor="let story of currentStories">
        <a [href]="story.url" target="_blank">{{ story.title }}</a>
      </li>
    </ul>
    <div *ngIf="totalPages > 0">
      <button (click)="previousPage()" [disabled]="page === 1">Previous</button>
      <span>Page {{ page }} of {{ totalPages }}</span>
      <button (click)="nextPage()" [disabled]="page === totalPages">Next</button>
    </div>
  `,
  styleUrls: []
})
export class HackerNewsComponent implements OnInit {
  currentStories: Story[] = [];
  page = 1;
  pageSize = 20;
  total = 0;
  searchTerm = '';

  constructor(private service: HackerNewsService) { }

  ngOnInit(): void {
    this.loadData();
  }

  search(): void {
    this.page = 1;
    this.loadData();
  }

  previousPage(): void {
    if (this.page > 1) {
      this.page--;
      this.loadData();
    }
  }

  nextPage(): void {
    if (this.page < this.totalPages) {
      this.page++;
      this.loadData();
    }
  }

  get totalPages(): number {
    return Math.ceil(this.total / this.pageSize);
  }

  private loadData(): void {
    this.service.getStories(this.searchTerm, this.page, this.pageSize).subscribe({
      next: (data: PagedStories) => {
        this.currentStories = data.stories;
        this.total = data.total;
      },
      error: (err: unknown) => console.error(err)
    });
  }
}
