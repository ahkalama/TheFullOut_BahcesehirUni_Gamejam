using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSettings : MonoBehaviour
{
    public GameObject[] Lamps;
    public GameObject[] Lights;
    private bool lightsBlinking = false;
    private Coroutine blinkCoroutine;
    public Animator dolapanim;
    public ParticleSystem vfx;

    public void Start()
    {
        vfx.Stop();
        for (int i = 0; i < Lamps.Length; i++)
        {
            Renderer lampRenderer = Lamps[i].GetComponent<Renderer>();
            if (lampRenderer != null)
            {
                Material material = lampRenderer.material;
                material.DisableKeyword("_EMISSION");
            }
            else
            {
                Debug.LogWarning($"Lamp {Lamps[i].name} does not have a Renderer component.");
            }
            Lights[i].SetActive(false);
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            dolapanim.SetTrigger("Open");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            vfx.Play();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            if (lightsBlinking)
            {
                StopBlinking();
            }
            else
            {
                StartBlinking();
            }
        }
    }

    public void StartBlinking()
    {
        lightsBlinking = true;
        blinkCoroutine = StartCoroutine(BlinkLightsCoroutine());
    }

    public void StopBlinking()
    {
        lightsBlinking = false;
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
        // Ensure all lights are off when stopping
        for (int i = 0; i < Lights.Length; i++)
        {
            Renderer lampRenderer = Lamps[i].GetComponent<Renderer>();
            if (lampRenderer != null)
            {
                Material material = lampRenderer.material;
                material.DisableKeyword("_EMISSION");
            }
            Lights[i].SetActive(false);
        }
    }

    IEnumerator BlinkLightsCoroutine()
    {
        while (lightsBlinking)
        {
            for (int i = 0; i < Lamps.Length; i++)
            {
                Renderer lampRenderer = Lamps[i].GetComponent<Renderer>();
                if (lampRenderer != null)
                {
                    Material material = lampRenderer.material;
                    material.EnableKeyword("_EMISSION");
                }
                Lights[i].SetActive(true);
                yield return new WaitForSeconds(0.1f);
                if (lampRenderer != null)
                {
                    Material material = lampRenderer.material;
                    material.DisableKeyword("_EMISSION");
                }
                Lights[i].SetActive(false);
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
