namespace Forbes.Inventory
{
    [System.Serializable]
    public class ContainerItem
    {
        public System.Guid ID;
        public string Name;
        public int Amount;
        public int Maximum;

        public ContainerItem(string name, int amount, int maximum)
        {
            ID = System.Guid.NewGuid();
            Name = name;
            Maximum = maximum;
            Add(amount);
        }

        public System.Guid Add(int amount)
        {
            int dMax = (Amount + amount >= Maximum) ? Amount + amount - Maximum : 0;
            Amount += amount - dMax;
            return ID;
        }

        public int Get(int amount)
        {
            int dMin = (Amount - amount <= 0) ? amount - Amount : 0;
            amount -= dMin;
            Amount -= amount;
            return amount;
        }
    }
}