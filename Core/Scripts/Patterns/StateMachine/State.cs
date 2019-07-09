//Developed by Pavel Kravtsov.
namespace Core.StateMachine
{
/// <summary>
/// Describes the current state of state machine.
/// Init, Update, Close methods describes the behaviour of machine in state.
/// </summary>
public class State
{
    public delegate void StateHandler();
    protected StateHandler InitCallback, UpdateCallback, CloseCallback;

    public State() { }
    public State(StateHandler init, StateHandler update, StateHandler close)
    {
        InitCallback = init;
        UpdateCallback = update;
        CloseCallback = close;
    }
    #region Methods
    public virtual void Init()
    {
        InitCallback?.Invoke();
    }

    public virtual void Update()
    {
        UpdateCallback?.Invoke();
    }

    public virtual void Close()
    {
        CloseCallback?.Invoke();
    }

    public override string ToString()
    {
        return string.Format("[State]: Init{0}, Close{1}, Update{2}", InitCallback != null ? InitCallback.Method.Name : "null", CloseCallback != null ? CloseCallback.Method.Name : "null", UpdateCallback != null ? UpdateCallback.Method.Name : "null");
    }
    #endregion
}
}