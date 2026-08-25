using System.Collections.Generic;
using UnityEngine;

public class CatSlotRoot : MonoBehaviour
{
    public Transform[] GetSlots()
    {
        List<Transform> slots = new List<Transform>();

        foreach (Transform child in transform)
        {
            slots.Add(child);
        }

        return slots.ToArray();
    }
}
