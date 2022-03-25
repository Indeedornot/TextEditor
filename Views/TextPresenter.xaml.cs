using System.Windows.Controls;

using TextEditor.Helpers;

namespace TextEditor.Views;

/// <summary>
/// Interaction logic for TextPresenter.xaml
/// </summary>
public partial class TextPresenter : UserControl
{
    public TextPresenter()
    {
        InitializeComponent();
    }

    private void ScrollChanged(object sender, ScrollChangedEventArgs e)
    {
        if (Equals(sender, sv1))
        {
            var sv2ScrollViewer = sv2.FindChild<ScrollViewer>();
            sv2ScrollViewer?.ScrollToHorizontalOffset(e.HorizontalOffset);
            sv2ScrollViewer?.ScrollToVerticalOffset(e.VerticalOffset);
        }

        else
        {
            var sv1ScrollViewer = sv1.FindChild<ScrollViewer>();
            sv1ScrollViewer?.ScrollToHorizontalOffset(e.HorizontalOffset);
            sv1ScrollViewer?.ScrollToVerticalOffset(e.VerticalOffset);
        }
    }
}