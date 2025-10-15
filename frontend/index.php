<?php
/**
 * ===========================
 * PHP 라우터 (PHP Router)
 * ===========================
 *
 * 모든 요청의 진입점 (Entry point for all requests)
 *
 * 역할 (Responsibilities):
 * 1. 환경 설정 로드 (Load configuration)
 * 2. URL 파라미터 분석 (Parse URL parameters)
 * 3. 적절한 페이지로 라우팅 (Route to appropriate page)
 * 4. 에러 처리 (Handle errors)
 *
 * URL 구조 (URL Structure):
 * - index.php?page=list           → 할일 목록
 * - index.php?page=form           → 새 할일 작성
 * - index.php?page=form&id=5      → 할일 수정 (ID: 5)
 * - index.php?page=detail&id=5    → 할일 상세보기 (ID: 5)
 */

// ===========================
// 1. 환경 설정 로드
// Load Configuration
// ===========================

/**
 * config.php 파일을 로드하여 애플리케이션 설정 초기화
 * Load config.php to initialize application settings
 */
require_once __DIR__ . '/config.php';

// ===========================
// 2. 라우팅 로직
// Routing Logic
// ===========================

/**
 * 현재 요청된 페이지 가져오기
 * Get currently requested page
 *
 * $_GET['page']: URL 쿼리 파라미터에서 page 값 추출
 * 기본값: 'list' (페이지 파라미터가 없으면 목록 페이지)
 */
$page = getCurrentPage();

/**
 * 페이지 라우팅 맵 (Page routing map)
 *
 * 키: URL의 page 파라미터 값
 * 값: 실제 로드할 PHP 파일 경로
 */
$routes = [
    'list'   => PAGES_PATH . '/list.php',      // 할일 목록 페이지
    'form'   => PAGES_PATH . '/form.php',      // 할일 작성/수정 페이지
    'detail' => PAGES_PATH . '/detail.php',    // 할일 상세보기 페이지
];

/**
 * 보안: 허용된 페이지인지 검증
 * Security: Validate if page is allowed
 *
 * array_key_exists(): 배열에 키가 존재하는지 확인
 * 존재하지 않으면 404 에러 처리
 */
if (!array_key_exists($page, $routes)) {
    // 404 에러 처리
    http_response_code(404);
    include PAGES_PATH . '/404.php';
    exit;
}

/**
 * 라우팅된 파일이 실제로 존재하는지 확인
 * Check if routed file actually exists
 */
$pageFile = $routes[$page];

if (!file_exists($pageFile)) {
    // 파일이 존재하지 않으면 500 에러
    http_response_code(500);
    echo '<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>서버 오류 - Server Error</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            display: flex;
            justify-content: center;
            align-items: center;
            height: 100vh;
            margin: 0;
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            color: white;
        }
        .error-container {
            text-align: center;
            padding: 40px;
            background: rgba(255, 255, 255, 0.1);
            border-radius: 16px;
            backdrop-filter: blur(10px);
        }
        h1 { font-size: 72px; margin: 0; }
        p { font-size: 24px; }
        a {
            display: inline-block;
            margin-top: 20px;
            padding: 12px 24px;
            background: white;
            color: #667eea;
            text-decoration: none;
            border-radius: 8px;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <div class="error-container">
        <h1>500</h1>
        <p>서버 오류 (Server Error)</p>
        <p>페이지 파일을 찾을 수 없습니다.</p>
        <p>Cannot find page file.</p>
        <a href="index.php">← 홈으로 돌아가기 (Back to Home)</a>
    </div>
</body>
</html>';
    exit;
}

// ===========================
// 3. 페이지 로드
// Load Page
// ===========================

/**
 * include: 페이지 파일 포함 및 실행
 * include: Include and execute page file
 *
 * 각 페이지 파일(list.php, form.php, detail.php)이 실행됨
 * Each page file (list.php, form.php, detail.php) is executed
 *
 * 페이지 파일 내에서:
 * - header.php 포함 (HTML 시작 부분)
 * - 페이지 컨텐츠 출력
 * - footer.php 포함 (HTML 종료 부분)
 */
include $pageFile;

// ===========================
// 디버그 정보 (개발용)
// Debug Information (For Development)
// ===========================

debugLog([
    'REQUESTED_PAGE' => $page,
    'LOADED_FILE' => $pageFile,
    'GET_PARAMS' => $_GET,
    'SESSION_DATA' => $_SESSION
], 'ROUTER_DEBUG');
