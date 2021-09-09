namespace CaloriesSmartCalulator.Dtos
{
    public class CalculationTaskResult
    {
        public Status Status { get; set; }
        public string[] Products { get; set; }
        public int Total { get; set; }
    }
}
