using BankApp.Models.Entity;
using BankApp.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BankApp.Models.Entity.SalaryModel;
using BankApp.ViewModels;
using BankApp.Models.InstallmentOrCreditModel;

namespace BankApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Bank> Banks { get; set; }
        public virtual DbSet<CreateClient> CreateClients { get; set; }
        //public virtual DbSet<MoneyTrans> MoneyTranses { get; set; }
        public virtual DbSet<BankAccount> BankAccounts { get; set; }
        public virtual DbSet<InstallmentPlan> Installments { get; set; }
        public virtual DbSet<Salary> Salarys { get; set; }
        //public virtual DbSet<Models.Actions.Action> Actions { get; set; }
        public virtual DbSet<Company> Companies { get; set; }///
        public virtual DbSet<Specialist> Specialists { get; set; }

        public virtual DbSet<Operator> Operators { get; set; }
        public virtual DbSet<Manager> Managers { get; set; }
        public virtual DbSet<Admin> Admins { get; set; }
        public virtual  DbSet<ClientSalary> ClientSalaries { get; set; }
        //public virtual DbSet<UserCreation> UserCreation { get; set; }
        public virtual DbSet<CreateInstallment> InstallmentCreation { get; set; }
        public virtual DbSet<MoneyStatistic> MoneyStatistics { get; set; }
        public virtual DbSet<SalaryProject> SalaryProjects { get; set; }
        //public virtual DbSet<MoneyRequest> MoneyRequests { get; set; }
        public virtual DbSet<SpecStatistic> SpecStatistics { get; set; }
       // public virtual DbSet<ClientStatistic> ClientStatistics { get; set; }
        //public virtual DbSet<MoneyOfCompany> MoneyOfCompanies { get; set; }
        public virtual DbSet<UserCreation> UserCreations { get; set; }

       public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.UserId).HasMaxLength(450);
                entity.HasOne(e => e.User)
                .WithMany(p => p.Clients)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Clients_Users");

                entity.HasMany(e => e.Banks)
                .WithMany(p => p.Clients)
                .UsingEntity(u => u.ToTable("ClientsBanks"));
            });

            modelBuilder.Entity<Manager>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.UserId).HasMaxLength(450);
                entity.HasOne(e => e.User)
                .WithMany(p => p.Managers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Managers_Users");

                entity.HasOne(e => e.Bank)
                .WithMany(p => p.Managers)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Managers_Banks");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.UserId).HasMaxLength(450);
                entity.HasOne(e => e.User)
                .WithMany(p => p.Admins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Admins_Users");
            });

            modelBuilder.Entity<Specialist>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.UserId).HasMaxLength(450);
                entity.HasOne(e => e.User)
                .WithMany(p => p.Specialists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Specialists_Users");

                entity.HasOne(e => e.Bank)
                .WithMany(p => p.Specialists)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Specialists_Banks");

                entity.HasOne(e => e.Company)
                .WithMany(p => p.Specialists)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Specialists_Companys");
            });

            modelBuilder.Entity<Operator>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(p => p.UserId).HasMaxLength(450);
                entity.HasOne(e => e.User)
                .WithMany(p => p.Operators)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operators_Users");

                entity.HasOne(e => e.Bank)
                .WithMany(p => p.Operators)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Operators_Banks");
            });

            modelBuilder.Entity<Bank>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(p=>p.Name).IsRequired();
            });

            modelBuilder.Entity<BankAccount>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Bank)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Banks");

                entity.HasOne(e => e.Client)
                .WithMany(p => p.Accounts)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Accounts_Clients");
                
            });

            modelBuilder.Entity<InstallmentPlan>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Bank)
                .WithMany(p => p.Installments)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Installments_Banks");

                entity.HasOne(e => e.Client)
                .WithMany(p => p.Installments)
                .HasForeignKey(d => d.ClientId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Intallments_Clients");

                entity.HasOne(e => e.BankAccount)
                .WithMany(p => p.Installments)
                .HasForeignKey(d => d.AccId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Intallments_Accounts");
            });

            modelBuilder.Entity<Credit>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<CreateInstallment>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<MoneyStatistic>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            /*modelBuilder.Entity<ClientStatistic>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });*/

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Company)
                .WithMany(p => p.Salarys)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Salarys_Companys");

            });

            modelBuilder.Entity<SalaryProject>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Bank)
                .WithMany(p => p.SalaryProjects)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SalaryProject_Banks");

            });

            modelBuilder.Entity<ClientSalary>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Company)
                .WithMany(p => p.ClientSalaries)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ClientSalarys_Companys");

            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(e => e.Bank)
                .WithMany(p => p.Companys)
                .HasForeignKey(d => d.BankId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Companys_Banks");

            });


            modelBuilder.Entity<Bank>().HasData(
               new Bank[]
               {
               new Bank { Id=1, Name="BelarusBank"},
               new Bank { Id=2, Name="BelinvestBank"},
               new Bank { Id=3, Name="Tehnobank"}
               });
            /*
            modelBuilder.Entity<User>().HasData(
               new User[]
               {
               new User { Id="1", Email="user@gmail.com", Password="123@Qa"},
               new User { Id="2", Email="client1@gmail.com", Password="123@Qa"},
               }); */
            // создание и добавление моделей


            base.OnModelCreating(modelBuilder);
        }

    }
}
            