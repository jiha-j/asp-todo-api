<?php
/**
 * ===========================
 * 할일 목록 페이지 (To-Do List Page)
 * ===========================
 *
 * 모든 할일을 목록으로 표시하는 메인 페이지
 * Main page displaying all to-dos in a list
 */

// 페이지 설정 (Page configuration)
$pageTitle = '할일 목록';
$jsFiles = ['app.js'];

// 헤더 포함 (Include header)
include LAYOUT_PATH . '/header.php';
?>

<div class="container">
    <!--
        헤더 섹션 (Header Section)
        - 애플리케이션 제목 표시
        - Displays application title
    -->
    <header class="header">
        <h1>📝 <?= APP_NAME ?></h1>
        <p class="subtitle"><?= APP_SUBTITLE ?></p>
    </header>

    <!--
        API 연결 상태 표시 (API Connection Status)
        - API 서버와의 연결 상태를 실시간으로 표시
        - Shows real-time connection status with API server
        - JavaScript에서 동적으로 업데이트됨 (Dynamically updated by JavaScript)
    -->
    <div class="status-container">
        <div id="apiStatus" class="status-indicator">
            <span class="status-dot"></span>
            <span class="status-text">API 연결 확인 중... (Checking API connection...)</span>
        </div>
    </div>

    <!--
        에러 메시지 영역 (Error Message Area)
        - API 호출 중 발생하는 에러를 사용자에게 표시
        - Displays errors that occur during API calls to users
    -->
    <div id="errorMessage" class="error-message" style="display: none;"></div>

    <!--
        성공 메시지 영역 (Success Message Area)
        - 작업 성공 시 피드백을 사용자에게 표시
        - Displays success feedback to users when operations succeed
    -->
    <div id="successMessage" class="success-message" style="display: none;"></div>

    <!--
        ===========================
        할일 추가 버튼 (Add To-Do Button)
        ===========================

        새로운 할일을 추가하기 위해 폼 페이지로 이동하는 버튼
        Button to navigate to form page for creating new to-do
    -->
    <div class="add-todo-container">
        <a href="<?= buildUrl('form') ?>" class="btn-add">
            <span class="btn-icon">➕</span>
            새 할일 만들기 (Create New Todo)
        </a>
    </div>

    <!--
        ===========================
        검색 및 필터 섹션 (Search & Filter Section)
        ===========================

        한 줄에 배치: 우선순위 필터 30% + 제목 검색 70%
        Single row layout: Priority filter 30% + Title search 70%
    -->
    <div class="search-filter-container">
        <!-- 우선순위 필터 (30%) -->
        <div class="priority-filter">
            <select id="priorityFilter" class="priority-select" onchange="applyFilters()">
                <option value="">우선순위: 전체</option>
                <option value="0">우선순위: 낮음</option>
                <option value="1">우선순위: 보통</option>
                <option value="2">우선순위: 높음</option>
                <option value="3">우선순위: 긴급</option>
            </select>
        </div>

        <!-- 제목 검색 (70%) -->
        <div class="search-box">
            <input
                type="text"
                id="searchInput"
                class="search-input"
                placeholder="🔍 제목으로 검색... (Search by title...)"
                autocomplete="off"
            >
            <button id="searchButton" class="btn-search" onclick="applyFilters()">검색</button>
            <button id="clearButton" class="btn-clear" onclick="clearFilters()" style="display: none;">✕</button>
        </div>
    </div>

    <!--
        ===========================
        필터 버튼 섹션 (Filter Buttons Section)
        ===========================

        할일 항목을 상태별로 필터링하는 버튼들
        Buttons to filter to-do items by status
    -->
    <div class="filter-container">
        <button class="filter-btn active" data-filter="all" onclick="filterTodos('all')">
            전체 (All) <span class="count" id="countAll">0</span>
        </button>
        <button class="filter-btn" data-filter="active" onclick="filterTodos('active')">
            진행중 (Active) <span class="count" id="countActive">0</span>
        </button>
        <button class="filter-btn" data-filter="completed" onclick="filterTodos('completed')">
            완료됨 (Completed) <span class="count" id="countCompleted">0</span>
        </button>
    </div>

    <!--
        ===========================
        할일 목록 영역 (To-Do List Area)
        ===========================

        이 영역은 JavaScript에서 동적으로 채워집니다.
        This area is dynamically populated by JavaScript.
    -->
    <div id="todoList" class="todo-list">
        <!--
            로딩 중 표시 (Loading Indicator)
            - 데이터를 불러오는 동안 표시됨
            - Shown while loading data
        -->
        <div class="loading">
            <div class="spinner"></div>
            <p>할일 목록을 불러오는 중... (Loading to-do list...)</p>
        </div>
    </div>

    <!--
        푸터 섹션 (Footer Section)
        - 기술 스택 정보 표시
        - Displays technology stack information
    -->
    <footer class="footer">
        <p>
            <strong>Backend:</strong> ASP.NET Core REST API (Port 5000) |
            <strong>Frontend:</strong> PHP + JavaScript |
            <strong>Database:</strong> SQL Server
        </p>
        <p class="tech-info">
            💡 이 애플리케이션은 RESTful API 아키텍처를 사용합니다
            <br>
            💡 This application uses RESTful API architecture
        </p>
    </footer>
</div>

<?php
// 푸터 포함 (Include footer)
include LAYOUT_PATH . '/footer.php';
?>
