# 빠른 시작 가이드 (Quick Start Guide)

> **5분 안에 실행하기**
>
> **Get Running in 5 Minutes**

---

## ⚡ 전제조건 확인 (Prerequisites Check)

아래 항목들이 설치되어 있어야 합니다:

The following must be installed:

- ✅ .NET SDK 8.0 이상
- ✅ PHP 8.0 이상
- ✅ SQL Server (또는 SQL Server Express)

### 설치 확인 명령어 (Verification Commands)

```powershell
# .NET SDK 확인
dotnet --version
# 출력 예: 8.0.100

# PHP 확인
php -v
# 출력 예: PHP 8.3.0

# SQL Server 확인 (서비스)
# services.msc 실행 → "SQL Server (SQLEXPRESS)" 확인
```

❌ 설치되지 않았다면 → **README.md의 "설치 가이드" 섹션 참조**

---

## 🚀 3단계 실행 (3-Step Launch)

### 1단계: 데이터베이스 생성 (Create Database)

```powershell
# 프로젝트 디렉토리로 이동
cd C:\claude\asp_practice_1

# 데이터베이스 마이그레이션 실행 (최초 1회만)
dotnet ef migrations add InitialCreate
dotnet ef database update
```

**성공 메시지:**
```
Done. To undo this action, use 'ef migrations remove'
```

---

### 2단계: 백엔드 API 실행 (Start Backend API)

```powershell
# 같은 디렉토리에서
dotnet run
```

**출력 예시:**
```
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5000
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
```

✅ **백엔드 실행 확인:**

브라우저에서 http://localhost:5000/api/todos 접속

→ `[]` (빈 배열) 또는 JSON 데이터가 보이면 성공!

⚠️ **이 창을 닫지 마세요! 백엔드가 계속 실행되어야 합니다.**

---

### 3단계: 프론트엔드 실행 (Start Frontend)

**새로운 PowerShell 창을 열고:**

```powershell
# frontend 디렉토리로 이동
cd C:\claude\asp_practice_1\frontend

# PHP 내장 서버 실행
php -S localhost:8080
```

**출력 예시:**
```
[Mon Jan 01 12:00:00 2024] PHP 8.3.0 Development Server (http://localhost:8080) started
```

---

## 🌐 애플리케이션 사용 (Use Application)

### 브라우저에서 열기

```
http://localhost:8080
```

### 기대되는 화면

```
┌────────────────────────────────────────┐
│   📝 할일 목록                          │
│   ASP.NET Core API + PHP Frontend      │
├────────────────────────────────────────┤
│  ✅ API 연결됨 (API Connected)         │
├────────────────────────────────────────┤
│  [새로운 할일을 입력하세요...] [➕ 추가]│
├────────────────────────────────────────┤
│  [전체 0] [진행중 0] [완료됨 0]         │
├────────────────────────────────────────┤
│                                        │
│         📝                             │
│    할일이 없습니다                      │
│  위에서 새로운 할일을 추가해보세요!     │
│                                        │
└────────────────────────────────────────┘
```

---

## ✨ 첫 할일 추가하기 (Add Your First To-Do)

1. **입력 필드에 텍스트 입력**
   ```
   예: "ASP.NET Core 학습하기"
   ```

2. **"추가" 버튼 클릭 또는 Enter 키**

3. **결과 확인**
   ```
   ✅ 할일이 추가되었습니다!

   [✓] ASP.NET Core 학습하기  [✏️] [🗑️]
   ```

4. **추가 작업 테스트**
   - ✓ 체크박스 클릭 → 완료 처리
   - ✏️ 수정 버튼 → 제목 변경
   - 🗑️ 삭제 버튼 → 항목 삭제

---

## 🔧 문제 발생 시 (If Something Goes Wrong)

### 문제 1: "API 연결 실패" 메시지

**원인:** 백엔드가 실행되지 않음

**해결:**
```powershell
# 백엔드 디렉토리에서
cd C:\claude\asp_practice_1
dotnet run
```

---

### 문제 2: "dotnet: command not found"

**원인:** .NET SDK가 설치되지 않았거나 경로 설정 안됨

**해결:**
1. https://dotnet.microsoft.com/download
2. .NET 8.0 SDK 다운로드 및 설치
3. 컴퓨터 재시작

---

### 문제 3: "php: command not found"

**원인:** PHP가 설치되지 않았거나 환경 변수 미설정

**해결:**
1. https://windows.php.net/download/
2. PHP 8.x Thread Safe 다운로드
3. C:\PHP에 압축 해제
4. 환경 변수 Path에 C:\PHP 추가
5. 컴퓨터 재시작

---

### 문제 4: 데이터베이스 연결 에러

**에러 메시지:**
```
SqlException: A network-related error occurred...
```

**해결:**
1. SQL Server 서비스 확인
   ```
   services.msc → "SQL Server (SQLEXPRESS)" 시작
   ```

2. 연결 문자열 확인 (appsettings.json)
   ```json
   "ConnectionStrings": {
       "DefaultConnection": "Server=localhost\\SQLEXPRESS;..."
   }
   ```

---

### 문제 5: 포트가 이미 사용 중

**에러 메시지:**
```
Error: Failed to bind to address http://localhost:5000
```

**해결:**
```powershell
# 5000 포트 사용 중인 프로세스 확인
netstat -ano | findstr :5000

# 프로세스 종료 (PID 확인 후)
taskkill /PID [프로세스번호] /F

# 또는 다른 포트 사용 (appsettings.json 수정)
"Urls": "http://localhost:5001"
```

---

## 📂 프로젝트 구조 한눈에 보기

```
asp_practice_1/
│
├── 🎯 Program.cs              ← 백엔드 시작점
├── 🎯 appsettings.json        ← 설정 파일
│
├── Controllers/
│   └── TodoController.cs      ← REST API 엔드포인트
│
├── Models/
│   └── TodoItem.cs            ← 데이터 모델
│
├── Services/
│   ├── ITodoService.cs        ← 서비스 인터페이스
│   └── TodoService.cs         ← 비즈니스 로직
│
├── Data/
│   └── ApplicationDbContext.cs ← DB 컨텍스트
│
└── frontend/                  ← 프론트엔드
    ├── 🌐 index.php          ← HTML 페이지
    ├── 🎨 styles.css         ← 스타일
    └── ⚡ app.js            ← JavaScript 로직
```

---

## 🎓 다음 단계 (Next Steps)

### 학습 경로

1. **코드 이해하기**
   - 📖 **README.md** → 전체 아키텍처 이해
   - 📖 **API_DOCUMENTATION.md** → API 사용법 상세 학습

2. **코드 수정해보기**
   - frontend/app.js의 주석을 읽으며 JavaScript 이해
   - Controllers/TodoController.cs의 REST API 구현 분석
   - frontend/styles.css에서 색상이나 스타일 변경

3. **기능 추가하기**
   - 우선순위 기능 추가 (High, Medium, Low)
   - 마감일 기능 추가
   - 검색 기능 구현

4. **고급 주제 학습**
   - 인증 시스템 (JWT)
   - 테스팅 (xUnit)
   - 배포 (Azure, IIS)

---

## 📚 주요 문서 링크

| 문서 | 내용 | 읽는 순서 |
|-----|------|----------|
| **QUICKSTART.md** | 빠른 시작 (이 문서) | 1️⃣ 먼저 |
| **README.md** | 전체 가이드 (설치, 아키텍처, 문제해결) | 2️⃣ 다음 |
| **API_DOCUMENTATION.md** | API 상세 문서 (엔드포인트, 예제) | 3️⃣ 마지막 |

---

## 💻 개발자 도구 활용 (Developer Tools)

### 브라우저 개발자 도구 (F12)

**Network 탭:**
```
1. F12 키 눌러서 개발자 도구 열기
2. Network 탭 선택
3. 할일 추가/수정/삭제 시도
4. HTTP 요청/응답 확인 가능

볼 수 있는 것:
- 요청 URL
- HTTP 메서드 (GET, POST, PUT, DELETE)
- 요청 헤더 및 본문
- 응답 상태 코드 (200, 201, 404 등)
- 응답 데이터 (JSON)
```

**Console 탭:**
```
JavaScript 로그 및 에러 확인
- console.log() 출력 보기
- JavaScript 에러 디버깅
- 직접 JavaScript 코드 실행 가능
```

---

## 🎯 핵심 개념 요약

### 백엔드 (ASP.NET Core)

```
HTTP 요청 받기
    ↓
Controller (라우팅)
    ↓
Service (비즈니스 로직)
    ↓
Entity Framework (데이터베이스 접근)
    ↓
SQL Server (데이터 저장)
```

### 프론트엔드 (PHP + JavaScript)

```
사용자 입력
    ↓
JavaScript 이벤트 처리
    ↓
fetch() API로 HTTP 요청
    ↓
JSON 응답 받기
    ↓
DOM 조작으로 화면 업데이트
```

### 통신 흐름

```
Frontend (localhost:8080)
    ↓ HTTP Request (JSON)
Backend API (localhost:5000)
    ↓ SQL Query
Database (SQL Server)
    ↑ Data
Backend API
    ↑ HTTP Response (JSON)
Frontend
```

---

## ✅ 체크리스트 (Checklist)

시작하기 전에 확인하세요:

- [ ] .NET SDK 설치됨
- [ ] PHP 설치됨
- [ ] SQL Server 실행 중
- [ ] 두 개의 터미널/PowerShell 창 준비
- [ ] 브라우저 준비 (Chrome/Edge 권장)

실행 후 확인사항:

- [ ] 백엔드 실행 중 (http://localhost:5000/api/todos 접속 가능)
- [ ] 프론트엔드 실행 중 (http://localhost:8080 접속 가능)
- [ ] "API 연결됨" 메시지 표시
- [ ] 할일 추가 가능
- [ ] 할일 수정/삭제 가능

---

## 🆘 도움이 필요하신가요?

### 자주 묻는 질문 (FAQ)

**Q: 백엔드를 중지하려면?**
```
A: 백엔드 터미널에서 Ctrl+C
```

**Q: 데이터베이스를 초기화하려면?**
```powershell
A: dotnet ef database drop
   dotnet ef database update
```

**Q: 코드를 수정했는데 반영이 안 됩니다.**
```
A: 백엔드는 Ctrl+C로 중지 후 다시 dotnet run
   프론트엔드는 브라우저 새로고침 (Ctrl+F5)
```

**Q: IIS로 실행하고 싶습니다.**
```
A: README.md의 "IIS 설정" 섹션 참조
```

### 더 많은 도움말

📖 **상세 문서**: README.md
📖 **API 가이드**: API_DOCUMENTATION.md
🔧 **문제 해결**: README.md의 "문제 해결" 섹션

---

**축하합니다! 이제 첫 번째 Full-Stack 애플리케이션이 실행 중입니다! 🎉**

**Congratulations! Your first Full-Stack application is now running! 🎉**

---

## 📸 예상 스크린샷

### 성공적인 실행 화면

```
┌─────────────────────────────────────────────────────┐
│ 백엔드 터미널 (Backend Terminal)                     │
├─────────────────────────────────────────────────────┤
│ PS C:\claude\asp_practice_1> dotnet run             │
│ Building...                                         │
│ info: Microsoft.Hosting.Lifetime[14]                │
│       Now listening on: http://localhost:5000       │
│ info: Microsoft.Hosting.Lifetime[0]                 │
│       Application started.                          │
└─────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────┐
│ 프론트엔드 터미널 (Frontend Terminal)                 │
├─────────────────────────────────────────────────────┤
│ PS C:\claude\asp_practice_1\frontend> php -S        │
│    localhost:8080                                   │
│ [Mon Jan 01 12:00:00 2024] PHP 8.3.0 Development   │
│ Server (http://localhost:8080) started             │
└─────────────────────────────────────────────────────┘

┌─────────────────────────────────────────────────────┐
│ 브라우저 (Browser) - http://localhost:8080          │
├─────────────────────────────────────────────────────┤
│                                                     │
│        📝 할일 목록                                  │
│        ASP.NET Core API + PHP Frontend             │
│                                                     │
│  ✅ API 연결됨 (API Connected)                      │
│                                                     │
│  [새로운 할일을 입력하세요...]  [➕ 추가]            │
│                                                     │
│  [전체 2] [진행중 1] [완료됨 1]                      │
│                                                     │
│  [ ] ASP.NET Core 학습하기    [✏️] [🗑️]           │
│  [✓] REST API 이해하기        [✏️] [🗑️]           │
│                                                     │
└─────────────────────────────────────────────────────┘
```

Happy Coding! 🚀
