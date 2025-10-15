// ==========================================
// TodoPriority Enum
// ==========================================
//
// [한글 설명]
// 할일의 우선순위를 나타내는 열거형입니다.
// 데이터베이스에는 숫자로 저장됩니다 (0, 1, 2, 3).
//
// [English Explanation]
// Enum representing the priority of a to-do item.
// Stored as numbers in the database (0, 1, 2, 3).
//
namespace TodoApi.Models
{
    /// <summary>
    /// 할일 우선순위 열거형
    /// To-Do priority enumeration
    ///
    /// [한글] 낮음(0) → 보통(1) → 높음(2) → 긴급(3) 순서로 우선순위가 높아집니다.
    /// [English] Priority increases in order: Low(0) → Normal(1) → High(2) → Urgent(3).
    /// </summary>
    public enum TodoPriority
    {
        /// <summary>
        /// 낮음 - 급하지 않은 작업
        /// Low - Non-urgent tasks
        /// </summary>
        Low = 0,

        /// <summary>
        /// 보통 - 일반적인 작업 (기본값)
        /// Normal - Regular tasks (default)
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 높음 - 중요한 작업
        /// High - Important tasks
        /// </summary>
        High = 2,

        /// <summary>
        /// 긴급 - 즉시 처리 필요
        /// Urgent - Requires immediate attention
        /// </summary>
        Urgent = 3
    }

    // ==========================================
    // Enum Explanation (열거형 설명)
    // ==========================================
    //
    // [한글] C#의 Enum은 Java의 Enum과 유사하지만 몇 가지 차이가 있습니다:
    //       1. C# Enum은 기본적으로 정수 값을 가집니다 (Java는 객체)
    //       2. 데이터베이스에 저장 시 자동으로 숫자로 변환됩니다
    //       3. Entity Framework가 자동으로 매핑 처리
    //
    // [English] C# Enum is similar to Java Enum but has some differences:
    //           1. C# Enum has integer values by default (Java has objects)
    //           2. Automatically converted to numbers when stored in database
    //           3. Entity Framework handles mapping automatically
    //
    // Java 비교 (Java Comparison):
    // public enum TodoPriority {
    //     LOW(0), NORMAL(1), HIGH(2), URGENT(3);
    //     private final int value;
    //     TodoPriority(int value) { this.value = value; }
    // }
}
