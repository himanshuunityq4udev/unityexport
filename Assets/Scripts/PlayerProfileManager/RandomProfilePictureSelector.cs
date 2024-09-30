using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomProfilePictureSelector : MonoBehaviour
{
    // List of available profile pictures (sprites)
    [SerializeField] private List<Sprite> profilePictures;

    // UI Image element where the random profile picture will be displayed
    [SerializeField] private Image profilePictureDisplay;

    // Returns a random profile picture and assigns it to the UI Image
    public void SetRandomProfilePicture()
    {
        if (profilePictures == null || profilePictures.Count == 0)
        {
            Debug.LogError("Profile picture list is empty!");
            return;
        }

        int randomIndex = Random.Range(0, profilePictures.Count);
        Sprite randomProfilePicture = profilePictures[randomIndex];

        // Assign the random sprite to the UI Image
        profilePictureDisplay.sprite = randomProfilePicture;
    }
}
