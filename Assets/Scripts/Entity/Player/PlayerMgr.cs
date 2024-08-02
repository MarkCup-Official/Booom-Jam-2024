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
        targetCamera.Init(moveController);
        targetCamera.SetPlayer(transform);





        fsm = new FSM();
        fsm.SetValue("mgr", this);
        fsm.AddState<State_IDLE>(0);
        fsm.AddState<State_PushBox>(1);
        fsm.AddState<State_CatchBox>(2);
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
            mgr.moveController.SetJumpForce(6.5f);
            mgr.moveController.SetSpeed(3.5f);
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
           
            mgr.moveController.JumpLogic(InputMgr.GetSpaceDown());
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            if (CheckBox() && InputMgr.IsCatching()&&mgr.moveController.IsGround) owner.SwitchState(1);
            if (CheckBox() && InputMgr.IsRaiseTheBox() && mgr.moveController.IsGround) owner.SwitchState(2);
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

            RaycastHit2D hit = Physics2D.Raycast(mgr.transform.position, Vector2.down, 1, 1 << 6);
            target = hit.collider.GetComponent<ICacthable>();
            
            target.Set(mgr.transform,CatchType.Push);
            target.OnEnterCatch();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (!(CheckBox() && InputMgr.IsCatching())) owner.SwitchState(0);

            target.OnUpdate();
        }
        public override void OnExit()
        {
            target.OnExitCatch();
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
            mgr.moveController.SetJumpForce(0f);
            mgr.moveController.SetSpeed(1.5f);

            RaycastHit2D hit = Physics2D.Raycast(mgr.transform.position, Vector2.down, 1, 1 << 6);
            if (hit == false) return;
            target = hit.collider.GetComponent<ICacthable>();
            target.Set(mgr.transform,CatchType.Raise);
            target.OnEnterCatch();
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            target.OnUpdate();
            if (!(CheckBox() && InputMgr.IsRaiseTheBox())) owner.SwitchState(0);
        }
        public override void OnExit()
        {
            target.OnExitCatch();
        }
    }
}
