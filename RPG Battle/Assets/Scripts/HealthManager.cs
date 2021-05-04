using System;

public class HealthManager
{
    public event EventHandler OnHealthChanged;

    private int healthMax;
    private int health;

    public HealthManager(int healthMax)
    {
        this.healthMax = healthMax;
        health = healthMax;
    }

    public int GetHealth()
    {
        return health;
    }

    public float GetHealthPercent()
    {
        return (float) health / healthMax;
    }

    public void TakeDamate(int damage)
    {
        health -= damage;
        
        if (health < 0) {
            health = 0;
        }

        if (OnHealthChanged != null) {
            OnHealthChanged(this, EventArgs.Empty);
        }
    }

    public void Heal(int healAmount)
    {
        health += healAmount;

        if (health > healthMax) {
            health = healthMax;
        }

        if (OnHealthChanged != null) {
            OnHealthChanged(this, EventArgs.Empty);
        }
    }

    public bool IsDead()
    {
        return health <= 0;
    }
}
