# DI Sandbox — learning exercise

Owner: apprentice. Reviewer: Jørgen.

This is a warm-up before [ChatManager-Spec.md](ChatManager-Spec.md). Its goal is not to ship anything — it's to give you a small, safe project where dependency injection lifetimes stop being three words in a docs page and start being something you can see with your own eyes.

**Keep this project.** It stays on your machine as a permanent playground. Section 7 lists further experiments you can come back to over the next months whenever a new DI concept confuses you. Add your own experiments to it. Break things on purpose. That's what it's for.

You should not paste code you didn't write. Everything you type, you type yourself. If you get stuck, ask — don't copy.

---

## 1. Why this exists

`ChatSession` is registered as `Scoped` in [Program.cs](../kisatsingen/Program.cs). When you build `ChatManager`, you will have to decide its lifetime and defend the choice. You cannot make that call by reading three sentences about "transient vs scoped vs singleton". You need to have watched the difference happen.

That's what this project does. Three services, one endpoint, refresh a browser tab, watch the numbers.

## 2. What you will be able to answer at the end

Come back to Jørgen when you can answer all of these without hedging:

1. What does "scope" mean in ASP.NET Core? Who creates a scope, and when?
2. What is the difference between `AddSingleton`, `AddScoped`, and `AddTransient` — in one sentence each, in terms of *when a new instance is created*?
3. Why does scoped look identical to transient if you only resolve each service once per request? What's the setup that makes scoped visibly different?
4. In Blazor Server, what is the "scope" that `AddScoped` binds to? (Hint: it's not what it is in a Web API.) Read the answer in `Program.cs` and confirm against the framework docs.
5. If you register a singleton that depends on a scoped service, what happens at startup? Why?

## 3. Setup

- Create a new project: `dotnet new web` in the DOTNET-MORO dir (or on root here). Call it whatever you like (`DOTNET-MORO` works).
- One file: `Program.cs`. Everything lives there for now — no `Services/` folder, no split files. Keep the whole exercise in ~50 lines of code so the wiring is visible at a glance.
- Target the same .NET version this repo targets. Check [kisatsingen.csproj](../kisatsingen/kisatsingen.csproj).

## 4. What to build

You are building three service classes and one endpoint. Do not look up an example — think about the shape from the description.

### The three services
Each service is a plain class. Each one holds:
- An `Id` field of type `Guid`, assigned to `Guid.NewGuid()` in its constructor.
- A `Count` field of type `int`.
- A method (name it what you like) that increments `Count` and returns the new value.

Register the three services in `Program.cs` with three different lifetimes — one as singleton, one as scoped, one as transient. Name the classes so the lifetime is obvious at a glance (your choice how).

### The middleman
A fourth class that depends on all three counter services (constructor injection). It exposes a method that reads each counter's `Id` and returns them. Register it as scoped.

The middleman exists for one reason: **it lets a single HTTP request resolve each counter twice — once directly from the endpoint, once via the middleman**. Without a second resolution point inside the same scope, you cannot see the difference between scoped and transient. Understand *why* before you write the class.

### The endpoint
One `GET /` endpoint. It:
- Receives all three counters and the middleman via parameter injection (minimal APIs support this — read the docs if you haven't).
- Calls each counter's increment method once directly.
- Asks the middleman for its view of the ids.
- Returns a JSON object containing, for each counter: the id the endpoint saw, the id the middleman saw, and the current count.

That's the whole app.

## 5. What to observe

Run it. Open `/` in a browser. Refresh five times. For each service, answer:

- Does the id from the endpoint match the id from the middleman **within a single request**?
- Does the id stay the same **across refreshes**?
- Does the count go up by 1 or by 2 per refresh?

Write down your predictions **before** you run it. Then run it and compare. If any prediction was wrong, don't move on until you understand why.

Extra observation: open a private/incognito window and hit `/` from there. Which of the three ids change? Which don't? Explain it in one sentence.

## 6. Definition of done for the base exercise

- [ ] The project runs and returns JSON when you hit `/`.
- [ ] You can predict what the JSON will contain before you refresh, for all three lifetimes.
- [ ] You have answered the five questions in §2 out loud to Jørgen.
- [ ] You have read [Program.cs:87-89](../kisatsingen/Program.cs#L87-L89) and explained why `ChatSession` is registered as scoped, and what "scoped" means specifically in a Blazor Server app (it is not the same as in a Web API — this is important, and it is the bridge to your real task).

## 7. Further experiments (self-directed, come back over time)

Do these in the same project. Add each as a new endpoint (`/experiment-1`, `/experiment-2`, ...) so you can flip between them without losing earlier work. Each experiment is a paragraph, not a recipe — you decide the shape.

**7.1 Interface and implementation.** Introduce an interface for one of the counter services. Register the interface → implementation binding. What changes at the call site? What does this buy you that the concrete registration didn't?

**7.2 Two implementations of one interface.** Register two different implementations of the same interface. What does `sp.GetRequiredService<IThing>()` return? What does `sp.GetServices<IThing>()` return? When would you want each?

**7.3 Factory registration.** Register a service using the factory overload: `services.AddSingleton<T>(sp => new T(...))`. Use it to construct a service that needs a value that isn't itself in the DI container (e.g. a string from configuration). This is the escape hatch for services that can't be built by "just call the constructor".

**7.4 The captive dependency trap.** Register a singleton service that depends on your `ScopedCounter`. Run the app. Read the exception. Understand why the framework refuses this at startup and what it protects you from. Then find the flag that turns the check off and *don't use it* — but know it exists.

**7.5 Manual scopes.** Add a background loop (a `Task.Run` on startup, or better, an `IHostedService`) that needs to use your `ScopedCounter`. It can't inject it directly — a singleton/hosted service outlives any single scope. Use `IServiceScopeFactory` to create a scope on each iteration. Watch the id change.

**7.6 IOptions and configuration.** Bind a POCO to a section of `appsettings.json` and inject it as `IOptions<T>` into a service. Then try `IOptionsSnapshot<T>` and `IOptionsMonitor<T>` — what are the lifetime and reload semantics of each?

**7.7 Keyed services (.NET 8+).** Register two implementations of the same interface with different keys. Resolve them by key at the call site. This is the modern replacement for the "resolve by tag" trick in 7.2 — worth knowing which one to reach for.

Each of these is a real thing you will meet in production code. Doing them in a sandbox is 20 minutes each. Doing them for the first time in a real feature is a wasted afternoon.

## 8. What NOT to do

- Do not add EF Core, authentication, or a database. This is about DI, not about building an app.
- Do not split the code into a `Services/` folder. Keep everything in `Program.cs` until it hurts.
- Do not use Blazor for the sandbox. The Blazor scoping story is subtler than Web API scoping — you learn Web API scoping first, *then* read `Program.cs` in the real repo to understand how Blazor differs. Doing Blazor here re-introduces the noise the sandbox exists to strip away.
- Do not copy code from the internet or from this repo. Look up API shapes (parameter names, method signatures) — but the wiring is yours to write.

## 9. Bridge back to the real codebase

When you're done, before you touch `ChatManager`, re-read:

- [Program.cs:87-89](../kisatsingen/Program.cs#L87-L89) — the three lines that register the chat services.
- [ChatSession.cs](../kisatsingen/Services/Chat/ChatSession.cs) — top of the class, notice the injected dependencies and their lifetimes.

You should now be able to answer §1 of [ChatManager-Spec.md](ChatManager-Spec.md) open questions ("Scoped, Singleton or Transient? Why?") with a reason, not a guess.
