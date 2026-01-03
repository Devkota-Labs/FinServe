# 🤝 Contributing to FinServe

Thank you for your interest in contributing to **FinServe** 🎉  
This project follows **strict enterprise architecture rules**.  
Please read this document carefully before contributing.

> ❗ Contributions that do not follow these rules **will not be accepted**.

---

## 📌 Guiding Principles

- Architecture first
- Security by default
- Clear ownership & boundaries
- Long-term maintainability over short-term speed
- Backend is the source of truth

---

## 🧱 Architecture Rules (Non-Negotiable)

### ❌ Forbidden
- Business logic inside UI components
- API calls outside `app-core`
- Hardcoded permissions or roles
- Cross-module **Domain** references (backend)
- Direct DB access outside Infrastructure
- Sharing mutable state across modules
- Skipping validation or authorization

### ✅ Required
- Feature-based structure
- Strict layer separation
- Typed contracts (DTOs)
- Backend-driven RBAC
- Tests for business logic
- Lint + typecheck must pass

---

## 🏗️ Backend Contribution Guidelines

### 📦 Module Structure (Mandatory)

Every backend module **must** follow:
ModuleName/ ├── Module.Domain/ ├── Module.Application/ ├── Module.Infrastructure/ └── Module.Api/
Copy code

### 🔒 Backend Rules

- Domain layer contains **pure business logic**
- Application layer orchestrates use cases
- Infrastructure handles DB, email, external APIs
- API layer exposes **DTOs only**
- Modules communicate via **events or contracts only**

❌ Never reference another module’s `Domain` directly.

---

## 🌐 Frontend Contribution Guidelines

### 📁 Frontend Structure Rules
apps/        → routing, layouts, guards only packages/ ├── app-core/  → business logic, API, domain ├── ui/        → reusable UI components
Copy code

### ❗ Frontend Rules

- `apps/*` must remain **thin shells**
- All API calls go through `app-core`
- UI must not depend on DTOs directly
- Domain logic must be framework-agnostic
- No routing logic inside `app-core`

---

## 🔐 Security & RBAC

- Permissions always come from backend APIs
- Frontend enforces UX only
- Backend enforces authorization
- Never trust client-side checks alone

---

## 🧪 Quality Gates (Required)

Before opening a Pull Request, ensure:

- ✅ ESLint passes
- ✅ TypeScript strict mode passes
- ✅ Tests are added (where applicable)
- ✅ No architecture violations
- ✅ No unused exports or dead code

Recommended commands:

```bash
pnpm lint
pnpm typecheck
pnpm test
pnpm build
🔁 Git Workflow
Branching Strategy
main → production
develop → integration
feature/<feature-name>
fix/<issue-name>
Commit Message Convention
Copy code

type(scope): short description
Examples:
feat(auth): add password expiry flow
fix(admin): correct role permission mapping
refactor(core): isolate api client
📥 Pull Request Checklist
Before submitting a PR:
[ ] Code follows architecture rules
[ ] No breaking changes without discussion
[ ] Tests added or updated
[ ] Clear PR description
[ ] Screenshots (if UI-related)
🧭 Code Review Process
At least one approval required
Architecture violations = automatic rejection
Security-related changes require extra review
Refactors must not change behavior unless stated
📄 Documentation
If your change affects:
Architecture → update /docs/architecture.md
API contracts → update OpenAPI spec
Developer workflow → update README or docs
🚫 What Not to Contribute
Experimental spikes
Demo-only code
Quick hacks
Unreviewed dependencies
Architecture shortcuts
🆘 Need Help?
If you are unsure:
Ask before implementing
Open a discussion or draft PR
Follow existing patterns
📜 License
By contributing, you agree that your contributions are licensed under the project’s license.
© Devkota Labs. All rights reserved.
Copy code

---

### ✅ This file will:
- Render perfectly on GitHub
- Enforce architectural discipline
- Scale with team size
- Prevent tech debt from day one

If you want next:
- `docs/architecture.md`
- `SECURITY.md`
- `CODE_OF_CONDUCT.md`
- GitHub Actions CI

Just tell me 👍