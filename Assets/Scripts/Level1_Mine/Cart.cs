using UnityEngine;

public class Cart : MonoBehaviour
{
    [Header("Звуки")]
    [SerializeField] private AudioClip collectSound;

    [Header("Визуал")]
    [SerializeField] private GameObject highlightEffect;

    void OnTriggerEnter2D(Collider2D other)
    {
        DragDropItem item = other.GetComponent<DragDropItem>();
        
        if (item != null)
        {
            item.Collect();
            
            if (Audio.Instance != null && collectSound != null)
            {
                Audio.Instance.PlaySfx(collectSound);
            }
            
            if (highlightEffect != null)
            {
                StartCoroutine(ShowHighlight());
            }
        }
    }

    System.Collections.IEnumerator ShowHighlight()
    {
        if (highlightEffect != null)
        {
            highlightEffect.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            highlightEffect.SetActive(false);
        }
    }
}