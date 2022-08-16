using System.ComponentModel.DataAnnotations.Schema;
using BankApp.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace BankApp.Models.Entity
{
    public class InstallmentPlan
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public virtual Bank Bank { get; set; } = null!;
        public int BankId { get; set; }
        public int AccId { get; set; }
       // public string Name { get; set; }
        public virtual Client Client { get; set; } = null!;
        public bool? Status { get; set; }//approved
                                         //public double Money { get; set; }//в десятичных
        //public double Money { get; set; }
        public double NowMoney { get; set; }
        public double MinusMoney { get; set; }
        public int Count { get; set; }
        //public int Count { get; set; }//сколько раз заплатить
        public DateTime Time { get; set; }
        public DateTime CloseTime { get; set; }
        //public DateTime OpenTime
        public virtual BankAccount BankAccount { get; set; } = null!;
        public DateTime OpenTime { get; set; }//дата создания
        public string Name { get; internal set; }
        //public int AllMoney { get; set; }//СКОЛЬКО ОПЛАТИТИЛИ
        //        //public BankAccount? BankAccount { get; set; }

    }
}
