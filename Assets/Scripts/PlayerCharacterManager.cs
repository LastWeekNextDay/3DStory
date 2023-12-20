public class PlayerCharacterManager : CharacterManager
{
    private new void Awake()
    {
        CharacterInfo = new PlayerCharacterInfo(dashDuration: 0.1f);
        base.Awake();
    }
}
