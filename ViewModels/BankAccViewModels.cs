using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace BankApp.ViewModels
{
    public class BankAccViewModels
    {
        //public IEnumerable<User> Users { get; set; }
        public SelectList Companies { get; set; }
        public string Name { get; set; }
    }
}
 
