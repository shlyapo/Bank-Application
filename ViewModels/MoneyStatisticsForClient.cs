using  BankApp.Models.Entity;

namespace BankApp.ViewModels
{
    public class MoneyStatisticsForClient
    {
        public static Stack<MoneyStatistic> IndividualStatistic { get; set; } = new();
        public static List<MoneyStatistic> IndividualInformation { get; set; } = new();
        public static bool Status { get; set; } = true;
    }
}
