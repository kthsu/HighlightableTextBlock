using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace HighlightableTextblock
{
    public class HighlightableTextBlock : TextBlock
    {
        // Using a DependencyProperty as the backing store for HighlightText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighlightTextProperty =
            DependencyProperty.Register("HighlightText", typeof(string), typeof(HighlightableTextBlock), new PropertyMetadata(OnHighlightTextChanged));

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

                HighlightParts(textblock, text, highlightText);
            }
        }

        private static void HighlightParts(TextBlock textblock, string input, string toHighlight)
        {
            if (!String.IsNullOrEmpty(input))
            {
                if (!String.IsNullOrEmpty(toHighlight))
                {
                    var matches = Regex.Split(input, String.Format("({0})", toHighlight), RegexOptions.IgnoreCase);

                    textblock.Inlines.Clear();

                    foreach (var subString in matches)
                    {
                        if (String.Compare(subString, toHighlight, true) == 0)
                        {
                            textblock.Inlines.Add(new Span(new Run(toHighlight) { Background = SystemColors.HighlightBrush, Foreground = SystemColors.HighlightTextBrush }));
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
                    textblock.Text = input;
                }
            }
        }
    }
}
