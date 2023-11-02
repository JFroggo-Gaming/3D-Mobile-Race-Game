using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class TextPulse : MonoBehaviour
{
    public List<TMP_Text> texts = new List<TMP_Text>(); // Lista komponentów tekstowych
    public float pulseSpeed = 1.0f; // Prędkość pulsacji

    private List<Vector3> initialScales = new List<Vector3>();
    private List<bool> pulsingUps = new List<bool>();

    private void Start()
    {
        foreach (var text in texts)
        {
            if (text != null)
            {
                // Zachowaj początkową skalę tekstu
                initialScales.Add(text.transform.localScale);
                pulsingUps.Add(true);

                // Rozpocznij pulsację tekstu jako część inicjalizacji.
                StartCoroutine(PulseText(text));
            }
        }
    }

    private IEnumerator PulseText(TMP_Text text)
    {
        int index = texts.IndexOf(text);

        while (true)
        {
            float targetScale = pulsingUps[index] ? initialScales[index].x * 1.2f : initialScales[index].x;
            float currentScale = text.transform.localScale.x;

            // Animacja zmiany skali w czasie
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * pulseSpeed;
                float scale = Mathf.Lerp(currentScale, targetScale, t);
                text.transform.localScale = new Vector3(scale, scale, 1);

                yield return null;
            }

            // Zmiana kierunku pulsacji
            pulsingUps[index] = !pulsingUps[index];

            yield return null;
        }
    }
}
