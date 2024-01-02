public class Alive : Node
{
    private CharacterInfo _characterInfo;
    public Alive(CharacterInfo characterInfo) : base()
    {
        _characterInfo = characterInfo;
    }

    public override NodeState Evaluate()
    {
        if (_characterInfo.IsDead)
        {
            State = NodeState.FAILURE;
            return State;
        }
        State = NodeState.SUCCESS;
        return State;
    }
}
