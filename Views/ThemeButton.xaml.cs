using System.Windows;
using System.Windows.Controls;

using Icon = WPFUI.Common.Icon;

namespace TextEditor.Views;

public partial class ThemeButton : UserControl
{
    public ThemeButton()
    {
        InitializeComponent();

        bool isDarkTheme = WPFUI.Theme.Manager.CurrentTheme == WPFUI.Theme.Style.Dark;
        icon!.Glyph = isDarkTheme ? DarkMode : LightMode;

        themeButton.Click += (_, _) =>
        {
            switch (isDarkTheme)
            {
                case true:
                    icon!.Glyph = DarkMode;
                    WPFUI.Theme.Manager.Switch(WPFUI.Theme.Style.Dark, true);
                    break;

                case false:
                    icon!.Glyph = LightMode;
                    WPFUI.Theme.Manager.Switch(WPFUI.Theme.Style.Light, true);
                    break;
            }

            isDarkTheme = !isDarkTheme;
        };
    }

    #region Properties

    public static readonly DependencyProperty LightModeProperty = DependencyProperty.Register(
        "LightMode", typeof(Icon), typeof(ThemeButton), new PropertyMetadata(Icon.WeatherSunny48));

    public Icon LightMode
    {
        get => (Icon)GetValue(LightModeProperty);
        set => SetValue(LightModeProperty, value);
    }

    public static readonly DependencyProperty DarkModeProperty = DependencyProperty.Register(
        "DarkMode", typeof(Icon), typeof(ThemeButton), new PropertyMetadata(Icon.WeatherMoon48));

    public Icon DarkMode
    {
        get => (Icon)GetValue(DarkModeProperty);
        set => SetValue(DarkModeProperty, value);
    }

    #endregion Properties
}