using Animation;
using Cinemachine;
using Itens;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
    public TrailRenderer trailRenderer;



    [Header("PlayerSettings")]
    public float speed = 1f;
    public float turnSpeed = 1f;
    public float gravity = 9.8f;
    public float jumpSpeed = 15f;
    public float health = 20;
    public float vSpeed = 0f;
    public float healAmount = 5f;
    public float maxHealth = 20f;



    [Header("PlayerInnerInfo")]
    [SerializeField] private float _currentHealth;
    [SerializeField] private float invulnerabilityTime = 0.5f;
    

    [Header("Run Setup")]
    public KeyCode keyRun = KeyCode.LeftShift;
    public float speedRun = 1.5f;

    [Header("Air Control")]
    public float airControl = 0.5f;

    private float oldJumpSpeed;

    [Header("MegaBullets Settings")]
    public float megaBulletDamageMultiplier = 6f;
    public float megaBulletSizeMultiplier = 2f;

    


    public enum PowerUpType
    {
        InfiniteBullets,
        SuperJump,
        MegaBullets,
        

    }

    [System.Serializable]
    public class ActivePowerUp
    {
        public PowerUpType type;
        public float timer;
    }

    private List<ActivePowerUp> activePowerUps = new List<ActivePowerUp>();

    private SkinChanger skinChanger;


    private PowerUpType? lastPowerUp = null;
    private float lastDamageTime = -Mathf.Infinity;
    private bool isDead = false;
    private bool isMegaBulletsActive = false;
    private bool isInfiniteBulletsActive = false;


    void Awake()
    {

        characterController = GetComponent<CharacterController>();
        characterController.enabled = true;

        skinChanger = GetComponent<SkinChanger>();

  
    }

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
        if (isDead) return;

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        { 
            Heal(); 
        }

        if (characterController == null || !characterController.enabled) return;

        UpdatePowerUps();

        stateMachine.Update();
    }

    public void ApplyPowerUp(PowerUpType type, float duration, float value = 0f)
    {
        var existing = activePowerUps.Find(p => p.type == type);

        if (existing != null)
        {
            existing.timer += duration;
        }
        else
        {
            activePowerUps.Add(new ActivePowerUp { type = type, timer = duration });
            StartPowerUp(type, value);
        }

        
        lastPowerUp = type;
        UpdateSkin();
    }
    void UpdatePowerUps()
    {
        for (int i = activePowerUps.Count - 1; i >= 0; i--)
        {
            activePowerUps[i].timer -= Time.deltaTime;

            if (activePowerUps[i].timer <= 0)
            {
                var type = activePowerUps[i].type;

                activePowerUps.RemoveAt(i);   
                EndPowerUp(type);             
            }
        }
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

        if (EffectsManagert.Instance != null)
        {
            EffectsManagert.Instance.ChangeVignette();
        }
        else
        {
            Debug.LogError("EffectsManager NULL");
        }

        if (ScreenShaker.Instance != null)
        {
            ScreenShaker.Instance.Shake(2f, 2f, 0.2f);
        }
        else
        {
            Debug.LogError("ScreenShaker NULL");
        }

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
            Invoke(nameof(Kill), 1.5f);
        }
    }

    protected virtual void Kill()
    {

        OnKill();
    }

    protected virtual void OnKill()
    {
        isDead = true;

        characterController.enabled = false;

        GameManager.Instance.RespawnPlayer();

        Destroy(gameObject);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Enemy"))
        {
            OnDamage(1);
        }
    }
    #endregion

    #region PowerUp Logic
    private void StartPowerUp(PowerUpType type, float value)
    {
        switch (type)
        {
            case PowerUpType.InfiniteBullets:
                EnableInfiniteBullets();
                break;

            case PowerUpType.SuperJump:
                EnableSuperJump();
                break;

            case PowerUpType.MegaBullets:
                EnableMegaBullets();
                break;


        }
    }

    private void EndPowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.InfiniteBullets:
                DisableInfiniteBullets(); 
                break;

            case PowerUpType.SuperJump:
                DisableSuperJump();
                break;

            case PowerUpType.MegaBullets:
                DisableMegaBullets();
                break;


        }
        UpdateLastPowerUp();
        UpdateSkin();

    }
    #endregion

    void UpdateLastPowerUp()
    {
        if (activePowerUps.Count == 0)
        {
            lastPowerUp = null;
            return;
        }

        lastPowerUp = activePowerUps[activePowerUps.Count - 1].type;
    }

    void UpdateSkin()
    {
        if (skinChanger == null)
        {
            Debug.LogError("SkinChanger NULL");
            return;
        }

        if (lastPowerUp == null)
        {
            skinChanger.ApplySkin(skinChanger.defaultSkin);
            return;
        }

        switch (lastPowerUp)
        {
            case PowerUpType.InfiniteBullets:
                skinChanger.ApplySkin(skinChanger.infiniteBulletsSkin);
                break;

            case PowerUpType.SuperJump:
                skinChanger.ApplySkin(skinChanger.superJumpSkin);
                break;

            case PowerUpType.MegaBullets:
                skinChanger.ApplySkin(skinChanger.megaBulletsSkin);
                break;
        }
    }


    #region PowerUp Methods
    void EnableInfiniteBullets()
    {
        isInfiniteBulletsActive = true;

    }

    void DisableInfiniteBullets()
    {
        isInfiniteBulletsActive = false;
    }

    void EnableSuperJump()
    {
        oldJumpSpeed = jumpSpeed;
        jumpSpeed = 50f;
    }

    void DisableSuperJump()
    {
        jumpSpeed = oldJumpSpeed;
    }

    void EnableMegaBullets()
    {
        isMegaBulletsActive = true;
    }

    void DisableMegaBullets()
    {
        isMegaBulletsActive = false;
    }

    

    public bool IsMegaBulletsActive()
    {
        return isMegaBulletsActive;
    }

    public bool IsInfiniteBulletsActive()
    {
        return isInfiniteBulletsActive;
    }

    #endregion  

    public void Heal()
    {
        var item = ItemManager.Instance.itemSetups
            .Find(i => i.itemType == ItemType.LifePack);

        if (item == null) return;

        if (item.soInt.value > 0)
        {
            ItemManager.Instance.RemoveByType(ItemType.LifePack, 1);

            _currentHealth += healAmount;

            if (_currentHealth > maxHealth)
                _currentHealth = maxHealth;

            if (healthUI != null)
            {
                healthUI.UpdateValue(maxHealth, _currentHealth);
            }
        }
    }



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
            player.trailRenderer.enabled = true;
            move *= player.speedRun;
            player.animator.speed = player.speedRun;
        }
        else
        {
            player.trailRenderer.enabled = false;
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
