@echo off
REM ========================================
REM ngrok을 사용한 빠른 배포 스크립트
REM Quick deployment script using ngrok
REM ========================================

echo.
echo ========================================
echo   Todo App - ngrok 배포 스크립트
echo   Todo App - ngrok Deployment Script
echo ========================================
echo.

REM 컬러 출력 설정
REM Color output settings
color 0A

REM ngrok이 설치되어 있는지 확인
REM Check if ngrok is installed
where ngrok >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [오류] ngrok이 설치되지 않았습니다!
    echo [ERROR] ngrok is not installed!
    echo.
    echo 설치 방법 (Installation):
    echo   1. choco install ngrok
    echo   2. 또는 https://ngrok.com/download 에서 다운로드
    echo.
    pause
    exit /b 1
)

REM dotnet이 설치되어 있는지 확인
REM Check if dotnet is installed
where dotnet >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [오류] .NET SDK가 설치되지 않았습니다!
    echo [ERROR] .NET SDK is not installed!
    echo.
    echo 설치: https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

REM PHP가 설치되어 있는지 확인
REM Check if PHP is installed
where php >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo [오류] PHP가 설치되지 않았습니다!
    echo [ERROR] PHP is not installed!
    echo.
    echo 설치: https://windows.php.net/download/
    echo.
    pause
    exit /b 1
)

echo [1/7] 프로젝트 빌드 중... (Building project...)
echo.
dotnet build --configuration Release
if %ERRORLEVEL% NEQ 0 (
    echo [오류] 빌드 실패! (Build failed!)
    pause
    exit /b 1
)

echo.
echo [2/7] 백엔드 시작 중... (Starting backend...)
echo.

REM 백엔드를 백그라운드에서 실행
REM Start backend in background
start "ASP.NET Core Backend" cmd /k "dotnet run & echo 백엔드 실행 중 (Backend running)..."

REM 백엔드가 시작될 때까지 대기 (10초)
REM Wait for backend to start (10 seconds)
echo 백엔드 시작 대기 중... (Waiting for backend to start...)
timeout /t 10 /nobreak >nul

echo.
echo [3/7] 백엔드 ngrok 터널 생성 중... (Creating ngrok tunnel for backend...)
echo.

REM 백엔드 ngrok을 백그라운드에서 실행
REM Start backend ngrok in background
start "Backend ngrok" cmd /k "ngrok http 5000"

REM ngrok이 시작될 때까지 대기 (5초)
REM Wait for ngrok to start (5 seconds)
timeout /t 5 /nobreak >nul

echo.
echo ========================================
echo   중요! (IMPORTANT!)
echo ========================================
echo.
echo 1. "Backend ngrok" 창에서 URL 복사:
echo    Copy URL from "Backend ngrok" window:
echo    예시: https://xxxx-xxxx-xxxx.ngrok.io
echo.
echo 2. frontend\app.js 파일을 열어서 API_BASE_URL을 변경하세요:
echo    Open frontend\app.js and change API_BASE_URL:
echo.
echo    const API_BASE_URL = 'https://YOUR-NGROK-URL.ngrok.io/api/todo';
echo.
echo 3. 파일을 저장한 후 아무 키나 누르세요.
echo    Save the file and press any key to continue.
echo.
pause

echo.
echo [4/7] 프론트엔드 시작 중... (Starting frontend...)
echo.

REM 프론트엔드를 백그라운드에서 실행
REM Start frontend in background
start "PHP Frontend" cmd /k "cd frontend && php -S localhost:8080"

REM 프론트엔드가 시작될 때까지 대기 (3초)
REM Wait for frontend to start (3 seconds)
timeout /t 3 /nobreak >nul

echo.
echo [5/7] 프론트엔드 ngrok 터널 생성 중... (Creating ngrok tunnel for frontend...)
echo.

REM 프론트엔드 ngrok을 백그라운드에서 실행
REM Start frontend ngrok in background
start "Frontend ngrok" cmd /k "ngrok http 8080"

timeout /t 3 /nobreak >nul

echo.
echo ========================================
echo   🎉 배포 완료! (Deployment Complete!)
echo ========================================
echo.
echo 열린 창들 (Opened windows):
echo   1. ASP.NET Core Backend (포트 5000)
echo   2. Backend ngrok (외부 URL)
echo   3. PHP Frontend (포트 8080)
echo   4. Frontend ngrok (외부 URL)
echo.
echo 다음 단계 (Next steps):
echo   1. "Frontend ngrok" 창에서 URL 복사
echo      Copy URL from "Frontend ngrok" window
echo      예시: https://yyyy-yyyy-yyyy.ngrok.io
echo.
echo   2. 이 URL을 면접관에게 공유하세요!
echo      Share this URL with your interviewer!
echo.
echo   3. 브라우저에서 테스트:
echo      Test in browser:
echo      - 할일 추가 (Add todo)
echo      - 할일 수정 (Edit todo)
echo      - 할일 삭제 (Delete todo)
echo.
echo 모니터링 (Monitoring):
echo   - ngrok 대시보드: http://127.0.0.1:4040
echo   - API 직접 확인: [백엔드 ngrok URL]/api/todo
echo.
echo 종료하려면 (To stop):
echo   모든 창을 닫거나 이 창을 닫으세요
echo   Close all windows or close this window
echo.
echo ========================================
echo   면접 성공을 기원합니다! Good luck! 🚀
echo ========================================
echo.
pause
