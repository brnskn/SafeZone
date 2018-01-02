using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using BrightIdeasSoftware;
using System.Threading;
using StartUpLibrary;

namespace SafeZone
{
    public partial class SafeZone : Form
    {
        int TogMove;
        int MValX;
        int MValY;
        List<SafeFile> list = new List<SafeFile>();
        ZipManager zipManager = new ZipManager();
        Form3 form3 = new Form3();
        public SafeZone()
        {
            StartUp startUp = new StartUp("SafeZone");
            startUp.RegisterOrRemove(StartUp.Action.Register);
            if (!SingleInstance())
            {
                Exit();
            }
            FileDB.Init();
            new FileWatcher(zipManager);
            var thread = new Thread(KeepingAlive);
            thread.Start();
            InitializeComponent();
            if (UserDB.CurrentUser == null)
            {
                form3.ShowDialog();
            }
            updateFileList();
            zipManager.integrityConrol();
        }
        private void updateFileList()
        {
            list = zipManager.list;
            fileListView.SetObjects(list);
        }
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            TogMove = 1;
            MValX = e.X;
            MValY = e.Y;
        }

        private void panel4_MouseUp(object sender, MouseEventArgs e)
        {
            TogMove = 0;
        }

        private void panel4_MouseMove(object sender, MouseEventArgs e)
        {
            if (TogMove == 1)
            {
                SetDesktopLocation(MousePosition.X - MValX, MousePosition.Y - MValY);
            }
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            Hide();
            UserDB.CurrentUser = null;
            notifyIcon1.Visible = false;
            notifyIcon1.Visible = true;
        }

        private void minimizeButton_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void fileListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void fileListView_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            zipManager.add(files);
            updateFileList();
        }
        private void createIsoButton_Click(object sender, EventArgs e)
        {
            zipManager.createISO();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if(UserDB.CurrentUser == null)
            {
                form3.ShowDialog();
            }
            Show();
        }

        private void SafeZone_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel= true;
            Hide();
            UserDB.CurrentUser = null;
        }

        private void çıkışToolStripMenuItem_Click(object sender, EventArgs e)
        {
            form3.ShowDialog();
            if (zipManager.isSafe)
            {
                DialogResult dialogResult = MessageBox.Show("Korumayı durdurmak istediğinize emin misiniz? Dosyalarınız artık korunmayacak!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    zipManager.isSafe = false;
                    çıkışToolStripMenuItem.Text = "Korumayı Başlat";
                }
            }
            else
            {
                zipManager.isSafe = true;
                çıkışToolStripMenuItem.Text = "Korumayı Duraklat";
            }
        }

        private void fileListView_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Delete == e.KeyCode)
            {
                DialogResult dialogResult = MessageBox.Show("Dosyaları silmek istediğinize emin misiniz? Bu işlemin geri dönüşü yoktur!", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    zipManager.delete((ObjectListView)sender);
                    updateFileList();
                }
            }
        }
        private static void KeepingAlive()
        {
            while (true)
            {
                if (_exiting)
                    return;

                if (_processId == 0)
                {
                    var kamikazeProcess = Process.Start(KeepAlive, string.Concat("launchselfandexit ", Process.GetCurrentProcess().Id));
                    if (kamikazeProcess == null)
                        return;

                    kamikazeProcess.WaitForExit();
                    _keepAliveProcess = Process.GetProcessById(kamikazeProcess.ExitCode);
                }
                else
                {
                    _keepAliveProcess = Process.GetProcessById(_processId);
                    _processId = 0;
                }

                _keepAliveProcess.WaitForExit();
            }
        }

        private static bool SingleInstance()
        {
            bool createdNew;
            _instanceMutex = new Mutex(true, @"Local\4A31488B-F86F-4970-AF38-B45761F9F060", out createdNew);
            if (createdNew) return true;
            Debug.WriteLine("Application already launched. Shutting down.");
            _instanceMutex = null;
            return false;
        }

        private static void ReleaseSingleInstance()
        {
            if (_instanceMutex == null)
                return;
            _instanceMutex.ReleaseMutex();
            _instanceMutex.Close();
            _instanceMutex = null;
        }

        private void Exit()
        {
            _exiting = true;
            ReleaseSingleInstance();
            _keepAliveProcess.Kill();
            Application.Exit();
        }
        private const string KeepAlive = "KeepAlive.exe";
        private static Process _keepAliveProcess;
        private static Mutex _instanceMutex;
        private static bool _exiting;
        private static int _processId = 0;

        private void logButton_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.ShowDialog();
        }

        private void SafeZone_FormClosed(object sender, FormClosedEventArgs e)
        {
            notifyIcon1.Visible = false;
        }
    }
}
