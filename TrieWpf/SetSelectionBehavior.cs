using System;
using System.Windows;
using System.Windows.Controls;

namespace TrieWpf
{
    public class SetSelectionBehavior
    {


        public static SelectionSpecifier GetIndex(DependencyObject obj)
        {
            return (SelectionSpecifier)obj.GetValue(IndexProperty);
        }

        public static void SetIndex(DependencyObject obj, SelectionSpecifier value)
        {
            obj.SetValue(IndexProperty, value);
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.RegisterAttached("Index", typeof(SelectionSpecifier), typeof(SetSelectionBehavior), new PropertyMetadata(null, OnIndexChanged));

        private static void OnIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;
            if (textbox == null) return;

            var idx = GetIndex(textbox);
            if (idx == null) return;

            textbox.SelectionLength = idx.SelectionLength;
            textbox.SelectionStart = idx.SelectionStart;
            textbox.Focus();
        }
    }
}
