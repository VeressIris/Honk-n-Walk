using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingFX : MonoBehaviour
{
    [SerializeField] private PostProcessVolume postProcessingVol;
    private ChromaticAberration chromaticAberration;
    private LensDistortion lensDistortion;
    private Vignette vignette;
    [SerializeField] private AnimationCurve chromaticAberrationCurve;
    [SerializeField] private AnimationCurve lensDistortionCurve;
    [SerializeField] private AnimationCurve vignetteCurve;


    void Start()
    {
        chromaticAberration = postProcessingVol.profile.GetSetting<ChromaticAberration>();
        lensDistortion = postProcessingVol.profile.GetSetting<LensDistortion>();
        vignette = postProcessingVol.profile.GetSetting<Vignette>();
    }

    public IEnumerator PlayerDamageEffect(float duration)
    {
        float time = 0f;
        while (time < duration)
        {
            chromaticAberration.intensity.value = chromaticAberrationCurve.Evaluate(time);
            lensDistortion.intensity.value = lensDistortionCurve.Evaluate(time);
            vignette.intensity.value = vignetteCurve.Evaluate(time);

            time += Time.deltaTime;
            yield return null;
        }
    }
}
