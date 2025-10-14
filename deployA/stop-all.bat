@echo off
REM ========================================
REM 모든 서비스 중지 스크립트
REM Stop all services script
REM ========================================

echo.
echo ========================================
echo   모든 서비스 중지 중...
echo   Stopping all services...
echo ========================================
echo.

REM .NET 프로세스 종료 (Stop .NET processes)
echo [1/4] ASP.NET Core 백엔드 종료... (Stopping ASP.NET Core backend...)
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5000') do (
    taskkill /PID %%a /F >nul 2>&1
)

REM PHP 프로세스 종료 (Stop PHP processes)
echo [2/4] PHP 프론트엔드 종료... (Stopping PHP frontend...)
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :8080') do (
    taskkill /PID %%a /F >nul 2>&1
)

REM ngrok 프로세스 종료 (Stop ngrok processes)
echo [3/4] ngrok 터널 종료... (Stopping ngrok tunnels...)
taskkill /IM ngrok.exe /F >nul 2>&1

REM 모든 cmd 창 정리 (Close all cmd windows except current)
echo [4/4] 관련 창 정리... (Cleaning up windows...)
timeout /t 2 /nobreak >nul

echo.
echo ========================================
echo   ✅ 모든 서비스가 중지되었습니다!
echo   ✅ All services stopped!
echo ========================================
echo.
pause
