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
        playerView.Init(this);
        targetCamera.Init(this);
        targetCamera.SetPlayer(transform);
        fsm = new FSM();
        fsm.SetValue("mgr", this);
        fsm.AddState<State_IDLE>(0);
        fsm.AddState<State_CatchBox>(1);
        fsm.AddState<State_ClimbLadder>(2);
        fsm.AddTrisition(new List<int>() { 0, 1 }, () => {
            return moveController.isTouchLadder && InputMgr.GetVertical() != 0f;
        },2);
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
            mgr.moveController.AddWaterForce();
        }

        public bool CheckBox()
        {
            RaycastHit2D hit = Physics2D.Raycast(mgr.transform.position - new Vector3(0,0.6f,0), Vector2.up, 1.2f, 1 << 6);
            return hit;
        }
    }

    public class State_IDLE : PlayerState
    {
        public State_IDLE(FSM owner) : base(owner)
        {
            
        }

        public override void OnEnter()
        {
            mgr.playerView.PlayAnimation("Idle");
            mgr.moveController.SetJumpForce(12f);
            mgr.moveController.SetSpeed(3.5f);
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
           
            mgr.moveController.JumpLogic(InputMgr.GetSpaceDown());
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            if (CheckBox() && InputMgr.IsCatchButtonDown() && mgr.moveController.IsGround) owner.SwitchState(1);
        }
        

    }
    public class State_CatchBox : PlayerState
    {
        private ICacthable target;
        public State_CatchBox(FSM owner) : base(owner)
        {
        }

        public override void OnEnter()
        {
            mgr.moveController.SetJumpForce(8f);
            mgr.moveController.SetSpeed(2.5f);
            mgr.playerView.PlayAnimation("Idle2");

            RaycastHit2D hit = Physics2D.Raycast(mgr.transform.position, Vector2.down, 1, 1 << 6);
            if (hit == false) return;
            target = hit.collider.GetComponent<ICacthable>();
            target.Set(mgr.transform);
            target.OnEnterCatch();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            target.OnUpdate();
            mgr.moveController.JumpLogic(InputMgr.GetSpaceDown());
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            if (InputMgr.IsCatchButtonDown())
            {
                owner.SwitchState(0);
                target.OnExitCatch();
            }
                
        }
        public override void OnExit()
        {
            
        }
    }

    public class State_ClimbLadder : PlayerState
    {
        public State_ClimbLadder(FSM owner) : base(owner)
        {

        }

        public override void OnEnter()
        {
            mgr.moveController.SetGravity(false);
        }
        public override void OnExit()
        {
            mgr.moveController.SetGravity(true);
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate(); 
            mgr.moveController.VerticalMove( 4 *InputMgr.GetVertical());
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

          
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            if (!mgr.moveController.isTouchLadder || InputMgr.GetSpaceDown()) 
                owner.SwitchState(owner.preState);
        }
        

    }
}
