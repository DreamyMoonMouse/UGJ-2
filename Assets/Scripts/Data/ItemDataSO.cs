using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Game/Item Data")]
public class ItemDataSO : ScriptableObject
{
    public string itemName;
    public int value;
    public Sprite icon;
    public AudioClip collectSound;
}
