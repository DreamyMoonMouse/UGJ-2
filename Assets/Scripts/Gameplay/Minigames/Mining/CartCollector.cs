using UnityEngine;

public class CartCollector : MonoBehaviour
{
    [SerializeField] private AudioClip _collectSound;
    
    public void PlayCollectSound()
    {
        if (_collectSound != null)
            AudioSource.PlayClipAtPoint(_collectSound, Camera.main.transform.position);
    }
}