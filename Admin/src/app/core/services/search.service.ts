import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  baseUrl = environment.apiUrl;
  private searchQuerySubject = new BehaviorSubject<string>('');
  searchQuery$: Observable<string> = this.searchQuerySubject.asObservable();

  constructor(private http: HttpClient) { }

  search(query: string, module: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}?q=${query}&module=${module}`);
  }

  setSearchQuery(query: string) {
    this.searchQuerySubject.next(query);
  }
}
