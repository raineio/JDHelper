using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BeatSaberMarkupLanguage;
using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using BeatSaberMarkupLanguage.ViewControllers;
using HMUI;
using IPA.Utilities;
using SiraUtil.Logging;
using Zenject;

namespace JDHelper
{
    internal class JumpDistanceViewController : IInitializable, IDisposable, INotifyPropertyChanged
    {
        private IDifficultyBeatmap _beatmap;
        [Inject] private LevelSelectionNavigationController _levelSelectionNavigationController;
        [Inject] private GameplaySetupViewController _setupViewController;

        [Inject] private SiraLog _log;
        [Inject] private Config _config;

        internal static readonly FieldAccessor<GameplaySetupViewController, GameplayModifiersPanelController>.Accessor ModifiersView = FieldAccessor<GameplaySetupViewController, GameplayModifiersPanelController>.GetAccessor("_gameplayModifiersPanelController");

        [UIComponent("jdModal")]
        private ModalView _enterJdModal;

        public event PropertyChangedEventHandler PropertyChanged;
        public string JDVIEW_PATH => "JDHelper.Resources.JDView.bsml";

        internal float beatmapBaseJd;

        [UIValue("jd-text")]
        private string jdText => $"Set JD: {GetJumpDistance()} ({GetJumpDistanceDifference()})";

        [UIValue("rt-text")]
        private string rtText => $"Set RT: {GetReactionTime()} ({GetReactionTimeDifference()})";

        [UIValue("expected-range")]
        private string expectedRange => $"Expected Range: {GetExpectedRange()}";

        [UIValue("recommended-rt")]
        private string recommendedRt => $"Recommended RT: {GetRecommendedRT()}";

        [UIValue("jdInput")]
        private string jdInput { get; set; }

        [UIAction("keyModal")]
        public void OpenKeyboardModal() => ToggleModal();

        [UIAction("set-to-default")]
        private void SetToBeatmapBaseJD() => SetJD(beatmapBaseJd);

        [UIAction("applyJd")]
        public void ApplyJDAction() => ApplyJDFromInput();

        [UIValue("mod-enabled")]
        public bool SetActive
        {
            get => _config.Enabled;
            set => _config.Enabled = value;
        }

        public void Initialize()
        {
            GameplaySetup.instance.AddTab("JDHelper", JDVIEW_PATH, this);
            jdInput = _config.JumpDistance.ToString("0.00");
            _levelSelectionNavigationController.didChangeDifficultyBeatmapEvent += (controller, beatmap) => OnBeatmapSelect(beatmap);
            _levelSelectionNavigationController.didChangeLevelDetailContentEvent += (controller, type) => OnBeatmapSelect(controller?.selectedDifficultyBeatmap);
            ModifiersView(ref _setupViewController).didChangeGameplayModifiersEvent += OnMultiplierChange;
        }

        private void OnBeatmapSelect(IDifficultyBeatmap beatmap)
        {
            if (beatmap == null) return;
            _beatmap = beatmap;
            float beatmapBaseNjs = _beatmap.noteJumpMovementSpeed;
            float offset = _beatmap.noteJumpStartBeatOffset;
            float oneBeatDuration = _beatmap.level.beatsPerMinute.OneBeatDuration();
            float num = CoreMathUtils.CalculateHalfJumpDurationInBeats(4f, 18f, beatmapBaseNjs, oneBeatDuration, offset) * 2f;
            
            beatmapBaseJd = num * oneBeatDuration * beatmapBaseNjs;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(jdText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(rtText)));
        }

        private void OnMultiplierChange()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(rtText)));
        }

        private string GetJumpDistance()
        {
            return _config.JumpDistance.ToString("0.00");
        }

        private string GetReactionTime()
        {
            return JDToRT(_config.JumpDistance).ToString("0ms");
        }

        private string GetJumpDistanceDifference()
        {
            float diff = _config.JumpDistance - beatmapBaseJd;
            
            string formattedDifference = diff.ToString("+ 0.00;- #.00");
            string colouring = diff >= 0 ? "green" : "red";
            string output = $"<color={colouring}>{formattedDifference}</color>";
            return output;
        }

        private string GetReactionTimeDifference()
        {
            float diff = JDToRT(_config.JumpDistance) - JDToRT(beatmapBaseJd);

            string formattedDifference = diff.ToString("+ 0ms;- #ms");
            string colouring = diff >= 0 ? "green" : "red";
            string output = $"<color={colouring}>{formattedDifference}</color>";
            return output;
        }

        private string GetExpectedRange()
        {
            return "18-22JD";
        }

        private string GetRecommendedRT()
        {
            return "450ms";
        }

        private void ToggleModal() => _enterJdModal.Show(true, true, null);

        private void ApplyJDFromInput()
        {
            float newjD;
            if (float.TryParse(jdInput, out newjD)) SetJD(newjD);
            _enterJdModal.Hide(true);
        }

        private void SetJD(float jd)
        {
            _config.JumpDistance = jd;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(jdText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(rtText)));
        }

        private float JDToRT(float jd)
        {
            if (_beatmap == null) return 0;
            float speedMultiplier = ModifiersView(ref _setupViewController)?.gameplayModifiers?.songSpeedMul ?? 1;
            return jd / (2 * (_beatmap.noteJumpMovementSpeed * speedMultiplier)) * 1000;
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
