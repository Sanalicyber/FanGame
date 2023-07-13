using GameFolders.Scripts.Helpers;
using Lofelt.NiceVibrations;

namespace GameFolders.Scripts.Controllers
{
    public class VibrationController : Singleton<VibrationController>
    {
        public void Vibrate()
        {
            HapticPatterns.PlayConstant(1.0f, 0.0f, 0.2f);
            //https://developer.lofelt.com/integrating-haptics/nice-vibrations-by-lofelt/#how-do-i-trigger-a-vibration
        }

        public void Haptic(HapticPatterns.PresetType haptic)
        {
            HapticPatterns.PlayPreset(haptic);
        }

        private bool _isLightPlaying = false;
        private bool _isHeavyPlaying = false;

        public void Light(float interval)
        {
            if (_isLightPlaying)
            {
                return;
            }

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
            _isLightPlaying = true;

            CoroutineController.DoAfterGivenTime(interval, () => _isLightPlaying = false);
        }

        public void Medium()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }

        public void Heavy(float interval)
        {
            if (_isHeavyPlaying)
            {
                return;
            }

            HapticPatterns.PlayPreset(HapticPatterns.PresetType.HeavyImpact);
            _isHeavyPlaying = true;

            CoroutineController.DoAfterGivenTime(interval, () => _isHeavyPlaying = false);
        }

        public void Success()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Success);
        }

        public void Failure()
        {
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.Failure);
        }

        public void StopHaptics()
        {
            HapticController.Stop();
        }
    }
}