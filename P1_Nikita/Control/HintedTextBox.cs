using System;
using System.Windows;
using System.Windows.Controls;

namespace P1_Nikita.Control;

public sealed class HintedTextBox : TextBox
{
    public static readonly DependencyProperty HintProperty = DependencyProperty.Register(nameof(Hint),
        typeof(string), typeof(HintedTextBox),
        new PropertyMetadata(string.Empty, OnHintPropertyChanged));

    public string Hint
    {
        get => (string) GetValue(HintProperty);
        set => SetValue(TextProperty, value);
    }

    public HintedTextBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(HintedTextBox),
            new FrameworkPropertyMetadata(typeof(HintedTextBox)));
    }
    
    private static void OnHintPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        d.SetValue(HintProperty, e.NewValue);
    }
}