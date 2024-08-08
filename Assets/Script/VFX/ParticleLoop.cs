using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleLoop : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem = null;
    [SerializeField]
    private float loopTime = 1.0f;
    private float timer = 0.0f;

    void Start()
    {
        if (particleSystem == null)
        {
            particleSystem = GetComponent<ParticleSystem>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= loopTime)
        {
            particleSystem.Stop();
            particleSystem.Play();
            timer = 0.0f;
        }
    }
}
