#!/bin/zsh

set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
API_DIR="$(cd "$SCRIPT_DIR/.." && pwd)"
COMPOSE_FILE="$API_DIR/docker-compose.local-db.yml"
CONTAINER_NAME="hrm-sqlserver"
DB_NAME="HRM2AI"
SA_PASSWORD="HrmLocalDb@2026"
SQLCMD="/opt/mssql-tools18/bin/sqlcmd"

docker compose -f "$COMPOSE_FILE" up -d

echo "Waiting for SQL Server to become ready..."
for attempt in {1..30}; do
  if docker exec "$CONTAINER_NAME" "$SQLCMD" -S localhost -U sa -P "$SA_PASSWORD" -C -Q "SELECT 1" >/dev/null 2>&1; then
    break
  fi

  if [ "$attempt" -eq 30 ]; then
    echo "SQL Server did not become ready in time." >&2
    exit 1
  fi

  sleep 2
done

docker exec "$CONTAINER_NAME" "$SQLCMD" \
  -S localhost \
  -U sa \
  -P "$SA_PASSWORD" \
  -C \
  -Q "IF DB_ID(N'$DB_NAME') IS NULL CREATE DATABASE [$DB_NAME];"

echo "Database $DB_NAME is ready at localhost:14333"
