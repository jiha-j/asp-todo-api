# API 문서 (API Documentation)

> **할일 목록 REST API 전체 가이드**
>
> **Complete Guide to To-Do List REST API**

---

## 📋 목차 (Table of Contents)

1. [API 개요](#-api-개요-api-overview)
2. [인증](#-인증-authentication)
3. [Base URL](#-base-url)
4. [요청/응답 형식](#-요청응답-형식-requestresponse-format)
5. [엔드포인트 목록](#-엔드포인트-목록-endpoints)
6. [데이터 모델](#-데이터-모델-data-models)
7. [에러 코드](#-에러-코드-error-codes)
8. [예제 코드](#-예제-코드-example-code)
9. [테스팅 도구](#-테스팅-도구-testing-tools)

---

## 🌐 API 개요 (API Overview)

### 무엇을 하는 API인가? (What does this API do?)

이 API는 할일 목록을 관리하는 RESTful 웹 서비스입니다.

This API is a RESTful web service for managing a to-do list.

**주요 기능 (Main Features):**
- ✅ 할일 목록 조회 (Retrieve to-do list)
- ✅ 새로운 할일 추가 (Add new to-dos)
- ✅ 할일 수정 (Update to-dos)
- ✅ 할일 삭제 (Delete to-dos)
- ✅ 완료 상태 관리 (Manage completion status)

### API 아키텍처 스타일

**RESTful API Principles:**

| 원칙 (Principle) | 설명 (Description) | 구현 (Implementation) |
|-----------------|-------------------|---------------------|
| **Client-Server** | 클라이언트와 서버 분리 | Frontend (PHP) ↔ Backend (ASP.NET Core) |
| **Stateless** | 각 요청이 독립적 | 요청에 모든 정보 포함 |
| **Cacheable** | 응답 캐시 가능 | HTTP 캐시 헤더 사용 |
| **Uniform Interface** | 일관된 인터페이스 | HTTP 메서드 + URI |
| **Layered System** | 계층화된 시스템 | Controller → Service → Data |

---

## 🔐 인증 (Authentication)

### 현재 구현 (Current Implementation)

**⚠️ 인증 없음 (No Authentication)**

이 API는 학습 목적으로 만들어져 인증이 구현되지 않았습니다.

This API was created for learning purposes and does not implement authentication.

**프로덕션 환경에서는 다음 중 하나를 구현해야 합니다:**

For production environments, implement one of the following:

1. **JWT (JSON Web Tokens)**
   ```http
   Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
   ```

2. **API Keys**
   ```http
   X-API-Key: your-api-key-here
   ```

3. **OAuth 2.0**
   - 제3자 인증 (Google, Microsoft, etc.)

---

## 🌍 Base URL

### 개발 환경 (Development)

```
http://localhost:5000
```

### API 엔드포인트 Base Path

```
http://localhost:5000/api/todos
```

### 프로덕션 환경 (Production) - 예시

```
https://your-domain.com/api/todos
```

---

## 📦 요청/응답 형식 (Request/Response Format)

### Content Type

모든 요청과 응답은 JSON 형식을 사용합니다.

All requests and responses use JSON format.

```http
Content-Type: application/json
Accept: application/json
```

### 요청 구조 (Request Structure)

```http
[METHOD] [URL] HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json

{
    "field1": "value1",
    "field2": "value2"
}
```

### 응답 구조 (Response Structure)

#### 성공 응답 (Success Response)

```http
HTTP/1.1 [STATUS_CODE] [STATUS_MESSAGE]
Content-Type: application/json

{
    "id": 1,
    "title": "할일 제목",
    "isCompleted": false
}
```

#### 에러 응답 (Error Response)

```http
HTTP/1.1 [ERROR_CODE] [ERROR_MESSAGE]
Content-Type: application/json

{
    "message": "Error description",
    "errors": {
        "field": ["Error detail"]
    }
}
```

---

## 🔗 엔드포인트 목록 (Endpoints)

### 전체 엔드포인트 요약 (Endpoint Summary)

| 메서드 | 엔드포인트 | 설명 | 인증 필요 |
|-------|----------|------|---------|
| GET | `/api/todos` | 모든 할일 조회 | ❌ |
| GET | `/api/todos/{id}` | 특정 할일 조회 | ❌ |
| POST | `/api/todos` | 새 할일 생성 | ❌ |
| PUT | `/api/todos/{id}` | 할일 수정 | ❌ |
| DELETE | `/api/todos/{id}` | 할일 삭제 | ❌ |

---

## 📝 엔드포인트 상세 (Endpoint Details)

### 1. 모든 할일 조회 (Get All To-Dos)

모든 할일 항목을 배열로 반환합니다.

Returns all to-do items as an array.

#### 요청 (Request)

```http
GET /api/todos HTTP/1.1
Host: localhost:5000
Accept: application/json
```

#### URL 파라미터 (URL Parameters)

없음 (None)

#### 쿼리 파라미터 (Query Parameters)

없음 (None)

**향후 추가 가능한 파라미터 (Future Possible Parameters):**
```
?completed=true          # 완료된 항목만
?sort=title             # 제목으로 정렬
?limit=10&offset=0      # 페이지네이션
```

#### 응답 (Response)

**성공 (Success) - 200 OK:**

```json
[
    {
        "id": 1,
        "title": "공부하기",
        "isCompleted": false
    },
    {
        "id": 2,
        "title": "운동하기",
        "isCompleted": true
    },
    {
        "id": 3,
        "title": "책 읽기",
        "isCompleted": false
    }
]
```

**빈 목록 (Empty List) - 200 OK:**

```json
[]
```

#### 예제 코드 (Example Code)

**JavaScript (fetch):**
```javascript
async function getAllTodos() {
    try {
        const response = await fetch('http://localhost:5000/api/todos', {
            method: 'GET',
            headers: {
                'Accept': 'application/json'
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const todos = await response.json();
        console.log('모든 할일:', todos);
        return todos;
    } catch (error) {
        console.error('에러:', error);
    }
}
```

**C# (HttpClient):**
```csharp
using System.Net.Http;
using System.Net.Http.Json;

public async Task<List<TodoItem>> GetAllTodosAsync()
{
    using var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5000");

    var response = await client.GetAsync("/api/todos");
    response.EnsureSuccessStatusCode();

    var todos = await response.Content.ReadFromJsonAsync<List<TodoItem>>();
    return todos;
}
```

**Python (requests):**
```python
import requests

def get_all_todos():
    url = "http://localhost:5000/api/todos"
    headers = {"Accept": "application/json"}

    response = requests.get(url, headers=headers)

    if response.status_code == 200:
        todos = response.json()
        print("모든 할일:", todos)
        return todos
    else:
        print(f"에러: {response.status_code}")
        return None
```

**curl:**
```bash
curl -X GET "http://localhost:5000/api/todos" \
     -H "Accept: application/json"
```

---

### 2. 특정 할일 조회 (Get To-Do By ID)

ID로 특정 할일 항목을 조회합니다.

Retrieves a specific to-do item by ID.

#### 요청 (Request)

```http
GET /api/todos/{id} HTTP/1.1
Host: localhost:5000
Accept: application/json
```

#### URL 파라미터 (URL Parameters)

| 파라미터 | 타입 | 필수 | 설명 |
|---------|------|------|------|
| id | integer | ✅ | 할일 ID (1 이상) |

#### 응답 (Response)

**성공 (Success) - 200 OK:**

```json
{
    "id": 1,
    "title": "공부하기",
    "isCompleted": false
}
```

**할일 없음 (Not Found) - 404 Not Found:**

```json
{
    "message": "Todo item not found"
}
```

#### 예제 코드 (Example Code)

**JavaScript:**
```javascript
async function getTodoById(id) {
    const response = await fetch(`http://localhost:5000/api/todos/${id}`, {
        method: 'GET',
        headers: { 'Accept': 'application/json' }
    });

    if (response.status === 404) {
        console.error('할일을 찾을 수 없습니다.');
        return null;
    }

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    const todo = await response.json();
    return todo;
}

// 사용 예시
const todo = await getTodoById(1);
console.log(todo); // { id: 1, title: "공부하기", isCompleted: false }
```

**curl:**
```bash
# 성공 케이스
curl -X GET "http://localhost:5000/api/todos/1" \
     -H "Accept: application/json"

# 404 케이스
curl -X GET "http://localhost:5000/api/todos/9999" \
     -H "Accept: application/json"
```

---

### 3. 새 할일 생성 (Create To-Do)

새로운 할일 항목을 생성합니다.

Creates a new to-do item.

#### 요청 (Request)

```http
POST /api/todos HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json

{
    "title": "새로운 할일"
}
```

#### 요청 본문 (Request Body)

| 필드 | 타입 | 필수 | 제약사항 | 설명 |
|-----|------|------|---------|------|
| title | string | ✅ | 1-200자 | 할일 제목 |

**참고 (Note):**
- `id`는 자동 생성됨 (서버에서 할당)
- `isCompleted`는 자동으로 `false`로 설정

#### 응답 (Response)

**성공 (Success) - 201 Created:**

```http
HTTP/1.1 201 Created
Content-Type: application/json
Location: http://localhost:5000/api/todos/3

{
    "id": 3,
    "title": "새로운 할일",
    "isCompleted": false
}
```

**검증 실패 (Validation Error) - 400 Bad Request:**

```json
{
    "errors": {
        "Title": [
            "The Title field is required.",
            "The Title field must be between 1 and 200 characters."
        ]
    }
}
```

#### 예제 코드 (Example Code)

**JavaScript:**
```javascript
async function createTodo(title) {
    const response = await fetch('http://localhost:5000/api/todos', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: JSON.stringify({ title: title })
    });

    if (!response.ok) {
        const errorData = await response.json();
        console.error('생성 실패:', errorData);
        throw new Error('Failed to create todo');
    }

    const newTodo = await response.json();
    console.log('생성된 할일:', newTodo);
    return newTodo;
}

// 사용 예시
const todo = await createTodo('책 읽기');
// 반환: { id: 3, title: "책 읽기", isCompleted: false }
```

**C#:**
```csharp
public async Task<TodoItem> CreateTodoAsync(string title)
{
    using var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5000");

    var newTodo = new { title = title };

    var response = await client.PostAsJsonAsync("/api/todos", newTodo);
    response.EnsureSuccessStatusCode();

    var createdTodo = await response.Content.ReadFromJsonAsync<TodoItem>();
    return createdTodo;
}
```

**curl:**
```bash
curl -X POST "http://localhost:5000/api/todos" \
     -H "Content-Type: application/json" \
     -H "Accept: application/json" \
     -d '{"title":"새로운 할일"}'
```

#### 검증 규칙 (Validation Rules)

| 규칙 | 설명 |
|-----|------|
| **Required** | title은 필수 입력 |
| **Length** | title은 1-200자 사이 |
| **Not Empty** | 빈 문자열 또는 공백만 있는 문자열 불가 |

**잘못된 요청 예시 (Invalid Request Examples):**

```javascript
// ❌ title 없음
await fetch('/api/todos', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({})
});
// 결과: 400 Bad Request - "The Title field is required."

// ❌ 빈 문자열
await fetch('/api/todos', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ title: "" })
});
// 결과: 400 Bad Request

// ❌ 너무 긴 제목 (200자 초과)
const longTitle = "A".repeat(201);
await fetch('/api/todos', {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ title: longTitle })
});
// 결과: 400 Bad Request
```

---

### 4. 할일 수정 (Update To-Do)

기존 할일 항목을 수정합니다.

Updates an existing to-do item.

#### 요청 (Request)

```http
PUT /api/todos/{id} HTTP/1.1
Host: localhost:5000
Content-Type: application/json
Accept: application/json

{
    "id": 1,
    "title": "수정된 할일",
    "isCompleted": true
}
```

#### URL 파라미터 (URL Parameters)

| 파라미터 | 타입 | 필수 | 설명 |
|---------|------|------|------|
| id | integer | ✅ | 수정할 할일의 ID |

#### 요청 본문 (Request Body)

| 필드 | 타입 | 필수 | 제약사항 | 설명 |
|-----|------|------|---------|------|
| id | integer | ✅ | URL의 id와 일치 | 할일 ID |
| title | string | ✅ | 1-200자 | 할일 제목 |
| isCompleted | boolean | ✅ | true/false | 완료 상태 |

**⚠️ 중요: URL의 ID와 본문의 ID가 일치해야 합니다.**

#### 응답 (Response)

**성공 (Success) - 204 No Content:**

```http
HTTP/1.1 204 No Content
```

응답 본문 없음 (No response body)

**ID 불일치 (ID Mismatch) - 400 Bad Request:**

```json
{
    "message": "ID mismatch between URL and body"
}
```

**할일 없음 (Not Found) - 404 Not Found:**

```json
{
    "message": "Todo item not found"
}
```

#### 예제 코드 (Example Code)

**JavaScript:**
```javascript
async function updateTodo(id, title, isCompleted) {
    const response = await fetch(`http://localhost:5000/api/todos/${id}`, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        },
        body: JSON.stringify({
            id: id,
            title: title,
            isCompleted: isCompleted
        })
    });

    if (response.status === 400) {
        console.error('ID가 일치하지 않습니다.');
        return false;
    }

    if (response.status === 404) {
        console.error('할일을 찾을 수 없습니다.');
        return false;
    }

    if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    console.log('할일이 성공적으로 수정되었습니다.');
    return true;
}

// 사용 예시
await updateTodo(1, '수정된 할일', true);
```

**curl:**
```bash
curl -X PUT "http://localhost:5000/api/todos/1" \
     -H "Content-Type: application/json" \
     -H "Accept: application/json" \
     -d '{"id":1,"title":"수정된 할일","isCompleted":true}'
```

#### 일반적인 사용 사례 (Common Use Cases)

**1. 제목만 수정 (Update title only):**
```javascript
// 현재 할일 가져오기
const todo = await getTodoById(1);

// 제목만 변경하고 나머지는 유지
await updateTodo(1, '새로운 제목', todo.isCompleted);
```

**2. 완료 상태만 토글 (Toggle completion only):**
```javascript
const todo = await getTodoById(1);
await updateTodo(1, todo.title, !todo.isCompleted);
```

**3. 전체 수정 (Full update):**
```javascript
await updateTodo(1, '완전히 새로운 할일', true);
```

---

### 5. 할일 삭제 (Delete To-Do)

할일 항목을 삭제합니다.

Deletes a to-do item.

#### 요청 (Request)

```http
DELETE /api/todos/{id} HTTP/1.1
Host: localhost:5000
```

#### URL 파라미터 (URL Parameters)

| 파라미터 | 타입 | 필수 | 설명 |
|---------|------|------|------|
| id | integer | ✅ | 삭제할 할일의 ID |

#### 요청 본문 (Request Body)

없음 (None)

#### 응답 (Response)

**성공 (Success) - 204 No Content:**

```http
HTTP/1.1 204 No Content
```

응답 본문 없음 (No response body)

**할일 없음 (Not Found) - 404 Not Found:**

```json
{
    "message": "Todo item not found"
}
```

#### 예제 코드 (Example Code)

**JavaScript:**
```javascript
async function deleteTodo(id) {
    // 사용자 확인
    if (!confirm('정말로 삭제하시겠습니까?')) {
        return false;
    }

    const response = await fetch(`http://localhost:5000/api/todos/${id}`, {
        method: 'DELETE'
    });

    if (response.status === 404) {
        console.error('할일을 찾을 수 없습니다.');
        return false;
    }

    if (!response.ok && response.status !== 204) {
        throw new Error(`HTTP error! status: ${response.status}`);
    }

    console.log('할일이 성공적으로 삭제되었습니다.');
    return true;
}

// 사용 예시
await deleteTodo(1);
```

**C#:**
```csharp
public async Task<bool> DeleteTodoAsync(int id)
{
    using var client = new HttpClient();
    client.BaseAddress = new Uri("http://localhost:5000");

    var response = await client.DeleteAsync($"/api/todos/{id}");

    if (response.StatusCode == HttpStatusCode.NotFound)
    {
        Console.WriteLine("할일을 찾을 수 없습니다.");
        return false;
    }

    response.EnsureSuccessStatusCode();
    Console.WriteLine("할일이 삭제되었습니다.");
    return true;
}
```

**curl:**
```bash
curl -X DELETE "http://localhost:5000/api/todos/1"
```

---

## 📊 데이터 모델 (Data Models)

### TodoItem

할일 항목을 나타내는 데이터 모델입니다.

Data model representing a to-do item.

#### 필드 (Fields)

| 필드 | 타입 | 필수 | 기본값 | 설명 |
|-----|------|------|-------|------|
| id | integer | - | 자동 생성 | 고유 식별자 (Primary Key) |
| title | string | ✅ | - | 할일 제목 (1-200자) |
| isCompleted | boolean | - | false | 완료 여부 |

#### JSON 스키마 (JSON Schema)

```json
{
    "$schema": "http://json-schema.org/draft-07/schema#",
    "type": "object",
    "properties": {
        "id": {
            "type": "integer",
            "minimum": 1,
            "description": "고유 식별자"
        },
        "title": {
            "type": "string",
            "minLength": 1,
            "maxLength": 200,
            "description": "할일 제목"
        },
        "isCompleted": {
            "type": "boolean",
            "description": "완료 상태"
        }
    },
    "required": ["title"]
}
```

#### C# 클래스 정의

```csharp
public class TodoItem
{
    /// <summary>
    /// 고유 식별자 (자동 증가)
    /// Unique identifier (auto-increment)
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 할일 제목 (1-200자 필수)
    /// To-do title (1-200 characters required)
    /// </summary>
    [Required]
    [StringLength(200, MinimumLength = 1)]
    public string Title { get; set; }

    /// <summary>
    /// 완료 여부 (기본값: false)
    /// Completion status (default: false)
    /// </summary>
    public bool IsCompleted { get; set; } = false;
}
```

#### TypeScript 인터페이스

```typescript
interface TodoItem {
    /**
     * 고유 식별자
     * Unique identifier
     */
    id: number;

    /**
     * 할일 제목
     * To-do title
     */
    title: string;

    /**
     * 완료 여부
     * Completion status
     */
    isCompleted: boolean;
}
```

---

## ⚠️ 에러 코드 (Error Codes)

### HTTP 상태 코드 설명

| 코드 | 이름 | 설명 | 발생 상황 |
|-----|------|------|----------|
| **200** | OK | 성공 | GET 요청 성공 |
| **201** | Created | 생성됨 | POST 요청으로 리소스 생성 성공 |
| **204** | No Content | 내용 없음 | PUT/DELETE 성공 (응답 본문 없음) |
| **400** | Bad Request | 잘못된 요청 | 검증 실패, ID 불일치 |
| **404** | Not Found | 찾을 수 없음 | 존재하지 않는 ID |
| **500** | Internal Server Error | 서버 오류 | 서버 내부 에러 |

### 에러 응답 형식

#### 검증 에러 (Validation Error)

```json
{
    "errors": {
        "Title": [
            "The Title field is required."
        ]
    }
}
```

#### 일반 에러 (General Error)

```json
{
    "message": "Error description here"
}
```

### 에러 처리 예시 (Error Handling Examples)

**JavaScript:**
```javascript
async function callApi() {
    try {
        const response = await fetch('http://localhost:5000/api/todos/1');

        // 상태 코드별 처리
        switch(response.status) {
            case 200:
                const data = await response.json();
                return data;

            case 404:
                console.error('할일을 찾을 수 없습니다.');
                return null;

            case 400:
                const errorData = await response.json();
                console.error('검증 에러:', errorData.errors);
                return null;

            case 500:
                console.error('서버 오류가 발생했습니다.');
                return null;

            default:
                console.error(`예상치 못한 에러: ${response.status}`);
                return null;
        }
    } catch (error) {
        console.error('네트워크 에러:', error);
        return null;
    }
}
```

---

## 💻 예제 코드 (Example Code)

### 완전한 CRUD 예제 (Complete CRUD Example)

#### JavaScript 클래스

```javascript
class TodoApiClient {
    constructor(baseUrl = 'http://localhost:5000') {
        this.baseUrl = baseUrl;
        this.apiPath = '/api/todos';
    }

    // 모든 할일 조회
    async getAll() {
        const response = await fetch(`${this.baseUrl}${this.apiPath}`);
        if (!response.ok) throw new Error('Failed to fetch todos');
        return await response.json();
    }

    // 특정 할일 조회
    async getById(id) {
        const response = await fetch(`${this.baseUrl}${this.apiPath}/${id}`);
        if (response.status === 404) return null;
        if (!response.ok) throw new Error('Failed to fetch todo');
        return await response.json();
    }

    // 할일 생성
    async create(title) {
        const response = await fetch(`${this.baseUrl}${this.apiPath}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ title })
        });
        if (!response.ok) throw new Error('Failed to create todo');
        return await response.json();
    }

    // 할일 수정
    async update(id, title, isCompleted) {
        const response = await fetch(`${this.baseUrl}${this.apiPath}/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ id, title, isCompleted })
        });
        if (!response.ok) throw new Error('Failed to update todo');
        return true;
    }

    // 할일 삭제
    async delete(id) {
        const response = await fetch(`${this.baseUrl}${this.apiPath}/${id}`, {
            method: 'DELETE'
        });
        if (!response.ok && response.status !== 204) {
            throw new Error('Failed to delete todo');
        }
        return true;
    }

    // 완료 상태 토글
    async toggleComplete(id) {
        const todo = await this.getById(id);
        if (!todo) return false;
        return await this.update(id, todo.title, !todo.isCompleted);
    }
}

// 사용 예시
const api = new TodoApiClient();

// 할일 생성
const newTodo = await api.create('새로운 할일');
console.log('생성됨:', newTodo);

// 모든 할일 조회
const allTodos = await api.getAll();
console.log('전체 목록:', allTodos);

// 할일 수정
await api.update(1, '수정된 제목', true);

// 완료 상태 토글
await api.toggleComplete(1);

// 할일 삭제
await api.delete(1);
```

---

## 🧪 테스팅 도구 (Testing Tools)

### Postman 컬렉션

**Postman을 사용한 API 테스트**

1. **Postman 다운로드**: https://www.postman.com/downloads/

2. **새 컬렉션 생성**: "To-Do API"

3. **요청 추가**:

#### GET All To-Dos
```
GET http://localhost:5000/api/todos
Headers:
  Accept: application/json
```

#### POST Create To-Do
```
POST http://localhost:5000/api/todos
Headers:
  Content-Type: application/json
  Accept: application/json
Body (raw JSON):
{
    "title": "Postman으로 생성한 할일"
}
```

#### PUT Update To-Do
```
PUT http://localhost:5000/api/todos/1
Headers:
  Content-Type: application/json
Body (raw JSON):
{
    "id": 1,
    "title": "수정된 할일",
    "isCompleted": true
}
```

#### DELETE To-Do
```
DELETE http://localhost:5000/api/todos/1
```

### curl 명령어 모음

```bash
# 모든 할일 조회
curl -X GET "http://localhost:5000/api/todos"

# 특정 할일 조회
curl -X GET "http://localhost:5000/api/todos/1"

# 할일 생성
curl -X POST "http://localhost:5000/api/todos" \
     -H "Content-Type: application/json" \
     -d '{"title":"curl로 생성"}'

# 할일 수정
curl -X PUT "http://localhost:5000/api/todos/1" \
     -H "Content-Type: application/json" \
     -d '{"id":1,"title":"curl로 수정","isCompleted":true}'

# 할일 삭제
curl -X DELETE "http://localhost:5000/api/todos/1"
```

### PowerShell 스크립트

```powershell
# 모든 할일 조회
Invoke-RestMethod -Uri "http://localhost:5000/api/todos" -Method Get

# 할일 생성
$body = @{
    title = "PowerShell로 생성"
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/todos" `
                  -Method Post `
                  -Body $body `
                  -ContentType "application/json"

# 할일 수정
$updateBody = @{
    id = 1
    title = "PowerShell로 수정"
    isCompleted = $true
} | ConvertTo-Json

Invoke-RestMethod -Uri "http://localhost:5000/api/todos/1" `
                  -Method Put `
                  -Body $updateBody `
                  -ContentType "application/json"

# 할일 삭제
Invoke-RestMethod -Uri "http://localhost:5000/api/todos/1" -Method Delete
```

---

## 🔍 API 테스트 시나리오

### 시나리오 1: 기본 CRUD 흐름

```javascript
// 1. 초기 상태 확인
const initial = await api.getAll();
console.log('초기 할일 수:', initial.length);

// 2. 새 할일 3개 추가
await api.create('할일 1');
await api.create('할일 2');
await api.create('할일 3');

// 3. 목록 확인
const afterCreate = await api.getAll();
console.log('추가 후 할일 수:', afterCreate.length);

// 4. 첫 번째 할일 완료 처리
const firstTodo = afterCreate[0];
await api.update(firstTodo.id, firstTodo.title, true);

// 5. 두 번째 할일 수정
const secondTodo = afterCreate[1];
await api.update(secondTodo.id, '수정된 제목', false);

// 6. 세 번째 할일 삭제
const thirdTodo = afterCreate[2];
await api.delete(thirdTodo.id);

// 7. 최종 상태 확인
const final = await api.getAll();
console.log('최종 할일 수:', final.length);
console.log('최종 목록:', final);
```

### 시나리오 2: 에러 처리 테스트

```javascript
// 1. 존재하지 않는 ID 조회
const notFound = await api.getById(9999);
console.log('존재하지 않는 할일:', notFound); // null

// 2. 빈 제목으로 생성 시도
try {
    await api.create('');
} catch (error) {
    console.log('검증 에러 발생:', error.message);
}

// 3. ID 불일치로 수정 시도
try {
    await fetch('http://localhost:5000/api/todos/1', {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ id: 2, title: 'test', isCompleted: false })
    });
} catch (error) {
    console.log('ID 불일치 에러:', error.message);
}
```

---

## 📈 성능 및 제한사항

### 현재 제한사항

| 항목 | 제한 | 설명 |
|-----|------|------|
| **Rate Limiting** | 없음 | 프로덕션에서는 구현 필요 |
| **페이지네이션** | 없음 | 모든 데이터 반환 |
| **정렬** | 없음 | ID 순서로만 반환 |
| **필터링** | 없음 | 클라이언트 측에서 처리 |
| **검색** | 없음 | 향후 추가 가능 |
| **인증** | 없음 | 학습 목적으로 미구현 |

### 권장 사항 (Recommendations)

**프로덕션 환경을 위한 개선사항:**

1. **페이지네이션 추가**
   ```
   GET /api/todos?page=1&pageSize=10
   ```

2. **정렬 기능**
   ```
   GET /api/todos?sortBy=title&order=asc
   ```

3. **필터링**
   ```
   GET /api/todos?completed=true
   ```

4. **검색**
   ```
   GET /api/todos?search=공부
   ```

5. **Rate Limiting**
   - 분당 요청 수 제한
   - IP 기반 또는 API 키 기반

---

## 📞 지원 및 문의

이 API는 학습 목적으로 만들어졌습니다.

This API was created for learning purposes.

**질문이나 제안사항이 있으시면:**

- GitHub Issues를 통해 문의
- 이메일로 연락
- Pull Request를 통한 기여 환영

---

## 📝 버전 이력 (Version History)

### v1.0.0 (Current)
- ✅ 기본 CRUD 기능
- ✅ RESTful API 구현
- ✅ CORS 지원
- ✅ Entity Framework Core 통합

### 향후 계획 (Future Plans)
- 🔜 인증 시스템 (JWT)
- 🔜 페이지네이션
- 🔜 정렬 및 필터링
- 🔜 검색 기능
- 🔜 카테고리 기능

---

**Happy API Testing! 즐거운 API 테스팅 되세요! 🚀**
