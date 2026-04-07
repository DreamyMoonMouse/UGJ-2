using UnityEngine;

[CreateAssetMenu(fileName = "LetterData", menuName = "Game/Letter Data")]
public class LetterData : ScriptableObject
{
    [System.Serializable]
    public class LetterTexts
    {
        public string title1;
        public string content1;
        public string title2;
        public string content2;
    }

    [SerializeField] private LetterTexts[] _levels;

    public LetterTexts GetLevelTexts(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= _levels.Length)
        {
            return _levels[0];
        }
        return _levels[levelIndex];
    }

    public int GetLevelCount()
    {
        return _levels.Length;
    }
}
