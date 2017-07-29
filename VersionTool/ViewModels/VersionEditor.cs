using System.ComponentModel;
using System.Threading.Tasks;
using UWPVersioningToolkit.Models;
using Windows.UI.Core;

namespace UWPVersioningToolkit.ViewModels
{
    public class VersionEditor : INotifyPropertyChanged
    {
        public VersionEditor(VersionModel Model)
        {
            this.Model = Model;
            _New = Model.Version.New;
            _Fixed = Model.Version.Fixed;
            StoreSummary = Model.Version.StoreVersionSummary;
        }

        public void GenerateSummary()
        {
            if (!AutoGenerateSummary) return;
            StoreSummary = Clean(New);
            StoreSummary += "\r\rFixed:\r";
            StoreSummary += Clean(Fixed);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StoreSummary)));
        }

        private string Clean(string raw)
        {
            return raw?.Insert(0, "\r").Replace("\r\r", "\r").Replace("\r- ", "\r-").Remove(0, 1).TrimEnd();
        }

        public void Save()
        {
            Model.Version.New = New?.TrimEnd();
            Model.Version.Fixed = Fixed?.TrimEnd();
            Model.Version.StoreVersionSummary = StoreSummary?.TrimEnd();
            EditWaiter.TrySetResult(Model.Version);
        }

        public void Revert()
        {
            EditWaiter.TrySetResult(null);
        }

        private string _New;

        public string New
        {
            get { return _New; }
            set
            {
                _New = value;
                GenerateSummary();
            }
        }

        private string _Fixed;

        public string Fixed
        {
            get { return _Fixed; }
            set
            {
                _Fixed = value;
                GenerateSummary();
            }
        }

        public string StoreSummary { get; set; }

        public bool AutoGenerateSummary { get; set; }

        public VersionModel Model { get; }
        public TaskCompletionSource<VersionLog> EditWaiter { get; } = new TaskCompletionSource<VersionLog>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}