using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using GameFramework.FSM.Player;
public class PlayerMgr : MonoBehaviour
{
    private FSM fsm;
    public MoveController moveController { get; private set; }
    public PlayerView playerView { get; private set; }
    public CameraAction targetCamera;
    
    
    

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        playerView = GetComponent<PlayerView>();
        targetCamera.SetPlayer(transform);
        fsm = new FSM();
        fsm.SetValue("mgr", this);
        fsm.AddState<State_IsGround>(0);
        fsm.AddState<State_PushBox>(1);
        fsm.InitDefaultState(0);
    }
    private void Update()
    {
        fsm.Update();
        fsm.FixedUpdate();
    }
   
}
namespace GameFramework.FSM.Player
{
    public class PlayerState : BaseState
    {
        public PlayerMgr mgr;
        public PlayerState(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

     

        public override void OnUpdate()
        {
            mgr.targetCamera.FollowPlayer();
        }
        public override void OnFixedUpdate()
        {
            mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());
        }

        public bool CheckBox()
        {
            Debug.DrawRay(mgr.transform.position, Vector2.down);
            RaycastHit2D hit = Physics2D.Raycast(mgr.transform.position, Vector2.down, 1, 1 << 6);
            return hit && InputMgr.IsCatching();
        }
    }

    public class State_IsGround : PlayerState
    {
        public State_IsGround(FSM owner) : base(owner)
        {
            
        }

        public override void OnEnter()
        {
            mgr.moveController.SetJumpForce(6.5f);
            mgr.moveController.SetSpeed(3.5f);
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            mgr.moveController.JumpLogic(InputMgr.GetSpaceDown());
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            if (CheckBox()) owner.SwitchState(1);
        }
        

    }
    public class State_PushBox : PlayerState
    {
        private ICacthable target;
        public State_PushBox(FSM owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            mgr.moveController.SetJumpForce(0f);
            mgr.moveController.SetSpeed(1.5f);

            target = Physics2D.Raycast(mgr.transform.position, Vector2.down, 1, 1 << 6).collider.GetComponent<ICacthable>();
            target.SetOwner(mgr.transform);
            target.OnEnterCatch();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!CheckBox()) owner.SwitchState(0);

            target.OnUpdate();
        }
        public override void OnExit()
        {
            target.OnExitCatch();
        }
    }
}
