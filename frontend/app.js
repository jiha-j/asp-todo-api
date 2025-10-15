/*
    ===========================
    JavaScript 애플리케이션 - 할일 목록
    JavaScript Application - To-Do List
    ===========================

    이 파일은 프론트엔드의 모든 동적 기능을 담당합니다.
    This file handles all dynamic features of the frontend.

    주요 책임 (Main Responsibilities):
    1. REST API와 통신 (Communication with REST API)
    2. DOM 조작 (DOM Manipulation)
    3. 사용자 이벤트 처리 (User Event Handling)
    4. UI 상태 관리 (UI State Management)

    핵심 개념 (Core Concepts):
    - fetch API: HTTP 요청을 보내는 모던 JavaScript API
    - fetch API: Modern JavaScript API for making HTTP requests

    - async/await: 비동기 코드를 동기 코드처럼 작성
    - async/await: Write asynchronous code like synchronous code

    - DOM: Document Object Model - HTML 요소를 JavaScript로 조작
    - DOM: Document Object Model - Manipulate HTML elements with JavaScript
*/

/*
    ===========================
    전역 변수 (Global Variables)
    ===========================

    애플리케이션 전체에서 사용되는 변수들
    Variables used throughout the application
*/

/*
    API_BASE_URL: REST API의 기본 주소
    API_BASE_URL: Base URL of the REST API

    여기서 ASP.NET Core 백엔드가 실행되고 있어야 합니다.
    ASP.NET Core backend must be running here.

    포트 5000: appsettings.json에서 설정한 기본 포트
    Port 5000: Default port configured in appsettings.json
*/
const API_BASE_URL = 'http://localhost:5000/api/todos';

/*
    현재 활성화된 필터 상태 저장
    Store currently active filter state

    가능한 값 (Possible values):
    - 'all': 모든 할일 표시 (Show all to-dos)
    - 'active': 완료되지 않은 할일만 (Only incomplete to-dos)
    - 'completed': 완료된 할일만 (Only completed to-dos)
*/
let currentFilter = 'all';

/*
    모든 할일 데이터를 메모리에 저장
    Store all to-do data in memory

    API에서 받은 데이터를 여기에 저장하여 필터링 시 재사용
    Store data from API here for reuse during filtering
*/
let allTodos = [];

/*
    ===========================
    초기화 함수
    Initialization Function
    ===========================

    DOMContentLoaded 이벤트: HTML이 완전히 로드되면 실행
    DOMContentLoaded event: Executes when HTML is fully loaded

    왜 필요한가? (Why needed?)
    - JavaScript가 HTML보다 먼저 실행되면 요소를 찾을 수 없음
    - If JavaScript executes before HTML, elements cannot be found
    - 모든 요소가 준비된 후 이벤트 리스너 등록
    - Register event listeners after all elements are ready
*/
document.addEventListener('DOMContentLoaded', function() {
    console.log('🚀 애플리케이션 초기화 중... (Initializing application...)');

    /*
        초기화 순서 (Initialization sequence):
        1. API 연결 상태 확인 (Check API connection status)
        2. 이벤트 리스너 등록 (Register event listeners)
        3. 할일 목록 로드 (Load to-do list)
    */

    checkApiStatus();      // API 연결 확인 (Check API connection)
    loadTodos();          // 데이터 로드 (Load data)

    /*
        검색 입력 필드에 Enter 키 이벤트 등록
        Register Enter key event for search input field
    */
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('keypress', function(event) {
            // Enter 키를 누르면 검색 실행 (Execute search on Enter key)
            if (event.key === 'Enter') {
                event.preventDefault();  // 폼 제출 방지 (Prevent form submission)
                applyFilters();
            }
        });
    }
});

/*
    ===========================
    API 연결 상태 확인
    Check API Connection Status
    ===========================

    async 함수: 비동기 작업을 수행하는 함수
    async function: Function that performs asynchronous operations

    비동기란? (What is asynchronous?)
    - 작업이 완료될 때까지 기다리지 않고 다음 코드 실행
    - Execute next code without waiting for operation to complete
    - 네트워크 요청은 시간이 걸리므로 비동기로 처리
    - Network requests take time, so handled asynchronously
*/
async function checkApiStatus() {
    console.log('🔍 API 연결 상태 확인 중... (Checking API connection...)');

    /*
        document.getElementById(): HTML 요소를 ID로 찾기
        document.getElementById(): Find HTML element by ID
    */
    const statusIndicator = document.getElementById('apiStatus');

    try {
        /*
            fetch(): HTTP 요청을 보내는 함수
            fetch(): Function to send HTTP requests

            await: Promise가 완료될 때까지 기다림
            await: Wait until Promise completes

            fetch의 반환값 (fetch return value):
            - Promise: 미래에 완료될 작업을 나타내는 객체
            - Promise: Object representing a future completion of an operation
        */
        const response = await fetch(API_BASE_URL, {
            method: 'GET',           // HTTP 메서드: GET (조회)
            headers: {
                'Accept': 'application/json'  // JSON 형식으로 응답 요청
            }
        });

        /*
            HTTP 상태 코드 확인 (Check HTTP status code)
            - 200-299: 성공 (Success)
            - 400-499: 클라이언트 에러 (Client error)
            - 500-599: 서버 에러 (Server error)
        */
        if (response.ok) {
            // 연결 성공 (Connection successful)
            statusIndicator.className = 'status-indicator connected';
            statusIndicator.innerHTML = `
                <span class="status-dot"></span>
                <span class="status-text">✅ API 연결됨 (API Connected)</span>
            `;
            console.log('✅ API 연결 성공 (API connection successful)');
        } else {
            throw new Error(`HTTP 에러: ${response.status}`);
        }
    } catch (error) {
        /*
            catch: try 블록에서 에러 발생 시 실행
            catch: Executes when error occurs in try block
        */
        console.error('❌ API 연결 실패:', error);
        statusIndicator.className = 'status-indicator disconnected';
        statusIndicator.innerHTML = `
            <span class="status-dot"></span>
            <span class="status-text">❌ API 연결 실패 (API Connection Failed)</span>
        `;

        // 사용자에게 에러 표시 (Show error to user)
        showError('백엔드 API에 연결할 수 없습니다. 서버가 실행 중인지 확인하세요. (Cannot connect to backend API. Check if server is running.)');
    }
}


/*
    ===========================
    할일 목록 로드 (READ)
    Load To-Do List (READ)
    ===========================

    REST API의 GET 요청을 사용하여 모든 할일을 조회
    Use REST API's GET request to retrieve all to-dos

    HTTP 요청 예시 (HTTP Request Example):
    GET http://localhost:5000/api/todos
    GET http://localhost:5000/api/todos?priority=2&search=회의

    응답 예시 (Response Example):
    [
        { "id": 1, "title": "공부하기", "isCompleted": false },
        { "id": 2, "title": "운동하기", "isCompleted": true }
    ]

    파라미터 (Parameters):
    - priority: 우선순위 필터 (0=낮음, 1=보통, 2=높음, 3=긴급)
    - search: 제목 검색어
*/
async function loadTodos(filters = {}) {
    console.log('📥 할일 목록 로드 중... (Loading to-do list...)');

    try {
        /*
            쿼리 파라미터 생성 (Build query parameters)
            URLSearchParams: URL 쿼리 문자열을 쉽게 만들어주는 API
        */
        const params = new URLSearchParams();

        if (filters.priority !== undefined && filters.priority !== null && filters.priority !== '') {
            params.append('priority', filters.priority);
        }

        if (filters.search && filters.search.trim() !== '') {
            params.append('search', filters.search.trim());
        }

        const queryString = params.toString();
        const url = queryString ? `${API_BASE_URL}?${queryString}` : API_BASE_URL;

        console.log(`🔍 API 요청 URL: ${url}`);

        /*
            fetch() 호출 과정 (fetch() call process):
            1. HTTP GET 요청을 API 서버로 전송
               Send HTTP GET request to API server

            2. 서버가 데이터베이스에서 데이터 조회
               Server retrieves data from database

            3. 서버가 JSON 형식으로 응답
               Server responds in JSON format

            4. fetch()가 Response 객체 반환
               fetch() returns Response object
        */
        const response = await fetch(url, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        /*
            응답 상태 확인 (Check response status)
        */
        if (!response.ok) {
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        /*
            response.json(): JSON 문자열을 JavaScript 객체로 변환
            response.json(): Convert JSON string to JavaScript object

            JSON (JavaScript Object Notation):
            - 데이터를 텍스트 형식으로 표현하는 방법
            - Method to represent data in text format
            - 예: {"name": "홍길동", "age": 20}
        */
        const todos = await response.json();

        console.log(`✅ ${todos.length}개의 할일을 불러왔습니다. (Loaded ${todos.length} to-dos)`);

        /*
            전역 변수에 저장 (Store in global variable)
            필터링 시 재사용하기 위해 (For reuse during filtering)
        */
        allTodos = todos;

        /*
            UI에 할일 목록 표시 (Display to-do list in UI)
        */
        renderTodos(todos);

        /*
            카운트 업데이트 (Update counts)
        */
        updateCounts(todos);

    } catch (error) {
        console.error('❌ 할일 로드 실패:', error);
        showError(`할일을 불러오는데 실패했습니다: ${error.message} (Failed to load to-dos)`);
    }
}


/*
    ===========================
    할일 완료 상태 토글 (TOGGLE COMPLETION)
    Toggle To-Do Completion Status
    ===========================

    체크박스 클릭 시 호출되어 완료 상태를 반전
    Called when checkbox is clicked to toggle completion status
*/
async function toggleTodo(id) {
    console.log(`🔄 할일 완료 상태 토글 중 (ID: ${id})`);

    try {
        /*
            현재 할일 찾기 (Find current to-do)

            Array.find(): 배열에서 조건에 맞는 첫 번째 요소 반환
            Array.find(): Return first element matching condition in array
        */
        const todo = allTodos.find(t => t.id === id);

        if (!todo) {
            throw new Error('할일을 찾을 수 없습니다. (To-do not found)');
        }

        /*
            완료 상태 반전 (Toggle completion status)
            ! 연산자: 논리 부정 (true → false, false → true)
            ! operator: Logical NOT (true → false, false → true)
        */
        const newStatus = !todo.isCompleted;

        /*
            PUT 요청으로 서버 업데이트
            Update server with PUT request
        */
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                id: id,
                title: todo.title,
                isCompleted: newStatus
            })
        });

        if (!response.ok) {
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        console.log(`✅ 할일 상태 변경: ${newStatus ? '완료' : '미완료'}`);

        /*
            목록 새로고침 (Refresh list)
        */
        await loadTodos();

    } catch (error) {
        console.error('❌ 상태 변경 실패:', error);
        showError(`상태 변경 실패: ${error.message} (Failed to change status)`);
    }
}

/*
    ===========================
    할일 삭제 (DELETE)
    Delete To-Do (DELETE)
    ===========================

    REST API의 DELETE 요청을 사용하여 할일 삭제
    Use REST API's DELETE request to delete to-do

    HTTP 요청 예시 (HTTP Request Example):
    DELETE http://localhost:5000/api/todos/1
*/
async function deleteTodo(id) {
    /*
        confirm(): 사용자에게 확인 대화상자 표시
        confirm(): Show confirmation dialog to user
        - 확인: true 반환 (OK: returns true)
        - 취소: false 반환 (Cancel: returns false)
    */
    if (!confirm('정말로 이 항목을 삭제하시겠습니까? (Are you sure you want to delete this item?)')) {
        return;  // 취소하면 함수 종료 (Exit function if cancelled)
    }

    console.log(`🗑️ 할일 삭제 중 (ID: ${id})`);

    try {
        /*
            DELETE 요청 보내기 (Send DELETE request)
        */
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json'
            }
        });

        /*
            204 No Content: 성공적으로 삭제되었지만 응답 본문 없음
            204 No Content: Successfully deleted but no response body
        */
        if (!response.ok && response.status !== 204) {
            if (response.status === 404) {
                throw new Error('할일을 찾을 수 없습니다. (To-do not found)');
            }
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        console.log('✅ 할일 삭제 성공 (Delete successful)');
        showSuccess('할일이 삭제되었습니다! (To-do deleted!)');

        /*
            목록 새로고침 (Refresh list)
        */
        await loadTodos();

    } catch (error) {
        console.error('❌ 할일 삭제 실패:', error);
        showError(`할일 삭제 실패: ${error.message} (Failed to delete to-do)`);
    }
}

/*
    ===========================
    검색 및 필터 적용
    Apply Search and Filters
    ===========================

    사용자가 입력한 검색어와 우선순위 필터를 적용하여 할일 목록 다시 로드
    Reload to-do list with user-entered search term and priority filter
*/
function applyFilters() {
    console.log('🔍 검색 및 필터 적용 중... (Applying search and filters...)');

    // 검색 입력값 가져오기 (Get search input value)
    const searchInput = document.getElementById('searchInput');
    const searchValue = searchInput ? searchInput.value.trim() : '';

    // 우선순위 필터값 가져오기 (Get priority filter value)
    const prioritySelect = document.getElementById('priorityFilter');
    const priorityValue = prioritySelect ? prioritySelect.value : '';

    console.log(`검색어: "${searchValue}", 우선순위: "${priorityValue}"`);

    // 필터 객체 생성 (Create filter object)
    const filters = {};

    if (searchValue !== '') {
        filters.search = searchValue;
    }

    if (priorityValue !== '') {
        filters.priority = parseInt(priorityValue);
    }

    // 클리어 버튼 표시/숨김 (Show/hide clear button)
    const clearButton = document.getElementById('clearButton');
    if (clearButton) {
        if (searchValue !== '' || priorityValue !== '') {
            clearButton.style.display = 'inline-block';
        } else {
            clearButton.style.display = 'none';
        }
    }

    // 필터 적용하여 할일 목록 다시 로드 (Reload to-do list with filters)
    loadTodos(filters);
}

/*
    ===========================
    검색 및 필터 초기화
    Clear Search and Filters
    ===========================

    모든 검색 및 필터를 초기화하고 전체 목록 다시 로드
    Reset all search and filters and reload full list
*/
function clearFilters() {
    console.log('🔄 검색 및 필터 초기화... (Clearing search and filters...)');

    // 검색 입력 초기화 (Reset search input)
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.value = '';
    }

    // 우선순위 필터 초기화 (Reset priority filter)
    const prioritySelect = document.getElementById('priorityFilter');
    if (prioritySelect) {
        prioritySelect.value = '';
    }

    // 클리어 버튼 숨김 (Hide clear button)
    const clearButton = document.getElementById('clearButton');
    if (clearButton) {
        clearButton.style.display = 'none';
    }

    // 필터 없이 전체 목록 다시 로드 (Reload full list without filters)
    loadTodos();
}

/*
    ===========================
    상세 페이지로 이동
    Navigate to Detail Page
    ===========================
*/
function viewTodoDetail(id, event) {
    if (event) event.stopPropagation();
    console.log(`👁️ 상세 페이지로 이동 (ID: ${id})`);
    window.location.href = `index.php?page=detail&id=${id}`;
}

/*
    ===========================
    수정 페이지로 이동
    Navigate to Edit Page
    ===========================
*/
function editTodo(id, event) {
    if (event) event.stopPropagation();
    console.log(`✏️ 수정 페이지로 이동 (ID: ${id})`);
    window.location.href = `index.php?page=form&id=${id}`;
}

/*
    ===========================
    우선순위 뱃지 생성
    Create Priority Badge
    ===========================
*/
function getPriorityBadge(priority) {
    const badges = {
        0: '<span class="priority-badge priority-low">낮음</span>',
        1: '<span class="priority-badge priority-normal">보통</span>',
        2: '<span class="priority-badge priority-high">높음</span>',
        3: '<span class="priority-badge priority-urgent">긴급</span>'
    };
    return badges[priority] || badges[1];
}

/*
    ===========================
    날짜 포맷팅
    Format Date
    ===========================
*/
function formatDate(date) {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
}

/*
    ===========================
    필터링
    Filtering
    ===========================

    할일을 상태별로 필터링하여 표시
    Filter and display to-dos by status
*/
function filterTodos(filter) {
    console.log(`🔍 필터 적용: ${filter}`);

    currentFilter = filter;  // 현재 필터 저장 (Save current filter)

    /*
        모든 필터 버튼에서 active 클래스 제거
        Remove active class from all filter buttons

        querySelectorAll(): 모든 일치하는 요소 반환
        querySelectorAll(): Return all matching elements

        forEach(): 배열의 각 요소에 대해 함수 실행
        forEach(): Execute function for each element in array
    */
    document.querySelectorAll('.filter-btn').forEach(btn => {
        btn.classList.remove('active');
    });

    /*
        클릭된 필터 버튼에 active 클래스 추가
        Add active class to clicked filter button
    */
    const activeBtn = document.querySelector(`[data-filter="${filter}"]`);
    if (activeBtn) {
        activeBtn.classList.add('active');
    }

    /*
        필터에 따라 할일 표시/숨김
        Show/hide to-dos based on filter
    */
    const todoItems = document.querySelectorAll('.todo-item');

    todoItems.forEach(item => {
        const isCompleted = item.classList.contains('completed');

        /*
            필터 로직 (Filter logic):
            - all: 모두 표시 (Show all)
            - active: 완료되지 않은 것만 (Only incomplete)
            - completed: 완료된 것만 (Only completed)
        */
        if (filter === 'all') {
            item.classList.remove('hidden');
        } else if (filter === 'active') {
            if (isCompleted) {
                item.classList.add('hidden');
            } else {
                item.classList.remove('hidden');
            }
        } else if (filter === 'completed') {
            if (isCompleted) {
                item.classList.remove('hidden');
            } else {
                item.classList.add('hidden');
            }
        }
    });
}

/*
    ===========================
    UI 렌더링
    UI Rendering
    ===========================

    할일 데이터를 받아서 HTML로 변환하여 화면에 표시
    Take to-do data, convert to HTML, and display on screen

    DOM 조작 (DOM Manipulation):
    - DOM: Document Object Model
    - HTML 요소를 JavaScript로 생성, 수정, 삭제
    - Create, modify, delete HTML elements with JavaScript
*/
function renderTodos(todos) {
    console.log(`🎨 UI 렌더링 중... ${todos.length}개 항목 (Rendering UI... ${todos.length} items)`);

    /*
        할일 목록을 표시할 컨테이너
        Container to display to-do list
    */
    const todoList = document.getElementById('todoList');

    /*
        할일이 없는 경우 (When there are no to-dos)
    */
    if (todos.length === 0) {
        todoList.innerHTML = `
            <div class="empty-state">
                <div class="empty-state-icon">📝</div>
                <h3>할일이 없습니다 (No to-dos)</h3>
                <p>위에서 새로운 할일을 추가해보세요! (Add a new to-do above!)</p>
            </div>
        `;
        return;
    }

    /*
        할일 목록을 HTML 문자열로 변환
        Convert to-do list to HTML string

        map(): 배열의 각 요소를 변환하여 새 배열 생성
        map(): Transform each element of array to create new array

        join(): 배열의 요소들을 문자열로 결합
        join(): Combine array elements into string
    */
    const todosHtml = todos.map(todo => {
        // 우선순위 뱃지
        const priorityBadge = getPriorityBadge(todo.priority);

        // 마감일 표시
        let dueDateHtml = '';
        if (todo.dueDate) {
            const dueDate = new Date(todo.dueDate);
            const dateStr = formatDate(dueDate);
            const isOverdue = dueDate < new Date() && !todo.isCompleted;
            dueDateHtml = `<span class="todo-due-date ${isOverdue ? 'overdue' : ''}">${dateStr}</span>`;
        }

        // 카테고리 표시
        const categoryHtml = todo.category
            ? `<span class="todo-category">${escapeHtml(todo.category)}</span>`
            : '';

        return `
        <div class="todo-item ${todo.isCompleted ? 'completed' : ''}" data-id="${todo.id}" onclick="viewTodoDetail(${todo.id}, event)">
            <div class="todo-content">
                <input
                    type="checkbox"
                    class="todo-checkbox"
                    ${todo.isCompleted ? 'checked' : ''}
                    onchange="toggleTodo(${todo.id})"
                    onclick="event.stopPropagation()"
                >
                <div class="todo-info">
                    <span class="todo-text">${escapeHtml(todo.title)}</span>
                    <div class="todo-meta">
                        ${priorityBadge}
                        ${categoryHtml}
                        ${dueDateHtml}
                    </div>
                </div>
            </div>
            <div class="todo-actions" onclick="event.stopPropagation()">
                <button
                    class="btn-icon-action btn-view"
                    onclick="viewTodoDetail(${todo.id}, event)"
                    title="상세보기 (View)"
                >
                    👁️
                </button>
                <button
                    class="btn-icon-action btn-edit"
                    onclick="editTodo(${todo.id}, event)"
                    title="수정 (Edit)"
                >
                    ✏️
                </button>
                <button
                    class="btn-icon-action btn-delete"
                    onclick="deleteTodo(${todo.id})"
                    title="삭제 (Delete)"
                >
                    🗑️
                </button>
            </div>
        </div>
    `;
    }).join('');

    /*
        innerHTML: 요소의 HTML 내용을 설정
        innerHTML: Set HTML content of element
    */
    todoList.innerHTML = todosHtml;

    /*
        현재 필터 재적용 (Reapply current filter)
    */
    if (currentFilter !== 'all') {
        filterTodos(currentFilter);
    }
}

/*
    ===========================
    카운트 업데이트
    Update Counts
    ===========================

    각 필터의 항목 개수를 업데이트
    Update item count for each filter
*/
function updateCounts(todos) {
    /*
        filter(): 조건에 맞는 요소들만 필터링
        filter(): Filter only elements matching condition
    */
    const totalCount = todos.length;
    const activeCount = todos.filter(t => !t.isCompleted).length;
    const completedCount = todos.filter(t => t.isCompleted).length;

    /*
        textContent: 요소의 텍스트 내용 설정
        textContent: Set text content of element

        innerHTML vs textContent:
        - innerHTML: HTML 태그 포함 (Includes HTML tags)
        - textContent: 순수 텍스트만 (Pure text only)
    */
    document.getElementById('countAll').textContent = totalCount;
    document.getElementById('countActive').textContent = activeCount;
    document.getElementById('countCompleted').textContent = completedCount;
}

/*
    ===========================
    에러 메시지 표시
    Show Error Message
    ===========================

    사용자에게 에러를 시각적으로 알림
    Visually notify user of error
*/
function showError(message) {
    const errorDiv = document.getElementById('errorMessage');
    errorDiv.textContent = message;
    errorDiv.style.display = 'block';  // 표시 (Show)

    /*
        setTimeout(): 일정 시간 후 함수 실행
        setTimeout(): Execute function after certain time

        5000ms = 5초 후 자동으로 숨김
        Hide automatically after 5000ms = 5 seconds
    */
    setTimeout(() => {
        errorDiv.style.display = 'none';  // 숨김 (Hide)
    }, 5000);
}

/*
    ===========================
    성공 메시지 표시
    Show Success Message
    ===========================
*/
function showSuccess(message) {
    const successDiv = document.getElementById('successMessage');
    successDiv.textContent = message;
    successDiv.style.display = 'block';

    setTimeout(() => {
        successDiv.style.display = 'none';
    }, 3000);  // 3초 후 숨김 (Hide after 3 seconds)
}

/*
    ===========================
    HTML 이스케이프
    HTML Escape
    ===========================

    보안을 위해 HTML 특수 문자를 이스케이프
    Escape HTML special characters for security

    XSS 공격 방지 (Prevent XSS attacks):
    - 사용자 입력에 <script> 등이 있으면 실행되지 않도록 함
    - Prevent execution if user input contains <script> etc.
*/
function escapeHtml(text) {
    /*
        createElement(): 새 HTML 요소 생성
        createElement(): Create new HTML element
    */
    const div = document.createElement('div');
    div.textContent = text;  // textContent는 자동으로 이스케이프 (textContent auto-escapes)
    return div.innerHTML;
}

/*
    ===========================
    디버그 정보 (개발용)
    Debug Information (For Development)
    ===========================

    개발 중 유용한 정보를 콘솔에 출력
    Output useful information to console during development
*/
console.log('📱 애플리케이션 정보 (Application Info):');
console.log(`- API URL: ${API_BASE_URL}`);
console.log(`- JavaScript 버전: ES6+ (async/await, arrow functions, etc.)`);
console.log(`- 주요 기능: fetch API, DOM Manipulation, Event Handling`);
console.log('💡 개발자 도구(F12)를 열어 네트워크 탭에서 API 요청을 확인할 수 있습니다.');
console.log('💡 Open DevTools (F12) and check Network tab to see API requests.');
