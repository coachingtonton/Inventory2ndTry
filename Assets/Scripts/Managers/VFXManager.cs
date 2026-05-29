using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script exists to play particles affects when needed
/// TODO: need to find a way to feed the VFX to the 
/// </summary>

public enum VFX 
{
    HitBurst,
    Explosion,
    Dash,
    Muzzle
}

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance { get; private set; }

    //a STRUCT that bundles type and prefab as one
    //Allows adding particle type and prefab under one data type
    //the list will be used by the dictionary to store VFXEntries that will be instantiated
    //the PLAYVFX method that other scripts will use relies on the input of a particle enum type
    //that enum type will call the value for the VFX

    [System.Serializable] public struct VFXEntry 
    {
        public VFX particleType;
        public ParticleSystem prefab;
    }

    [SerializeField] List<VFXEntry> effects = new();
    //This list allows me to pair each enum to a particle prefab
    //List exists so at runtime the lookup dictionary can store the different VFX and their particle systems

    Dictionary<VFX, ParticleSystem> lookup;
    //Hand this dict a VFX and itll give you a particle system
    //the effects list will give this dict all the VFX created in the inspector 

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        lookup = new Dictionary<VFX, ParticleSystem>();

        //For every VFX in effects list, if a prefab is present it will be added to the dictonary 
        foreach(VFXEntry effectStruct in effects)
            if (effectStruct.prefab != null)
            {
                lookup[effectStruct.particleType] = effectStruct.prefab;
                //^Dict name ^Key for lookup        ^ value for each key 
            }
    }

    //PLAYVFX will be called by other methods to instantiate teh particles 
    public void PlayVFX(VFX type, Vector2 position, Quaternion rotation = default)
    {           //      ^ The dictionary key that returns the particle value
        if (!lookup.TryGetValue(type, out var dictPrefab))
        {
            Debug.LogWarning($"No VFX prefab assigned for {type}");
            return;
        }

        //instantiates the particlsystem via arguments, 
        //Type determines the prefab to instntiate via the dictionary
        //position is where itll need to spawn and rotation is its rotation
        ParticleSystem p = Instantiate(dictPrefab, position, rotation);
        
        //Makes the instantiated particle run. this is a method from ParticleSystems library
        //Particle destroys itself for now, eventually will need to learn object pooling.
        p.Play();
    }
}
