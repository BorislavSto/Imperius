using Player;
using UnityEngine;

/// <summary>
/// VERY SIMPLE PLAYER ATTACK HANDLER JUST FOR TESTING
/// This is a script meant to test if the attack handler is ->
/// versatile and usable for both enemies and the player and if
/// all attack types work for both!!
/// </summary>
public class PlayerAttackHandler : AttackHandler
{
    [Header("Player Attack Configuration")] [SerializeField]
    private bool useMouseAiming = true;

    [SerializeField] private Transform playerTransform;

    private PlayerInputHandler inputHandler;

    private void Start()
    {
        inputHandler = InputManager.Instance.InputHandler;

        if (playerTransform == null)
            playerTransform = transform;
    }

    protected override void Update()
    {
        base.Update();

        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (inputHandler == null)
            inputHandler = InputManager.Instance.InputHandler;

        AttackContext ctx = CreateAttackContext();

        if (inputHandler.AttackPressed)
        {
            ExecuteAttackByIndex(0, ctx);
        }

        if (inputHandler.Skill1Pressed)
        {
            ExecuteAttackByIndex(1, ctx);
        }

        if (inputHandler.Skill2Pressed)
        {
            ExecuteAttackByIndex(2, ctx);
        }

        if (inputHandler.Skill3Pressed)
        {
            ExecuteAttackByIndex(3, ctx);
        }
    }

    private AttackContext CreateAttackContext()
    {
        Vector3 aimDirection = Vector3.forward;

        if (useMouseAiming)
        {
            aimDirection = inputHandler.GetAimDirection(playerTransform.position);
        }
        else
        {
            float horizontal = inputHandler.MoveHorizontal;
            float vertical = inputHandler.MoveVertical;

            if (horizontal != 0f || vertical != 0f)
                aimDirection = new Vector3(horizontal, 0f, vertical).normalized;
            else
                aimDirection = playerTransform.forward;
        }

        return new AttackContext
        {
            Animator = GetComponent<Animator>(),
            Audio = GetComponent<AudioSource>(),
            Attacker = gameObject,
            //Target = // for now cant get the target simple test script
        };
    }

    private void ExecuteAttackByIndex(int index, AttackContext ctx)
    {
        if (index < 0 || index >= attackDatas.Length)
        {
            Debug.LogWarning($"Attack index {index} is out of range. Available attacks: {attackDatas.Length}");
            return;
        }

        AttackData attackData = attackDatas[index];
        Attack(attackData, ctx, OnAttackFinished);
    }

    private void OnAttackFinished()
    {
    }
}