namespace Assets._Scripts.Game
{
    public interface IGameSavesHandler
    {
        void DeleteAllSaves();
        void SaveGame();
    }
}