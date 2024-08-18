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
    public Transform respawnPos;

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
    public void SetRespawnPos(Transform transform)
    {
        respawnPos = transform;
    }
    public void Kill()
    {
        if (respawnPos == null) return;
        GameObject deathParticle = ObjectPool.Instance.GetObject(playerView.killParticle);
        deathParticle.transform.position = transform.position;
        transform.position = respawnPos.position;
        playerView.SetSpringCenterBackToOriginPos();
        UIManager.Instance.GetUI(UIManager.DeathUI).OnShowUp();
       

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
            childFSM.AddState<State_Normal_Fan>(2);
            childFSM.AddState<State_Normal_Swim>(4);
            childFSM.AddTrisition(0, () => mgr.moveController.isTouchLadder && InputMgr.GetVertical() != 0f, 1);
            childFSM.AddTrisition(1, () =>
            {
                return (!mgr.moveController.isTouchLadder || InputMgr.GetSpaceDown());
            }, 0);
            childFSM.AddTrisition(0, () => mgr.moveController.OnFan, 2);
            childFSM.AddTrisition(2, () => !mgr.moveController.OnFan, 0);
            childFSM.AddTrisition(0, () => mgr.moveController.isInPool, 4);
            childFSM.AddTrisition(4, () => !mgr.moveController.isInPool, 0);

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
            childFSM.AddState<State_Swim_Force>(3);
            childFSM.AddTrisition(0, () => mgr.moveController.isTouchLadder && InputMgr.GetVertical() != 0f, 1);
            childFSM.AddTrisition(0, () => mgr.moveController.isTouchWater, 2);
            childFSM.AddTrisition(2, () => !mgr.moveController.isTouchWater, 0);
            childFSM.AddTrisition(2, () => mgr.moveController.isInPool, 3);
            childFSM.AddTrisition(3, () => !mgr.moveController.isTouchWater, 0);
            childFSM.AddTrisition(3, () => !mgr.moveController.isInPool, 2);
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
            mgr.moveController.GetRigidBody().drag = 0;
        }
        public override void OnUpdate()
        {
            childFSM.Update();
            mgr.targetCamera.FollowPlayer();

        }
        public override void OnFixedUpdate()
        {
            childFSM.FixedUpdate();
            //mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());

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
                mgr.speed = 3f;
                mgr.jumpForce = 9f;//6
                mgr.AnimationName = "Idle3";

            }
            else
            {
                mgr.AnimationName = "Idle4";
                mgr.speed = 3f;
                mgr.jumpForce = 9f;//6
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
    public class State_Normal_Swim : BaseState
    {
        public PlayerMgr mgr;
        int isJump = 0;
        float roof = 0;
        public State_Normal_Swim(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            if (mgr.battery == null)
            {
                mgr.AnimationName = "FanBall";
                mgr.UpdateState();
            }
            isJump = 0;
        }

        public override void OnExit()
        {
            base.OnExit();
            if (mgr.battery == null)
            {
                mgr.AnimationName = "Idle";
            }
            else
            {
                mgr.AnimationName = "Idle2";
            }
            mgr.UpdateState();
            mgr.playerView.Rotate(0);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());

            if (isJump > 0)
            {
                mgr.moveController.VerticalMove(-100);
                mgr.moveController.LimitVelocity(10);
                isJump--;
            }

        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            mgr.playerView.Flip(InputMgr.GetHorizontal());

            roof = mgr.moveController.CheckIsRoof();

            if (InputMgr.GetSpaceDown()&&isJump==0)
            {
                if(0 < roof && roof <0.2f)
                {
                    isJump = 15;
                }
                else
                {
                    mgr.moveController.JumpLogic(true);
                }
            }
            if (0< roof &&roof< 0.2f)
            {
                mgr.playerView.Rotate(180);
            }
            else
            {
                mgr.playerView.Rotate(0);
            }
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


    public class State_Normal_Fan_Air : BaseState
    {
        public PlayerMgr mgr;
        public State_Normal_Fan_Air(FSM owner) : base(owner)
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
                /*
                if (mgr.battery == null && !first)
                {
                    mgr.AnimationName = "FanBall";
                }
                else
                {
                    mgr.AnimationName = "Idle2";
                }
                mgr.UpdateState();
                */
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

    public class State_Normal_Fan_Roof : BaseState
    {
        public PlayerMgr mgr;
        public State_Normal_Fan_Roof(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        int isJump = 0;

        public override void OnEnter()
        {
            base.OnEnter();

            mgr.playerView.Rotate(180);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            //mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());

            mgr.moveController.AddForce(Vector2.up * 0.1f);

            if (isJump > 0)
            {
                mgr.moveController.VerticalMove(-100);
                isJump--;
            }

        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            mgr.playerView.Flip(-InputMgr.GetHorizontal());
        }
    }

    public class State_Normal_Fan_LeftWall : BaseState
    {
        public PlayerMgr mgr;
        public State_Normal_Fan_LeftWall(FSM owner) : base(owner)
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
                    /*
                    if (mgr.battery == null)
                    {
                        mgr.AnimationName = "Idle";
                        mgr.UpdateState();
                    }*/
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
                /*
                if (mgr.battery == null&&isOnWall)
                {
                    mgr.AnimationName = "FanBall";
                    mgr.UpdateState();
                }*/
                isOnWall = false;
            }
        }

        int shake = 0;

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (mgr.moveController.OnFanSpeed < 0.1f)
            {
                mgr.moveController.AddForce(Vector2.up * InputMgr.GetVertical() * 0.1f * 0.7f);
            }
            else
            {
                mgr.moveController.AddForce(Vector2.up * InputMgr.GetVertical() * mgr.moveController.OnFanSpeed * 0.7f);
            }
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
    public class State_Normal_Fan_RightWall : BaseState
    {
        public PlayerMgr mgr;
        public State_Normal_Fan_RightWall(FSM owner) : base(owner)
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
                    /*
                    if (mgr.battery == null)
                    {
                        mgr.AnimationName = "Idle";
                        mgr.UpdateState();
                    }*/
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

                /*
            if (mgr.battery == null && isOnWall)
            {
                mgr.AnimationName = "FanBall";
                mgr.UpdateState();
            }*/
                isOnWall = false;
            }

        }


        int shake = 0;
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            
            if (mgr.moveController.OnFanSpeed < 0.1f)
            {
                mgr.moveController.AddForce(Vector2.up * InputMgr.GetVertical() * 0.1f * 0.7f);
            }
            else
            {
                mgr.moveController.AddForce(Vector2.up * InputMgr.GetVertical() * mgr.moveController.OnFanSpeed * 0.7f);
            }
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

    public class State_Normal_Fan : BaseState
    {
        public PlayerMgr mgr;
        private FSM childFSM;

        private float isWall = 0;
        private float isRoof = 0;

        public State_Normal_Fan(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");


            childFSM = new FSM();
            childFSM.SetValue("mgr", mgr);
            childFSM.AddState<State_Normal_Fan_Air>(0);
            childFSM.AddState<State_Normal_Fan_LeftWall>(1);
            childFSM.AddState<State_Normal_Fan_RightWall>(2);
            childFSM.AddState<State_Normal_Fan_Roof>(3);
            childFSM.AddTrisition(0, () => isWall<-0.1f && mgr.moveController.OnFanDirection != Vector3.zero, 1);
            childFSM.AddTrisition(1, () => isWall > -0.1f, 0);
            childFSM.AddTrisition(0, () => isWall > 0.1f && mgr.moveController.OnFanDirection != Vector3.zero, 2);
            childFSM.AddTrisition(2, () => isWall < 0.1f, 0);
            childFSM.AddTrisition(0, () => 0 < isRoof && isRoof < 0.2f && mgr.moveController.OnFanDirection != Vector3.zero, 3);
            childFSM.AddTrisition(3, () => isRoof > 0.2f, 0);

            childFSM.AddTrisition(2, () => isWall < -0.1f && InputMgr.GetHorizontal() < -0.1f, 1);
            childFSM.AddTrisition(3, () => isWall < -0.1f && InputMgr.GetHorizontal() < -0.1f, 1);
            childFSM.AddTrisition(1, () => isWall > 0.1f && InputMgr.GetHorizontal() > 0.1f, 2);
            childFSM.AddTrisition(3, () => isWall > 0.1f && InputMgr.GetHorizontal() > 0.1f, 2);
            childFSM.AddTrisition(1, () => 0 < isRoof && isRoof < 0.2f && InputMgr.GetVertical() > 0.1f, 3);
            childFSM.AddTrisition(2, () => 0 < isRoof && isRoof < 0.2f && InputMgr.GetVertical() > 0.1f, 3);
            childFSM.AddTrisition(1, () => isRoof < 0 && isRoof > -0.2f && InputMgr.GetVertical() < -0.1f, 0);
            childFSM.AddTrisition(2, () => isRoof < 0 && isRoof > -0.2f && InputMgr.GetVertical() < -0.1f, 0);

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
        int isJumpV = 0;

        public override void OnUpdate()
        {
            base.OnUpdate();

            isRoof = mgr.moveController.CheckIsRoof();
            childFSM.Update();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            int currentState =childFSM.GetCurrentStateID();

            isWall = mgr.moveController.CheckIsWall();
            mgr.moveController.isWall = mgr.moveController.CheckIsWall();

            childFSM.FixedUpdate();

            if (isJump == 0 && (mgr.moveController.OnFanDirection != Vector3.left && mgr.moveController.OnFanDirection != Vector3.right))
                mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());
            if (isJumpV == 0&&(mgr.moveController.OnFanDirection != Vector3.down&& mgr.moveController.OnFanDirection != Vector3.up))
                mgr.moveController.VerticalMove(InputMgr.GetVertical()*4);

            if ((InputMgr.GetSpaceDown()) && Mathf.Abs(mgr.moveController.isWall) < 0.6f &&isWall<-0.1f&& currentState==1)
            {
                isJump = 12;
                mgr.playerView.springRb.velocity = Vector2.down * 25;
            }
            if ((InputMgr.GetSpaceDown()) && Mathf.Abs(mgr.moveController.isWall) < 0.6f && isWall > 0.1f && currentState == 2)
            {
                isJump = -12;
                mgr.playerView.springRb.velocity = Vector2.down  * 25;
            }
            if ((InputMgr.GetSpaceDown()) && Mathf.Abs(isRoof) < 0.2f && isRoof < 0 && currentState == 0)
            {
                isJumpV = 12;
                mgr.playerView.springRb.velocity = Vector2.down * 25;
            }
            if ((InputMgr.GetSpaceDown()) && Mathf.Abs(isRoof) < 0.2f && isRoof > 0 && currentState == 3)
            {
                isJumpV = -12;
                mgr.playerView.springRb.velocity = Vector2.down * 25;
            }
            if (isJump > 0)
            {
                mgr.moveController.HorizontalMove(100);
                isJump--;
            }
            if (isJump < 0)
            {
                mgr.moveController.HorizontalMove(-100);
                isJump++;
            }
            if (isJumpV > 0)
            {
                mgr.moveController.VerticalMove(100);
                isJumpV--;
            }
            if (isJumpV < 0)
            {
                mgr.moveController.VerticalMove(-100);
                isJumpV++;
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
            mgr.moveController.GetRigidBody().drag = 6;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            mgr.playerView.Flip(InputMgr.GetHorizontal());

        }
        public override void OnExit()
        {
            mgr.moveController.GetRigidBody().drag = 0;
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            mgr.moveController.VerticalMove(3 * InputMgr.GetVertical());
            mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());
        }


    }
    public class State_Swim_Force : BaseState
    {
        public PlayerMgr mgr;
        public State_Swim_Force(FSM owner) : base(owner)
        {
            mgr = owner.GetValue<PlayerMgr>("mgr");
        }

        public override void OnEnter()
        {
            mgr.moveController.GetRigidBody().drag = 6;
        }
        public override void OnUpdate()
        {
            base.OnUpdate();

            mgr.playerView.Flip(InputMgr.GetHorizontal());

        }
        public override void OnExit()
        {
            mgr.moveController.GetRigidBody().drag = 0;
        }
        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();
            //mgr.moveController.VerticalMove(3 * InputMgr.GetVertical());
            //mgr.moveController.HorizontalMove(InputMgr.GetHorizontal());

            if (InputMgr.GetVertical() > 0)
            {
                mgr.moveController.AddForce(new Vector2(InputMgr.GetHorizontal() * 0.15f, InputMgr.GetVertical() * 0.2f));
            }
            else
            {
                mgr.moveController.AddForce(new Vector2(InputMgr.GetHorizontal() * 0.15f, InputMgr.GetVertical() * 0.2f));
            }

        }


    }
}
