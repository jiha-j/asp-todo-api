<?php
/**
 * ===========================
 * 할일 상세보기 페이지 (To-Do Detail Page)
 * ===========================
 *
 * 특정 할일의 모든 정보를 보여주는 페이지
 * Page displaying all information of a specific to-do
 */

// 페이지 설정 (Page configuration)
$pageTitle = '할일 상세보기';
$jsFiles = ['detail.js'];

// 인라인 스타일 설정 (Inline styles configuration)
$inlineStyles = '
/* 상세 페이지 전용 스타일 */
.detail-container {
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

.detail-card {
    background: white;
    border-radius: 12px;
    padding: 30px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
}

.detail-header {
    display: flex;
    justify-content: space-between;
    align-items: flex-start;
    margin-bottom: 24px;
    padding-bottom: 20px;
    border-bottom: 2px solid #f0f0f0;
}

.detail-title-section {
    flex: 1;
}

.detail-title {
    font-size: 28px;
    font-weight: bold;
    margin: 0 0 12px 0;
    color: #333;
}

.detail-title.completed {
    text-decoration: line-through;
    color: #999;
}

.priority-badge {
    display: inline-block;
    padding: 4px 12px;
    border-radius: 12px;
    font-size: 14px;
    font-weight: 600;
    margin-right: 8px;
}

.priority-low { background: #e3f2fd; color: #1976d2; }
.priority-normal { background: #f3e5f5; color: #7b1fa2; }
.priority-high { background: #fff3e0; color: #f57c00; }
.priority-urgent { background: #ffebee; color: #c62828; }

.status-badge {
    display: inline-block;
    padding: 4px 12px;
    border-radius: 12px;
    font-size: 14px;
    font-weight: 600;
}

.status-badge.completed { background: #c8e6c9; color: #2e7d32; }
.status-badge.active { background: #fff9c4; color: #f57f17; }

.detail-info-grid {
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
    gap: 20px;
    margin-bottom: 24px;
}

.info-item {
    padding: 16px;
    background: #f9f9f9;
    border-radius: 8px;
}

.info-label {
    font-size: 12px;
    color: #666;
    text-transform: uppercase;
    letter-spacing: 0.5px;
    margin-bottom: 8px;
}

.info-value {
    font-size: 16px;
    font-weight: 600;
    color: #333;
}

.info-value.empty {
    color: #999;
    font-style: italic;
}

.detail-description {
    margin-bottom: 24px;
}

.description-label {
    font-size: 14px;
    color: #666;
    margin-bottom: 8px;
    font-weight: 600;
}

.description-content {
    background: #f9f9f9;
    padding: 16px;
    border-radius: 8px;
    line-height: 1.6;
    color: #333;
    white-space: pre-wrap;
    min-height: 60px;
}

.description-content.empty {
    color: #999;
    font-style: italic;
}

.tags-container {
    display: flex;
    flex-wrap: wrap;
    gap: 8px;
}

.tag {
    display: inline-block;
    padding: 6px 12px;
    background: #e0e0e0;
    color: #333;
    border-radius: 16px;
    font-size: 14px;
}

.detail-actions {
    display: flex;
    gap: 12px;
    justify-content: flex-end;
    padding-top: 24px;
    border-top: 2px solid #f0f0f0;
}

.btn {
    padding: 12px 24px;
    border: none;
    border-radius: 8px;
    cursor: pointer;
    font-size: 16px;
    font-weight: 600;
    transition: all 0.3s;
}

.btn-complete {
    background: #4caf50;
    color: white;
}

.btn-complete:hover {
    background: #45a049;
}

.btn-incomplete {
    background: #ff9800;
    color: white;
}

.btn-incomplete:hover {
    background: #fb8c00;
}

.btn-edit {
    background: #2196f3;
    color: white;
}

.btn-edit:hover {
    background: #1976d2;
}

.btn-delete {
    background: #f44336;
    color: white;
}

.btn-delete:hover {
    background: #d32f2f;
}

.loading {
    text-align: center;
    padding: 60px 20px;
}

.error-message {
    background: #ffebee;
    color: #c62828;
    padding: 16px;
    border-radius: 8px;
    margin-bottom: 20px;
}
';

// 헤더 포함 (Include header)
include LAYOUT_PATH . '/header.php';
?>

<div class="detail-container">
    <!-- 뒤로가기 버튼 -->
    <a href="<?= buildUrl('list') ?>" class="back-button">
        ← 목록으로 돌아가기 (Back to List)
    </a>

    <!-- 에러 메시지 -->
    <div id="errorMessage" class="error-message" style="display: none;"></div>

    <!-- 로딩 표시 -->
    <div id="loadingIndicator" class="loading">
        <div class="spinner"></div>
        <p>할일 정보를 불러오는 중... (Loading to-do details...)</p>
    </div>

    <!-- 상세 정보 카드 -->
    <div id="detailCard" class="detail-card" style="display: none;">
        <!-- 헤더: 제목 및 뱃지 -->
        <div class="detail-header">
            <div class="detail-title-section">
                <h1 id="todoTitle" class="detail-title"></h1>
                <div>
                    <span id="priorityBadge" class="priority-badge"></span>
                    <span id="statusBadge" class="status-badge"></span>
                </div>
            </div>
        </div>

        <!-- 정보 그리드 -->
        <div class="detail-info-grid">
            <!-- 마감일 -->
            <div class="info-item">
                <div class="info-label">📅 마감일 (Due Date)</div>
                <div id="dueDate" class="info-value"></div>
            </div>

            <!-- 카테고리 -->
            <div class="info-item">
                <div class="info-label">📁 카테고리 (Category)</div>
                <div id="category" class="info-value"></div>
            </div>

            <!-- 생성일 -->
            <div class="info-item">
                <div class="info-label">🕐 생성일 (Created)</div>
                <div id="createdAt" class="info-value"></div>
            </div>

            <!-- 수정일 -->
            <div class="info-item">
                <div class="info-label">🔄 수정일 (Updated)</div>
                <div id="updatedAt" class="info-value"></div>
            </div>
        </div>

        <!-- 상세 설명 -->
        <div class="detail-description">
            <div class="description-label">📝 상세 설명 (Description)</div>
            <div id="description" class="description-content"></div>
        </div>

        <!-- 태그 -->
        <div class="detail-description">
            <div class="description-label">🏷️ 태그 (Tags)</div>
            <div id="tagsContainer" class="tags-container">
                <span class="info-value empty">태그 없음</span>
            </div>
        </div>

        <!-- 액션 버튼 -->
        <div class="detail-actions">
            <button id="toggleCompleteBtn" class="btn btn-complete" onclick="toggleComplete()"></button>
            <button class="btn btn-edit" onclick="editTodo()">✏️ 수정 (Edit)</button>
            <button class="btn btn-delete" onclick="deleteTodo()">🗑️ 삭제 (Delete)</button>
        </div>
    </div>
</div>

<?php
// 푸터 포함 (Include footer)
include LAYOUT_PATH . '/footer.php';
?>
