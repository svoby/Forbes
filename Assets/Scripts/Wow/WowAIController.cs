using UnityEngine;

[RequireComponent(typeof(MonsterController))]
public class WowAIController : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// AI first random command timeout
    /// </summary>
    public float WakeupCommandTimeout = 3f;

    /// <summary>
    /// AI next random command timeout
    /// </summary>
    [Range(1, 4)]
    public int NextCommandTimeout = 2;

    /// <summary>
    /// Max distance from spawn point
    /// </summary>
    public float MaxSpawnDistance = 10f;

    /// <summary>
    /// Ranged attack distance from target
    /// </summary>
    public float RangedDistance = 10f;

    /// <summary>
    /// Run to target distance from target
    /// </summary>
    public float ClosingDistance = 5f;

    /// <summary>
    /// Melee attack distance from target
    /// </summary>
    public float MeleeDistance = 1f;

    #endregion

    InputFrame m_InputFrame;
    Vector3 m_InitPosition;

    private RaycastHit _hitInfo;

    // Monster controller
    MonsterController m_MC;
    public MonsterController MC
    {
        get
        {
            if (m_MC == null)
                m_MC = gameObject.GetComponent<MonsterController>();
            return m_MC;
        }
    }

    GameObject m_EnemyTarget;

    void Start()
    {
        m_InitPosition = transform.position;
        GameManager.Instance.Timer.Add(AINextRandomCommand, WakeupCommandTimeout);
        MC.SetWeapon(0);
    }

    void FixedUpdate()
    {
        if (m_InputFrame == null)
            return;

        // Check for player
        this.CheckForTarget();

        // Check distance from spawn point
        this.CheckPerimeter();

        // Force Body rotate to aim rotate, if AI not moving
        InputMask inputMask = new InputMask();
        inputMask = !this.IsMoving() ? InputMask.SilenceRotate : InputMask.None;

        // Apply AI inputs
        MC.ApplyInputs(m_InputFrame, inputMask);
    }

    void CheckForTarget()
    {
        // Local player in scene
        GameObject player = GameManager.Instance.LocalPlayer.MC.gameObject;

        // Is target in range?
        if (this.IsOnRangedDistance(player.transform.position) && this.IsTargetInView(player.transform.position))
        {
            m_EnemyTarget = player;
            m_InputFrame.Fire = 1;
            this.StopMoving();
            this.LockToEnemy();
            return;
        }
        else
        {
            this.StopShooting();
        }

        // Target was set but got lost?
        if (m_EnemyTarget != null)
        {
            // AI reset
            this.StopMoving();
            this.StopShooting();

            // Clear target and get new command
            m_EnemyTarget = null;
            this.AINextRandomCommand();
        }
    }

    // Check distance from spawn point
    void CheckPerimeter()
    {
        if (IsOverSpawnDistance() && m_EnemyTarget == null)
        {
            Vector3 _initPosDir = transform.position - m_InitPosition;
            m_InputFrame.Vertical = -_initPosDir.normalized.z;
            m_InputFrame.Horizontal = -_initPosDir.normalized.x;
        }
    }

    void AINextRandomCommand()
    {
        // Cancel if enemy was spoted after last timeout invoked
        if (m_EnemyTarget != null)
            return;

        // Set new empty input frame
        m_InputFrame = new InputFrame();

        // Set new commands
        this.SetRandomMove();
        this.SetRandomRotation();

        // Invoke next command
        GameManager.Instance.Timer.Add(AINextRandomCommand, Random.Range(1f, NextCommandTimeout));
    }

    void LockToEnemy()
    {
        Vector3 dir = transform.position - m_EnemyTarget.transform.position;
        float angle = Vector2.Angle(new Vector2(dir.x, dir.z), new Vector2(0, -1));
        angle *= dir.x > 0 ? -1 : 1;

        m_InputFrame.Rotation = angle;

        if (IsOnClosingDistance(m_EnemyTarget.transform.position))
        {
            m_InputFrame.Horizontal = -dir.normalized.x;
            m_InputFrame.Vertical = -dir.normalized.z;
            this.StopShooting();
        }

        if (IsOnMeleeDistance(m_EnemyTarget.transform.position))
        {
            this.StopMoving();
            m_InputFrame.Fire = 2;
        }
    }

    void SetRandomMove()
    {
        var rand = Random.Range(0, 1f);

        // Move vertical
        if (rand > 0.9f)
        {
            m_InputFrame.Vertical = 1;
        }
        else if (rand < 0.9f)
        {
            m_InputFrame.Vertical = -1;
        }
        else
        {
            m_InputFrame.Vertical = 0;
        }

        // Move horizontal
        rand = Random.Range(0, 1f);

        if (rand > 0.9f)
        {
            m_InputFrame.Horizontal = 1;
        }
        else if (rand < 0.9f)
        {
            m_InputFrame.Horizontal = -1;
        }
        else
        {
            m_InputFrame.Horizontal = 0;
        }

        // Move stop chance
        rand = Random.Range(0, 1f);
        if (rand > 0.3f)
        {
            this.StopMoving();
        }
    }

    void StopMoving()
    {
        m_InputFrame.Vertical = 0;
        m_InputFrame.Horizontal = 0;
    }

    void StopShooting()
    {
        m_InputFrame.Fire = 0;
    }

    bool IsMoving()
    {
        return m_InputFrame.Horizontal + m_InputFrame.Vertical != 0;
    }

    void SetRandomRotation()
    {
        // Dont rotate when moving
        if (m_InputFrame.Horizontal != 0 && m_InputFrame.Vertical != 0)
            return;

        var rand = Random.Range(0, 1f);

        if (rand < 0.25f)
        {
            m_InputFrame.Rotation = 180f;
        }
        else if (rand > 0.25f && rand <= 0.5f)
        {
            m_InputFrame.Rotation = 135f;
        }
        else if (rand > 0.5f && rand <= 0.75f)
        {
            m_InputFrame.Rotation = 45f;
        }
        else if (rand > 0.75f && rand <= 1f)
        {
            m_InputFrame.Rotation = 0.1f;
        }
    }

    // Distance checkers
    bool IsOverSpawnDistance()
    {
        return (Vector3.Distance(transform.position, m_InitPosition) > MaxSpawnDistance);
    }

    bool IsOnClosingDistance(Vector3 _tarPos)
    {
        return (Vector3.Distance(transform.position, _tarPos) <= ClosingDistance);
    }

    bool IsOnRangedDistance(Vector3 _tarPos)
    {
        return (Vector3.Distance(transform.position, _tarPos) <= RangedDistance);
    }

    bool IsOnMeleeDistance(Vector3 _tarPos)
    {
        return (Vector3.Distance(transform.position, _tarPos) <= MeleeDistance);
    }

    bool IsTargetInView(Vector3 _tar)
    {
        return !Physics.Raycast(transform.position, _tar - transform.position, out _hitInfo, RangedDistance, LayerMask.GetMask("Floor"));
    }

    // Show ranges as gizmos in editor
    private void OnDrawGizmos()
    {
        if (m_EnemyTarget == null)
            return;

        if (IsTargetInView(m_EnemyTarget.transform.position))
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.localPosition, m_EnemyTarget.transform.localPosition);
        }

        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, RangedDistance);

        Gizmos.color = Color.green;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, ClosingDistance);

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, MeleeDistance);
    }
}