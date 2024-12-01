using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CommunityToolkit.Mvvm.ComponentModel;
using LiberPrimusUi.Models;

namespace LiberPrimusUi.ViewModels;

public partial class CreditsViewModel: ViewModelBase
{
    public CreditsViewModel()
    {
        Items = new ObservableCollection<CreditItemTemplate>(_templates);
    }
    
    public ObservableCollection<CreditItemTemplate> Items { get; }
    
    [ObservableProperty]
    private CreditItemTemplate? _selectedListItem;

    private readonly List<CreditItemTemplate> _templates =
    [
        new CreditItemTemplate("Words", "https://github.com/dwyl/english-words"),
        new CreditItemTemplate("Inspiration for sequence code", "https://github.com/TheAlgorithms/C-Sharp"),
        new CreditItemTemplate("File detection", "https://github.com/ghost1face/FileTypeInterrogator"),
        new CreditItemTemplate("Images from", "https://github.com/rtkd/iddqd"),
    ];

    partial void OnSelectedListItemChanged(CreditItemTemplate? value)
    {
        if (value.Link is not null)
        {
            try
            {
                Process.Start(value.Link);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo(value.Link) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", value.Link);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", value.Link);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}