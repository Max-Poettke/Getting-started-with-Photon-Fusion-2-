public abstract class GamePhase
{
    protected GamePlayState game;

    protected GamePhase(GamePlayState game)
    {
        this.game = game;
    }

    // Called once when the phase starts
    public virtual void Enter() { }

    // Called every frame while active (optional)
    public virtual void Tick() { }

    // Called once when the phase ends
    public virtual void Exit() { }
}
