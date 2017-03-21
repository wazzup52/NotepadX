using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace NotepadX
{
    class Notepad : TextBox
    {
        private bool changed = false;
        private string path = null;
        private TabItem tab;

        public string getPath()
        {
            return path;
        }

        /*Menu items functions*/
        public void New()
        {
            if (changed)
            {
                string save = SaveFirst();
                if (save != "Cancel")
                {
                    Clear();
                }
            }
            else
                Clear();
        }
        public void Open()
        {
            if (changed)
            {
                SaveFirst();
                ShowOpen();
            }
            else
            {
                ShowOpen();
            }
        }
        public void Save()
        {
            if (path == null)
            {
                ShowSave();
            }
            else
            {
                SaveFile(path);
            }
        }

        /*Dialogs for Menu*/
        public void ShowOpen()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text Files|*.txt|All|*.*";
            if (dialog.ShowDialog() == true)
            {
                path = dialog.FileName;
                ReadFile(path);
                SetTitle(dialog.SafeFileName);
                changed = false;
            }
        }
        public void ShowSave()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Text Document (.txt)|*.txt";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                path = dialog.FileName;
                SaveFile(path);
                SetTitle(dialog.SafeFileName);
            }
        }

        /*Helper functions */

        public string SaveFirst()
        {
            MessageBoxResult result = MessageBox.Show("Do you want to save your changes?", "Save File?", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                if (path != null)
                {
                    SaveFile(path);
                }
                else
                {
                    Save();
                }
            }
            else if (result == MessageBoxResult.Cancel)
            {
                return "Cancel";
            }
            return "Nothing";
        }

        private void ReadFile(string filepath)
        {
            StreamReader reader = new StreamReader(path);
            StringBuilder sb = new StringBuilder();
            string r = reader.ReadLine();
            while (r != null)
            {
                sb.Append(r);
                sb.Append(Environment.NewLine);
                r = reader.ReadLine();
            }
            reader.Close();
            this.Text = sb.ToString();
        }
        private void SaveFile(string filepath)
        {
            StreamWriter writer = new StreamWriter(path);
            writer.Write(this.Text);
            writer.Close();
            changed = false;
        }


        public void Reset()
        {
            path = null;
            changed = false;
            this.Text = "";
            ResetTitle();
        }

        private void SetTitle(string newTitle)
        {
            tab.Header= newTitle;
        }
        private void ResetTitle()
        {
            tab.Header = "Untitled";
        }

        public void setChange(bool k)
        {
            changed = k;
        }
        public bool isChanged()
        {
            return changed;
        }
    }

}
