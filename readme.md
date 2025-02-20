# RabbitMQ Full Stack Application

A full-stack application demonstrating message queuing with RabbitMQ, .NET Web API backend, and React frontend.

## Prerequisites

- .NET 9.0
- Node.js and npm
- Docker
- Visual Studio 2022 or VS Code

## Installation & Setup

### 1. RabbitMQ Setup

```bash
# Start RabbitMQ container
docker run -d --hostname my-rabbit --name my-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
```

RabbitMQ Management UI will be available at: http://localhost:15672

Username: guest

Password: guest

### 2. Backend Setup

#### Visual Studio

1. Open the RabbitMQFullStack solution in Visual Studio.

2. Right-click on the API project and select Set as Startup Project.

3. Press F5 to run the project. The API will be available at: http://localhost:5000

##### Visual Studio Code

```bash
cd src/Backend
dotnet restore
dotnet build
cd API
dotnet run
```

The API will be available at: http://localhost:5000

### 3. Frontend Setup

##### Visual Studio Code

```bash
cd src/Frontend/client
npm install
npm start
```

The React app will be available at: http://localhost:3000

## Project Components

### Backend

API: ASP.NET Core Web API

Core: Domain models and interfaces

Infrastructure: RabbitMQ implementation

### Frontend

React application for sending and receiving messages

Real-time message updates

Message status tracking

### API Endpoints

POST /api/message - Send a message
GET /api/message - Health check

## Configuration

#### Backend (appsettings.json)

```json
json
{
  "RabbitMQ": {
    "HostName": "localhost",
    "UserName": "guest",
    "Password": "guest",
    "Port": 5672
  }
}
```

#### Running the Application

Start RabbitMQ:

```bash
docker start my-rabbit
Start Backend
Visual Studio
Press F5 in Visual Studio.
```

Visual Studio Code

```bash
cd src/Backend/API
dotnet run
Start Frontend
```

```bash
cd src/Frontend/client
npm start
```

## Development Notes

### Backend Dependencies

RabbitMQ.Client (6.5.0)

Microsoft.Extensions.Options

Microsoft.Extensions.Logging

### Frontend Dependencies

React

Modern JavaScript features

Fetch API for HTTP requests

## Troubleshooting

### RabbitMQ Connection Issues:

Verify RabbitMQ container is running

Check connection settings in appsettings.json

Ensure ports 5672 and 15672 are available

### API Connection Issues:

Check CORS settings in Program.cs

Verify API URL in frontend code

Check if API is running on correct port

### Frontend Issues:

Clear npm cache: npm cache clean --force
Delete node_modules and reinstall: rm -rf node_modules && npm install
