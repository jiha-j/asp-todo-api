using TodoApi.Models;

// ==========================================
// ITodoService Interface
// ==========================================
//
// [한글 설명]
// 이것은 서비스 레이어의 인터페이스입니다.
// Java Spring의 @Service 인터페이스와 동일한 개념입니다.
// 비즈니스 로직의 계약(Contract)을 정의하고, 구현체와 분리합니다.
//
// [English Explanation]
// This is the service layer interface.
// Same concept as @Service interface in Java Spring.
// Defines business logic contract and separates from implementation.
//
// [한글] 인터페이스를 사용하는 이유:
// 1. 의존성 역전 원칙(DIP): 구현체가 아닌 추상화에 의존
// 2. 테스트 용이성: Mock 객체로 쉽게 대체 가능
// 3. 유연성: 다양한 구현체를 교체 가능 (예: InMemory → SQL Server)
//
// [English] Why use interfaces:
// 1. Dependency Inversion Principle: Depend on abstraction, not implementation
// 2. Testability: Easy to replace with mock objects
// 3. Flexibility: Can swap implementations (e.g., InMemory → SQL Server)
//
namespace TodoApi.Services
{
    /// <summary>
    /// Todo 서비스 인터페이스 - 비즈니스 로직 계약 정의
    /// Todo service interface - Defines business logic contract
    ///
    /// [한글] 이 인터페이스는 Todo 관련 모든 비즈니스 로직의 메서드 시그니처를 정의합니다.
    /// [English] This interface defines method signatures for all Todo-related business logic.
    /// </summary>
    public interface ITodoService
    {
        // ==========================================
        // GetAllTodosAsync - 모든 Todo 항목 조회
        // ==========================================
        //
        // [한글] 비동기(Async) 메서드입니다.
        //       Task<T>는 Java의 CompletableFuture<T>와 유사합니다.
        //       IEnumerable<TodoItem>은 Java의 List<TodoItem>과 유사합니다.
        //
        // [English] This is an asynchronous method.
        //           Task<T> is similar to CompletableFuture<T> in Java.
        //           IEnumerable<TodoItem> is similar to List<TodoItem> in Java.
        //
        // 반환 타입 설명:
        // Task<IEnumerable<TodoItem>>
        // └─ Task: 비동기 작업을 나타냄 (Java: Future/CompletableFuture)
        //    └─ IEnumerable<TodoItem>: Todo 항목의 컬렉션 (Java: List<TodoItem>)
        /// <summary>
        /// 모든 Todo 항목을 조회합니다
        /// Retrieves all todo items
        /// </summary>
        /// <returns>Todo 항목 컬렉션 / Collection of todo items</returns>
        Task<IEnumerable<TodoItem>> GetAllTodosAsync();

        // ==========================================
        // GetTodoByIdAsync - ID로 특정 Todo 항목 조회
        // ==========================================
        //
        // [한글] nullable 반환 타입 (TodoItem?)을 사용합니다.
        //       ?는 해당 항목이 존재하지 않을 수 있음을 나타냅니다.
        //       Java의 Optional<TodoItem>과 유사한 개념입니다.
        //
        // [English] Uses nullable return type (TodoItem?).
        //           ? indicates the item may not exist.
        //           Similar concept to Optional<TodoItem> in Java.
        /// <summary>
        /// ID로 특정 Todo 항목을 조회합니다
        /// Retrieves a specific todo item by ID
        /// </summary>
        /// <param name="id">조회할 Todo의 ID / ID of the todo to retrieve</param>
        /// <returns>Todo 항목 또는 null / Todo item or null</returns>
        Task<TodoItem?> GetTodoByIdAsync(int id);

        // ==========================================
        // CreateTodoAsync - 새 Todo 항목 생성
        // ==========================================
        //
        // [한글] 새로운 Todo 항목을 데이터베이스에 추가합니다.
        //       생성된 항목(Id가 할당된)을 반환합니다.
        //
        // [English] Adds a new todo item to the database.
        //           Returns the created item (with Id assigned).
        /// <summary>
        /// 새 Todo 항목을 생성합니다
        /// Creates a new todo item
        /// </summary>
        /// <param name="todoItem">생성할 Todo 항목 / Todo item to create</param>
        /// <returns>생성된 Todo 항목 (ID 포함) / Created todo item (with ID)</returns>
        Task<TodoItem> CreateTodoAsync(TodoItem todoItem);

        // ==========================================
        // UpdateTodoAsync - 기존 Todo 항목 수정
        // ==========================================
        //
        // [한글] 기존 Todo 항목을 수정합니다.
        //       수정 성공 시 true, 실패 시 false를 반환합니다.
        //       (예: 항목이 존재하지 않는 경우 false)
        //
        // [English] Updates an existing todo item.
        //           Returns true if successful, false if failed.
        //           (e.g., false if item doesn't exist)
        /// <summary>
        /// 기존 Todo 항목을 수정합니다
        /// Updates an existing todo item
        /// </summary>
        /// <param name="id">수정할 Todo의 ID / ID of the todo to update</param>
        /// <param name="todoItem">수정할 데이터 / Data to update</param>
        /// <returns>성공 여부 / Success status</returns>
        Task<bool> UpdateTodoAsync(int id, TodoItem todoItem);

        // ==========================================
        // DeleteTodoAsync - Todo 항목 삭제
        // ==========================================
        //
        // [한글] 지정된 ID의 Todo 항목을 삭제합니다.
        //       삭제 성공 시 true, 실패 시 false를 반환합니다.
        //
        // [English] Deletes the todo item with specified ID.
        //           Returns true if successful, false if failed.
        /// <summary>
        /// Todo 항목을 삭제합니다
        /// Deletes a todo item
        /// </summary>
        /// <param name="id">삭제할 Todo의 ID / ID of the todo to delete</param>
        /// <returns>성공 여부 / Success status</returns>
        Task<bool> DeleteTodoAsync(int id);

        // ==========================================
        // GetTodosByStatusAsync - 완료 상태로 필터링
        // ==========================================
        //
        // [한글] 완료/미완료 상태에 따라 Todo 항목을 필터링합니다.
        //       추가적인 비즈니스 로직의 예시입니다.
        //
        // [English] Filters todo items by completion status.
        //           Example of additional business logic.
        /// <summary>
        /// 완료 상태로 Todo 항목을 필터링합니다
        /// Filters todo items by completion status
        /// </summary>
        /// <param name="isCompleted">완료 여부 / Completion status</param>
        /// <returns>필터링된 Todo 항목 컬렉션 / Filtered collection of todo items</returns>
        Task<IEnumerable<TodoItem>> GetTodosByStatusAsync(bool isCompleted);
    }

    // ==========================================
    // Interface vs Abstract Class Comparison
    // ==========================================
    //
    // C# Interface                    →  Java Interface
    // -------------------------------------------------
    // public interface ITodoService   →  public interface TodoService
    // Task<T> Method()                →  CompletableFuture<T> method()
    // No implementation               →  No implementation (before Java 8)
    //                                 →  Default methods (Java 8+)
    //
    // [한글] C#에서는 인터페이스 이름 앞에 'I'를 붙이는 것이 관례입니다.
    //       (예: ITodoService, IRepository, ILogger)
    //       Java Spring에서는 보통 Impl 접미사를 사용합니다.
    //       (예: TodoService 인터페이스 → TodoServiceImpl 구현체)
    //
    // [English] In C#, it's convention to prefix interface names with 'I'.
    //           (e.g., ITodoService, IRepository, ILogger)
    //           In Java Spring, typically uses Impl suffix.
    //           (e.g., TodoService interface → TodoServiceImpl implementation)

    // ==========================================
    // Async/Await Pattern in C#
    // ==========================================
    //
    // [한글] C#의 async/await는 비동기 프로그래밍을 위한 핵심 패턴입니다.
    //       Java의 CompletableFuture보다 더 간단하고 직관적입니다.
    //
    //       C# async/await              Java Equivalent
    //       ----------------------------------------------------------------
    //       async Task<T> Method()  →  CompletableFuture<T> method()
    //       await service.Get()     →  service.get().get() or .thenApply()
    //       Task.Run(...)           →  CompletableFuture.supplyAsync(...)
    //
    // [English] C# async/await is core pattern for asynchronous programming.
    //           More simple and intuitive than Java's CompletableFuture.
    //
    // 사용 예시 (Usage Example):
    //
    // C#:
    //   var result = await todoService.GetAllTodosAsync();
    //
    // Java:
    //   TodoItem result = todoService.getAllTodos()
    //                                 .get(); // blocks
    //   // or
    //   todoService.getAllTodos()
    //              .thenApply(items -> { ... }); // non-blocking
}
