
namespace Nmind.pcsc.reader {
    partial class FrmMain {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMain));
            this.gpValidation = new System.Windows.Forms.GroupBox();
            this.raValidationTabulation = new System.Windows.Forms.RadioButton();
            this.raValidationEntree = new System.Windows.Forms.RadioButton();
            this.raValidationAucune = new System.Windows.Forms.RadioButton();
            this.notTray = new System.Windows.Forms.NotifyIcon(this.components);
            this.cbReaders = new System.Windows.Forms.ComboBox();
            this.rtHistorique = new System.Windows.Forms.RichTextBox();
            this.btnTray = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.cbRunWindowsStart = new System.Windows.Forms.CheckBox();
            this.gpValidation.SuspendLayout();
            this.SuspendLayout();
            // 
            // gpValidation
            // 
            this.gpValidation.AutoSize = true;
            this.gpValidation.Controls.Add(this.raValidationTabulation);
            this.gpValidation.Controls.Add(this.raValidationEntree);
            this.gpValidation.Controls.Add(this.raValidationAucune);
            this.gpValidation.Location = new System.Drawing.Point(12, 119);
            this.gpValidation.Name = "gpValidation";
            this.gpValidation.Size = new System.Drawing.Size(256, 55);
            this.gpValidation.TabIndex = 0;
            this.gpValidation.TabStop = false;
            this.gpValidation.Text = "Validation";
            // 
            // raValidationTabulation
            // 
            this.raValidationTabulation.AutoSize = true;
            this.raValidationTabulation.Location = new System.Drawing.Point(175, 19);
            this.raValidationTabulation.Name = "raValidationTabulation";
            this.raValidationTabulation.Size = new System.Drawing.Size(75, 17);
            this.raValidationTabulation.TabIndex = 2;
            this.raValidationTabulation.TabStop = true;
            this.raValidationTabulation.Text = "Tabulation";
            this.raValidationTabulation.UseVisualStyleBackColor = true;
            // 
            // raValidationEntree
            // 
            this.raValidationEntree.AutoSize = true;
            this.raValidationEntree.Location = new System.Drawing.Point(93, 19);
            this.raValidationEntree.Name = "raValidationEntree";
            this.raValidationEntree.Size = new System.Drawing.Size(56, 17);
            this.raValidationEntree.TabIndex = 1;
            this.raValidationEntree.TabStop = true;
            this.raValidationEntree.Text = "Entrée";
            this.raValidationEntree.UseVisualStyleBackColor = true;
            // 
            // raValidationAucune
            // 
            this.raValidationAucune.AutoSize = true;
            this.raValidationAucune.Location = new System.Drawing.Point(6, 19);
            this.raValidationAucune.Name = "raValidationAucune";
            this.raValidationAucune.Size = new System.Drawing.Size(62, 17);
            this.raValidationAucune.TabIndex = 0;
            this.raValidationAucune.TabStop = true;
            this.raValidationAucune.Tag = "";
            this.raValidationAucune.Text = "Aucune";
            this.raValidationAucune.UseVisualStyleBackColor = true;
            // 
            // notTray
            // 
            this.notTray.Icon = ((System.Drawing.Icon)(resources.GetObject("notTray.Icon")));
            this.notTray.Visible = true;
            this.notTray.Click += new System.EventHandler(this.notTray_Click);
            // 
            // cbReaders
            // 
            this.cbReaders.FormattingEnabled = true;
            this.cbReaders.Location = new System.Drawing.Point(12, 90);
            this.cbReaders.Name = "cbReaders";
            this.cbReaders.Size = new System.Drawing.Size(256, 21);
            this.cbReaders.TabIndex = 4;
            // 
            // rtHistorique
            // 
            this.rtHistorique.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(28)))));
            this.rtHistorique.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtHistorique.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.rtHistorique.Location = new System.Drawing.Point(12, 203);
            this.rtHistorique.Name = "rtHistorique";
            this.rtHistorique.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.rtHistorique.Size = new System.Drawing.Size(256, 426);
            this.rtHistorique.TabIndex = 0;
            this.rtHistorique.Text = "";
            this.rtHistorique.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            this.rtHistorique.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyboardEventTrap);
            this.rtHistorique.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            // 
            // btnTray
            // 
            this.btnTray.FlatAppearance.BorderSize = 0;
            this.btnTray.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTray.Image = global::Nmind.pcsc.reader.Properties.Resources._2downarrow;
            this.btnTray.Location = new System.Drawing.Point(196, 13);
            this.btnTray.Margin = new System.Windows.Forms.Padding(0);
            this.btnTray.Name = "btnTray";
            this.btnTray.Size = new System.Drawing.Size(72, 64);
            this.btnTray.TabIndex = 5;
            this.btnTray.UseVisualStyleBackColor = true;
            this.btnTray.Click += new System.EventHandler(this.btTray_Click);
            this.btnTray.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            this.btnTray.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyboardEventTrap);
            this.btnTray.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            // 
            // btnPause
            // 
            this.btnPause.BackColor = System.Drawing.Color.Transparent;
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Image = global::Nmind.pcsc.reader.Properties.Resources.pause;
            this.btnPause.Location = new System.Drawing.Point(12, 13);
            this.btnPause.Margin = new System.Windows.Forms.Padding(0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(72, 64);
            this.btnPause.TabIndex = 3;
            this.btnPause.UseVisualStyleBackColor = false;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            this.btnPause.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            this.btnPause.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyboardEventTrap);
            this.btnPause.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.Transparent;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Image = global::Nmind.pcsc.reader.Properties.Resources.forward;
            this.btnPlay.Location = new System.Drawing.Point(105, 13);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(72, 64);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            this.btnPlay.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            this.btnPlay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Control_KeyboardEventTrap);
            this.btnPlay.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Control_KeyboardEventTrap);
            // 
            // cbRunWindowsStart
            // 
            this.cbRunWindowsStart.AutoSize = true;
            this.cbRunWindowsStart.Location = new System.Drawing.Point(12, 180);
            this.cbRunWindowsStart.Name = "cbRunWindowsStart";
            this.cbRunWindowsStart.Size = new System.Drawing.Size(127, 17);
            this.cbRunWindowsStart.TabIndex = 8;
            this.cbRunWindowsStart.Text = "Lancer au démarrage";
            this.cbRunWindowsStart.UseVisualStyleBackColor = true;
            this.cbRunWindowsStart.CheckedChanged += new System.EventHandler(this.cbRunWindowsStart_CheckedChanged);
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 641);
            this.Controls.Add(this.cbRunWindowsStart);
            this.Controls.Add(this.btnTray);
            this.Controls.Add(this.rtHistorique);
            this.Controls.Add(this.cbReaders);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.gpValidation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "NFC - Reader";
            this.Activated += new System.EventHandler(this.FrmMain_Activated);
            this.Deactivate += new System.EventHandler(this.FrmMain_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMain_FormClosing);
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.Shown += new System.EventHandler(this.FrmMain_Shown);
            this.gpValidation.ResumeLayout(false);
            this.gpValidation.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox gpValidation;
        private System.Windows.Forms.RadioButton raValidationTabulation;
        private System.Windows.Forms.RadioButton raValidationEntree;
        private System.Windows.Forms.RadioButton raValidationAucune;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.NotifyIcon notTray;
        private System.Windows.Forms.ComboBox cbReaders;
        private System.Windows.Forms.RichTextBox rtHistorique;
        private System.Windows.Forms.Button btnTray;
        private System.Windows.Forms.CheckBox cbRunWindowsStart;
    }
}

