# ⚡ 빠른 배포 가이드 (Quick Deployment Guide)

면접 전날에 5-10분 안에 배포를 완료하는 방법입니다.

---

## 🎯 방법 선택

### 1️⃣ ngrok (5분) - 가장 빠름 ⭐⭐⭐⭐⭐

**장점:**
- ✅ 5분 안에 완료
- ✅ 무료
- ✅ 설정 거의 없음

**단점:**
- ⚠️ 본인 컴퓨터가 켜져 있어야 함
- ⚠️ URL이 매번 바뀜

**언제 사용?**
- 면접이 내일이거나 급할 때
- 처음 배포해보는 경우
- 복잡한 설정을 원하지 않을 때

### 2️⃣ Railway.app (15분) - 실전 배포 ⭐⭐⭐⭐

**장점:**
- ✅ 무료
- ✅ 실제 클라우드 배포 경험
- ✅ URL 고정
- ✅ GitHub 자동 배포

**단점:**
- ⚠️ 초기 설정 필요
- ⚠️ Git 사용 필요

**언제 사용?**
- 제대로 된 배포를 보여주고 싶을 때
- 포트폴리오로 사용할 때
- 면접까지 시간이 있을 때

---

## ⚡ ngrok 5분 배포 (권장)

### 준비물:
- [ ] ngrok 계정 (https://ngrok.com - 무료)
- [ ] ngrok 설치
- [ ] 인터넷 연결

### 1단계: ngrok 설치 (1분)

**방법 A: Chocolatey (권장)**
```powershell
# PowerShell 관리자 권한으로 실행
choco install ngrok
```

**방법 B: 직접 다운로드**
1. https://ngrok.com/download
2. Windows 버전 다운로드
3. 압축 해제 → `ngrok.exe`를 `C:\ngrok\` 에 저장
4. 시스템 환경 변수 PATH에 `C:\ngrok` 추가

### 2단계: ngrok 인증 (30초)

```powershell
# 1. https://dashboard.ngrok.com/get-started/your-authtoken 방문
# 2. authtoken 복사
# 3. 아래 명령 실행 (YOUR_TOKEN을 실제 토큰으로 교체)

ngrok config add-authtoken YOUR_AUTH_TOKEN
```

### 3단계: 자동 배포 스크립트 실행 (1분)

```powershell
cd C:\claude\asp_practice_1
.\deploy-ngrok.bat
```

**스크립트가 자동으로 실행:**
1. ✅ 프로젝트 빌드
2. ✅ 백엔드 실행 (포트 5000)
3. ✅ 백엔드 ngrok 터널 생성
4. ✅ 프론트엔드 실행 (포트 8080)
5. ✅ 프론트엔드 ngrok 터널 생성

### 4단계: URL 복사 (30초)

**스크립트가 멈추면:**

1. "Backend ngrok" 창 열기
2. 이런 URL 찾기: `https://1a2b-3c4d.ngrok.io`
3. 복사

### 5단계: 프론트엔드 설정 (1분)

`frontend/app.js` 파일 열기:

**변경 전:**
```javascript
const API_BASE_URL = 'http://localhost:5000/api/todo';
```

**변경 후:**
```javascript
// 여기에 복사한 ngrok URL 붙여넣기
const API_BASE_URL = 'https://1a2b-3c4d.ngrok.io/api/todo';
```

**저장** (Ctrl+S)

### 6단계: 스크립트 계속 (30초)

1. 배포 스크립트 창으로 돌아가기
2. 아무 키나 누르기
3. "Frontend ngrok" 창 열기
4. 프론트엔드 URL 복사: `https://5e6f-7g8h.ngrok.io`

### 🎉 완료!

이제 이 URL을 면접관에게 공유하면 됩니다:
```
https://5e6f-7g8h.ngrok.io
```

### ✅ 테스트 체크리스트:

```
[ ] 브라우저에서 URL 열기
[ ] 할일 추가 테스트
[ ] 할일 완료 토글 테스트
[ ] 할일 수정 테스트
[ ] 할일 삭제 테스트
[ ] 필터링 테스트 (전체/진행중/완료)
```

---

## 🚀 Railway 15분 배포

### 준비물:
- [ ] GitHub 계정
- [ ] Railway 계정 (https://railway.app - 무료)
- [ ] Git 설치

### 1단계: GitHub 레포지토리 생성 (3분)

```powershell
cd C:\claude\asp_practice_1

# Git 초기화
git init
git add .
git commit -m "Initial commit: ASP.NET Core Todo API"

# GitHub에서 새 레포지토리 생성 (웹에서)
# 그 다음:
git remote add origin https://github.com/YOUR_USERNAME/asp-todo-api.git
git branch -M main
git push -u origin main
```

### 2단계: Dockerfile 생성 (2분)

`C:\claude\asp_practice_1\Dockerfile` 생성:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TodoApi.csproj", "./"]
RUN dotnet restore
COPY . .
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TodoApi.dll"]
```

```powershell
git add Dockerfile
git commit -m "Add Dockerfile"
git push
```

### 3단계: Railway 배포 (5분)

1. https://railway.app 방문
2. **"Start a New Project"**
3. **"Deploy from GitHub repo"**
4. 레포지토리 선택
5. **"Deploy Now"** 클릭

### 4단계: 환경 변수 설정 (2분)

Railway 대시보드:
1. 프로젝트 클릭
2. **"Variables"** 탭
3. 추가:
   ```
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=http://0.0.0.0:$PORT
   ```
4. **"Deploy"** 클릭

### 5단계: 프론트엔드 배포 (3분)

**옵션 A: Vercel (더 쉬움)**

1. https://vercel.com 방문
2. **"Import Project"**
3. GitHub 레포지토리 선택
4. **Root Directory**: `frontend` 입력
5. **"Deploy"** 클릭

**옵션 B: Railway (같은 플랫폼)**

1. `frontend` 폴더를 별도 Git 레포지토리로 생성
2. Railway에서 새 프로젝트 생성
3. 배포

### 6단계: CORS 및 URL 업데이트

**백엔드 `Program.cs`:**
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:8080",
            "https://YOUR-FRONTEND.vercel.app" // Vercel URL 추가
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
```

**프론트엔드 `app.js`:**
```javascript
const API_BASE_URL = 'https://YOUR-BACKEND.railway.app/api/todo';
```

Git push로 자동 배포:
```powershell
git add .
git commit -m "Update CORS and API URL"
git push
```

### 🎉 완료!

Railway/Vercel이 자동으로 배포:
```
Backend: https://asp-todo-api-production.up.railway.app
Frontend: https://your-project.vercel.app
```

---

## 📱 면접 당일 체크리스트

### ngrok 사용 시:

**면접 1시간 전:**
```
[ ] 노트북/PC 충전 100%
[ ] 인터넷 연결 확인
[ ] ngrok 계정 로그인 확인
[ ] deploy-ngrok.bat 실행
[ ] URL 테스트
[ ] URL을 핸드폰 메모장에 저장 (백업)
```

**면접 중:**
```
[ ] 모든 창 열어두기 (백엔드, 프론트엔드, ngrok)
[ ] URL 바로 복사할 수 있게 준비
[ ] ngrok 대시보드 열어두기 (http://127.0.0.1:4040)
```

### 클라우드 사용 시:

**면접 1시간 전:**
```
[ ] 배포 URL 확인
[ ] 브라우저에서 테스트
[ ] 모든 기능 동작 확인
[ ] URL 북마크
```

---

## 🆘 긴급 문제 해결

### ngrok이 "Session Expired" 에러

**원인:** 8시간 세션 제한 (무료 플랜)

**해결:**
```powershell
# ngrok 재시작
# "Backend ngrok" 창에서 Ctrl+C
ngrok http 5000

# 새 URL 복사 → app.js 업데이트
```

### CORS 에러 (빨간색 에러)

**원인:** 백엔드가 프론트엔드 도메인을 허용하지 않음

**해결:**
1. `Program.cs` 열기
2. CORS 설정에 ngrok URL 추가:
```csharp
policy.WithOrigins(
    "http://localhost:8080",
    "https://YOUR-NGROK-URL.ngrok.io" // 추가
)
```
3. 백엔드 재시작

### 백엔드가 시작되지 않음

**확인:**
```powershell
# 포트 5000이 사용 중인지 확인
netstat -ano | findstr :5000

# 프로세스 종료 (PID는 위 명령에서 확인)
taskkill /PID [프로세스ID] /F

# 백엔드 재시작
dotnet run
```

### 프론트엔드가 "Cannot GET /" 에러

**원인:** PHP 서버가 제대로 시작되지 않음

**해결:**
```powershell
cd C:\claude\asp_practice_1\frontend
php -S localhost:8080

# 또는 다른 포트 사용
php -S localhost:8081
```

---

## 💡 면접관에게 보여줄 때

### 1. URL 공유 (10초)
```
"이 주소에서 직접 확인하실 수 있습니다:
https://xxxx.ngrok.io"
```

### 2. 기능 시연 (2분)
```
1. 할일 추가 → "면접 준비"
2. 완료 체크
3. 수정 → "면접 합격!"
4. 필터링 → "완료됨"
5. 삭제
```

### 3. 기술 설명 (1분)
```
"백엔드는 ASP.NET Core로 REST API를 구현했고,
프론트엔드는 JavaScript fetch API로 통신합니다.
[ngrok 사용 시] 로컬 환경을 ngrok으로 외부에 노출했습니다.
[클라우드 사용 시] Railway/Azure에 배포했습니다."
```

### 4. 모니터링 보여주기 (보너스)
```
"ngrok 대시보드에서 실시간 API 호출을 확인할 수 있습니다."
→ http://127.0.0.1:4040 열기
```

---

## 🎯 최종 점검

### 면접 전날:
```
[ ] 배포 방법 선택 (ngrok or 클라우드)
[ ] 배포 완료
[ ] URL 저장 (여러 곳에 백업)
[ ] 스크린샷 촬영 (백업용)
[ ] 모든 기능 테스트
```

### 면접 당일:
```
[ ] 배포 상태 확인
[ ] URL 바로 열 수 있게 준비
[ ] 백업 계획 (스크린샷/동영상)
[ ] 설명 리허설
```

---

## 📞 도움이 필요하면

### 공식 문서:
- ngrok: https://ngrok.com/docs
- Railway: https://docs.railway.app
- ASP.NET Core: https://docs.microsoft.com/aspnet/core

### 커뮤니티:
- Stack Overflow
- Reddit: r/dotnet, r/webdev
- Discord: ASP.NET Core 서버

---

## 🎉 마무리

**가장 중요한 것:**
1. ✅ 동작하는 것을 보여주는 것
2. ✅ 설명할 수 있는 것
3. ✅ 자신감 있는 태도

**완벽하지 않아도 괜찮습니다!**
- 에러가 나면 어떻게 해결했는지 설명
- 배운 점을 이야기
- 다음에 어떻게 개선할지 제시

**면접 성공을 기원합니다! 화이팅! 🚀**
