import { Component } from '@angular/core'
import { CommonModule } from '@angular/common'
import { FormsModule } from '@angular/forms'
import { Router } from '@angular/router'
import { ApiService } from '../../services/api'

@Component({
  selector: 'app-login',
  imports: [CommonModule, FormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class Login{
  email = ''
  password = ''
  error = ''
  loading = false

  constructor(private api: ApiService, private router: Router) {}

  onSubmit() {
    this.loading = true
    this.error = ''

    this.api.login({ email: this.email, password: this.password })
      .subscribe({
        next: (response: any) => {
          if (response.role !== 'Admin') {
            this.error = 'Access denied. Admin account required.'
            this.loading = false
            return
          }
          localStorage.setItem('token', response.token)
          localStorage.setItem('userName', response.fullName)
          localStorage.setItem('userRole', response.role)
          this.router.navigate(['/dashboard'])
        },
        error: () => {
          this.error = 'Invalid email or password.'
          this.loading = false
        }
      })
  }
}