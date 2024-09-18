using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100f;
    [SerializeField] TMP_Text health_text;


    private void Update()
    {
        health_text.text = health.ToString();
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Damage(int amount)
    {
        health -= amount;
    }
}
