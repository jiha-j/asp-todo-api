using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

// ==========================================
// TodoService Implementation
// ==========================================
//
// [한글 설명]
// ITodoService 인터페이스의 구체적인 구현 클래스입니다.
// Java Spring의 @Service 구현체와 동일한 역할을 합니다.
// 비즈니스 로직을 처리하고, 데이터베이스 작업을 수행합니다.
//
// [English Explanation]
// Concrete implementation of ITodoService interface.
// Same role as @Service implementation in Java Spring.
// Handles business logic and performs database operations.
//
namespace TodoApi.Services
{
    /// <summary>
    /// Todo 서비스 구현체 - 비즈니스 로직 실행
    /// Todo service implementation - Executes business logic
    ///
    /// [한글] 이 클래스는 ITodoService를 구현하고 TodoContext를 사용하여 데이터베이스 작업을 수행합니다.
    /// [English] This class implements ITodoService and uses TodoContext for database operations.
    /// </summary>
    public class TodoService : ITodoService
    {
        // ==========================================
        // Dependency Injection Field
        // ==========================================
        //
        // [한글] private readonly 필드로 의존성을 선언합니다.
        //       readonly는 생성자에서만 값을 할당할 수 있음을 의미합니다 (Java의 final과 유사).
        //       '_context'에서 '_'는 private 필드임을 나타내는 C# 관례입니다.
        //
        // [English] Declare dependency as private readonly field.
        //           readonly means can only be assigned in constructor (similar to Java's final).
        //           '_' prefix in '_context' is C# convention for private fields.
        private readonly TodoContext _context;

        // ==========================================
        // Constructor with Dependency Injection
        // ==========================================
        //
        // [한글] 생성자 주입(Constructor Injection) 패턴입니다.
        //       ASP.NET Core의 DI 컨테이너가 자동으로 TodoContext를 주입합니다.
        //       Java Spring의 다음 코드와 동일합니다:
        //       @Autowired
        //       public TodoService(TodoContext context) { this.context = context; }
        //
        // [English] Constructor Injection pattern.
        //           ASP.NET Core's DI container automatically injects TodoContext.
        //           Equivalent to this Java Spring code:
        //           @Autowired
        //           public TodoService(TodoContext context) { this.context = context; }
        //
        // DI 흐름 (DI Flow):
        // 1. Program.cs에서 services.AddScoped<ITodoService, TodoService>() 등록
        // 2. Controller가 ITodoService를 요청
        // 3. DI Container가 TodoService 인스턴스 생성
        // 4. TodoService 생성 시 필요한 TodoContext를 자동 주입
        // 5. Controller에 TodoService 전달
        public TodoService(TodoContext context)
        {
            _context = context;
        }

        // ==========================================
        // GetAllTodosAsync - 모든 Todo 조회
        // ==========================================
        //
        // [한글] 모든 Todo 항목을 비동기로 조회합니다.
        //       async 키워드는 이 메서드가 비동기임을 나타냅니다.
        //       await 키워드는 비동기 작업이 완료될 때까지 기다립니다.
        //
        // [English] Retrieves all todo items asynchronously.
        //           async keyword indicates this method is asynchronous.
        //           await keyword waits for async operation to complete.
        public async Task<IEnumerable<TodoItem>> GetAllTodosAsync()
        {
            // [한글] _context.Todos는 DbSet<TodoItem>입니다.
            //       ToListAsync()는 데이터베이스에서 모든 레코드를 조회하고 List로 변환합니다.
            //       SQL: SELECT * FROM TodoItems
            //
            // [English] _context.Todos is DbSet<TodoItem>.
            //           ToListAsync() queries all records from database and converts to List.
            //           SQL: SELECT * FROM TodoItems
            //
            // Java Spring JPA 비교:
            // return todoRepository.findAll();
            return await _context.Todos.ToListAsync();
        }

        // ==========================================
        // GetTodoByIdAsync - ID로 특정 Todo 조회
        // ==========================================
        //
        // [한글] 지정된 ID의 Todo 항목을 조회합니다.
        //       FindAsync()는 기본 키로 항목을 찾는 최적화된 메서드입니다.
        //       항목이 없으면 null을 반환합니다.
        //
        // [English] Retrieves todo item with specified ID.
        //           FindAsync() is optimized method to find by primary key.
        //           Returns null if item not found.
        public async Task<TodoItem?> GetTodoByIdAsync(int id)
        {
            // [한글] FindAsync(id)는 기본 키로 검색합니다.
            //       SQL: SELECT * FROM TodoItems WHERE Id = @id
            //
            // [English] FindAsync(id) searches by primary key.
            //           SQL: SELECT * FROM TodoItems WHERE Id = @id
            //
            // Java Spring JPA 비교:
            // return todoRepository.findById(id).orElse(null);
            return await _context.Todos.FindAsync(id);
        }

        // ==========================================
        // CreateTodoAsync - 새 Todo 생성
        // ==========================================
        //
        // [한글] 새로운 Todo 항목을 데이터베이스에 추가합니다.
        //       Add()로 추가하고, SaveChangesAsync()로 커밋합니다.
        //
        // [English] Adds new todo item to database.
        //           Add() stages the item, SaveChangesAsync() commits.
        public async Task<TodoItem> CreateTodoAsync(TodoItem todoItem)
        {
            // ==========================================
            // Business Logic - Set Creation Timestamp
            // ==========================================
            // [한글] 비즈니스 로직: 생성 시간을 현재 시간으로 설정합니다.
            //       클라이언트가 보낸 CreatedAt 값을 무시하고 서버 시간을 사용합니다.
            //       이렇게 하면 클라이언트의 시간 조작을 방지할 수 있습니다.
            //
            // [English] Business logic: Set creation time to current time.
            //           Ignores CreatedAt value from client and uses server time.
            //           This prevents client-side time manipulation.
            var now = DateTime.UtcNow;
            todoItem.CreatedAt = now;
            todoItem.UpdatedAt = now; // 생성 시 UpdatedAt도 같은 값으로 설정

            // ==========================================
            // Add to DbSet (Not yet saved to database)
            // ==========================================
            // [한글] Add()는 Entity를 DbContext의 변경 추적 시스템에 추가합니다.
            //       아직 데이터베이스에 저장되지 않았습니다 (Pending 상태).
            //       Java JPA의 persist()와 유사합니다.
            //
            // [English] Add() adds entity to DbContext's change tracking system.
            //           Not yet saved to database (Pending state).
            //           Similar to persist() in Java JPA.
            _context.Todos.Add(todoItem);

            // ==========================================
            // Save Changes to Database
            // ==========================================
            // [한글] SaveChangesAsync()는 모든 변경사항을 데이터베이스에 커밋합니다.
            //       이 시점에 실제 INSERT 쿼리가 실행됩니다.
            //       SQL: INSERT INTO TodoItems (Title, Description, ...) VALUES (...)
            //       저장 후 todoItem.Id에 자동 생성된 ID가 할당됩니다.
            //
            // [English] SaveChangesAsync() commits all changes to database.
            //           Actual INSERT query executes at this point.
            //           SQL: INSERT INTO TodoItems (Title, Description, ...) VALUES (...)
            //           After save, auto-generated ID is assigned to todoItem.Id.
            //
            // Java Spring JPA 비교:
            // return todoRepository.save(todoItem);
            await _context.SaveChangesAsync();

            // [한글] 생성된 항목(ID 포함)을 반환합니다.
            // [English] Return created item (with ID).
            return todoItem;
        }

        // ==========================================
        // UpdateTodoAsync - 기존 Todo 수정
        // ==========================================
        //
        // [한글] 기존 Todo 항목을 수정합니다.
        //       1) 먼저 항목이 존재하는지 확인
        //       2) 존재하면 필드 업데이트
        //       3) SaveChangesAsync()로 변경사항 저장
        //
        // [English] Updates existing todo item.
        //           1) First check if item exists
        //           2) If exists, update fields
        //           3) Save changes with SaveChangesAsync()
        public async Task<bool> UpdateTodoAsync(int id, TodoItem todoItem)
        {
            // ==========================================
            // Validation - Check if item exists
            // ==========================================
            // [한글] 수정하려는 항목이 실제로 존재하는지 확인합니다.
            //       존재하지 않으면 false를 반환하여 Controller가 404 응답을 보낼 수 있게 합니다.
            //
            // [English] Check if the item to update actually exists.
            //           If not exists, return false so Controller can send 404 response.
            var existingTodo = await _context.Todos.FindAsync(id);
            if (existingTodo == null)
            {
                return false; // 항목이 존재하지 않음 / Item not found
            }

            // ==========================================
            // Update Entity Properties
            // ==========================================
            // [한글] 기존 항목의 필드를 새 값으로 업데이트합니다.
            //       Id와 CreatedAt는 변경하지 않습니다 (불변 필드).
            //       Entity Framework의 변경 추적 시스템이 자동으로 변경을 감지합니다.
            //
            // [English] Update existing item's fields with new values.
            //           Don't change Id and CreatedAt (immutable fields).
            //           Entity Framework's change tracking automatically detects changes.
            existingTodo.Title = todoItem.Title;
            existingTodo.Description = todoItem.Description;
            existingTodo.IsCompleted = todoItem.IsCompleted;

            // 새로 추가된 필드들 업데이트 / Update newly added fields
            existingTodo.DueDate = todoItem.DueDate;
            existingTodo.Priority = todoItem.Priority;
            existingTodo.Category = todoItem.Category;
            existingTodo.Tags = todoItem.Tags;

            // 수정 시간 자동 업데이트 / Auto-update modification time
            existingTodo.UpdatedAt = DateTime.UtcNow;

            // ==========================================
            // Alternative Update Method (Commented)
            // ==========================================
            // [한글] 대안적 방법: Update()를 명시적으로 호출할 수도 있습니다.
            //       하지만 변경 추적 시스템이 이미 변경을 감지했으므로 불필요합니다.
            //
            // [English] Alternative: Can explicitly call Update().
            //           But unnecessary as change tracking already detected changes.
            // _context.Todos.Update(existingTodo);

            // ==========================================
            // Try-Catch for Concurrency Issues
            // ==========================================
            // [한글] DbUpdateConcurrencyException 처리:
            //       여러 사용자가 동시에 같은 항목을 수정하려고 할 때 발생할 수 있습니다.
            //       이 경우 false를 반환하여 클라이언트가 재시도하도록 합니다.
            //
            // [English] Handle DbUpdateConcurrencyException:
            //           Can occur when multiple users try to update same item simultaneously.
            //           Return false to let client retry.
            try
            {
                await _context.SaveChangesAsync();
                // SQL: UPDATE TodoItems SET Title=@title, Description=@desc, ... WHERE Id=@id
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                // [한글] 동시성 충돌 발생
                // [English] Concurrency conflict occurred
                return false;
            }
        }

        // ==========================================
        // DeleteTodoAsync - Todo 삭제
        // ==========================================
        //
        // [한글] 지정된 ID의 Todo 항목을 삭제합니다.
        //       1) 항목이 존재하는지 확인
        //       2) 존재하면 Remove()로 삭제 표시
        //       3) SaveChangesAsync()로 실제 삭제 실행
        //
        // [English] Deletes todo item with specified ID.
        //           1) Check if item exists
        //           2) If exists, mark for deletion with Remove()
        //           3) Execute actual deletion with SaveChangesAsync()
        public async Task<bool> DeleteTodoAsync(int id)
        {
            // ==========================================
            // Find Item to Delete
            // ==========================================
            var todoItem = await _context.Todos.FindAsync(id);
            if (todoItem == null)
            {
                return false; // 항목이 존재하지 않음 / Item not found
            }

            // ==========================================
            // Mark for Deletion
            // ==========================================
            // [한글] Remove()는 Entity를 삭제 대기 상태로 표시합니다.
            //       실제 삭제는 SaveChangesAsync()에서 실행됩니다.
            //       Java JPA의 remove()와 유사합니다.
            //
            // [English] Remove() marks entity for deletion.
            //           Actual deletion executes in SaveChangesAsync().
            //           Similar to remove() in Java JPA.
            _context.Todos.Remove(todoItem);

            // ==========================================
            // Execute Deletion
            // ==========================================
            // [한글] 데이터베이스에서 실제로 삭제합니다.
            //       SQL: DELETE FROM TodoItems WHERE Id = @id
            //
            // [English] Actually delete from database.
            //           SQL: DELETE FROM TodoItems WHERE Id = @id
            //
            // Java Spring JPA 비교:
            // todoRepository.deleteById(id);
            await _context.SaveChangesAsync();
            return true;
        }

        // ==========================================
        // GetTodosByStatusAsync - 상태별 필터링
        // ==========================================
        //
        // [한글] 완료/미완료 상태에 따라 Todo 항목을 필터링합니다.
        //       LINQ (Language Integrated Query)를 사용하여 쿼리를 작성합니다.
        //
        // [English] Filters todo items by completion status.
        //           Uses LINQ (Language Integrated Query) to write query.
        public async Task<IEnumerable<TodoItem>> GetTodosByStatusAsync(bool isCompleted)
        {
            // ==========================================
            // LINQ Query with Where Clause
            // ==========================================
            //
            // [한글] LINQ는 C#의 강력한 쿼리 언어입니다.
            //       Where()는 SQL의 WHERE 절과 같습니다.
            //       람다 식 (t => t.IsCompleted == isCompleted)으로 조건을 지정합니다.
            //
            // [English] LINQ is C#'s powerful query language.
            //           Where() is like SQL WHERE clause.
            //           Lambda expression (t => t.IsCompleted == isCompleted) specifies condition.
            //
            // LINQ vs SQL 비교:
            // LINQ: _context.Todos.Where(t => t.IsCompleted == isCompleted)
            // SQL:  SELECT * FROM TodoItems WHERE IsCompleted = @isCompleted
            //
            // Java Spring JPA 비교:
            // return todoRepository.findByIsCompleted(isCompleted);
            return await _context.Todos
                .Where(t => t.IsCompleted == isCompleted)
                .ToListAsync();
        }

        // ==========================================
        // Additional Business Logic Examples (Commented)
        // ==========================================
        //
        // [한글] 필요에 따라 추가 비즈니스 로직을 구현할 수 있습니다:
        // [English] Can implement additional business logic as needed:
        //
        // 예시 1: 완료 상태 토글
        // Example 1: Toggle completion status
        /*
        public async Task<bool> ToggleTodoCompletionAsync(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) return false;

            todo.IsCompleted = !todo.IsCompleted;
            await _context.SaveChangesAsync();
            return true;
        }
        */

        // 예시 2: 특정 날짜 범위의 Todo 조회
        // Example 2: Get todos within date range
        /*
        public async Task<IEnumerable<TodoItem>> GetTodosByDateRangeAsync(
            DateTime startDate, DateTime endDate)
        {
            return await _context.Todos
                .Where(t => t.CreatedAt >= startDate && t.CreatedAt <= endDate)
                .ToListAsync();
        }
        */

        // 예시 3: 제목으로 검색
        // Example 3: Search by title
        /*
        public async Task<IEnumerable<TodoItem>> SearchTodosByTitleAsync(string searchTerm)
        {
            return await _context.Todos
                .Where(t => t.Title.Contains(searchTerm))
                .ToListAsync();
        }
        */
    }

    // ==========================================
    // Summary: Service Layer Pattern
    // ==========================================
    //
    // [한글] 서비스 레이어 패턴의 장점:
    // 1. 비즈니스 로직을 한 곳에 집중 (Single Responsibility)
    // 2. Controller는 HTTP 처리만 담당 (Separation of Concerns)
    // 3. 테스트 용이성: Service를 독립적으로 테스트 가능
    // 4. 재사용성: 여러 Controller에서 같은 Service 사용 가능
    //
    // [English] Advantages of Service Layer Pattern:
    // 1. Centralize business logic (Single Responsibility)
    // 2. Controller only handles HTTP (Separation of Concerns)
    // 3. Testability: Service can be tested independently
    // 4. Reusability: Same Service can be used by multiple Controllers
    //
    // Architecture Flow (아키텍처 흐름):
    // Client Request → Controller → Service → DbContext → Database
    //                      ↓          ↓          ↓
    //                  HTTP        Business    Data
    //                  Layer       Logic       Access
    //
    // Java Spring 비교:
    // C# ASP.NET Core              →  Java Spring
    // -----------------------------------------------
    // Controller                   →  @RestController
    // Service (ITodoService)       →  @Service interface
    // DbContext                    →  JpaRepository
    // Entity Framework Core        →  Hibernate / JPA
    // Dependency Injection         →  @Autowired
}
