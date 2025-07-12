using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArrowSystem;

public class PlayerInputManager : MonoBehaviour
{
    [SerializeField]
    private ArrowSystem arrowSystem;
    [SerializeField]
    private EnemyAttackController enemyAttackController;

    private PlayerInput inputActions;

    private void Awake()
    {
        inputActions = new PlayerInput();
    }

    private void OnEnable()
    {
        inputActions.GamePlay.Enable();

        inputActions.GamePlay.InputUp.performed += ctx => arrowSystem.CheckInput(ArrowKey.Up);
        inputActions.GamePlay.InputDown.performed += ctx => arrowSystem.CheckInput(ArrowKey.Down);
        inputActions.GamePlay.InputLeft.performed += ctx => arrowSystem.CheckInput(ArrowKey.Left);
        inputActions.GamePlay.InputRight.performed += ctx => arrowSystem.CheckInput(ArrowKey.Right);
        inputActions.GamePlay.InputSpace.performed += ctx => arrowSystem.CheckInput(ArrowKey.Space);
        inputActions.GamePlay.ParryingLeft.performed += ctx => enemyAttackController.ParryEnemyAttack(0);
        inputActions.GamePlay.ParryingRight.performed += ctx => enemyAttackController.ParryEnemyAttack(1);

    }

    private void OnDisable()
    {
        inputActions.GamePlay.Disable();
    }
}
