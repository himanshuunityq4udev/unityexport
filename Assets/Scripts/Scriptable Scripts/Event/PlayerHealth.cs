using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameEvent onPlayerDamagedEvent; // Using ScriptableObject event for global use

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Raise the ScriptableObject event
        onPlayerDamagedEvent.Invoke();

        // Publish to EventBus as well
        EventBus.Publish(new PlayerDamagedEvent(currentHealth));

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player Died");
        // You can use either ScriptableObject or EventBus here as per your design choice.
    }
}

public class PlayerDamagedEvent
{
    public int CurrentHealth;

    public PlayerDamagedEvent(int currentHealth)
    {
        CurrentHealth = currentHealth;
    }
}
