using UnityEngine;

[CreateAssetMenu(fileName = "FigurineData", menuName = "Game/Figurine Data")]
public class FigurineData : ScriptableObject
{
    [Header("Основное")]
    public string figurineName;
    public float baseValue;
    public float spawnChance;

    [Header("Спрайты")]
    public Sprite spriteGood;
    public Sprite[] spriteBadVariants;
    public Sprite spriteBad;

    [Header("Тип предмета (галочки)")]
    public bool isBadItem = false;
    public bool isEdible = false;

    [Header("Визуал")]
    public bool canHaveColor = false;
    public float minAlpha = 0.9f;
    public float maxAlpha = 1f;
}