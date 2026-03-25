# 🏠 PropNest — UAE Real Estate Microservices Platform

A full-stack real estate platform built with microservices 
architecture, demonstrating enterprise-grade development patterns.

## Architecture

### Backend — 3 Microservices (.NET Core 9)
| Service | Port | Responsibility |
|---------|------|----------------|
| Auth Service | 5002 | JWT authentication, user management |
| Property Service | 5001 | Listings, search, CRUD |
| Analytics Service | 5003 | Views, price drops, dashboard stats |

### Frontend — 3 Applications
| App | Port | Users | Tech |
|-----|------|-------|------|
| Customer Portal | 5173 | Customers browsing properties | React.js + Vite + Tailwind |
| Admin Dashboard | 4200 | Admins viewing analytics | Angular 19 + Tailwind |
| Agent Portal | 5046 | Agents managing listings | .NET Core MVC + Tailwind |

### Python Scripts
| Script | Purpose |
|--------|---------|
| health_monitor.py | Check all 3 services are running |
| seed_properties.py | Generate 100 fake UAE properties |
| analytics_report.py | Export stats to Excel (5 sheets) |

## Tech Stack
- **Backend:** .NET Core 9, Entity Framework Core, SQL Server
- **Auth:** JWT tokens, BCrypt password hashing
- **Frontend:** React.js, Angular 19, .NET Core MVC
- **Styling:** Tailwind CSS
- **Database:** SQL Server (Docker)
- **DevOps:** Docker, GitHub Actions ready
- **Scripting:** Python 3.13, pandas, faker, requests

## Key Concepts Demonstrated
- Microservices architecture
- Event-driven communication (In-memory event bus)
- JWT authentication shared across services
- Repository pattern with EF Core
- CORS handling with proxy configuration
- Database per service pattern
- RESTful API design
- Role-based authorization (Customer, Agent, Admin)

## UAE Domain Knowledge
Built with real UAE real estate context:
- Communities: Downtown Dubai, Palm Jumeirah, Dubai Marina
- Emirates: Dubai, Abu Dhabi, Sharjah, Ajman
- Pricing in AED
- Property types: Apartment, Villa, Townhouse, Penthouse
- Service charges, furnished/unfurnished

## Getting Started

### Prerequisites
- .NET 9 SDK
- Docker Desktop
- Node.js 22+
- Python 3.13+
- Angular CLI

### Run the project

1. Start SQL Server:
```bash
docker start propnest-sql
```

2. Start backend services:
```bash
cd backend/PropNest.AuthService && dotnet run
cd backend/PropNest.PropertyService && dotnet run
cd backend/PropNest.AnalyticsService && dotnet run
```

3. Start frontends:
```bash
cd frontend/propnest-react && npm run dev
cd frontend/propnest-angular && ng serve --proxy-config src/proxy.conf.json
cd frontend/PropNest.AgentPortal && dotnet run
```

4. Run Python scripts:
```bash
cd scripts && venv\Scripts\activate
python health_monitor.py
python seed_properties.py
python analytics_report.py
```

## Author
**Sumayya Shahul** — Senior Full Stack Engineer
- 9 years experience in .NET Core, React, SQL Server, Azure
- Domain expertise in  Real Estate,Contruction,Industrial Construction,Logistics
- GitHub: github.com/sumayyashahul-git
```

---

**Commit everything:**
```
cd "D:\Github Projects\PropNest"
git add .
git commit -m "feat: Python scripts + README complete"
git push
```

---

**How to present this on LinkedIn:**
```
Post this on LinkedIn:

"Just built PropNest — a UAE Real Estate platform using 
microservices architecture!

🏗️ 3 .NET Core 9 microservices
⚛️ React.js customer portal
🅰️ Angular 19 admin dashboard  
🖥️ .NET MVC agent portal
🐍 Python automation scripts
🐳 Docker + SQL Server

All 3 frontends talk to the same backend via JWT auth 
and an event bus.

GitHub: github.com/sumayyashahul-git/PropNest

#dotnet #react #angular #microservices #python #dubai"
