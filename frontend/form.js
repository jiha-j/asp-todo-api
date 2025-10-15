/*
    ===========================
    Todo 폼 페이지 JavaScript
    Todo Form Page JavaScript
    ===========================

    주요 기능:
    - 생성/수정 모드 자동 감지
    - 폼 데이터 로드 및 저장
    - 태그 동적 관리
    - 유효성 검사
*/

const API_BASE_URL = 'http://localhost:5000/api/todos';

// 현재 모드 (create 또는 edit)
let currentMode = 'create';
let currentTodoId = null;
let tags = []; // 태그 배열

/*
    ===========================
    초기화
    ===========================
*/
document.addEventListener('DOMContentLoaded', function() {
    console.log('📝 폼 페이지 초기화 중... (Initializing form page...)');

    // URL에서 ID 확인
    const todoId = getTodoIdFromUrl();

    if (todoId) {
        // 수정 모드
        currentMode = 'edit';
        currentTodoId = todoId;
        document.getElementById('formTitle').textContent = '할일 수정 (Edit Todo)';
        document.getElementById('submitButtonText').textContent = '수정 (Update)';

        // 기존 데이터 로드
        loadTodoData(todoId);
    } else {
        // 생성 모드
        currentMode = 'create';
        console.log('🆕 생성 모드 (Create mode)');
    }

    // 이벤트 리스너 설정
    setupEventListeners();
});

/*
    ===========================
    이벤트 리스너 설정
    ===========================
*/
function setupEventListeners() {
    // 폼 제출
    document.getElementById('todoForm').addEventListener('submit', handleFormSubmit);

    // 태그 입력
    const tagInput = document.getElementById('tagInput');
    tagInput.addEventListener('keydown', handleTagInput);
}

/*
    ===========================
    URL에서 Todo ID 추출
    ===========================
*/
function getTodoIdFromUrl() {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    return id ? parseInt(id) : null;
}

/*
    ===========================
    Todo 데이터 로드 (수정 모드)
    ===========================
*/
async function loadTodoData(id) {
    console.log(`📥 Todo 데이터 로드 중... ID: ${id}`);

    const loadingIndicator = document.getElementById('loadingIndicator');
    const formCard = document.getElementById('formCard');

    loadingIndicator.style.display = 'block';
    formCard.style.display = 'none';

    try {
        const response = await fetch(`${API_BASE_URL}/${id}`, {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        const todo = await response.json();
        console.log('✅ Todo 로드 성공:', todo);

        // 폼 필드 채우기
        populateForm(todo);

        loadingIndicator.style.display = 'none';
        formCard.style.display = 'block';

    } catch (error) {
        console.error('❌ Todo 로드 실패:', error);
        showError(`데이터 로드 실패: ${error.message}`);
        loadingIndicator.style.display = 'none';
        formCard.style.display = 'block';
    }
}

/*
    ===========================
    폼 필드 채우기 (수정 모드)
    ===========================
*/
function populateForm(todo) {
    // 제목
    document.getElementById('title').value = todo.title || '';

    // 우선순위
    const priorityRadio = document.querySelector(`input[name="priority"][value="${todo.priority}"]`);
    if (priorityRadio) {
        priorityRadio.checked = true;
    }

    // 마감일
    if (todo.dueDate) {
        // ISO 8601 형식을 datetime-local 형식으로 변환
        const dueDate = new Date(todo.dueDate);
        const year = dueDate.getFullYear();
        const month = String(dueDate.getMonth() + 1).padStart(2, '0');
        const day = String(dueDate.getDate()).padStart(2, '0');
        const hours = String(dueDate.getHours()).padStart(2, '0');
        const minutes = String(dueDate.getMinutes()).padStart(2, '0');

        document.getElementById('dueDate').value = `${year}-${month}-${day}T${hours}:${minutes}`;
    }

    // 카테고리
    document.getElementById('category').value = todo.category || '';

    // 태그
    if (todo.tags && todo.tags.trim()) {
        try {
            tags = JSON.parse(todo.tags);
            renderTags();
        } catch (error) {
            console.error('태그 파싱 실패:', error);
            tags = [];
        }
    }

    // 상세 설명
    document.getElementById('description').value = todo.description || '';
}

/*
    ===========================
    태그 입력 처리
    ===========================
*/
function handleTagInput(event) {
    if (event.key === 'Enter') {
        event.preventDefault();

        const input = event.target;
        const tagText = input.value.trim();

        if (tagText && !tags.includes(tagText)) {
            tags.push(tagText);
            input.value = '';
            renderTags();
        }
    }
}

/*
    ===========================
    태그 렌더링
    ===========================
*/
function renderTags() {
    const container = document.getElementById('tagsContainer');
    const tagInput = document.getElementById('tagInput');

    // 기존 태그 요소들 제거
    const existingTags = container.querySelectorAll('.tag-item');
    existingTags.forEach(tag => tag.remove());

    // 새 태그 요소들 추가
    tags.forEach((tag, index) => {
        const tagElement = document.createElement('div');
        tagElement.className = 'tag-item';
        tagElement.innerHTML = `
            ${escapeHtml(tag)}
            <span class="tag-remove" onclick="removeTag(${index})">×</span>
        `;

        // 입력 필드 앞에 삽입
        container.insertBefore(tagElement, tagInput);
    });
}

/*
    ===========================
    태그 제거
    ===========================
*/
function removeTag(index) {
    tags.splice(index, 1);
    renderTags();
}

/*
    ===========================
    폼 제출 처리
    ===========================
*/
async function handleFormSubmit(event) {
    event.preventDefault();

    console.log(`📤 폼 제출 중... 모드: ${currentMode}`);

    // 폼 데이터 수집
    const formData = collectFormData();

    // 유효성 검사
    if (!validateForm(formData)) {
        return;
    }

    try {
        let response;

        if (currentMode === 'create') {
            // 새 Todo 생성
            response = await fetch(API_BASE_URL, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(formData)
            });
        } else {
            // 기존 Todo 수정
            response = await fetch(`${API_BASE_URL}/${currentTodoId}`, {
                method: 'PUT',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify({
                    ...formData,
                    id: currentTodoId
                })
            });
        }

        if (!response.ok) {
            throw new Error(`HTTP 에러! 상태: ${response.status}`);
        }

        console.log(`✅ ${currentMode === 'create' ? '생성' : '수정'} 성공`);

        // 성공 메시지 표시
        const message = currentMode === 'create'
            ? '할일이 생성되었습니다! (Todo created!)'
            : '할일이 수정되었습니다! (Todo updated!)';

        showSuccess(message);

        // 잠시 후 목록 페이지로 이동
        setTimeout(() => {
            window.location.href = 'index.php';
        }, 1500);

    } catch (error) {
        console.error(`❌ ${currentMode === 'create' ? '생성' : '수정'} 실패:`, error);
        showError(`작업 실패: ${error.message}`);
    }
}

/*
    ===========================
    폼 데이터 수집
    ===========================
*/
function collectFormData() {
    const title = document.getElementById('title').value.trim();
    const priority = parseInt(document.querySelector('input[name="priority"]:checked').value);
    const dueDateInput = document.getElementById('dueDate').value;
    const category = document.getElementById('category').value.trim();
    const description = document.getElementById('description').value.trim();

    // 마감일 처리
    let dueDate = null;
    if (dueDateInput) {
        dueDate = new Date(dueDateInput).toISOString();
    }

    // 태그 JSON 변환
    const tagsJson = tags.length > 0 ? JSON.stringify(tags) : null;

    return {
        title: title,
        description: description || null,
        priority: priority,
        dueDate: dueDate,
        category: category || null,
        tags: tagsJson,
        isCompleted: false // 새 할일은 항상 미완료로 시작
    };
}

/*
    ===========================
    폼 유효성 검사
    ===========================
*/
function validateForm(formData) {
    // 제목 필수
    if (!formData.title || formData.title.length === 0) {
        showError('제목을 입력해주세요. (Please enter a title)');
        document.getElementById('title').focus();
        return false;
    }

    // 제목 길이 제한
    if (formData.title.length > 200) {
        showError('제목은 200자를 초과할 수 없습니다. (Title cannot exceed 200 characters)');
        document.getElementById('title').focus();
        return false;
    }

    return true;
}

/*
    ===========================
    뒤로가기
    ===========================
*/
function goBack() {
    if (confirm('작성 중인 내용이 저장되지 않습니다. 계속하시겠습니까? (Unsaved changes will be lost. Continue?)')) {
        if (currentMode === 'edit' && currentTodoId) {
            window.location.href = `index.php?page=detail&id=${currentTodoId}`;
        } else {
            window.location.href = 'index.php';
        }
    }
}

/*
    ===========================
    유틸리티 함수들
    ===========================
*/

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

    // 페이지 상단으로 스크롤
    window.scrollTo(0, 0);
}

// 성공 메시지 표시
function showSuccess(message) {
    const successDiv = document.getElementById('successMessage');
    successDiv.textContent = message;
    successDiv.style.display = 'block';

    setTimeout(() => {
        successDiv.style.display = 'none';
    }, 3000);

    // 페이지 상단으로 스크롤
    window.scrollTo(0, 0);
}
