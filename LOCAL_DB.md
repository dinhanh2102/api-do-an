# Local SQL Server

Project API is configured to use a local SQL Server container on:

- Host: `localhost`
- Port: `14333`
- Database: `HRM2AI`
- User: `sa`
- Password: `HrmLocalDb@2026`

Project API is also configured to use local Redis on:

- Host: `localhost`
- Port: `6379`

Start the database:

```bash
cd /Users/mac/Downloads/do-an-2024-main/source_code/api
chmod +x scripts/start-local-db.sh
./scripts/start-local-db.sh
```

Stop the database:

```bash
cd /Users/mac/Downloads/do-an-2024-main/source_code/api
docker compose -f docker-compose.local-db.yml down
```

Important:

- This creates the SQL Server container and the empty `HRM2AI` database.
- This compose file also starts a local Redis container required by the API startup.
- The repository currently has no EF Core `Migrations`, `.sql`, `.bak`, or `.bacpac` files, so tables are not created by this setup alone.
- The API still calls `Database.Migrate()` on startup. Without schema/migrations, the app may fail when it starts seeding data.
