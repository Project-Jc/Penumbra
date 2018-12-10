namespace Penumbra
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Logger = new System.Windows.Forms.ListBox();
            this.Bot = new System.ComponentModel.BackgroundWorker();
            this.PotTick = new System.Windows.Forms.Timer(this.components);
            this.SomaPanel = new System.Windows.Forms.Panel();
            this.TargetBox = new System.Windows.Forms.GroupBox();
            this.TargetDamageBox = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.TargetHPMax = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.StatsLabel = new System.Windows.Forms.Label();
            this.TargetHP = new System.Windows.Forms.Label();
            this.PotBox = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.PotManaPercent = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.PotHealthPercent = new System.Windows.Forms.TextBox();
            this.ClickBox = new System.Windows.Forms.GroupBox();
            this.ClickLeft = new System.Windows.Forms.CheckBox();
            this.ClickPK = new System.Windows.Forms.CheckBox();
            this.ClickRight = new System.Windows.Forms.CheckBox();
            this.ClickDrops = new System.Windows.Forms.CheckBox();
            this.TargetTick = new System.Windows.Forms.Timer(this.components);
            this.BottingBox = new System.Windows.Forms.GroupBox();
            this.CraftButton = new System.Windows.Forms.Button();
            this.BotAdvanced = new System.Windows.Forms.Button();
            this.ClearWaypoints = new System.Windows.Forms.Button();
            this.SaveWaypoints = new System.Windows.Forms.Button();
            this.LoadWaypoints = new System.Windows.Forms.Button();
            this.SetEntityTypeID = new System.Windows.Forms.Button();
            this.ClickTick = new System.Windows.Forms.Timer(this.components);
            this.EntityIDWorker = new System.ComponentModel.BackgroundWorker();
            this.Clock = new System.Windows.Forms.Timer(this.components);
            this.Craftbot = new System.ComponentModel.BackgroundWorker();
            this.EmbedSoma = new System.Windows.Forms.Button();
            this.LoadGame = new System.Windows.Forms.Button();
            this.ShiftTimer = new System.Windows.Forms.Timer(this.components);
            this.gmDetection = new System.Windows.Forms.Timer(this.components);
            this.StatTrackTick = new System.Windows.Forms.Timer(this.components);
            this.TargetBox.SuspendLayout();
            this.PotBox.SuspendLayout();
            this.ClickBox.SuspendLayout();
            this.BottingBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // Logger
            // 
            this.Logger.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.Logger.Cursor = System.Windows.Forms.Cursors.Default;
            this.Logger.ForeColor = System.Drawing.Color.DeepSkyBlue;
            this.Logger.FormattingEnabled = true;
            this.Logger.Location = new System.Drawing.Point(11, 7);
            this.Logger.Name = "Logger";
            this.Logger.Size = new System.Drawing.Size(212, 238);
            this.Logger.TabIndex = 0;
            this.Logger.TabStop = false;
            // 
            // Bot
            // 
            this.Bot.WorkerSupportsCancellation = true;
            this.Bot.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Bot_DoWork);
            // 
            // PotTick
            // 
            this.PotTick.Interval = 10;
            this.PotTick.Tick += new System.EventHandler(this.PotTick_Tick);
            // 
            // SomaPanel
            // 
            this.SomaPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.SomaPanel.ForeColor = System.Drawing.Color.DarkRed;
            this.SomaPanel.Location = new System.Drawing.Point(236, 12);
            this.SomaPanel.Name = "SomaPanel";
            this.SomaPanel.Size = new System.Drawing.Size(1280, 770);
            this.SomaPanel.TabIndex = 3;
            // 
            // TargetBox
            // 
            this.TargetBox.Controls.Add(this.TargetDamageBox);
            this.TargetBox.Controls.Add(this.label7);
            this.TargetBox.Controls.Add(this.label6);
            this.TargetBox.Controls.Add(this.TargetHPMax);
            this.TargetBox.Controls.Add(this.label1);
            this.TargetBox.Controls.Add(this.StatsLabel);
            this.TargetBox.Controls.Add(this.TargetHP);
            this.TargetBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetBox.ForeColor = System.Drawing.Color.Gray;
            this.TargetBox.Location = new System.Drawing.Point(11, 254);
            this.TargetBox.Name = "TargetBox";
            this.TargetBox.Size = new System.Drawing.Size(213, 102);
            this.TargetBox.TabIndex = 5;
            this.TargetBox.TabStop = false;
            this.TargetBox.Text = " Target ";
            // 
            // TargetDamageBox
            // 
            this.TargetDamageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.TargetDamageBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TargetDamageBox.Cursor = System.Windows.Forms.Cursors.Default;
            this.TargetDamageBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetDamageBox.ForeColor = System.Drawing.Color.Red;
            this.TargetDamageBox.FormattingEnabled = true;
            this.TargetDamageBox.ItemHeight = 17;
            this.TargetDamageBox.Location = new System.Drawing.Point(171, 21);
            this.TargetDamageBox.Name = "TargetDamageBox";
            this.TargetDamageBox.Size = new System.Drawing.Size(40, 68);
            this.TargetDamageBox.TabIndex = 13;
            this.TargetDamageBox.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(78, 21);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(25, 20);
            this.label7.TabIndex = 8;
            this.label7.Text = "- [";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(69, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 15);
            this.label6.TabIndex = 7;
            this.label6.Text = "]";
            // 
            // TargetHPMax
            // 
            this.TargetHPMax.AutoSize = true;
            this.TargetHPMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetHPMax.Location = new System.Drawing.Point(100, 21);
            this.TargetHPMax.Name = "TargetHPMax";
            this.TargetHPMax.Size = new System.Drawing.Size(38, 24);
            this.TargetHPMax.TabIndex = 6;
            this.TargetHPMax.Text = "HP";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(11, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "[";
            // 
            // StatsLabel
            // 
            this.StatsLabel.AutoSize = true;
            this.StatsLabel.Font = new System.Drawing.Font("Franklin Gothic Medium", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatsLabel.Location = new System.Drawing.Point(12, 76);
            this.StatsLabel.Name = "StatsLabel";
            this.StatsLabel.Size = new System.Drawing.Size(34, 16);
            this.StatsLabel.TabIndex = 4;
            this.StatsLabel.Text = "Stats";
            // 
            // TargetHP
            // 
            this.TargetHP.AutoSize = true;
            this.TargetHP.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TargetHP.Location = new System.Drawing.Point(16, 23);
            this.TargetHP.Name = "TargetHP";
            this.TargetHP.Size = new System.Drawing.Size(33, 20);
            this.TargetHP.TabIndex = 3;
            this.TargetHP.Text = "HP";
            // 
            // PotBox
            // 
            this.PotBox.Controls.Add(this.label9);
            this.PotBox.Controls.Add(this.label8);
            this.PotBox.Controls.Add(this.label3);
            this.PotBox.Controls.Add(this.PotManaPercent);
            this.PotBox.Controls.Add(this.label2);
            this.PotBox.Controls.Add(this.PotHealthPercent);
            this.PotBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PotBox.ForeColor = System.Drawing.Color.Gray;
            this.PotBox.Location = new System.Drawing.Point(11, 448);
            this.PotBox.Name = "PotBox";
            this.PotBox.Size = new System.Drawing.Size(213, 80);
            this.PotBox.TabIndex = 8;
            this.PotBox.TabStop = false;
            this.PotBox.Text = " Pot ";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(35, 49);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 17);
            this.label9.TabIndex = 9;
            this.label9.Text = "Mana";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(35, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 17);
            this.label8.TabIndex = 8;
            this.label8.Text = "Health";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(157, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "%";
            // 
            // PotManaPercent
            // 
            this.PotManaPercent.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PotManaPercent.ForeColor = System.Drawing.Color.Gray;
            this.PotManaPercent.Location = new System.Drawing.Point(126, 46);
            this.PotManaPercent.MaxLength = 2;
            this.PotManaPercent.Name = "PotManaPercent";
            this.PotManaPercent.Size = new System.Drawing.Size(25, 22);
            this.PotManaPercent.TabIndex = 6;
            this.PotManaPercent.Text = "8";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(158, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "%";
            // 
            // PotHealthPercent
            // 
            this.PotHealthPercent.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PotHealthPercent.ForeColor = System.Drawing.Color.Gray;
            this.PotHealthPercent.Location = new System.Drawing.Point(126, 19);
            this.PotHealthPercent.MaxLength = 2;
            this.PotHealthPercent.Name = "PotHealthPercent";
            this.PotHealthPercent.Size = new System.Drawing.Size(25, 22);
            this.PotHealthPercent.TabIndex = 4;
            this.PotHealthPercent.Text = "25";
            // 
            // ClickBox
            // 
            this.ClickBox.Controls.Add(this.ClickLeft);
            this.ClickBox.Controls.Add(this.ClickPK);
            this.ClickBox.Controls.Add(this.ClickRight);
            this.ClickBox.Controls.Add(this.ClickDrops);
            this.ClickBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClickBox.ForeColor = System.Drawing.Color.Gray;
            this.ClickBox.Location = new System.Drawing.Point(11, 362);
            this.ClickBox.Name = "ClickBox";
            this.ClickBox.Size = new System.Drawing.Size(213, 80);
            this.ClickBox.TabIndex = 7;
            this.ClickBox.TabStop = false;
            this.ClickBox.Text = " Click ";
            // 
            // ClickLeft
            // 
            this.ClickLeft.AutoSize = true;
            this.ClickLeft.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClickLeft.Location = new System.Drawing.Point(31, 21);
            this.ClickLeft.Name = "ClickLeft";
            this.ClickLeft.Size = new System.Drawing.Size(48, 21);
            this.ClickLeft.TabIndex = 3;
            this.ClickLeft.Text = "Left";
            this.ClickLeft.UseVisualStyleBackColor = true;
            // 
            // ClickPK
            // 
            this.ClickPK.AutoSize = true;
            this.ClickPK.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClickPK.Location = new System.Drawing.Point(116, 21);
            this.ClickPK.Name = "ClickPK";
            this.ClickPK.Size = new System.Drawing.Size(43, 21);
            this.ClickPK.TabIndex = 2;
            this.ClickPK.Text = "PK";
            this.ClickPK.UseVisualStyleBackColor = true;
            // 
            // ClickRight
            // 
            this.ClickRight.AutoSize = true;
            this.ClickRight.Checked = true;
            this.ClickRight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClickRight.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClickRight.Location = new System.Drawing.Point(31, 48);
            this.ClickRight.Name = "ClickRight";
            this.ClickRight.Size = new System.Drawing.Size(56, 21);
            this.ClickRight.TabIndex = 1;
            this.ClickRight.Text = "Right";
            this.ClickRight.UseVisualStyleBackColor = true;
            // 
            // ClickDrops
            // 
            this.ClickDrops.AutoSize = true;
            this.ClickDrops.Checked = true;
            this.ClickDrops.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ClickDrops.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClickDrops.Location = new System.Drawing.Point(116, 48);
            this.ClickDrops.Name = "ClickDrops";
            this.ClickDrops.Size = new System.Drawing.Size(60, 21);
            this.ClickDrops.TabIndex = 0;
            this.ClickDrops.Text = "Drops";
            this.ClickDrops.UseVisualStyleBackColor = true;
            // 
            // TargetTick
            // 
            this.TargetTick.Tick += new System.EventHandler(this.TargetTick_Tick);
            // 
            // BottingBox
            // 
            this.BottingBox.Controls.Add(this.CraftButton);
            this.BottingBox.Controls.Add(this.BotAdvanced);
            this.BottingBox.Controls.Add(this.ClearWaypoints);
            this.BottingBox.Controls.Add(this.SaveWaypoints);
            this.BottingBox.Controls.Add(this.LoadWaypoints);
            this.BottingBox.Controls.Add(this.SetEntityTypeID);
            this.BottingBox.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BottingBox.ForeColor = System.Drawing.Color.Gray;
            this.BottingBox.Location = new System.Drawing.Point(11, 531);
            this.BottingBox.Name = "BottingBox";
            this.BottingBox.Size = new System.Drawing.Size(213, 144);
            this.BottingBox.TabIndex = 6;
            this.BottingBox.TabStop = false;
            this.BottingBox.Text = " Bot ";
            // 
            // CraftButton
            // 
            this.CraftButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.CraftButton.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CraftButton.Location = new System.Drawing.Point(20, 51);
            this.CraftButton.Name = "CraftButton";
            this.CraftButton.Size = new System.Drawing.Size(62, 27);
            this.CraftButton.TabIndex = 13;
            this.CraftButton.TabStop = false;
            this.CraftButton.Text = "Craft";
            this.CraftButton.UseVisualStyleBackColor = false;
            this.CraftButton.Click += new System.EventHandler(this.CraftButton_Click);
            // 
            // BotAdvanced
            // 
            this.BotAdvanced.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.BotAdvanced.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotAdvanced.ForeColor = System.Drawing.Color.Gray;
            this.BotAdvanced.Location = new System.Drawing.Point(70, 17);
            this.BotAdvanced.Name = "BotAdvanced";
            this.BotAdvanced.Size = new System.Drawing.Size(80, 25);
            this.BotAdvanced.TabIndex = 12;
            this.BotAdvanced.TabStop = false;
            this.BotAdvanced.Text = "Configure";
            this.BotAdvanced.UseVisualStyleBackColor = false;
            this.BotAdvanced.Click += new System.EventHandler(this.BotAdvanced_Click);
            // 
            // ClearWaypoints
            // 
            this.ClearWaypoints.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.ClearWaypoints.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearWaypoints.Location = new System.Drawing.Point(19, 94);
            this.ClearWaypoints.Name = "ClearWaypoints";
            this.ClearWaypoints.Size = new System.Drawing.Size(50, 25);
            this.ClearWaypoints.TabIndex = 12;
            this.ClearWaypoints.TabStop = false;
            this.ClearWaypoints.Text = "Clear";
            this.ClearWaypoints.UseVisualStyleBackColor = false;
            this.ClearWaypoints.Click += new System.EventHandler(this.ClearWaypoints_Click);
            // 
            // SaveWaypoints
            // 
            this.SaveWaypoints.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.SaveWaypoints.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveWaypoints.Location = new System.Drawing.Point(81, 108);
            this.SaveWaypoints.Name = "SaveWaypoints";
            this.SaveWaypoints.Size = new System.Drawing.Size(50, 25);
            this.SaveWaypoints.TabIndex = 11;
            this.SaveWaypoints.TabStop = false;
            this.SaveWaypoints.Text = "Save";
            this.SaveWaypoints.UseVisualStyleBackColor = false;
            this.SaveWaypoints.Click += new System.EventHandler(this.SaveWaypoints_Click);
            // 
            // LoadWaypoints
            // 
            this.LoadWaypoints.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.LoadWaypoints.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadWaypoints.Location = new System.Drawing.Point(144, 94);
            this.LoadWaypoints.Name = "LoadWaypoints";
            this.LoadWaypoints.Size = new System.Drawing.Size(50, 25);
            this.LoadWaypoints.TabIndex = 10;
            this.LoadWaypoints.TabStop = false;
            this.LoadWaypoints.Text = "Load";
            this.LoadWaypoints.UseVisualStyleBackColor = false;
            this.LoadWaypoints.Click += new System.EventHandler(this.LoadWaypoints_Click);
            // 
            // SetEntityTypeID
            // 
            this.SetEntityTypeID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.SetEntityTypeID.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SetEntityTypeID.Location = new System.Drawing.Point(132, 51);
            this.SetEntityTypeID.Name = "SetEntityTypeID";
            this.SetEntityTypeID.Size = new System.Drawing.Size(62, 27);
            this.SetEntityTypeID.TabIndex = 4;
            this.SetEntityTypeID.TabStop = false;
            this.SetEntityTypeID.Text = "Entity";
            this.SetEntityTypeID.UseVisualStyleBackColor = false;
            this.SetEntityTypeID.Click += new System.EventHandler(this.SetEntityTypeID_Click);
            // 
            // ClickTick
            // 
            this.ClickTick.Interval = 50;
            this.ClickTick.Tick += new System.EventHandler(this.LClickTick_Tick);
            // 
            // EntityIDWorker
            // 
            this.EntityIDWorker.WorkerSupportsCancellation = true;
            this.EntityIDWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            // 
            // Clock
            // 
            this.Clock.Interval = 1000;
            this.Clock.Tick += new System.EventHandler(this.Clock_Tick);
            // 
            // Craftbot
            // 
            this.Craftbot.WorkerSupportsCancellation = true;
            this.Craftbot.DoWork += new System.ComponentModel.DoWorkEventHandler(this.Craftbot_DoWork);
            // 
            // EmbedSoma
            // 
            this.EmbedSoma.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.EmbedSoma.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EmbedSoma.ForeColor = System.Drawing.Color.Gray;
            this.EmbedSoma.Location = new System.Drawing.Point(37, 681);
            this.EmbedSoma.Name = "EmbedSoma";
            this.EmbedSoma.Size = new System.Drawing.Size(76, 25);
            this.EmbedSoma.TabIndex = 11;
            this.EmbedSoma.TabStop = false;
            this.EmbedSoma.Text = "Embed";
            this.EmbedSoma.UseVisualStyleBackColor = false;
            this.EmbedSoma.Click += new System.EventHandler(this.EmbedSoma_Click);
            // 
            // LoadGame
            // 
            this.LoadGame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.LoadGame.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoadGame.ForeColor = System.Drawing.Color.Gray;
            this.LoadGame.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.LoadGame.Location = new System.Drawing.Point(119, 681);
            this.LoadGame.Name = "LoadGame";
            this.LoadGame.Size = new System.Drawing.Size(76, 25);
            this.LoadGame.TabIndex = 12;
            this.LoadGame.TabStop = false;
            this.LoadGame.Text = "Run";
            this.LoadGame.UseVisualStyleBackColor = false;
            this.LoadGame.Click += new System.EventHandler(this.LoadGame_Click);
            // 
            // ShiftTimer
            // 
            this.ShiftTimer.Tick += new System.EventHandler(this.ShiftTimer_Tick);
            // 
            // gmDetection
            // 
            this.gmDetection.Interval = 5000;
            this.gmDetection.Tick += new System.EventHandler(this.gmDetection_Tick);
            // 
            // StatTrackTick
            // 
            this.StatTrackTick.Interval = 1000;
            this.StatTrackTick.Tick += new System.EventHandler(this.StatTrackTick_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(234, 712);
            this.Controls.Add(this.TargetBox);
            this.Controls.Add(this.PotBox);
            this.Controls.Add(this.LoadGame);
            this.Controls.Add(this.ClickBox);
            this.Controls.Add(this.BottingBox);
            this.Controls.Add(this.EmbedSoma);
            this.Controls.Add(this.SomaPanel);
            this.Controls.Add(this.Logger);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Penumbra V2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.TargetBox.ResumeLayout(false);
            this.TargetBox.PerformLayout();
            this.PotBox.ResumeLayout(false);
            this.PotBox.PerformLayout();
            this.ClickBox.ResumeLayout(false);
            this.ClickBox.PerformLayout();
            this.BottingBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Logger;
        private System.ComponentModel.BackgroundWorker Bot;
        private System.Windows.Forms.Timer PotTick;
        private System.Windows.Forms.Panel SomaPanel;
        private System.Windows.Forms.Timer TargetTick;
        private System.Windows.Forms.GroupBox TargetBox;
        private System.Windows.Forms.Label TargetHP;
        private System.Windows.Forms.GroupBox BottingBox;
        private System.Windows.Forms.GroupBox ClickBox;
        private System.Windows.Forms.CheckBox ClickRight;
        private System.Windows.Forms.CheckBox ClickDrops;
        private System.Windows.Forms.Timer ClickTick;
        private System.Windows.Forms.CheckBox ClickPK;
        private System.Windows.Forms.CheckBox ClickLeft;
        private System.Windows.Forms.GroupBox PotBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox PotHealthPercent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PotManaPercent;
        private System.Windows.Forms.Label StatsLabel;
        private System.ComponentModel.BackgroundWorker EntityIDWorker;
        private System.Windows.Forms.Button LoadWaypoints;
        private System.Windows.Forms.Button SaveWaypoints;
        private System.Windows.Forms.Button ClearWaypoints;
        private System.Windows.Forms.Timer Clock;
        private System.ComponentModel.BackgroundWorker Craftbot;
        private System.Windows.Forms.Button EmbedSoma;
        private System.Windows.Forms.Button BotAdvanced;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label TargetHPMax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button LoadGame;
        private System.Windows.Forms.ListBox TargetDamageBox;
        private System.Windows.Forms.Timer ShiftTimer;
        private System.Windows.Forms.Button CraftButton;
        private System.Windows.Forms.Button SetEntityTypeID;
        private System.Windows.Forms.Timer gmDetection;
        private System.Windows.Forms.Timer StatTrackTick;
    }
}

