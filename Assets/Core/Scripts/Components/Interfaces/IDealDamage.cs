public interface IDealDamage
{
  public void InitHealth(int healthInit);
  public void DealDamage(int damage);
  public void UpdateHealthReality(); // Use when need update renderer at this time (heath reality)
  public void UpdateHealthAnim(); // Use when need update renderer animation (health following time of animation)
  public bool IsDead();
}