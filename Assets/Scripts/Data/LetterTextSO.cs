using UnityEngine;

[CreateAssetMenu(fileName = "LetterText", menuName = "Game/Letter Text")]
public class LetterTextSO : ScriptableObject
{
    [Header("Stage 1")]
    public string title1;
    public string content1;
    
    [Header("Stage 2")]
    public string title2;
    public string content2;
    
    [Header("Stage 3")]
    public string title3;
    public string content3;
    
    public bool hasRefuseButton;
    public int targetLevel;
}
