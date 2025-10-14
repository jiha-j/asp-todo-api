using Microsoft.AspNetCore.Mvc;
using TodoApi.Models;
using TodoApi.Services;

// ==========================================
// TodoController - REST API Controller
// ==========================================
//
// [한글 설명]
// 이것은 REST API의 진입점입니다.
// Java Spring의 @RestController와 동일한 역할을 합니다.
// HTTP 요청을 받아 적절한 서비스 메서드를 호출하고, HTTP 응답을 반환합니다.
//
// [English Explanation]
// This is the entry point of REST API.
// Same role as @RestController in Java Spring.
// Receives HTTP requests, calls appropriate service methods, and returns HTTP responses.
//
namespace TodoApi.Controllers
{
    // ==========================================
    // Controller Attributes (어노테이션)
    // ==========================================
    //
    // [한글] [ApiController]는 이 클래스가 API Controller임을 나타냅니다.
    //       자동으로 다음 기능을 제공합니다:
    //       - 자동 모델 유효성 검사 (Automatic model validation)
    //       - 자동 400 Bad Request 응답 (Automatic 400 responses)
    //       - 바인딩 소스 추론 (Binding source inference)
    //
    // [English] [ApiController] indicates this class is an API Controller.
    //           Automatically provides:
    //           - Automatic model validation
    //           - Automatic 400 Bad Request responses
    //           - Binding source inference
    //
    // Java Spring 비교:
    // [ApiController] + [Route] → @RestController + @RequestMapping
    [ApiController]
    [Route("api/todos")]
    public class TodoController : ControllerBase
    {
        // ==========================================
        // Route Explanation (라우팅 설명)
        // ==========================================
        //
        // [한글] [Route("api/todos")]는 이 Controller의 기본 경로를 설정합니다.
        //       모든 엔드포인트는 "api/todos" 경로로 시작합니다.
        //       RESTful 관례에 따라 복수형(todos)을 사용합니다.
        //
        // [English] [Route("api/todos")] sets base path for this Controller.
        //           All endpoints start with "api/todos" path.
        //           Use plural form (todos) according to RESTful convention.
        //
        // Java Spring 비교:
        // @RequestMapping("/api/todos")

        // ==========================================
        // Dependency Injection
        // ==========================================
        //
        // [한글] 생성자를 통해 ITodoService를 주입받습니다.
        //       Java Spring의 @Autowired 생성자 주입과 동일합니다.
        //
        // [English] Receive ITodoService through constructor injection.
        //           Same as @Autowired constructor injection in Java Spring.
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // ==========================================
        // GET: api/todo
        // ==========================================
        // [한글] 모든 Todo 항목을 조회하는 엔드포인트입니다.
        //       HTTP GET 요청을 처리합니다.
        //
        // [English] Endpoint to retrieve all todo items.
        //           Handles HTTP GET requests.
        //
        // Java Spring 비교:
        // @GetMapping("")
        // public List<TodoItem> getAllTodos()
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllTodos()
        {
            // ==========================================
            // ActionResult<T> Explanation
            // ==========================================
            //
            // [한글] ActionResult<T>는 HTTP 응답을 나타냅니다.
            //       T는 성공 시 반환할 데이터 타입입니다.
            //       다양한 HTTP 상태 코드와 함께 데이터를 반환할 수 있습니다.
            //
            // [English] ActionResult<T> represents HTTP response.
            //           T is the data type to return on success.
            //           Can return data with various HTTP status codes.
            //
            // Java Spring 비교:
            // ActionResult<T> → ResponseEntity<T>

            var todos = await _todoService.GetAllTodosAsync();

            // [한글] Ok(data)는 200 OK 응답과 함께 데이터를 반환합니다.
            //       JSON 형식으로 자동 직렬화됩니다.
            //
            // [English] Ok(data) returns 200 OK response with data.
            //           Automatically serialized to JSON format.
            //
            // HTTP Response:
            // Status: 200 OK
            // Body: [{"id":1,"title":"...","description":"...","isCompleted":false,"createdAt":"..."}]
            return Ok(todos);
        }

        // ==========================================
        // GET: api/todo/{id}
        // ==========================================
        // [한글] 특정 ID의 Todo 항목을 조회하는 엔드포인트입니다.
        //       경로에서 ID를 추출하여 사용합니다.
        //
        // [English] Endpoint to retrieve specific todo item by ID.
        //           Extracts ID from route path.
        //
        // 예시 요청 (Example Request):
        // GET http://localhost:5000/api/todos/1
        //
        // Java Spring 비교:
        // @GetMapping("/{id}")
        // public ResponseEntity<TodoItem> getTodoById(@PathVariable int id)
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoById(int id)
        {
            // ==========================================
            // Route Parameter Binding
            // ==========================================
            //
            // [한글] {id}는 라우트 파라미터입니다.
            //       URL의 값이 자동으로 int id 매개변수에 바인딩됩니다.
            //       Java Spring의 @PathVariable과 동일합니다.
            //
            // [English] {id} is route parameter.
            //           URL value is automatically bound to int id parameter.
            //           Same as @PathVariable in Java Spring.

            var todo = await _todoService.GetTodoByIdAsync(id);

            if (todo == null)
            {
                // [한글] NotFound()는 404 Not Found 응답을 반환합니다.
                //       클라이언트에게 요청한 리소스가 존재하지 않음을 알립니다.
                //
                // [English] NotFound() returns 404 Not Found response.
                //           Informs client that requested resource doesn't exist.
                //
                // HTTP Response:
                // Status: 404 Not Found
                return NotFound(new { message = $"Todo with id {id} not found" });
            }

            // [한글] 항목이 존재하면 200 OK와 함께 데이터를 반환합니다.
            // [English] If item exists, return 200 OK with data.
            return Ok(todo);
        }

        // ==========================================
        // GET: api/todo/status/{isCompleted}
        // ==========================================
        // [한글] 완료 상태로 Todo 항목을 필터링하는 엔드포인트입니다.
        //       추가적인 비즈니스 로직의 예시입니다.
        //
        // [English] Endpoint to filter todo items by completion status.
        //           Example of additional business logic.
        //
        // 예시 요청 (Example Requests):
        // GET http://localhost:5000/api/todos/status/true   → 완료된 항목
        // GET http://localhost:5000/api/todos/status/false  → 미완료 항목
        [HttpGet("status/{isCompleted}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodosByStatus(bool isCompleted)
        {
            var todos = await _todoService.GetTodosByStatusAsync(isCompleted);
            return Ok(todos);
        }

        // ==========================================
        // POST: api/todo
        // ==========================================
        // [한글] 새로운 Todo 항목을 생성하는 엔드포인트입니다.
        //       HTTP POST 요청을 처리하고 201 Created 응답을 반환합니다.
        //
        // [English] Endpoint to create new todo item.
        //           Handles HTTP POST request and returns 201 Created response.
        //
        // 예시 요청 (Example Request):
        // POST http://localhost:5000/api/todos
        // Content-Type: application/json
        // {
        //   "title": "Learn ASP.NET Core",
        //   "description": "Complete the tutorial",
        //   "isCompleted": false
        // }
        //
        // Java Spring 비교:
        // @PostMapping("")
        // public ResponseEntity<TodoItem> createTodo(@RequestBody TodoItem item)
        [HttpPost]
        public async Task<ActionResult<TodoItem>> CreateTodo([FromBody] TodoItem todoItem)
        {
            // ==========================================
            // [FromBody] Attribute Explanation
            // ==========================================
            //
            // [한글] [FromBody]는 HTTP 요청 본문에서 데이터를 읽어옵니다.
            //       JSON 데이터가 자동으로 TodoItem 객체로 역직렬화됩니다.
            //       [ApiController] 속성 덕분에 [FromBody]는 생략 가능합니다 (자동 추론).
            //
            // [English] [FromBody] reads data from HTTP request body.
            //           JSON data is automatically deserialized to TodoItem object.
            //           Thanks to [ApiController], [FromBody] can be omitted (auto-inferred).
            //
            // Java Spring 비교:
            // [FromBody] TodoItem → @RequestBody TodoItem

            // ==========================================
            // Automatic Model Validation
            // ==========================================
            //
            // [한글] [ApiController] 속성 덕분에 모델 유효성 검사가 자동으로 수행됩니다.
            //       TodoItem의 [Required], [StringLength] 등의 속성이 자동으로 검증됩니다.
            //       유효하지 않으면 자동으로 400 Bad Request가 반환됩니다.
            //
            // [English] Thanks to [ApiController], model validation is automatic.
            //           TodoItem's [Required], [StringLength] etc. are automatically validated.
            //           If invalid, 400 Bad Request is automatically returned.
            //
            // 수동 검증 예시 (Manual validation example):
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            var createdTodo = await _todoService.CreateTodoAsync(todoItem);

            // ==========================================
            // CreatedAtAction Response
            // ==========================================
            //
            // [한글] CreatedAtAction()은 201 Created 응답을 반환합니다.
            //       세 가지 매개변수:
            //       1. nameof(GetTodoById): 생성된 리소스를 조회할 수 있는 액션 이름
            //       2. new { id = createdTodo.Id }: 해당 액션의 라우트 파라미터
            //       3. createdTodo: 응답 본문에 포함할 데이터
            //
            //       Location 헤더에 생성된 리소스의 URL이 포함됩니다:
            //       Location: http://localhost:5000/api/todos/1
            //
            // [English] CreatedAtAction() returns 201 Created response.
            //           Three parameters:
            //           1. nameof(GetTodoById): Action name to retrieve created resource
            //           2. new { id = createdTodo.Id }: Route parameters for that action
            //           3. createdTodo: Data to include in response body
            //
            //           Location header includes URL of created resource:
            //           Location: http://localhost:5000/api/todos/1
            //
            // HTTP Response:
            // Status: 201 Created
            // Location: http://localhost:5000/api/todos/1
            // Body: {"id":1,"title":"...","description":"...","isCompleted":false,"createdAt":"..."}
            //
            // Java Spring 비교:
            // return ResponseEntity.created(URI.create("/api/todos/" + id)).body(createdTodo);
            return CreatedAtAction(
                nameof(GetTodoById),
                new { id = createdTodo.Id },
                createdTodo
            );
        }

        // ==========================================
        // PUT: api/todo/{id}
        // ==========================================
        // [한글] 기존 Todo 항목을 수정하는 엔드포인트입니다.
        //       HTTP PUT 요청을 처리하고 204 No Content 응답을 반환합니다.
        //
        // [English] Endpoint to update existing todo item.
        //           Handles HTTP PUT request and returns 204 No Content response.
        //
        // 예시 요청 (Example Request):
        // PUT http://localhost:5000/api/todos/1
        // Content-Type: application/json
        // {
        //   "title": "Learn ASP.NET Core - Updated",
        //   "description": "Complete the advanced tutorial",
        //   "isCompleted": true
        // }
        //
        // Java Spring 비교:
        // @PutMapping("/{id}")
        // public ResponseEntity<Void> updateTodo(@PathVariable int id, @RequestBody TodoItem item)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodo(int id, [FromBody] TodoItem todoItem)
        {
            // ==========================================
            // IActionResult vs ActionResult<T>
            // ==========================================
            //
            // [한글] IActionResult는 데이터 없이 상태 코드만 반환할 때 사용합니다.
            //       ActionResult<T>는 데이터와 함께 반환할 때 사용합니다.
            //       PUT/DELETE는 보통 데이터를 반환하지 않으므로 IActionResult를 사용합니다.
            //
            // [English] IActionResult is used when returning only status code without data.
            //           ActionResult<T> is used when returning with data.
            //           PUT/DELETE usually don't return data, so use IActionResult.

            // ==========================================
            // Business Logic Validation
            // ==========================================
            //
            // [한글] URL의 ID와 요청 본문의 ID가 일치하는지 확인합니다.
            //       (선택사항: 보안 및 일관성을 위해 권장됩니다)
            //
            // [English] Check if URL's ID matches request body's ID.
            //           (Optional: Recommended for security and consistency)
            if (id != todoItem.Id && todoItem.Id != 0)
            {
                return BadRequest(new { message = "ID mismatch between URL and request body" });
            }

            // [한글] 서비스 레이어에 업데이트 요청
            // [English] Request update to service layer
            var success = await _todoService.UpdateTodoAsync(id, todoItem);

            if (!success)
            {
                // [한글] 항목이 존재하지 않으면 404 반환
                // [English] Return 404 if item doesn't exist
                return NotFound(new { message = $"Todo with id {id} not found" });
            }

            // ==========================================
            // NoContent Response
            // ==========================================
            //
            // [한글] NoContent()는 204 No Content 응답을 반환합니다.
            //       수정 성공을 나타내지만 응답 본문은 없습니다.
            //       REST API의 PUT 메서드에서 일반적으로 사용됩니다.
            //
            // [English] NoContent() returns 204 No Content response.
            //           Indicates successful update but no response body.
            //           Commonly used in REST API PUT methods.
            //
            // HTTP Response:
            // Status: 204 No Content
            //
            // Java Spring 비교:
            // return ResponseEntity.noContent().build();
            return NoContent();

            // [한글] 대안: 수정된 데이터를 반환하고 싶다면 Ok()를 사용할 수 있습니다
            // [English] Alternative: Can use Ok() if you want to return updated data
            // return Ok(await _todoService.GetTodoByIdAsync(id));
        }

        // ==========================================
        // DELETE: api/todo/{id}
        // ==========================================
        // [한글] Todo 항목을 삭제하는 엔드포인트입니다.
        //       HTTP DELETE 요청을 처리하고 204 No Content 응답을 반환합니다.
        //
        // [English] Endpoint to delete todo item.
        //           Handles HTTP DELETE request and returns 204 No Content response.
        //
        // 예시 요청 (Example Request):
        // DELETE http://localhost:5000/api/todos/1
        //
        // Java Spring 비교:
        // @DeleteMapping("/{id}")
        // public ResponseEntity<Void> deleteTodo(@PathVariable int id)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo(int id)
        {
            var success = await _todoService.DeleteTodoAsync(id);

            if (!success)
            {
                // [한글] 삭제할 항목이 존재하지 않으면 404 반환
                // [English] Return 404 if item to delete doesn't exist
                return NotFound(new { message = $"Todo with id {id} not found" });
            }

            // [한글] 삭제 성공 시 204 No Content 반환
            // [English] Return 204 No Content on successful deletion
            return NoContent();
        }

        // ==========================================
        // HTTP Status Codes Summary
        // ==========================================
        //
        // [한글] REST API에서 사용하는 주요 HTTP 상태 코드:
        // [English] Main HTTP status codes used in REST API:
        //
        // 200 OK              - 요청 성공, 데이터 반환 (GET)
        //                     - Request successful, return data (GET)
        //
        // 201 Created         - 리소스 생성 성공 (POST)
        //                     - Resource created successfully (POST)
        //
        // 204 No Content      - 요청 성공, 데이터 없음 (PUT, DELETE)
        //                     - Request successful, no data (PUT, DELETE)
        //
        // 400 Bad Request     - 잘못된 요청 (유효성 검사 실패)
        //                     - Invalid request (validation failed)
        //
        // 404 Not Found       - 리소스를 찾을 수 없음
        //                     - Resource not found
        //
        // 500 Internal Error  - 서버 오류 (예외 발생)
        //                     - Server error (exception occurred)

        // ==========================================
        // RESTful API Best Practices
        // ==========================================
        //
        // [한글] RESTful API 설계 원칙:
        // 1. 리소스는 명사로 표현 (todos, not getTodos)
        // 2. HTTP 메서드로 동작 표현 (GET, POST, PUT, DELETE)
        // 3. 적절한 HTTP 상태 코드 사용
        // 4. 일관된 URL 구조 (/api/resource/{id})
        // 5. 복수형 리소스 이름 사용 (todos, not todo)
        //
        // [English] RESTful API design principles:
        // 1. Resources expressed as nouns (todos, not getTodos)
        // 2. Actions expressed with HTTP methods (GET, POST, PUT, DELETE)
        // 3. Use appropriate HTTP status codes
        // 4. Consistent URL structure (/api/resource/{id})
        // 5. Use plural resource names (todos, not todo)
        //
        // URL 설계 예시 (URL design examples):
        // GET    /api/todos          → 모든 항목 조회
        // GET    /api/todos/{id}     → 특정 항목 조회
        // POST   /api/todos          → 새 항목 생성
        // PUT    /api/todos/{id}     → 항목 수정
        // DELETE /api/todos/{id}     → 항목 삭제

        // ==========================================
        // Additional Features (Optional)
        // ==========================================
        //
        // [한글] 실제 프로젝트에서 추가할 수 있는 기능:
        // 1. 페이징 (Pagination): GetTodos(int page, int pageSize)
        // 2. 정렬 (Sorting): GetTodos(string sortBy, string order)
        // 3. 필터링 (Filtering): GetTodos(string? searchTerm)
        // 4. PATCH 메서드: 부분 업데이트 지원
        // 5. 에러 핸들링: try-catch 블록으로 예외 처리
        // 6. 로깅: ILogger를 사용한 요청/응답 로깅
        // 7. 인증/인가: [Authorize] 속성으로 보안 강화
        //
        // [English] Features to add in real projects:
        // 1. Pagination: GetTodos(int page, int pageSize)
        // 2. Sorting: GetTodos(string sortBy, string order)
        // 3. Filtering: GetTodos(string? searchTerm)
        // 4. PATCH method: Support partial updates
        // 5. Error handling: Exception handling with try-catch
        // 6. Logging: Request/response logging with ILogger
        // 7. Authentication/Authorization: Secure with [Authorize]
    }

    // ==========================================
    // ASP.NET Core vs Java Spring Comparison
    // ==========================================
    //
    // ASP.NET Core                     →  Java Spring
    // ------------------------------------------------------
    // [ApiController]                  →  @RestController
    // [Route("api/[controller]")]      →  @RequestMapping("/api/...")
    // [HttpGet]                        →  @GetMapping
    // [HttpPost]                       →  @PostMapping
    // [HttpPut]                        →  @PutMapping
    // [HttpDelete]                     →  @DeleteMapping
    // [FromBody]                       →  @RequestBody
    // {id} in route                    →  @PathVariable
    // ActionResult<T>                  →  ResponseEntity<T>
    // Ok(data)                         →  ResponseEntity.ok(data)
    // CreatedAtAction(...)             →  ResponseEntity.created(...).body(data)
    // NoContent()                      →  ResponseEntity.noContent().build()
    // NotFound()                       →  ResponseEntity.notFound().build()
    // BadRequest()                     →  ResponseEntity.badRequest().body(...)
}
