using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace GameFramework.FSM
{
    public abstract class BaseState
    {
        private List<Func<bool>> actions = new List<Func<bool>>();
        private Dictionary<Func<bool>, BaseState> TrisitionDic = new Dictionary<Func<bool>, BaseState>();
        public  FSM owner;
        public BaseState(FSM owner)
        {
            this.owner = owner; 
           
        }
        //检测方法,判断是否转移到下一个状态
        public void Reason()
        {
            for (int i = 0; i < actions.Count; i++)
            {
                if (actions[i]?.Invoke() == true)
                {
                    owner.SwitchState(TrisitionDic[actions[i]]);
                    return;
                }
            }
        }
        public void AddMap(Func<bool> func,BaseState nextState)
        {
            TrisitionDic.Add(func, nextState);
            actions.Add(func);
        }

        public virtual void OnEnter()
        {

        }
        public virtual void OnUpdate()
        {

        }
        public virtual void OnFixedUpdate()
        {

        }
        public virtual void OnExit()
        {

        }


    }

}
