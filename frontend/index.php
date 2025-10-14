<!DOCTYPE html>
<html lang="ko">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>할일 목록 (To-Do List)</title>
    <link rel="stylesheet" href="styles.css">
</head>
<body>
    <!--
        ===========================
        PHP 프론트엔드 - 할일 목록 애플리케이션
        PHP Frontend - To-Do List Application
        ===========================

        이 파일은 PHP 프론트엔드의 메인 페이지입니다.
        This file is the main page of the PHP frontend.

        구조 설명 (Structure Explanation):
        - HTML: 페이지의 구조와 레이아웃을 정의 (Defines page structure and layout)
        - CSS: 스타일과 디자인을 담당 (Handles styling and design)
        - JavaScript: ASP.NET Core API와 통신하고 동적 기능 제공 (Communicates with API and provides dynamic features)

        이 페이지는 ASP.NET Core REST API와 통신하여 할일 항목을 관리합니다.
        This page communicates with ASP.NET Core REST API to manage to-do items.
    -->

    <!--
        메인 컨테이너 (Main Container)
        - 모든 콘텐츠를 감싸는 래퍼
        - Wrapper that contains all content
    -->
    <div class="container">

        <!--
            헤더 섹션 (Header Section)
            - 애플리케이션 제목 표시
            - Displays application title
        -->
        <header class="header">
            <h1>📝 할일 목록</h1>
            <p class="subtitle">ASP.NET Core API + PHP Frontend</p>
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
            - 기본적으로 숨겨져 있고, 에러 발생 시만 표시됨
            - Hidden by default, shown only when errors occur
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
            할일 추가 폼 (Add To-Do Form)
            ===========================

            새로운 할일을 추가하는 입력 폼
            Input form to add new to-do items

            작동 방식 (How it works):
            1. 사용자가 텍스트 입력 (User enters text)
            2. "추가" 버튼 클릭 또는 Enter 키 누름 (Click "Add" button or press Enter)
            3. JavaScript의 addTodo() 함수가 실행됨 (JavaScript addTodo() function executes)
            4. fetch()를 통해 POST 요청이 API로 전송됨 (POST request sent to API via fetch())
            5. API가 데이터베이스에 저장하고 응답 반환 (API saves to database and returns response)
            6. 목록이 자동으로 새로고침됨 (List automatically refreshes)
        -->
        <div class="add-todo-container">
            <form id="addTodoForm" class="add-todo-form">
                <input
                    type="text"
                    id="todoInput"
                    class="todo-input"
                    placeholder="새로운 할일을 입력하세요... (Enter a new to-do...)"
                    required
                    autocomplete="off"
                >
                <!--
                    type="submit": 폼 제출 버튼 (Form submit button)
                    JavaScript에서 form의 submit 이벤트를 감지하여 처리
                    JavaScript detects and handles the form's submit event
                -->
                <button type="submit" class="btn-add">
                    <span class="btn-icon">➕</span>
                    추가 (Add)
                </button>
            </form>
        </div>

        <!--
            ===========================
            필터 버튼 섹션 (Filter Buttons Section)
            ===========================

            할일 항목을 상태별로 필터링하는 버튼들
            Buttons to filter to-do items by status

            필터 옵션 (Filter Options):
            - 전체 (All): 모든 할일 표시 (Show all to-dos)
            - 진행중 (Active): 완료되지 않은 할일만 표시 (Show only incomplete to-dos)
            - 완료됨 (Completed): 완료된 할일만 표시 (Show only completed to-dos)

            작동 방식 (How it works):
            - 클릭 시 JavaScript의 filterTodos() 함수 호출
            - Click triggers JavaScript filterTodos() function
            - CSS 클래스를 통해 항목을 숨기거나 표시
            - Hides or shows items through CSS classes
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

            데이터 흐름 (Data Flow):
            1. 페이지 로드 시 loadTodos() 함수 실행
               loadTodos() function executes on page load

            2. fetch()로 GET 요청을 API로 전송
               GET request sent to API via fetch()
               URL: http://localhost:5000/api/todos

            3. API가 데이터베이스에서 모든 할일을 조회하여 JSON으로 반환
               API retrieves all to-dos from database and returns as JSON

            4. JavaScript가 JSON 데이터를 파싱하여 HTML 요소 생성
               JavaScript parses JSON data and creates HTML elements

            5. 생성된 HTML을 이 div에 삽입
               Generated HTML inserted into this div

            각 할일 항목의 구조 (Structure of each to-do item):
            - 체크박스: 완료 상태 토글 (Checkbox: Toggle completion status)
            - 텍스트: 할일 내용 (Text: To-do content)
            - 수정 버튼: 인라인 편집 모드 활성화 (Edit button: Activate inline edit mode)
            - 삭제 버튼: 항목 삭제 (Delete button: Remove item)
        -->
        <div id="todoList" class="todo-list">
            <!--
                로딩 중 표시 (Loading Indicator)
                - 데이터를 불러오는 동안 표시됨
                - Shown while loading data
                - JavaScript에서 실제 데이터로 교체됨
                - Replaced with actual data by JavaScript
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

    <!--
        ===========================
        JavaScript 파일 로드 (Load JavaScript File)
        ===========================

        app.js 파일을 로드하여 모든 동적 기능을 활성화합니다.
        Loads app.js file to activate all dynamic features.

        왜 body 태그 끝에 위치하는가? (Why placed at the end of body tag?)
        - HTML 요소들이 먼저 로드되어야 JavaScript가 접근 가능
        - HTML elements must be loaded first for JavaScript to access them
        - 페이지 렌더링 속도 향상
        - Improves page rendering speed

        app.js의 주요 기능 (Main features of app.js):
        1. API와의 통신 (fetch API) - Communication with API
        2. DOM 조작으로 동적 콘텐츠 생성 - Dynamic content creation via DOM manipulation
        3. 이벤트 처리 (클릭, 제출 등) - Event handling (click, submit, etc.)
        4. 에러 처리 및 사용자 피드백 - Error handling and user feedback
    -->
    <script src="app.js"></script>

    <!--
        ===========================
        PHP 서버 정보 (선택사항)
        PHP Server Information (Optional)
        ===========================

        개발 중에 PHP 서버 정보를 확인하고 싶다면 아래 주석을 해제하세요.
        Uncomment below to check PHP server information during development.

        <?php
        // PHP 버전 정보 출력
        // Output PHP version information
        // echo "PHP Version: " . phpversion();
        ?>
    -->
</body>
</html>
