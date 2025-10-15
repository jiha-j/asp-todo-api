using System.ComponentModel.DataAnnotations;

// ==========================================
// TodoItem Model Class
// ==========================================
//
// [한글 설명]
// 이 클래스는 데이터 모델(Entity)입니다.
// Java Spring의 @Entity와 유사한 역할을 합니다.
// 데이터베이스 테이블의 구조를 정의하고, 각 컬럼에 대한 유효성 검사 규칙을 설정합니다.
//
// [English Explanation]
// This is the data model (Entity) class.
// Similar to @Entity in Java Spring.
// Defines the database table structure and validation rules for each column.
//
namespace TodoApi.Models
{
    /// <summary>
    /// To-Do 항목을 나타내는 엔티티 클래스
    /// Entity class representing a To-Do item
    ///
    /// [한글] 이 클래스는 데이터베이스의 TodoItems 테이블과 매핑됩니다.
    /// [English] This class maps to the TodoItems table in the database.
    /// </summary>
    public class TodoItem
    {
        // ==========================================
        // Primary Key (기본 키)
        // ==========================================
        // [한글] Id는 각 TodoItem을 식별하는 고유 번호입니다.
        //       Entity Framework는 'Id' 또는 'ClassName + Id' 패턴을 자동으로 기본 키로 인식합니다.
        //       Java Spring의 @Id와 @GeneratedValue(strategy = GenerationType.IDENTITY)와 동일합니다.
        //
        // [English] Id is the unique identifier for each TodoItem.
        //           Entity Framework automatically recognizes 'Id' or 'ClassName + Id' as primary key.
        //           Equivalent to @Id and @GeneratedValue(strategy = GenerationType.IDENTITY) in Java Spring.
        public int Id { get; set; }

        // ==========================================
        // Title Property with Validation
        // ==========================================
        // [한글] Required 속성은 필수 입력 필드를 지정합니다.
        //       Java Spring의 @NotNull 또는 @NotBlank와 동일합니다.
        //       StringLength는 최대 길이를 제한합니다 (@Size(max=200)과 유사).
        //
        // [English] Required attribute marks this as a mandatory field.
        //           Equivalent to @NotNull or @NotBlank in Java Spring.
        //           StringLength limits maximum length (similar to @Size(max=200)).
        [Required(ErrorMessage = "Title is required / 제목은 필수입니다")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters / 제목은 1~200자여야 합니다")]
        public string Title { get; set; } = string.Empty;

        // ==========================================
        // Description Property (Optional)
        // ==========================================
        // [한글] Description은 선택 사항입니다 (Required 속성이 없음).
        //       null을 허용하기 위해 'string?'로 선언합니다 (C# 8.0+ nullable reference types).
        //
        // [English] Description is optional (no Required attribute).
        //           Declared as 'string?' to allow null values (C# 8.0+ nullable reference types).
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters / 설명은 1000자를 초과할 수 없습니다")]
        public string? Description { get; set; }

        // ==========================================
        // IsCompleted Status Flag
        // ==========================================
        // [한글] 할 일의 완료 여부를 나타내는 불리언 값입니다.
        //       기본값은 false (미완료)로 설정됩니다.
        //
        // [English] Boolean flag indicating whether the to-do is completed.
        //           Default value is false (not completed).
        public bool IsCompleted { get; set; } = false;

        // ==========================================
        // CreatedAt Timestamp
        // ==========================================
        // [한글] 할 일이 생성된 시간을 저장합니다.
        //       기본값으로 현재 UTC 시간을 사용합니다.
        //       UTC (협정 세계시)를 사용하면 시간대 문제를 방지할 수 있습니다.
        //
        // [English] Stores the creation timestamp of the to-do item.
        //           Defaults to current UTC time.
        //           Using UTC prevents timezone issues.
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // ==========================================
        // UpdatedAt Timestamp
        // ==========================================
        // [한글] 할 일이 마지막으로 수정된 시간을 저장합니다.
        //       생성 시에는 CreatedAt과 같은 값을 가집니다.
        //
        // [English] Stores the last modification timestamp of the to-do item.
        //           Initially equals CreatedAt when first created.
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // ==========================================
        // DueDate - Deadline
        // ==========================================
        // [한글] 할 일의 마감기한입니다.
        //       Nullable 타입으로 선택 사항입니다.
        //       마감기한이 없는 할 일도 있을 수 있습니다.
        //
        // [English] Deadline for the to-do item.
        //           Nullable type makes it optional.
        //           Some to-dos may not have a deadline.
        public DateTime? DueDate { get; set; }

        // ==========================================
        // Priority - Task Priority
        // ==========================================
        // [한글] 할 일의 우선순위입니다.
        //       기본값은 Normal(보통)입니다.
        //       TodoPriority Enum을 사용합니다.
        //
        // [English] Priority of the to-do item.
        //           Defaults to Normal.
        //           Uses TodoPriority enum.
        public TodoPriority Priority { get; set; } = TodoPriority.Normal;

        // ==========================================
        // Category - Task Category
        // ==========================================
        // [한글] 할 일의 카테고리입니다.
        //       예: "업무", "개인", "쇼핑", "공부" 등
        //       Nullable 타입으로 선택 사항입니다.
        //
        // [English] Category of the to-do item.
        //           Examples: "Work", "Personal", "Shopping", "Study"
        //           Nullable type makes it optional.
        [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters / 카테고리는 50자를 초과할 수 없습니다")]
        public string? Category { get; set; }

        // ==========================================
        // Tags - Task Tags (JSON Array)
        // ==========================================
        // [한글] 할 일에 붙일 수 있는 태그들입니다.
        //       JSON 형식의 문자열로 저장됩니다.
        //       예: ["중요", "긴급", "프로젝트A"]
        //       Nullable 타입으로 선택 사항입니다.
        //
        // [English] Tags that can be attached to the to-do item.
        //           Stored as JSON string.
        //           Example: ["important", "urgent", "projectA"]
        //           Nullable type makes it optional.
        [StringLength(500, ErrorMessage = "Tags cannot exceed 500 characters / 태그는 500자를 초과할 수 없습니다")]
        public string? Tags { get; set; }

        // ==========================================
        // Property Explanation (속성 설명)
        // ==========================================
        //
        // [한글] C#의 속성(Property)은 Java의 getter/setter를 간단하게 표현한 것입니다.
        //       예: public string Title { get; set; }
        //       위 코드는 Java의 다음 코드와 동일합니다:
        //       private String title;
        //       public String getTitle() { return title; }
        //       public void setTitle(String title) { this.title = title; }
        //
        // [English] C# properties are a concise way to express Java's getter/setter.
        //           Example: public string Title { get; set; }
        //           This is equivalent to the following Java code:
        //           private String title;
        //           public String getTitle() { return title; }
        //           public void setTitle(String title) { this.title = title; }

        // ==========================================
        // Data Annotations vs Java Spring Annotations
        // ==========================================
        //
        // ASP.NET Core Data Annotations  →  Java Spring Equivalent
        // -------------------------------------------------------
        // [Required]                     →  @NotNull / @NotBlank
        // [StringLength(max)]            →  @Size(max = value)
        // [Range(min, max)]              →  @Min / @Max
        // [EmailAddress]                 →  @Email
        // [RegularExpression(pattern)]   →  @Pattern(regexp = "...")
        // [Key]                          →  @Id
    }
}
