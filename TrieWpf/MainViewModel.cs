using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using TrieLib;

namespace TrieWpf
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TrieTreeItem> _trieTree;

        public ObservableCollection<TrieTreeItem> TrieTree
        {
            get { return _trieTree; }
            set { 
                _trieTree = value;
                OnPropertyChanged();
            }
        }

        private string _textFilePath;
        public string TextFilePath
        {
            get => _textFilePath;
            set
            {
                _textFilePath = value;
                OnPropertyChanged();
            }
        }

        private string _text = "Sample Text";

        public string Text
        {
            get { return _text; }
            set { 
                _text = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<FoundListItem> _foundListItems = new ObservableCollection<FoundListItem>()
        {
            //new FoundListItem { Name = "Dokumentname1", FileName = "Dateiname1.pdf", FilePath = @"e:\temp\xxx1.pdf" },
            //new FoundListItem { Name = "Dokumentname2", FileName = "Dateiname2.pdf", FilePath = @"e:\temp\xxx2.pdf" },
            //new FoundListItem { Name = "Dokumentname3", FileName = "Dateiname3.pdf", FilePath = @"e:\temp\xxx3.pdf" },
        };
        public ObservableCollection<FoundListItem> FoundListItems
        {
            get => _foundListItems;
            set
            {
                _foundListItems = value;
                OnPropertyChanged();
            }
        }

        private string _outputText = "";
        public string OutputText
        {
            get => _outputText;
            set
            {
                _outputText = value;
                OnPropertyChanged();
            }
        }

        private string UserAppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private string ApplicationName => Assembly.GetExecutingAssembly().GetName().Name;

        public ObservableCollection<MRUMenuItem> MRUMenuItems { get; set; } = new ObservableCollection<MRUMenuItem>();

        public MainViewModel()
        {
            InitCommands();
        }

        private void InitCommands()
        {
            ÖffnenCommand = new CommandBase(ÖffnenExecute, ÖffnenCanExecute);
            ZuletztGeöffnetCommand = new CommandBase(ZuletztGeöffnetExecute, ZuletztGeöffnetCanExecute);
            SpeichernCommand = new CommandBase(SpeichernExecute, SpeichernCanExecute);
            VerlassenCommand = new CommandBase(VerlassenExecute, VerlassenCanExecute);
            MRUOpenCommand = new CommandBase(MRUOpenExecute, MRUOpenCanExecute);
            EmbeddedDocumentMouseDoubleClickCommand = new CommandBase(FoundItemMouseDoubleClickExecute, FoundItemMouseDoubleClickCanExecute);
            TrieTreeSelectedItemChangedCommand = new RelayCommand<TrieTreeItem>(SelectTrieTreeItemChangedExecute, SelectTrieTreeItemChangedCanExecute);
            MRUMenuItems = LoadMruFiles();
        }

        private void SelectTrieTreeItemChangedExecute(TrieTreeItem item)
        {
            MessageBox.Show(item.FullText);
        }

        private bool SelectTrieTreeItemChangedCanExecute(TrieTreeItem item)
        {
            return item.IsLeaf;
        }

        MRUMenuItem NewMRUMenuItemFromFileName(string fileName)
        {
            return new MRUMenuItem() { FileName = fileName, OpenCommand = MRUOpenCommand, OpenParameter = fileName, ShortName = Path.GetFileName(fileName) };
        }
        private ObservableCollection<MRUMenuItem> LoadMruFiles()
        {
            var mrufiles = new ObservableCollection<MRUMenuItem>();
            var appDataPath = UserAppDataPath;
            var myAppDataPath = Path.Combine(appDataPath, ApplicationName);
            var mruFilePath = Path.Combine(myAppDataPath, "MRU.txt");

            if (File.Exists(mruFilePath))
            {
                foreach (var file in File.ReadAllLines(mruFilePath))
                {
                    mrufiles.Add(NewMRUMenuItemFromFileName(file));
                }
            }
            return mrufiles;
        }
        private void SaveMruFiles(ObservableCollection<MRUMenuItem> mrufiles)
        {
            var appDataPath = UserAppDataPath;
            var myAppDataPath = Path.Combine(appDataPath, ApplicationName);
            if (!Directory.Exists(myAppDataPath)) Directory.CreateDirectory(myAppDataPath);
            var mruFilePath = Path.Combine(myAppDataPath, "MRU.txt");

            File.WriteAllLines(mruFilePath, mrufiles.Select(x => x.FileName));
        }
        private void AddFilePathToMruList(string dateiname)
        {
            var item = MRUMenuItems.FirstOrDefault(x => x.FileName == dateiname);
            if (item != null) MRUMenuItems.Remove(item);

            MRUMenuItems.Insert(0, NewMRUMenuItemFromFileName(dateiname));
            if (MRUMenuItems.Count() > 10)
            {
                MRUMenuItems.RemoveAt(MRUMenuItems.Count() - 1);
            }

            SaveMruFiles(MRUMenuItems);
        }

        private bool VerlassenCanExecute(object arg)
        {
            return true;
        }

        private void VerlassenExecute(object obj)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private string CurrentDirectory { get; set; }

        void OpenFile(string fileName)
        {
            WriteOutput("");

            if (!File.Exists(fileName))
            {
                WriteOutput($"Datei {fileName} wurde nicht gefunden.");
                return;
            }
            try
            {
                using (new WaitCursor())
                {
                    TextFilePath = fileName;

                    Text = File.ReadAllText(TextFilePath);

                    UpdateTrieTreeFromFile(TextFilePath);

                    AddFilePathToMruList(TextFilePath);

                    OnDocumentChanged();
                }
            }
            catch (Exception ex)
            {
                WriteOutput(ex.Message, true);
            }
        }
        private void MRUOpenExecute(object obj)
        {
            OpenFile((string)obj);

        }
        private bool MRUOpenCanExecute(object arg) => true;

        private void ÖffnenExecute(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "txt Files|*.txt";
            openFileDialog.InitialDirectory = Directory.Exists(Path.GetDirectoryName(TextFilePath)) ? Path.GetDirectoryName(TextFilePath) : CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                OpenFile(openFileDialog.FileName);
            }
        }
        private bool ÖffnenCanExecute(object arg)
        {
            return true;
        }
        private void ZuletztGeöffnetExecute(object obj)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.CheckFileExists = true;
            openFileDialog.Filter = "txt Files|*.txt";
            openFileDialog.InitialDirectory = Directory.Exists(Path.GetDirectoryName(TextFilePath)) ? Path.GetDirectoryName(TextFilePath) : CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                OpenFile(openFileDialog.FileName);
            }
        }
        private bool ZuletztGeöffnetCanExecute(object arg)
        {
            return true;
        }

        private void SpeichernExecute(object obj)
        {
            if (TextFilePath == null || TextFilePath.Length == 0)
            {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.DefaultExt = ".txt";
                if (dlg.ShowDialog() ?? false)
                {
                    TextFilePath = dlg.FileName;
                }
                else
                {
                    return;
                }
            }
            // TODO
            // XmlEditor.Save(TextFilePath);
        }

        private bool SpeichernCanExecute(object arg)
        {
            return true;
        }

        private void FoundItemMouseDoubleClickExecute(object obj)
        {
            if (!(obj is string filename)) return;
        }
        private bool FoundItemMouseDoubleClickCanExecute(object arg)
        {
            return true;
        }

        private void OnDocumentChanged()
        {
            // UpdateTrieTree(Text);
        }



        private void UpdateTrieTreeFromFile(string textFile)
        {
            if (!File.Exists(textFile))
            {
                WriteOutput($"Datei {textFile} wurde nicht gefunden.");
                return;
            }
            using (var stream = File.Open(textFile, FileMode.Open))
            {
                var wordwiseReader = new WordWiseReader(stream);
                Trie trie = TrieFromWords(wordwiseReader);
                CreateTrieTree(trie);
            }
        }



        private void CreateTrieTree(Trie trie)
        {
            var rootItem = new TrieTreeItem(trie.Root, "");
            CreateChildren(trie.Root, rootItem, "");

            TrieTree = new ObservableCollection<TrieTreeItem>();
            TrieTree.Add(rootItem);
        }

        private void CreateChildren(TrieNode trieItem, TrieTreeItem item, string fullText)
        {
            if (trieItem.Children == null) return;
            if (item.Children == null) item.Children = new ObservableCollection<TrieTreeItem>();
            foreach(var trieChild in trieItem.Children)
            {
                var newText = fullText + trieChild.Text;
                var newItem = new TrieTreeItem(trieChild, newText);
                item.Children.Add(newItem);
                CreateChildren(trieChild, newItem, newText );
            }
        }

        private Trie TrieFromWords(IEnumerable<WordInfo> words)
        {
            var trie = new Trie();
            foreach (var wordInfo in words)
            {
                trie.InsertWord(wordInfo);
            }
            return trie;
        }


        private void WriteOutput(string text, bool append = false)
        {
            if (append && OutputText?.Length > 0)
            {
                OutputText = OutputText + Environment.NewLine + text;
            }
            else
            {
                OutputText = text;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;


        public ICommand ÖffnenCommand { get; private set; }
        public ICommand ZuletztGeöffnetCommand { get; private set; }
        public ICommand SpeichernCommand { get; private set; }
        public ICommand VerlassenCommand { get; private set; }
        public ICommand MRUOpenCommand { get; private set; }
        public ICommand EmbeddedDocumentMouseDoubleClickCommand { get; private set; }
        public ICommand TrieTreeSelectedItemChangedCommand { get; private set; }
    }
}
