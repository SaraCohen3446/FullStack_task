import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GarageService {
  private apiUrl = `${environment.apiUrl}/Garage`;

  constructor(private http: HttpClient) {}

  getGaragesFromGov(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/fetchFromAPI`);
  }

  getSavedGarages(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/saved`);
  }

  addGarages(garages: any[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/add`, garages);
  }
}
