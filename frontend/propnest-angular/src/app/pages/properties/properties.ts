import { Component, OnInit } from '@angular/core'
import { CommonModule } from '@angular/common'
import { Router, RouterLink } from '@angular/router'
import { ApiService } from '../../services/api'

@Component({
  selector: 'app-properties',
  imports: [CommonModule],
  templateUrl: './properties.html',
  styleUrl: './properties.css'
})
export class Properties implements OnInit {
  properties: any[] = []
  total = 0
  loading = true
  userName = localStorage.getItem('userName') || 'Admin'

  propertyTypes = [
    'Apartment', 'Villa', 'Townhouse',
    'Penthouse', 'Studio', 'Office'
  ]

  constructor(private api: ApiService, private router: Router) {}

  ngOnInit() {
    const token = localStorage.getItem('token')
    if (!token) {
      this.router.navigate(['/login'])
      return
    }
    this.loadProperties()
  }

  loadProperties() {
    this.loading = true
    this.api.getProperties().subscribe({
      next: (data) => {
        this.properties = data.items
        this.total = data.total
        this.loading = false
      },
      error: (err) => {
        console.error('Properties error', err)
        this.loading = false
      }
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