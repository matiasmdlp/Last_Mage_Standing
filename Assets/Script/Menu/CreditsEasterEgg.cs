using UnityEngine;
using UnityEngine.UI; 
using System.Collections;

public class CreditsEasterEgg : MonoBehaviour
{
    [Header("Configuración")]
    public Image secretImage; 
    public float delayTime = 3f;    
    public float fadeDuration = 2f; 
    
    [Range(0f, 1f)] 
    public float maxAlpha = 0.5f;   

    [Header("Audio (Opcional)")]
    public AudioSource audioSource;

    void OnEnable()
    {
        if (secretImage != null)
        {
            secretImage.gameObject.SetActive(true);
            SetImageAlpha(0f);
        }

        StartCoroutine(ShowImageRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
        
        if (secretImage != null)
        {
            SetImageAlpha(0f);
            secretImage.gameObject.SetActive(false);
        }
    }

    IEnumerator ShowImageRoutine()
    {
        yield return new WaitForSeconds(delayTime);

        if (audioSource != null) audioSource.Play();

        float timer = 0f;
        
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime; 
            
            float progress = timer / fadeDuration;
            
            float newAlpha = Mathf.Lerp(0f, maxAlpha, progress);
            
            SetImageAlpha(newAlpha);

            yield return null; 
        }

        SetImageAlpha(maxAlpha);
    }

    void SetImageAlpha(float alpha)
    {
        if (secretImage != null)
        {
            Color c = secretImage.color;
            c.a = alpha;
            secretImage.color = c;
        }
    }
}