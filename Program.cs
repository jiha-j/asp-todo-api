using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Services;

// ==========================================
// Program.cs - Application Entry Point
// ==========================================
//
// [한글 설명]
// 이 파일은 ASP.NET Core 애플리케이션의 진입점입니다.
// Java Spring의 @SpringBootApplication과 Application.java의 main() 메서드와 유사합니다.
// 여기서 다음을 구성합니다:
// 1. 의존성 주입 (Dependency Injection)
// 2. 미들웨어 파이프라인 (Middleware Pipeline)
// 3. 데이터베이스 연결 (Database Connection)
// 4. CORS 설정 (Cross-Origin Resource Sharing)
//
// [English Explanation]
// This file is the entry point of ASP.NET Core application.
// Similar to @SpringBootApplication and Application.java's main() method in Java Spring.
// Here we configure:
// 1. Dependency Injection
// 2. Middleware Pipeline
// 3. Database Connection
// 4. CORS (Cross-Origin Resource Sharing)
//
// Java Spring 비교:
// @SpringBootApplication
// public class Application {
//     public static void main(String[] args) {
//         SpringApplication.run(Application.class, args);
//     }
// }

// ==========================================
// WebApplication Builder - 애플리케이션 구성
// ==========================================
//
// [한글] WebApplicationBuilder는 애플리케이션을 구성하는 빌더 패턴입니다.
//       Java Spring의 SpringApplicationBuilder와 유사합니다.
//
// [English] WebApplicationBuilder is builder pattern for configuring application.
//           Similar to SpringApplicationBuilder in Java Spring.
var builder = WebApplication.CreateBuilder(args);

// ==========================================
// STEP 1: Configure Services (서비스 구성)
// ==========================================
//
// [한글] builder.Services는 DI 컨테이너에 서비스를 등록하는 곳입니다.
//       Java Spring의 @Configuration과 @Bean 메서드와 유사합니다.
//
// [English] builder.Services is where we register services in DI container.
//           Similar to @Configuration and @Bean methods in Java Spring.

// ==========================================
// 1.1. Add Controllers
// ==========================================
//
// [한글] Controller를 DI 컨테이너에 등록합니다.
//       MVC 패턴의 Controller 기능을 활성화합니다.
//
// [English] Register Controllers in DI container.
//           Enables Controller functionality of MVC pattern.
//
// Java Spring 비교:
// @EnableWebMvc (자동으로 활성화됨)
builder.Services.AddControllers();

// ==========================================
// 1.2. Add Database Context
// ==========================================
//
// [한글] Entity Framework Core의 DbContext를 등록합니다.
//       여기서는 In-Memory 데이터베이스를 사용합니다 (학습용으로 간단함).
//
//       옵션 설명:
//       - AddDbContext<TodoContext>: TodoContext를 DI 컨테이너에 등록
//       - UseInMemoryDatabase("TodoDb"): 메모리 내 데이터베이스 사용 (재시작 시 데이터 초기화)
//
//       실제 프로젝트에서는 다음과 같이 사용합니다:
//       - SQLite: options.UseSqlite(connectionString)
//       - SQL Server: options.UseSqlServer(connectionString)
//       - PostgreSQL: options.UseNpgsql(connectionString)
//
// [English] Register Entity Framework Core's DbContext.
//           Using In-Memory database here (simple for learning).
//
//           Option explanations:
//           - AddDbContext<TodoContext>: Register TodoContext in DI container
//           - UseInMemoryDatabase("TodoDb"): Use in-memory database (data reset on restart)
//
//           In real projects, use:
//           - SQLite: options.UseSqlite(connectionString)
//           - SQL Server: options.UseSqlServer(connectionString)
//           - PostgreSQL: options.UseNpgsql(connectionString)
//
// Java Spring 비교:
// @Configuration
// public class DatabaseConfig {
//     @Bean
//     public DataSource dataSource() { ... }
// }

// ==========================================
// SQL Server 연결 설정
// ==========================================
// [한글] appsettings.json에서 연결 문자열을 읽어와 SQL Server를 사용합니다.
// [English] Read connection string from appsettings.json and use SQL Server.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<TodoContext>(options =>
    options.UseSqlServer(connectionString));

// ==========================================
// Alternative: SQLite Configuration (대안: SQLite 설정)
// ==========================================
//
// [한글] SQLite를 사용하려면 아래 코드의 주석을 해제하고
//       위의 UseInMemoryDatabase 코드를 주석 처리하세요.
//       NuGet 패키지 필요: Microsoft.EntityFrameworkCore.Sqlite
//
// [English] To use SQLite, uncomment code below
//           and comment out UseInMemoryDatabase code above.
//           Required NuGet package: Microsoft.EntityFrameworkCore.Sqlite
//
// var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// builder.Services.AddDbContext<TodoContext>(options =>
//     options.UseSqlite(connectionString));
//
// appsettings.json에 추가:
// "ConnectionStrings": {
//   "DefaultConnection": "Data Source=todos.db"
// }

// ==========================================
// 1.3. Add Service Layer
// ==========================================
//
// [한글] ITodoService 인터페이스와 TodoService 구현체를 DI 컨테이너에 등록합니다.
//
//       AddScoped: 요청당 하나의 인스턴스 생성
//       - HTTP 요청이 들어올 때마다 새 인스턴스 생성
//       - 같은 요청 내에서는 동일한 인스턴스 재사용
//       - Java Spring의 @RequestScope와 유사
//
//       다른 생명주기 옵션:
//       - AddTransient: 요청할 때마다 새 인스턴스 생성 (@Prototype)
//       - AddSingleton: 애플리케이션당 하나의 인스턴스 (@Singleton)
//
// [English] Register ITodoService interface and TodoService implementation in DI container.
//
//           AddScoped: Create one instance per request
//           - New instance created for each HTTP request
//           - Same instance reused within same request
//           - Similar to @RequestScope in Java Spring
//
//           Other lifecycle options:
//           - AddTransient: New instance every time requested (@Prototype)
//           - AddSingleton: One instance per application (@Singleton)
//
// Java Spring 비교:
// @Service
// @Scope("request")
// public class TodoService implements ITodoService { ... }
builder.Services.AddScoped<ITodoService, TodoService>();

// ==========================================
// 1.4. Add CORS Policy
// ==========================================
//
// [한글] CORS (Cross-Origin Resource Sharing)를 구성합니다.
//       PHP 프론트엔드가 다른 포트에서 실행될 때 API 호출을 허용합니다.
//
//       예시:
//       - API 서버: http://localhost:5000
//       - PHP 프론트엔드: http://localhost:8080
//       → CORS 설정 없이는 브라우저가 요청을 차단합니다.
//
//       이 설정은 개발 환경용입니다. 프로덕션에서는 특정 도메인만 허용하세요.
//
// [English] Configure CORS (Cross-Origin Resource Sharing).
//           Allows API calls when PHP frontend runs on different port.
//
//           Example:
//           - API server: http://localhost:5000
//           - PHP frontend: http://localhost:8080
//           → Without CORS, browser blocks the request.
//
//           This is for development. In production, allow only specific domains.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            // [한글] 모든 출처(Origin), 헤더, HTTP 메서드를 허용합니다.
            // [English] Allow all origins, headers, and HTTP methods.
            policy.AllowAnyOrigin()      // 모든 도메인 허용 / Allow any domain
                  .AllowAnyMethod()      // 모든 HTTP 메서드 허용 (GET, POST, PUT, DELETE 등)
                  .AllowAnyHeader();     // 모든 HTTP 헤더 허용 / Allow any HTTP header
        });

    // ==========================================
    // Production CORS Configuration (프로덕션 CORS 설정)
    // ==========================================
    //
    // [한글] 프로덕션 환경에서는 특정 도메인만 허용하세요:
    // [English] In production, allow only specific domains:
    //
    // options.AddPolicy("ProductionPolicy",
    //     policy =>
    //     {
    //         policy.WithOrigins("https://yourdomain.com", "https://www.yourdomain.com")
    //               .AllowAnyMethod()
    //               .AllowAnyHeader()
    //               .AllowCredentials(); // 쿠키/인증 정보 허용 시
    //     });
});

// ==========================================
// 1.5. Add Swagger/OpenAPI (선택사항)
// ==========================================
//
// [한글] Swagger는 API 문서를 자동으로 생성하고 테스트할 수 있는 도구입니다.
//       개발 환경에서 매우 유용합니다.
//       Java Spring의 SpringDoc OpenAPI와 동일한 역할입니다.
//
// [English] Swagger automatically generates API documentation and testing UI.
//           Very useful in development environment.
//           Same role as SpringDoc OpenAPI in Java Spring.
//
// Swagger UI 접속: http://localhost:5000/swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ==========================================
// STEP 2: Build Application (애플리케이션 빌드)
// ==========================================
//
// [한글] WebApplication 인스턴스를 생성합니다.
//       이 시점에 모든 서비스가 DI 컨테이너에 등록됩니다.
//
// [English] Create WebApplication instance.
//           All services are registered in DI container at this point.
var app = builder.Build();

// ==========================================
// STEP 3: Database Initialization (데이터베이스 초기화)
// ==========================================
//
// [한글] 애플리케이션 시작 시 데이터베이스를 초기화합니다.
//       In-Memory 데이터베이스는 자동으로 생성되지만,
//       실제 데이터베이스(SQLite, SQL Server)는 마이그레이션이 필요합니다.
//
// [English] Initialize database on application startup.
//           In-Memory database is auto-created,
//           but real databases (SQLite, SQL Server) require migrations.
using (var scope = app.Services.CreateScope())
{
    // [한글] Scope를 생성하여 DI 컨테이너에서 서비스를 가져옵니다.
    // [English] Create scope to get services from DI container.
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<TodoContext>();

    // [한글] 마이그레이션을 실행하여 데이터베이스를 업데이트합니다.
    //       모든 대기 중인 마이그레이션이 자동으로 적용됩니다.
    // [English] Run migrations to update the database.
    //           All pending migrations are automatically applied.
    await context.Database.MigrateAsync();

    // ==========================================
    // Migration Commands (마이그레이션 명령어)
    // ==========================================
    //
    // [한글] 마이그레이션 관리 명령어:
    // [English] Migration management commands:
    //
    // 마이그레이션 생성: dotnet ef migrations add MigrationName
    // 데이터베이스 업데이트: dotnet ef database update
    // 마이그레이션 제거: dotnet ef migrations remove
    // 데이터베이스 삭제: dotnet ef database drop
    //
    // Note: EnsureCreated()는 migration과 함께 사용하면 안 됩니다.
    //       Use Migrate() when using migrations, EnsureCreated() for quick prototyping only.
}

// ==========================================
// STEP 4: Configure Middleware Pipeline (미들웨어 파이프라인 구성)
// ==========================================
//
// [한글] 미들웨어 파이프라인은 HTTP 요청이 처리되는 순서를 정의합니다.
//       각 미들웨어는 요청을 처리하고 다음 미들웨어로 전달합니다.
//
//       처리 순서:
//       1. 요청 수신 → Swagger → CORS → Authorization → Routing → Endpoint
//       2. 응답 반환 ← Swagger ← CORS ← Authorization ← Routing ← Endpoint
//
// [English] Middleware pipeline defines order of HTTP request processing.
//           Each middleware processes request and passes to next middleware.
//
//           Processing order:
//           1. Request → Swagger → CORS → Authorization → Routing → Endpoint
//           2. Response ← Swagger ← CORS ← Authorization ← Routing ← Endpoint
//
// Java Spring 비교:
// Filter Chain (ServletFilter) 또는 Interceptor

// ==========================================
// 4.1. Swagger Middleware (개발 환경만)
// ==========================================
//
// [한글] 개발 환경에서만 Swagger UI를 활성화합니다.
//       프로덕션에서는 보안을 위해 비활성화합니다.
//
// [English] Enable Swagger UI only in development environment.
//           Disabled in production for security.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==========================================
// 4.2. HTTPS Redirection
// ==========================================
//
// [한글] HTTP 요청을 HTTPS로 리디렉션합니다.
//       보안을 위해 권장됩니다.
//
// [English] Redirect HTTP requests to HTTPS.
//           Recommended for security.
app.UseHttpsRedirection();

// ==========================================
// 4.3. CORS Middleware
// ==========================================
//
// [한글] CORS 정책을 적용합니다.
//       반드시 UseRouting() 이전과 UseAuthorization() 이전에 호출해야 합니다.
//
// [English] Apply CORS policy.
//           Must be called before UseRouting() and UseAuthorization().
app.UseCors("AllowAll");

// ==========================================
// 4.4. Authorization Middleware (선택사항)
// ==========================================
//
// [한글] 인증/인가 미들웨어를 활성화합니다.
//       JWT 토큰 인증이나 OAuth를 사용할 때 필요합니다.
//       현재 프로젝트에서는 인증을 사용하지 않지만, 확장성을 위해 추가했습니다.
//
// [English] Enable authentication/authorization middleware.
//           Required when using JWT token authentication or OAuth.
//           Not used in current project, but added for extensibility.
app.UseAuthorization();

// ==========================================
// 4.5. Map Controllers
// ==========================================
//
// [한글] Controller의 엔드포인트를 HTTP 요청에 매핑합니다.
//       이것이 실제 API 라우팅이 작동하는 곳입니다.
//
// [English] Map Controller endpoints to HTTP requests.
//           This is where actual API routing works.
//
// Java Spring 비교:
// @EnableWebMvc + @RequestMapping 자동 스캔
app.MapControllers();

// ==========================================
// STEP 5: Run Application (애플리케이션 실행)
// ==========================================
//
// [한글] 애플리케이션을 실행하고 HTTP 요청을 수신 대기합니다.
//       기본 포트:
//       - HTTP: 5000
//       - HTTPS: 5001
//
//       실행 후 접속 URL:
//       - API: http://localhost:5000/api/todo
//       - Swagger: http://localhost:5000/swagger
//
// [English] Run application and listen for HTTP requests.
//           Default ports:
//           - HTTP: 5000
//           - HTTPS: 5001
//
//           After running, access URLs:
//           - API: http://localhost:5000/api/todo
//           - Swagger: http://localhost:5000/swagger
//
// Java Spring 비교:
// SpringApplication.run(Application.class, args);
app.Run();

// ==========================================
// Middleware Pipeline Diagram (미들웨어 파이프라인 다이어그램)
// ==========================================
//
// [한글] 요청 처리 흐름:
// [English] Request processing flow:
//
// ┌─────────────────────────────────────────────────┐
// │  HTTP Request (클라이언트 요청)                  │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  1. UseHttpsRedirection() - HTTPS 리디렉션       │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  2. UseCors() - CORS 정책 적용                   │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  3. UseAuthorization() - 인증/인가 검사          │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  4. MapControllers() - Controller 라우팅         │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  5. TodoController - 엔드포인트 실행             │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  6. TodoService - 비즈니스 로직                  │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  7. TodoContext - 데이터베이스 작업              │
// └─────────────────────────────────────────────────┘
//                      ↓
// ┌─────────────────────────────────────────────────┐
// │  8. HTTP Response (응답 반환)                    │
// └─────────────────────────────────────────────────┘

// ==========================================
// Dependency Injection Lifecycle (의존성 주입 생명주기)
// ==========================================
//
// [한글] DI 생명주기 비교:
// [English] DI lifecycle comparison:
//
// ASP.NET Core          Lifecycle           Java Spring
// --------------------------------------------------------
// AddSingleton<T>()     앱당 1개            @Singleton
//                       (App lifetime)
//
// AddScoped<T>()        요청당 1개          @RequestScope
//                       (Request lifetime)
//
// AddTransient<T>()     매번 새로 생성      @Prototype
//                       (Created each time)
//
// [한글] 사용 가이드:
// - Singleton: 상태가 없는 서비스 (예: 로거, 설정)
// - Scoped: 데이터베이스 컨텍스트, 비즈니스 서비스
// - Transient: 가벼운 서비스, 단순 유틸리티
//
// [English] Usage guide:
// - Singleton: Stateless services (e.g., logger, configuration)
// - Scoped: Database context, business services
// - Transient: Lightweight services, simple utilities

// ==========================================
// Running the Application (애플리케이션 실행)
// ==========================================
//
// [한글] 명령어:
// 1. 프로젝트 빌드: dotnet build
// 2. 애플리케이션 실행: dotnet run
// 3. 개발 모드 실행 (자동 재시작): dotnet watch run
//
// [English] Commands:
// 1. Build project: dotnet build
// 2. Run application: dotnet run
// 3. Development mode (auto-restart): dotnet watch run
//
// 테스트 URL (Test URLs):
// - GET all todos: http://localhost:5000/api/todos
// - GET by id: http://localhost:5000/api/todos/1
// - POST create: http://localhost:5000/api/todos (with JSON body)
// - PUT update: http://localhost:5000/api/todos/1 (with JSON body)
// - DELETE: http://localhost:5000/api/todos/1
// - Swagger UI: http://localhost:5000/swagger

// ==========================================
// Project Structure Summary (프로젝트 구조 요약)
// ==========================================
//
// TodoApi/
// ├── Controllers/
// │   └── TodoController.cs      (API 엔드포인트 / API endpoints)
// ├── Models/
// │   └── TodoItem.cs             (데이터 모델 / Data model)
// ├── Data/
// │   └── TodoContext.cs          (DB 컨텍스트 / DB context)
// ├── Services/
// │   ├── ITodoService.cs         (서비스 인터페이스 / Service interface)
// │   └── TodoService.cs          (서비스 구현 / Service implementation)
// ├── Program.cs                  (애플리케이션 진입점 / Entry point)
// └── appsettings.json            (설정 파일 / Configuration)
