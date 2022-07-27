namespace DomainModule.Model
{
    public class Task
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int Progress { get; set; }
        public int Priority { get; set; }
        public int ProcessId { get; set; }
        public int? UserId { get; set; }

        public virtual Process Process { get; set; }
        public virtual User User { get; set; }
    }
}