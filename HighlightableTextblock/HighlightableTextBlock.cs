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
        #region HighlightTextBrush

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

        #endregion

        #region HighlightBrush

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

        #endregion

        #region HighlightText

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
            DependencyProperty.RegisterAttached("HightlightText", typeof(string), typeof(HighlightableTextBlock), new PropertyMetadata(string.Empty, OnHighlightTextChanged));

        private static void OnHighlightTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Highlight(d as TextBlock, e.NewValue as string);
        }

        #endregion

        #region InternalText

        protected static string GetInternalText(DependencyObject obj)
        {
            return (string)obj.GetValue(InternalTextProperty);
        }

        protected static void SetInternalText(DependencyObject obj, string value)
        {
            obj.SetValue(InternalTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for InternalText.  This enables animation, styling, binding, etc...
        protected static readonly DependencyProperty InternalTextProperty =
            DependencyProperty.RegisterAttached("InternalText", typeof(string),
                typeof(HighlightableTextBlock), new PropertyMetadata(string.Empty, OnInternalTextChanged));

        private static void OnInternalTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textblock = d as TextBlock;

            if (textblock != null)
            {
                textblock.Text = e.NewValue as string;
                Highlight(textblock, GetHightlightText(textblock));
            }
        }

        #endregion

        #region  IsBusy 

        private static bool GetIsBusy(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsBusyProperty);
        }

        private static void SetIsBusy(DependencyObject obj, bool value)
        {
            obj.SetValue(IsBusyProperty, value);
        }

        // Using a DependencyProperty as the backing store for IsBusy.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty IsBusyProperty =
            DependencyProperty.RegisterAttached("IsBusy", typeof(bool), typeof(HighlightableTextBlock), new PropertyMetadata(false));

        #endregion

        #region Methods

        private static void Highlight(TextBlock textblock, string toHighlight)
        {
            if (textblock == null) return;

            string text = textblock.Text;

            if (textblock.GetBindingExpression(HighlightableTextBlock.InternalTextProperty) == null)
            {
                var textBinding = textblock.GetBindingExpression(TextBlock.TextProperty);

                if (textBinding != null)
                {
                    textblock.SetBinding(HighlightableTextBlock.InternalTextProperty, textBinding.ParentBindingBase);

                    var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));

                    propertyDescriptor.RemoveValueChanged(textblock, OnTextChanged);
                }
                else
                {
                    var propertyDescriptor = DependencyPropertyDescriptor.FromProperty(TextBlock.TextProperty, typeof(TextBlock));

                    propertyDescriptor.AddValueChanged(textblock, OnTextChanged);
                }
            }

            if (!String.IsNullOrEmpty(text))
            {
                SetIsBusy(textblock, true);

                if (!String.IsNullOrEmpty(toHighlight))
                {
                    var matches = Regex.Split(text, String.Format("({0})", Regex.Escape(toHighlight)), RegexOptions.IgnoreCase);

                    textblock.Inlines.Clear();

                    var highlightBrush = GetHighlightBrush(textblock);
                    var highlightTextBrush = GetHighlightTextBrush(textblock);

                    foreach (var subString in matches)
                    {
                        if (String.Compare(subString, toHighlight, true) == 0)
                        {
                            textblock.Inlines.Add(new Span(
                                new Run(subString))
                            {
                                Background = highlightBrush,
                                Foreground = highlightTextBrush
                            });
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
                    textblock.SetCurrentValue(TextBlock.TextProperty, text);
                }

                SetIsBusy(textblock, false);
            }
        }

        private static void OnTextChanged(object sender, EventArgs e)
        {
            var textBlock = sender as TextBlock;

            if (textBlock != null &&
                !GetIsBusy(textBlock))
            {
                Highlight(textBlock, GetHightlightText(textBlock));
            }
        }

        #endregion
    }
}
