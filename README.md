# 할일 목록 애플리케이션 (To-Do List Application)

> **학습 프로젝트**: ASP.NET Core REST API + PHP Frontend 통합
>
> **Learning Project**: ASP.NET Core REST API + PHP Frontend Integration

---

## 📚 목차 (Table of Contents)

1. [프로젝트 개요](#-프로젝트-개요-project-overview)
2. [시스템 아키텍처](#-시스템-아키텍처-system-architecture)
3. [필수 요구사항](#-필수-요구사항-prerequisites)
4. [설치 가이드](#-설치-가이드-installation-guide)
   - [IIS 설치](#1-iis-설치)
   - [.NET SDK 설치](#2-net-sdk-설치)
   - [PHP 설치](#3-php-설치)
   - [SQL Server 설치](#4-sql-server-설치)
5. [IIS 설정](#-iis-설정-iis-configuration)
6. [애플리케이션 실행](#-애플리케이션-실행-running-the-application)
7. [핵심 개념 설명](#-핵심-개념-설명-core-concepts)
8. [API 문서](#-api-문서-api-documentation)
9. [프로젝트 구조](#-프로젝트-구조-project-structure)
10. [문제 해결](#-문제-해결-troubleshooting)
11. [학습 자료](#-학습-자료-learning-resources)

---

## 🎯 프로젝트 개요 (Project Overview)

### 무엇을 만들었나요? (What did we build?)

이 프로젝트는 **REST API 아키텍처**를 사용하여 할일 관리 애플리케이션을 구현했습니다.

This project implements a to-do management application using **REST API architecture**.

```
┌─────────────────┐         HTTP/REST API         ┌─────────────────┐
│                 │ ←────────────────────────────→ │                 │
│  PHP Frontend   │   JSON Data Exchange          │  ASP.NET Core   │
│  (Port 8080)    │                                │  Backend API    │
│                 │                                │  (Port 5000)    │
└─────────────────┘                                └────────┬────────┘
                                                            │
                                                            │ Entity Framework
                                                            │
                                                    ┌───────▼────────┐
                                                    │                │
                                                    │  SQL Server    │
                                                    │  Database      │
                                                    │                │
                                                    └────────────────┘
```

### 기술 스택 (Technology Stack)

| 구성 요소 (Component) | 기술 (Technology) | 역할 (Role) |
|----------------------|-------------------|------------|
| **Frontend** | PHP 8.x + HTML5 + CSS3 + JavaScript (ES6+) | 사용자 인터페이스 (User Interface) |
| **Backend** | ASP.NET Core 8.0 (C#) | REST API 서버 (REST API Server) |
| **Database** | SQL Server / SQL Server Express | 데이터 저장소 (Data Storage) |
| **ORM** | Entity Framework Core | 데이터베이스 접근 (Database Access) |
| **Web Server** | IIS (Internet Information Services) | 웹 호스팅 (Web Hosting) |

### Java Spring과의 비교 (Comparison with Java Spring)

| 측면 (Aspect) | ASP.NET Core | Java Spring |
|--------------|--------------|-------------|
| **언어** | C# | Java |
| **프레임워크** | ASP.NET Core MVC | Spring Boot |
| **ORM** | Entity Framework Core | JPA/Hibernate |
| **의존성 주입** | Built-in DI Container | Spring IoC Container |
| **라우팅** | Attribute Routing (`[HttpGet]`) | Annotation Routing (`@GetMapping`) |
| **설정** | appsettings.json | application.properties |
| **패키지 관리** | NuGet | Maven/Gradle |

**공통점 (Similarities)**:
- 둘 다 MVC 패턴 사용 (Both use MVC pattern)
- 의존성 주입 지원 (Support dependency injection)
- REST API 개발에 최적화 (Optimized for REST API development)
- 엔터프라이즈급 애플리케이션 개발 가능 (Can build enterprise applications)

---

## 🏗️ 시스템 아키텍처 (System Architecture)

### 전체 시스템 흐름도 (Overall System Flow)

```
┌──────────────────────────────────────────────────────────────────────┐
│                          사용자 (User)                                │
│                     브라우저 (Web Browser)                             │
└────────────────────────────┬─────────────────────────────────────────┘
                             │
                             │ 1. HTTP 요청 (HTTP Request)
                             │    http://localhost:8080
                             ▼
┌──────────────────────────────────────────────────────────────────────┐
│                     IIS - PHP 처리 (IIS - PHP Handler)               │
│                                                                       │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  index.php (HTML + JavaScript)                               │   │
│  │  - HTML 구조 렌더링 (Render HTML structure)                  │   │
│  │  - CSS 스타일 적용 (Apply CSS styles)                        │   │
│  │  - JavaScript 실행 (Execute JavaScript)                       │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                       │
└────────────────────────────┬─────────────────────────────────────────┘
                             │
                             │ 2. API 요청 (API Request)
                             │    fetch('http://localhost:5000/api/todos')
                             │    Method: GET/POST/PUT/DELETE
                             │    Content-Type: application/json
                             ▼
┌──────────────────────────────────────────────────────────────────────┐
│               ASP.NET Core Kestrel 서버 (Kestrel Server)             │
│                                                                       │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Middleware Pipeline (미들웨어 파이프라인)                   │   │
│  │  ┌─────────┐  ┌──────┐  ┌──────────┐  ┌──────────────┐     │   │
│  │  │  CORS   │→│ Auth │→│ Routing  │→│ Controllers  │     │   │
│  │  └─────────┘  └──────┘  └──────────┘  └──────────────┘     │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                       │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Controllers/TodoController.cs                               │   │
│  │  - [HttpGet] GetAll() → 모든 할일 조회                       │   │
│  │  - [HttpGet("{id}")] GetById() → 특정 할일 조회              │   │
│  │  - [HttpPost] Create() → 새 할일 생성                        │   │
│  │  - [HttpPut("{id}")] Update() → 할일 수정                    │   │
│  │  - [HttpDelete("{id}")] Delete() → 할일 삭제                 │   │
│  └──────────────────────────┬───────────────────────────────────┘   │
│                              │                                        │
│                              │ 3. 비즈니스 로직 처리                  │
│                              ▼                                        │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Services/TodoService.cs                                     │   │
│  │  - 비즈니스 로직 (Business logic)                            │   │
│  │  - 데이터 검증 (Data validation)                             │   │
│  │  - 트랜잭션 관리 (Transaction management)                    │   │
│  └──────────────────────────┬───────────────────────────────────┘   │
│                              │                                        │
│                              │ 4. 데이터 접근                         │
│                              ▼                                        │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  Data/ApplicationDbContext.cs                                │   │
│  │  - Entity Framework Core                                     │   │
│  │  - DbSet<TodoItem> (데이터베이스 테이블 매핑)                │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                       │
└────────────────────────────┬─────────────────────────────────────────┘
                             │
                             │ 5. SQL 쿼리 (SQL Query)
                             │    SELECT/INSERT/UPDATE/DELETE
                             ▼
┌──────────────────────────────────────────────────────────────────────┐
│                    SQL Server 데이터베이스                            │
│                                                                       │
│  ┌──────────────────────────────────────────────────────────────┐   │
│  │  TodoDb 데이터베이스                                          │   │
│  │  ┌──────────────────────────────────────────────────────┐    │   │
│  │  │  TodoItems 테이블                                    │    │   │
│  │  │  ┌────┬─────────────────────┬──────────────┐        │    │   │
│  │  │  │ Id │ Title               │ IsCompleted  │        │    │   │
│  │  │  ├────┼─────────────────────┼──────────────┤        │    │   │
│  │  │  │ 1  │ "공부하기"          │ false        │        │    │   │
│  │  │  │ 2  │ "운동하기"          │ true         │        │    │   │
│  │  │  └────┴─────────────────────┴──────────────┘        │    │   │
│  │  └──────────────────────────────────────────────────────┘    │   │
│  └──────────────────────────────────────────────────────────────┘   │
│                                                                       │
└───────────────────────────────────────────────────────────────────────┘
```

### 요청/응답 흐름 예시 (Request/Response Flow Example)

#### 예시: 새로운 할일 추가하기

```
1. [사용자] 브라우저에서 "운동하기" 입력 후 "추가" 버튼 클릭
   User enters "Exercise" in browser and clicks "Add" button

2. [JavaScript] addTodo() 함수 실행
   JavaScript executes addTodo() function

   fetch('http://localhost:5000/api/todos', {
       method: 'POST',
       headers: { 'Content-Type': 'application/json' },
       body: JSON.stringify({ title: '운동하기' })
   })

3. [ASP.NET Core] TodoController.Create() 메서드 호출
   ASP.NET Core calls TodoController.Create() method

   [HttpPost]
   public async Task<ActionResult<TodoItem>> Create(TodoItem item)
   {
       await _service.CreateAsync(item);
       return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
   }

4. [Service] TodoService.CreateAsync() 비즈니스 로직 실행
   Service executes TodoService.CreateAsync() business logic

   - 데이터 검증 (Validate data)
   - 기본값 설정 (Set defaults: IsCompleted = false)

5. [Entity Framework] 데이터베이스에 INSERT 쿼리 실행
   Entity Framework executes INSERT query to database

   INSERT INTO TodoItems (Title, IsCompleted)
   VALUES ('운동하기', 0)

6. [SQL Server] 데이터 저장 및 ID 생성 (Id = 3)
   SQL Server saves data and generates ID (Id = 3)

7. [응답] ASP.NET Core → JavaScript로 JSON 반환
   Response: ASP.NET Core returns JSON to JavaScript

   {
       "id": 3,
       "title": "운동하기",
       "isCompleted": false
   }

8. [JavaScript] 성공 메시지 표시 및 목록 새로고침
   JavaScript shows success message and refreshes list

   showSuccess('할일이 추가되었습니다!');
   loadTodos(); // 목록 다시 로드
```

---

## 📋 필수 요구사항 (Prerequisites)

### 소프트웨어 요구사항 (Software Requirements)

| 소프트웨어 | 버전 | 용도 | 다운로드 링크 |
|-----------|------|------|--------------|
| **Windows** | 10/11 또는 Server 2016+ | 운영체제 | - |
| **.NET SDK** | 8.0 이상 | ASP.NET Core 개발 | [다운로드](https://dotnet.microsoft.com/download) |
| **IIS** | 10.0 이상 | 웹 서버 | Windows 기능 |
| **PHP** | 8.0 이상 | 프론트엔드 | [다운로드](https://windows.php.net/download/) |
| **SQL Server** | 2019 이상 (Express 가능) | 데이터베이스 | [다운로드](https://www.microsoft.com/sql-server/sql-server-downloads) |

### 하드웨어 요구사항 (Hardware Requirements)

- **RAM**: 최소 4GB (권장 8GB 이상)
- **디스크 공간**: 최소 10GB
- **프로세서**: x64 아키텍처

---

## 🔧 설치 가이드 (Installation Guide)

### 1. IIS 설치

**IIS란? (What is IIS?)**

IIS (Internet Information Services)는 Microsoft가 개발한 웹 서버입니다.
- Apache, Nginx와 같은 역할
- Windows에서 웹 애플리케이션을 호스팅
- ASP.NET, PHP, Node.js 등을 실행 가능

IIS (Internet Information Services) is a web server developed by Microsoft.
- Similar role to Apache, Nginx
- Hosts web applications on Windows
- Can run ASP.NET, PHP, Node.js, etc.

#### 설치 단계 (Installation Steps)

**Windows 10/11:**

1. **제어판 열기 (Open Control Panel)**
   ```
   시작 메뉴 → "제어판" 검색 → 제어판 실행
   Start Menu → Search "Control Panel" → Run Control Panel
   ```

2. **프로그램 및 기능 (Programs and Features)**
   ```
   제어판 → 프로그램 → 프로그램 및 기능
   Control Panel → Programs → Programs and Features
   ```

3. **Windows 기능 켜기/끄기 (Turn Windows features on or off)**
   ```
   왼쪽 사이드바 → "Windows 기능 켜기/끄기" 클릭
   Left sidebar → Click "Turn Windows features on or off"
   ```

4. **IIS 기능 선택 (Select IIS features)**

   다음 항목들을 체크하세요:
   Check the following items:

   ```
   ✅ Internet Information Services
      ✅ Web Management Tools
         ✅ IIS Management Console
      ✅ World Wide Web Services
         ✅ Application Development Features
            ✅ .NET Extensibility 4.x
            ✅ ASP.NET 4.x
            ✅ CGI (PHP용 필수!)
            ✅ ISAPI Extensions
            ✅ ISAPI Filters
         ✅ Common HTTP Features
            ✅ Default Document
            ✅ Directory Browsing
            ✅ HTTP Errors
            ✅ Static Content
         ✅ Health and Diagnostics
            ✅ HTTP Logging
         ✅ Performance Features
            ✅ Static Content Compression
         ✅ Security
            ✅ Request Filtering
   ```

5. **설치 확인 (Verify Installation)**
   ```
   브라우저에서 http://localhost 접속
   Browser: Navigate to http://localhost

   IIS 기본 페이지가 보이면 성공!
   If you see IIS default page, success!
   ```

**문제 해결 (Troubleshooting):**
- IIS가 시작되지 않으면: `services.msc` → "World Wide Web Publishing Service" 시작
- If IIS doesn't start: `services.msc` → Start "World Wide Web Publishing Service"

---

### 2. .NET SDK 설치

**.NET이란? (What is .NET?)**

.NET은 Microsoft의 오픈소스 개발 플랫폼입니다.
- C#, F#, Visual Basic 등의 언어 지원
- 크로스 플랫폼 (Windows, Linux, macOS)
- ASP.NET Core로 웹 API 개발

.NET is Microsoft's open-source development platform.
- Supports C#, F#, Visual Basic languages
- Cross-platform (Windows, Linux, macOS)
- Build web APIs with ASP.NET Core

#### 설치 단계 (Installation Steps)

1. **다운로드 페이지 방문**
   ```
   https://dotnet.microsoft.com/download
   ```

2. **.NET 8.0 SDK 다운로드**
   - "Download .NET SDK x64" 클릭
   - `dotnet-sdk-8.0.xxx-win-x64.exe` 다운로드

3. **설치 프로그램 실행**
   ```
   다운로드한 파일 더블클릭 → "Install" 클릭
   Double-click downloaded file → Click "Install"
   ```

4. **설치 확인 (Verify Installation)**
   ```powershell
   # PowerShell 또는 명령 프롬프트에서 실행
   # Run in PowerShell or Command Prompt

   dotnet --version
   # 출력 예: 8.0.100
   # Output example: 8.0.100

   dotnet --info
   # .NET SDK 정보 출력
   # Shows .NET SDK information
   ```

---

### 3. PHP 설치

**PHP란? (What is PHP?)**

PHP는 서버 사이드 스크립팅 언어입니다.
- Hypertext Preprocessor의 약자
- 웹 페이지를 동적으로 생성
- WordPress, Laravel 등이 PHP로 만들어짐

PHP is a server-side scripting language.
- Stands for Hypertext Preprocessor
- Generates web pages dynamically
- WordPress, Laravel, etc. are built with PHP

#### 설치 단계 (Installation Steps)

1. **PHP 다운로드**
   ```
   https://windows.php.net/download/

   다운로드 항목:
   - PHP 8.x
   - Thread Safe (TS) x64
   - Zip 파일

   예: php-8.3.0-Win32-vs16-x64.zip
   ```

2. **압축 해제**
   ```
   C:\PHP 폴더에 압축 해제
   Unzip to C:\PHP folder

   결과:
   C:\PHP\php.exe
   C:\PHP\php.ini-development
   C:\PHP\ext\ (확장 모듈)
   ```

3. **php.ini 설정**
   ```powershell
   # php.ini-development를 php.ini로 복사
   # Copy php.ini-development to php.ini

   cd C:\PHP
   copy php.ini-development php.ini
   ```

4. **php.ini 편집 (메모장으로 열기)**

   다음 줄들의 주석(`;`)을 제거하세요:
   Uncomment (remove `;`) from these lines:

   ```ini
   ; 기본 설정
   extension_dir = "ext"

   ; 필수 확장 모듈 활성화
   extension=curl
   extension=mbstring
   extension=openssl
   extension=pdo_sqlsrv  ; SQL Server용
   extension=sqlsrv      ; SQL Server용

   ; 에러 표시 (개발 중) 
   display_errors = On
   error_reporting = E_ALL

   ; 시간대 설정
   date.timezone = Asia/Seoul
   ```

5. **시스템 환경 변수에 PHP 추가** //여기위까지 진행.
   ```
   1. 시작 → "환경 변수" 검색
   2. "시스템 환경 변수 편집" 클릭
   3. "환경 변수" 버튼 클릭
   4. "시스템 변수"에서 "Path" 선택 → "편집"
   5. "새로 만들기" → "C:\PHP" 입력
   6. 확인 → 확인 → 확인
   ```

6. **IIS에서 PHP 설정**

   **IIS Manager 열기:**
   ```
   시작 → "IIS" 검색 → "Internet Information Services (IIS) Manager"
   ```

   **FastCGI 설정:**
   ```
   1. 서버 이름 클릭 (왼쪽)
   2. "Handler Mappings" 더블클릭
   3. 오른쪽 "Add Module Mapping..." 클릭

   설정:
   - Request path: *.php
   - Module: FastCgiModule
   - Executable: C:\PHP\php-cgi.exe
   - Name: PHP_via_FastCGI

   4. "OK" 클릭
   5. "Yes" 클릭 (Create FastCGI application)
   ```

7. **설치 확인**
   ```powershell
   # 명령 프롬프트에서
   # In Command Prompt

   php -v
   # 출력 예: PHP 8.3.0 (cli)
   # Output example: PHP 8.3.0 (cli)

   php -m
   # 설치된 모듈 목록 표시
   # Shows list of installed modules
   ```

---

### 4. SQL Server 설치

**SQL Server란? (What is SQL Server?)**

SQL Server는 Microsoft의 관계형 데이터베이스 관리 시스템입니다.
- 엔터프라이즈급 데이터베이스
- T-SQL (Transact-SQL) 언어 사용
- MySQL, PostgreSQL과 유사한 역할

SQL Server is Microsoft's relational database management system.
- Enterprise-grade database
- Uses T-SQL (Transact-SQL) language
- Similar role to MySQL, PostgreSQL

#### 설치 단계 (Installation Steps)

1. **SQL Server Express 다운로드**
   ```
   https://www.microsoft.com/sql-server/sql-server-downloads

   "Download now" (Express 버전) 클릭
   ```

2. **설치 실행**
   ```
   1. 다운로드한 파일 실행
   2. "Basic" 설치 유형 선택
   3. 라이선스 동의
   4. 설치 경로 선택 (기본값 사용 권장)
   5. "Install" 클릭
   6. 설치 완료 대기 (5-10분)
   ```

3. **SQL Server Management Studio (SSMS) 설치 (선택사항이지만 권장)**
   ```
   https://docs.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms

   SSMS는 데이터베이스를 GUI로 관리하는 도구
   SSMS is a GUI tool for managing databases
   ```

4. **연결 문자열 확인**

   설치 완료 후 표시되는 연결 문자열을 메모하세요:
   Note the connection string shown after installation:

   ```
   Server=localhost\SQLEXPRESS;Database=master;Trusted_Connection=True;
   ```

5. **데이터베이스 생성 (자동)**

   ASP.NET Core 애플리케이션이 첫 실행 시 자동으로 데이터베이스를 생성합니다.
   The ASP.NET Core application automatically creates the database on first run.

---

## ⚙️ IIS 설정 (IIS Configuration)

### ASP.NET Core API를 IIS에서 실행 (선택사항)

**참고**: 개발 중에는 Visual Studio나 `dotnet run` 명령으로 직접 실행하는 것이 더 간단합니다.

**Note**: During development, running directly with Visual Studio or `dotnet run` command is simpler.

프로덕션 환경에서 IIS를 사용하려면:

1. **ASP.NET Core Hosting Bundle 설치**
   ```
   https://dotnet.microsoft.com/permalink/dotnetcore-current-windows-runtime-bundle-installer
   ```

2. **애플리케이션 게시 (Publish)**
   ```powershell
   cd C:\claude\asp_practice_1
   dotnet publish -c Release -o C:\inetpub\TodoApi
   ```

3. **IIS에서 사이트 생성**
   ```
   IIS Manager → Sites → Add Website
   - Site name: TodoApi
   - Physical path: C:\inetpub\TodoApi
   - Port: 5000
   ```

### PHP 프론트엔드를 IIS에서 실행

1. **프론트엔드 파일 복사**
   ```powershell
   # frontend 폴더를 IIS 경로로 복사
   # Copy frontend folder to IIS path

   xcopy /E /I C:\claude\asp_practice_1\frontend C:\inetpub\wwwroot\TodoFrontend
   ```

2. **IIS에서 사이트 생성**
   ```
   IIS Manager → Sites → Add Website

   설정:
   - Site name: TodoFrontend
   - Physical path: C:\inetpub\wwwroot\TodoFrontend
   - Port: 8080
   ```

3. **기본 문서 설정**
   ```
   IIS Manager → TodoFrontend 사이트 선택 → "Default Document"
   - index.php가 목록에 있는지 확인
   - 없으면 "Add..." → "index.php" 추가
   ```

4. **브라우저에서 확인**
   ```
   http://localhost:8080
   ```

---

## 🚀 애플리케이션 실행 (Running the Application)

### 방법 1: 개발 모드 (Development Mode) - 권장

#### 백엔드 실행 (Run Backend)

**PowerShell 또는 명령 프롬프트:**

```powershell
# 1. 프로젝트 디렉토리로 이동
# Navigate to project directory
cd C:\claude\asp_practice_1

# 2. 데이터베이스 마이그레이션 (최초 1회)
# Database migration (first time only)
dotnet ef database update

# 3. 애플리케이션 실행
# Run application
dotnet run

# 출력 예:
# info: Microsoft.Hosting.Lifetime[14]
#       Now listening on: http://localhost:5000
# info: Microsoft.Hosting.Lifetime[0]
#       Application started. Press Ctrl+C to shut down.
```

**백엔드가 실행 중인지 확인:**
```powershell
# 브라우저나 PowerShell에서
# In browser or PowerShell

Invoke-WebRequest -Uri http://localhost:5000/api/todos

# 또는 브라우저에서 http://localhost:5000/api/todos 방문
```

#### 프론트엔드 실행 (Run Frontend)

**방법 A: PHP 내장 서버 사용 (가장 간단)**

```powershell
# 새 PowerShell 창에서
# In a new PowerShell window

cd C:\claude\asp_practice_1\frontend

php -S localhost:8080

# 출력:
# [Mon Jan 01 12:00:00 2024] PHP 8.3.0 Development Server (http://localhost:8080) started
```

**방법 B: IIS 사용**

위의 "IIS 설정" 섹션을 참조하세요.
Refer to the "IIS Configuration" section above.

#### 애플리케이션 사용 (Use Application)

1. **브라우저에서 프론트엔드 열기**
   ```
   http://localhost:8080
   ```

2. **API 연결 상태 확인**
   - 페이지 상단에 "✅ API 연결됨" 표시되어야 함
   - Should see "✅ API Connected" at top of page

3. **할일 추가 테스트**
   - 입력 필드에 "테스트 할일" 입력
   - "추가" 버튼 클릭
   - 목록에 항목이 나타나는지 확인

---

## 💡 핵심 개념 설명 (Core Concepts)

### 1. REST API란? (What is REST API?)

**REST** (Representational State Transfer)는 웹 서비스 아키텍처 스타일입니다.

**REST** (Representational State Transfer) is a web service architecture style.

#### REST의 핵심 원칙 (Core Principles of REST)

1. **자원 (Resource)**
   - 모든 것을 자원으로 표현
   - URI로 자원을 식별
   - 예: `/api/todos/1` → ID가 1인 할일

2. **HTTP 메서드 (HTTP Methods)**

   | 메서드 | 용도 | 예시 | SQL 비교 |
   |-------|------|------|---------|
   | GET | 조회 (Read) | `GET /api/todos` | SELECT |
   | POST | 생성 (Create) | `POST /api/todos` | INSERT |
   | PUT | 수정 (Update) | `PUT /api/todos/1` | UPDATE |
   | DELETE | 삭제 (Delete) | `DELETE /api/todos/1` | DELETE |

3. **무상태성 (Stateless)**
   - 각 요청은 독립적
   - 서버는 클라이언트 상태를 저장하지 않음
   - 모든 필요한 정보를 요청에 포함

4. **표준 형식 (Standard Format)**
   - JSON 형식으로 데이터 교환
   - 예:
   ```json
   {
       "id": 1,
       "title": "할일",
       "isCompleted": false
   }
   ```

#### REST API 요청/응답 예시

**GET 요청 (모든 할일 조회):**
```http
GET http://localhost:5000/api/todos HTTP/1.1
Accept: application/json
```

**응답:**
```http
HTTP/1.1 200 OK
Content-Type: application/json

[
    {
        "id": 1,
        "title": "공부하기",
        "isCompleted": false
    },
    {
        "id": 2,
        "title": "운동하기",
        "isCompleted": true
    }
]
```

**POST 요청 (새 할일 생성):**
```http
POST http://localhost:5000/api/todos HTTP/1.1
Content-Type: application/json
Accept: application/json

{
    "title": "책 읽기"
}
```

**응답:**
```http
HTTP/1.1 201 Created
Content-Type: application/json
Location: http://localhost:5000/api/todos/3

{
    "id": 3,
    "title": "책 읽기",
    "isCompleted": false
}
```

### 2. ASP.NET Core MVC 패턴

**MVC** = Model-View-Controller

```
┌─────────────┐
│   클라이언트   │  (PHP Frontend)
│   (Client)   │
└──────┬───────┘
       │ HTTP Request
       ▼
┌──────────────────────────────────────┐
│          Controller                  │  ← 요청 라우팅, 흐름 제어
│     (TodoController.cs)              │    Request routing, flow control
│                                      │
│  [HttpGet]                           │
│  public async Task<ActionResult>     │
│  GetAll() { ... }                    │
└──────────────┬───────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│          Service                     │  ← 비즈니스 로직
│      (TodoService.cs)                │    Business logic
│                                      │
│  public async Task<List<TodoItem>>   │
│  GetAllAsync() { ... }               │
└──────────────┬───────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│          Model                       │  ← 데이터 구조
│       (TodoItem.cs)                  │    Data structure
│                                      │
│  public class TodoItem {             │
│      public int Id { get; set; }     │
│      public string Title { get;set;} │
│      public bool IsCompleted {...}   │
│  }                                   │
└──────────────┬───────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│      DbContext (EF Core)             │  ← 데이터 접근
│  (ApplicationDbContext.cs)           │    Data access
│                                      │
│  public DbSet<TodoItem> TodoItems    │
└──────────────┬───────────────────────┘
               │
               ▼
       [SQL Server Database]
```

### 3. CORS (Cross-Origin Resource Sharing)

**CORS란? (What is CORS?)**

CORS는 다른 도메인 간의 HTTP 요청을 제어하는 보안 메커니즘입니다.

CORS is a security mechanism that controls HTTP requests between different domains.

**왜 필요한가? (Why needed?)**

```
Frontend:  http://localhost:8080  (PHP)
Backend:   http://localhost:5000  (ASP.NET Core)

↑ 다른 포트 = 다른 Origin = CORS 정책 적용
  Different ports = Different origins = CORS policy applies
```

**브라우저의 동일 출처 정책 (Same-Origin Policy):**
- 보안상의 이유로 브라우저는 기본적으로 다른 출처로의 요청을 차단
- For security, browsers block requests to different origins by default

**CORS 설정 예시 (ASP.NET Core):**

```csharp
// Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:8080")  // 프론트엔드 주소
              .AllowAnyMethod()                       // 모든 HTTP 메서드 허용
              .AllowAnyHeader();                      // 모든 헤더 허용
    });
});

app.UseCors("AllowFrontend");  // CORS 미들웨어 사용
```

### 4. Entity Framework Core (ORM)

**ORM이란? (What is ORM?)**

ORM (Object-Relational Mapping)은 객체와 데이터베이스 테이블을 매핑하는 기술입니다.

ORM (Object-Relational Mapping) is technology that maps objects to database tables.

**장점 (Benefits):**
- SQL을 직접 작성하지 않아도 됨 (No need to write SQL directly)
- 타입 안정성 (Type safety)
- 데이터베이스 독립성 (Database independence)

**예시: C# 코드 vs SQL**

**C# (Entity Framework):**
```csharp
// 모든 할일 조회
var todos = await _context.TodoItems.ToListAsync();

// 특정 할일 조회
var todo = await _context.TodoItems.FindAsync(id);

// 새 할일 추가
_context.TodoItems.Add(newTodo);
await _context.SaveChangesAsync();

// 할일 삭제
_context.TodoItems.Remove(todo);
await _context.SaveChangesAsync();
```

**자동 생성되는 SQL:**
```sql
-- 모든 할일 조회
SELECT [Id], [Title], [IsCompleted] FROM [TodoItems];

-- 특정 할일 조회
SELECT [Id], [Title], [IsCompleted] FROM [TodoItems]
WHERE [Id] = @p0;

-- 새 할일 추가
INSERT INTO [TodoItems] ([Title], [IsCompleted])
VALUES (@p0, @p1);

-- 할일 삭제
DELETE FROM [TodoItems] WHERE [Id] = @p0;
```

### 5. 의존성 주입 (Dependency Injection)

**DI란? (What is DI?)**

의존성 주입은 객체의 의존성을 외부에서 주입하는 디자인 패턴입니다.

Dependency Injection is a design pattern where object dependencies are injected from outside.

**없을 때 (Without DI):**
```csharp
public class TodoController
{
    private TodoService _service;

    public TodoController()
    {
        // 직접 생성 = 강한 결합
        // Direct creation = Tight coupling
        _service = new TodoService();
    }
}
```

**있을 때 (With DI):**
```csharp
public class TodoController
{
    private readonly ITodoService _service;

    // 생성자에서 주입받음
    // Injected through constructor
    public TodoController(ITodoService service)
    {
        _service = service;
    }
}

// Program.cs에서 등록
// Register in Program.cs
builder.Services.AddScoped<ITodoService, TodoService>();
```

**장점 (Benefits):**
- 느슨한 결합 (Loose coupling)
- 테스트 용이 (Easy testing)
- 코드 재사용성 증가 (Increased code reusability)

### 6. JavaScript fetch() API

**fetch()란? (What is fetch()?)**

fetch()는 HTTP 요청을 보내는 모던 JavaScript API입니다.

fetch() is a modern JavaScript API for making HTTP requests.

**구조:**
```javascript
fetch(url, options)
    .then(response => response.json())
    .then(data => console.log(data))
    .catch(error => console.error(error));
```

**async/await 사용:**
```javascript
async function getData() {
    try {
        const response = await fetch(url);
        const data = await response.json();
        return data;
    } catch (error) {
        console.error(error);
    }
}
```

**예시: GET 요청**
```javascript
const response = await fetch('http://localhost:5000/api/todos', {
    method: 'GET',
    headers: {
        'Accept': 'application/json'
    }
});

if (response.ok) {
    const todos = await response.json();
    console.log(todos);
}
```

**예시: POST 요청**
```javascript
const response = await fetch('http://localhost:5000/api/todos', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify({
        title: '새로운 할일'
    })
});

if (response.ok) {
    const newTodo = await response.json();
    console.log(newTodo);
}
```

---

## 📖 API 문서 (API Documentation)

### 기본 정보 (Basic Information)

- **Base URL**: `http://localhost:5000/api/todos`
- **Content-Type**: `application/json`
- **인증 (Authentication)**: 없음 (None) - 학습 프로젝트용

### 엔드포인트 (Endpoints)

#### 1. 모든 할일 조회 (Get All To-Dos)

**요청 (Request):**
```http
GET /api/todos HTTP/1.1
Host: localhost:5000
Accept: application/json
```

**curl 예시:**
```bash
curl -X GET "http://localhost:5000/api/todos" -H "Accept: application/json"
```

**JavaScript fetch 예시:**
```javascript
const response = await fetch('http://localhost:5000/api/todos', {
    method: 'GET',
    headers: { 'Accept': 'application/json' }
});
const todos = await response.json();
```

**성공 응답 (Success Response):**
```http
HTTP/1.1 200 OK
Content-Type: application/json

[
    {
        "id": 1,
        "title": "공부하기",
        "isCompleted": false
    },
    {
        "id": 2,
        "title": "운동하기",
        "isCompleted": true
    }
]
```

---

#### 2. 특정 할일 조회 (Get To-Do By ID)

**요청 (Request):**
```http
GET /api/todos/1 HTTP/1.1
Host: localhost:5000
Accept: application/json
```

**curl 예시:**
```bash
curl -X GET "http://localhost:5000/api/todos/1" -H "Accept: application/json"
```

**JavaScript fetch 예시:**
```javascript
const id = 1;
const response = await fetch(`http://localhost:5000/api/todos/${id}`, {
    method: 'GET',
    headers: { 'Accept': 'application/json' }
});
const todo = await response.json();
```

**성공 응답 (Success Response):**
```http
HTTP/1.1 200 OK
Content-Type: application/json

{
    "id": 1,
    "title": "공부하기",
    "isCompleted": false
}
```

**실패 응답 (Error Response):**
```http
HTTP/1.1 404 Not Found
Content-Type: application/json

{
    "message": "Todo item not found"
}
```

---

#### 3. 새 할일 생성 (Create To-Do)

**요청 (Request):**
```http
POST /api/todos HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json

{
    "title": "책 읽기"
}
```

**curl 예시:**
```bash
curl -X POST "http://localhost:5000/api/todos" \
     -H "Content-Type: application/json" \
     -H "Accept: application/json" \
     -d '{"title":"책 읽기"}'
```

**JavaScript fetch 예시:**
```javascript
const response = await fetch('http://localhost:5000/api/todos', {
    method: 'POST',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify({
        title: '책 읽기'
    })
});
const newTodo = await response.json();
```

**요청 본문 (Request Body):**

| 필드 | 타입 | 필수 | 설명 |
|-----|------|------|------|
| title | string | ✅ | 할일 제목 (1-200자) |

**성공 응답 (Success Response):**
```http
HTTP/1.1 201 Created
Content-Type: application/json
Location: http://localhost:5000/api/todos/3

{
    "id": 3,
    "title": "책 읽기",
    "isCompleted": false
}
```

**검증 실패 (Validation Error):**
```http
HTTP/1.1 400 Bad Request
Content-Type: application/json

{
    "errors": {
        "Title": [
            "The Title field is required."
        ]
    }
}
```

---

#### 4. 할일 수정 (Update To-Do)

**요청 (Request):**
```http
PUT /api/todos/1 HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json

{
    "id": 1,
    "title": "공부하기 (수정됨)",
    "isCompleted": true
}
```

**curl 예시:**
```bash
curl -X PUT "http://localhost:5000/api/todos/1" \
     -H "Content-Type: application/json" \
     -H "Accept: application/json" \
     -d '{"id":1,"title":"공부하기 (수정됨)","isCompleted":true}'
```

**JavaScript fetch 예시:**
```javascript
const id = 1;
const response = await fetch(`http://localhost:5000/api/todos/${id}`, {
    method: 'PUT',
    headers: {
        'Content-Type': 'application/json',
        'Accept': 'application/json'
    },
    body: JSON.stringify({
        id: 1,
        title: '공부하기 (수정됨)',
        isCompleted: true
    })
});
```

**요청 본문 (Request Body):**

| 필드 | 타입 | 필수 | 설명 |
|-----|------|------|------|
| id | integer | ✅ | 할일 ID (URL의 ID와 일치해야 함) |
| title | string | ✅ | 할일 제목 |
| isCompleted | boolean | ✅ | 완료 상태 |

**성공 응답 (Success Response):**
```http
HTTP/1.1 204 No Content
```

**ID 불일치 에러:**
```http
HTTP/1.1 400 Bad Request
Content-Type: application/json

{
    "message": "ID mismatch"
}
```

**할일 없음:**
```http
HTTP/1.1 404 Not Found
```

---

#### 5. 할일 삭제 (Delete To-Do)

**요청 (Request):**
```http
DELETE /api/todos/1 HTTP/1.1
Host: localhost:5000
```

**curl 예시:**
```bash
curl -X DELETE "http://localhost:5000/api/todos/1"
```

**JavaScript fetch 예시:**
```javascript
const id = 1;
const response = await fetch(`http://localhost:5000/api/todos/${id}`, {
    method: 'DELETE'
});
```

**성공 응답 (Success Response):**
```http
HTTP/1.1 204 No Content
```

**할일 없음:**
```http
HTTP/1.1 404 Not Found
```

---

### HTTP 상태 코드 (HTTP Status Codes)

| 코드 | 의미 | 설명 |
|-----|------|------|
| **200 OK** | 성공 | 요청이 성공적으로 처리됨 |
| **201 Created** | 생성됨 | 새 리소스가 성공적으로 생성됨 |
| **204 No Content** | 내용 없음 | 성공했지만 반환할 내용 없음 |
| **400 Bad Request** | 잘못된 요청 | 요청 데이터가 유효하지 않음 |
| **404 Not Found** | 찾을 수 없음 | 요청한 리소스가 존재하지 않음 |
| **500 Internal Server Error** | 서버 에러 | 서버 내부 오류 발생 |

---

## 📁 프로젝트 구조 (Project Structure)

```
asp_practice_1/
│
├── Controllers/                    # API 컨트롤러 (API Controllers)
│   └── TodoController.cs          # 할일 REST API 엔드포인트
│                                   # To-Do REST API endpoints
│
├── Models/                         # 데이터 모델 (Data Models)
│   └── TodoItem.cs                # 할일 엔터티 클래스
│                                   # To-Do entity class
│
├── Services/                       # 비즈니스 로직 (Business Logic)
│   ├── ITodoService.cs            # 서비스 인터페이스
│   └── TodoService.cs             # 서비스 구현체
│
├── Data/                           # 데이터 접근 계층 (Data Access Layer)
│   └── ApplicationDbContext.cs    # EF Core DbContext
│
├── frontend/                       # PHP 프론트엔드 (PHP Frontend)
│   ├── index.php                  # 메인 HTML 페이지
│   │                               # - HTML 구조 정의
│   │                               # - 할일 목록 UI
│   │                               # - 입력 폼, 필터 버튼
│   │
│   ├── styles.css                 # 스타일시트
│   │                               # - CSS 변수로 일관된 디자인
│   │                               # - 반응형 레이아웃
│   │                               # - 애니메이션 효과
│   │
│   └── app.js                     # JavaScript 로직
│                                   # - fetch() API로 백엔드 통신
│                                   # - DOM 조작으로 동적 UI
│                                   # - CRUD 기능 구현
│
├── Program.cs                      # 애플리케이션 진입점
│                                   # - 서비스 등록 (DI 설정)
│                                   # - 미들웨어 파이프라인 구성
│                                   # - CORS 정책 설정
│
├── appsettings.json               # 애플리케이션 설정
│                                   # - 데이터베이스 연결 문자열
│                                   # - 로깅 레벨
│                                   # - 기타 설정
│
├── asp_practice_1.csproj          # 프로젝트 파일
│                                   # - NuGet 패키지 의존성
│                                   # - 빌드 설정
│
└── README.md                       # 이 문서!
                                    # This documentation!
```

### 파일별 역할 설명 (File Roles Explained)

#### Backend Files

**1. Program.cs**
```csharp
// 애플리케이션의 시작점
// Application entry point

var builder = WebApplication.CreateBuilder(args);

// 서비스 등록 (의존성 주입 설정)
// Service registration (DI configuration)
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<ITodoService, TodoService>();

// CORS 설정
builder.Services.AddCors(options => { ... });

var app = builder.Build();

// 미들웨어 파이프라인 구성
// Middleware pipeline configuration
app.UseCors("AllowFrontend");
app.UseRouting();
app.MapControllers();

app.Run();
```

**2. Controllers/TodoController.cs**
```csharp
// HTTP 요청을 받아 처리하는 컨트롤러
// Controller that receives and processes HTTP requests

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    // GET /api/todos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll() { ... }

    // POST /api/todos
    [HttpPost]
    public async Task<ActionResult<TodoItem>> Create([FromBody] TodoItem item) { ... }

    // ... 기타 메서드
}
```

**3. Services/TodoService.cs**
```csharp
// 비즈니스 로직을 처리하는 서비스
// Service that handles business logic

public class TodoService : ITodoService
{
    private readonly ApplicationDbContext _context;

    // 데이터 검증, 변환, 비즈니스 규칙 적용
    // Data validation, transformation, business rules

    public async Task<List<TodoItem>> GetAllAsync() { ... }
    public async Task CreateAsync(TodoItem item) { ... }
    // ...
}
```

**4. Data/ApplicationDbContext.cs**
```csharp
// Entity Framework Core의 데이터베이스 컨텍스트
// Entity Framework Core database context

public class ApplicationDbContext : DbContext
{
    // 데이터베이스 테이블을 나타내는 DbSet
    // DbSet representing database table
    public DbSet<TodoItem> TodoItems { get; set; }

    // 데이터베이스 설정
    // Database configuration
    protected override void OnModelCreating(ModelBuilder modelBuilder) { ... }
}
```

**5. Models/TodoItem.cs**
```csharp
// 할일 데이터 구조를 정의하는 모델
// Model defining to-do data structure

public class TodoItem
{
    public int Id { get; set; }                // 기본 키 (Primary key)
    public string Title { get; set; }          // 제목
    public bool IsCompleted { get; set; }      // 완료 상태
}
```

#### Frontend Files

**1. frontend/index.php**
- HTML 구조 정의
- 사용자 인터페이스 레이아웃
- 입력 폼, 버튼, 목록 영역
- 상태 표시, 메시지 영역

**2. frontend/styles.css**
- 모든 시각적 스타일
- 색상, 폰트, 간격, 크기
- 반응형 디자인 (@media queries)
- 애니메이션 효과 (@keyframes)

**3. frontend/app.js**
- 모든 동적 기능
- fetch() API로 백엔드 통신
- DOM 조작으로 UI 업데이트
- 이벤트 처리 (클릭, 입력 등)
- 에러 처리 및 사용자 피드백

---

## 🔧 문제 해결 (Troubleshooting)

### 일반적인 문제와 해결책 (Common Issues and Solutions)

#### 1. "❌ API 연결 실패" 메시지

**증상 (Symptom):**
- 프론트엔드 페이지에서 "API 연결 실패" 표시
- 할일 목록이 로드되지 않음

**원인 (Causes):**
- 백엔드 API가 실행되지 않음
- 포트 충돌
- 방화벽 차단

**해결책 (Solutions):**

1. **백엔드가 실행 중인지 확인**
   ```powershell
   # 백엔드 디렉토리에서
   cd C:\claude\asp_practice_1
   dotnet run

   # "Now listening on: http://localhost:5000" 메시지 확인
   ```

2. **포트가 사용 중인지 확인**
   ```powershell
   netstat -ano | findstr :5000

   # 다른 프로세스가 5000 포트를 사용 중이면:
   # - 해당 프로세스 종료
   # - 또는 appsettings.json에서 다른 포트로 변경
   ```

3. **브라우저에서 직접 API 테스트**
   ```
   http://localhost:5000/api/todos

   JSON 데이터가 보이면 백엔드는 정상
   ```

4. **방화벽 확인**
   ```
   Windows Defender 방화벽 → 허용되는 앱
   → "dotnet.exe"가 허용되어 있는지 확인
   ```

---

#### 2. CORS 에러

**증상 (Symptom):**
```
Access to fetch at 'http://localhost:5000/api/todos' from origin
'http://localhost:8080' has been blocked by CORS policy
```

**원인 (Cause):**
- CORS 정책이 올바르게 설정되지 않음
- 프론트엔드 URL이 허용 목록에 없음

**해결책 (Solution):**

1. **Program.cs 확인**
   ```csharp
   builder.Services.AddCors(options =>
   {
       options.AddPolicy("AllowFrontend", policy =>
       {
           policy.WithOrigins("http://localhost:8080")  // ← 프론트엔드 URL 확인
                 .AllowAnyMethod()
                 .AllowAnyHeader();
       });
   });

   app.UseCors("AllowFrontend");  // ← 이 줄이 있는지 확인
   ```

2. **프론트엔드 포트 확인**
   ```
   프론트엔드가 실행 중인 포트와 CORS 설정의 포트가 일치해야 함
   ```

3. **백엔드 재시작**
   ```powershell
   # Ctrl+C로 중지 후 다시 실행
   dotnet run
   ```

---

#### 3. 데이터베이스 연결 에러

**증상 (Symptom):**
```
SqlException: A network-related or instance-specific error occurred
while establishing a connection to SQL Server
```

**원인 (Causes):**
- SQL Server가 실행되지 않음
- 연결 문자열이 잘못됨
- 데이터베이스가 생성되지 않음

**해결책 (Solutions):**

1. **SQL Server 서비스 확인**
   ```
   서비스 (services.msc) 실행 → "SQL Server (SQLEXPRESS)" 확인
   상태가 "실행 중"이어야 함
   ```

2. **연결 문자열 확인 (appsettings.json)**
   ```json
   {
       "ConnectionStrings": {
           "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=TodoDb;Trusted_Connection=True;TrustServerCertificate=True;"
       }
   }
   ```

3. **데이터베이스 마이그레이션 실행**
   ```powershell
   cd C:\claude\asp_practice_1

   # 마이그레이션 생성 (최초 1회)
   dotnet ef migrations add InitialCreate

   # 데이터베이스 업데이트
   dotnet ef database update
   ```

4. **SSMS로 연결 테스트 (선택사항)**
   ```
   SQL Server Management Studio 실행
   서버 이름: localhost\SQLEXPRESS
   인증: Windows 인증
   연결 → 성공하면 SQL Server 정상
   ```

---

#### 4. PHP 페이지가 다운로드됨 (실행되지 않음)

**증상 (Symptom):**
- index.php 파일이 브라우저에서 실행되지 않고 다운로드됨
- PHP 코드가 그대로 보임

**원인 (Cause):**
- IIS에서 PHP가 올바르게 설정되지 않음
- FastCGI가 활성화되지 않음

**해결책 (Solution):**

1. **PHP 내장 서버 사용 (가장 간단)**
   ```powershell
   cd C:\claude\asp_practice_1\frontend
   php -S localhost:8080
   ```

2. **IIS에서 Handler Mapping 확인**
   ```
   IIS Manager → 사이트 선택 → Handler Mappings
   → "*.php" 매핑이 있는지 확인
   → 없으면 위의 "PHP 설치" 섹션 참조
   ```

---

#### 5. 할일 추가 시 아무 반응 없음

**증상 (Symptom):**
- "추가" 버튼 클릭해도 아무 일도 일어나지 않음
- 에러 메시지도 없음

**해결책 (Solution):**

1. **브라우저 개발자 도구 확인**
   ```
   F12 키 → Console 탭
   → 에러 메시지 확인

   Network 탭
   → API 요청이 전송되는지 확인
   → 응답 코드 확인 (200, 400, 500 등)
   ```

2. **JavaScript 에러 확인**
   ```javascript
   // app.js에서 에러가 발생하면 전체 스크립트가 멈출 수 있음
   // 콘솔에서 에러 메시지 확인
   ```

3. **API 직접 테스트**
   ```powershell
   # PowerShell에서
   Invoke-WebRequest -Uri http://localhost:5000/api/todos `
                      -Method POST `
                      -Headers @{"Content-Type"="application/json"} `
                      -Body '{"title":"테스트"}'

   # 성공하면 백엔드는 정상
   # 실패하면 백엔드 문제
   ```

---

#### 6. "dotnet: command not found" 에러

**증상 (Symptom):**
```powershell
dotnet : The term 'dotnet' is not recognized...
```

**원인 (Cause):**
- .NET SDK가 설치되지 않음
- 환경 변수에 경로가 추가되지 않음

**해결책 (Solution):**

1. **.NET SDK 설치 확인**
   ```
   제어판 → 프로그램 및 기능
   → "Microsoft .NET SDK" 검색
   ```

2. **재설치**
   ```
   https://dotnet.microsoft.com/download
   → .NET 8.0 SDK 다운로드 및 설치
   ```

3. **시스템 재시작**
   ```
   설치 후 컴퓨터 재시작
   → 환경 변수가 자동으로 업데이트됨
   ```

---

#### 7. IIS에서 500 Internal Server Error

**증상 (Symptom):**
- IIS에서 사이트 실행 시 500 에러
- 상세 에러 메시지 없음

**해결책 (Solution):**

1. **상세 에러 메시지 활성화**
   ```xml
   <!-- web.config 파일 (없으면 생성) -->
   <?xml version="1.0" encoding="utf-8"?>
   <configuration>
     <system.webServer>
       <httpErrors errorMode="Detailed" />
       <asp scriptErrorSentToBrowser="true"/>
     </system.webServer>
     <system.web>
       <customErrors mode="Off"/>
     </system.web>
   </configuration>
   ```

2. **IIS 로그 확인**
   ```
   C:\inetpub\logs\LogFiles\
   → 최신 로그 파일 열기
   → 에러 내용 확인
   ```

3. **개발 모드로 전환**
   ```
   dotnet run 명령으로 직접 실행하여 에러 확인
   ```

---

## 📚 학습 자료 (Learning Resources)

### 공식 문서 (Official Documentation)

#### ASP.NET Core
- **공식 문서**: https://docs.microsoft.com/aspnet/core
- **튜토리얼**: https://docs.microsoft.com/aspnet/core/tutorials
- **REST API 가이드**: https://docs.microsoft.com/aspnet/core/web-api

#### Entity Framework Core
- **공식 문서**: https://docs.microsoft.com/ef/core
- **시작하기**: https://docs.microsoft.com/ef/core/get-started

#### C# 언어
- **C# 가이드**: https://docs.microsoft.com/dotnet/csharp
- **C# 튜토리얼**: https://docs.microsoft.com/dotnet/csharp/tour-of-csharp

#### PHP
- **공식 문서**: https://www.php.net/manual/en/
- **PHP 튜토리얼**: https://www.w3schools.com/php/

#### JavaScript
- **MDN Web Docs**: https://developer.mozilla.org/en-US/docs/Web/JavaScript
- **fetch API**: https://developer.mozilla.org/en-US/docs/Web/API/Fetch_API

### 학습 경로 (Learning Path)

#### 초급 (Beginner)
1. **HTTP 기초**
   - HTTP 메서드 (GET, POST, PUT, DELETE)
   - 상태 코드 (200, 404, 500 등)
   - 헤더와 바디

2. **REST API 개념**
   - REST 원칙
   - 자원과 URI
   - JSON 형식

3. **C# 기초**
   - 변수와 데이터 타입
   - 클래스와 객체
   - 비동기 프로그래밍 (async/await)

4. **JavaScript 기초**
   - 변수와 함수
   - DOM 조작
   - fetch API

#### 중급 (Intermediate)
1. **ASP.NET Core MVC 패턴**
   - Controller
   - Service (비즈니스 로직)
   - Model (데이터 모델)

2. **Entity Framework Core**
   - DbContext
   - 마이그레이션
   - LINQ 쿼리

3. **의존성 주입 (DI)**
   - 서비스 등록
   - 생명주기 (Scoped, Transient, Singleton)

4. **프론트엔드-백엔드 통신**
   - CORS
   - JSON 직렬화/역직렬화
   - 에러 처리

#### 고급 (Advanced)
1. **인증과 권한 부여**
   - JWT (JSON Web Tokens)
   - Identity Framework

2. **데이터베이스 최적화**
   - 인덱싱
   - 쿼리 최적화
   - 캐싱

3. **배포와 호스팅**
   - IIS 프로덕션 설정
   - Azure App Service
   - Docker 컨테이너

4. **테스팅**
   - 단위 테스트 (xUnit)
   - 통합 테스트
   - API 테스팅 (Postman)

### 추천 학습 순서 (Recommended Study Order)

```
1주차: HTTP와 REST API 기본 개념 이해
       ↓
2주차: C# 기초 문법 학습
       ↓
3주차: ASP.NET Core MVC 패턴 이해
       ↓
4주차: Entity Framework Core 데이터 접근
       ↓
5주차: JavaScript와 fetch API
       ↓
6주차: 프론트엔드-백엔드 통합
       ↓
7주차: 에러 처리와 검증
       ↓
8주차: 배포와 프로덕션 설정
```

### 연습 과제 (Practice Projects)

#### 레벨 1: 기능 추가
1. **우선순위 기능**
   - TodoItem에 Priority (Low, Medium, High) 필드 추가
   - 우선순위별 필터링

2. **마감일 기능**
   - DueDate 필드 추가
   - 날짜별 정렬

3. **검색 기능**
   - 제목으로 할일 검색
   - API에 검색 엔드포인트 추가

#### 레벨 2: UI 개선
1. **드래그 앤 드롭**
   - 할일 순서 변경
   - 우선순위 변경

2. **카테고리**
   - 할일을 카테고리로 분류
   - 카테고리별 보기

3. **테마 변경**
   - 다크 모드/라이트 모드
   - 색상 테마 커스터마이징

#### 레벨 3: 고급 기능
1. **사용자 인증**
   - 회원가입/로그인
   - 사용자별 할일 분리

2. **실시간 업데이트**
   - SignalR을 사용한 실시간 동기화
   - 여러 브라우저에서 동시 작업

3. **파일 첨부**
   - 할일에 파일 첨부 기능
   - 이미지 미리보기

---

## 💼 프로덕션 배포 체크리스트

### 보안 (Security)
- [ ] HTTPS 활성화 (SSL 인증서 설정)
- [ ] CORS 정책을 프로덕션 URL로 제한
- [ ] SQL Injection 방지 (Entity Framework 사용)
- [ ] XSS 방지 (입력 검증 및 이스케이프)
- [ ] 인증 및 권한 부여 구현
- [ ] API Rate Limiting 설정
- [ ] 민감한 정보 환경 변수로 관리

### 성능 (Performance)
- [ ] 응답 압축 활성화
- [ ] 정적 파일 캐싱 설정
- [ ] 데이터베이스 쿼리 최적화
- [ ] 로깅 레벨 조정 (Information 이상)
- [ ] 연결 풀링 설정

### 모니터링 (Monitoring)
- [ ] 에러 로깅 구성
- [ ] 성능 모니터링 도구 설정
- [ ] 헬스 체크 엔드포인트 추가
- [ ] 알림 시스템 구성

### 백업 (Backup)
- [ ] 데이터베이스 자동 백업 설정
- [ ] 백업 복원 테스트
- [ ] 재해 복구 계획 수립

---

## 🤝 기여 및 피드백 (Contributing and Feedback)

이 프로젝트는 학습 목적으로 만들어졌습니다. 개선 사항이나 질문이 있으시면 편하게 연락 주세요!

This project was created for learning purposes. Feel free to reach out with improvements or questions!

---

## 📝 라이선스 (License)

이 프로젝트는 교육 목적으로 자유롭게 사용할 수 있습니다.

This project is free to use for educational purposes.

---

## 🎓 결론 (Conclusion)

이 프로젝트를 통해 다음을 배울 수 있습니다:

Through this project, you can learn:

✅ **REST API 아키텍처**: 프론트엔드와 백엔드 분리
✅ **ASP.NET Core**: C#으로 웹 API 개발
✅ **Entity Framework Core**: ORM을 통한 데이터베이스 접근
✅ **의존성 주입**: 느슨한 결합과 테스트 용이성
✅ **CORS**: 크로스 오리진 리소스 공유
✅ **PHP 프론트엔드**: 서버 사이드 렌더링
✅ **JavaScript fetch API**: 비동기 HTTP 통신
✅ **DOM 조작**: 동적 웹 페이지 생성

**다음 단계 (Next Steps):**
1. 이 프로젝트를 완전히 이해할 때까지 실행하고 수정해보세요
2. 새로운 기능을 추가해보세요 (위의 연습 과제 참고)
3. 다른 프레임워크 (React, Vue.js)로 프론트엔드를 재작성해보세요
4. 인증 시스템을 추가해보세요
5. 클라우드에 배포해보세요 (Azure, AWS)

**Happy Coding! 즐거운 코딩 되세요! 🚀**
