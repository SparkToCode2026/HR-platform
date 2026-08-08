namespace ProjectX.DTOs
{
    public class InterviewDto
    {
        public int ApplicationId { get; set; }
        public DateTime ScheduledDate { get; set; }
        public string InterviewType { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}