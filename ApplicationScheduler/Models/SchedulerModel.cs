namespace ApplicationScheduler.Models
{
    public class SchedulerModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set;}
        public DateTime UpdatedDate { get; set;} = DateTime.Now;
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public string Status { get; set; }
        public string meeting_person { get; set; }
        public string place { get; set; }

    }
}
