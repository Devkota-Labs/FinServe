FinServe/
├── FinServe.sln
│
├── src/                               # Backend (Modular Monolith)
│   ├── FinServe.Api/                  # Single Host API
│   │   ├── Program.cs
│   │   ├── appsettings.json
│   │   ├── /Middlewares
│   │   ├── /Configurations
│   │   ├── /Extensions
│   │   └── /wwwroot                   # Static web files (optional)
│   │
│   ├── Modules/
│   │   ├── Auth/
│   │   │   ├── Auth.Application/
│   │   │   │   ├── Commands/
│   │   │   │   ├── Queries/
│   │   │   │   ├── Handlers/
│   │   │   │   ├── Services/
│   │   │   │   └── Dtos/
│   │   │   ├── Auth.Domain/
│   │   │   │   └── Entities/
│   │   │   ├── Auth.Infrastructure/
│   │   │   │   ├── AuthDbContext.cs
│   │   │   │   └── Migrations/
│   │   │   ├── Auth.Api/
│   │   │   └── Auth.Module.cs
│   │   │
│   │   ├── Admin/
│   │   │   ├── Admin.Application/
│   │   │   ├── Admin.Domain/
│   │   │   ├── Admin.Infrastructure/
│   │   │   ├── Admin.Api/
│   │   │   └── Admin.Module.cs
│   │   │
│   │   ├── Customer/
│   │   │   ├── Customer.Application/
│   │   │   ├── Customer.Domain/
│   │   │   ├── Customer.Infrastructure/
│   │   │   ├── Customer.Api/
│   │   │   └── Customer.Module.cs
│   │   │
│   │   ├── Loan/
│   │   │   ├── Loan.Application/
│   │   │   ├── Loan.Domain/
│   │   │   ├── Loan.Infrastructure/
│   │   │   ├── Loan.Api/
│   │   │   └── Loan.Module.cs
│   │   │
│   │   ├── Document/
│   │   │   ├── Document.Application/
│   │   │   ├── Document.Domain/
│   │   │   ├── Document.Infrastructure/
│   │   │   ├── Document.Api/
│   │   │   └── Document.Module.cs
│   │   │
│   │   ├── Payment/
│   │   │   ├── Payment.Application/
│   │   │   ├── Payment.Domain/
│   │   │   ├── Payment.Infrastructure/
│   │   │   ├── Payment.Api/
│   │   │   └── Payment.Module.cs
│   │   │
│   │   └── Notification/
│   │       ├── Notification.Application/
│   │       ├── Notification.Domain/
│   │       ├── Notification.Infrastructure/
│   │       ├── Notification.Api/
│   │       └── Notification.Module.cs
│   │
│   └── Shared/
│       ├── Shared.Kernel/
│       │   ├── BaseEntity.cs
│       │   ├── DomainEvent.cs
│       │   └── Result.cs
│       ├── Shared.Common/
│       ├── Shared.Security/
│       ├── Shared.Logging/
│       └── Shared.Messaging/
│
├── frontend/                          # Single Frontend App (Customer + Admin)
│   └── webapp/
│       ├── package.json
│       ├── vite.config.js
│       └── src/
│           ├── assets/
│           ├── components/
│           ├── layouts/
│           │   ├── AdminLayout.jsx
│           │   ├── CustomerLayout.jsx
│           │   └── PublicLayout.jsx
│           │
│           ├── pages/
│           │   ├── public/
│           │   │   ├── Login.jsx
│           │   │   └── Register.jsx
│           │   ├── customer/
│           │   │   ├── Dashboard.jsx
│           │   │   └── Loans.jsx
│           │   └── admin/
│           │       ├── Dashboard.jsx
│           │       ├── Users.jsx
│           │       └── LoanApprovals.jsx
│           │
│           ├── routes/
│           │   ├── ProtectedRoute.jsx
│           │   ├── AdminRoutes.jsx
│           │   └── CustomerRoutes.jsx
│           │
│           ├── services/
│           │   ├── api.js
│           │   ├── authService.js
│           │   ├── customerService.js
│           │   ├── adminService.js
│           │   └── loanService.js
│           │
│           ├── context/
│           │   └── AuthContext.jsx
│           │
│           ├── hooks/
│           │   └── useAuth.js
│           │
│           └── utils/
│               └── roleUtils.js
│
│
├── mobile/                             # Future Mobile App (optional now)
│   ├── react-native/
│   │   ├── App.js
│   │   ├── services/
│   │   └── screens/
│   └── flutter/
│       └── lib/
│
├── tests/                               # Automated testing suite
│   ├── Auth.Tests/
│   ├── Customer.Tests/
│   ├── Loan.Tests/
│   ├── Admin.Tests/
│   ├── Document.Tests/
│   ├── Payment.Tests/
│   ├── Notification.Tests/
│   └── Shared.Tests/
│
├── docker/
│   ├── docker-compose.yml
│   ├── FinServe.Api/
│   │   └── Dockerfile
│   └── webapp/
│       └── Dockerfile
│
└── docs/
    ├── architecture.md
    ├── database-schema.md
    ├── api-contracts.md
    └── dev-guidelines.md
