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
        private static bool GetIsHighlighting(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsHighlightingProperty);
        }

        private static void SetIsHighlighting(DependencyObject obj, bool value)
        {
            obj.SetValue(IsHighlightingProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsHighlighting.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsHighlightingProperty =
            DependencyProperty.RegisterAttached("IsHighlighting", typeof(bool), typeof(HighlightableTextBlock), new PropertyMetadata(false));


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

                HighlightParts(textblock, highlightText);
            }
        }

        private static void OnTextChanged(object sender, EventArgs e)
        {
            var textblock = sender as DependencyObject;

            if (textblock != null &&
                !GetIsHighlighting(textblock))
            {
                HighlightParts(sender as TextBlock, GetHightlightText(textblock));
            }
        }

        private static void HighlightParts(TextBlock textblock, string toHighlight)
        {
            if (textblock == null) return;

            string text = textblock.Text;

            SetIsHighlighting(textblock, true);

            if (!String.IsNullOrEmpty(text))
            {
                if (!String.IsNullOrEmpty(toHighlight))
                {
                    var matches = Regex.Split(text, String.Format("({0})", toHighlight), RegexOptions.IgnoreCase);

                    textblock.Inlines.Clear();

                    foreach (var subString in matches)
                    {
                        if (String.Compare(subString, toHighlight, true) == 0)
                        {
                            textblock.Inlines.Add(new Span(new Run(subString)) { Background = SystemColors.HighlightBrush, Foreground = SystemColors.HighlightTextBrush });
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
            }

            SetIsHighlighting(textblock, false);
        }
    }

    public class HighlightableTextBlock2 : TextBlock
    {
        #region Fields

        // Using a DependencyProperty as the backing store for HighlightText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightTextProperty =
            DependencyProperty.Register("HighlightText", typeof(string), typeof(HighlightableTextBlock2), new PropertyMetadata(OnHighlightTextChanged));

        // Using a DependencyProperty as the backing store for HighlightBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightBrushProperty =
            DependencyProperty.Register("HighlightBrush", typeof(Brush), typeof(HighlightableTextBlock2), new PropertyMetadata(SystemColors.HighlightBrush));

        // Using a DependencyProperty as the backing store for HighlightTextBrush.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightTextBrushProperty =
            DependencyProperty.Register("HighlightTextBrush", typeof(Brush), typeof(HighlightableTextBlock2), new PropertyMetadata(SystemColors.HighlightTextBrush));

        #endregion

        #region Constructors

        public HighlightableTextBlock2()
        {
            var textProperty = DependencyPropertyDescriptor.FromProperty(HighlightableTextBlock2.TextProperty, typeof(HighlightableTextBlock2));

            textProperty.AddValueChanged(this, (sender, e) =>
            {
                HighlightParts(sender as TextBlock, HighlightText);
            });
        }

        #endregion 

        public Brush HighlightBrush
        {
            get { return (Brush)GetValue(HighlightBrushProperty); }
            set { SetValue(HighlightBrushProperty, value); }
        }

        public Brush HighlightTextBrush
        {
            get { return (Brush)GetValue(HighlightTextBrushProperty); }
            set { SetValue(HighlightTextBrushProperty, value); }
        }

        public string HighlightText
        {
            get { return (string)GetValue(HighlightTextProperty); }
            set { SetValue(HighlightTextProperty, value); }
        }

        private static void OnHighlightTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textblock = d as TextBlock;

            if (textblock != null)
            {
                var text = textblock.Text;
                var highlightText = e.NewValue as string;

                HighlightParts(textblock, highlightText);
            }
        }

        private static void HighlightParts(TextBlock textblock, string toHighlight)
        {
            if (textblock == null) return;

            string text = textblock.Text;

            if (!String.IsNullOrEmpty(text))
            {
                   
                if (!String.IsNullOrEmpty(toHighlight))
                {
                    var matches = Regex.Split(text, String.Format("({0})", toHighlight), RegexOptions.IgnoreCase);

                    textblock.Inlines.Clear();

                    foreach (var subString in matches)
                    {
                        if (String.Compare(subString, toHighlight, true) == 0)
                        {
                            textblock.Inlines.Add(new Span(new Run(toHighlight)) { Background = SystemColors.HighlightBrush, Foreground = SystemColors.HighlightTextBrush });
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
            }
        }
    }
}
