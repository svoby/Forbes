using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Container : IContainer
{
    List<ContainerItem> m_items;
    public List<ContainerItem> Items
    {
        get
        {
            if (m_items == null)
                m_items = new List<ContainerItem>();
            return m_items;
        }
        set
        {
            m_items = value;
        }
    }

    public int Get(String name, int amount)
    {
        ContainerItem item = Items.Where(x => x.Name == name).FirstOrDefault();
        if (item == null)
            return -1;

        return item.Get(amount);
    }

    public int Get(System.Guid id, int amount)
    {
        ContainerItem item = Items.Where(x => x.ID == id).FirstOrDefault();
        if (item == null)
            return -1;

        return item.Get(amount);
    }

    public System.Guid Add(string name, int amount, int maximum)
    {
        ContainerItem item = Items.Where(x => x.Name == name).FirstOrDefault();
        if (item == null) {
            Items.Add(new ContainerItem(name, amount, maximum));
            return Items.Last().ID;
        }

        return item.Add(amount);
    }

    public ContainerItem Contains(string name)
    {
        return Items.Where(x => x.Name == name).FirstOrDefault();
    }

    internal void AddOnce(string name)
    {
        if (this.Contains(name) == null) this.Add(name, 1, 1);
    }
}