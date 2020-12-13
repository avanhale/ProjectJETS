using UnityEngine;

namespace VRTK.CustomScripts
{
    public class VRTKCustom_TouchHapticFeedback : VRTKCustom_HapticFeedback
    {
        protected void OnEnable()
        {
            if (linkedObject != null)
            {
                linkedObject.InteractableObjectTouched += InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched += InteractableObjectUntouched;
            }
        }

        

        protected void OnDisable()
        {
            if (linkedObject != null)
            {
                linkedObject.InteractableObjectTouched -= InteractableObjectTouched;
                linkedObject.InteractableObjectUntouched -= InteractableObjectUntouched;
            }
        }

        protected virtual void InteractableObjectTouched(object sender, InteractableObjectEventArgs e)
        {
            var isRightHand = e.interactingObject == VRTK_DeviceFinder.GetControllerRightHand();
            _asyncHapticPulse = HapticPulseRoutine(m_pulseSpan, m_spanBetweenpulse, m_duration, m_pulseIntensity, m_isContinuous, isRightHand);
            StartCoroutine(_asyncHapticPulse);

            //Debug.Log($"Feedback {linkedObject.name} OnTouched.");
        }

        protected virtual void InteractableObjectUntouched(object sender, InteractableObjectEventArgs e)
        {
            CancelHapticPulse();

            //Debug.Log($"Feedback {linkedObject.name} OnUntouched.");
        }
    }
}