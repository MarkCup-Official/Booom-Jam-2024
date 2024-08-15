using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.FSM;
using GameFramework.FSM.Player;
using System;
public class PlayerMgr : MonoBehaviour
{
    public FSM fsm;
    public MoveController moveController { get; private set; }
    public PlayerDetector playerDetector { get; private set; }
    public PlayerView playerView { get; private set; }
    public CameraAction targetCamera;
    public float speed;
    public float jumpForce;
    public string AnimationName = "Idle";

    public Box battery;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
        playerDetector = GetComponent<PlayerDetector>();
        playerView = GetComponent<PlayerView>();
        playerView.Init(this);
        targetCamera.Init(this);
        targetCamera.SetPlayer(transform);

        fsm = new FSM();
        fsm.SetValue("mgr", this);
        fsm.AddState<PlayerNormalState>(0);
        fsm.AddState<PlayerSwimState>(1);
        fsm.AddTrisition(0, () => Input.GetKeyDown(KeyCode.T), 1);
        fsm.AddTrisition(1, () => Input.GetKeyDown(KeyCode.T), 0);
        fsm.InitDefaultState(0);

    }
    private void Update()
    {
        fsm.Update();
        fsm.FixedUpdate();
    }
    private void Start()
    {
        RemoveBattery();
    }
    public void EquipBattery(Box battery)
    {

        (fsm.GetCurrentState() as PlayerState).EquipBattery(battery);

    }
    public Box RemoveBattery()
    {
        return (fsm.GetCurrentState() as PlayerState).RemoveBattery();


    }
    public void UpdateState()
    {
        moveController.SetJumpForce(jumpForce);//12
        moveController.SetSpeed(speed); //3.5
        playerView.PlayAnimation(AnimationName);
    }

}
namespace GameFramework.FSM.Player
{
    public class PlayerState : BaseState
    {
        public PlayerState(FSM owner) : base(owner)
        {
        }
        public virtual void EquipBattery(Box battery)
        {

        }
        public virtual Box RemoveBattery()
        {
            return null;
        }
    }

    public class PlayerNormalState : PlayerState
    {
        public PlayerMgr mgr;
        private FSM childFSM;
        public PlayerNormalState(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
            childFSM = new FSM();
            childFSM.SetValue("mgr", mgr);
            childFSM.AddState<State_IDLE>(0);
            childFSM.AddState<State_ClimbLadder>(1);
            childFSM.AddState<State_FanVector>(2);
            childFSM.AddTrisition(0, () => mgr.moveController.isTouchLadder && InputMgr.GetVertical() != 0f, 1);
            childFSM.AddTrisition(1, () =>
            {
                return (!mgr.moveController.isTouchLadder || InputMgr.GetSpaceDown());
            }, 0);
            childFSM.AddTrisition(0, () => mgr.moveController.OnFan, 2);
            childFSM.AddTrisition(2, () => !mgr.moveController.OnFan, 0);

            childFSM.InitDefaultState(0);
        }
        public override void OnEnter()
        {
            Set();
        }
        public override void OnUpdate()
        {
            childFSM.Update();
            mgr.targetCamera.FollowPlayer();

        }
        public override void OnFixedUpdate()
        {
            childFSM.FixedUpdate();

        }

        public override void EquipBattery(Box battery)
        {
            mgr.playerView.blueLight.SetActive(true);
            mgr.battery = battery;
            Set();
        }
        public override Box RemoveBattery()
        {
            mgr.playerView.blueLight.SetActive(false);
            Box go = mgr.battery;
            mgr.battery = null;
            Set();

            return go;
        }
        public void Set()
        {
         

            if (mgr.battery == null)
            {
                mgr.speed = 3.5f;
                mgr.jumpForce = 11.5f;
                mgr.AnimationName = "Idle";

            }
            else
            {
                mgr.AnimationName = "Idle2";
               
                mgr.speed = 3;
                mgr.jumpForce = 9f;
            }
            
            mgr.UpdateState();
        }
    }
    public class PlayerSwimState : PlayerState
    {
        public PlayerMgr mgr;
        private FSM childFSM;


        public PlayerSwimState(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
            childFSM = new FSM();
            childFSM.SetValue("mgr", mgr);
            childFSM.AddState<State_IDLE>(0);
            childFSM.AddState<State_ClimbLadder>(1);
            childFSM.AddState<State_Swim>(2);
            childFSM.AddTrisition(0, () => mgr.moveController.isTouchLadder && InputMgr.GetVertical() != 0f, 1);
            childFSM.AddTrisition(0, () => mgr.moveController.isTouchWater, 2);
            childFSM.AddTrisition(2, () => !mgr.moveController.isTouchWater, 0);
            childFSM.AddTrisition(1, () =>
            {
                return (!mgr.moveController.isTouchLadder || InputMgr.GetSpaceDown());
            }, 0);



            childFSM.InitDefaultState(0);
        }
        public override void OnEnter()
        {

            Set();

        }
        public override void OnExit()
        {

        }
        public override void OnUpdate()
        {
            childFSM.Update();
            mgr.targetCamera.FollowPlayer();

        }
        public override void OnFixedUpdate()
        {
            childFSM.FixedUpdate();
            mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());

        }
        public override void EquipBattery(Box battery)
        {
            mgr.playerView.blueLight.SetActive(true);
            mgr.battery = battery;

            Set();
        }
        public override Box RemoveBattery()
        {
            mgr.playerView.blueLight.SetActive(false);


            Box go = mgr.battery;
            mgr.battery = null;
            Set();
            return go;
        }
        public void Set()
        {
            if (mgr.battery == null)
            {
                mgr.speed = 2.5f;
                mgr.jumpForce = 6f;
                mgr.AnimationName = "Idle3";

            }
            else
            {
                mgr.AnimationName = "Idle4";
                mgr.speed = 2;
                mgr.jumpForce = 6f;
            }
            mgr.UpdateState();
        }
    }



    public class State_IDLE : BaseState
    {
        public PlayerMgr mgr;
        public State_IDLE(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        public override void OnEnter()
        {

        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());

        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            mgr.moveController.JumpLogic(InputMgr.GetSpaceDown());
            mgr.playerView.Flip(InputMgr.GetHorizontal());

        }


    }
    public class State_ClimbLadder : BaseState
    {
        public PlayerMgr mgr;
        public State_ClimbLadder(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
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
            mgr.moveController.VerticalMove(4 * InputMgr.GetVertical());
            mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());
        }
        public override void OnUpdate()
        {
            base.OnUpdate();


            mgr.playerView.Flip(InputMgr.GetHorizontal());

        }


    }


    public class State_FanVector_Air : BaseState
    {
        public PlayerMgr mgr;
        public State_FanVector_Air(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        bool first = true;

        public override void OnEnter()
        {
            base.OnEnter();
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            mgr.playerView.Rotate(0);
            if (!first)
            {
                if (mgr.battery == null && !first)
                {
                    mgr.AnimationName = "FanBall";
                }
                else
                {
                    mgr.AnimationName = "Idle2";
                }
                mgr.UpdateState();
            }
            else
            {
                first = false;
            }

        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            mgr.moveController.AddForce(mgr.moveController.OnFanDirection * mgr.moveController.OnFanSpeed);
        }


        public override void OnUpdate()
        {
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            mgr.playerView.Rotate(0);
        }
    }

    public class State_FanVector_LiftWall : BaseState
    {
        public PlayerMgr mgr;
        public State_FanVector_LiftWall(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        bool isOnWall = false;


        public override void OnEnter()
        {
            base.OnEnter();
            isOnWall = false;
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            mgr.playerView.Rotate(0);
        }
        public override void OnUpdate()
        {
            base.OnUpdate();
            
            float v = InputMgr.GetVertical();
            if (Mathf.Abs(mgr.moveController.isWall) < 0.6f)
            {
                if (!isOnWall)
                {
                    isOnWall = true;
                    mgr.playerView.FlipNow(-1);
                    mgr.playerView.Rotate(90);
                    if (mgr.battery == null)
                    {
                        mgr.AnimationName = "Idle";
                        mgr.UpdateState();
                    }
                }
                if (v > 0.1f)
                {
                    mgr.playerView.FlipNow(-1);
                    mgr.playerView.Rotate(90);
                }
                else if (v < -0.1f)
                {
                    mgr.playerView.FlipNow(1);
                    mgr.playerView.Rotate(-90);
                }
            }
            else
            {
                mgr.playerView.Flip(InputMgr.GetHorizontal());
                mgr.playerView.Rotate(0);

                if (mgr.battery == null&&isOnWall)
                {
                    mgr.AnimationName = "FanBall";
                    mgr.UpdateState();
                }
                isOnWall = false;
            }
        }

        int shake = 0;

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            mgr.moveController.AddForce(Vector2.up * InputMgr.GetVertical() * mgr.moveController.OnFanSpeed * 0.7f);
            mgr.moveController.LimitVelocity(2);


            mgr.moveController.AddForce(Vector2.left * 0.1f);
            mgr.moveController.AddForce(mgr.moveController.OnFanDirection * mgr.moveController.OnFanSpeed * 0.5f);

            shake++;
            if (shake > 200)
            {
                mgr.playerView.springRb.velocity = mgr.moveController.OnFanDirection * mgr.moveController.OnFanSpeed * 20;
                shake = 0;
            }


        }
    }
    public class State_FanVector_RightWall : BaseState
    {
        public PlayerMgr mgr;
        public State_FanVector_RightWall(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        bool isOnWall = false;

        public override void OnEnter()
        {
            base.OnEnter();
            isOnWall = false;
            mgr.playerView.Flip(InputMgr.GetHorizontal());
            mgr.playerView.Rotate(0);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            float v = InputMgr.GetVertical();
            if (Mathf.Abs(mgr.moveController.isWall) < 0.6f)
            {
                if (!isOnWall)
                {
                    isOnWall = true;
                    mgr.playerView.FlipNow(-1);
                    mgr.playerView.Rotate(-90);
                    if (mgr.battery == null)
                    {
                        mgr.AnimationName = "Idle";
                        mgr.UpdateState();
                    }
                }
                if (v > 0.1f)
                {
                    mgr.playerView.FlipNow(1);
                    mgr.playerView.Rotate(90);
                }
                else if (v < -0.1f)
                {
                    mgr.playerView.FlipNow(-1);
                    mgr.playerView.Rotate(-90);
                }
            }
            else
            {
                mgr.playerView.Flip(InputMgr.GetHorizontal());
                mgr.playerView.Rotate(0);

                if (mgr.battery == null && isOnWall)
                {
                    mgr.AnimationName = "FanBall";
                    mgr.UpdateState();
                }
                isOnWall = false;
            }

        }


        int shake = 0;
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            mgr.moveController.AddForce(Vector2.up * InputMgr.GetVertical() * mgr.moveController.OnFanSpeed * 0.7f);
            mgr.moveController.LimitVelocity(2);


            mgr.moveController.AddForce(Vector2.right * 0.1f);
            mgr.moveController.AddForce(mgr.moveController.OnFanDirection * mgr.moveController.OnFanSpeed * 0.5f);


            shake++;
            if (shake > 200)
            {
                mgr.playerView.springRb.velocity = mgr.moveController.OnFanDirection * mgr.moveController.OnFanSpeed*20;
                shake = 0;
            }

        }
    }

    public class State_FanVector : BaseState
    {
        public PlayerMgr mgr;
        private FSM childFSM;

        private float isWall = 0;

        public State_FanVector(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");


            childFSM = new FSM();
            childFSM.SetValue("mgr", mgr);
            childFSM.AddState<State_FanVector_Air>(0);
            childFSM.AddState<State_FanVector_LiftWall>(1);
            childFSM.AddState<State_FanVector_RightWall>(2);
            childFSM.AddTrisition(0, () => isWall<-0.1f, 1);
            childFSM.AddTrisition(0, () => isWall > 0.1f, 2);
            childFSM.AddTrisition(1, () => isWall > -0.1f,0);
            childFSM.AddTrisition(2, () => isWall < 0.1f, 0);

            childFSM.InitDefaultState(0);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            mgr.moveController.DisableGravity();
            mgr.moveController.ChangePhysicMatSmooth(true);

            if (mgr.battery == null)
            {
                mgr.AnimationName = "FanBall";
                mgr.UpdateState();
            }
            childFSM.SwitchState(0);
            isJump = 0;

            mgr.playerView.springRb.velocity = mgr.moveController.OnFanDirection * mgr.moveController.OnFanSpeed;

        }
        public override void OnExit()
        {
            base.OnExit();
            mgr.moveController.EnableGravity();
            mgr.moveController.ChangePhysicMatSmooth(false);
            mgr.playerView.Rotate(0);


            if (mgr.battery == null)
            {
                mgr.AnimationName = "Idle";
            }
            else
            {
                mgr.AnimationName = "Idle2";
            }
            mgr.UpdateState();
        }

        int isJump = 0;

        public override void OnUpdate()
        {
            base.OnUpdate();

            childFSM.Update();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            isWall = mgr.moveController.CheckIsWall();
            mgr.moveController.isWall = mgr.moveController.CheckIsWall();

            childFSM.FixedUpdate();

            if(isJump==0)
            mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());

            if ((InputMgr.GetSpaceDown()) && Mathf.Abs(mgr.moveController.isWall) > -0.6f&&isWall<-0.1f)
            {
                isJump = 50;
                mgr.playerView.springRb.velocity = Vector2.down * 25;
            }
            if ((InputMgr.GetSpaceDown()) && Mathf.Abs(mgr.moveController.isWall) < 0.6f && isWall > 0.1f)
            {
                isJump = -50;
                mgr.playerView.springRb.velocity = Vector2.down  * 25;
            }
            if (isJump > 0)
            {
                mgr.moveController.HorizontalMove(100);
                //mgr.moveController.AddForce(Vector2.down * mgr.moveController.OnFanSpeed * 0.1f);
                isJump--;
            }
            if (isJump < 0)
            {
                mgr.moveController.HorizontalMove(-100);
                //mgr.moveController.AddForce(Vector2.down * mgr.moveController.OnFanSpeed * 0.1f);
                isJump++;
            }

            mgr.moveController.LimitVelocity(10);
        }


    }

    public class State_Swim : BaseState
    {
        public PlayerMgr mgr;
        public State_Swim(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        public override void OnEnter()
        {

        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            mgr.playerView.Flip(InputMgr.GetHorizontal());

        }
        public override void OnExit()
        {

        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            mgr.moveController.VerticalMove(3 * InputMgr.GetVertical());
        }


    }
}
