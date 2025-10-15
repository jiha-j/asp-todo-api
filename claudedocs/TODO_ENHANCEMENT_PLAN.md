# Todo 애플리케이션 확장 기능 명세서

## 📋 프로젝트 개요

**목표**: 기본 Todo CRUD 기능을 실제 사용 가능한 Todo 애플리케이션으로 확장

**현재 상태**:
- 기본 CRUD 기능만 있음 (생성, 조회, 수정, 삭제)
- Todo 항목: Id, Title, IsComplete만 존재

**목표 상태**:
- Todo 클릭 시 상세 페이지 이동
- 마감기한, 우선순위, 상세 설명 등 추가 필드
- 실제 사용 가능한 수준의 기능

---

## 🎯 추가할 주요 기능

### 1. Todo 상세 정보
- **마감기한 (Due Date)**: 언제까지 완료해야 하는지
- **우선순위 (Priority)**: 낮음/보통/높음/긴급
- **상세 설명 (Description)**: Todo에 대한 자세한 설명
- **카테고리/태그 (Category/Tags)**: 분류 기능
- **생성일/수정일 (CreatedAt/UpdatedAt)**: 추적 기능

### 2. UI/UX 개선
- Todo 목록 페이지: 간략한 정보만 표시
- Todo 클릭 → 상세 페이지로 이동
- 상세 페이지에서 모든 정보 확인/수정 가능

### 3. 추가 기능 (선택사항)
- 필터링: 완료/미완료, 우선순위별, 카테고리별
- 정렬: 마감일순, 우선순위순, 생성일순
- 검색: 제목/설명으로 검색
- 마감일 임박 알림

---

## 🗄️ 데이터베이스 모델 변경

### 현재 Todo 모델
```csharp
public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; }
}
```

### 확장된 Todo 모델 (제안)
```csharp
public class TodoItem
{
    // 기존 필드
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public bool IsComplete { get; set; }

    // 새로운 필드
    public string? Description { get; set; }           // 상세 설명
    public DateTime? DueDate { get; set; }             // 마감기한
    public TodoPriority Priority { get; set; }         // 우선순위
    public string? Category { get; set; }              // 카테고리
    public List<string>? Tags { get; set; }            // 태그 (JSON)
    public DateTime CreatedAt { get; set; }            // 생성일
    public DateTime UpdatedAt { get; set; }            // 수정일
}

public enum TodoPriority
{
    Low = 0,      // 낮음
    Normal = 1,   // 보통
    High = 2,     // 높음
    Urgent = 3    // 긴급
}
```

---

## 🔌 API 엔드포인트 설계

### 현재 API
```
GET    /api/todos          - 모든 Todo 조회
GET    /api/todos/{id}     - 특정 Todo 조회
POST   /api/todos          - Todo 생성
PUT    /api/todos/{id}     - Todo 수정
DELETE /api/todos/{id}     - Todo 삭제
```

### 추가/변경할 API
```
# 기본 CRUD (기존과 동일하지만 확장된 필드 포함)
GET    /api/todos                        - 전체 목록 (간략 정보)
GET    /api/todos/{id}                   - 상세 정보
POST   /api/todos                        - Todo 생성 (확장 필드 포함)
PUT    /api/todos/{id}                   - Todo 수정
DELETE /api/todos/{id}                   - Todo 삭제

# 필터링/정렬
GET    /api/todos?status=completed       - 완료된 Todo만
GET    /api/todos?priority=high          - 높은 우선순위만
GET    /api/todos?category=work          - 특정 카테고리
GET    /api/todos?sortBy=dueDate         - 마감일순 정렬
GET    /api/todos?search=keyword         - 검색

# 통계
GET    /api/todos/stats                  - 통계 정보 (전체/완료/미완료)
GET    /api/todos/upcoming               - 마감 임박 Todo
```

---

## 🎨 프론트엔드 UI 설계

### 1. Todo 목록 페이지 (List View)
```
┌─────────────────────────────────────────────┐
│  📝 My Todo List                            │
│  ┌─────────┐ ┌──────────┐ ┌──────────┐     │
│  │ All (12)│ │ Active (8)│ │ Done (4) │     │
│  └─────────┘ └──────────┘ └──────────┘     │
├─────────────────────────────────────────────┤
│  ☐ [높음] 프로젝트 보고서 작성              │
│     📅 2025-10-20  📁 업무                  │
│                                             │
│  ☑ [보통] 장보기                            │
│     📅 2025-10-15  📁 개인                  │
│                                             │
│  ☐ [긴급] IIS 배포 완료                     │
│     📅 오늘  📁 개발                        │
└─────────────────────────────────────────────┘
```

### 2. Todo 상세 페이지 (Detail View)
```
┌─────────────────────────────────────────────┐
│  ← 뒤로가기                                 │
│                                             │
│  ☐ IIS 배포 완료                            │
│     [긴급]                                  │
│                                             │
│  📅 마감일: 2025-10-15 23:59               │
│  📁 카테고리: 개발                          │
│  🏷️ 태그: #IIS #배포 #ASP.NET              │
│                                             │
│  📝 상세 설명:                              │
│  ┌─────────────────────────────────────┐   │
│  │ ASP.NET Core 프로젝트를              │   │
│  │ IIS에 배포하고 테스트 완료           │   │
│  └─────────────────────────────────────┘   │
│                                             │
│  📊 생성일: 2025-10-14 10:30               │
│  📊 수정일: 2025-10-15 14:20               │
│                                             │
│  [수정] [삭제] [완료 처리]                  │
└─────────────────────────────────────────────┘
```

### 3. Todo 생성/수정 폼
```
┌─────────────────────────────────────────────┐
│  새 Todo 만들기                             │
├─────────────────────────────────────────────┤
│  제목: [____________________________]       │
│                                             │
│  우선순위:                                  │
│  ◉ 낮음  ○ 보통  ○ 높음  ○ 긴급            │
│                                             │
│  마감일: [📅 2025-10-20]                   │
│                                             │
│  카테고리: [업무 ▼]                         │
│                                             │
│  태그: [#프로젝트 #중요] [+ 추가]           │
│                                             │
│  상세 설명:                                 │
│  ┌─────────────────────────────────────┐   │
│  │                                     │   │
│  │                                     │   │
│  └─────────────────────────────────────┘   │
│                                             │
│  [취소] [저장]                              │
└─────────────────────────────────────────────┘
```

---

## 📂 프로젝트 구조 변경

```
TodoApi/
├── Controllers/
│   ├── TodoController.cs         (수정: 확장 필드 처리)
│   └── TodoStatsController.cs    (신규: 통계 API)
│
├── Models/
│   ├── TodoItem.cs               (수정: 필드 추가)
│   ├── TodoPriority.cs           (신규: Enum)
│   └── DTOs/
│       ├── TodoListDto.cs        (신규: 목록용 간략 DTO)
│       ├── TodoDetailDto.cs      (신규: 상세용 DTO)
│       └── CreateTodoDto.cs      (신규: 생성용 DTO)
│
├── Data/
│   └── TodoContext.cs            (수정: 새 필드 반영)
│
├── Services/
│   ├── ITodoService.cs           (신규: 비즈니스 로직 인터페이스)
│   └── TodoService.cs            (신규: 비즈니스 로직)
│
├── frontend/                      (프론트엔드)
│   ├── index.html                (수정: 간략 정보만)
│   ├── detail.html               (신규: 상세 페이지)
│   ├── create.html               (신규: 생성/수정 폼)
│   └── js/
│       ├── list.js               (수정: 목록 로직)
│       ├── detail.js             (신규: 상세 페이지 로직)
│       └── form.js               (신규: 폼 로직)
│
└── Migrations/                    (마이그레이션 파일)
```

---

## 🚀 구현 단계

### Phase 1: 백엔드 확장 (우선순위: 높음)
1. **모델 확장**
   - [ ] TodoItem 클래스에 새 필드 추가
   - [ ] TodoPriority Enum 생성
   - [ ] DTO 클래스 생성 (List, Detail, Create)

2. **데이터베이스 마이그레이션**
   - [ ] 마이그레이션 생성: `dotnet ef migrations add AddTodoExtendedFields`
   - [ ] 데이터베이스 업데이트: `dotnet ef database update`

3. **컨트롤러 수정**
   - [ ] GET /api/todos - 간략 정보 반환 (TodoListDto)
   - [ ] GET /api/todos/{id} - 상세 정보 반환 (TodoDetailDto)
   - [ ] POST /api/todos - 확장 필드 포함 생성
   - [ ] PUT /api/todos/{id} - 확장 필드 포함 수정

4. **추가 API 구현**
   - [ ] 필터링 기능 (status, priority, category)
   - [ ] 정렬 기능 (sortBy 파라미터)
   - [ ] 검색 기능 (search 파라미터)
   - [ ] 통계 API (GET /api/todos/stats)

### Phase 2: 프론트엔드 개발 (우선순위: 높음)
1. **목록 페이지 개선**
   - [ ] 간략 정보 표시 (제목, 우선순위, 마감일, 카테고리)
   - [ ] Todo 클릭 시 상세 페이지로 이동
   - [ ] 우선순위별 색상 표시
   - [ ] 마감일 임박 표시

2. **상세 페이지 생성**
   - [ ] detail.html 생성
   - [ ] 모든 Todo 정보 표시
   - [ ] 수정/삭제 버튼
   - [ ] 완료 상태 토글

3. **생성/수정 폼**
   - [ ] create.html (생성/수정 공통 사용)
   - [ ] 모든 필드 입력 가능
   - [ ] 날짜 선택기 (Date Picker)
   - [ ] 우선순위 선택
   - [ ] 태그 입력 (동적 추가/삭제)

### Phase 3: 고급 기능 (우선순위: 중간)
1. **필터링/정렬 UI**
   - [ ] 드롭다운으로 필터 선택
   - [ ] 정렬 옵션
   - [ ] 검색창

2. **통계 대시보드**
   - [ ] 전체/완료/미완료 개수
   - [ ] 우선순위별 분포
   - [ ] 카테고리별 분포
   - [ ] 마감 임박 Todo 목록

### Phase 4: UX 개선 (우선순위: 낮음)
1. **UI 개선**
   - [ ] 반응형 디자인 (모바일 대응)
   - [ ] 애니메이션 효과
   - [ ] 로딩 스피너
   - [ ] 에러 메시지 표시

2. **추가 기능**
   - [ ] 드래그 앤 드롭으로 우선순위 변경
   - [ ] 일괄 작업 (다중 선택)
   - [ ] 마감일 알림 (브라우저 알림)
   - [ ] 다크 모드

---

## 🛠️ 기술 스택

### 백엔드
- **ASP.NET Core 9.0**: Web API
- **Entity Framework Core**: ORM
- **SQL Server LocalDB**: 데이터베이스
- **Swagger**: API 문서화

### 프론트엔드
- **HTML5/CSS3**: 마크업/스타일
- **JavaScript (Vanilla)**: 로직
- **Fetch API**: HTTP 통신
- **선택사항**: Bootstrap, Tailwind CSS 등 CSS 프레임워크

---

## 📊 예상 작업 시간

| 단계 | 예상 시간 | 난이도 |
|------|-----------|--------|
| Phase 1: 백엔드 확장 | 3-4시간 | 중 |
| Phase 2: 프론트엔드 개발 | 4-5시간 | 중 |
| Phase 3: 고급 기능 | 2-3시간 | 중 |
| Phase 4: UX 개선 | 2-3시간 | 낮 |
| **총 예상 시간** | **11-15시간** | - |

---

## 📝 참고 사항

### 데이터베이스 마이그레이션 시 주의사항
- 기존 데이터가 있다면 백업 필요
- 새 필드는 Nullable 또는 기본값 설정 필요
- 마이그레이션 전 테스트 환경에서 먼저 테스트

### API 호환성
- 기존 API 엔드포인트는 유지 (하위 호환성)
- 새 필드는 선택적(Optional)으로 처리
- API 버전 관리 고려 (v1, v2 등)

### 프론트엔드 라우팅
- 단일 페이지 앱(SPA)으로 전환 고려
- URL 구조:
  - `/` - 목록
  - `/todo/{id}` - 상세
  - `/create` - 생성
  - `/edit/{id}` - 수정

---

## ✅ 다음 단계

1. **현재**: IIS 배포 완료
2. **다음**: Phase 1 백엔드 확장 시작
3. **필요 시**: 추가 요구사항 논의

---

**생성일**: 2025-10-15
**버전**: 1.0
**상태**: 계획 단계
