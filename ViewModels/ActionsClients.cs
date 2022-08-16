using BankApp.Models.Entity;

namespace BankApp.ViewModels
{
    public class ActionsClients
    {
        public static List<string> Operations { get; set; } = new();
        public static List<string> NamesUsers { get; set; } = new();
        public static List<Client> ClientId { get; set; } = new();
        public static Dictionary<string, string> Names { get; set; } = new();
    }
}
