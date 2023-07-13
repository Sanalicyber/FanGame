using Framework.Core;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Models;
using Lofelt.NiceVibrations;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class HapticManager : BaseManager
    {
        private bool _isHapticOn;

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            if (!_isHapticOn)
                return;
        }

        public void InjectModel(GameModel gameModel)
        {
            _isHapticOn = gameModel.IsHapticOn;
            gameModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(gameModel.IsHapticOn))
                    _isHapticOn = gameModel.IsHapticOn;
            };
            HapticController.Init();
        }

        private void RunHaptic(HapticPatterns.PresetType HapticTypesEnum)
        {
            Vibrator.Haptic(HapticTypesEnum);
        }

        private void RunHaptic()
        {
            Vibrator.Vibrate();
        }
    }
}