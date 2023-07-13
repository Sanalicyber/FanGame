using GameFolders.Scripts.Controllers;
using Lofelt.NiceVibrations;

namespace GameFolders.Scripts.Helpers
{
    public static class Vibrator
    {
        private static VibrationController _vibrationController;

        private static VibrationController VibrationController
        {
            get
            {
                if (_vibrationController == null)
                {
                    _vibrationController = VibrationController.Instance;
                }

                return _vibrationController;
            }
        }

        public static void Vibrate()
        {
            VibrationController.Vibrate();
        }

        public static void Haptic(HapticPatterns.PresetType haptic)
        {
            VibrationController.Haptic(haptic);
        }

        public static void Light(float interval = 0)
        {
            VibrationController.Light(interval);
        }

        public static void Medium()
        {
            VibrationController.Medium();
        }

        public static void Heavy(float interval = 0)
        {
            VibrationController.Heavy(interval);
        }

        public static void Success()
        {
            VibrationController.Success();
        }

        public static void Failure()
        {
            VibrationController.Failure();
        }

        public static void StopHaptics()
        {
            VibrationController.StopHaptics();
        }
    }
}