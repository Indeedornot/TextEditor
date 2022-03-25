using System.Windows;
using System.Windows.Media;

namespace TextEditor.Helpers
{
    public static class ControlHelpers
    {
        public static T? FindChild<T>(this DependencyObject parent) where T : UIElement
        {
            if (parent is T element) return element;
            int children = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < children; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child is not T uiElement)
                {
                    var tChild = FindChild<T>(child);
                    if (tChild is not null) return tChild;
                }
                else return uiElement;
            }
            return null;
        }
    }
}