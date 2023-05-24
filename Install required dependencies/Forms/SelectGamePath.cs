using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using PrepareStella.Properties;

namespace PrepareStella.Forms
{
    public partial class SelectGamePath : Form
    {
        public SelectGamePath()
        {
            InitializeComponent();
        }

        private void SelectPath_Load(object sender, EventArgs e)
        {
            if (File.Exists(Program.GamePathSfn))
            {
                string path = File.ReadAllText(Program.GamePathSfn).Trim();
                label4.Text += path;
            }
            else
            {
                label4.Text += $"{Program.GameGenshinImpact} and\n{Program.GameYuanShen}";
            }
        }

        private void Browse_Click(object sender, EventArgs e)
        {
            string filePath = string.Empty;

            Thread t = new Thread(() =>
            {
                using (OpenFileDialog dialog = new OpenFileDialog())
                {
                    dialog.InitialDirectory = Program.ProgramFiles;
                    dialog.Filter = "Process (*.exe)|*.exe";
                    dialog.FilterIndex = 0;
                    dialog.RestoreDirectory = true;

                    if (dialog.ShowDialog() != DialogResult.OK) return;
                    string selectedFile = dialog.FileName;

                    if (!selectedFile.Contains("GenshinImpact.exe") && !selectedFile.Contains("YuanShen.exe"))
                    {
                        MessageBox.Show("Please select the game exe.\n\nGenshinImpact.exe for OS version.\nYuanShen.exe for CN version.", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    string directory = Path.GetDirectoryName(selectedFile);
                    if (!File.Exists(Path.Combine(directory, "UnityPlayer.dll")))
                    {
                        MessageBox.Show("That's not the right place.", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    filePath = selectedFile;
                }
            });

            t.SetApartmentState(ApartmentState.STA);
            t.Start();
            t.Join();

            if (string.IsNullOrEmpty(filePath)) return;
            comboBox1.Items.Clear();
            comboBox1.Items.Add(filePath);
            comboBox1.SelectedIndex = 0;
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            string selectedFile = comboBox1.GetItemText(comboBox1.SelectedItem);
            if (!selectedFile.Contains("GenshinImpact.exe") && !selectedFile.Contains("YuanShen.exe"))
            {
                MessageBox.Show(
                    "We can't save your settings. Please select the game exe.\n\nGenshinImpact.exe for OS version.\nYuanShen.exe for CN version.",
                    Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string directory = Path.GetDirectoryName(selectedFile);
            if (!File.Exists($@"{directory}\UnityPlayer.dll"))
            {
                MessageBox.Show("That's not the right place. UnityPlayer.dll file was not found.", Program.AppName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            File.WriteAllText(Path.Combine(Program.AppData, "game-version.sfn"), Path.GetFileName(selectedFile) == "GenshinImpact.exe" ? "1" : "2");

            Program.GameExeGlobal = selectedFile;
            Program.GameDirGlobal = Path.GetDirectoryName(Path.GetDirectoryName(selectedFile));
            File.WriteAllText(Program.GamePathSfn, Program.GameDirGlobal);

            Console.WriteLine(Program.GameDirGlobal);

            Close();
        }

        private void Help_Click(object sender, EventArgs e)
        {
            new Help { Icon = Resources.icon }.Show();
        }
    }
}
