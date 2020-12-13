using UnityEngine;
using System.Collections;

public class VRTKCustom_Haptics : VRTK.CustomScripts.VRTKCustom_HapticFeedback
{
    public static VRTKCustom_Haptics instance;

    [Header("Throw Pulses")]
    public float strongThreshold;
    public float mediumThreshold;
    public Pulse throwPulseStrong;
    public Pulse throwPulseMedium;
    public Pulse throwPulseWeak;

    [Header("OutOfBounds Pulse")]
    public Pulse outOfBoundsPulse;

    [Header("InBounds Pulse")]
    public Pulse inBoundsPulse;

    [Header("MPTurn Pulse")]
    public Pulse mpTurnPulse;

    [System.Serializable]
    public struct Pulse
    {
        [Range(0,1)]
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

    public void OutOfBounnds()
    {
        HapticPulse(outOfBoundsPulse, false);
    }
    public void InBounds()
    {
        HapticPulse(inBoundsPulse, false);
    }

    public void ThrowStrong(bool isRightHand)
    {
        HapticPulse(throwPulseStrong, isRightHand);
    }

    public void ThrowMedium(bool isRightHand)
    {
        HapticPulse(throwPulseMedium, isRightHand);
    }
    public void ThrowWeak(bool isRightHand)
    {
        HapticPulse(throwPulseWeak, isRightHand);
    }

    public void MPTurn()
	{
        HapticPulse(mpTurnPulse, false);
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
