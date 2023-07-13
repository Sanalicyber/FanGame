using Framework.Core;
using GameFolders.Scripts.Helpers;

namespace GameFolders.Scripts.Events
{
    #region High Level Events

    #region Game Manager Events

    public class ResetTheManagersEventArgs : BaseEventArgs
    {
    }

    #endregion

    #endregion

    #region Mid Level Events

    public class AddRevertModelEventArgs : BaseEventArgs
    {
        public RevertModel Model { get; }

        public AddRevertModelEventArgs(RevertModel model)
        {
            Model = model;
        }
    }

    public class MoveCountChangedEventArgs : BaseEventArgs
    {
        public int MoveCount { get; }

        public MoveCountChangedEventArgs(int moveCount)
        {
            MoveCount = moveCount;
        }
    }

    #region Level Manger Events

    public class OnLevelCreatedEventArgs : BaseEventArgs
    {
        public int CorrectMoveCount { get; set; }
        //miyav progress
    }

    public class OnLevelCompletedEventArgs : BaseEventArgs
    {
        public int FinalMoveCount { get; set; }
        
        public OnLevelCompletedEventArgs(int finalMoveCount)
        {
            FinalMoveCount = finalMoveCount;
        }
    }

    #endregion

    #region Input Manager Events

    #endregion

    #region Player Manager Events

    public class OnPlayerCreatedEventArgs : BaseEventArgs
    {
        public int Id { get; set; }

        public OnPlayerCreatedEventArgs(int ıd)
        {
            Id = ıd;
        }
    }

    #endregion

    #endregion

    #region Presenter Events

    public class RevertButtonClickedEventArgs : BaseEventArgs
    {
    }

    public class SettingsButtonClickedEventArgs : BaseEventArgs
    {
    }

    public class SoundToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public SoundToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class HapticToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public HapticToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class JoystickToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public JoystickToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class MusicToggleClickedEventArgs : BaseEventArgs
    {
        public bool IsOn { get; }

        public MusicToggleClickedEventArgs(bool isOn)
        {
            IsOn = isOn;
        }
    }

    public class SettingsButtonCloseClickedEventArgs : BaseEventArgs
    {
    }

    public class NextLevelButtonClickedEventArgs : BaseEventArgs
    {
    }

    #endregion
}