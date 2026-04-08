using UnityEngine;

[CreateAssetMenu(fileName = "FigurineData", menuName = "Game/Figurine Data")]
public class FigurineData : ScriptableObject
{
    [Header("Основное")]
    public string figurineName;
    public float baseValue;
    public float spawnChance;

    [Header("Спрайт")]
    public Sprite itemSprite;
    public Sprite[] badSpriteVariants;

    [Header("Тип предмета (галочки)")]
    public bool isBadItem = false;
    public bool isEdible = false;
    public bool canBeBadVariant = false;

    [Header("Визуал")]
    public bool canHaveColor = false;
    public float minAlpha = 0.9f;
    public float maxAlpha = 1f;

    public float GetEffectiveSpawnChance()
    {
        float multiplier = 1f;
        
        if (canBeBadVariant && badSpriteVariants != null)
        {
            multiplier += badSpriteVariants.Length;
        }
        
        return spawnChance * multiplier;
    }
}