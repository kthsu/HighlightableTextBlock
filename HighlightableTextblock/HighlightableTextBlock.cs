using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace HighlightableTextBlock
{
    public class HighlightableTextBlock
    {
        public static Brush GetHighlightTextBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HighlightTextBrushProperty);
        }

        public static void SetHighlightTextBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(HighlightTextBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for HighlightTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightTextBrushProperty =
            DependencyProperty.RegisterAttached("HighlightTextBrush", typeof(Brush), typeof(HighlightableTextBlock), new PropertyMetadata(SystemColors.HighlightTextBrush));
        

        public static Brush GetHighlightBrush(DependencyObject obj)
        {
            return (Brush)obj.GetValue(HighlightBrushProperty);
        }

        public static void SetHighlightBrush(DependencyObject obj, Brush value)
        {
            obj.SetValue(HighlightBrushProperty, value);
        }

        // Using a DependencyProperty as the backing store for HighlightBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.RegisterAttached("HighlightBrush", typeof(Brush), typeof(HighlightableTextBlock), new PropertyMetadata(SystemColors.HighlightBrush));


        private static bool GetIsBusy(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsBusyProperty);
        }

        private static void SetIsBusy(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBusyProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsHighlighting.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached("IsBusy", typeof(bool), typeof(HighlightableTextBlock), new PropertyMetadata(false));


        public static string GetHightlightText(DependencyObject obj)
        {
            return (string)obj.GetValue(HightlightTextProperty);
        }

        public static void SetHightlightText(DependencyObject obj, string value)
        {
            obj.SetValue(HightlightTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for HightlightText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HightlightTextProperty =
            DependencyProperty.RegisterAttached("HightlightText", typeof(string), typeof(HighlightableTextBlock), new PropertyMetadata(string.Empty,OnHighlightTextChanged));

        private static void OnHighlightTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textblock = d as TextBlock;

            if (textblock != null)
            {
                var highlightText = e.NewValue as string;
                var textProperty = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));

                if (!String.IsNullOrEmpty(highlightText))
                {
                    textProperty.AddValueChanged(textblock, OnTextChanged);
                }
                else
                {
                    textProperty.RemoveValueChanged(textblock, OnTextChanged);
                }

                Highlight(textblock, highlightText);
            }
        }

        private static void OnTextChanged(object sender, EventArgs e)
        {
            var textblock = sender as DependencyObject;

            if (textblock != null &&
                !GetIsBusy(textblock))
            {
                Highlight(sender as TextBlock, GetHightlightText(textblock));
            }
        }

        private static void Highlight(TextBlock textblock, string toHighlight)
        {
            if (textblock == null) return;

            string text = textblock.Text;

            if (!String.IsNullOrEmpty(text))
            {
                SetIsBusy(textblock, true);

                if (!String.IsNullOrEmpty(toHighlight))
                {
                    var matches = Regex.Split(text, String.Format("({0})", toHighlight), RegexOptions.IgnoreCase);

                    textblock.Inlines.Clear();

                    var highlightBrush = GetHighlightBrush(textblock);
                    var highlightTextBrush = GetHighlightTextBrush(textblock);

                    foreach (var subString in matches)
                    {
                        if (String.Compare(subString, toHighlight, true) == 0)
                        {
                            textblock.Inlines.Add(new Span(new Run(subString)) { Background = highlightBrush, Foreground = highlightTextBrush });
                        }
                        else
                        {
                            textblock.Inlines.Add(subString);
                        }
                    }
                }
                else
                {
                    textblock.Inlines.Clear();
                    textblock.Text = text;
                }

                SetIsBusy(textblock, false);
            }            
        }
    }    
}
