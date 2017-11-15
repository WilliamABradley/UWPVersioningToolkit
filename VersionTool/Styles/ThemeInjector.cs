using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using System.Globalization;
using Windows.Foundation.Metadata;

namespace UWPVersioningToolkit.Styles
{
    public static class ThemeInjector
    {
        public static bool SupportsFluentAcrylic { get { return ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush"); } }

        private static Color ColorFromHex(string hexColor)
        {
            byte a = byte.Parse(hexColor.Substring(1, 2), NumberStyles.HexNumber);
            byte r = byte.Parse(hexColor.Substring(3, 2), NumberStyles.HexNumber);
            byte g = byte.Parse(hexColor.Substring(5, 2), NumberStyles.HexNumber);
            byte b = byte.Parse(hexColor.Substring(7, 2), NumberStyles.HexNumber);

            return Color.FromArgb(a, r, g, b);
        }

        public static void InjectThemeResources(ResourceDictionary dict)
        {
            var dark = dict.ThemeDictionaries["Dark"] as ResourceDictionary;
            var light = dict.ThemeDictionaries["Light"] as ResourceDictionary;

            string background = "PageBackground";

            string menu = "SplitViewBacking";
            string menuOpen = "SplitViewBackingOpen";
            string statusbar = "StatusBar";

            var FlatMenuDark = ColorFromHex("#FF171717");
            var FlatMenuLight = ColorFromHex("#FFF2F2F2");

            var FlatBackgroundDark = ColorFromHex("#FF000000");
            var FlatBackgroundLight = ColorFromHex("#FFFFFFFFF");

            var FlatStatusBarDark = Colors.DarkGray;
            var FlatStatusBarLight = Colors.LightGray;

            if (SupportsFluentAcrylic)
            {
                var DarkMenu = new AcrylicBrush
                {
                    TintColor = Colors.Black,
                    TintOpacity = 0.5,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    FallbackColor = FlatMenuDark
                };

                var LightMenu = new AcrylicBrush
                {
                    TintColor = Colors.White,
                    TintOpacity = 0.5,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    FallbackColor = FlatMenuLight
                };

                var DarkMenuOpen = new AcrylicBrush
                {
                    TintColor = Colors.Black,
                    TintOpacity = 0.5,
                    BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    FallbackColor = FlatMenuDark
                };

                var LightMenuOpen = new AcrylicBrush
                {
                    TintColor = Colors.White,
                    TintOpacity = 0.5,
                    BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    FallbackColor = FlatMenuLight
                };

                var DarkBackground = new AcrylicBrush
                {
                    TintColor = Colors.Black,
                    TintOpacity = 0.6,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    FallbackColor = FlatBackgroundDark
                };

                var LightBackground = new AcrylicBrush
                {
                    TintColor = Colors.White,
                    TintOpacity = 0.6,
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    FallbackColor = FlatBackgroundLight
                };

                var StatusBarBackgroundDark = new AcrylicBrush
                {
                    TintColor = Colors.Black,
                    BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    TintOpacity = 0.4,
                    FallbackColor = FlatStatusBarDark
                };

                var StatusBarBackgroundLight = new AcrylicBrush
                {
                    TintColor = Colors.White,
                    BackgroundSource = AcrylicBackgroundSource.Backdrop,
                    TintOpacity = 0.3,
                    FallbackColor = FlatStatusBarLight
                };

                dark[menu] = DarkMenu;
                light[menu] = LightMenu;

                dark[menuOpen] = DarkMenuOpen;
                light[menuOpen] = LightMenuOpen;

                dark[background] = DarkBackground;
                light[background] = LightBackground;

                dark[statusbar] = StatusBarBackgroundDark;
                light[statusbar] = StatusBarBackgroundLight;
            }
        }
    }
}