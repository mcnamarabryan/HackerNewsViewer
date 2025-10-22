import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

interface PagedStories {
  stories: { id: number; title: string; url: string }[];
  total: number;
}

@Injectable({
  providedIn: 'root'
})
export class HackerNewsService {
  private apiUrl = '/api/stories';

  constructor(private http: HttpClient) { }

  getStories(search: string, page: number, pageSize: number): Observable<PagedStories> {
    let url = `${this.apiUrl}?page=${page}&pageSize=${pageSize}`;
    if (search) {
      url += `&search=${encodeURIComponent(search)}`;
    }
    return this.http.get<PagedStories>(url);
  }
}
