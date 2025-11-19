# SyncFlow - Offline-First Data Collection App

A hybrid Blazor application that enables data collection on mobile, tablet, or PC with full offline capability and automatic synchronization.

## 🚀 Features

-   ✅ Works online and offline
-   ✅ Automatic data synchronization when connection restored
-   ✅ Cross-platform (Web, Mobile, Desktop)
-   ✅ Real-time data collection and submission

## 🛠️ Tech Stack

-   **Frontend**: Blazor WebAssembly
-   **Backend**: Blazor Server + ASP.NET Core
-   **Database**: Microsoft SQL Server
-   **Database Management**: Azure Data Studio
-   **ORM**: Entity Framework Core
-   **Offline Storage**: Browser LocalStorage/IndexedDB

## 🏃 Getting Started

```bash
# Clone the repository
git clone https://github.com/yourusername/SyncFlow.git

# Restore dependencies
dotnet restore

# Update database connection string in Server/appsettings.json

# Run migrations
cd Server
dotnet ef database update

# Run the applications
dotnet run --project Server
dotnet run --project Client
```

## 📄 License

Licensed under GNU AGPL v3.0 - see [LICENSE](LICENSE) for details.

**Commercial licensing available** - contact me for inquiries.

## 👤 Author

Abdulmalek Boulellou - malek.boulellou@proton.me
