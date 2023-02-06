namespace API.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public double AnnualInterestRate { get; set; }
        public bool Is2MonthsInterestFree { get; set; }
    }
}