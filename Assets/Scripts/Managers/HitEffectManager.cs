using System.Collections;
using UnityEngine;
/// <summary>
/// This script exists to handle any effects that will happen on being damaged
/// Using the observer pattern this script will subscribe to the gameObjects Health script 
/// Health script has a ondamaged Event, HitEffectManager WILL subscribe to this method 
/// 
/// TODO: add a player layer check, if is player ad hit flash and IFrames 
/// </summary>

public class HitEffectManager : MonoBehaviour
{
    [Header("FIELDS")]
    public float HitFlashDuration;
    
    [SerializeField]

    private Health health;
    private SpriteRenderer sr;

    public void Awake()
    {
        health = GetComponent<Health>();
        sr = GetComponent<SpriteRenderer>();
    }

    #region OnEnable/Disable
    private void OnEnable()
    {
        health.onHealthChanged += HandleDamageEffects;
    }
    private void OnDisable()
    {
        health.onHealthChanged -= HandleDamageEffects;
    }
    #endregion

    public void HandleDamageEffects()
    {
        StartCoroutine(HitFlashRoutine());
    }

    public IEnumerator HitFlashRoutine()
    {
        Debug.Log("HitflashRutineStarted");
        sr.material.SetFloat("_FlashAmount", 1f);
        //          ^method   ^which property  ^value to set
        //String name for material will be searched for on Gameobject

        yield return new WaitForSeconds(HitFlashDuration);

        sr.material.SetFloat("_FlashAmount", 0); 
    }
}
