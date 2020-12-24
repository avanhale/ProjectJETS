using UnityEngine;
using System.Collections;

public class VRTKCustom_Haptics : VRTK.CustomScripts.VRTKCustom_HapticFeedback
{
    public static VRTKCustom_Haptics instance;

    [Header("Throw Pulses")]
    public float strongThreshold;
    public float mediumThreshold;
    public Pulse throwPulseStrong;
    public Pulse blasterShot;
    public Pulse grabBlaster;

    [Header("HyperSpace Pulse")]
    public Pulse hyperSpacePulse;

    [Header("Sandcrawler Pulse")]
    public Pulse sandcrawlerPulse;

    [Header("Worm Slam")]
    public Pulse wormSlam;

    [Header("Bird Buzz")]
    public Pulse birdBuzz;

    [System.Serializable]
    public struct Pulse
    {
        [Range(0,5)]
        public float intensity, pulseDuration, intervalDuration, totalDuration;
        public bool isContinuous;

        public Pulse(float intensity, float pulseDuration, float intervalDuration, float totalDuration, bool isContinuous)
        {
            this.intensity = intensity; this.pulseDuration = pulseDuration; this.intervalDuration = intervalDuration; this.totalDuration = totalDuration; this.isContinuous = isContinuous;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    [ContextMenu("HyperSpace")]
    public void HyperSpace()
    {
        StartCoroutine(HyperPulser());
    }

    IEnumerator HyperPulser()
	{
        HapticPulse(hyperSpacePulse, true);
        HapticPulse(hyperSpacePulse, false);
        yield return new WaitForSeconds(2);
        HapticPulse(hyperSpacePulse, true);
        HapticPulse(hyperSpacePulse, false);
        yield return new WaitForSeconds(2);
        HapticPulse(hyperSpacePulse, true);
        HapticPulse(hyperSpacePulse, false);
    }

    [ContextMenu("BirdBuzz")]
    public void BirdBuzz()
    {
        HapticPulse(birdBuzz, false);
    }

    [ContextMenu("Sandcrawler")]
    public void SandcrawlerPulse()
    {
        HapticPulse(sandcrawlerPulse, true);
        HapticPulse(sandcrawlerPulse, false);
    }

    [ContextMenu("StopPulsing")]
    public void StopPulsing()
	{
        CancelHapticPulse();
        //StopCoroutine(_asyncHapticPulse);
        //HapticPulse(new Pulse(0, 0, 0, 0, false), true);
        //HapticPulse(new Pulse(0, 0, 0, 0, false), false);
    }

    public void ThrowStrong(bool isRightHand)
    {
        HapticPulse(throwPulseStrong, isRightHand);
    }


    public void BlasterShot()
    {
        HapticPulse(blasterShot, false);
    }

    [ContextMenu("GrabBlaster")]
    public void GrabBlaster()
    {
        HapticPulse(grabBlaster, false);
    }

    [ContextMenu("Hit")]
    public void Hit()
    {
        HapticPulse(grabBlaster, false);
        HapticPulse(grabBlaster, true);
    }

    [ContextMenu("WormSlam")]
    public void WormSlam()
    {
        HapticPulse(wormSlam, true);
        HapticPulse(wormSlam, false);
    }

    public void HapticPulse(Pulse pulse, bool isRightHand)
    {
        HapticPulse(pulse.pulseDuration, pulse.intervalDuration, pulse.totalDuration, pulse.intensity, pulse.isContinuous, isRightHand);
    }

    public void HapticPulse(float pulseDuration, float intervalDuration, float totalDuration, float intensity, bool isContinuous, bool isRHand)
    {
        _asyncHapticPulse = HapticPulseRoutine(pulseDuration, intervalDuration, totalDuration, intensity, isContinuous, isRHand);
        StartCoroutine(_asyncHapticPulse);

        //Debug.Log($"Feedback {linkedObject.name} OnHaptics.");
    }
}
