using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyClass.FSM;
using System.Reflection;
using System;
namespace MyClass.FSM
{
    public class FSM
    {
        
        protected Dictionary<int,BaseState> FSMActDic;//状态列表
        protected Dictionary<string, object> FSMValueDic;
        private BaseState curState;
        private BaseState defaultState;

        public FSM()
        {
            FSMActDic = new Dictionary<int,BaseState>();
            FSMValueDic = new Dictionary<string, object>();
        }

        public void SetValue(string key, object value)
        {
            if(!FSMValueDic.ContainsKey(key))
            FSMValueDic.Add(key, value);
            else
            FSMValueDic[key] = value;
        }
        public T GetValue<T>(string key) where T:class
        {
            if (!FSMValueDic.ContainsKey(key)) throw new Exception("no such key in the FSM value Dictionary");
            return FSMValueDic[key] as T;
        }
        public void InitDefaultState(int StateID)
        {
            if (defaultState != null) return;

            if(FSMActDic.ContainsKey(StateID))
            {
                defaultState = FSMActDic[StateID];
                curState = defaultState;
                curState.OnEnter();
            }
        }
        //根据类型自动创建状态
        public void AddState<T>(int stateID)where T:BaseState
        {
            ConstructorInfo constructor = typeof(T).GetConstructor(new Type[] {typeof(FSM)});
            AddState(stateID, (T)constructor.Invoke(new object[] { this }));
        }
        public void AddState(int stateID,BaseState state)
        {
            if (FSMActDic.ContainsKey(stateID)) throw new Exception("the state is alredy generated");
            FSMActDic.Add(stateID, state);
        }
        public void AddTrisition(int stateID,Func<bool> func,int nextstateID)
        {
            if (FSMActDic.ContainsKey(stateID) && FSMActDic.ContainsKey(nextstateID))
            {
                FSMActDic[stateID].AddMap(func,FSMActDic[nextstateID]);
            }
        }
        public void AddTrisition(List<int> stateIDList, Func<bool> func, int nextstateID)
        {
            foreach(var item in stateIDList)
            if (FSMActDic.ContainsKey(item) && FSMActDic.ContainsKey(nextstateID))
            {
                FSMActDic[item].AddMap(func, FSMActDic[nextstateID]);
            }
        }
        public BaseState GetState(int stateID)
        {
            if (FSMActDic.ContainsKey(stateID))
            {
                return FSMActDic[stateID];
            }
            Debug.Log("Cant Find stateID");
            return null;
        }
        public BaseState GetCurrentState()
        {
            return curState;
        }
        public virtual void SwitchState(BaseState nextState)
        {
            if (curState == nextState) return;
            curState.OnExit();
            curState = nextState;
            curState.OnEnter();
        }
        public void SwitchState(int nextstateID)
        {
            if(FSMActDic.ContainsKey(nextstateID))
            SwitchState(FSMActDic[nextstateID]);
        }

        public void Update()
        {
            curState.Reason();
            curState.OnUpdate();
        }

    }
}

