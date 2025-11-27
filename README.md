# SyncFlow - Offline-First Data Collection App

A Blazor WebAssembly application that enables seamless data collection with full offline capability and intelligent automatic synchronization.

## 🚀 Features

-   ✅ **Offline-First Architecture** - Works without internet connection
-   ✅ **Smart Sync** - Automatic synchronization when connection is restored
-   ✅ **Real-Time Status** - Connection, queue, and sync indicators
-   ✅ **Local Persistence** - Data saved in browser LocalStorage
-   ✅ **Queue Management** - Offline operations queued and synced automatically
-   ✅ **Clean UI** - Modern, responsive interface with status indicators

## 🛠️ Tech Stack

-   **Frontend**: Blazor WebAssembly (.NET 9.0)
-   **Backend**: Blazor Server + ASP.NET Core Web API (.NET 9.0)
-   **Database**: Microsoft SQL Server (LocalDB)
-   **ORM**: Entity Framework Core 9.0
-   **Storage**: Browser LocalStorage (via JavaScript Interop)

## 📋 Requirements

-   .NET 9.0 SDK or higher
-   SQL Server LocalDB (included with Visual Studio)

## 🗄️ Database Schema

### ProductEntry Table

| Column        | Type          | Description                  |
| ------------- | ------------- | ---------------------------- |
| Id            | int           | Primary key (auto-increment) |
| UserName      | string        | Name of user entering data   |
| EntryDateTime | DateTime      | Auto-captured timestamp      |
| ProductModel  | string        | Product model name           |
| PartNumber    | string        | Product part number          |
| Quantity      | int           | Quantity of items            |
| Price         | decimal(18,2) | Price per item               |
| CreatedAt     | DateTime      | Record creation timestamp    |

## 🚀 Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/SiliconCatalyst/SyncFlow.git
cd SyncFlow
```

### 2. Setup Database

```bash
cd Server
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 3. Run the Application

**Terminal 1 - Start Server:**

```bash
cd Server
dotnet run
```

**Terminal 2 - Start Client:**

```bash
cd Client
dotnet run
```

Access the application at the URL shown in the Client terminal (typically `http://localhost:5007`).

## 🎯 How It Works

### Online Mode

1. User submits data through the form
2. Data is sent directly to the API
3. API stores data in SQL Server database
4. Success confirmation shown to user

### Offline Mode

1. User submits data through the form
2. Data is stored in browser LocalStorage with temporary negative ID
3. Operation is added to sync queue
4. Success confirmation shown (no errors!)
5. Data remains accessible in the app

### Reconnection

1. Connection indicator detects internet is back
2. Automatic sync is triggered
3. Queued operations are sent to API in order
4. Temporary IDs are replaced with real database IDs
5. Queue count updates to reflect synced items

## 📊 Status Indicators

The navbar displays three real-time indicators:

-   **Queue: X** - Number of pending operations waiting to sync (blue)
-   **Syncing / Not Syncing** - Current sync status with animated spinner (gray/blue)
-   **Connected / Disconnected** - Internet connection status (green/red)

## 🧪 Testing Offline Mode

1. **Go Offline**: Stop the Server terminal (Ctrl+C)
2. **Add Entries**: Create 2-3 product entries via the form
3. **Verify Queue**: Check navbar shows "Queue: 3"
4. **Go Online**: Restart the Server (`dotnet run` in Server folder)
5. **Watch Sync**: Observe "Syncing" indicator and queue count drop to 0
6. **Verify Data**: Check Database Viewer page - all entries should have real IDs

## 📝 Development Phases

### ✅ Phase 1: Project Setup & Database

-   Created Blazor WASM + ASP.NET Core Web API architecture
-   Configured Entity Framework Core with SQL Server LocalDB
-   Created ProductEntry model and database migrations
-   Set up API controllers with CRUD operations

### ✅ Phase 2: Client UI

-   Built product entry form with validation
-   Created database viewer with table display
-   Implemented navigation and routing
-   Added README.md display with Markdown rendering

### ✅ Phase 3: Offline-First Architecture

-   Implemented LocalStorage service for data persistence
-   Created sync queue system for offline operations
-   Built ProductEntryService with offline/online logic
-   Added connection monitoring with reconnection detection

### ✅ Phase 4: Smart Synchronization

-   Implemented automatic sync on reconnection (offline → online)
-   Added conflict resolution (negative IDs → real IDs)
-   Created sync queue management
-   Built status indicators (connection, queue, sync)

### ✳️ Phase 5: Polish User Experience (In Progress)

-   Adding real-time status indicators in navbar
-   Implementing visual feedback for all operations
-   Styling components with modern CSS
-   Ensuring no errors shown to users when offline

## 🔧 Configuration

### Connection String (appsettings.json)

```json
{
	"ConnectionStrings": {
		"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=SyncFlowDb;Trusted_Connection=true;MultipleActiveResultSets=true"
	}
}
```

### API Base URL (Client)

Update in service classes if your server runs on a different port:

```csharp
private const string ApiUrl = "http://localhost:5188/api/productentries";
```

## 🤝 Contributing

This is a portfolio/interview project demonstrating offline-first architecture. Feel free to fork and experiment!

## 📄 License

Licensed under GNU AGPL v3.0 - see [LICENSE](LICENSE) for details.

## 👤 Author

**Abdulmalek Boulellou**

-   📧 [malek.boulellou@proton.me](mailto:malek.boulellou@proton.me)
-   🐙 [GitHub](https://github.com/SiliconCatalyst)

## 🙏 Acknowledgments

Built as part of a technical interview assessment demonstrating:

-   Offline-first web application architecture
-   Blazor WebAssembly development
-   ASP.NET Core Web API design
-   Entity Framework Core and database migrations
-   Browser storage and synchronization patterns
-   Event-driven architecture
-   Clean code and separation of concerns

---

**Status**:

-   ✅ Phase 1 - Project Setup & Database (Complete)
-   ✅ Phase 2 - Client UI (Complete)
-   ✅ Phase 3 - Offline-First Architecture (Complete)
-   ✅ Phase 4 - Smart Synchronization (Complete)
-   ✳️ Phase 5 - Polish User Experience (In Progress)
