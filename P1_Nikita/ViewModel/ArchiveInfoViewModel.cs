using P1_Nikita.Model;

namespace P1_Nikita.ViewModel;

public class ArchiveInfoViewModel : ViewModelBase
{
    private ArchiveInfo _archiveInfo;

    public ArchiveInfo ArchiveInfo
    {
        get => _archiveInfo;
        set
        {
            _archiveInfo = value;
            OnPropertyChanged();
        }
    }

    public ArchiveInfoViewModel(ArchiveInfo archiveInfo)
    {
        _archiveInfo = archiveInfo;
    }
}