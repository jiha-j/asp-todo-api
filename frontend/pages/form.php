<?php
/**
 * ===========================
 * 할일 생성/수정 폼 페이지 (To-Do Create/Edit Form Page)
 * ===========================
 *
 * 새로운 할일을 생성하거나 기존 할일을 수정하는 페이지
 * Page for creating new to-do or editing existing to-do
 */

// 페이지 설정 (Page configuration)
$pageTitle = '할일 작성/수정';
$jsFiles = ['form.js'];

// 인라인 스타일 설정 (Inline styles configuration)
$inlineStyles = '
/* 폼 페이지 전용 스타일 */
.form-container {
    max-width: 800px;
    margin: 0 auto;
    padding: 20px;
}

.back-button {
    display: inline-flex;
    align-items: center;
    gap: 8px;
    padding: 10px 20px;
    background-color: #f0f0f0;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    font-size: 16px;
    margin-bottom: 20px;
    text-decoration: none;
    color: #333;
    transition: background-color 0.3s;
}

.back-button:hover {
    background-color: #e0e0e0;
}

.form-card {
    background: white;
    border-radius: 12px;
    padding: 30px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.form-header {
    margin-bottom: 30px;
    padding-bottom: 20px;
    border-bottom: 2px solid #f0f0f0;
}

.form-title {
    font-size: 28px;
    font-weight: bold;
    margin: 0;
    color: #333;
}

.form-group {
    margin-bottom: 24px;
}

.form-label {
    display: block;
    font-size: 14px;
    font-weight: 600;
    color: #333;
    margin-bottom: 8px;
}

.form-label.required::after {
    content: " *";
    color: #c62828;
}

.form-input,
.form-textarea,
.form-select {
    width: 100%;
    padding: 12px 16px;
    font-size: 16px;
    border: 2px solid #e0e0e0;
    border-radius: 8px;
    transition: border-color 0.3s;
    font-family: inherit;
}

.form-input:focus,
.form-textarea:focus,
.form-select:focus {
    outline: none;
    border-color: #2196f3;
}

.form-textarea {
    resize: vertical;
    min-height: 120px;
}

.form-hint {
    font-size: 12px;
    color: #666;
    margin-top: 4px;
}

.priority-options {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(120px, 1fr));
    gap: 12px;
}

.priority-option {
    position: relative;
}

.priority-option input[type="radio"] {
    display: none;
}

.priority-label {
    display: block;
    padding: 12px;
    text-align: center;
    border: 2px solid #e0e0e0;
    border-radius: 8px;
    cursor: pointer;
    transition: all 0.3s;
}

.priority-option input[type="radio"]:checked + .priority-label {
    border-color: #2196f3;
    background-color: #e3f2fd;
    font-weight: 600;
}

.priority-label:hover {
    background-color: #f5f5f5;
}

.tags-input-container {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
    padding: 8px;
    border: 2px solid #e0e0e0;
    border-radius: 8px;
    min-height: 48px;
}

.tags-input-container:focus-within {
    border-color: #2196f3;
}

.tag-item {
    display: inline-flex;
    align-items: center;
    gap: 6px;
    padding: 6px 12px;
    background: #e0e0e0;
    color: #333;
    border-radius: 16px;
    font-size: 14px;
}

.tag-remove {
    cursor: pointer;
    font-weight: bold;
    color: #666;
}

.tag-remove:hover {
    color: #333;
}

.tag-input {
    flex: 1;
    min-width: 120px;
    border: none;
    outline: none;
    padding: 8px;
    font-size: 14px;
}

.form-actions {
    display: flex;
    gap: 12px;
    justify-content: flex-end;
    padding-top: 24px;
    border-top: 2px solid #f0f0f0;
}

.btn {
    padding: 12px 32px;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    font-size: 16px;
    font-weight: 600;
    transition: all 0.3s;
}

.btn-primary {
    background: #2196f3;
    color: white;
}

.btn-primary:hover {
    background: #1976d2;
}

.btn-secondary {
    background: #f0f0f0;
    color: #333;
}

.btn-secondary:hover {
    background: #e0e0e0;
}

.loading {
    text-align: center;
    padding: 60px 20px;
}

.error-message,
.success-message {
    padding: 16px;
    border-radius: 8px;
    margin-bottom: 20px;
}

.error-message {
    background: #ffebee;
    color: #c62828;
}

.success-message {
    background: #e8f5e9;
    color: #2e7d32;
}
';

// 헤더 포함 (Include header)
include LAYOUT_PATH . '/header.php';
?>

<div class="form-container">
    <!-- 뒤로가기 버튼 -->
    <a href="<?= buildUrl('list') ?>" class="back-button">
        ← 목록으로 돌아가기 (Back to List)
    </a>

    <!-- 메시지 영역 -->
    <div id="errorMessage" class="error-message" style="display: none;"></div>
    <div id="successMessage" class="success-message" style="display: none;"></div>

    <!-- 로딩 표시 -->
    <div id="loadingIndicator" class="loading" style="display: none;">
        <div class="spinner"></div>
        <p>데이터 로드 중... (Loading data...)</p>
    </div>

    <!-- 폼 카드 -->
    <div id="formCard" class="form-card">
        <!-- 헤더 -->
        <div class="form-header">
            <h1 id="formTitle" class="form-title">새 할일 만들기 (Create New Todo)</h1>
        </div>

        <!-- 폼 -->
        <form id="todoForm">
            <!-- 제목 -->
            <div class="form-group">
                <label for="title" class="form-label required">제목 (Title)</label>
                <input
                    type="text"
                    id="title"
                    class="form-input"
                    placeholder="할일 제목을 입력하세요..."
                    required
                    maxlength="200"
                >
                <div class="form-hint">최대 200자까지 입력 가능합니다.</div>
            </div>

            <!-- 우선순위 -->
            <div class="form-group">
                <label class="form-label">우선순위 (Priority)</label>
                <div class="priority-options">
                    <div class="priority-option">
                        <input type="radio" id="priority-low" name="priority" value="0">
                        <label for="priority-low" class="priority-label">낮음 (Low)</label>
                    </div>
                    <div class="priority-option">
                        <input type="radio" id="priority-normal" name="priority" value="1" checked>
                        <label for="priority-normal" class="priority-label">보통 (Normal)</label>
                    </div>
                    <div class="priority-option">
                        <input type="radio" id="priority-high" name="priority" value="2">
                        <label for="priority-high" class="priority-label">높음 (High)</label>
                    </div>
                    <div class="priority-option">
                        <input type="radio" id="priority-urgent" name="priority" value="3">
                        <label for="priority-urgent" class="priority-label">긴급 (Urgent)</label>
                    </div>
                </div>
            </div>

            <!-- 마감일 -->
            <div class="form-group">
                <label for="dueDate" class="form-label">마감일 (Due Date)</label>
                <input
                    type="datetime-local"
                    id="dueDate"
                    class="form-input"
                    value="<?= date('Y-m-d\T18:00', strtotime('+1 day')) ?>"
                >
                <div class="form-hint">마감일을 설정하지 않으려면 비워두세요.</div>
            </div>

            <!-- 카테고리 -->
            <div class="form-group">
                <label for="category" class="form-label">카테고리 (Category)</label>
                <input
                    type="text"
                    id="category"
                    class="form-input"
                    placeholder="예: 업무, 개인, 쇼핑..."
                    maxlength="50"
                >
                <div class="form-hint">최대 50자까지 입력 가능합니다.</div>
            </div>

            <!-- 태그 -->
            <div class="form-group">
                <label for="tagInput" class="form-label">태그 (Tags)</label>
                <div class="tags-input-container" id="tagsContainer">
                    <input
                        type="text"
                        id="tagInput"
                        class="tag-input"
                        placeholder="태그를 입력하고 Enter를 누르세요..."
                    >
                </div>
                <div class="form-hint">Enter 키를 눌러 태그를 추가합니다.</div>
            </div>

            <!-- 상세 설명 -->
            <div class="form-group">
                <label for="description" class="form-label">상세 설명 (Description)</label>
                <textarea
                    id="description"
                    class="form-textarea"
                    placeholder="할일에 대한 자세한 설명을 입력하세요..."
                    maxlength="1000"
                ></textarea>
                <div class="form-hint">최대 1000자까지 입력 가능합니다.</div>
            </div>

            <!-- 액션 버튼 -->
            <div class="form-actions">
                <button type="button" class="btn btn-secondary" onclick="goBack()">
                    취소 (Cancel)
                </button>
                <button type="submit" class="btn btn-primary">
                    <span id="submitButtonText">저장 (Save)</span>
                </button>
            </div>
        </form>
    </div>
</div>

<?php
// 푸터 포함 (Include footer)
include LAYOUT_PATH . '/footer.php';
?>
