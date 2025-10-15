<?php
/**
 * ===========================
 * 환경 설정 파일 (Configuration File)
 * ===========================
 *
 * 애플리케이션의 모든 설정을 중앙에서 관리
 * Centrally manage all application settings
 *
 * 주요 기능 (Main Features):
 * - API 엔드포인트 URL 관리
 * - 페이지 타이틀 설정
 * - 환경별 설정 (개발/프로덕션)
 */

// ===========================
// API 설정 (API Configuration)
// ===========================

/**
 * API_BASE_URL: ASP.NET Core REST API 기본 주소
 * API_BASE_URL: Base URL for ASP.NET Core REST API
 *
 * 개발 환경: http://localhost:5000
 * 프로덕션: 실제 서버 URL로 변경
 *
 * Development: http://localhost:5000
 * Production: Change to actual server URL
 */
define('API_BASE_URL', 'http://localhost:5000/api/todos');

/**
 * API_TIMEOUT: API 요청 타임아웃 (초)
 * API_TIMEOUT: API request timeout (seconds)
 */
define('API_TIMEOUT', 30);

// ===========================
// 애플리케이션 설정 (Application Settings)
// ===========================

/**
 * APP_NAME: 애플리케이션 이름
 * APP_NAME: Application name
 */
define('APP_NAME', '할일 목록');

/**
 * APP_SUBTITLE: 애플리케이션 부제목
 * APP_SUBTITLE: Application subtitle
 */
define('APP_SUBTITLE', 'ASP.NET Core API + PHP Frontend');

/**
 * DEFAULT_PAGE_TITLE: 기본 페이지 타이틀
 * DEFAULT_PAGE_TITLE: Default page title
 */
define('DEFAULT_PAGE_TITLE', APP_NAME);

// ===========================
// 경로 설정 (Path Configuration)
// ===========================

/**
 * BASE_PATH: 프론트엔드 기본 경로
 * BASE_PATH: Frontend base path
 */
define('BASE_PATH', __DIR__);

/**
 * LAYOUT_PATH: 레이아웃 파일 경로
 * LAYOUT_PATH: Layout files path
 */
define('LAYOUT_PATH', BASE_PATH . '/layout');

/**
 * PAGES_PATH: 페이지 파일 경로
 * PAGES_PATH: Page files path
 */
define('PAGES_PATH', BASE_PATH . '/pages');

// ===========================
// 디버그 모드 (Debug Mode)
// ===========================

/**
 * DEBUG_MODE: 디버그 모드 활성화 여부
 * DEBUG_MODE: Enable debug mode
 *
 * true: 에러 메시지 표시, 상세 로그
 * false: 에러 숨김, 프로덕션 모드
 *
 * true: Show error messages, detailed logs
 * false: Hide errors, production mode
 */
define('DEBUG_MODE', true);

// 디버그 모드에 따라 에러 표시 설정
// Set error display based on debug mode
if (DEBUG_MODE) {
    error_reporting(E_ALL);
    ini_set('display_errors', 1);
} else {
    error_reporting(0);
    ini_set('display_errors', 0);
}

// ===========================
// 세션 시작 (Start Session)
// ===========================

/**
 * 세션: 사용자별 데이터 저장소
 * Session: Per-user data storage
 *
 * 용도 (Uses):
 * - 사용자 설정 (테마, 언어 등)
 * - 임시 메시지 저장
 * - 상태 관리
 */
if (session_status() === PHP_SESSION_NONE) {
    session_start();
}

// ===========================
// 헬퍼 함수 (Helper Functions)
// ===========================

/**
 * getApiUrl(): API 기본 URL 반환
 * getApiUrl(): Return API base URL
 *
 * @return string API URL
 */
function getApiUrl() {
    return API_BASE_URL;
}

/**
 * getPageTitle(): 페이지 타이틀 생성
 * getPageTitle(): Generate page title
 *
 * @param string $title 페이지별 타이틀
 * @return string 완성된 타이틀
 */
function getPageTitle($title = '') {
    if (empty($title)) {
        return DEFAULT_PAGE_TITLE;
    }
    return $title . ' - ' . APP_NAME;
}

/**
 * getCurrentPage(): 현재 페이지 가져오기
 * getCurrentPage(): Get current page
 *
 * @return string 현재 페이지 이름
 */
function getCurrentPage() {
    return isset($_GET['page']) ? $_GET['page'] : 'list';
}

/**
 * buildUrl(): URL 생성 헬퍼
 * buildUrl(): URL building helper
 *
 * @param string $page 페이지 이름
 * @param array $params 추가 파라미터
 * @return string 생성된 URL
 *
 * 사용 예 (Usage example):
 * buildUrl('form', ['id' => 5]) → index.php?page=form&id=5
 */
function buildUrl($page, $params = []) {
    $url = 'index.php?page=' . urlencode($page);

    foreach ($params as $key => $value) {
        $url .= '&' . urlencode($key) . '=' . urlencode($value);
    }

    return $url;
}

/**
 * debugLog(): 디버그 로그 출력
 * debugLog(): Output debug log
 *
 * @param mixed $data 로그 데이터
 * @param string $label 레이블
 */
function debugLog($data, $label = 'DEBUG') {
    if (DEBUG_MODE) {
        echo "<!-- {$label}: ";
        print_r($data);
        echo " -->\n";
    }
}

// ===========================
// 초기화 완료 로그
// Initialization Complete Log
// ===========================

debugLog([
    'API_URL' => API_BASE_URL,
    'APP_NAME' => APP_NAME,
    'DEBUG_MODE' => DEBUG_MODE,
    'SESSION_ID' => session_id()
], 'CONFIG_LOADED');
