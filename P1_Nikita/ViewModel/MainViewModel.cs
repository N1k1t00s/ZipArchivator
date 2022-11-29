using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using Aspose.Zip;
using Aspose.Zip.Saving;
using Microsoft.WindowsAPICodePack.Dialogs;
using P1_Nikita.Command;
using P1_Nikita.Model;

namespace P1_Nikita.ViewModel;

public class MainViewModel : ViewModelBase
{
    private ObservableCollection<FileInfo> _files = new();
    private FileInfo? _selectedFile;
    private string _selectedPath = @"C:\";
    private string _archiveName = "archive.zip";
    private ArchiveInfoViewModel? _archiveInfoViewModel;

    public ObservableCollection<FileInfo> Files
    {
        get => _files;
        set
        {
            _files = value;
            OnPropertyChanged();
        }
    }

    public FileInfo? SelectedFile
    {
        get => _selectedFile;
        set
        {
            _selectedFile = value;
            OnPropertyChanged();
        }
    }

    public IList? SelectedFiles { get; set; }

    public string SelectedPath
    {
        get => _selectedPath;
        set
        {
            _selectedPath = value;
            try
            {
                Files = new ObservableCollection<FileInfo>(new DirectoryInfo(value).GetFiles("*.*"));
            }
            catch (DirectoryNotFoundException e)
            {
                Files = new ObservableCollection<FileInfo>();
            }

            OnPropertyChanged();
        }
    }

    public string ArchiveName
    {
        get => _archiveName;
        set
        {
            _archiveName = value;
            OnPropertyChanged();
        }
    }

    public ArchiveInfoViewModel? ArchiveInfoViewModel
    {
        get => _archiveInfoViewModel;
        set
        {
            _archiveInfoViewModel = value;
            OnPropertyChanged();
        }
    }

    public ICommand SelectDirectoryDialog => new RelayCommand<string>(directory =>
    {
        using var dialog = new CommonOpenFileDialog("Select directory");
        dialog.InitialDirectory = directory;
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
        {
            SelectedPath = dialog.FileName;
        }
    });

    public ICommand OpenSelectedDirectory => new RelayCommand<string>(directory =>
    {
        if (!Directory.Exists(directory)) return;
        Process.Start("explorer.exe", directory);
    });

    public ICommand Archive => new AsyncRelayCommand<IList>(files =>
    {
        var path = $@"C:\P1_Archive\{(ArchiveName.EndsWith(".zip") ? ArchiveName : ArchiveName + ".zip")}";
        var directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
            if (directory != null)
                Directory.CreateDirectory(directory);
        if (File.Exists(path))
        {
            var name = Path.GetFileNameWithoutExtension(path);
            path = $@"{directory}\{name}_{DateTime.Now.Ticks}{Path.GetExtension(path)}";
        }
        using var zipFile =
            File.Open(path,
                FileMode.Create);
        using var archive = new Archive();
        foreach (var info in files)
            if (info is FileInfo fileInfo)
                archive.CreateEntry(fileInfo.Name, fileInfo);
        archive.Save(zipFile, new ArchiveSaveOptions { Encoding = Encoding.UTF8 });
        var fileCount = archive.Entries.Count;
        var totalSize = archive.Entries.Sum(entry => (long) entry.UncompressedSize);
        var compressedSize = archive.Entries.Sum(entry => (long)entry.CompressedSize);
        var archiveInfo = new ArchiveInfo
        {
            FileCount = fileCount, TotalSize = totalSize, SizeInArchive = compressedSize
        };
        ArchiveInfoViewModel = new ArchiveInfoViewModel(archiveInfo);
    }, files => files?.Count > 0);

    public ICommand UnArchive => new AsyncRelayCommand<FileInfo>(info =>
    {
        using var dialog = new CommonOpenFileDialog("Select directory");
        dialog.InitialDirectory = SelectedPath;
        dialog.IsFolderPicker = true;
        if (dialog.ShowDialog() != CommonFileDialogResult.Ok) return;
        var extractDirectory = dialog.FileName;
        using var archive = new Archive(info.FullName, new ArchiveLoadOptions() {Encoding = Encoding.UTF8});
        archive.ExtractToDirectory(extractDirectory);
    }, info => info != null && info.Name.EndsWith(".zip"));

    public ICommand Reset => new RelayCommand(_ =>
    {
        SelectedPath = @"C:\";
        ArchiveName = "archive.zip";
        ArchiveInfoViewModel = null;
    });

    public MainViewModel()
    {
        Files = new ObservableCollection<FileInfo>(new DirectoryInfo($@"{SelectedPath}").GetFiles("*.*"));
    }
}