namespace YAGTSS.Serialization
{
    [System.Serializable]
    public class SaveData
    {
        public static readonly int formatVersion = 1;

        public HighScores highScores = new HighScores();
    }
}
