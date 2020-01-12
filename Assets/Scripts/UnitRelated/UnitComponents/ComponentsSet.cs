using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Представляет множество компонентов в пределах одного GameObject, 
/// над которыми можно совершать безопасные пакетные операции, 
/// такие как отключение/включение.
/// Применимо, например, для реализации выключения функционала при смерти персонажа.
/// </summary>
public class ComponentsSet
{
    public ComponentsSet(Component host, params string[] componentsNames)
    {
        Host = host;
        Components = new List<Component>();
        foreach (string compName in componentsNames)
        {
            Components.Add(Host.GetComponent(compName));
        }
    }

    public List<Component> Components { get; protected set; }

    public Component Host { get; protected set; }

    public void SetActive(bool newActive)
    {
        foreach(Behaviour comp in Components)
        {
            if (comp == null)
                continue;
            comp.enabled = newActive;
        }
    }
}
