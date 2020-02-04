using SQLite;

namespace SimpleBudgetManager
{
    [Table("Accounts")]
    public class Account
    {
        [PrimaryKey, AutoIncrement, Column("_id")]
        public int Id { set; get; }
        public string Name { set; get; }
        public double Amount { set; get; }
        public bool IsWallet { set; get; }
        public bool IsActive { set; get; }

        public Account() { }
        public Account(string name, double amount, bool isWallet)
        {
            Name = name;
            Amount = amount;
            IsWallet = isWallet;
            IsActive = true;
        }

        /*public void SetAccount(int id, string name, double amount, bool isWallet, bool isActive)
        {
            Id = id;
            Name = name;
            Amount = amount;
            IsWallet = isWallet;
            IsActive = isActive;
        }*/

        public override string ToString()
        {
            return Name + ": " + string.Format("{0:n}", Amount) + " ₪";
        }
    }
}