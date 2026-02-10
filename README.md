# FundNavTracker (FundMaster)

MVP .NET MVC web app for tracking Indian mutual fund NAVs with search, plus Nifty/Sensex in the header. The app is structured for future extensions like live updates and notification triggers.

## Features (MVP)
- Fund NAV list with search.
- Header shows Nifty 50 and Sensex (placeholder values).
- SignalR hub endpoint stub for live updates.

## Tech Stack
- ASP.NET Core MVC (.NET 10)
- Razor Views + Bootstrap
- SignalR (real-time updates, stubbed)
- Hangfire (background jobs, reserved for notifications)
- EF Core SQLite (future persistence)

## Quick Start
From the repo root:

```bash
cd src
dotnet restore
dotnet run
```

Open the app at `https://localhost:5001` (or the URL printed by `dotnet run`).

## Key Routes
- `/Nav` - Fund NAV list with search.
- `/` - Home page.

## Configuration
Edit [src/appsettings.json](src/appsettings.json):
- `DataSources:AmfiNavUrl` - AMFI NAV data source (placeholder).
- `DataSources:MarketIndexProvider` - Placeholder for Nifty/Sensex provider.

## Live Tracking (Planned)
SignalR hub is available at `/hubs/nav`. Wire a client to this hub once a real NAV data feed is integrated.

## Notifications (Planned)
Hangfire and EF Core are included for future NAV trigger notifications.
Suggested next steps:
1. Define `NavAlert` and `UserPreference` models.
2. Add a background job to check NAV thresholds.
3. Add email/SMS delivery providers.

## Notes
- NAV and index values are currently stubbed in services and should be replaced with real APIs.
- If you plan to deploy, configure HTTPS certs and production settings as usual for ASP.NET Core.