namespace Marbles.Initialization
{
    public interface IDataProvider
    {
        int GetActorsOnStart();
        int GetMarblesOnStart();
        int GetMarblesOnRuntime();
        int GetMarblesOffset();
        float GetDetectorSize();
    }   
}
