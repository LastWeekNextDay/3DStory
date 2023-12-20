public class PlayerCharacterInfo : CharacterInfo
{
    public int TimesHit = 0;
    
    public PlayerCharacterInfo(float defaultSpeed = 5, float dashSpeed = 50, float dashDuration = 0.15f,
        float dashCooldown = 1.5f) 
        : base(defaultSpeed, dashSpeed, dashDuration, dashCooldown)
    {
    }
}
