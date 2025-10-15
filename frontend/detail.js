/*
    ===========================
    Todo 상세 페이지 JavaScript
    Todo Detail Page JavaScript
    ===========================

    주요 기능:
    - URL 파라미터에서 ID 추출
    - API에서 상세 정보 조회
    - 화면에 정보 표시
    - 완료 상태 토글
    - 수정/삭제 기능
*/

const API_BASE_URL = 'http://localhost:5000/api/todos';

// 현재 표시 중인 Todo 데이터
let currentTodo = null;

/*
    ===========================
    초기화
    ===========================
*/
document.addEventListener('DOMContentLoaded', function() {
    console.log('📄 상세 페이지 초기화 중... (Initializing detail page...)');

    // URL에서 ID 추출
    const todoId = getTodoIdFromUrl();

    if (!todoId) {
        showError('할일 ID가 제공되지 않았습니다. (No todo ID provided)');
        return;
    }

    // 상세 정보 로드
    loadTodoDetail(todoId);
});

/*
    ===========================
    URL에서 Todo ID 추출
    ===========================

    예: detail.html?id=5 → 5 반환
*/
function getTodoIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');

    console.log(`🔍 URL에서 추출한 ID: ${id}`);

    return id ? parseInt(id) : null;
}

/*
    ===========================
    Todo 상세 정보 로드
    ===========================
*/
async function loadTodoDetail(id) {
    console.log(`📥 Todo 상세 정보 로드 중... ID: ${id}`);

    const loadingIndicator = document.getElementById('loadingIndicator');
    const detailCard = document.getElementById('detailCard');

    try {
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            if (response.status === 404) {
                throw new Error('할일을 찾을 수 없습니다. (Todo not found)');
            }
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        const todo = await response.json();
        console.log('✅ Todo 로드 성공:', todo);

        // 전역 변수에 저장
        currentTodo = todo;

        // 화면에 표시
        displayTodoDetail(todo);

        // 로딩 숨기고 카드 표시
        loadingIndicator.style.display = 'none';
        detailCard.style.display = 'block';

    } catch (error) {
        console.error('❌ Todo 로드 실패:', error);
        showError(`할일을 불러오는데 실패했습니다: ${error.message}`);
        loadingIndicator.style.display = 'none';
    }
}

/*
    ===========================
    Todo 상세 정보 화면에 표시
    ===========================
*/
function displayTodoDetail(todo) {
    // 제목
    const titleElement = document.getElementById('todoTitle');
    titleElement.textContent = todo.title;
    if (todo.isCompleted) {
        titleElement.classList.add('completed');
    }

    // 우선순위 뱃지
    const priorityBadge = document.getElementById('priorityBadge');
    const priorityText = getPriorityText(todo.priority);
    priorityBadge.textContent = priorityText;
    priorityBadge.className = `priority-badge priority-${getPriorityClass(todo.priority)}`;

    // 상태 뱃지
    const statusBadge = document.getElementById('statusBadge');
    statusBadge.textContent = todo.isCompleted ? '✅ 완료됨' : '⏳ 진행중';
    statusBadge.className = `status-badge ${todo.isCompleted ? 'completed' : 'active'}`;

    // 마감일
    const dueDateElement = document.getElementById('dueDate');
    if (todo.dueDate) {
        dueDateElement.textContent = formatDate(todo.dueDate);
        dueDateElement.classList.remove('empty');

        // 마감일 임박 표시
        if (isOverdue(todo.dueDate) && !todo.isCompleted) {
            dueDateElement.style.color = '#c62828';
            dueDateElement.textContent += ' ⚠️ 마감 임박!';
        }
    } else {
        dueDateElement.textContent = '마감일 없음';
        dueDateElement.classList.add('empty');
    }

    // 카테고리
    const categoryElement = document.getElementById('category');
    if (todo.category) {
        categoryElement.textContent = todo.category;
        categoryElement.classList.remove('empty');
    } else {
        categoryElement.textContent = '카테고리 없음';
        categoryElement.classList.add('empty');
    }

    // 생성일
    const createdAtElement = document.getElementById('createdAt');
    createdAtElement.textContent = formatDateTime(todo.createdAt);

    // 수정일
    const updatedAtElement = document.getElementById('updatedAt');
    updatedAtElement.textContent = formatDateTime(todo.updatedAt);

    // 상세 설명
    const descriptionElement = document.getElementById('description');
    if (todo.description && todo.description.trim()) {
        descriptionElement.textContent = todo.description;
        descriptionElement.classList.remove('empty');
    } else {
        descriptionElement.textContent = '상세 설명 없음';
        descriptionElement.classList.add('empty');
    }

    // 태그
    const tagsContainer = document.getElementById('tagsContainer');
    if (todo.tags && todo.tags.trim()) {
        try {
            const tagsArray = JSON.parse(todo.tags);
            if (tagsArray && tagsArray.length > 0) {
                tagsContainer.innerHTML = tagsArray
                    .map(tag => `<span class="tag">${escapeHtml(tag)}</span>`)
                    .join('');
            } else {
                tagsContainer.innerHTML = '<span class="info-value empty">태그 없음</span>';
            }
        } catch (error) {
            console.error('태그 파싱 실패:', error);
            tagsContainer.innerHTML = '<span class="info-value empty">태그 없음</span>';
        }
    } else {
        tagsContainer.innerHTML = '<span class="info-value empty">태그 없음</span>';
    }

    // 완료 토글 버튼
    const toggleBtn = document.getElementById('toggleCompleteBtn');
    if (todo.isCompleted) {
        toggleBtn.textContent = '↩️ 미완료로 변경 (Mark Incomplete)';
        toggleBtn.className = 'btn btn-incomplete';
    } else {
        toggleBtn.textContent = '✅ 완료 처리 (Mark Complete)';
        toggleBtn.className = 'btn btn-complete';
    }
}

/*
    ===========================
    완료 상태 토글
    ===========================
*/
async function toggleComplete() {
    if (!currentTodo) return;

    const newStatus = !currentTodo.isCompleted;
    console.log(`🔄 완료 상태 토글: ${currentTodo.isCompleted} → ${newStatus}`);

    try {
        const response = await fetch(`${API_BASE_URL}/${currentTodo.id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Accept': 'application/json'
            },
            body: JSON.stringify({
                ...currentTodo,
                isCompleted: newStatus
            })
        });

        if (!response.ok) {
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        console.log('✅ 상태 변경 성공');

        // 페이지 새로고침
        loadTodoDetail(currentTodo.id);

    } catch (error) {
        console.error('❌ 상태 변경 실패:', error);
        showError(`상태 변경 실패: ${error.message}`);
    }
}

/*
    ===========================
    수정 페이지로 이동
    ===========================
*/
function editTodo() {
    if (!currentTodo) return;

    // 수정 페이지로 이동 (ID 전달)
    window.location.href = `index.php?page=form&id=${currentTodo.id}`;
}

/*
    ===========================
    Todo 삭제
    ===========================
*/
async function deleteTodo() {
    if (!currentTodo) return;

    if (!confirm('정말로 이 할일을 삭제하시겠습니까? (Are you sure you want to delete this todo?)')) {
        return;
    }

    console.log(`🗑️ Todo 삭제 중... ID: ${currentTodo.id}`);

    try {
        const response = await fetch(`${API_BASE_URL}/${currentTodo.id}`, {
            method: 'DELETE',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok && response.status !== 204) {
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        console.log('✅ Todo 삭제 성공');

        // 목록 페이지로 이동
        alert('할일이 삭제되었습니다! (Todo deleted!)');
        window.location.href = 'index.php';

    } catch (error) {
        console.error('❌ Todo 삭제 실패:', error);
        showError(`삭제 실패: ${error.message}`);
    }
}

/*
    ===========================
    유틸리티 함수들
    ===========================
*/

// 우선순위 텍스트 반환
function getPriorityText(priority) {
    const priorities = {
        0: '낮음 (Low)',
        1: '보통 (Normal)',
        2: '높음 (High)',
        3: '긴급 (Urgent)'
    };
    return priorities[priority] || '보통';
}

// 우선순위 CSS 클래스 반환
function getPriorityClass(priority) {
    const classes = {
        0: 'low',
        1: 'normal',
        2: 'high',
        3: 'urgent'
    };
    return classes[priority] || 'normal';
}

// 날짜 포맷팅 (날짜만)
function formatDate(dateString) {
    if (!dateString) return '';

    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
}

// 날짜 포맷팅 (날짜 + 시간)
function formatDateTime(dateString) {
    if (!dateString) return '';

    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');

    return `${year}-${month}-${day} ${hours}:${minutes}`;
}

// 마감일이 지났는지 확인
function isOverdue(dueDateString) {
    if (!dueDateString) return false;

    const dueDate = new Date(dueDateString);
    const now = new Date();

    return dueDate < now;
}

// HTML 이스케이프 (XSS 방지)
function escapeHtml(text) {
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

// 에러 메시지 표시
function showError(message) {
    const errorDiv = document.getElementById('errorMessage');
    errorDiv.textContent = message;
    errorDiv.style.display = 'block';

    setTimeout(() => {
        errorDiv.style.display = 'none';
    }, 5000);
}
