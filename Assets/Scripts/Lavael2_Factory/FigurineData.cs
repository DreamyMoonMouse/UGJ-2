using UnityEngine;

[CreateAssetMenu(fileName = "FigurineData", menuName = "Game/Figurine Data")]
public class FigurineData : ScriptableObject
{
    public string figurineName;
    public Sprite spriteGood;
    public Sprite[] spriteCrackedVariants;
    public float baseValue;
    public float spawnChance;
    public bool canHaveColor;
    public float minAlpha = 0.9f;
    public float maxAlpha = 1f;
}