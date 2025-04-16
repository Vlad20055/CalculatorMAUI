using SQLite;

namespace CalculatorMAUI.Entities
{
    [Table("tasks")]
    public class Task
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]  
        public int TaskId { get; set; }
        public string Description { get; set; } = String.Empty;
        public int Duration { get; set; }
        [Indexed]
        public int ProjectId {  get; set; }

    }
}
