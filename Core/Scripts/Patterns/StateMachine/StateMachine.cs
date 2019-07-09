//Developed by Pavel Kravtsov.
using UnityEngine;

namespace Core.StateMachine
{
    /// <summary>
    /// Class StateMacine it is a base class of state machine.
    /// Init, Update, Close methods describes the behaviour of machine in state.
    /// </summary>
    public abstract class StateMachine : MonoBehaviour
    {
        #region Fields
        private int currentStateIndex;
        private State currentState;
        private State[] states;
        #endregion

        #region Properties
        protected State CurrentState
        {
            get { return currentState; }
            private set
            {
                currentState = value;
                if (currentState != null)
                    currentState.Init();
                else
                    Debug.LogWarning("[" + GetType() + "] " + "New state is null", this);
            }
        }

        protected int CurrentStateIndex
        {
            get { return currentStateIndex; }
            private set
            {
                if (currentState != null)
                    currentState.Close();
                currentStateIndex = value;
                CurrentState = states[currentStateIndex];
            }
        }
        #endregion

        #region Methods
        protected virtual void SetState(int index)
        {
            if (index >= 0 && index < states.Length)
                CurrentStateIndex = index;
        }

        protected void Update()
        {
            if (CurrentState != null)
                CurrentState.Update();
        }

        protected void CreateStates(int statesCount)
        {
            states = new State[statesCount];
        }

        protected void CreateStates(System.Type statesEnum)
        {
            states = new State[System.Enum.GetValues(statesEnum).Length];
        }

        /// <summary>
        /// Method InitializeState sets the Init, Update, Close handlers.
        /// </summary>
        /// <param name="stateIndex">Index of state. Convenient use the enum member.</param>
        /// <param name="init">The handler of init state.</param>
        /// <param name="update">The handler of update state.</param>
        /// <param name="close">The handler of close state.</param>
        protected void InitializeState(int stateIndex, State.StateHandler init, State.StateHandler update, State.StateHandler close)
        {
            if (stateIndex >= states.Length)
                Debug.LogError($"[{this.GetType()}] {System.Reflection.MethodInfo.GetCurrentMethod().Name} index of state {stateIndex} is out of range!");
            states[stateIndex] = new State(init, update, close);
        }

        protected abstract void InitializeMachine();
        #endregion
    }
}