# 🚀 .NET Clean Architecture + DDD Template

This repository is a **lightweight, opinionated .NET template** for building applications using:

* **Clean Architecture**
* **Domain-Driven Design (DDD)**
* **CQRS (Command / Query separation)**
* **Pipeline behaviors (MediatR-like, but without MediatR)**

The goal is to provide a **framework-free core** that is:

* Easy to understand
* Easy to extend
* Suitable for real production systems

---

## 🎯 Goals of this Template

* Keep **Domain** pure and independent
* Keep **Application** free from infrastructure and frameworks
* Allow flexible **cross-cutting concerns** (logging, validation, transactions)
* Avoid hidden magic (reflection-heavy or hard-to-debug patterns)
* Provide a solid foundation for **enterprise-grade systems**

---

## 🧱 Architecture Overview

The solution follows **Clean Architecture boundaries**:

```
Domain        → Pure business logic
Application   → Use cases (CQRS)
Infrastructure→ Technical implementations
Web/API       → Delivery mechanism
```

Dependencies always point **inward**.

---

## 📦 Projects & Structure

### `Domain`

Contains the **core business model**.

* Entities & Aggregate Roots
* Value Objects
* Domain Events
* Domain Exceptions
* Guards
* Strongly-typed IDs
* Clock abstraction (`IClock`)

📌 **No dependencies on frameworks or infrastructure**

---

### `Application`

Contains **use cases and orchestration logic**.

#### Key concepts implemented:

* Commands & Queries
* Command / Query Handlers
* Pipeline Behaviors (MediatR-style)
* Dispatcher abstraction
* `Unit` for command acknowledgements

#### Folder structure (simplified):

```
Application/
 ├─ Shared/
 │   ├─ Messaging/
 │   │   ├─ ICommand.cs
 │   │   ├─ IQuery.cs
 │   │   ├─ ICommandHandler.cs
 │   │   ├─ IQueryHandler.cs
 │   │   ├─ Unit.cs
 │   │   └─ RequestHandlerDelegate.cs
 │   │
 │   ├─ Pipeline/
 │   │   ├─ ICommandPipelineBehavior.cs
 │   │   └─ IQueryPipelineBehavior.cs
 │   │
 │   └─ Dispatching/
 │       └─ IDispatcher.cs
```

📌 **Application does NOT depend on DI containers, EF Core, logging frameworks, etc.**

---

## 🔁 CQRS & Dispatcher

This template uses a **custom dispatcher** inspired by MediatR:

* Commands and Queries are dispatched via `IDispatcher`
* Handlers are resolved by DI (outside Application layer)
* Pipeline behaviors are composed explicitly

### Why not MediatR?

* Full control
* No hidden reflection
* Easier debugging
* Clear learning experience

---

## 🧩 Pipeline Behaviors

Pipeline behaviors allow cross-cutting concerns without polluting handlers:

Examples:

* Logging
* Validation
* Transactions
* Performance monitoring

Commands and Queries have **separate pipelines** by design.

---

## ❌ What This Template Does NOT Do (By Design)

* No EF Core in Domain or Application
* No MediatR dependency
* No Result<T> forcing (exceptions are used for domain invariants)
* No reflection-heavy magic

---

## 🛣 Roadmap

Planned next steps:

### Phase 1 – Core Foundation ✅

* [x] Domain base (Entity, AggregateRoot, ValueObject)
* [x] Domain Events
* [x] Strongly-typed IDs
* [x] CQRS abstractions
* [x] Dispatcher contract
* [x] Pipeline behaviors abstraction

### Phase 2 – Infrastructure

* [ ] Dispatcher implementation (DI-based)
* [ ] Logging pipeline behavior
* [ ] Validation pipeline behavior
* [ ] Transaction behavior (Unit of Work)
* [ ] Clock implementation

### Phase 3 – Examples

* [ ] Sample Aggregate
* [ ] Sample Command + Handler
* [ ] Sample Query + Handler
* [ ] End-to-end request flow

---

## 🧠 Target Audience

This template is for:

* Developers learning **Clean Architecture + DDD**
* Teams who want **control over infrastructure**
* Senior engineers building long-living systems
* Anyone who prefers **clarity over magic**

---

## 📌 Final Notes

This is **not a framework**.
It’s a **starting point** meant to be adapted to your needs.

Feedback, ideas, and improvements are welcome.
