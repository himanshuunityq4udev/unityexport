using UnityEngine;
using UnityEngine.UI;

public class UIHealthDisplay : MonoBehaviour
{
    [SerializeField] private GameEvent onPlayerDamagedEvent; // Listening to the ScriptableObject event
    [SerializeField] private Text healthText;

    private void OnEnable()
    {
        onPlayerDamagedEvent.AddListener(UpdateHealthUI);

        // Subscribe to the EventBus
        EventBus.Subscribe<PlayerDamagedEvent>(OnPlayerDamaged);
    }

    private void OnDisable()
    {
        onPlayerDamagedEvent.RemoveListener(UpdateHealthUI);

        // Unsubscribe from the EventBus
        EventBus.Unsubscribe<PlayerDamagedEvent>(OnPlayerDamaged);
    }

    private void UpdateHealthUI()
    {
        // Example: Update UI when player is damaged
        Debug.Log("Player Damaged (via ScriptableObject Event)");
    }

    private void OnPlayerDamaged(PlayerDamagedEvent eventData)
    {
        // Update the UI through EventBus event
        healthText.text = "Health: " + eventData.CurrentHealth;
        Debug.Log("Player Damaged (via EventBus), Health: " + eventData.CurrentHealth);
    }
}
