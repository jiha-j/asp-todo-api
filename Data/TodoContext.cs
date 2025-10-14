using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// ==========================================
// TodoContext - Database Context Class
// ==========================================
//
// [한글 설명]
// 이 클래스는 Entity Framework Core의 DbContext를 상속받습니다.
// Java Spring의 JpaRepository 또는 Hibernate SessionFactory와 유사한 역할을 합니다.
// 데이터베이스와의 연결을 관리하고, CRUD 작업을 위한 인터페이스를 제공합니다.
//
// [English Explanation]
// This class inherits from Entity Framework Core's DbContext.
// Similar to JpaRepository or Hibernate SessionFactory in Java Spring.
// Manages database connections and provides interfaces for CRUD operations.
//
namespace TodoApi.Data
{
    /// <summary>
    /// 데이터베이스 컨텍스트 - Entity Framework Core의 핵심 클래스
    /// Database context - Core class of Entity Framework Core
    ///
    /// [한글] 이 클래스는 데이터베이스와 애플리케이션 간의 다리 역할을 합니다.
    /// [English] This class acts as a bridge between the database and the application.
    /// </summary>
    public class TodoContext : DbContext
    {
        // ==========================================
        // Constructor with Dependency Injection
        // ==========================================
        //
        // [한글] 생성자에서 DbContextOptions를 주입받습니다.
        //       이것이 ASP.NET Core의 의존성 주입(Dependency Injection) 패턴입니다.
        //       Java Spring의 @Autowired 또는 생성자 주입과 동일한 개념입니다.
        //
        // [English] Constructor receives DbContextOptions through dependency injection.
        //           This is ASP.NET Core's Dependency Injection pattern.
        //           Same concept as @Autowired or constructor injection in Java Spring.
        //
        // 호출 흐름 (Call Flow):
        // Program.cs에서 services.AddDbContext<TodoContext>() 설정
        // → ASP.NET Core DI Container가 TodoContext 인스턴스 생성
        // → 생성 시 필요한 DbContextOptions를 자동으로 주입
        // → TodoService에서 TodoContext를 주입받아 사용
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            // [한글] base(options)는 부모 클래스(DbContext)의 생성자를 호출합니다.
            //       Java의 super(options)와 동일합니다.
            //
            // [English] base(options) calls the parent class (DbContext) constructor.
            //           Same as super(options) in Java.
        }

        // ==========================================
        // DbSet Property - Database Table
        // ==========================================
        //
        // [한글] DbSet<TodoItem>은 데이터베이스의 TodoItems 테이블을 나타냅니다.
        //       Java Spring의 JpaRepository<TodoItem, Integer>와 유사한 역할입니다.
        //       이 속성을 통해 CRUD 작업을 수행할 수 있습니다.
        //
        // [English] DbSet<TodoItem> represents the TodoItems table in the database.
        //           Similar to JpaRepository<TodoItem, Integer> in Java Spring.
        //           Provides CRUD operations through this property.
        //
        // 사용 예시 (Usage Examples):
        // - Todos.ToListAsync()              → SELECT * FROM TodoItems
        // - Todos.FindAsync(id)              → SELECT * FROM TodoItems WHERE Id = {id}
        // - Todos.Add(item)                  → INSERT INTO TodoItems
        // - Todos.Update(item)               → UPDATE TodoItems
        // - Todos.Remove(item)               → DELETE FROM TodoItems
        public DbSet<TodoItem> Todos { get; set; }

        // ==========================================
        // OnModelCreating - Database Schema Configuration
        // ==========================================
        //
        // [한글] 이 메서드는 데이터베이스 스키마를 세밀하게 구성할 수 있습니다.
        //       Java Spring JPA의 @Table, @Column 어노테이션과 유사한 역할을 합니다.
        //       여기서는 테이블 이름, 인덱스, 제약조건 등을 설정할 수 있습니다.
        //
        // [English] This method allows fine-grained database schema configuration.
        //           Similar to @Table, @Column annotations in Java Spring JPA.
        //           Here you can configure table names, indexes, constraints, etc.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // [한글] TodoItem 엔티티 구성
            // [English] Configure TodoItem entity
            modelBuilder.Entity<TodoItem>(entity =>
            {
                // ==========================================
                // Table Name Configuration (InMemory에서는 무시됨)
                // ==========================================
                // [한글] 테이블 이름을 명시적으로 지정합니다.
                //       지정하지 않으면 클래스 이름의 복수형(TodoItems)이 기본값입니다.
                //       참고: InMemory 데이터베이스에서는 이 설정이 무시됩니다.
                //
                // [English] Explicitly specify the table name.
                //           If not specified, plural form of class name (TodoItems) is default.
                //           Note: This setting is ignored in InMemory database.
                // entity.ToTable("TodoItems"); // InMemory에서는 사용 불가

                // ==========================================
                // Primary Key Configuration
                // ==========================================
                // [한글] Id를 기본 키로 명시적으로 설정합니다.
                //       관례상 'Id' 또는 'TodoItemId'는 자동으로 인식되지만,
                //       명시적으로 설정하면 코드가 더 명확해집니다.
                //
                // [English] Explicitly set Id as primary key.
                //           By convention, 'Id' or 'TodoItemId' is auto-recognized,
                //           but explicit configuration makes code clearer.
                entity.HasKey(e => e.Id);

                // ==========================================
                // Auto-Increment Configuration
                // ==========================================
                // [한글] Id 컬럼이 자동으로 증가하도록 설정합니다.
                //       Java Spring JPA의 @GeneratedValue(strategy = GenerationType.IDENTITY)와 동일합니다.
                //       InMemory 데이터베이스에서도 작동합니다.
                //
                // [English] Configure Id to auto-increment.
                //           Same as @GeneratedValue(strategy = GenerationType.IDENTITY) in Java Spring JPA.
                //           Works with InMemory database.
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                // ==========================================
                // Title Column Configuration
                // ==========================================
                // [한글] Title 컬럼의 세부 설정입니다.
                //       IsRequired()는 NOT NULL 제약조건을 추가합니다.
                //       HasMaxLength()는 최대 길이를 설정합니다.
                //
                // [English] Detailed configuration for Title column.
                //           IsRequired() adds NOT NULL constraint.
                //           HasMaxLength() sets maximum length.
                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                // ==========================================
                // Description Column Configuration
                // ==========================================
                // [한글] Description은 선택적 필드입니다 (IsRequired 없음).
                //       NULL 값을 허용합니다.
                //
                // [English] Description is optional (no IsRequired).
                //           Allows NULL values.
                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                // ==========================================
                // IsCompleted Column Configuration
                // ==========================================
                // [한글] bool 타입은 데이터베이스에서 BIT 또는 BOOLEAN으로 저장됩니다.
                //       InMemory에서는 HasDefaultValue()를 사용할 수 없습니다.
                //       대신 TodoItem 클래스의 속성 초기화를 사용합니다.
                //
                // [English] bool type is stored as BIT or BOOLEAN in database.
                //           HasDefaultValue() cannot be used with InMemory.
                //           Use property initialization in TodoItem class instead.
                entity.Property(e => e.IsCompleted)
                    .IsRequired();
                    // .HasDefaultValue(false); // InMemory에서는 사용 불가

                // ==========================================
                // CreatedAt Column Configuration
                // ==========================================
                // [한글] DateTime 타입으로 생성 시간을 저장합니다.
                //       InMemory에서는 HasDefaultValueSql()을 사용할 수 없습니다.
                //       대신 TodoItem 클래스의 속성 초기화를 사용합니다.
                //
                // [English] Stores creation time as DateTime type.
                //           HasDefaultValueSql() cannot be used with InMemory.
                //           Use property initialization in TodoItem class instead.
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                    // .HasDefaultValueSql("CURRENT_TIMESTAMP"); // InMemory에서는 사용 불가

                // ==========================================
                // Index Configuration (InMemory에서는 무시됨)
                // ==========================================
                // [한글] 인덱스를 추가하여 검색 성능을 향상시킵니다.
                //       InMemory 데이터베이스에서는 인덱스가 무시되지만,
                //       실제 데이터베이스로 전환할 때를 위해 설정할 수 있습니다.
                //
                // [English] Add index to improve search performance.
                //           Indexes are ignored in InMemory database, but
                //           can be configured for future migration to real database.
                entity.HasIndex(e => e.IsCompleted);
                    // .HasDatabaseName("IX_TodoItems_IsCompleted"); // InMemory에서는 사용 불가

                // ==========================================
                // Seed Data (Optional Initial Data)
                // ==========================================
                // [한글] 초기 데이터를 데이터베이스에 추가할 수 있습니다.
                //       Java Spring의 data.sql 또는 @PostConstruct와 유사합니다.
                //       개발 및 테스트 환경에서 유용합니다.
                //
                // [English] Can add initial data to database.
                //           Similar to data.sql or @PostConstruct in Java Spring.
                //           Useful for development and testing environments.
                entity.HasData(
                    new TodoItem
                    {
                        Id = 1,
                        Title = "Welcome to Todo API",
                        Description = "This is a sample todo item created on startup",
                        IsCompleted = false,
                        CreatedAt = DateTime.UtcNow
                    }
                );
            });

            // [한글] 부모 클래스의 OnModelCreating 호출
            // [English] Call parent class's OnModelCreating
            base.OnModelCreating(modelBuilder);
        }

        // ==========================================
        // Additional Notes: EF Core vs Java Spring JPA
        // ==========================================
        //
        // Entity Framework Core          →  Java Spring JPA / Hibernate
        // ---------------------------------------------------------------
        // DbContext                      →  EntityManager / SessionFactory
        // DbSet<T>                       →  JpaRepository<T, ID>
        // DbContextOptions               →  @PersistenceContext configuration
        // OnModelCreating()              →  @Table, @Column annotations
        // Migrations                     →  Flyway / Liquibase
        // .ToListAsync()                 →  .findAll()
        // .FindAsync(id)                 →  .findById(id)
        // .Add(entity)                   →  .save(entity)
        // .Update(entity)                →  .save(entity) (merge)
        // .Remove(entity)                →  .delete(entity)
        // SaveChangesAsync()             →  .flush() (auto in transaction)
        //
        // [한글] Entity Framework Core는 기본적으로 변경 추적(Change Tracking)을 수행합니다.
        //       SaveChangesAsync()를 호출하면 모든 변경사항이 데이터베이스에 반영됩니다.
        //       Java Spring JPA의 영속성 컨텍스트(Persistence Context)와 동일한 개념입니다.
        //
        // [English] Entity Framework Core performs Change Tracking by default.
        //           Calling SaveChangesAsync() commits all changes to the database.
        //           Same concept as Persistence Context in Java Spring JPA.
    }
}
