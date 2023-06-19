using ComputerInterface;
using ComputerInterface.ViewLib;
using UnityEngine;

namespace DevMinecraftMod.ComputerInterface
{
    public class MinecraftView : ComputerView
    {
        public static MinecraftView Instance;
        private readonly UISelectionHandler _selectionHandler;

        private readonly string titleColour = "2C9047";
        private readonly string subtitleColour = "824A30";
        private readonly string enableColour = "80FF80ff";
        private readonly string disableColour = "FF5454ff";
        private readonly string selectionColour = "808080ff";

        public MinecraftView()
        {
            Instance = this;

            _selectionHandler = new UISelectionHandler(EKeyboardKey.Up, EKeyboardKey.Down, EKeyboardKey.Enter);
            _selectionHandler.MaxIdx = 3;
            _selectionHandler.OnSelected += OnEntrySelected;
            _selectionHandler.ConfigureSelectionIndicator($"<color=#{selectionColour}>></color> ", "", "  ", "");
        }

        public override void OnShow(object[] args)
        {
            base.OnShow(args);

            UpdateScreen();
        }

        private void OnEntrySelected(int index)
        {
            switch (index)
            {
                case 0:
                    Plugin.Instance.sIndicatorEnabled = !Plugin.Instance.sIndicatorEnabled;

                    if (!Plugin.Instance.sIndicatorEnabled && !Plugin.Instance.lIndicatorEnabled)
                        Plugin.Instance.lIndicatorEnabled = true;

                    break;
                case 1:
                    Plugin.Instance.lIndicatorEnabled = !Plugin.Instance.lIndicatorEnabled;

                    if (!Plugin.Instance.sIndicatorEnabled && !Plugin.Instance.lIndicatorEnabled)
                        Plugin.Instance.sIndicatorEnabled = true;

                    break;
            }

            Plugin.Instance.SetSettings();
        }

        private void OnEntryAdjusted(int index, bool increase)
        {
            switch (index)
            {
                case 2:
                    float offset = increase ? 0.025f : -0.025f;
                    Plugin.Instance.musicVolume = Mathf.Clamp(Plugin.Instance.musicVolume + offset, 0.025f, 0.125f);
                    break;
                case 3:
                    float offsetB = increase ? 0.1f : -0.1f;
                    Plugin.Instance.blockVolume = Mathf.Clamp(Plugin.Instance.blockVolume + offsetB, 0.1f, 0.5f);
                    break;
            }
            Plugin.Instance.SetSettings();
        }

        public void UpdateScreen()
        {
            SetText(str =>
            {
                str.BeginCenter().MakeBar('-', SCREEN_WIDTH, 0, "ffffff10");
                str.AppendClr(PluginInfo.Name, titleColour)
                    .EndColor()
                    .Append(" v")
                    .Append(PluginInfo.Version).AppendLine();

                str.Append("by ").AppendClr("dev9998", subtitleColour)
                    .AppendLine();

                str.MakeBar('-', SCREEN_WIDTH, 0, "ffffff10").EndAlign().AppendLines(2);

                str.AppendLine(_selectionHandler.GetIndicatedText(0, $"Box Indicator: <color={(Plugin.Instance.sIndicatorEnabled ? "#" + enableColour : "#" + disableColour)}>[{(Plugin.Instance.sIndicatorEnabled ? "Enabled" : "Disabled")}]</color>"));
                str.AppendLine(_selectionHandler.GetIndicatedText(1, $"Line Indicator: <color={(Plugin.Instance.lIndicatorEnabled ? "#" + enableColour : "#" + disableColour)}>[{(Plugin.Instance.lIndicatorEnabled ? "Enabled" : "Disabled")}]</color>"))
                    .AppendLine();

                str.AppendLine(_selectionHandler.GetIndicatedText(2, $"Music Volume: <color={"#" + selectionColour}>{Plugin.Instance.musicVolume}</color>"));
                str.AppendLine(_selectionHandler.GetIndicatedText(3, $"Block Volume: <color={"#" + selectionColour}>{Plugin.Instance.blockVolume}</color>"))
                .AppendLines(1);

                str.BeginCenter()
                .AppendLine($"<color={"#" + selectionColour}>Update the screen with Option 1</color>")
                .EndAlign();

            });
        }

        public override void OnKeyPressed(EKeyboardKey key)
        {
            if (_selectionHandler.HandleKeypress(key))
            {
                UpdateScreen();
                return;
            }

            if (key == EKeyboardKey.Left || key == EKeyboardKey.Right)
            {
                OnEntryAdjusted(_selectionHandler.CurrentSelectionIndex, key == EKeyboardKey.Right);
                UpdateScreen();
            }

            switch (key)
            {
                case EKeyboardKey.Back:
                    ReturnToMainMenu();
                    break;
                case EKeyboardKey.Option1:
                    UpdateScreen();
                    break;
            }
        }
    }
}
