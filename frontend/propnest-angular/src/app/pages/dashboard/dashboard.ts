import { Component, OnInit } from '@angular/core'
import { CommonModule } from '@angular/common'
import { Router, RouterLink } from '@angular/router'
import { ApiService } from '../../services/api'

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  stats: any = null
  topViewed: any[] = []
  recentListings: any[] = []
  userName = localStorage.getItem('userName') || 'Admin'

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit() {
    const token = localStorage.getItem('token')
    if (!token) {
      this.router.navigate(['/login'])
      return
    }
    this.loadData()
  }

  loadData() {
    this.api.getDashboardStats().subscribe({
      next: (data) => this.stats = data,
      error: (err) => console.error('Stats error', err)
    })

    this.api.getTopViewed().subscribe({
      next: (data) => this.topViewed = data,
      error: (err) => console.error('Top viewed error', err)
    })

    this.api.getRecentListings().subscribe({
      next: (data) => this.recentListings = data,
      error: (err) => console.error('Recent error', err)
    })
  }

  logout() {
    localStorage.removeItem('token')
    localStorage.removeItem('userName')
    localStorage.removeItem('userRole')
    this.router.navigate(['/login'])
  }

  formatPrice(price: number): string {
    return new Intl.NumberFormat('en-AE', {
      style: 'currency',
      currency: 'AED',
      maximumFractionDigits: 0
    }).format(price)
  }
}