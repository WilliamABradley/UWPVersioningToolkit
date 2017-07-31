using System.ComponentModel;
using System.Threading.Tasks;
using UWPVersioningToolkit.Models;

namespace UWPVersioningToolkit.ViewModels
{
    /// <summary>
    /// Viewmodel to handle Editing, Saving and Cleaning of Version Logs.
    /// </summary>
    public class VersionEditor : INotifyPropertyChanged
    {
        public VersionEditor(VersionModel Model)
        {
            this.Model = Model;
            _New = Model.Version.New;
            _Fixed = Model.Version.Fixed;
            StoreSummary = Model.Version.StoreVersionSummary;
        }

        /// <summary>
        /// Generates the Store Summary if <see cref="AutoGenerateSummary"/> is True
        /// </summary>
        public void GenerateSummary()
        {
            if (!AutoGenerateSummary) return;
            StoreSummary = Clean(New);
            if (!string.IsNullOrWhiteSpace(Fixed))
            {
                StoreSummary += "\r\rFixed:\r";
                StoreSummary += Clean(Fixed);
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StoreSummary)));
        }

        /// <summary>
        /// Cleans the Markdown String for Plain Text Consumption
        /// </summary>
        private string Clean(string raw)
        {
            return raw?.Insert(0, "\r").Replace("\r\r", "\r").Replace("\r- ", "\r-").Remove(0, 1).TrimEnd();
        }

        /// <summary>
        /// Saves the Changes made in the Editor to the VersionModel.
        /// </summary>
        public void Save()
        {
            Model.Version.New = New?.TrimEnd();
            Model.Version.Fixed = Fixed?.TrimEnd();
            Model.Version.StoreVersionSummary = StoreSummary?.TrimEnd();
            EditWaiter.TrySetResult(Model.Version);
        }

        /// <summary>
        /// Clears all changes made in the Editor.
        /// </summary>
        public void Revert()
        {
            EditWaiter.TrySetResult(null);
        }

        private string _New;

        /// <summary>
        /// Binding and Update for Whats New.
        /// </summary>
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

        /// <summary>
        /// Binding and Update for Fixed.
        /// </summary>
        public string Fixed
        {
            get { return _Fixed; }
            set
            {
                _Fixed = value;
                GenerateSummary();
            }
        }

        /// <summary>
        /// Binding and Update for Store Summary.
        /// </summary>
        public string StoreSummary { get; set; }

        /// <summary>
        /// Toggle for Allowing Automatic Store Summary Generation.
        /// </summary>
        public bool AutoGenerateSummary { get; set; }

        /// <summary>
        /// Model currently being Edited.
        /// </summary>
        public VersionModel Model { get; }

        /// <summary>
        /// TaskCompletionSource for Telling the UI that the Edit has Completed.
        /// </summary>
        public TaskCompletionSource<VersionLog> EditWaiter { get; } = new TaskCompletionSource<VersionLog>();

        public event PropertyChangedEventHandler PropertyChanged;
    }
}