# IIS TodoApi 애플리케이션 설정 스크립트
# 관리자 권한으로 실행 필요

Import-Module WebAdministration

$appName = "TodoApi"
$publishPath = "C:\claude\asp_practice_1\publish"
$siteName = "Default Web Site"
$appPath = "/$appName"

Write-Host "IIS TodoApi 애플리케이션 설정 시작..." -ForegroundColor Green

# 1. 기존 애플리케이션 확인 및 제거
$existingApp = Get-WebApplication -Site $siteName -Name $appName -ErrorAction SilentlyContinue
if ($existingApp) {
    Write-Host "기존 애플리케이션 제거 중..." -ForegroundColor Yellow
    Remove-WebApplication -Site $siteName -Name $appName
}

# 2. 새 애플리케이션 생성
Write-Host "새 애플리케이션 생성 중..." -ForegroundColor Cyan
New-WebApplication -Site $siteName -Name $appName -PhysicalPath $publishPath -ApplicationPool "DefaultAppPool"

# 3. 애플리케이션 풀 설정 (.NET Core는 "No Managed Code" 사용)
Write-Host "애플리케이션 풀 설정 중..." -ForegroundColor Cyan
Set-ItemProperty "IIS:\AppPools\DefaultAppPool" -Name "managedRuntimeVersion" -Value ""

# 4. 권한 설정
Write-Host "폴더 권한 설정 중..." -ForegroundColor Cyan
$acl = Get-Acl $publishPath
$permission = "IIS_IUSRS", "ReadAndExecute", "ContainerInherit,ObjectInherit", "None", "Allow"
$accessRule = New-Object System.Security.AccessControl.FileSystemAccessRule $permission
$acl.SetAccessRule($accessRule)
Set-Acl $publishPath $acl

# 5. 설정 확인
Write-Host "`n설정 완료!" -ForegroundColor Green
Write-Host "애플리케이션 정보:" -ForegroundColor Yellow
Get-WebApplication -Site $siteName -Name $appName | Select-Object Path, PhysicalPath, ApplicationPool

Write-Host "`n다음 URL에서 애플리케이션을 테스트하세요:" -ForegroundColor Cyan
Write-Host "  http://localhost/$appName/api/todos" -ForegroundColor White
Write-Host "  http://localhost/$appName/swagger" -ForegroundColor White

# 6. 애플리케이션 풀 재시작
Write-Host "`n애플리케이션 풀 재시작 중..." -ForegroundColor Cyan
Restart-WebAppPool -Name "DefaultAppPool"

Write-Host "`n완료!" -ForegroundColor Green
