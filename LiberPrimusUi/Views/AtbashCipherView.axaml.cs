using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LiberPrimusUi.ViewModels;

namespace LiberPrimusUi.Views;

public partial class AtbashCipherView : UserControl
{
    public AtbashCipherView()
    {
        InitializeComponent();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var viewModel = (AtbashCipherViewModel)DataContext;
        viewModel.SelectedEncoding = (string)e.AddedItems[0];
        
        switch (viewModel.SelectedEncoding)
        {
            case "Gematria":
                viewModel.Alphabet = "ᚠ,ᚢ,ᚦ,ᚩ,ᚱ,ᚳ,ᚷ,ᚹ,ᚻ,ᚾ,ᛁ,ᛂ,ᛇ,ᛈ,ᛉ,ᛋ,ᛏ,ᛒ,ᛖ,ᛗ,ᛚ,ᛝ,ᛟ,ᛞ,ᚪ,ᚫ,ᚣ,ᛡ,ᛠ";
                break;
            case "English":
                viewModel.Alphabet = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
                break;
            default:
                break;
        }
    }

    private void TextBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        int count = 0;
        var viewModel = (AtbashCipherViewModel)DataContext;
        foreach (var charToCalc in viewModel.StringToDecode)
        {
            if (char.IsLetter(charToCalc) || IsRune(charToCalc.ToString()))
            {
                count++;
            }
        }
        
        viewModel.CharCount = count.ToString();
    }
    
    private bool IsRune(string value)
    {
        bool retval = false;
        switch (value)
        {
            case "ᛝ":
            case "ᛟ":
            case "ᛇ":
            case "ᛡ":
            case "ᛠ":
            case "ᚫ":
            case "ᚦ":
            case "ᚠ":
            case "ᚢ":
            case "ᚩ":
            case "ᚱ":
            case "ᚳ":
            case "ᚷ":
            case "ᚹ":
            case "ᚻ":
            case "ᚾ":
            case "ᛁ":
            case "ᛄ":
            case "ᛈ":
            case "ᛉ":
            case "ᛋ":
            case "ᛏ":
            case "ᛒ":
            case "ᛖ":
            case "ᛗ":
            case "ᛚ":
            case "ᛞ":
            case "ᚪ":
            case "ᚣ":
                retval = true;
                break;

            default:
                retval = false;
                break;
        }

        return retval;
    }
}