using UnityEngine;

[CreateAssetMenu(fileName = "EggData", menuName = "Game/Egg Data")]
public class EggData : ScriptableObject
{
    public string eggName;
    public Sprite spriteGood;
    public Sprite spriteBroken;
    public float baseValue;
    [Range(0, 1)] public float spawnChance;
    public float mass;
    [Range(0.01f, 2f)] public float scale = 1f;
}
