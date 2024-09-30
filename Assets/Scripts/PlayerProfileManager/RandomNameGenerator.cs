using System.Collections.Generic;
using UnityEngine;

public class RandomNameGenerator : MonoBehaviour
{
    // List of random names
    [SerializeField] private List<string> names;

    // Returns a random name from the list
    public string GetRandomName()
    {
        if (names == null || names.Count == 0)
        {
            Debug.LogError("Name list is empty!");
            return "DefaultName";
        }

        int randomIndex = Random.Range(0, names.Count);
        return names[randomIndex];
    }
}
