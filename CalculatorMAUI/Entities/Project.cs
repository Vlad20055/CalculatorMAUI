using SQLite;

namespace CalculatorMAUI.Entities
{
    [Table("projects")]
    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
