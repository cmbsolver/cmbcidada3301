using System.Collections.ObjectModel;

namespace LiberPrimusUi.ViewModels;

public partial class CreditsViewModel: ViewModelBase
{
    public CreditsViewModel()
    {
        Credits.Add("Words - https://github.com/dwyl/english-words");
        Credits.Add("Inspiration for sequence code - https://github.com/TheAlgorithms/C-Sharp");
        Credits.Add("File detection - https://github.com/ghost1face/FileTypeInterrogator");
        Credits.Add("Images from - https://github.com/rtkd/iddqd");
    }

    public ObservableCollection<string> Credits { get; } = new ObservableCollection<string>();
}