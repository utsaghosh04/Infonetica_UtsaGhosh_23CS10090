# Infonetica WorkflowEngine

Welcome! This is a minimal, easy-to-understand backend service for building and running configurable workflow state machines. It's designed to be simple, readable, and a great starting point for learning or extending.

## 🚀 Quick Start

1. **Build & Run**
   ```sh
   dotnet run
   ```
   This will start the API server (usually at http://localhost:5000).

2. **API Overview & Examples**

| Method & Path                                 | What it does                                 |
|-----------------------------------------------|----------------------------------------------|
| `POST /workflows`                            | Create or update a workflow definition       |
| `GET /workflows/{id}`                        | Get a workflow definition by ID              |
| `GET /workflows`                             | List all workflow definitions                |
| `POST /instances?definitionId=...`           | Start a new workflow instance                |
| `GET /instances/{id}`                        | Get instance state and action history        |
| `GET /instances`                             | List all workflow instances                  |
| `POST /instances/{id}/actions/{actionId}`    | Execute an action on an instance             |

**Example: Create a workflow (using Postman or PowerShell)**

See the code comments for a sample JSON payload!

## 📝 Assumptions & Notes
- All data is stored in memory (no database needed).
- No authentication required.
- IDs (for states, actions, workflows) must be unique.
- Disabled states/actions are ignored by the engine.
- Each workflow must have exactly one initial state.
- Once an instance reaches a final state, no more actions can be performed.

## 🛠️ Extending & Learning
- Want to save data? Add file-based persistence in `WorkflowService`.
- Need more features? Add properties to the models or new endpoints.
- All code is heavily commented for learning and clarity.

## ❓ Troubleshooting
- If `dotnet run` doesn't work, make sure you have the .NET 8 SDK installed and in your PATH.
- If you get errors about missing IDs or invalid transitions, check your JSON payloads and workflow definitions.

## 📚 Resources
- [Official .NET Documentation](https://learn.microsoft.com/en-us/dotnet/)
- [Minimal APIs in .NET](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)

---

MIT License