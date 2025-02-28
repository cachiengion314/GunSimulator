using Unity.Mathematics;

public interface IGunFire
{
  public void SetInitAmmo(int ammo);
  public void SetCurrentAmmo(int ammo);
  public int GetCurrentAmmo();
  public float3 GetMuzzlePosition();
  public float3 GetBulletBurstUpPosition();
}