using Framework.Game;
using UnityEngine;

namespace GameFolders.Scripts.Models
{
    public class GameModel : BaseGameModel
    {
        #region Private Variables

        private readonly int _maxLevel = 13;

        private readonly int _minLevel = 5;
        // private string _jsonFileName;

        private readonly string _prefKeyLevelID = "LevelID";
        private readonly string _prefKeyShowingLevelNumber = "ShowingLevelID";
        private readonly string _prefKeyTutorial = "Tutorial";
        private readonly string _prefKeyMoney = "Money";
        private readonly string _prefKeyIsFirstSession = "IsFirstSession";
        private readonly string _prefKeyIsFirstWronglyKnitted = "IsFirstWronglyKnitted";
        private readonly string _prefKeyMoveCount = "MoveCount";
        protected string PrefKeyIsJoysticOn = "IsJoystickOn";

        #endregion

        #region Properties

        public bool LevelHasTutorial()
        {
            return LevelID <= _minLevel;
        }

        public int LevelID
        {
            get
            {
                if (PlayerPrefs.HasKey(_prefKeyTutorial))
                {
                    if (PlayerPrefs.GetInt(_prefKeyTutorial) < _minLevel)
                    {
                        return PlayerPrefs.GetInt(_prefKeyTutorial);
                    }
                }
                else
                {
                    PlayerPrefs.SetInt(_prefKeyTutorial, 0);
                    PlayerPrefs.SetInt(_prefKeyLevelID, 0);
                }

                if (!PlayerPrefs.HasKey(_prefKeyLevelID))
                {
                    PlayerPrefs.SetInt(_prefKeyLevelID, 0);
                    return 0;
                }

                return PlayerPrefs.GetInt(_prefKeyLevelID);
            }
            set
            {
                var tempValue = value;
                if (tempValue > _maxLevel) tempValue = _minLevel;
                //tempValue = Random.Range(0, maxLevel);
                if (tempValue <= _minLevel) PlayerPrefs.SetInt(_prefKeyTutorial, tempValue);
                PlayerPrefs.SetInt(_prefKeyLevelID, tempValue);
                ShowingLevelNumber++;
                OnPropertyChanged();
            }
        }

        public int ShowingLevelNumber
        {
            get => PlayerPrefs.GetInt(_prefKeyShowingLevelNumber, 1);
            set
            {
                PlayerPrefs.SetInt(_prefKeyShowingLevelNumber, value);
                OnPropertyChanged();
            }
        }

        public int Money
        {
            //get => 99999;
            get => PlayerPrefs.GetInt(_prefKeyMoney, 100);

            set
            {
                PlayerPrefs.SetInt(_prefKeyMoney, value);
                OnPropertyChanged();
            }
        }

        public int MoveCount
        {
            get => PlayerPrefs.GetInt(_prefKeyMoveCount, 10);
            set
            {
                PlayerPrefs.SetInt(_prefKeyMoveCount, value);
                OnPropertyChanged();
            }
        }

        public bool IsFirstSession
        {
            get => PlayerPrefs.GetInt(_prefKeyIsFirstSession, 1) == 1;
            set => PlayerPrefs.SetInt(_prefKeyIsFirstSession, value ? 1 : 0);
        }

        public bool IsFirstWronglyKnitted
        {
            get => PlayerPrefs.GetInt(_prefKeyIsFirstWronglyKnitted, 1) == 1;
            set => PlayerPrefs.SetInt(_prefKeyIsFirstWronglyKnitted, value ? 1 : 0);
        }

        public int PercentageOfTheRequiredCompletion { get; } = 90;

        public bool IsJoystickOn
        {
            get => PlayerPrefs.GetInt(prefKey_IsAdminOn) == 1 ? true : false;
            set
            {
                PlayerPrefs.SetInt(PrefKeyIsJoysticOn, value ? 1 : 0);
                OnPropertyChanged();
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}