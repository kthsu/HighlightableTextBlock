# HighlightableTextBlock - Highlighting add-on for WPF TextBlock

## Quick Start
### XAML

After installing HighlightableTextBlock:

* Open up the xaml file containing the TextBlock you wish to add highlighting. 
* Add this namespace reference to the XAML: 
  
  xmlns:controls="clr-namespace:HighlightableTextBlock;assembly=HighlightableTextBlock"
* Locate the TextBlock declaration in the XAML. 
* Add this attribute:

  controls:HighlightableTextBlock.HightlightText="{Binding SearchText}" 
* Ta-da! Now you have highlightable TextBlocks in your application!
* Customization

  * Highlight color - controls:HighlightableTextBlock.HighlightBrush="Yellow" 
  * Highlight text color - controls:HighlightableTextBlock.HighlightTextBrush="Red"
  * Bold - controls:HighlightableTextBlock.Bold="True"
  * Italic - controls:HighlightableTextBlock.Italic="True"
  * Underline - controls:HighlightableTextBlock.Underline="True"
