using UnityEngine;

[RequireComponent(typeof(MonsterController))]
public class PacManAIController : MonoBehaviour
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
    /// Ranged attack distance from target
    /// </summary>
    public float RangedDistance = 10f;

    /// <summary>
    /// Melee attack distance from target
    /// </summary>
    public float MeleeDistance = 1f;

    GameObject m_EnemyTarget;
    float m_TargetLockedTime;

    #endregion

    InputFrame m_InputFrame;
    Vector3 m_InitPosition;

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

    void Start()
    {
        m_InitPosition = transform.position;
        GameManager.Instance.Timer.Add(AINextRandomCommand, WakeupCommandTimeout);
    }

    void FixedUpdate()
    {
        if (m_InputFrame == null)
            return;

        // Check for player
        this.CheckForTarget();

        // Force Body rotate to aim rotate, if AI not moving
        InputMask inputMask = new InputMask();
        inputMask = InputMask.SilenceRotate;

        // Apply AI inputs
        MC.ApplyInputs(m_InputFrame, inputMask);
    }

    void CheckForTarget()
    {
        // Local player in scene
        GameObject player = GameManager.Instance.LocalPlayer.MC.gameObject;
        if (player == null)
            return;

        // IsAlive?
        Destructable _destructable = player.GetComponent<Destructable>();
        if (_destructable != null)
            if (!_destructable.IsAlive)
                return;

        // Is target in range?
        if (IsOnRangedDistance(player.transform.position))
        {
            m_EnemyTarget = player;

            if (m_TargetLockedTime > 1f)
            {
                Vector3 _dir = this.LockToEnemyAngleY();
                m_InputFrame.Horizontal = -_dir.normalized.x;
                m_InputFrame.Vertical = -_dir.normalized.z;
            }
            else
            {
                this.LockToEnemyAngleY();
                m_TargetLockedTime += Time.fixedDeltaTime;
            }

            if (IsOnMeleeDistance(m_EnemyTarget.transform.position))
            {
                m_InputFrame.Fire = 2;
            }

            return;
        }

        // Target was set but got lost?
        if (m_EnemyTarget != null)
        {
            // Clear target and get new command
            m_EnemyTarget = null;
            m_TargetLockedTime = 0;
            this.StopMoving(2);
        }
    }

    void StopMoving(int _delay)
    {
        m_InputFrame.Horizontal = 0;
        m_InputFrame.Vertical = 0;

        GameManager.Instance.Timer.Add(AINextRandomCommand, _delay);
    }

    Vector3 LockToEnemyAngleY()
    {
        Vector3 dir = transform.position - m_EnemyTarget.transform.position;
        float angle = Vector2.Angle(new Vector2(dir.x, dir.z), new Vector2(0, -1));
        angle *= dir.x > 0 ? -1 : 1;

        m_InputFrame.Horizontal = 0;
        m_InputFrame.Vertical = 0;
        m_InputFrame.Rotation = angle;

        return dir;
    }

    void AINextRandomCommand()
    {
        if (m_EnemyTarget != null)
            return;

        // Set new empty input frame
        m_InputFrame = new InputFrame();

        // Set new commands
        this.SetRandomMove();

        // Invoke next command
        GameManager.Instance.Timer.Add(AINextRandomCommand, Random.Range(1f, NextCommandTimeout));
    }

    void SetRandomMove()
    {
        var rand = Random.Range(0, 1f);

        if (rand < 0.25f)
        {
            m_InputFrame.Horizontal = 1f;
            m_InputFrame.Vertical = 0;
        }
        else if (rand > 0.25f && rand <= 0.5f)
        {
            m_InputFrame.Horizontal = -1f;
            m_InputFrame.Vertical = 0;
        }
        else if (rand > 0.5f && rand <= 0.75f)
        {
            m_InputFrame.Horizontal = 0;
            m_InputFrame.Vertical = 1f;
        }
        else if (rand > 0.75f && rand <= 1f)
        {
            m_InputFrame.Horizontal = 0;
            m_InputFrame.Vertical = -1f;
        }
    }

    bool IsOnRangedDistance(Vector3 _tarPos)
    {
        return (Vector3.Distance(transform.position, _tarPos) <= RangedDistance);
    }

    bool IsOnMeleeDistance(Vector3 _tarPos)
    {
        return (Vector3.Distance(transform.position, _tarPos) <= MeleeDistance);
    }

    // Show ranges as gizmos in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, RangedDistance);

        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireSphere(Vector3.zero, MeleeDistance);
    }
}