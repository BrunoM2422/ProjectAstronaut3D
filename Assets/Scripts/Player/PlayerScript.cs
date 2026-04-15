using Animation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Move,
    Jump,
    Dead
}

public class PlayerScript : MonoBehaviour, IDamageable
{
    [Header("PlayerNeeds")]
    public StateMachine<PlayerState> stateMachine;
    public CharacterController characterController;
    public Animator animator;
    public PlayerFlashColor flashColor;
    public PlayerHealthUpdater healthUI;

    [Header("PlayerSettings")]
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 15f;
    public float health = 20;


    [Header("PlayerInnerInfo")]
    [SerializeField] private float _currentHealth;
    [SerializeField] private float invulnerabilityTime = 0.5f;
    

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Air Control")]
    public float airControl = 0.5f;

    private float lastDamageTime = -Mathf.Infinity;
    public float vSpeed = 0f;

    public void Start()
    {
        _currentHealth = health;

        stateMachine = new StateMachine<PlayerState>();
        stateMachine.Init();

        stateMachine.RegisterStates(PlayerState.Idle, new PlayerIdleState(this));
        stateMachine.RegisterStates(PlayerState.Move, new PlayerMoveState(this));
        stateMachine.RegisterStates(PlayerState.Jump, new PlayerJumpState(this));
        stateMachine.RegisterStates(PlayerState.Dead, new PlayerDeadState(this));

        stateMachine.SwitchState(PlayerState.Idle);

        if (healthUI != null)
        {
            healthUI.UpdateValue(health, _currentHealth);
        }
    }
    void Update()
    {
        if (characterController == null) return;

        stateMachine.Update();
    }

    #region Damage and Death Logic

    public void Damage(float damage)
    {
        OnDamage(damage);
    }

    public void OnDamage(float f)
    {
        if (Time.time < lastDamageTime + invulnerabilityTime)
            return;

        lastDamageTime = Time.time;

        if (flashColor != null)
        {
            flashColor.Flash();
        }
        _currentHealth -= f;

        if (healthUI != null)
        {
            healthUI.UpdateValue(health, _currentHealth);
        }

        if (_currentHealth <= 0)
        {
            stateMachine.SwitchState(PlayerState.Dead);
        }
    }

    protected virtual void Kill()
    {

        OnKill();
    }

    protected virtual void OnKill()
    {
        characterController.enabled = false;
        Destroy(gameObject, 0.7f);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            OnDamage(1);
        }
    }

    #endregion

}

public class PlayerIdleState : StateBase
{
    private PlayerScript player;

    public PlayerIdleState(PlayerScript player)
    {
        this.player = player;
    }

    public override void OnStateEnter(object o = null)
    {
        player.animator.SetBool("Run", false);
    }

    public override void OnStateStay(object o = null)
    {

        if (player.characterController.isGrounded && player.vSpeed < 0)
            player.vSpeed = -2f; 

        player.vSpeed -= player.gravity * Time.deltaTime;

        Vector3 move = Vector3.up * player.vSpeed;
        player.characterController.Move(move * Time.deltaTime);

        float input = Input.GetAxis("Vertical");

        if (input != 0)
        {
            player.stateMachine.SwitchState(PlayerState.Move);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.SwitchState(PlayerState.Jump);
        }
    }
}

public class PlayerMoveState : StateBase
{
    private PlayerScript player;

    public PlayerMoveState(PlayerScript player)
    {
        this.player = player;
    }

    public override void OnStateEnter(object o = null)
    {
        player.animator.SetBool("Run", true);
    }

    public override void OnStateStay(object o = null)
    {
        float input = Input.GetAxis("Vertical");

        player.transform.Rotate(0, Input.GetAxis("Horizontal") * player.turnSpeed * Time.deltaTime, 0);

        var move = player.transform.forward * input * player.speed;

        if (player.characterController.isGrounded && player.vSpeed < 0)
            player.vSpeed = -2f;

        player.vSpeed -= player.gravity * Time.deltaTime;
        move.y = player.vSpeed;

        if (Input.GetKey(player.keyRun))
        {
            move *= player.speedRun;
            player.animator.speed = player.speedRun;
        }
        else
        {
            player.animator.speed = 1;
        }

        player.characterController.Move(move * Time.deltaTime);

        if (input == 0)
        {
            player.stateMachine.SwitchState(PlayerState.Idle);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.stateMachine.SwitchState(PlayerState.Jump);
        }
    }
}

public class PlayerJumpState : StateBase
{
    private PlayerScript player;


    public PlayerJumpState(PlayerScript player)
    {
        this.player = player;
    }

    public override void OnStateEnter(object o = null)
    {
        player.vSpeed = player.jumpSpeed;
    }

    public override void OnStateStay(object o = null)
    {
        float inputVertical = Input.GetAxis("Vertical");
        float inputHorizontal = Input.GetAxis("Horizontal");

        // Rotação no ar
        player.transform.Rotate(0, inputHorizontal * player.turnSpeed * Time.deltaTime, 0);

        // Movimento base
        var move = player.transform.forward * inputVertical * player.speed;

        // Controle reduzido no ar
        move *= player.airControl;

        // Gravidade
        player.vSpeed -= player.gravity * Time.deltaTime;
        move.y = player.vSpeed;

        player.characterController.Move(move * Time.deltaTime);

        // Quando encosta no chão
        if (player.characterController.isGrounded && player.vSpeed <= 0)
        {
            if (inputVertical != 0)
                player.stateMachine.SwitchState(PlayerState.Move);
            else
                player.stateMachine.SwitchState(PlayerState.Idle);
        }
    }
}

public class PlayerDeadState : StateBase
{
    private PlayerScript player;

    public PlayerDeadState(PlayerScript player)
    {
        this.player = player;
    }

    public override void OnStateEnter(object o = null)
    {
        player.characterController.enabled = false;
        player.animator.SetTrigger("Death");
        
    }
}
