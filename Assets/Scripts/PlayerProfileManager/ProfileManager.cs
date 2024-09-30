using TMPro;
using UnityEngine;

public class ProfileManager : MonoBehaviour
{
    [SerializeField] private RandomNameGenerator randomNameGenerator;
    [SerializeField] private RandomProfilePictureSelector randomProfilePictureSelector;

    // UI text element to display the generated random name
    [SerializeField] private TMP_Text nameDisplay;

    // Generates a random profile (name + picture)
    public void GenerateRandomProfile()
    {
        // Get and display a random name
        string randomName = randomNameGenerator.GetRandomName();
        nameDisplay.text = randomName;

        // Set a random profile picture
        randomProfilePictureSelector.SetRandomProfilePicture();
    }
}
