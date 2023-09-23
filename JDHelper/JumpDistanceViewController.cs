using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using SiraUtil.Logging;
using Zenject;

namespace JDHelper
{
    internal class JumpDistanceViewController : IInitializable, IDisposable, INotifyPropertyChanged
    {
        private IBeatmapObjectSpawnController _movementData;
        private IDifficultyBeatmap _beatmap;
        private StandardLevelDetailViewController _levelDetailViewController;

        [Inject] private SiraLog _log;
        [Inject] private Config _config;

        [UIComponent("jdModal")]
        private ModalView _enterJdModal;

        public event PropertyChangedEventHandler PropertyChanged;
        public string JDVIEW_PATH => "JDHelper.Resources.JDView.bsml";
        
        
        internal float jd;
        internal float rt;

        internal string jdDifference;
        internal string rtDifference;

        [UIValue("activityStatus")]
        public bool ActivityStatus = true;

        [UIValue("jd")]
        public string SetJDText => $"Set JD: {GetJumpDistance()} ({GetJumpDistanceDifference()})";

        [UIValue("rt")]
        private string jdInput;

        [UIAction("keyModal")]
        public void OpenKeyboardModal() => ToggleModal();

        [UIAction("applyJd")]
        public void ApplyJDAction() => ApplyJD();

        [UIAction("setActive")]
        public bool SetActive => ToggleActive();

        public void Initialize()
        {
            GameplaySetup.instance.AddTab("JDHelper", JDVIEW_PATH, this);
        }

        private bool ToggleActive()
        {
            if (_config.UseReactionTime == false)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SetActive)));
                return ActivityStatus = false;
            }

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SetActive)));
            return ActivityStatus = true;
        }

        private string GetJumpDistance()
        {
            _config.BaseJumpDistance = 14f;
            jd = _config.BaseJumpDistance;
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SetJDText)));
            return jd.ToString();
        }

        

        private string GetReactionTime()
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(nameof(SetRTText)));
            return rt.ToString();
        }

        private string GetJumpDistanceDifference()
        {
            // if new jd is higher than base jd, return "+ difference" with green text

            // if new jd is lower than base jd, return "- difference" with red text

            jdDifference = "+ 38";
            return jdDifference;
        }

        private string GetReactionTimeDifference()
        {
            // if new jd is higher than base jd, return "+ difference" with green text

            // if new jd is lower than base jd, return "- difference" with red text

            rtDifference = "+ 38";
            return rtDifference;
        }

        private void ToggleModal() => _enterJdModal.Show(true, true, null);


        private void ApplyJD()
        {

        }

        public void Dispose()
        {
            if (GameplaySetup.instance != null)
            {
                GameplaySetup.instance.RemoveTab("JDHelper");
            }
        }
    }
}
