using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;
using PCSC.Exceptions;
using PCSC.Monitoring;
using WindowsInput;
using WindowsInput.Native;

/// <summary>
/// 
/// </summary>
namespace Nmind.pcsc.reader {

    /// <summary>
    /// 
    /// </summary>
    public partial class FrmMain : Form, IDevicesMonitor, IReaderMonitor {

        /// <summary>
        /// 
        /// </summary>
        NFCService service;

        /// <summary>
        /// 
        /// </summary>
        protected InputSimulator simulator;

        /// <summary>
        /// 
        /// </summary>
        protected IniData configuration;

        /// <summary>
        /// 
        /// </summary>
        protected bool stateIsClosing = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool stateIsReading = false;

        /// <summary>
        /// 
        /// </summary>
        public FrmMain() {
            InitializeComponent();

            notTray.Text = this.Text;
            service = new NFCService();
            simulator = new InputSimulator();

            rtHistorique.BackColor = Color.FromArgb(28, 28, 28);
            this.Text = Application.ProductName + " - " + Application.ProductVersion;

            cbRunWindowsStart.Checked = Helper.IsRegisteredAtWindowsStartup();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        /// <param name="bold"></param>
        /// <param name="color"></param>
        protected void LogHistory(String line, bool bold, Color color) {

            this.Invoke((MethodInvoker)delegate {

                rtHistorique.SelectionStart = rtHistorique.Text.Length;
                rtHistorique.SelectionLength = 0;
                rtHistorique.SelectionColor = color;

                if (bold) {
                    rtHistorique.SelectionFont = new Font(rtHistorique.Font, FontStyle.Bold);
                }

                rtHistorique.AppendText(line);
                rtHistorique.SelectionStart = rtHistorique.Text.Length;
                rtHistorique.ScrollToCaret();

            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected void LogHistoryLn(String line) {
            LogHistory(line + "\r\n", false, rtHistorique.ForeColor);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected void WarnHistoryLn(String line) {
            LogHistory(line + "\r\n", false, Color.DarkOrange);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected void ErrorHistoryLn(String line) {
            LogHistory(line + "\r\n", false, Color.IndianRed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected void SuccessHistoryLn(String line) {
            LogHistory(line + "\r\n", false, Color.DarkGreen);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected void InfoHistoryLn(String line) {
            LogHistory(line + "\r\n", false, Color.DeepSkyBlue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="line"></param>
        protected void DebugHistoryLn(String line) {
            LogHistory(line + "\r\n", false, Color.DimGray);
        }

        /// <summary>
        /// 
        /// </summary>
        protected void DefaultConfiguration() {

            if (configuration == null) {
                configuration = new IniData();
            }

            if (!configuration.Sections.ContainsSection("NFCReader")) {
                configuration.Sections.AddSection("NFCReader");
            }

            if (!configuration["NFCReader"].ContainsKey("device")) {
                configuration["NFCReader"].AddKey("device");
            }

            if (!configuration["NFCReader"].ContainsKey("validation")) {
                configuration["NFCReader"].AddKey("validation");
            }
            configuration["NFCReader"]["device"] = "";
            configuration["NFCReader"]["validation"] = "NONE";

        }

        /// <summary>
        /// 
        /// </summary>
        protected void ReadConfiguration() {

            DefaultConfiguration();

            var parser = new FileIniDataParser();
            if (File.Exists("config.ini")) {

                try {
                    configuration = parser.ReadFile("config.ini");
                } finally {

                }
            } 

        }

        /// <summary>
        /// 
        /// </summary>²
        protected void SaveConfiguration() {

            if(cbReaders.SelectedIndex > -1) {
                configuration["NFCReader"]["device"] = cbReaders.Items[cbReaders.SelectedIndex].ToString();
            } else {
                configuration["NFCReader"]["device"] = "";
            }

            if (raValidationEntree.Checked) {
                configuration["NFCReader"]["validation"] = "RETURN";
            } else if (raValidationTabulation.Checked) {
                configuration["NFCReader"]["validation"] = "TAB";
            } else {
                configuration["NFCReader"]["validation"] = "NONE";
            }

            var parser = new FileIniDataParser();
            parser.WriteFile("config.ini", configuration);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        protected void SimulateKeyboardInput(string tag) {

            simulator.Keyboard.TextEntry(tag);
            simulator.Keyboard.Sleep(100);

            if (raValidationEntree.Checked) {
                simulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);

            } else if (raValidationTabulation.Checked) {
                simulator.Keyboard.KeyPress(VirtualKeyCode.TAB);

            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected void UIHydrate() {

            cbReaders.Items.Clear();
            cbReaders.ResetText();

            foreach (var reader in service.ListReaders()) {
                DebugHistoryLn($"Found : {reader}");
                cbReaders.Items.Add(reader);
            }

            Console.WriteLine(configuration["NFCReader"]["device"]);
            int index = cbReaders.FindStringExact(configuration["NFCReader"]["device"]);
            if (index > -1) {
                cbReaders.SelectedIndex = index;
            }

            if (configuration["NFCReader"]["validation"].Equals("RETURN")) {
                raValidationEntree.Checked = true;
            } else if (configuration["NFCReader"]["validation"].Equals("TAB")) {
                raValidationTabulation.Checked = true;
            } else {
                raValidationAucune.Checked = true;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        protected void InitializeService() {
            service.AddDevicesMonitor(this);
            ReadConfiguration();
            UIHydrate();
            UIEnabled();
        }

        /// <summary>
        /// 
        /// </summary>
        protected bool StartMonitoring() {

            if (cbReaders.Items.Count == 0) {

                return false;
            }

            if (cbReaders.SelectedIndex < 0) {

                return false;
            }

            if (cbReaders.SelectedIndex > cbReaders.Items.Count) {

                return false;
            }

            if (service.StartReaderMonitor(cbReaders.Items[cbReaders.SelectedIndex].ToString(), this)) {
                UIDisabled();
                stateIsReading = true;
                return true;
            } else {
                stateIsReading = false;
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void StopMonitoring() {
            service.StopReaderMonitor();
            UIEnabled();
            stateIsReading = false;
        }

        /// <summary>
        /// 
        /// </summary>
        protected void TryMonitoring(bool tray) {
            // If wen can start monitoring with saved configuration
            if (StartMonitoring()) {

                SaveConfiguration();
                if (tray) {
                    UITray();
                }
             
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        /// <returns></returns>
        protected string ReadCardUid(string readerName) {
            NFCReader reader = new NFCReader();
            reader.Connect(readerName);
            string tag = BitConverter.ToString(reader.GetUID()).Replace("-", string.Empty);
            reader.Disconnect();
            return tag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        /// <returns></returns>
        protected void SendCardUid(string readerName) {
            string tag = ReadCardUid(readerName);
            SimulateKeyboardInput(tag);
            InfoHistoryLn($"Card: {tag}");
        }

        /// <summary>
        /// 
        /// </summary>
        protected void UIDisabled() {

            btnPause.Enabled = true;
            btnPlay.Enabled = false;
            btnTray.Enabled = true;
            cbReaders.Enabled = false;
            cbRunWindowsStart.Enabled = false;
            gpValidation.Enabled = false;
            rtHistorique.Focus();

        }

        /// <summary>
        /// 
        /// </summary>
        protected void UIEnabled() {

            btnPause.Enabled = false;
            btnPlay.Enabled = true;
            btnTray.Enabled = false;
            cbReaders.Enabled = true;
            cbRunWindowsStart.Enabled = true;
            gpValidation.Enabled = true;

        }

        /// <summary>
        /// 
        /// </summary>
        protected void UITray() {

           Hide();
           notTray.Visible = true;

        }

        /// <summary>
        /// 
        /// </summary>
        protected void UIUntray() {

            Show();
            notTray.Visible = true;

        }




        #region DevicesMonitor events

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        void IDevicesMonitor.OnAttachedReader(string readerName) {
            WarnHistoryLn($"Attached: {readerName}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readersName"></param>
        void IDevicesMonitor.OnAttachedReaders(IEnumerable<string> readersName) {

            this.Invoke((MethodInvoker)delegate {
                StopMonitoring();
                UIUntray();
                UIHydrate();
                TryMonitoring(true);
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        void IDevicesMonitor.OnDetachedReader(string readerName) {
            WarnHistoryLn($"Detached: {readerName}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readersName"></param>
        void IDevicesMonitor.OnDetachedReaders(IEnumerable<string> readersName) {

            this.Invoke((MethodInvoker)delegate {
                StopMonitoring();
                UIUntray();
                UIHydrate();
                TryMonitoring(true);
            });

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readerName"></param>
        void IDevicesMonitor.OnInitialized(string readerName) {
            LogHistoryLn($"Connected: {readerName}");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="readersName"></param>
        void IDevicesMonitor.OnInitialized(IEnumerable<string> readersName) {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        void IDevicesMonitor.OnMonitorException(Exception e) {
            ErrorHistoryLn($"Exception: {e}");
        }

        #endregion





        #region ReaderMonitor events

        void IReaderMonitor.OnCardInserted(CardStatusEventArgs args) {
            SendCardUid(args.ReaderName);
        }

        void IReaderMonitor.OnCardRemoved(CardStatusEventArgs args) {

        }

        void IReaderMonitor.OnInitialized(CardStatusEventArgs args) {

        }

        void IReaderMonitor.OnStatusChanged(StatusChangeEventArgs args) {

        }

        void IReaderMonitor.OnMonitorException(PCSCException ex) {

        }

        #endregion





        #region UI Events

        private void FrmMain_Load(object sender, EventArgs e) {
            InitializeService();
        }

        private void FrmMain_Shown(object sender, EventArgs e) {
            TryMonitoring(true);
        }

        private void btnPlay_Click(object sender, EventArgs e) {
            TryMonitoring(false);
        }

        private void btnPause_Click(object sender, EventArgs e) {
            StopMonitoring();
        }

        private void btTray_Click(object sender, EventArgs e) {
            UITray();
        }

        private void notTray_Click(object sender, EventArgs e) {
            UIUntray();
        }

        private void FrmMain_Activated(object sender, EventArgs e) {
            Opacity = 1;
        }

        private void FrmMain_Deactivate(object sender, EventArgs e) {
            if (!stateIsClosing) {
                Opacity = 0.5;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e) {
            stateIsClosing = true;
        }

        private void Control_KeyboardEventTrap(object sender, KeyEventArgs e) {
            e.Handled = true;
        }

        private void Control_KeyboardEventTrap(object sender, KeyPressEventArgs e) {
            e.Handled = true;
        }

        private void cbRunWindowsStart_CheckedChanged(object sender, EventArgs e) {
            if (((CheckBox)sender).Checked) {
                Helper.RegisterAtWindowsStartup();
            } else {
                Helper.UnregisterAtWindowsStartup();
            }
        }

        #endregion
    }
}
