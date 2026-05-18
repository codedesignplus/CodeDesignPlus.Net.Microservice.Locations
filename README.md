# 📍 Locations Microservice

[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?logo=.net)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](LICENSE.md)
[![Tests](https://img.shields.io/badge/tests-passing-success)](tests/)
[![Coverage](https://img.shields.io/badge/coverage-80%25-green)]()
[![Docker](https://img.shields.io/badge/docker-ready-2496ED?logo=docker)](Dockerfile)

A production-ready microservice for managing geographic locations (countries, states, cities, localities, neighborhoods) with timezone and currency information. Built with .NET 9, implementing Clean Architecture, DDD, and CQRS patterns with REST and gRPC entrypoints.

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Key Features](#-key-features)
- [Technology Stack](#️-technology-stack)
- [Prerequisites](#️-prerequisites)
- [Getting Started](#-getting-started)
- [API Endpoints](#-api-endpoints)
- [gRPC Services](#-grpc-services)
- [Configuration](#️-configuration)
- [Use Cases & Scenarios](#-use-cases--scenarios)
- [Architecture](#️-architecture)
- [Domain Model](#-domain-model)
- [Testing](#-testing)
- [Best Practices](#-best-practices)
- [Troubleshooting](#-troubleshooting)
- [Deployment](#-deployment)
- [FAQ](#-faq)
- [Contributing](#-contributing)
- [License](#-license)

---

## 🎯 Overview

## What is this microservice?

The Locations microservice provides the geographic reference data that the entire platform needs: countries, states/departments, cities, localities, and neighborhoods, each with its associated timezone and currency. It solves the problem of having a single source of truth for geographic information so that when a new organization is registered, it can select its country and city from a standardized catalog rather than free-text input. The Tenants microservice uses it to set an organization's location, the Accounting microservice uses it to determine the fiscal regime by country, and any form that requires an address (units, residents, contacts) pulls from this catalog.

---

The Locations microservice provides a centralized API for managing hierarchical geographic data with comprehensive location information. It serves as the source of truth for location-based services across your microservices ecosystem.

### What It Does

- **Geographic Hierarchy Management**: Countries → States → Cities → Localities → Neighborhoods
- **Currency Information**: Country-specific currency data with ISO codes and symbols
- **Timezone Support**: Timezone information with aliases, offsets, and geographic coordinates
- **Region Classification**: Regional and sub-regional categorization of countries
- **Multi-tenancy**: Isolate location data by tenant
- **Flexible Querying**: REST API with criteria-based filtering and gRPC for high-performance access
- **Audit Trail**: Track creation, updates, and deletions with user information
- **Soft Deletes**: Maintain referential integrity with logical deletion

### 🚀 Quick Start

```bash
# 1. Start infrastructure services
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d

# 2. Configure Vault secrets
cd ../../CodeDesignPlus.Net.Microservice.Locations/tools/vault
./config-vault.sh

# 3. Run the REST API
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.Rest

# 4. Access Swagger UI
open http://localhost:5000/ms-locations/swagger
```

### 📊 High-Level Architecture

```
┌──────────────────────────────────────────────────────┐
│              Client Applications                     │
│  (Web Apps, Mobile Apps, Other Microservices)       │
└───────────┬──────────────────────┬───────────────────┘
            │ REST (HTTP)          │ gRPC
            │ + JWT Auth           │ + JWT Auth
            │                      │
┌───────────▼──────────────────────▼───────────────────┐
│         Locations Microservice                       │
│  ┌──────────────┐           ┌──────────────┐        │
│  │ REST API     │           │ gRPC Service │        │
│  │ (Controllers)│           │  (Protos)    │        │
│  └──────┬───────┘           └──────┬───────┘        │
│         │                          │                │
│  ┌──────▼──────────────────────────▼──────┐         │
│  │          MediatR (CQRS)                │         │
│  │  ┌──────────────┐  ┌─────────────┐    │         │
│  │  │  Commands    │  │   Queries   │    │         │
│  │  │ (Create/     │  │  (GetAll/   │    │         │
│  │  │ Update/      │  │   GetById)  │    │         │
│  │  │ Delete)      │  │             │    │         │
│  │  └──────┬───────┘  └──────┬──────┘    │         │
│  └─────────┼──────────────────┼───────────┘         │
│            │                  │                     │
│  ┌─────────▼──────────────────▼──────────┐          │
│  │      Domain Layer (Aggregates)        │          │
│  │  • CountryAggregate                   │          │
│  │  • StateAggregate                     │          │
│  │  • CityAggregate                      │          │
│  │  • LocalityAggregate                  │          │
│  │  • NeighborhoodAggregate              │          │
│  │  • CurrencyAggregate                  │          │
│  │  • TimezoneAggregate                  │          │
│  │  • RegionAggregate                    │          │
│  └───────────┬───────────────────────────┘          │
│              │                                      │
│  ┌───────────▼────────────────────────────┐         │
│  │   Infrastructure Layer                 │         │
│  │  (Repositories + Event Publishing)     │         │
│  └───────────┬────────────────────────────┘         │
└──────────────┼──────────────────────────────────────┘
               │
      ┌────────┼────────┐
      │        │        │
┌─────▼────┐ ┌─▼──────┐ ┌──▼───────┐
│ MongoDB  │ │ Redis  │ │ RabbitMQ │
│(Location │ │(Cache) │ │ (Events) │
│  Data)   │ │        │ │          │
└──────────┘ └────────┘ └──────────┘
```

## 🚀 Key Features

### Core Capabilities

- ✅ **Hierarchical Location Data**: Country → State → City → Locality → Neighborhood
- ✅ **ISO Standards Support**: Alpha-2, Alpha-3, and numeric country codes
- ✅ **Currency Management**: Code, symbol, numeric code, and decimal digits per currency
- ✅ **Timezone Information**: Aliases, UTC offsets, and geographic coordinates
- ✅ **Region Classification**: Region and subregion data for countries
- ✅ **Multi-tenancy**: Tenant isolation for all location data
- ✅ **Flexible Querying**: Criteria-based filtering with pagination, sorting, and search
- ✅ **Dual Protocols**: REST API (HTTP) and gRPC for different performance needs
- ✅ **Soft Delete**: Logical deletion preserves referential integrity
- ✅ **Audit Trail**: CreatedBy, UpdatedBy, DeletedBy with timestamps

### Technical Features

- Clean Architecture with DDD and CQRS
- Domain events for all aggregate changes
- MongoDB for document persistence
- RabbitMQ for event publishing
- Redis for distributed caching
- OAuth2/OpenID Connect security
- Multi-tenancy support
- Swagger/OpenAPI documentation
- gRPC with reflection support
- Docker containerization
- Comprehensive test coverage (Unit, Integration)
- FluentValidation for input validation
- Mapster for object mapping
- NodaTime for timezone-aware dates

## 🛠️ Technology Stack

### Core
- **.NET 9** - Runtime and framework
- **ASP.NET Core** - Web API framework
- **C# 13** - Programming language

### Storage & Data
- **MongoDB** - Document database for location data
- **Redis** - Distributed caching and session storage

### Messaging & Events
- **RabbitMQ** - Event publishing and message broker

### Architecture & Patterns
- **MediatR** - CQRS command/query handling
- **FluentValidation** - Input validation
- **Mapster** - Object mapping
- **NodaTime** - Timezone-aware date/time handling

### Security & Configuration
- **Vault** - Secret management
- **OAuth2/OpenID Connect** - Authentication
- **JWT Bearer** - Token-based security
- **HTTPS** - Encrypted communication

### Protocols
- **REST API** - HTTP/HTTPS with JSON
- **gRPC** - High-performance RPC with Protocol Buffers

### DevOps & Testing
- **Docker** - Containerization
- **xUnit** - Unit/integration testing
- **Swagger/OpenAPI** - API documentation
- **Helm** - Kubernetes deployment

## ⚙️ Prerequisites

### Required
- **.NET 9 SDK** - [Download](https://dotnet.microsoft.com/download/dotnet/9.0)
- **Docker & Docker Compose** - For infrastructure services
- **MongoDB 6.0+** - Document database
- **Redis 7.0+** - Caching layer
- **RabbitMQ 3.12+** - Message broker

### Optional
- **Vault** - Secret management (can use appsettings for local dev)
- **Kubernetes** - For production deployment (Helm charts included)

## 🚀 Getting Started

The following instructions will help you set up the project on your local machine for development and testing purposes.

### 1. Clone the Repository

```bash
git clone <repository-url>
cd CodeDesignPlus.Net.Microservice.Locations
```

### 2. Start Infrastructure Services

Clone and run the development environment:

```bash
git clone https://github.com/codedesignplus/CodeDesignPlus.Environment.Dev
cd CodeDesignPlus.Environment.Dev/resources
docker-compose up -d
```

This starts:
- MongoDB (port 27017)
- Redis (port 6379)
- RabbitMQ (port 5672, management UI at 15672)
- Vault (port 8200)

### 3. Configure Vault

Run the Vault configuration script to set up secrets:

```bash
cd ../../CodeDesignPlus.Net.Microservice.Locations/tools/vault
./config-vault.sh
```

This configures:
- MongoDB credentials
- RabbitMQ credentials
- Security tokens

### 4. Build the Solution

```bash
dotnet build
```

### 5. Run the Microservice

Choose your desired entrypoint:

#### REST API (Port 5000)
```bash
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.Rest
```

Access Swagger UI at: `http://localhost:5000/ms-locations/swagger`

#### gRPC (Port 5001)
```bash
dotnet run --project src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.gRpc
```

Use gRPC reflection or the proto files in `src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.gRpc/Protos/`

### 6. Verify Health

```bash
# REST API
curl http://localhost:5000/ms-locations/health

# gRPC
grpcurl -plaintext localhost:5001 grpc.health.v1.Health/Check
```

## 📡 API Endpoints

### Country Operations

#### Get All Countries
```http
GET /api/country?page=1&pageSize=20&orderBy=name&filters=region:Americas
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

**Response:**
```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "name": "United States",
      "alpha2": "US",
      "alpha3": "USA",
      "code": "840",
      "capital": "Washington, D.C.",
      "idCurrency": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
      "timezone": "America/New_York",
      "nameNative": "United States",
      "region": "Americas",
      "subRegion": "Northern America",
      "latitude": 38.8951,
      "longitude": -77.0364,
      "flag": "🇺🇸",
      "isActive": true,
      "createdAt": "2024-01-15T10:30:00Z"
    }
  ],
  "total": 195,
  "page": 1,
  "pageSize": 20
}
```

#### Get Country by ID
```http
GET /api/country/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create Country
```http
POST /api/country
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "United States",
  "alpha2": "US",
  "alpha3": "USA",
  "code": "840",
  "capital": "Washington, D.C.",
  "idCurrency": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
  "timezone": "America/New_York",
  "nameNative": "United States",
  "region": "Americas",
  "subRegion": "Northern America",
  "latitude": 38.8951,
  "longitude": -77.0364,
  "flag": "🇺🇸",
  "isActive": true
}
```

#### Update Country
```http
PUT /api/country/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "United States of America",
  "alpha2": "US",
  "alpha3": "USA",
  "code": "840",
  "capital": "Washington, D.C.",
  "idCurrency": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
  "timezone": "America/New_York",
  "nameNative": "United States",
  "region": "Americas",
  "subRegion": "Northern America",
  "latitude": 38.8951,
  "longitude": -77.0364,
  "flag": "🇺🇸",
  "isActive": true
}
```

#### Delete Country
```http
DELETE /api/country/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

### State Operations

#### Get All States
```http
GET /api/state?filters=idCountry:{country-id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Get State by ID
```http
GET /api/state/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create State
```http
POST /api/state
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "idCountry": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "code": "CA",
  "name": "California"
}
```

#### Update State
```http
PUT /api/state/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idCountry": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "code": "CA",
  "name": "California",
  "isActive": true
}
```

#### Delete State
```http
DELETE /api/state/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

### City Operations

#### Get All Cities
```http
GET /api/city?filters=idState:{state-id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Get City by ID
```http
GET /api/city/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create City
```http
POST /api/city
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
  "idState": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "name": "Los Angeles",
  "timezone": "America/Los_Angeles"
}
```

#### Update City
```http
PUT /api/city/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idState": "4fa85f64-5717-4562-b3fc-2c963f66afa7",
  "name": "Los Angeles",
  "timezone": "America/Los_Angeles",
  "isActive": true
}
```

#### Delete City
```http
DELETE /api/city/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

### Locality Operations

#### Get All Localities
```http
GET /api/locality?filters=idCity:{city-id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Get Locality by ID
```http
GET /api/locality/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create Locality
```http
POST /api/locality
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "6fa85f64-5717-4562-b3fc-2c963f66afa9",
  "idCity": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
  "name": "Downtown"
}
```

#### Update Locality
```http
PUT /api/locality/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idCity": "5fa85f64-5717-4562-b3fc-2c963f66afa8",
  "name": "Downtown District",
  "isActive": true
}
```

#### Delete Locality
```http
DELETE /api/locality/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

### Neighborhood Operations

#### Get All Neighborhoods
```http
GET /api/neighborhood?filters=idLocality:{locality-id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Get Neighborhood by ID
```http
GET /api/neighborhood/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create Neighborhood
```http
POST /api/neighborhood
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "7fa85f64-5717-4562-b3fc-2c963f66afaa",
  "idLocality": "6fa85f64-5717-4562-b3fc-2c963f66afa9",
  "name": "Historic Core"
}
```

#### Update Neighborhood
```http
PUT /api/neighborhood/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "idLocality": "6fa85f64-5717-4562-b3fc-2c963f66afa9",
  "name": "Historic Core District",
  "isActive": true
}
```

#### Delete Neighborhood
```http
DELETE /api/neighborhood/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

### Currency Operations

#### Get All Currencies
```http
GET /api/currency
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Get Currency by ID
```http
GET /api/currency/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create Currency
```http
POST /api/currency
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "8fa85f64-5717-4562-b3fc-2c963f66afa8",
  "code": "USD",
  "numericCode": 840,
  "decimalDigits": 2,
  "symbol": "$",
  "name": "US Dollar"
}
```

#### Update Currency
```http
PUT /api/currency/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "code": "USD",
  "numericCode": 840,
  "decimalDigits": 2,
  "symbol": "$",
  "name": "United States Dollar",
  "isActive": true
}
```

#### Delete Currency
```http
DELETE /api/currency/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

### Timezone Operations

#### Get All Timezones
```http
GET /api/timezone
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Get Timezone by ID
```http
GET /api/timezone/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create Timezone
```http
POST /api/timezone
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "9fa85f64-5717-4562-b3fc-2c963f66afab",
  "name": "America/New_York",
  "aliases": ["US/Eastern", "EST"],
  "location": {
    "latitude": 40.7128,
    "longitude": -74.0060
  },
  "offsets": ["-05:00", "-04:00"],
  "currentOffset": "-04:00"
}
```

#### Update Timezone
```http
PUT /api/timezone/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "America/New_York",
  "aliases": ["US/Eastern", "EST", "Eastern Time"],
  "location": {
    "latitude": 40.7128,
    "longitude": -74.0060
  },
  "offsets": ["-05:00", "-04:00"],
  "currentOffset": "-04:00",
  "isActive": true
}
```

#### Delete Timezone
```http
DELETE /api/timezone/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

### Region Operations

#### Get All Regions
```http
GET /api/region
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Get Region by ID
```http
GET /api/region/{id}
Authorization: Bearer {token}
X-Tenant: {tenant-id}
```

#### Create Region
```http
POST /api/region
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "id": "afa85f64-5717-4562-b3fc-2c963f66afac",
  "name": "Americas",
  "subRegions": [
    "Northern America",
    "Central America",
    "South America",
    "Caribbean"
  ]
}
```

#### Update Region
```http
PUT /api/region/{id}
Content-Type: application/json
Authorization: Bearer {token}
X-Tenant: {tenant-id}

{
  "name": "Americas",
  "subRegions": [
    "Northern America",
    "Central America",
    "South America",
    "Caribbean",
    "Latin America"
  ],
  "isActive": true
}
```

### Criteria-Based Filtering

All `GET` endpoints support advanced filtering via query parameters:

```http
GET /api/country?page=1&pageSize=20&orderBy=name&filters=region:Americas,isActive:true
```

**Query Parameters:**
- `page` - Page number (default: 1)
- `pageSize` - Items per page (default: 20, max: 100)
- `orderBy` - Sort field (e.g., `name`, `-createdAt` for descending)
- `filters` - Comma-separated filters (e.g., `field:value,field2:value2`)

**Filter Operators:**
- `:` - Equals
- `>:` - Greater than
- `<:` - Less than
- `>=:` - Greater than or equal
- `<=:` - Less than or equal
- `*:` - Contains (case-insensitive)

**Examples:**
```http
# Countries in Americas region
GET /api/country?filters=region:Americas

# Cities in a specific state, ordered by name
GET /api/city?filters=idState:4fa85f64-5717-4562-b3fc-2c963f66afa7&orderBy=name

# Active currencies
GET /api/currency?filters=isActive:true

# Countries created after a date
GET /api/country?filters=createdAt>:2024-01-01T00:00:00Z
```

## 🔌 gRPC Services

The microservice exposes gRPC services for high-performance queries.

### Country Service

**Proto Definition:** `src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.gRpc/Protos/country.proto`

```protobuf
service CountryService {
  rpc GetCountry (GetCountryRequest) returns (GetCountryResponse);
}

message GetCountryRequest {
  google.protobuf.StringValue id = 1;
  google.protobuf.StringValue name = 2;
  google.protobuf.StringValue alpha2 = 3;
  google.protobuf.StringValue alpha3 = 4;
  google.protobuf.StringValue code = 5;
}

message GetCountryResponse {
  string id = 1;
  string name = 2;
  string alpha2 = 3;
  string alpha3 = 4;
  string code = 5;
  optional string capital = 6;
  Currency currency = 7;
  string timezone = 8;
  string name_native = 9;
  string region = 10;
  string subregion = 11;
  double latitude = 12;
  double longitude = 13;
  optional string flag = 14;
}
```

**Usage Example (C#):**
```csharp
var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new CountryService.CountryServiceClient(channel);

var request = new GetCountryRequest 
{ 
    Alpha2 = "US" 
};

var response = await client.GetCountryAsync(request);

Console.WriteLine($"Country: {response.Name} ({response.Alpha2})");
Console.WriteLine($"Currency: {response.Currency.Code} ({response.Currency.Symbol})");
```

**Usage Example (grpcurl):**
```bash
# Get country by ID
grpcurl -plaintext \
  -d '{"id": "3fa85f64-5717-4562-b3fc-2c963f66afa6"}' \
  localhost:5001 \
  country.CountryService/GetCountry

# Get country by alpha2 code
grpcurl -plaintext \
  -d '{"alpha2": "US"}' \
  localhost:5001 \
  country.CountryService/GetCountry

# List available services (reflection enabled in Development)
grpcurl -plaintext localhost:5001 list
```

### Currency Service

**Proto Definition:** `src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.gRpc/Protos/currency.proto`

```protobuf
service CurrencyService {
  rpc GetCurrency (GetCurrencyRequest) returns (GetCurrencyResponse);
}

message GetCurrencyRequest {
  google.protobuf.StringValue id = 1;
  google.protobuf.StringValue code = 2;
}

message GetCurrencyResponse {
  string id = 1;
  string code = 2;
  int32 numeric_code = 3;
  int32 decimal_digits = 4;
  string symbol = 5;
  string name = 6;
}
```

**Usage Example (C#):**
```csharp
var channel = GrpcChannel.ForAddress("https://localhost:5001");
var client = new CurrencyService.CurrencyServiceClient(channel);

var request = new GetCurrencyRequest 
{ 
    Code = "USD" 
};

var response = await client.GetCurrencyAsync(request);

Console.WriteLine($"Currency: {response.Name}");
Console.WriteLine($"Symbol: {response.Symbol}, Decimals: {response.DecimalDigits}");
```

### gRPC Authentication

gRPC endpoints require JWT authentication:

```csharp
var credentials = CallCredentials.FromInterceptor((context, metadata) =>
{
    metadata.Add("Authorization", $"Bearer {token}");
    metadata.Add("X-Tenant", tenantId);
    return Task.CompletedTask;
});

var channel = GrpcChannel.ForAddress("https://localhost:5001", new GrpcChannelOptions
{
    Credentials = ChannelCredentials.Create(
        new SslCredentials(),
        credentials
    )
});
```

## ⚙️ Configuration

### Core Configuration (appsettings.json)

```json
{
  "Core": {
    "Id": "069b9617-dddd-4d0a-b3d0-c04723e6e78b",
    "PathBase": "/ms-locations",
    "AppName": "ms-locations",
    "TypeEntryPoint": "rest",
    "Version": "v1",
    "Description": "Microservice to manage the locations",
    "Business": "CodeDesignPlus",
    "Contact": {
      "Name": "CodeDesignPlus",
      "Email": "support@codedesignplus.com"
    }
  }
}
```

### MongoDB Configuration

```json
{
  "Mongo": {
    "Enable": true,
    "Database": "db-ms-locations",
    "Diagnostic": {
      "Enable": false,
      "EnableCommandText": false
    }
  }
}
```

**Collections:**
- `countries` - Country aggregates
- `states` - State aggregates
- `cities` - City aggregates
- `localities` - Locality aggregates
- `neighborhoods` - Neighborhood aggregates
- `currencies` - Currency aggregates
- `timezones` - Timezone aggregates
- `regions` - Region aggregates

### Redis Cache Configuration

```json
{
  "Redis": {
    "Instances": {
      "Core": {
        "ConnectionString": "localhost:6379"
      }
    }
  },
  "RedisCache": {
    "Enable": true,
    "Expiration": "00:05:00"
  }
}
```

**Cache Keys:**
- `location:country:{id}` - Country by ID
- `location:country:alpha2:{code}` - Country by Alpha-2 code
- `location:state:{id}` - State by ID
- `location:city:{id}` - City by ID
- `location:currency:{code}` - Currency by code

### RabbitMQ Configuration

```json
{
  "RabbitMQ": {
    "Enable": true,
    "Host": "localhost",
    "Port": 5672,
    "UserName": "user",
    "Password": "pass",
    "EnableDiagnostic": false
  }
}
```

**Published Events:**
- `CountryCreatedDomainEvent`
- `CountryUpdatedDomainEvent`
- `CountryDeletedDomainEvent`
- `StateCreatedDomainEvent`
- `StateUpdatedDomainEvent`
- `StateDeletedDomainEvent`
- `CityCreatedDomainEvent`
- `CityUpdatedDomainEvent`
- `CityDeletedDomainEvent`
- `LocalityCreatedDomainEvent`
- `LocalityUpdatedDomainEvent`
- `LocalityDeletedDomainEvent`
- `NeighborhoodCreatedDomainEvent`
- `NeighborhoodUpdatedDomainEvent`
- `NeighborhoodDeletedDomainEvent`
- `CurrencyCreatedDomainEvent`
- `CurrencyUpdatedDomainEvent`
- `CurrencyDeletedDomainEvent`
- `TimezoneCreatedDomainEvent`
- `TimezoneUpdatedDomainEvent`
- `TimezoneDeletedDomainEvent`

### Security Configuration

```json
{
  "Security": {
    "IncludeErrorDetails": true,
    "ValidateAudience": true,
    "ValidateIssuer": true,
    "ValidateLifetime": true,
    "RequireHttpsMetadata": true,
    "ValidIssuer": "",
    "ValidAudiences": [],
    "Applications": [],
    "ValidateLicense": false,
    "ValidateRbac": false
  }
}
```

### Vault Configuration

```json
{
  "Vault": {
    "Enable": true,
    "Address": "http://localhost:8200",
    "AppName": "ms-locations",
    "Solution": "security-codedesignplus",
    "Token": "root",
    "Mongo": {
      "Enable": true,
      "TemplateConnectionString": "mongodb://{0}:{1}@localhost:27017"
    },
    "RabbitMQ": {
      "Enable": true
    }
  }
}
```

**Vault Paths:**
- `security-codedesignplus/ms-locations/mongo` - MongoDB credentials
- `security-codedesignplus/ms-locations/rabbitmq` - RabbitMQ credentials

### Observability Configuration

```json
{
  "Logger": {
    "Enable": true,
    "OTelEndpoint": "http://localhost:4317",
    "Level": "Warning"
  },
  "Observability": {
    "Enable": true,
    "ServerOtel": "http://localhost:4317",
    "Trace": {
      "Enable": true,
      "AspNetCore": true,
      "GrpcClient": true,
      "CodeDesignPlusSdk": true,
      "Redis": true,
      "RabbitMQ": true
    },
    "Metrics": {
      "Enable": true,
      "AspNetCore": true
    }
  }
}
```

### Environment-Specific Configuration

The microservice supports multiple environments:

- **Development** (`appsettings.Development.json`) - Local development with Vault
- **Docker** (`appsettings.Docker.json`) - Docker Compose environment
- **Staging** (`appsettings.Staging.json`) - Staging environment with Kubernetes
- **Production** (`appsettings.json` + Vault) - Production with full observability

## 💼 Use Cases & Scenarios

### 1. Address Form Dropdowns

**Scenario:** Populate country/state/city dropdowns in a user registration form.

**Solution:**
```csharp
// 1. Get all countries
var countries = await httpClient.GetAsync("/api/country?orderBy=name");

// 2. User selects country -> load states
var states = await httpClient.GetAsync($"/api/state?filters=idCountry:{countryId}&orderBy=name");

// 3. User selects state -> load cities
var cities = await httpClient.GetAsync($"/api/city?filters=idState:{stateId}&orderBy=name");

// 4. User selects city -> load localities
var localities = await httpClient.GetAsync($"/api/locality?filters=idCity:{cityId}&orderBy=name");

// 5. User selects locality -> load neighborhoods
var neighborhoods = await httpClient.GetAsync($"/api/neighborhood?filters=idLocality:{localityId}&orderBy=name");
```

### 2. Currency Conversion Service

**Scenario:** E-commerce platform needs to display prices in user's local currency.

**Solution:**
```csharp
// Get country by user's location
var country = await grpcClient.GetCountryAsync(new GetCountryRequest { Alpha2 = "CO" });

// Get currency details
var currency = await httpClient.GetAsync($"/api/currency/{country.Currency.Id}");

// Format price with currency symbol and decimal places
var formattedPrice = price.ToString($"C{currency.DecimalDigits}", 
    new CultureInfo($"es-{country.Alpha2}"));
```

### 3. Timezone-Aware Scheduling

**Scenario:** Schedule notifications based on user's local time.

**Solution:**
```csharp
// Get city with timezone
var city = await httpClient.GetAsync($"/api/city/{cityId}");

// Get timezone details
var timezone = await httpClient.GetAsync($"/api/timezone?filters=name:{city.Timezone}");

// Convert UTC to local time
var localTime = SystemClock.Instance
    .GetCurrentInstant()
    .InZone(DateTimeZoneProviders.Tzdb[timezone.Name]);

// Schedule notification for 9 AM local time
var scheduledTime = localTime.Date
    .At(new LocalTime(9, 0))
    .InZoneLeniently(localTime.Zone);
```

### 4. Shipping Cost Calculation

**Scenario:** Calculate shipping costs based on hierarchical location data.

**Solution:**
```csharp
// Get full location hierarchy
var neighborhood = await httpClient.GetAsync($"/api/neighborhood/{neighborhoodId}");
var locality = await httpClient.GetAsync($"/api/locality/{neighborhood.IdLocality}");
var city = await httpClient.GetAsync($"/api/city/{locality.IdCity}");
var state = await httpClient.GetAsync($"/api/state/{city.IdState}");
var country = await httpClient.GetAsync($"/api/country/{state.IdCountry}");

// Calculate shipping based on location level
var shippingCost = shippingService.Calculate(
    country: country.Name,
    state: state.Name,
    city: city.Name,
    locality: locality.Name,
    neighborhood: neighborhood.Name
);
```

### 5. Regional Content Delivery

**Scenario:** Serve region-specific content (language, promotions, regulations).

**Solution:**
```csharp
// Get country with region information
var country = await grpcClient.GetCountryAsync(new GetCountryRequest { Alpha2 = userCountryCode });

// Get regional configuration
var region = await httpClient.GetAsync($"/api/region?filters=name:{country.Region}");

// Serve region-specific content
var content = contentService.GetContentForRegion(
    region: country.Region,
    subRegion: country.SubRegion,
    language: GetLanguageForCountry(country.Alpha2)
);
```

### 6. Geolocation Validation

**Scenario:** Validate user-provided address against stored location data.

**Solution:**
```csharp
// Validate hierarchical consistency
var neighborhood = await httpClient.GetAsync($"/api/neighborhood/{address.NeighborhoodId}");
var locality = await httpClient.GetAsync($"/api/locality/{neighborhood.IdLocality}");

if (locality.IdCity != address.CityId)
{
    return ValidationError("Neighborhood does not belong to the specified city");
}

// Validate coordinates are within country bounds
var country = await httpClient.GetAsync($"/api/country/{address.CountryId}");
if (!IsWithinBounds(address.Latitude, address.Longitude, country))
{
    return ValidationError("Coordinates are outside country bounds");
}
```

### 7. Bulk Location Import

**Scenario:** Import locations from external source (CSV, API).

**Solution:**
```csharp
// Import countries with currencies
foreach (var countryData in externalData.Countries)
{
    // Check if currency exists, create if not
    var currency = await GetOrCreateCurrency(countryData.CurrencyCode);
    
    // Create country
    await httpClient.PostAsync("/api/country", new
    {
        id = Guid.NewGuid(),
        name = countryData.Name,
        alpha2 = countryData.Alpha2,
        alpha3 = countryData.Alpha3,
        code = countryData.NumericCode,
        capital = countryData.Capital,
        idCurrency = currency.Id,
        timezone = countryData.Timezone,
        nameNative = countryData.NativeName,
        region = countryData.Region,
        subRegion = countryData.SubRegion,
        latitude = countryData.Latitude,
        longitude = countryData.Longitude,
        flag = countryData.Flag,
        isActive = true
    });
}
```

### 8. Multi-Tenant Location Management

**Scenario:** Different tenants maintain separate location catalogs.

**Solution:**
```csharp
// Tenant A creates custom location
var tenantARequest = new HttpRequestMessage(HttpMethod.Post, "/api/city")
{
    Headers = 
    {
        { "Authorization", $"Bearer {tenantAToken}" },
        { "X-Tenant", tenantAId }
    },
    Content = JsonContent.Create(cityData)
};

// Tenant B creates their own location
var tenantBRequest = new HttpRequestMessage(HttpMethod.Post, "/api/city")
{
    Headers = 
    {
        { "Authorization", $"Bearer {tenantBToken}" },
        { "X-Tenant", tenantBId }
    },
    Content = JsonContent.Create(cityData)
};

// Data is isolated by tenant - tenantA cannot see tenantB's cities
```

## 🏗️ Architecture

### Clean Architecture Layers

```
┌──────────────────────────────────────────────────────┐
│                  Entrypoints Layer                   │
│  • REST API Controllers (HTTP)                       │
│  • gRPC Services (Protos)                            │
│  • Program.cs (Startup Configuration)                │
└────────────────────┬─────────────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────────────┐
│                 Application Layer                    │
│  • Commands (Create/Update/Delete)                   │
│  • Queries (GetAll/GetById)                          │
│  • Command/Query Handlers (MediatR)                  │
│  • DTOs (Data Transfer Objects)                      │
│  • Validators (FluentValidation)                     │
│  • Mappers (Mapster)                                 │
└────────────────────┬─────────────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────────────┐
│                   Domain Layer                       │
│  • Aggregates (CountryAggregate, StateAggregate...)  │
│  • Domain Events (Created/Updated/Deleted)           │
│  • Value Objects (Location, etc.)                    │
│  • Domain Guards (Business Rules)                    │
│  • Errors (Domain-Specific Errors)                   │
└────────────────────┬─────────────────────────────────┘
                     │
                     ▼
┌──────────────────────────────────────────────────────┐
│                Infrastructure Layer                  │
│  • Repositories (MongoDB Implementation)             │
│  • Event Publishers (RabbitMQ)                       │
│  • Cache (Redis)                                     │
│  • External Service Integrations                     │
└──────────────────────────────────────────────────────┘
```

### Project Structure

```
CodeDesignPlus.Net.Microservice.Locations/
├── src/
│   ├── domain/
│   │   ├── Domain/                    # Aggregates, Domain Events, Value Objects
│   │   │   ├── CountryAggregate.cs
│   │   │   ├── StateAggregate.cs
│   │   │   ├── CityAggregate.cs
│   │   │   ├── LocalityAggregate.cs
│   │   │   ├── NeighborhoodAggregate.cs
│   │   │   ├── CurrencyAggregate.cs
│   │   │   ├── TimezoneAggregate.cs
│   │   │   ├── RegionAggregate.cs
│   │   │   ├── DomainEvents/
│   │   │   │   ├── CountryCreatedDomainEvent.cs
│   │   │   │   ├── CountryUpdatedDomainEvent.cs
│   │   │   │   └── ...
│   │   │   └── ValueObjects/
│   │   │       └── Location.cs
│   │   │
│   │   ├── Application/               # Commands, Queries, Handlers, DTOs
│   │   │   ├── Country/
│   │   │   │   ├── Commands/
│   │   │   │   │   ├── CreateCountry/
│   │   │   │   │   │   ├── CreateCountryCommand.cs
│   │   │   │   │   │   └── CreateCountryCommandHandler.cs
│   │   │   │   │   ├── UpdateCountry/
│   │   │   │   │   └── DeleteCountry/
│   │   │   │   ├── Queries/
│   │   │   │   │   ├── GetAllCountry/
│   │   │   │   │   │   ├── GetAllCountryQuery.cs
│   │   │   │   │   │   └── GetAllCountryQueryHandler.cs
│   │   │   │   │   └── GetCountryById/
│   │   │   │   └── DataTransferObjects/
│   │   │   │       └── CountryDto.cs
│   │   │   ├── State/
│   │   │   ├── City/
│   │   │   ├── Locality/
│   │   │   ├── Neighborhood/
│   │   │   ├── Currency/
│   │   │   ├── Timezone/
│   │   │   └── Region/
│   │   │
│   │   └── Infrastructure/            # Repositories, Persistence
│   │       ├── Repositories/
│   │       │   ├── CountryRepository.cs
│   │       │   ├── StateRepository.cs
│   │       │   ├── CityRepository.cs
│   │       │   ├── LocalityRepository.cs
│   │       │   ├── NeighborhoodRepository.cs
│   │       │   ├── CurrencyRepository.cs
│   │       │   ├── TimezoneRepository.cs
│   │       │   └── RegionRepository.cs
│   │       └── Startup.cs
│   │
│   └── entrypoints/
│       ├── Rest/                      # REST API
│       │   ├── Controllers/
│       │   │   ├── CountryController.cs
│       │   │   ├── StateController.cs
│       │   │   ├── CityController.cs
│       │   │   ├── LocalityController.cs
│       │   │   ├── NeighborhoodController.cs
│       │   │   ├── CurrencyController.cs
│       │   │   ├── TimezoneController.cs
│       │   │   └── RegionController.cs
│       │   ├── Program.cs
│       │   ├── appsettings.json
│       │   └── Dockerfile
│       │
│       └── gRpc/                      # gRPC Service
│           ├── Services/
│           │   ├── CountryService.cs
│           │   └── CurrencyService.cs
│           ├── Protos/
│           │   ├── country.proto
│           │   └── currency.proto
│           ├── Program.cs
│           ├── appsettings.json
│           └── Dockerfile
│
├── tests/
│   ├── unit/
│   │   ├── Domain.Test/               # Domain unit tests
│   │   ├── Application.Test/          # Application unit tests
│   │   ├── Infrastructure.Test/       # Infrastructure unit tests
│   │   ├── Rest.Test/                 # REST API unit tests
│   │   └── gRpc.Test/                 # gRPC unit tests
│   │
│   └── integration/
│       ├── Rest.Test/                 # REST API integration tests
│       └── gRpc.Test/                 # gRPC integration tests
│
├── charts/                            # Helm charts for Kubernetes
│   ├── ms-locations-rest/
│   │   ├── Chart.yaml
│   │   └── values.yaml
│   └── ms-locations-grpc/
│       ├── Chart.yaml
│       └── values.yaml
│
├── tools/                             # Utility scripts
│   ├── vault/
│   │   └── config-vault.sh
│   ├── update-packages/
│   ├── upgrade-dotnet/
│   └── sonarqube/
│
├── .github/
│   └── workflows/
│       ├── ci.yml                     # Continuous Integration
│       └── cd.yml                     # Continuous Deployment
│
├── README.md
├── LICENSE.md
├── archetype.json
└── CodeDesignPlus.Net.Microservice.Locations.sln
```

## 📦 Domain Model

### Aggregates

#### CountryAggregate
```csharp
public class CountryAggregate : AggregateRootBase
{
    public string Name { get; private set; }           // e.g., "United States"
    public string Alpha2 { get; private set; }         // e.g., "US"
    public string Alpha3 { get; private set; }         // e.g., "USA"
    public string Code { get; private set; }           // e.g., "840" (ISO 3166-1 numeric)
    public string? Capital { get; private set; }       // e.g., "Washington, D.C."
    public Guid IdCurrency { get; private set; }       // Reference to CurrencyAggregate
    public string Timezone { get; private set; }       // e.g., "America/New_York"
    public string NameNative { get; private set; }     // e.g., "United States"
    public string Region { get; private set; }         // e.g., "Americas"
    public string SubRegion { get; private set; }      // e.g., "Northern America"
    public double Latitude { get; private set; }       // e.g., 38.8951
    public double Longitude { get; private set; }      // e.g., -77.0364
    public string? Flag { get; private set; }          // e.g., "🇺🇸"
    
    // Inherited from AggregateRootBase
    public Guid Id { get; }
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }
    public Instant CreatedAt { get; private set; }
    public Guid CreatedBy { get; private set; }
    public Instant? UpdatedAt { get; private set; }
    public Guid? UpdatedBy { get; private set; }
    public Instant? DeletedAt { get; private set; }
    public Guid? DeletedBy { get; private set; }
}
```

**Business Rules:**
- Alpha-2 must be 2 characters (ISO 3166-1 alpha-2)
- Alpha-3 must be 3 characters (ISO 3166-1 alpha-3)
- Numeric code must be valid (ISO 3166-1 numeric)
- Latitude must be between -90 and 90
- Longitude must be between -180 and 180
- Currency must exist in the system
- Cannot delete country if states exist

#### StateAggregate
```csharp
public class StateAggregate : AggregateRootBase
{
    public Guid IdCountry { get; private set; }        // Reference to CountryAggregate
    public string Code { get; private set; }           // e.g., "CA"
    public string Name { get; private set; }           // e.g., "California"
}
```

**Business Rules:**
- Country must exist
- Code is typically 2 characters (ISO 3166-2)
- Cannot delete state if cities exist

#### CityAggregate
```csharp
public class CityAggregate : AggregateRootBase
{
    public Guid IdState { get; private set; }          // Reference to StateAggregate
    public string Name { get; private set; }           // e.g., "Los Angeles"
    public string Timezone { get; private set; }       // e.g., "America/Los_Angeles"
}
```

**Business Rules:**
- State must exist
- Timezone must be a valid IANA timezone identifier
- Cannot delete city if localities exist

#### LocalityAggregate
```csharp
public class LocalityAggregate : AggregateRootBase
{
    public Guid IdCity { get; private set; }           // Reference to CityAggregate
    public string Name { get; private set; }           // e.g., "Downtown"
}
```

**Business Rules:**
- City must exist
- Cannot delete locality if neighborhoods exist

#### NeighborhoodAggregate
```csharp
public class NeighborhoodAggregate : AggregateRootBase
{
    public Guid IdLocality { get; private set; }       // Reference to LocalityAggregate
    public string Name { get; private set; }           // e.g., "Historic Core"
}
```

**Business Rules:**
- Locality must exist

#### CurrencyAggregate
```csharp
public class CurrencyAggregate : AggregateRootBase
{
    public string Code { get; private set; }           // e.g., "USD" (ISO 4217)
    public short NumericCode { get; private set; }     // e.g., 840
    public short DecimalDigits { get; private set; }   // e.g., 2
    public string Symbol { get; private set; }         // e.g., "$"
    public string Name { get; private set; }           // e.g., "US Dollar"
}
```

**Business Rules:**
- Code must be 3 characters (ISO 4217)
- Numeric code must be valid (ISO 4217 numeric)
- Decimal digits typically 0-4
- Cannot delete currency if countries reference it

#### TimezoneAggregate
```csharp
public class TimezoneAggregate : AggregateRootBase
{
    public string Name { get; private set; }           // e.g., "America/New_York"
    public List<string> Aliases { get; set; }          // e.g., ["US/Eastern", "EST"]
    public Location Location { get; set; }             // Geographic coordinates
    public List<string> Offsets { get; set; }          // e.g., ["-05:00", "-04:00"]
    public string CurrentOffset { get; set; }          // e.g., "-04:00"
}
```

**Value Objects:**
```csharp
public class Location
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}
```

**Business Rules:**
- Name must be a valid IANA timezone identifier
- Latitude must be between -90 and 90
- Longitude must be between -180 and 180
- Offsets must be valid UTC offsets (e.g., "-05:00", "+03:30")

#### RegionAggregate
```csharp
public class RegionAggregate : AggregateRootBase
{
    public string Name { get; private set; }           // e.g., "Americas"
    public List<string> SubRegions { get; private set; } // e.g., ["Northern America", "South America"]
}
```

**Business Rules:**
- Must have at least one subregion
- Subregion names must be unique within the region

### Domain Events

All aggregates emit domain events for state changes:

```csharp
// Country Events
public record CountryCreatedDomainEvent(Guid Id, string Name, string Alpha2, ...);
public record CountryUpdatedDomainEvent(Guid Id, string Name, string Alpha2, ...);
public record CountryDeletedDomainEvent(Guid Id, string Name, string Alpha2, ...);

// State Events
public record StateCreatedDomainEvent(Guid Id, Guid IdCountry, string Code, string Name, bool IsActive);
public record StateUpdatedDomainEvent(Guid Id, Guid IdCountry, string Code, string Name, bool IsActive);
public record StateDeletedDomainEvent(Guid Id, Guid IdCountry, string Code, string Name, bool IsActive);

// City Events
public record CityCreatedDomainEvent(Guid Id, Guid IdState, string Name, string Timezone, bool IsActive);
public record CityUpdatedDomainEvent(Guid Id, Guid IdState, string Name, string Timezone, bool IsActive);
public record CityDeletedDomainEvent(Guid Id, Guid IdState, string Name, string Timezone, bool IsActive);

// And similar events for Locality, Neighborhood, Currency, Timezone
```

**Event Usage:**
- Published to RabbitMQ for event-driven integrations
- Can trigger side effects (notifications, audit logs, read model updates)
- Enable eventual consistency across microservices

### CQRS Command/Query Pattern

#### Commands (Write Operations)
```
• CreateCountryCommand → CreateCountryCommandHandler
• UpdateCountryCommand → UpdateCountryCommandHandler
• DeleteCountryCommand → DeleteCountryCommandHandler
• CreateStateCommand → CreateStateCommandHandler
• UpdateStateCommand → UpdateStateCommandHandler
• DeleteStateCommand → DeleteStateCommandHandler
• ... (similar pattern for all aggregates)
```

**Command Flow:**
1. Controller receives HTTP request
2. Maps DTO to Command
3. Sends Command via MediatR
4. CommandHandler validates and executes
5. Domain events published to RabbitMQ
6. Returns HTTP 204 No Content (success) or 400/404/500 (error)

#### Queries (Read Operations)
```
• GetAllCountryQuery → GetAllCountryQueryHandler
• GetCountryByIdQuery → GetCountryByIdQueryHandler
• FindAllCitiesQuery → FindAllCitiesQueryHandler
• FindCityByIdQuery → FindCityByIdQueryHandler
• ... (similar pattern for all aggregates)
```

**Query Flow:**
1. Controller receives HTTP request
2. Creates Query with criteria
3. Sends Query via MediatR
4. QueryHandler fetches from MongoDB (with Redis cache)
5. Returns DTOs with HTTP 200 OK

### CQRS Statistics

- **Commands**: 46 total (Create, Update, Delete for 8 aggregates + some special commands)
- **Queries**: 32 total (GetAll, GetById for 8 aggregates + some special queries)
- **Handlers**: 78 total (1 handler per command/query)

## 🧪 Testing

The microservice includes comprehensive test coverage across multiple levels:

### Test Projects

```
tests/
├── unit/
│   ├── Domain.Test/               # Domain logic tests
│   ├── Application.Test/          # Command/Query handler tests
│   ├── Infrastructure.Test/       # Repository tests
│   ├── Rest.Test/                 # REST controller tests
│   └── gRpc.Test/                 # gRPC service tests
│
└── integration/
    ├── Rest.Test/                 # End-to-end REST API tests
    └── gRpc.Test/                 # End-to-end gRPC tests
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run only unit tests
dotnet test --filter "FullyQualifiedName~.Test&Category!=Integration"

# Run only integration tests
dotnet test --filter "Category=Integration"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test tests/unit/CodeDesignPlus.Net.Microservice.Locations.Domain.Test/
```

### Unit Tests

#### Domain Tests
```csharp
[Fact]
public void Create_ValidCountry_Success()
{
    // Arrange
    var id = Guid.NewGuid();
    var currencyId = Guid.NewGuid();
    var createdBy = Guid.NewGuid();
    
    // Act
    var country = CountryAggregate.Create(
        id: id,
        name: "United States",
        alpha2: "US",
        alpha3: "USA",
        code: "840",
        capital: "Washington, D.C.",
        idCurrency: currencyId,
        timeZone: "America/New_York",
        nameNative: "United States",
        region: "Americas",
        subRegion: "Northern America",
        latitude: 38.8951,
        longitude: -77.0364,
        flag: "🇺🇸",
        isActive: true,
        createdBy: createdBy
    );
    
    // Assert
    Assert.NotNull(country);
    Assert.Equal("United States", country.Name);
    Assert.Equal("US", country.Alpha2);
    Assert.Single(country.GetDomainEvents());
    Assert.IsType<CountryCreatedDomainEvent>(country.GetDomainEvents().First());
}

[Fact]
public void Create_InvalidLatitude_ThrowsDomainException()
{
    // Arrange & Act & Assert
    Assert.Throws<DomainException>(() => CountryAggregate.Create(
        id: Guid.NewGuid(),
        name: "Test",
        alpha2: "TS",
        alpha3: "TST",
        code: "999",
        capital: "Capital",
        idCurrency: Guid.NewGuid(),
        timeZone: "UTC",
        nameNative: "Test",
        region: "Test",
        subRegion: "Test",
        latitude: 100, // Invalid!
        longitude: 0,
        flag: null,
        isActive: true,
        createdBy: Guid.NewGuid()
    ));
}
```

#### Application Tests
```csharp
[Fact]
public async Task Handle_CreateCountryCommand_Success()
{
    // Arrange
    var command = new CreateCountryCommand(
        Id: Guid.NewGuid(),
        Name: "United States",
        Alpha2: "US",
        Alpha3: "USA",
        Code: "840",
        Capital: "Washington, D.C.",
        IdCurrency: Guid.NewGuid(),
        Timezone: "America/New_York",
        NameNative: "United States",
        Region: "Americas",
        SubRegion: "Northern America",
        Latitude: 38.8951,
        Longitude: -77.0364,
        Flag: "🇺🇸"
    );
    
    var handler = new CreateCountryCommandHandler(mockRepository.Object);
    
    // Act
    await handler.Handle(command, CancellationToken.None);
    
    // Assert
    mockRepository.Verify(r => r.CreateAsync(It.IsAny<CountryAggregate>(), It.IsAny<CancellationToken>()), Times.Once);
}
```

### Integration Tests

#### REST API Tests
```csharp
[Fact]
public async Task CreateCountry_ValidData_ReturnsNoContent()
{
    // Arrange
    var client = factory.CreateClient();
    var countryDto = new CreateCountryDto
    {
        Id = Guid.NewGuid(),
        Name = "Test Country",
        Alpha2 = "TC",
        Alpha3 = "TST",
        Code = "999",
        Capital = "Test Capital",
        IdCurrency = currencyId,
        Timezone = "UTC",
        NameNative = "Test Country",
        Region = "Test Region",
        SubRegion = "Test SubRegion",
        Latitude = 0,
        Longitude = 0,
        Flag = null
    };
    
    // Act
    var response = await client.PostAsJsonAsync("/api/country", countryDto);
    
    // Assert
    Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
}

[Fact]
public async Task GetCountryById_ExistingId_ReturnsCountry()
{
    // Arrange
    var client = factory.CreateClient();
    var countryId = await CreateTestCountry();
    
    // Act
    var response = await client.GetAsync($"/api/country/{countryId}");
    
    // Assert
    response.EnsureSuccessStatusCode();
    var country = await response.Content.ReadFromJsonAsync<CountryDto>();
    Assert.NotNull(country);
    Assert.Equal(countryId, country.Id);
}
```

#### gRPC Tests
```csharp
[Fact]
public async Task GetCountry_ByAlpha2_ReturnsCountry()
{
    // Arrange
    var channel = GrpcChannel.ForAddress("http://localhost:5001");
    var client = new CountryService.CountryServiceClient(channel);
    await SeedTestCountry(alpha2: "US");
    
    // Act
    var response = await client.GetCountryAsync(new GetCountryRequest 
    { 
        Alpha2 = "US" 
    });
    
    // Assert
    Assert.NotNull(response);
    Assert.Equal("US", response.Alpha2);
    Assert.Equal("United States", response.Name);
    Assert.NotNull(response.Currency);
}
```

### Test Data Builders

Use builders for complex test data:

```csharp
public class CountryBuilder
{
    private Guid id = Guid.NewGuid();
    private string name = "Test Country";
    private string alpha2 = "TC";
    private string alpha3 = "TST";
    // ... other fields
    
    public CountryBuilder WithId(Guid id)
    {
        this.id = id;
        return this;
    }
    
    public CountryBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }
    
    public CountryAggregate Build()
    {
        return CountryAggregate.Create(
            id: id,
            name: name,
            alpha2: alpha2,
            alpha3: alpha3,
            // ... other fields
        );
    }
}

// Usage
var country = new CountryBuilder()
    .WithName("United States")
    .WithAlpha2("US")
    .Build();
```

### Test Coverage Goals

- **Domain Layer**: 95%+ coverage (critical business logic)
- **Application Layer**: 90%+ coverage (command/query handlers)
- **Infrastructure Layer**: 80%+ coverage (repository implementations)
- **API Layer**: 85%+ coverage (controllers/services)
- **Overall**: 80%+ coverage

## 🎯 Best Practices

### 1. Always Use Criteria for Queries

Instead of:
```csharp
var countries = await httpClient.GetAsync("/api/country");
```

Use:
```csharp
var countries = await httpClient.GetAsync("/api/country?page=1&pageSize=20&orderBy=name&filters=isActive:true");
```

### 2. Cache Frequently Accessed Data

```csharp
public class LocationService
{
    private readonly IDistributedCache cache;
    private readonly HttpClient httpClient;
    
    public async Task<CountryDto> GetCountryByAlpha2(string alpha2)
    {
        var cacheKey = $"location:country:alpha2:{alpha2}";
        
        var cached = await cache.GetStringAsync(cacheKey);
        if (cached != null)
            return JsonSerializer.Deserialize<CountryDto>(cached);
        
        var country = await httpClient.GetAsync($"/api/country?filters=alpha2:{alpha2}");
        
        await cache.SetStringAsync(cacheKey, 
            JsonSerializer.Serialize(country),
            new DistributedCacheEntryOptions { AbsoludeExpirationRelativeToNow = TimeSpan.FromHours(1) }
        );
        
        return country;
    }
}
```

### 3. Use gRPC for Inter-Service Communication

For microservice-to-microservice communication, prefer gRPC over REST:

```csharp
// Instead of REST
var response = await httpClient.GetAsync($"http://ms-locations/api/country/{id}");
var country = await response.Content.ReadFromJsonAsync<CountryDto>();

// Use gRPC
var channel = GrpcChannel.ForAddress("http://ms-locations-grpc:5001");
var client = new CountryService.CountryServiceClient(channel);
var country = await client.GetCountryAsync(new GetCountryRequest { Id = id.ToString() });
```

### 4. Handle Hierarchical Deletes Carefully

Before deleting a parent entity, check for children:

```csharp
// ❌ Bad - can orphan data
await httpClient.DeleteAsync($"/api/country/{countryId}");

// ✅ Good - check for dependencies first
var states = await httpClient.GetAsync($"/api/state?filters=idCountry:{countryId}");
if (states.Total > 0)
{
    throw new InvalidOperationException("Cannot delete country with existing states");
}
await httpClient.DeleteAsync($"/api/country/{countryId}");
```

### 5. Use Soft Deletes

The microservice uses soft deletes (IsDeleted flag) to maintain referential integrity:

```csharp
// Deleted entities are still in the database but marked IsDeleted = true
// They won't appear in queries unless explicitly requested
var activeCountries = await httpClient.GetAsync("/api/country?filters=isDeleted:false");

// To permanently delete (if needed), use a separate admin API
await adminClient.DeleteAsync($"/api/admin/country/{id}/permanent");
```

### 6. Validate Input at Multiple Levels

```csharp
// 1. Client-side validation (UI)
if (string.IsNullOrWhiteSpace(countryName))
    return ValidationError("Country name is required");

// 2. DTO validation (FluentValidation)
public class CreateCountryDtoValidator : AbstractValidator<CreateCountryDto>
{
    public CreateCountryDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Alpha2).Length(2);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
    }
}

// 3. Domain validation (DomainGuard)
DomainGuard.IsNullOrEmpty(name, Errors.NameIsInvalid);
DomainGuard.IsNotInRange(latitude, -90, 90, Errors.LatitudeIsInvalid);
```

### 7. Leverage Domain Events

Subscribe to domain events for loose coupling:

```csharp
public class NotificationService : INotificationHandler<CountryCreatedDomainEvent>
{
    public async Task Handle(CountryCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // Send notification to admins
        await notificationClient.SendAsync(new Notification
        {
            Title = "New Country Added",
            Message = $"Country {notification.Name} ({notification.Alpha2}) has been created",
            Recipients = await GetAdminUsers()
        });
    }
}
```

### 8. Use Pagination for Large Result Sets

```csharp
// ❌ Bad - can return thousands of records
var cities = await httpClient.GetAsync("/api/city");

// ✅ Good - paginate results
var pageSize = 20;
var page = 1;
var cities = await httpClient.GetAsync($"/api/city?page={page}&pageSize={pageSize}");

// Load more as needed
while (cities.Items.Count > 0)
{
    ProcessCities(cities.Items);
    page++;
    cities = await httpClient.GetAsync($"/api/city?page={page}&pageSize={pageSize}");
}
```

### 9. Implement Idempotency

Use client-generated GUIDs for idempotent creates:

```csharp
// Client generates ID
var countryId = Guid.NewGuid();

// First request - creates country
await httpClient.PostAsync("/api/country", new CreateCountryDto { Id = countryId, ... });

// Duplicate request (network retry) - idempotent
await httpClient.PostAsync("/api/country", new CreateCountryDto { Id = countryId, ... });
// Server checks if ID exists, returns success without creating duplicate
```

### 10. Monitor and Trace

Enable observability for production:

```json
{
  "Observability": {
    "Enable": true,
    "ServerOtel": "http://otel-collector:4317",
    "Trace": {
      "Enable": true,
      "AspNetCore": true,
      "GrpcClient": true,
      "CodeDesignPlusSdk": true,
      "Redis": true,
      "RabbitMQ": true
    }
  }
}
```

Then use distributed tracing tools (Jaeger, Zipkin, Grafana Tempo) to diagnose issues.

## 🔧 Troubleshooting

### Common Issues

#### 1. MongoDB Connection Fails

**Symptoms:**
```
System.TimeoutException: A timeout occurred after 30000ms selecting a server
```

**Solutions:**
```bash
# Check MongoDB is running
docker ps | grep mongo

# Verify connection string
echo $MONGO_CONNECTION_STRING

# Test connection manually
mongo mongodb://user:pass@localhost:27017/db-ms-locations

# Check Vault secrets
vault kv get security-codedesignplus/ms-locations/mongo

# Restart MongoDB
docker restart <mongo-container-id>
```

#### 2. Redis Cache Not Working

**Symptoms:**
- Cache misses on every request
- Performance degradation

**Solutions:**
```bash
# Check Redis is running
docker ps | grep redis

# Test Redis connection
redis-cli -h localhost -p 6379 ping

# Check cache configuration
# appsettings.json
{
  "RedisCache": {
    "Enable": true,  // ← Must be true
    "Expiration": "00:05:00"
  }
}

# Clear Redis cache
redis-cli -h localhost -p 6379 FLUSHALL
```

#### 3. RabbitMQ Events Not Publishing

**Symptoms:**
- Domain events not received by subscribers
- No messages in RabbitMQ queues

**Solutions:**
```bash
# Check RabbitMQ is running
docker ps | grep rabbitmq

# Access RabbitMQ management UI
open http://localhost:15672
# Login: user / pass

# Check exchanges and queues
# Should see exchanges: location.events, etc.

# Verify configuration
{
  "RabbitMQ": {
    "Enable": true,  // ← Must be true
    "Host": "localhost",
    "Port": 5672,
    "UserName": "user",
    "Password": "pass"
  }
}

# Check logs for publishing errors
docker logs <locations-container-id> | grep "RabbitMQ"
```

#### 4. Swagger UI Not Loading

**Symptoms:**
```
404 Not Found when accessing /ms-locations/swagger
```

**Solutions:**
```bash
# Check PathBase configuration
{
  "Core": {
    "PathBase": "/ms-locations"  // ← Must match URL path
  }
}

# Try without PathBase
open http://localhost:5000/swagger

# Check environment
echo $ASPNETCORE_ENVIRONMENT
# Swagger is enabled in Development by default

# Verify Swagger middleware order in Program.cs
app.UseCoreSwagger();  // ← After UseRouting, before MapControllers
```

#### 5. JWT Authentication Fails

**Symptoms:**
```
401 Unauthorized on all API requests
```

**Solutions:**
```bash
# Check Security configuration
{
  "Security": {
    "ValidIssuer": "https://your-auth-server",  // ← Must match token issuer
    "ValidAudiences": ["ms-locations"],
    "ValidateLifetime": true
  }
}

# Decode JWT to verify claims
# Use https://jwt.io or:
echo $TOKEN | cut -d. -f2 | base64 -d | jq

# Verify token has required claims
# - iss (issuer)
# - aud (audience)
# - exp (expiration)
# - sub (subject/user)

# Check token is not expired
date -u

# For local development, disable auth temporarily
[AllowAnonymous]  // ← Add to controller action
```

#### 6. Country Creation Fails - Invalid Currency

**Symptoms:**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "Domain Error",
  "status": 400,
  "detail": "IdCurrency is invalid"
}
```

**Solutions:**
```csharp
// Create currency first
var currency = new CreateCurrencyDto
{
    Id = Guid.NewGuid(),
    Code = "USD",
    NumericCode = 840,
    DecimalDigits = 2,
    Symbol = "$",
    Name = "US Dollar"
};
await httpClient.PostAsync("/api/currency", currency);

// Then create country with valid currency ID
var country = new CreateCountryDto
{
    // ...
    IdCurrency = currency.Id  // ← Use created currency ID
};
await httpClient.PostAsync("/api/country", country);
```

#### 7. gRPC Service Not Responding

**Symptoms:**
```
Status(StatusCode="Unavailable", Detail="failed to connect to all addresses")
```

**Solutions:**
```bash
# Check gRPC service is running
curl http://localhost:5001

# Verify gRPC reflection is enabled (Development only)
grpcurl -plaintext localhost:5001 list

# Check Kestrel configuration
{
  "Kestrel": {
    "Endpoints": {
      "Grpc": {
        "Url": "http://*:5001",
        "Protocols": "Http2"  // ← Must be Http2
      }
    }
  }
}

# Test with grpcurl
grpcurl -plaintext -d '{"alpha2": "US"}' localhost:5001 country.CountryService/GetCountry

# Check firewall rules
sudo ufw status
# Ensure port 5001 is open
```

#### 8. Vault Secrets Not Loading

**Symptoms:**
```
Microsoft.Extensions.Configuration.ConfigurationException: Vault is not available
```

**Solutions:**
```bash
# Check Vault is running
docker ps | grep vault

# Verify Vault configuration
{
  "Vault": {
    "Enable": true,
    "Address": "http://localhost:8200",
    "Token": "root",
    "AppName": "ms-locations",
    "Solution": "security-codedesignplus"
  }
}

# Run Vault configuration script
cd tools/vault
./config-vault.sh

# Manually write secrets
vault kv put security-codedesignplus/ms-locations/mongo \
  username=admin \
  password=pass123

# For local dev, disable Vault and use appsettings
{
  "Vault": {
    "Enable": false  // ← Use appsettings instead
  },
  "Mongo": {
    "ConnectionString": "mongodb://admin:pass123@localhost:27017"
  }
}
```

#### 9. Performance Issues - Slow Queries

**Symptoms:**
- API responses take > 1 second
- High CPU/memory usage

**Solutions:**
```bash
# Enable MongoDB diagnostics
{
  "Mongo": {
    "Diagnostic": {
      "Enable": true,
      "EnableCommandText": true
    }
  }
}

# Check for missing indexes
# MongoDB automatically indexes _id, but add indexes for common queries
db.countries.createIndex({ "alpha2": 1 });
db.states.createIndex({ "idCountry": 1 });
db.cities.createIndex({ "idState": 1 });

# Enable Redis cache
{
  "RedisCache": {
    "Enable": true,
    "Expiration": "00:05:00"
  }
}

# Use pagination
GET /api/country?page=1&pageSize=20

# Monitor with Application Insights / Grafana
{
  "Observability": {
    "Enable": true,
    "Metrics": {
      "Enable": true
    }
  }
}
```

#### 10. Docker Container Fails to Start

**Symptoms:**
```
Error response from daemon: Container ... is not running
```

**Solutions:**
```bash
# Check container logs
docker logs <container-id>

# Verify Docker network
docker network ls
docker network inspect backend

# Check environment variables
docker inspect <container-id> | jq '.[0].Config.Env'

# Rebuild Docker image
docker build -t ms-locations-rest . -f src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.Rest/Dockerfile

# Run with proper network
docker run -d -p 5000:5000 --network=backend \
  -e ASPNETCORE_ENVIRONMENT=Docker \
  --name ms-locations-rest \
  ms-locations-rest

# Check resource limits
docker stats
# Increase memory if needed
docker update --memory 2g <container-id>
```

### Logging and Diagnostics

Enable verbose logging for troubleshooting:

```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "System": "Warning"
      }
    }
  },
  "Logger": {
    "Enable": true,
    "Level": "Debug"
  }
}
```

View logs:
```bash
# Docker logs
docker logs -f ms-locations-rest

# Kubernetes logs
kubectl logs -f deployment/ms-locations-rest -n default

# Filter logs
docker logs ms-locations-rest | grep "ERROR"
docker logs ms-locations-rest | grep "CountryController"
```

## 🚢 Deployment

### Docker Deployment

#### Build Docker Images

```bash
# REST API
docker build -t ms-locations-rest:latest \
  -f src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.Rest/Dockerfile \
  .

# gRPC Service
docker build -t ms-locations-grpc:latest \
  -f src/entrypoints/CodeDesignPlus.Net.Microservice.Locations.gRpc/Dockerfile \
  .
```

#### Run with Docker Compose

```yaml
# docker-compose.yml
version: '3.8'

services:
  ms-locations-rest:
    image: ms-locations-rest:latest
    ports:
      - "5000:5000"
    environment:
      - ASPNETCORE_ENVIRONMENT=Docker
      - Core__PathBase=/ms-locations
    networks:
      - backend
    depends_on:
      - mongodb
      - redis
      - rabbitmq

  ms-locations-grpc:
    image: ms-locations-grpc:latest
    ports:
      - "5001:5001"
    environment:
      - ASPNETCORE_ENVIRONMENT=Docker
    networks:
      - backend
    depends_on:
      - mongodb
      - redis
      - rabbitmq

networks:
  backend:
    external: true
```

```bash
docker-compose up -d
```

### Kubernetes Deployment

#### Using Helm Charts

```bash
# Add Helm repository
helm repo add codedesignplus https://www.codedesignplus.com/helm-charts/
helm repo update

# Install REST API
helm install ms-locations-rest charts/ms-locations-rest \
  --namespace default \
  --set image.tag=1.0.0 \
  --set ingress.enabled=true \
  --set ingress.host=api.yourdomain.com

# Install gRPC Service
helm install ms-locations-grpc charts/ms-locations-grpc \
  --namespace default \
  --set image.tag=1.0.0
```

#### Manual Kubernetes Deployment

```yaml
# deployment.yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: ms-locations-rest
spec:
  replicas: 3
  selector:
    matchLabels:
      app: ms-locations-rest
  template:
    metadata:
      labels:
        app: ms-locations-rest
    spec:
      containers:
      - name: ms-locations-rest
        image: registry.yourdomain.com/ms-locations-rest:1.0.0
        ports:
        - containerPort: 5000
        env:
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
        - name: Core__PathBase
          value: "/ms-locations"
        resources:
          requests:
            memory: "256Mi"
            cpu: "250m"
          limits:
            memory: "512Mi"
            cpu: "500m"
        livenessProbe:
          httpGet:
            path: /ms-locations/health
            port: 5000
          initialDelaySeconds: 30
          periodSeconds: 10
        readinessProbe:
          httpGet:
            path: /ms-locations/health
            port: 5000
          initialDelaySeconds: 10
          periodSeconds: 5
---
apiVersion: v1
kind: Service
metadata:
  name: ms-locations-rest
spec:
  selector:
    app: ms-locations-rest
  ports:
  - protocol: TCP
    port: 80
    targetPort: 5000
  type: ClusterIP
```

```bash
kubectl apply -f deployment.yaml
```

### CI/CD Pipeline

The microservice includes GitHub Actions workflows:

#### Continuous Integration (.github/workflows/ci.yml)

Triggers on push to any branch:
- Build solution
- Run unit tests
- Run integration tests
- Code quality analysis (SonarQube)

#### Continuous Deployment (.github/workflows/cd.yml)

Manual trigger with environment selection (Staging/Production):
1. **Version Job**: Create GitHub release with semantic versioning
2. **Container Job**: Build and push Docker images to registry
3. **Deploy Job**: Deploy to Kubernetes via Helm

```bash
# Trigger deployment
gh workflow run cd.yml --field environment=Staging
```

### Environment Configuration

#### Staging
```json
{
  "Core": {
    "PathBase": "/ms-locations"
  },
  "Mongo": {
    "ConnectionString": "mongodb://mongo-staging:27017/db-ms-locations"
  },
  "Redis": {
    "ConnectionString": "redis-staging:6379"
  },
  "RabbitMQ": {
    "Host": "rabbitmq-staging",
    "Port": 5672
  },
  "Vault": {
    "Address": "https://vault-staging.yourdomain.com"
  }
}
```

#### Production
```json
{
  "Core": {
    "PathBase": "/ms-locations"
  },
  "Mongo": {
    "ConnectionString": "mongodb+srv://prod-cluster.mongodb.net/db-ms-locations"
  },
  "Redis": {
    "ConnectionString": "redis-prod.cache.amazonaws.com:6379,ssl=true"
  },
  "RabbitMQ": {
    "Host": "rabbitmq-prod.yourdomain.com",
    "Port": 5672,
    "UseSsl": true
  },
  "Vault": {
    "Address": "https://vault.yourdomain.com"
  },
  "Observability": {
    "Enable": true,
    "ServerOtel": "https://otel-collector.yourdomain.com:4317"
  }
}
```

### Scaling

#### Horizontal Scaling

```bash
# Scale REST API
kubectl scale deployment ms-locations-rest --replicas=5

# Scale gRPC Service
kubectl scale deployment ms-locations-grpc --replicas=3

# Auto-scaling based on CPU
kubectl autoscale deployment ms-locations-rest \
  --cpu-percent=70 \
  --min=2 \
  --max=10
```

#### Vertical Scaling

```yaml
# Increase resource limits
resources:
  requests:
    memory: "512Mi"
    cpu: "500m"
  limits:
    memory: "1Gi"
    cpu: "1000m"
```

### Monitoring

#### Health Checks

```bash
# Kubernetes health check
kubectl get pods -l app=ms-locations-rest

# HTTP health endpoint
curl http://ms-locations-rest/ms-locations/health

# gRPC health check
grpcurl -plaintext ms-locations-grpc:5001 grpc.health.v1.Health/Check
```

#### Metrics

```bash
# Prometheus metrics endpoint
curl http://ms-locations-rest/metrics

# Key metrics to monitor:
# - http_requests_duration_seconds
# - mongodb_operations_total
# - redis_cache_hits_total
# - rabbitmq_messages_published_total
```

#### Distributed Tracing

Enable OpenTelemetry and visualize traces in Jaeger/Grafana Tempo:

```json
{
  "Observability": {
    "Enable": true,
    "ServerOtel": "http://otel-collector:4317",
    "Trace": {
      "Enable": true,
      "AspNetCore": true,
      "GrpcClient": true,
      "Redis": true,
      "RabbitMQ": true
    }
  }
}
```

## ❓ FAQ

### General Questions

**Q: What is the difference between REST and gRPC entrypoints?**

A: 
- **REST API** (Port 5000): Human-readable JSON over HTTP. Best for web/mobile clients, external integrations, and when HTTP/JSON is required.
- **gRPC** (Port 5001): Binary Protocol Buffers over HTTP/2. Best for microservice-to-microservice communication, high-performance scenarios, and when type safety is critical.

**Q: Can I use both REST and gRPC simultaneously?**

A: Yes! Both entrypoints run independently and can be deployed together. Use REST for web clients and gRPC for internal microservice communication.

**Q: Is multi-tenancy enforced automatically?**

A: Yes, when the `X-Tenant` header is provided, the microservice automatically filters all queries by tenant and associates all writes with the tenant.

**Q: What happens if I delete a country that has states?**

A: The delete will succeed (soft delete), but you should check for dependent entities first. The microservice uses soft deletes (IsDeleted flag) to maintain referential integrity, but you're responsible for managing the hierarchy.

### Data Model Questions

**Q: Why use GUIDs instead of auto-increment integers?**

A: GUIDs provide several benefits:
- Client-generated IDs enable idempotent operations
- No need for database round-trips to get new IDs
- Distributed system friendly (no coordination needed)
- Prevents enumeration attacks
- Supports eventual consistency and offline scenarios

**Q: Can I add custom fields to Country/State/City?**

A: The domain aggregates have fixed schemas. For custom fields, you can:
1. Extend the domain model (requires code changes)
2. Use a separate "LocationMetadata" aggregate with key-value pairs
3. Maintain custom data in your own microservice and reference Locations by ID

**Q: How do I handle location data that changes over time?**

A: The microservice tracks audit fields (CreatedAt, UpdatedAt, DeletedAt). For historical tracking:
1. Use soft deletes (IsDeleted) to preserve old data
2. Implement event sourcing (store all domain events)
3. Create a separate "LocationHistory" aggregate
4. Use temporal tables in your data store

### Integration Questions

**Q: How do I integrate with external location APIs (Google Maps, Nominatim)?**

A: The microservice stores location data but doesn't integrate with external APIs directly. You can:
1. Import data from external sources via bulk API calls
2. Create a separate service that syncs Locations with external APIs
3. Use domain events to trigger external updates when locations change

**Q: Can I use this microservice for geocoding (address to coordinates)?**

A: Not directly. The microservice stores coordinates for countries, but not for states/cities/localities. For geocoding:
1. Add latitude/longitude to City/Locality/Neighborhood aggregates
2. Use an external geocoding API (Google Maps, Nominatim)
3. Store geocoded results in a separate "Address" microservice

**Q: How do I subscribe to location change events?**

A: Location changes publish domain events to RabbitMQ. Subscribe to these events:

```csharp
public class LocationChangeSubscriber : INotificationHandler<CountryUpdatedDomainEvent>
{
    public async Task Handle(CountryUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        // React to country updates
        await myService.UpdateRelatedData(notification.Id, notification.Name);
    }
}
```

### Performance Questions

**Q: What's the expected throughput?**

A: Depends on infrastructure, but typical results:
- **REST API**: 1,000-2,000 requests/second per instance
- **gRPC**: 5,000-10,000 requests/second per instance
- **MongoDB**: 10,000+ writes/second (cluster)
- **Redis**: 100,000+ cache hits/second

**Q: How do I optimize for high read volume?**

A:
1. Enable Redis caching (5-minute TTL default)
2. Use gRPC instead of REST for internal calls
3. Scale horizontally (add more REST/gRPC instances)
4. Add read replicas to MongoDB
5. Use CDN for static country/currency data

**Q: How do I optimize for high write volume?**

A:
1. Use bulk operations when creating multiple locations
2. Batch domain events for RabbitMQ
3. Scale MongoDB (sharding, replication)
4. Use asynchronous command handlers
5. Consider eventual consistency for non-critical updates

### Security Questions

**Q: How is authentication handled?**

A: The microservice uses OAuth2/OpenID Connect with JWT Bearer tokens. All endpoints (except anonymous queries like GetAllCountries) require authentication.

**Q: How do I implement authorization (who can create/update/delete)?**

A: The microservice supports RBAC (Role-Based Access Control) via the Security configuration:

```json
{
  "Security": {
    "ValidateRbac": true,
    "ServerRbac": "http://ms-rbac:5001"
  }
}
```

When enabled, it checks user roles against the RBAC microservice.

**Q: Are there rate limits?**

A: Rate limiting is not built-in. Implement it at the API Gateway level (Kong, Nginx, Traefik) or use ASP.NET Core Rate Limiting middleware.

**Q: How are secrets managed?**

A: Secrets are stored in HashiCorp Vault. The microservice loads secrets at startup:
- MongoDB credentials
- RabbitMQ credentials
- API keys (if any)

For local development, you can disable Vault and use appsettings.json.

## 🤝 Contributing

We welcome contributions! Please follow these guidelines:

### Getting Started

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Make your changes
4. Write tests for your changes
5. Run tests (`dotnet test`)
6. Commit your changes (`git commit -m 'Add amazing feature'`)
7. Push to the branch (`git push origin feature/amazing-feature`)
8. Open a Pull Request

### Coding Standards

- Follow C# coding conventions
- Use meaningful variable/method names
- Add XML comments for public APIs
- Keep methods small and focused (< 20 lines)
- Write unit tests for all business logic
- Maintain > 80% test coverage

### Commit Messages

Follow Conventional Commits:

```
feat: add timezone offset validation
fix: correct country latitude validation range
docs: update README with gRPC examples
test: add integration tests for state endpoints
refactor: simplify country aggregate creation
```

### Pull Request Process

1. Update README.md with details of changes (if applicable)
2. Add tests for new functionality
3. Ensure all tests pass
4. Update version number following SemVer
5. PR will be merged once reviewed and approved

## 📄 License

This project is licensed under the GNU Lesser General Public License v3.0 - see the [LICENSE.md](LICENSE.md) file for details.

### What This Means

- ✅ You can use this microservice in commercial projects
- ✅ You can modify the source code
- ✅ You can distribute modified versions
- ⚠️ You must disclose source code of modified versions
- ⚠️ You must include the original license
- ⚠️ Changes must be documented

## 📞 Support

### Documentation

- [CodeDesignPlus SDK Documentation](https://codedesignplus.github.io/)
- [Microservice Commons Library](https://github.com/codedesignplus/CodeDesignPlus.Net.Sdk)

### Contact

- **Email**: support@codedesignplus.com
- **Website**: https://www.codedesignplus.com
- **GitHub**: https://github.com/codedesignplus

### Reporting Issues

Found a bug? Have a feature request? Please open an issue on GitHub:

1. Go to https://github.com/codedesignplus/CodeDesignPlus.Net.Microservice.Locations/issues
2. Click "New Issue"
3. Choose appropriate template (Bug Report, Feature Request)
4. Provide detailed information
5. Submit issue

### Community

- **Discussions**: https://github.com/codedesignplus/CodeDesignPlus.Net.Microservice.Locations/discussions
- **Stack Overflow**: Tag questions with `codedesignplus` and `locations-microservice`

---

Made with ❤️ by [CodeDesignPlus](https://www.codedesignplus.com)

**Version**: 1.0.0  
**Last Updated**: 2025-05-15
