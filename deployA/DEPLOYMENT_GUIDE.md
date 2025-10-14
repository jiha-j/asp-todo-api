# 🚀 배포 가이드 (Deployment Guide)

면접에서 프로젝트를 시연하기 위한 웹 서버 배포 방법들을 소개합니다.

---

## 📋 목차 (Table of Contents)

1. [방법 비교표](#방법-비교표)
2. [방법 1: ngrok (가장 빠름, 권장)](#방법-1-ngrok-가장-빠름-권장)
3. [방법 2: Railway.app (무료 클라우드)](#방법-2-railwayapp-무료-클라우드)
4. [방법 3: Azure App Service (Microsoft 공식)](#방법-3-azure-app-service-microsoft-공식)
5. [방법 4: Render.com (무료 클라우드)](#방법-4-rendercom-무료-클라우드)
6. [면접 시연 팁](#면접-시연-팁)

---

## 방법 비교표

| 방법 | 설정 시간 | 비용 | 안정성 | 난이도 | 추천도 |
|------|----------|------|--------|--------|--------|
| **ngrok** | 5분 | 무료 | ⭐⭐⭐ | ⭐ | ⭐⭐⭐⭐⭐ |
| **Railway.app** | 15분 | 무료 | ⭐⭐⭐⭐ | ⭐⭐ | ⭐⭐⭐⭐ |
| **Azure** | 20분 | 무료/유료 | ⭐⭐⭐⭐⭐ | ⭐⭐⭐ | ⭐⭐⭐⭐ |
| **Render.com** | 15분 | 무료 | ⭐⭐⭐⭐ | ⭐⭐ | ⭐⭐⭐ |

### 🎯 추천 선택 기준:

**면접이 며칠 안 남았다면?**
→ **ngrok** (5분 안에 완료, 가장 쉬움)

**제대로 된 배포 경험을 보여주고 싶다면?**
→ **Railway.app** 또는 **Azure** (실제 클라우드 배포)

**Microsoft 기술 스택 회사 면접이라면?**
→ **Azure App Service** (ASP.NET Core와 최상의 호환성)

---

## 방법 1: ngrok (가장 빠름, 권장)

### 🌟 장점:
- ✅ **5분 안에 완료** - 가장 빠른 방법
- ✅ **무료** - 크레딧카드 불필요
- ✅ **로컬에서 실행** - 복잡한 배포 과정 없음
- ✅ **즉시 URL 생성** - `https://xxxx.ngrok.io` 형식
- ✅ **HTTPS 자동 지원**

### ⚠️ 단점:
- URL이 매번 변경됨 (유료 플랜은 고정 가능)
- 본인 컴퓨터가 켜져 있어야 함
- 8시간 세션 제한 (무료 플랜)

### 📝 설치 및 실행 방법:

#### 1단계: ngrok 설치

**방법 A: Chocolatey 사용 (권장)**
```powershell
# PowerShell을 관리자 권한으로 실행
choco install ngrok
```

**방법 B: 수동 설치**
1. https://ngrok.com/download 방문
2. Windows 버전 다운로드
3. `ngrok.exe`를 `C:\ngrok\` 폴더에 저장
4. 환경 변수 PATH에 추가

#### 2단계: ngrok 회원가입 및 인증 (무료)

1. https://dashboard.ngrok.com/signup 방문
2. 무료 계정 생성 (GitHub/Google 로그인 가능)
3. 대시보드에서 authtoken 복사
4. 터미널에서 인증:

```powershell
ngrok config add-authtoken YOUR_AUTH_TOKEN
```

#### 3단계: 백엔드 실행

```powershell
cd C:\claude\asp_practice_1
dotnet run
# → http://localhost:5000 에서 실행됨
```

#### 4단계: 백엔드를 ngrok으로 노출

**새 터미널 창을 열고:**
```powershell
ngrok http 5000
```

**출력 예시:**
```
Session Status                online
Account                       your-email@example.com
Forwarding                    https://1a2b-3c4d-5e6f.ngrok.io -> http://localhost:5000

Connections                   ttl     opn     rt1
                              0       0       0.00
```

**중요:** `https://1a2b-3c4d-5e6f.ngrok.io` ← 이 URL을 복사하세요!

#### 5단계: 프론트엔드 설정 수정

`frontend/app.js` 파일에서 API URL 변경:

```javascript
// 원래 코드 (로컬)
const API_BASE_URL = 'http://localhost:5000/api/todo';

// ngrok URL로 변경 (YOUR_NGROK_URL 부분을 실제 URL로 교체)
const API_BASE_URL = 'https://1a2b-3c4d-5e6f.ngrok.io/api/todo';
```

#### 6단계: 프론트엔드 실행

```powershell
cd C:\claude\asp_practice_1\frontend
php -S localhost:8080
```

#### 7단계: 프론트엔드도 ngrok으로 노출

**또 다른 터미널 창을 열고:**
```powershell
ngrok http 8080
```

**출력 예시:**
```
Forwarding                    https://9x8y-7w6v-5u4t.ngrok.io -> http://localhost:8080
```

### 🎉 완료!

이제 면접관에게 이 URL을 공유하면 됩니다:
```
https://9x8y-7w6v-5u4t.ngrok.io
```

### 📱 면접 당일 체크리스트:

```
[ ] 백엔드 실행 (dotnet run)
[ ] 백엔드 ngrok 실행 (ngrok http 5000)
[ ] 프론트엔드 app.js에 ngrok URL 수정
[ ] 프론트엔드 실행 (php -S localhost:8080)
[ ] 프론트엔드 ngrok 실행 (ngrok http 8080)
[ ] 브라우저에서 테스트
[ ] URL 복사해두기
```

### 🔧 ngrok 유용한 팁:

**1. 여러 포트를 하나의 URL로 (ngrok config)**
```yaml
# ngrok.yml 파일 생성
tunnels:
  backend:
    addr: 5000
    proto: http
  frontend:
    addr: 8080
    proto: http
```

```powershell
ngrok start --all
```

**2. 트래픽 모니터링**
- ngrok 실행 중에 `http://127.0.0.1:4040` 방문
- 실시간으로 HTTP 요청/응답 확인 가능
- 면접에서 API 호출을 보여줄 때 유용!

---

## 방법 2: Railway.app (무료 클라우드)

### 🌟 장점:
- ✅ **무료** - 월 $5 크레딧 제공 (크레딧카드 불필요)
- ✅ **실제 클라우드 배포 경험**
- ✅ **GitHub 연동** - Git push만 하면 자동 배포
- ✅ **고정 URL** - 변경되지 않음
- ✅ **PostgreSQL 무료 제공**

### 📝 배포 방법:

#### 1단계: Railway 계정 생성
1. https://railway.app 방문
2. GitHub 계정으로 로그인

#### 2단계: GitHub 레포지토리 생성

```powershell
cd C:\claude\asp_practice_1

# Git 초기화
git init

# .gitignore 이미 있음 (생성되어 있음)

# 파일 추가
git add .
git commit -m "Initial commit: ASP.NET Core Todo API"

# GitHub에 레포지토리 생성 후
git remote add origin https://github.com/YOUR_USERNAME/asp-todo-api.git
git branch -M main
git push -u origin main
```

#### 3단계: Railway에서 프로젝트 생성

1. Railway 대시보드에서 **"New Project"** 클릭
2. **"Deploy from GitHub repo"** 선택
3. 방금 생성한 레포지토리 선택
4. **"Deploy Now"** 클릭

#### 4단계: 환경 변수 설정

Railway 대시보드에서:
1. 프로젝트 클릭 → **"Variables"** 탭
2. 다음 변수 추가:

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://0.0.0.0:$PORT
```

#### 5단계: Dockerfile 생성 (Railway 자동 감지용)

`C:\claude\asp_practice_1\Dockerfile` 생성:

```dockerfile
# ASP.NET Core Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore "TodoApi.csproj"
COPY . .
RUN dotnet build "TodoApi.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "TodoApi.csproj" -c Release -o /app/publish

# Final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
```

#### 6단계: 프론트엔드 배포 (별도 프로젝트)

**옵션 A: Railway에 별도로 배포**
1. `frontend` 폴더를 별도 Git 레포지토리로 생성
2. Railway에서 새 프로젝트 생성
3. PHP 빌드팩이 자동으로 감지됨

**옵션 B: Vercel에 배포 (더 간단)**
1. https://vercel.com 방문
2. GitHub 레포지토리 연결
3. `frontend` 폴더 선택
4. 배포

#### 7단계: CORS 설정 업데이트

`Program.cs`에서 Railway URL 추가:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:8080",
            "https://your-railway-frontend-url.up.railway.app" // Railway URL 추가
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
```

### 🎉 완료!

Railway가 자동으로 URL 생성:
```
Backend: https://asp-todo-api-production.up.railway.app
Frontend: https://your-frontend.up.railway.app
```

---

## 방법 3: Azure App Service (Microsoft 공식)

### 🌟 장점:
- ✅ **Microsoft 공식** - ASP.NET Core와 최고의 호환성
- ✅ **무료 티어 제공** (F1 플랜)
- ✅ **프로덕션 수준** - 엔터프라이즈급 안정성
- ✅ **CI/CD 지원** - GitHub Actions 통합
- ✅ **Azure SQL Database** - 실제 데이터베이스 사용 가능

### 📝 배포 방법:

#### 1단계: Azure CLI 설치

```powershell
# Chocolatey 사용
choco install azure-cli

# 또는 MSI 설치: https://aka.ms/installazurecliwindows
```

#### 2단계: Azure 로그인

```powershell
az login
```

#### 3단계: 리소스 그룹 생성

```powershell
az group create --name TodoAppRG --location koreacentral
```

#### 4단계: App Service 플랜 생성 (무료 티어)

```powershell
az appservice plan create `
  --name TodoAppPlan `
  --resource-group TodoAppRG `
  --sku F1 `
  --is-linux
```

#### 5단계: Web App 생성

```powershell
az webapp create `
  --name my-todo-api `
  --resource-group TodoAppRG `
  --plan TodoAppPlan `
  --runtime "DOTNET|8.0"
```

#### 6단계: 배포

**방법 A: Visual Studio에서 직접 배포**
1. Visual Studio에서 프로젝트 열기
2. 우클릭 → **"Publish"**
3. **"Azure"** → **"Azure App Service (Windows/Linux)"**
4. 로그인 후 방금 생성한 Web App 선택
5. **"Publish"** 클릭

**방법 B: CLI로 배포**
```powershell
# 프로젝트 빌드
dotnet publish -c Release -o ./publish

# ZIP 파일 생성
Compress-Archive -Path ./publish/* -DestinationPath ./app.zip -Force

# Azure에 배포
az webapp deployment source config-zip `
  --resource-group TodoAppRG `
  --name my-todo-api `
  --src ./app.zip
```

#### 7단계: CORS 설정

```powershell
az webapp cors add `
  --resource-group TodoAppRG `
  --name my-todo-api `
  --allowed-origins '*'
```

#### 8단계: 프론트엔드 배포

**옵션 A: Azure Static Web Apps (무료)**
```powershell
az staticwebapp create `
  --name my-todo-frontend `
  --resource-group TodoAppRG `
  --source https://github.com/YOUR_USERNAME/todo-frontend `
  --location "East Asia" `
  --branch main `
  --app-location "/frontend" `
  --login-with-github
```

**옵션 B: Vercel (더 간단)**
1. https://vercel.com
2. GitHub 레포지토리 연결
3. 자동 배포

### 🎉 완료!

Azure가 URL 생성:
```
Backend: https://my-todo-api.azurewebsites.net
Frontend: https://my-todo-frontend.azurestaticapps.net
```

---

## 방법 4: Render.com (무료 클라우드)

### 🌟 장점:
- ✅ **완전 무료** - 크레딧카드 불필요
- ✅ **간단한 설정** - 클릭 몇 번으로 배포
- ✅ **PostgreSQL 무료** - 90일간 무료 DB
- ✅ **고정 URL**

### ⚠️ 단점:
- 무료 플랜은 15분 비활성 후 sleep 모드 (첫 요청이 느릴 수 있음)

### 📝 배포 방법:

#### 1단계: Render 계정 생성
1. https://render.com 방문
2. GitHub 계정으로 로그인

#### 2단계: 백엔드 배포

1. **"New +"** → **"Web Service"**
2. GitHub 레포지토리 연결
3. 설정:
   - **Name**: `todo-api`
   - **Environment**: `Docker` (Dockerfile 사용)
   - **Region**: `Singapore` (한국과 가장 가까움)
   - **Branch**: `main`
   - **Plan**: `Free`
4. **"Create Web Service"** 클릭

#### 3단계: 프론트엔드 배포

1. **"New +"** → **"Static Site"**
2. GitHub 레포지토리 연결 (frontend 폴더)
3. 설정:
   - **Name**: `todo-frontend`
   - **Build Command**: (비워두기)
   - **Publish Directory**: `frontend`
4. **"Create Static Site"** 클릭

### 🎉 완료!

Render가 URL 생성:
```
Backend: https://todo-api.onrender.com
Frontend: https://todo-frontend.onrender.com
```

---

## 🎤 면접 시연 팁

### 📱 면접 준비 체크리스트:

#### 면접 하루 전:
```
[ ] 배포 완료 및 URL 확인
[ ] 모든 기능 테스트 (CRUD 동작 확인)
[ ] 스크린샷 준비 (동작하는 화면)
[ ] 백업 URL 준비 (ngrok 로컬 버전)
[ ] 노트북/PC 인터넷 연결 확인
```

#### 면접 당일:
```
[ ] URL 바로 열 수 있게 북마크
[ ] 배포 과정 설명 준비
[ ] 기술 스택 설명 준비
[ ] 어려웠던 점/배운 점 정리
```

### 💬 면접관에게 할 수 있는 설명:

**1. 프로젝트 소개 (30초)**
```
"ASP.NET Core로 REST API 백엔드를 구축하고,
PHP와 JavaScript로 프론트엔드를 만들었습니다.
[URL 공유] 이 주소에서 직접 확인하실 수 있습니다."
```

**2. 기술 스택 설명 (1분)**
```
"백엔드는 ASP.NET Core 8.0을 사용했고,
Entity Framework Core로 데이터베이스를 관리합니다.
REST API 아키텍처로 프론트엔드와 완전히 분리했으며,
의존성 주입 패턴을 사용해 테스트 가능한 구조로 설계했습니다.

프론트엔드는 PHP로 서빙하고,
JavaScript fetch API로 비동기 통신을 구현했습니다."
```

**3. 배포 과정 설명 (1분)**
```
"[ngrok 사용 시]
로컬 환경을 ngrok을 통해 외부에 노출시켰습니다.
실제 프로덕션 환경에서는 Azure나 AWS를 사용할 예정입니다.

[클라우드 사용 시]
Railway/Azure/Render를 사용해 배포했습니다.
GitHub과 연동해 CI/CD 파이프라인을 구축했고,
코드를 push하면 자동으로 배포됩니다."
```

**4. 기능 시연 (2분)**
```
"실제로 동작하는 모습을 보여드리겠습니다.

[할일 추가]
새로운 할일을 추가하면 POST 요청이 API로 전송되고,
데이터베이스에 저장된 후 화면에 즉시 반영됩니다.

[할일 수정]
인라인 편집으로 바로 수정 가능하며,
PUT 요청으로 업데이트됩니다.

[할일 삭제]
삭제 버튼 클릭 시 DELETE 요청이 전송됩니다.

[필터링]
완료/미완료 상태로 필터링할 수 있습니다."
```

**5. 어려웠던 점 / 배운 점 (1분)**
```
"CORS 설정이 처음에 어려웠는데,
프론트엔드와 백엔드의 도메인이 다를 때
왜 필요한지 이해하게 되었습니다.

Entity Framework Core의 비동기 처리를 통해
성능 최적화를 배웠고,

REST API 설계 원칙을 따라
명확한 엔드포인트 구조를 만들었습니다."
```

### 🚨 예상 질문과 답변:

**Q: "왜 PHP를 프론트엔드로 선택했나요?"**
```
A: "학습 목적으로 다양한 기술을 경험하고 싶었습니다.
실제로는 React나 Vue.js를 사용하는 것이 더 적합하다고 생각하며,
다음 프로젝트에서는 React를 사용할 계획입니다."
```

**Q: "데이터베이스는 무엇을 사용했나요?"**
```
A: "현재는 In-Memory 데이터베이스를 사용하지만,
Entity Framework Core 덕분에 SQL Server, PostgreSQL 등
다른 데이터베이스로 쉽게 전환 가능합니다.
[클라우드 배포 시] 프로덕션에서는 SQL Server를 사용하고 있습니다."
```

**Q: "보안은 어떻게 처리했나요?"**
```
A: "현재는 기본 기능 구현에 집중했지만,
다음 단계로 JWT 토큰 기반 인증을 추가할 계획입니다.
또한 입력 검증을 통해 SQL Injection 같은
기본적인 보안 위협은 방어하고 있습니다."
```

**Q: "테스트는 작성했나요?"**
```
A: "[아직 작성 안 했다면]
현재는 Swagger를 통한 수동 테스트를 진행했지만,
xUnit을 사용한 단위 테스트와 통합 테스트를
추가할 계획입니다.

[작성했다면]
xUnit으로 단위 테스트를 작성했고,
서비스 계층의 비즈니스 로직을 테스트하고 있습니다."
```

### 📊 보너스: 개발 과정 설명

**아키텍처 다이어그램 보여주기:**
```
[브라우저]
    ↓ JavaScript fetch()
[PHP Frontend :8080]
    ↓ HTTP/JSON
[ASP.NET Core API :5000]
    ↓ Entity Framework
[Database]
```

**폴더 구조 설명:**
```
Controllers/ → API 엔드포인트 (MVC의 C)
Services/    → 비즈니스 로직
Data/        → 데이터 액세스 계층
Models/      → 데이터 모델
```

**의존성 주입 설명:**
```csharp
// Program.cs에서 서비스 등록
builder.Services.AddScoped<ITodoService, TodoService>();

// Controller에서 주입 받음
public TodoController(ITodoService todoService)
{
    _todoService = todoService;
}
```

---

## 🎯 최종 추천

### 면접이 며칠 안 남았다면:
→ **ngrok** 사용 (5분 완료)

### 제대로 배포 경험을 쌓고 싶다면:
→ **Railway.app** (15분, 무료, 간단)

### Microsoft 기술 스택 회사라면:
→ **Azure App Service** (20분, Microsoft 공식)

---

## 💡 추가 팁

### URL 짧게 만들기:
- **Bitly**: https://bitly.com (무료)
- 긴 ngrok URL을 짧게: `https://bit.ly/my-todo-app`

### 데모 데이터 준비:
```javascript
// 면접 전에 샘플 데이터 추가해두기
const sampleTodos = [
    "ASP.NET Core 학습 완료",
    "REST API 구현",
    "프론트엔드 개발",
    "클라우드 배포"
];
```

### 백업 계획:
- 인터넷이 안 될 경우를 대비해 **스크린샷/동영상** 준비
- 로컬 환경도 준비 (`localhost` 시연 가능하도록)

---

## 🎉 마무리

이제 면접에서 자신있게 프로젝트를 시연할 수 있습니다!

**핵심은:**
1. ✅ 동작하는 URL 준비
2. ✅ 기술 스택 설명 준비
3. ✅ 배포 과정 설명 준비
4. ✅ 어려웠던 점/배운 점 정리

**면접 성공을 기원합니다! 화이팅! 🚀**
