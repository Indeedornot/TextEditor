using System;
using System.ComponentModel;
using System.Windows;

namespace TextEditor.Helpers;

public class BindableTextEditor : ICSharpCode.AvalonEdit.TextEditor, INotifyPropertyChanged
{
    /// <summary>
    /// A bindable Text property
    /// </summary>
    public new string Text
    {
        get { return base.Text; }
        set { base.Text = value; }
    }

    /// <summary>
    /// The bindable text property dependency property
    /// </summary>
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(BindableTextEditor), new PropertyMetadata((obj, args) =>
        {
            var target = (BindableTextEditor)obj;
            target.Text = (string)args.NewValue;
        }));

    protected override void OnTextChanged(EventArgs e)
    {
        RaisePropertyChanged("Text");
        base.OnTextChanged(e);
    }

    /// <summary>
    /// Raises a property changed event
    /// </summary>
    /// <param name="property">The name of the property that updates</param>
    private void RaisePropertyChanged(string property)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}