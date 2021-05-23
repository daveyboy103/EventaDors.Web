namespace EventaDors.Entities.Classes
{
    public abstract class ProcessStatus
    {
        public ProcessingResult ProcessingResult { get; set; }
        public string ProcessingMessage { get; set; }
    }
}