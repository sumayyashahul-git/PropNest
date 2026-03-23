import { Injectable } from '@angular/core'
import { HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable } from 'rxjs'

@Injectable({
  providedIn: 'root'
})
export class ApiService {

 private AUTH_URL = '/auth-api/api'
private PROPERTY_URL = '/property-api/api'
private ANALYTICS_URL = '/analytics-api/api'

  constructor(private http: HttpClient) {}

  private getHeaders(): HttpHeaders {
    const token = localStorage.getItem('token')
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    })
  }

  login(data: any): Observable<any> {
    return this.http.post(`${this.AUTH_URL}/Auth/login`, data)
  }

  getProperties(): Observable<any> {
    return this.http.get(`${this.PROPERTY_URL}/Properties`, {
      headers: this.getHeaders()
    })
  }

  getDashboardStats(): Observable<any> {
    return this.http.get(
      `${this.ANALYTICS_URL}/Analytics/dashboard`,
      { headers: this.getHeaders() }
    )
  }

  getTopViewed(): Observable<any> {
    return this.http.get(
      `${this.ANALYTICS_URL}/Analytics/top-viewed`,
      { headers: this.getHeaders() }
    )
  }

  getRecentListings(): Observable<any> {
    return this.http.get(
      `${this.ANALYTICS_URL}/Analytics/recent-listings`,
      { headers: this.getHeaders() }
    )
  }
} 