import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GameService {
  apiUrl: string = 'https://localhost:44300/api';

  constructor(private http: HttpClient) {}

  getShips(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/game`);
  }

  fireShot(row: number, col: number): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/game`, { row, col });
  }

  resetGame(): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/game/reset`, null);
  }
}
