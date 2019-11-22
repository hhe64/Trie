using System;
using System.Windows;
using System.Windows.Controls;

namespace TrieWpf
{
    public class SetSelectionBehavior
    {


        public static SelectionPosition GetSelectionPosition(DependencyObject obj)
        {
            return (SelectionPosition)obj.GetValue(SelectionPositionProperty);
        }

        public static void SetSelectionPosition(DependencyObject obj, SelectionPosition value)
        {
            obj.SetValue(SelectionPositionProperty, value);
        }

        // Using a DependencyProperty as the backing store for SelectionPosition.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectionPositionProperty =
            DependencyProperty.RegisterAttached("SelectionPosition", typeof(SelectionPosition), typeof(SetSelectionBehavior), new PropertyMetadata(null, OnSelectionPositionChanged));

        private static void OnSelectionPositionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;
            if (textbox == null) return;

            var idx = GetSelectionPosition(textbox);
            if (idx == null) return;

            textbox.SelectionStart = idx.StartIndex;
            textbox.SelectionLength = idx.Length;
            textbox.Focus();
        }
    }
}
