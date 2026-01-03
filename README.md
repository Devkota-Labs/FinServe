🚀 FinServe – Enterprise Full-Stack Platform
FinServe is an enterprise-grade full-stack platform built with a Modular Monolith backend and a monorepo-based frontend architecture.
It is designed from day one for:
🔐 Security
📈 Scalability
🧩 Modularity
🧠 Long-term maintainability
🌐 Web & 📱 Mobile readiness
This repository follows strict architectural rules and is intended for production-scale systems, not demos.
🧭 Architecture Overview
Backend
ASP.NET Core
Modular Monolith
Clean Architecture
Event-driven internal communication
Versioned REST APIs
OpenAPI (Swagger) as contract source
Frontend
React / Next.js
Turborepo + pnpm
Feature-based architecture
Thin app shells
Shared business logic via app-core
API-driven RBAC & permissions
🏗️ High-Level System Design
Clients (Web / Mobile / Native)
↓
Frontend (Next.js / Capacitor / React Native)
↓ HTTPS (JWT)
Backend API (ASP.NET Core)
↓
Database (PostgreSQL / MySQL)
📁 Repository Structure
. ├── backend/        # Modular Monolith API
├── frontend/       # Monorepo (Web + Mobile)
├── docs/           # Architecture & documentation
└── README.md
🔙 Backend Architecture
🧩 Architectural Style
Modular Monolith
Single deployable unit
Strong internal boundaries
Domain-Driven Design (DDD)
Event-based communication between modules
📂 Backend Structure
backend/
├── src/
│   ├── BuildingBlocks/
│   │   ├── SharedKernel/
│   │   ├── Result/
│   │   ├── Events/
│   │   └── Logging/
│   │
│   ├── Modules/
│   │   ├── Auth/
│   │   ├── Users/
│   │   ├── Admin/
│   │   ├── Location/
│   │   ├── Notification/
│   │   └── MasterData/
│   │
│   └── ApiHost/
│
└── tests/
📦 Module Internal Structure (Mandatory)
Each backend module must follow this structure:
ModuleName/
├── Module.Domain/
│   ├── Entities
│   ├── ValueObjects
│   ├── Events
│   └── Interfaces
│
├── Module.Application/
│   ├── Commands
│   ├── Queries
│   ├── DTOs
│   ├── Validators
│   └── Handlers
│
├── Module.Infrastructure/
│   ├── Persistence
│   ├── ExternalServices
│   └── Configurations
│
└── Module.Api/
├── Controllers
└── Contracts
🔒 Backend Rules (Strict)
❌ No cross-module Domain references
❌ No direct DB access outside Infrastructure
✅ Communication via Events or Application contracts only
🔐 Backend Security
JWT authentication
Refresh tokens (rotating)
Device-aware sessions
RBAC + permission matrix
Auto logout on 401
Suspicious login detection
Audit logging using Serilog
🌐 Frontend Architecture
🧠 Core Principles
Thin app shells
Feature-based design
Platform isolation
API-driven permissions
Strict separation of concerns
📂 Frontend Monorepo Structure
frontend/
├── apps/
│   ├── web/              # Next.js web shell
│   ├── mobile/           # Capacitor wrapper
│   └── mobile-native/    # React Native app
│
├── packages/
│   ├── app-core/         # Business logic (platform-agnostic)
│   ├── ui/               # Design system
│   ├── config/           # Shared configuration
│   └── tooling/          # ESLint, TS configs
│
├── turbo.json
├── pnpm-workspace.yaml
└── eslint.config.mjs
🧠 app-core (Frontend Business Layer)
packages/app-core/
├── features/
│   ├── auth/
│   ├── admin/
│   └── users/
│
├── shared/
│   ├── api/
│   ├── auth/
│   ├── permissions/
│   ├── logger/
│   └── config/
Rules
❌ No UI rendering
❌ No framework-specific code
❌ No routing logic
✅ Reusable across Web, Capacitor, and React Native
🔐 Frontend Security & RBAC
Permissions fetched from backend APIs
Route-level guards
Layout-level guards
Component-level guards
Backend remains the source of truth
🔁 API & Contract Flow
Backend Domain
↓ DTO
OpenAPI (Swagger)
↓
Generated Frontend Types
↓
Typed API Client
Benefits
Single source of truth
Zero manual sync
Compile-time safety
Versioned APIs
🧪 Quality Gates (Enforced)
Before every push:
ESLint (Flat Config)
TypeScript (strict mode)
Module boundary enforcement
Unit tests
Turbo pipelines
Husky pre-push hooks
🚦 CI / CD Pipeline
git push
↓
lint → typecheck → test → build
↓
deploy
Frontend and backend are independently deployable.
📈 Scalability & Future Readiness
Microservices extraction ready
Native mobile support
Multi-tenant capable
Feature flags supported
White-label friendly
Zero vendor lock-in
📜 Architectural Rules (Non-Negotiable)
No business logic in UI
No API calls outside app-core
No hardcoded permissions
No cross-module domain access
No shared mutable state between modules
🤝 Contributing
Follow the architecture rules
Respect module boundaries
Write tests for business logic
Ensure all quality gates pass
Open PRs with clear descriptions
📄 License
Proprietary
© Devkota Labs. All rights reserved.