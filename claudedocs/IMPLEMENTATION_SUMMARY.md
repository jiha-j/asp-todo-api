# Todo 애플리케이션 확장 구현 완료 보고서

**프로젝트**: Todo 애플리케이션 기능 확장
**구현 날짜**: 2025-10-15
**버전**: 2.0
**상태**: ✅ 완료

---

## 📋 구현 개요

기본 CRUD 기능만 있던 Todo 애플리케이션을 실제 사용 가능한 수준으로 확장했습니다.

### 주요 개선사항
- ✅ 상세 페이지 추가 (Todo 클릭 시 상세 정보 확인)
- ✅ 전용 생성/수정 폼 페이지
- ✅ 마감기한, 우선순위, 카테고리, 태그 등 확장 필드
- ✅ 우선순위별 색상 뱃지
- ✅ 마감일 임박 경고
- ✅ 생성일/수정일 자동 추적

---

## 🗂️ 파일 변경 내역

### ✨ 새로 추가된 파일

#### 백엔드 (Backend)
```
Models/
└── TodoPriority.cs                    # 우선순위 Enum (Low, Normal, High, Urgent)

Migrations/
└── 20251015054212_AddTodoExtendedFields.cs  # 데이터베이스 마이그레이션
└── 20251015054212_AddTodoExtendedFields.Designer.cs
```

#### 프론트엔드 (Frontend)
```
frontend/
├── detail.html                        # Todo 상세 페이지
├── detail.js                          # 상세 페이지 로직
├── form.html                          # Todo 생성/수정 폼 페이지
└── form.js                            # 폼 페이지 로직
```

### 🔄 수정된 기존 파일

#### 백엔드 (Backend)
```
Models/
└── TodoItem.cs                        # 5개 필드 추가 (DueDate, Priority, Category, Tags, UpdatedAt)

Services/
└── TodoService.cs                     # UpdatedAt 자동 업데이트 로직 추가

Data/
└── TodoContext.cs                     # 자동 생성됨 (마이그레이션 반영)
```

#### 프론트엔드 (Frontend)
```
frontend/
├── index.php                          # "새 할일 만들기" 버튼으로 변경
├── app.js                             # 목록 렌더링 개선, 상세 페이지 연결
└── styles.css                         # 새로운 스타일 추가 (뱃지, 메타 정보)
```

---

## 🔧 백엔드 상세 변경사항

### 1. TodoPriority Enum (신규)
**파일**: `Models/TodoPriority.cs`

```csharp
public enum TodoPriority
{
    Low = 0,      // 낮음
    Normal = 1,   // 보통 (기본값)
    High = 2,     // 높음
    Urgent = 3    // 긴급
}
```

**용도**: Todo 항목의 우선순위를 4단계로 분류

---

### 2. TodoItem 모델 확장
**파일**: `Models/TodoItem.cs`

#### 추가된 필드

| 필드명 | 타입 | Nullable | 기본값 | 설명 |
|--------|------|----------|--------|------|
| `UpdatedAt` | DateTime | No | DateTime.UtcNow | 마지막 수정 시간 |
| `DueDate` | DateTime? | Yes | null | 마감기한 |
| `Priority` | TodoPriority | No | Normal | 우선순위 |
| `Category` | string? | Yes | null | 카테고리 (최대 50자) |
| `Tags` | string? | Yes | null | 태그 JSON 배열 (최대 500자) |

#### Before (기존)
```csharp
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }
}
```

#### After (확장)
```csharp
public class TodoItem
{
    // 기존 필드
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime CreatedAt { get; set; }

    // 새로 추가된 필드
    public DateTime UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public TodoPriority Priority { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
}
```

---

### 3. TodoService 수정
**파일**: `Services/TodoService.cs`

#### CreateTodoAsync 메서드
**변경사항**: UpdatedAt도 함께 설정

```csharp
// Before
todoItem.CreatedAt = DateTime.UtcNow;

// After
var now = DateTime.UtcNow;
todoItem.CreatedAt = now;
todoItem.UpdatedAt = now;
```

#### UpdateTodoAsync 메서드
**변경사항**: 새 필드 업데이트 로직 추가

```csharp
// 기존 필드 업데이트
existingTodo.Title = todoItem.Title;
existingTodo.Description = todoItem.Description;
existingTodo.IsCompleted = todoItem.IsCompleted;

// 새로 추가된 필드 업데이트
existingTodo.DueDate = todoItem.DueDate;
existingTodo.Priority = todoItem.Priority;
existingTodo.Category = todoItem.Category;
existingTodo.Tags = todoItem.Tags;

// 수정 시간 자동 업데이트
existingTodo.UpdatedAt = DateTime.UtcNow;
```

---

### 4. 데이터베이스 마이그레이션
**파일**: `Migrations/20251015054212_AddTodoExtendedFields.cs`

#### 실행된 SQL 변경사항

```sql
-- 5개 컬럼 추가
ALTER TABLE [Todos] ADD [Category] nvarchar(50) NULL;
ALTER TABLE [Todos] ADD [DueDate] datetime2 NULL;
ALTER TABLE [Todos] ADD [Priority] int NOT NULL DEFAULT 0;
ALTER TABLE [Todos] ADD [Tags] nvarchar(500) NULL;
ALTER TABLE [Todos] ADD [UpdatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';

-- 기존 데이터 업데이트 (기본값 설정)
UPDATE [Todos] SET
    [Category] = NULL,
    [DueDate] = NULL,
    [Priority] = 1,  -- Normal
    [Tags] = NULL,
    [UpdatedAt] = [CreatedAt]
WHERE [Id] > 0;
```

---

## 🎨 프론트엔드 상세 변경사항

### 1. 상세 페이지 (detail.html, detail.js)

#### 주요 기능
- ✅ URL 파라미터로 Todo ID 전달 (`detail.html?id=5`)
- ✅ 모든 Todo 정보 표시 (제목, 우선순위, 마감일, 카테고리, 태그, 설명)
- ✅ 완료 상태 토글 버튼
- ✅ 수정/삭제 버튼
- ✅ 뒤로가기 버튼

#### UI 구성
```
┌─────────────────────────────────────┐
│ ← 뒤로가기                          │
│                                     │
│ 할일 제목                           │
│ [긴급] [✅ 완료됨]                  │
│                                     │
│ 📅 마감일: 2025-10-20              │
│ 📁 카테고리: 업무                   │
│ 🕐 생성일: 2025-10-14 10:30        │
│ 🔄 수정일: 2025-10-15 14:20        │
│                                     │
│ 📝 상세 설명                        │
│ ┌─────────────────────────────┐   │
│ │ 설명 내용...                │   │
│ └─────────────────────────────┘   │
│                                     │
│ 🏷️ 태그                            │
│ [태그1] [태그2]                     │
│                                     │
│ [✅ 완료 처리] [✏️ 수정] [🗑️ 삭제] │
└─────────────────────────────────────┘
```

#### 핵심 함수

| 함수명 | 설명 |
|--------|------|
| `getTodoIdFromUrl()` | URL 파라미터에서 ID 추출 |
| `loadTodoDetail(id)` | API에서 상세 정보 조회 |
| `displayTodoDetail(todo)` | 화면에 정보 표시 |
| `toggleComplete()` | 완료 상태 토글 |
| `editTodo()` | 수정 페이지로 이동 |
| `deleteTodo()` | Todo 삭제 |
| `formatDate()` | 날짜 포맷팅 |
| `formatDateTime()` | 날짜+시간 포맷팅 |
| `isOverdue()` | 마감일 경과 확인 |

---

### 2. 생성/수정 폼 (form.html, form.js)

#### 주요 기능
- ✅ 생성/수정 모드 자동 감지 (URL에 ID 있으면 수정 모드)
- ✅ 모든 필드 입력 가능
- ✅ 동적 태그 추가/제거
- ✅ 우선순위 라디오 버튼
- ✅ 날짜/시간 선택기
- ✅ 폼 유효성 검사

#### UI 구성
```
┌─────────────────────────────────────┐
│ 새 할일 만들기 / 할일 수정          │
├─────────────────────────────────────┤
│ 제목 *                              │
│ [_____________________________]     │
│                                     │
│ 우선순위                            │
│ ◉낮음  ○보통  ○높음  ○긴급         │
│                                     │
│ 마감일                              │
│ [📅 2025-10-20 14:00]              │
│                                     │
│ 카테고리                            │
│ [업무_______]                       │
│                                     │
│ 태그                                │
│ [태그1 ×] [태그2 ×] [입력창___]    │
│                                     │
│ 상세 설명                           │
│ ┌─────────────────────────────┐   │
│ │                             │   │
│ │                             │   │
│ └─────────────────────────────┘   │
│                                     │
│         [취소]  [저장]              │
└─────────────────────────────────────┘
```

#### 핵심 함수

| 함수명 | 설명 |
|--------|------|
| `getTodoIdFromUrl()` | 수정 모드 감지 |
| `loadTodoData(id)` | 수정할 Todo 데이터 로드 |
| `populateForm(todo)` | 폼 필드 채우기 |
| `handleTagInput(event)` | Enter 키로 태그 추가 |
| `renderTags()` | 태그 UI 렌더링 |
| `removeTag(index)` | 태그 제거 |
| `handleFormSubmit(event)` | 폼 제출 처리 |
| `collectFormData()` | 폼 데이터 수집 |
| `validateForm(formData)` | 유효성 검사 |

#### 태그 관리 로직
```javascript
// 태그 배열
let tags = [];

// Enter 키로 태그 추가
function handleTagInput(event) {
    if (event.key === 'Enter') {
        event.preventDefault();
        const tagText = input.value.trim();
        if (tagText && !tags.includes(tagText)) {
            tags.push(tagText);
            renderTags();
        }
    }
}

// 서버 전송 시 JSON으로 변환
const tagsJson = tags.length > 0 ? JSON.stringify(tags) : null;
```

---

### 3. 목록 페이지 개선 (index.php, app.js)

#### index.php 변경사항

**Before**: 간단한 입력 폼
```html
<form id="addTodoForm">
    <input type="text" id="todoInput" placeholder="새로운 할일...">
    <button type="submit">추가</button>
</form>
```

**After**: 폼 페이지로 연결하는 버튼
```html
<div class="add-todo-container">
    <a href="form.html" class="btn-add">
        <span class="btn-icon">➕</span>
        새 할일 만들기 (Create New Todo)
    </a>
</div>
```

#### app.js 변경사항

**렌더링 로직 개선**

Before: 간단한 체크박스 + 텍스트 + 버튼
```javascript
const todosHtml = todos.map(todo => `
    <div class="todo-item">
        <input type="checkbox" ${todo.isCompleted ? 'checked' : ''}>
        <span class="todo-text">${todo.title}</span>
        <button onclick="editTodo()">✏️</button>
        <button onclick="deleteTodo()">🗑️</button>
    </div>
`).join('');
```

After: 우선순위, 마감일, 카테고리 표시 + 클릭 가능
```javascript
const todosHtml = todos.map(todo => {
    const priorityBadge = getPriorityBadge(todo.priority);
    const dueDateHtml = todo.dueDate ? `<span class="todo-due-date">${formatDate(todo.dueDate)}</span>` : '';
    const categoryHtml = todo.category ? `<span class="todo-category">${todo.category}</span>` : '';

    return `
        <div class="todo-item" onclick="viewTodoDetail(${todo.id}, event)">
            <div class="todo-content">
                <input type="checkbox" ${todo.isCompleted ? 'checked' : ''}>
                <div class="todo-info">
                    <span class="todo-text">${todo.title}</span>
                    <div class="todo-meta">
                        ${priorityBadge}
                        ${categoryHtml}
                        ${dueDateHtml}
                    </div>
                </div>
            </div>
            <div class="todo-actions">
                <button onclick="viewTodoDetail(${todo.id})">👁️</button>
                <button onclick="editTodo(${todo.id})">✏️</button>
                <button onclick="deleteTodo(${todo.id})">🗑️</button>
            </div>
        </div>
    `;
}).join('');
```

**새로 추가된 함수**

| 함수명 | 설명 |
|--------|------|
| `viewTodoDetail(id, event)` | 상세 페이지로 이동 |
| `editTodo(id, event)` | 수정 페이지로 이동 |
| `getPriorityBadge(priority)` | 우선순위 뱃지 HTML 생성 |
| `formatDate(date)` | 날짜 포맷팅 (YYYY-MM-DD) |

**제거된 함수** (더 이상 사용하지 않음)
- `setupEventListeners()` - 폼이 별도 페이지로 이동
- `addTodo()` - 폼 페이지에서 처리
- `updateTodo()` - 더 이상 인라인 편집 안 함
- `startEdit()` - 제거됨
- `saveEdit()` - 제거됨
- `cancelEdit()` - 제거됨

---

### 4. 스타일 추가 (styles.css)

#### 새로 추가된 CSS 클래스

```css
/* Todo 항목 레이아웃 */
.todo-content { }              /* Todo 내용 영역 */
.todo-info { }                 /* Todo 정보 컨테이너 */
.todo-meta { }                 /* 메타 정보 (우선순위, 카테고리, 마감일) */

/* 우선순위 뱃지 */
.priority-badge { }
.priority-low { }              /* 파란색 */
.priority-normal { }           /* 보라색 */
.priority-high { }             /* 주황색 */
.priority-urgent { }           /* 빨간색 */

/* 카테고리 뱃지 */
.todo-category { }             /* 회색 */

/* 마감일 표시 */
.todo-due-date { }             /* 초록색 */
.todo-due-date.overdue { }     /* 빨간색 (마감 경과) */

/* 버튼 */
.btn-view { }                  /* 상세보기 버튼 */
```

#### 색상 체계

| 우선순위 | 배경색 | 텍스트색 |
|----------|--------|----------|
| 낮음 (Low) | #e3f2fd (연한 파랑) | #1976d2 (진한 파랑) |
| 보통 (Normal) | #f3e5f5 (연한 보라) | #7b1fa2 (진한 보라) |
| 높음 (High) | #fff3e0 (연한 주황) | #f57c00 (진한 주황) |
| 긴급 (Urgent) | #ffebee (연한 빨강) | #c62828 (진한 빨강) |

---

## 📊 데이터베이스 스키마

### Todos 테이블 (최종)

| 컬럼명 | 타입 | Nullable | 기본값 | 제약조건 |
|--------|------|----------|--------|----------|
| Id | int | No | - | PRIMARY KEY, IDENTITY |
| Title | nvarchar(200) | No | - | REQUIRED |
| Description | nvarchar(1000) | Yes | null | - |
| IsCompleted | bit | No | 0 | - |
| CreatedAt | datetime2 | No | GETUTCDATE() | - |
| **UpdatedAt** | **datetime2** | **No** | **GETUTCDATE()** | **NEW** |
| **DueDate** | **datetime2** | **Yes** | **null** | **NEW** |
| **Priority** | **int** | **No** | **1** | **NEW** |
| **Category** | **nvarchar(50)** | **Yes** | **null** | **NEW** |
| **Tags** | **nvarchar(500)** | **Yes** | **null** | **NEW** |

---

## 🔄 API 응답 변경사항

### Before (기존)
```json
{
  "id": 1,
  "title": "프로젝트 완료하기",
  "description": "ASP.NET Core 프로젝트 마무리",
  "isCompleted": false,
  "createdAt": "2025-10-14T10:30:00Z"
}
```

### After (확장)
```json
{
  "id": 1,
  "title": "프로젝트 완료하기",
  "description": "ASP.NET Core 프로젝트 마무리",
  "isCompleted": false,
  "createdAt": "2025-10-14T10:30:00Z",
  "updatedAt": "2025-10-15T14:20:00Z",
  "dueDate": "2025-10-20T23:59:00Z",
  "priority": 2,
  "category": "업무",
  "tags": "[\"중요\",\"프로젝트\",\"마감\"]"
}
```

---

## 🎯 주요 기능 설명

### 1. 우선순위 시스템

**4단계 우선순위**
- **낮음 (0)**: 급하지 않은 작업
- **보통 (1)**: 일반 작업 (기본값)
- **높음 (2)**: 중요한 작업
- **긴급 (3)**: 즉시 처리 필요

**시각적 구분**
- 목록에서 색상 뱃지로 표시
- 상세 페이지에서 큰 뱃지로 강조

---

### 2. 마감일 관리

**기능**
- 선택적 설정 (Nullable)
- 날짜 + 시간 모두 지정 가능
- 마감일 임박/경과 시 빨간색 경고

**마감일 표시 로직**
```javascript
const dueDate = new Date(todo.dueDate);
const now = new Date();
const isOverdue = dueDate < now && !todo.isCompleted;

if (isOverdue) {
    // 빨간색 경고 표시
    dueDateElement.style.color = '#c62828';
    dueDateElement.textContent += ' ⚠️ 마감 임박!';
}
```

---

### 3. 카테고리 시스템

**특징**
- 자유 텍스트 입력 (최대 50자)
- 목록에서 회색 뱃지로 표시
- 예: "업무", "개인", "쇼핑", "공부"

---

### 4. 태그 시스템

**구현 방식**
- JSON 배열로 데이터베이스 저장
- JavaScript에서 동적 추가/제거
- Enter 키로 태그 추가
- × 버튼으로 태그 제거

**저장 형식**
```json
// 데이터베이스
tags: "[\"중요\",\"긴급\",\"프로젝트A\"]"

// JavaScript
tags = ["중요", "긴급", "프로젝트A"];
```

---

### 5. 자동 시간 추적

**CreatedAt** (생성일)
- Todo 생성 시 서버에서 자동 설정
- 클라이언트 값 무시 (보안)
- 변경 불가

**UpdatedAt** (수정일)
- Todo 수정 시 서버에서 자동 업데이트
- 수정 이력 추적
- 목록/상세 페이지에서 표시

---

## 🚀 사용 흐름도

### Todo 생성 흐름
```
1. 목록 페이지에서 "새 할일 만들기" 클릭
   ↓
2. form.html 페이지 로드 (생성 모드)
   ↓
3. 모든 정보 입력 (제목, 우선순위, 마감일, 카테고리, 태그, 설명)
   ↓
4. "저장" 버튼 클릭
   ↓
5. POST /api/todos → 서버에 저장
   ↓
6. 자동으로 목록 페이지로 리다이렉트
```

### Todo 수정 흐름
```
1. 목록 또는 상세 페이지에서 "수정" 버튼 클릭
   ↓
2. form.html?id=5 페이지 로드 (수정 모드)
   ↓
3. 기존 데이터 자동 로드 및 표시
   ↓
4. 정보 수정
   ↓
5. "수정" 버튼 클릭
   ↓
6. PUT /api/todos/5 → 서버에 업데이트
   ↓
7. UpdatedAt 자동 갱신
   ↓
8. 자동으로 목록 페이지로 리다이렉트
```

### Todo 상세보기 흐름
```
1. 목록에서 Todo 항목 클릭
   또는
   "👁️" 버튼 클릭
   ↓
2. detail.html?id=5 페이지 로드
   ↓
3. GET /api/todos/5 → 상세 정보 조회
   ↓
4. 모든 정보 표시 (제목, 우선순위, 마감일, 카테고리, 태그, 설명, 날짜)
   ↓
5. 가능한 작업:
   - 완료 상태 토글
   - 수정 (form.html로 이동)
   - 삭제 (확인 후 삭제)
   - 뒤로가기 (목록으로)
```

---

## 📁 프로젝트 구조 (최종)

```
C:\claude\asp_practice_1/
│
├── Controllers/
│   └── TodoController.cs              # REST API 컨트롤러 (기존, 변경 없음)
│
├── Models/
│   ├── TodoItem.cs                    # ✏️ 수정됨: 5개 필드 추가
│   └── TodoPriority.cs                # ✨ 신규: 우선순위 Enum
│
├── Services/
│   ├── ITodoService.cs                # 인터페이스 (기존)
│   └── TodoService.cs                 # ✏️ 수정됨: UpdatedAt 자동 업데이트
│
├── Data/
│   └── TodoContext.cs                 # DbContext (기존)
│
├── Migrations/
│   ├── 20251014083033_InitialCreate.cs                      # 기존 마이그레이션
│   └── 20251015054212_AddTodoExtendedFields.cs              # ✨ 신규: 필드 추가 마이그레이션
│
├── frontend/
│   ├── index.php                      # ✏️ 수정됨: 목록 페이지
│   ├── detail.html                    # ✨ 신규: 상세 페이지
│   ├── detail.js                      # ✨ 신규: 상세 페이지 로직
│   ├── form.html                      # ✨ 신규: 폼 페이지
│   ├── form.js                        # ✨ 신규: 폼 로직
│   ├── app.js                         # ✏️ 수정됨: 렌더링 개선
│   └── styles.css                     # ✏️ 수정됨: 새 스타일 추가
│
├── claudedocs/
│   ├── TODO_ENHANCEMENT_PLAN.md       # 기존 계획서
│   └── IMPLEMENTATION_SUMMARY.md      # ✨ 신규: 이 문서
│
├── Program.cs                         # 진입점 (기존)
├── TodoApi.csproj                     # 프로젝트 파일 (기존)
└── appsettings.json                   # 설정 파일 (기존)
```

---

## ✅ 테스트 체크리스트

### 백엔드 테스트
- [x] 빌드 성공 (경고 0, 오류 0)
- [x] 데이터베이스 마이그레이션 성공
- [x] 기존 Todo 항목에 기본값 적용 확인

### API 테스트 (예정)
- [ ] GET /api/todos - 확장된 필드 포함 확인
- [ ] GET /api/todos/{id} - 상세 정보 조회
- [ ] POST /api/todos - 새 필드와 함께 생성
- [ ] PUT /api/todos/{id} - UpdatedAt 자동 업데이트 확인

### 프론트엔드 테스트 (예정)
- [ ] 목록 페이지에서 우선순위 뱃지 표시 확인
- [ ] Todo 클릭 시 상세 페이지 이동 확인
- [ ] "새 할일 만들기" 버튼으로 폼 페이지 이동
- [ ] 폼에서 모든 필드 입력 및 저장
- [ ] 태그 추가/제거 기능 확인
- [ ] 수정 모드에서 기존 데이터 로드 확인
- [ ] 마감일 임박 경고 표시 확인
- [ ] 완료 상태 토글 확인

---

## 🔍 검색 및 필터링 기능 (Phase 2.5)

### 개요
사용자가 할일 목록을 쉽게 탐색할 수 있도록 검색 및 필터링 기능을 추가했습니다.

### 주요 기능
- ✅ **제목 검색**: 제목으로 할일 검색 (대소문자 무관)
- ✅ **우선순위 필터**: 우선순위 레벨별 필터링 (낮음/보통/높음/긴급)
- ✅ **복합 필터**: 검색과 우선순위 필터를 동시에 적용 가능
- ✅ **초기화 버튼**: 한 번의 클릭으로 모든 필터 제거
- ✅ **Enter 키 지원**: 검색 입력창에서 Enter 키로 검색 실행

### 백엔드 변경사항

#### TodoController.cs 수정
**파일**: `Controllers/TodoController.cs`

**GetAllTodos 메서드 확장**
```csharp
[HttpGet]
public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodos(
    [FromQuery] int? priority,
    [FromQuery] string? search
)
{
    var todos = await _todoService.GetAllTodosAsync();

    // 우선순위 필터 적용
    if (priority.HasValue)
    {
        todos = todos.Where(t => (int)t.Priority == priority.Value);
    }

    // 제목 검색 (대소문자 무관)
    if (!string.IsNullOrWhiteSpace(search))
    {
        todos = todos.Where(t => t.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
    }

    return Ok(todos);
}
```

**쿼리 파라미터**
| 파라미터 | 타입 | 필수 | 설명 |
|---------|------|-----|------|
| priority | int? | 선택 | 우선순위 필터 (0=낮음, 1=보통, 2=높음, 3=긴급) |
| search | string? | 선택 | 제목 검색어 (대소문자 무관) |

**API 요청 예시**
```http
GET /api/todos                              # 전체 조회
GET /api/todos?priority=2                    # 높은 우선순위만
GET /api/todos?search=회의                   # "회의" 포함 항목
GET /api/todos?priority=3&search=프로젝트    # 긴급 + "프로젝트" 포함
```

### 프론트엔드 변경사항

#### index.php 수정
**파일**: `frontend/index.php`

**새로 추가된 UI 섹션**
```html
<div class="search-filter-container">
    <!-- 제목 검색 -->
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

    <!-- 우선순위 필터 -->
    <div class="priority-filter">
        <label for="priorityFilter" class="filter-label">우선순위:</label>
        <select id="priorityFilter" class="priority-select" onchange="applyFilters()">
            <option value="">전체 (All)</option>
            <option value="0">낮음 (Low)</option>
            <option value="1">보통 (Normal)</option>
            <option value="2">높음 (High)</option>
            <option value="3">긴급 (Urgent)</option>
        </select>
    </div>
</div>
```

#### app.js 수정
**파일**: `frontend/app.js`

**loadTodos() 함수 확장**
```javascript
// Before: 파라미터 없음
async function loadTodos() { ... }

// After: 필터 객체를 파라미터로 받음
async function loadTodos(filters = {}) {
    // URLSearchParams로 쿼리 문자열 생성
    const params = new URLSearchParams();

    if (filters.priority !== undefined && filters.priority !== null && filters.priority !== '') {
        params.append('priority', filters.priority);
    }

    if (filters.search && filters.search.trim() !== '') {
        params.append('search', filters.search.trim());
    }

    const queryString = params.toString();
    const url = queryString ? `${API_BASE_URL}?${queryString}` : API_BASE_URL;

    const response = await fetch(url, { ... });
}
```

**새로 추가된 함수**

| 함수명 | 설명 | 동작 |
|--------|------|------|
| `applyFilters()` | 검색 및 필터 적용 | 검색어와 우선순위 값을 읽어서 loadTodos() 호출, 클리어 버튼 표시/숨김 처리 |
| `clearFilters()` | 검색 및 필터 초기화 | 입력 필드 초기화, 클리어 버튼 숨김, 전체 목록 다시 로드 |

**applyFilters() 구현**
```javascript
function applyFilters() {
    // 검색어 가져오기
    const searchInput = document.getElementById('searchInput');
    const searchValue = searchInput ? searchInput.value.trim() : '';

    // 우선순위 필터 가져오기
    const prioritySelect = document.getElementById('priorityFilter');
    const priorityValue = prioritySelect ? prioritySelect.value : '';

    // 필터 객체 생성
    const filters = {};
    if (searchValue !== '') filters.search = searchValue;
    if (priorityValue !== '') filters.priority = parseInt(priorityValue);

    // 클리어 버튼 표시/숨김
    const clearButton = document.getElementById('clearButton');
    if (clearButton) {
        clearButton.style.display = (searchValue !== '' || priorityValue !== '')
            ? 'inline-block'
            : 'none';
    }

    // 필터 적용하여 목록 다시 로드
    loadTodos(filters);
}
```

**clearFilters() 구현**
```javascript
function clearFilters() {
    // 검색 입력 초기화
    const searchInput = document.getElementById('searchInput');
    if (searchInput) searchInput.value = '';

    // 우선순위 필터 초기화
    const prioritySelect = document.getElementById('priorityFilter');
    if (prioritySelect) prioritySelect.value = '';

    // 클리어 버튼 숨김
    const clearButton = document.getElementById('clearButton');
    if (clearButton) clearButton.style.display = 'none';

    // 전체 목록 다시 로드
    loadTodos();
}
```

**Enter 키 이벤트 리스너**
```javascript
document.addEventListener('DOMContentLoaded', function() {
    // ... 기존 초기화 코드 ...

    // Enter 키로 검색 실행
    const searchInput = document.getElementById('searchInput');
    if (searchInput) {
        searchInput.addEventListener('keypress', function(event) {
            if (event.key === 'Enter') {
                event.preventDefault();
                applyFilters();
            }
        });
    }
});
```

### 사용 흐름

#### 제목 검색
```
1. 검색 입력창에 키워드 입력 (예: "회의")
   ↓
2. "검색" 버튼 클릭 또는 Enter 키 입력
   ↓
3. API 요청: GET /api/todos?search=회의
   ↓
4. 제목에 "회의"가 포함된 할일만 표시
   ↓
5. "✕" 클리어 버튼이 나타남
```

#### 우선순위 필터
```
1. 우선순위 드롭다운에서 "높음" 선택
   ↓
2. 자동으로 필터 적용 (onchange 이벤트)
   ↓
3. API 요청: GET /api/todos?priority=2
   ↓
4. 높은 우선순위 할일만 표시
   ↓
5. "✕" 클리어 버튼이 나타남
```

#### 복합 필터
```
1. 검색창에 "프로젝트" 입력
   ↓
2. 우선순위에서 "긴급" 선택
   ↓
3. API 요청: GET /api/todos?priority=3&search=프로젝트
   ↓
4. 긴급하면서 "프로젝트"가 포함된 할일만 표시
```

#### 필터 초기화
```
1. "✕" 클리어 버튼 클릭
   ↓
2. 검색창과 우선순위 드롭다운 모두 초기화
   ↓
3. API 요청: GET /api/todos
   ↓
4. 전체 할일 목록 다시 표시
   ↓
5. 클리어 버튼 숨김
```

### 기술적 구현 사항

#### URLSearchParams 활용
- 쿼리 파라미터를 안전하게 생성하는 표준 Web API
- 자동으로 URL 인코딩 처리
- 조건부 파라미터 추가 가능

#### 대소문자 무관 검색
- `StringComparison.OrdinalIgnoreCase` 사용
- "회의", "회의", "MEETING" 모두 검색 가능

#### 사용자 경험 개선
- Enter 키로 빠른 검색
- 우선순위 변경 시 자동 필터링 (버튼 클릭 불필요)
- 필터 활성화 시에만 클리어 버튼 표시
- 명확한 UI 피드백

### 제외된 기능
사용자 요청에 따라 다음 필드는 검색에서 제외:
- ❌ Description (상세 설명)
- ❌ CreatedAt (생성일)
- ❌ UpdatedAt (수정일)
- ❌ Category (카테고리) - 향후 추가 가능
- ❌ Tags (태그) - 향후 추가 가능

---

## 🎓 배운 점 / 개선 사항

### 성공한 부분
1. ✅ **점진적 확장**: 기존 코드를 깨뜨리지 않고 새 기능 추가
2. ✅ **Nullable 타입 활용**: 선택적 필드를 안전하게 처리
3. ✅ **자동 타임스탬프**: 서버에서 시간 관리로 보안 강화
4. ✅ **모듈화**: 페이지별로 HTML/JS 분리로 유지보수성 향상
5. ✅ **검색 및 필터링**: RESTful API 패턴으로 쿼리 파라미터 구현
6. ✅ **사용자 경험**: Enter 키 지원, 자동 필터링, 클리어 버튼 등

### 개선 가능한 부분
1. 🔄 **DTO 패턴**: TodoListDto, TodoDetailDto로 응답 최적화
2. ✅ **필터링 API**: 우선순위별 필터링 (완료)
3. 🔄 **정렬 기능**: 마감일순, 우선순위순 정렬
4. ✅ **검색 기능**: 제목으로 검색 (완료)
5. 🔄 **카테고리 필터**: 카테고리별 필터링
6. 🔄 **태그 검색**: 태그로 검색
7. 🔄 **단위 테스트**: xUnit 테스트 추가
8. 🔄 **통합 테스트**: API 엔드포인트 테스트

---

## 📌 다음 단계 제안

### Phase 3: 고급 기능 (선택사항)
- [ ] 필터링 UI 추가 (우선순위별, 카테고리별, 마감일별)
- [ ] 정렬 기능 (드롭다운)
- [ ] 검색창 추가
- [ ] 통계 대시보드 (전체/완료/미완료 개수, 우선순위별 분포)

### Phase 4: UX 개선 (선택사항)
- [ ] 반응형 디자인 개선 (모바일 최적화)
- [ ] 드래그 앤 드롭으로 우선순위 변경
- [ ] 일괄 작업 (여러 Todo 한 번에 완료/삭제)
- [ ] 다크 모드
- [ ] 로딩 스피너 개선
- [ ] 애니메이션 효과

---

## 🔗 참고 링크

- [TODO_ENHANCEMENT_PLAN.md](./TODO_ENHANCEMENT_PLAN.md) - 원래 계획서
- [ASP.NET Core 문서](https://learn.microsoft.com/ko-kr/aspnet/core/)
- [Entity Framework Core 마이그레이션](https://learn.microsoft.com/ko-kr/ef/core/managing-schemas/migrations/)

---

**마지막 업데이트**: 2025-10-15
**작성자**: Claude Code
**버전**: 2.0 (Implementation Complete)
