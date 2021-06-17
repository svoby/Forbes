using System.Collections.Generic;

namespace Forbes.Inventory
{
    public interface IContainer
    {
        int Get(System.Guid id, int amount);
        System.Guid Add(string name, int amount, int maximum);

        List<ContainerItem> Items { get; set; }

    }
}