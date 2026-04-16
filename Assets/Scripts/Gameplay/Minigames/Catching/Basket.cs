using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;
    [SerializeField] private SfxPlayer _sfxPlayer;
    [SerializeField] private AudioClip _catchSound;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Egg"))
        {
            Egg egg = other.GetComponent<Egg>();
            if (egg != null && !egg.IsBroken)
            {
                _wallet.Add(Mathf.RoundToInt(egg.Value));
                _sfxPlayer?.Play(_catchSound);
                Destroy(other.gameObject);
            }
        }
    }
}