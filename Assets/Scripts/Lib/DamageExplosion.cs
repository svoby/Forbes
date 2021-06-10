using UnityEngine;

public class DamageExplosion : MonoBehaviour
{
    public float AutoDestroyTime = 5f;
    public int Damage = 50;
    public float DamageRadius = 5f;
    public float PushForce = 1f;
    public int DamageMultipler = 1;

    [Header("Shake settings")]
    [SerializeField]
    public ShakeCfg shake;

    [Header("Sounds")]
    public AudioClip SoundExplosion;

    void Start()
    {
        if (SoundExplosion != null)
            AudioSource.PlayClipAtPoint(SoundExplosion, transform.position);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DamageRadius, LayerMask.GetMask("Floor", "Enemy", "Player"), QueryTriggerInteraction.UseGlobal);
        foreach (Collider collider in hitColliders)
        {
            GameObject obj = collider.gameObject;
            Vector3 closestPoint = collider.ClosestPoint(transform.position);
            Vector3 closestDirection = closestPoint - transform.position;

            float deltaMaxDamage = closestDirection.magnitude / DamageRadius;
            float deltaMaxPush = PushForce - PushForce * deltaMaxDamage;
            float finalDamage = Damage - Damage * deltaMaxDamage;

            finalDamage = (int)finalDamage;
            Vector3 finalForce = closestDirection.normalized * deltaMaxPush;
            finalForce = new Vector3(finalForce.x, finalForce.y * 0.3f, 0);

            if (finalDamage >= 1)
            {
                var destructable = collider.transform.GetComponent<Destructable>();
                if (destructable != null)
                    destructable.TakeDamage((int) finalDamage, null);

                Shaker shaker = obj.GetComponent<Shaker>();
                if (shaker)
                {
                    shaker.Set(new ShakeCfg
                    {
                        Amplitude = 0.1f,
                        Duration = 0.3f,
                        Origin = obj.transform.position,
                        Listener = obj.transform.position,
                        ShakeWith = obj.transform
                    });
                }
            }
        }

        // GameManager.Instance.CameraController.Shake(new ShakeCfg {
        // 	Amplitude = shake.Amplitude,
        // 	Duration = shake.Duration,
        // 	Origin = transform.position,
        // 	Listener = GameManager.Instance.LocalPlayer.transform.position,
        // 	ShakeWith = Camera.main.transform
        // });

        Destroy(gameObject, AutoDestroyTime);
    }
}