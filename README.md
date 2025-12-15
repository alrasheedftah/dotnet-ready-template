## Goals of this template
You get:

- Pure Domain (no EF, no framework coupling)
- Application layer for use cases (CQRS)
- Infrastructure layer for persistence + outbox + messaging
- Presentation layer (Web API)
- A pipeline system (like MediatR behaviors) but custom
- Reliable EDD via Outbox (at-least-once publishing)
- Clear module structure that scales