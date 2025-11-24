# SyncFlow - Offline-First Data Collection App

A hybrid Blazor application that enables data collection on mobile, tablet, or PC with full offline capability and automatic synchronization.

## 🚀 Features

-   ✅ Works online and offline seamlessly
-   ✅ Automatic data synchronization when connection restored
-   ✅ Cross-platform (Web, Mobile, Desktop)
-   ✅ Real-time data collection and submission
-   ✅ Dual database configuration (Development & Production)

## 🛠️ Tech Stack

-   **Frontend**: Blazor WebAssembly
-   **Backend**: Blazor Server + ASP.NET Core Web API
-   **Database**: Microsoft SQL Server
-   **ORM**: Entity Framework Core 9.0
-   **Offline Storage**: Blazored LocalStorage
-   **Containerization**: Docker (SQL Server)

## 📋 Requirements

-   .NET 9.0 SDK or higher
-   SQL Server (LocalDB for dev, Docker for production)
-   Docker Desktop (for production database)

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
git clone https://github.com/yourusername/SyncFlow.git
cd SyncFlow
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Setup Development Database (LocalDB)

```bash
cd Server
dotnet ef database update
```

### 4. Setup Production Database (Docker)

```bash
# Start Docker SQL Server
docker-compose up -d

# Apply migrations to Docker database
dotnet ef database update --connection "Server=localhost,1433;Database=DataCollectorDB;User Id=sa;Password=placeholder;TrustServerCertificate=True;"
```

### 5. Run the Application

#### Development Mode (LocalDB - Port 5000/5001)

```bash
cd Server
dotnet run --launch-profile Development
```

Access at: `https://localhost:5000`

#### Production Mode (Docker SQL - Port 6000/6001)

```bash
# Make sure Docker SQL is running
docker-compose up -d

cd Server
dotnet run --launch-profile Production-Local
```

Access at: `https://localhost:6000`

## 🧪 Testing Database Connection

Test endpoint available at:

```
Development: https://localhost:5000/api/test-db
Production:  https://localhost:6000/api/test-db
```

Expected response:

```json
{
	"connected": true,
	"message": "Database connection successful!"
}
```

## 🐳 Docker Commands

```bash
# Start SQL Server
docker-compose up -d

# Stop SQL Server
docker-compose down

# View logs
docker-compose logs -f sqlserver

# Restart SQL Server
docker-compose restart

# Remove all data (fresh start)
docker-compose down -v
```

## 📝 Development Progress

### ✅ Phase 1: Database & Models (Completed)

-   [x] Created ProductEntry data model
-   [x] Configured Entity Framework Core with SQL Server
-   [x] Set up dual database configuration (LocalDB + Docker)
-   [x] Created and applied database migrations
-   [x] Configured separate ports for Dev (5000/5001) and Prod (6000/6001)
-   [x] Added database connection health check endpoint

### 🔄 Phase 2: Server-Side API (In Progress)

-   [x] Create POST endpoint for receiving product entries
-   [x] Create GET endpoint for retrieving all entries
-   [x] Create GET endpoint to retieve specific entries using id
-   [x] Add DTOs and validation
-   [x] Implement error handling

### 📋 Phase 3: Client - Online Mode

-   [ ] Build data entry form component
-   [ ] Implement auto-capture datetime
-   [ ] Connect form to Server API
-   [ ] Add product entry loop functionality

### 🔌 Phase 4: Offline Capability

-   [ ] Implement LocalStorage for offline data
-   [ ] Add online/offline detection
-   [ ] Build sync mechanism
-   [ ] Add sync status indicators

### 📊 Phase 5: Server Dashboard

-   [ ] Create data viewing page
-   [ ] Display all collected entries
-   [ ] Add filtering/search functionality

### 🎨 Phase 6: Polish & Testing

-   [ ] Comprehensive testing
-   [ ] UI improvements
-   [ ] Mobile responsiveness
-   [ ] Documentation

## 🔧 Configuration

### Connection Strings

**Development (LocalDB):**

```json
"Server=(localdb)\\mssqllocaldb;Database=DataCollectorDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

**Production (Docker):**

```json
"Server=localhost,1433;Database=DataCollectorDB;User Id=sa;Password=placeholder;TrustServerCertificate=True;"
```

> ⚠️ **Note**: The production password is a placeholder for security reasons.
>
> To run locally with Docker:
>
> 1. Update `docker-compose.yml` with your own password
> 2. Update `appsettings.json` connection string to match
>
> In production environments, use:
>
> -   Azure Key Vault
> -   AWS Secrets Manager
> -   Environment variables
> -   .NET User Secrets (for local dev)

### Port Configuration

Configured in `Server/Properties/launchSettings.json`:

-   **Development**: HTTPS 5000, HTTP 5001
-   **Production-Local**: HTTPS 6000, HTTP 6001

## 🤝 Contributing

This is a portfolio/interview project. Feel free to fork and experiment!

## 📄 License

Licensed under GNU AGPL v3.0 - see [LICENSE](LICENSE) for details.

**What this means:**

-   ✅ Free to use for personal and educational purposes
-   ✅ Modifications must be open-sourced
-   ✅ Network use triggers license obligations
-   💼 Commercial licensing available - contact me for inquiries

## 👤 Author

**Abdulmalek Boulellou**

-   📧 malek.boulellou@proton.me
-   🐙 [GitHub](https://github.com/SiliconCatalyst)

## 🙏 Acknowledgments

Built as part of a technical interview assessment demonstrating:

-   Full-stack Blazor development
-   Offline-first architecture
-   Database design and migrations
-   Docker containerization
-   Clean code practices

---

**Status**: 🟢 Phase 1 Complete - Database layer fully configured and tested
