using DungeonPrisonLib.Actors.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DungeonPrisonLib
{
    public enum InterfaceRequest
    { 
        InventoryItem
    }

    public delegate void OnInventoryItemRequestEnded(Item item);

    public abstract class UserInterface
    {
        OnInventoryItemRequestEnded inventoryItemRequestCallback;

        public virtual void InventoryRequestItem(OnInventoryItemRequestEnded callback)
        {
            Debug.Assert(inventoryItemRequestCallback != null, "callback is already defined");

            inventoryItemRequestCallback = callback;
        }

        public virtual void EndInventoryItemRequest(Item item)
        {
            if (inventoryItemRequestCallback != null)
            {
                inventoryItemRequestCallback(item);
                inventoryItemRequestCallback = null;
            }
            else
                Debug.Fail("callback is NULL");
        }
    }
}
