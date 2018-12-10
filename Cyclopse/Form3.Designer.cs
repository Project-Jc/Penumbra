namespace Penumbra
{
    partial class Form3
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
            this.label7 = new System.Windows.Forms.Label();
            this.HealFKey = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.StopAfterValue = new System.Windows.Forms.TextBox();
            this.StopBotAfter = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.BotHealPercent = new System.Windows.Forms.TextBox();
            this.BotHeal = new System.Windows.Forms.CheckBox();
            this.AttackTimeOut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NoWarpOnDeath = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.AuraDelay = new System.Windows.Forms.TextBox();
            this.ShowDebug = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.BeforeCombat = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.EndIfZeroPotions = new System.Windows.Forms.CheckBox();
            this.PotToFull = new System.Windows.Forms.CheckBox();
            this.HealInCombat = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.IgnorePlayerKillers = new System.Windows.Forms.CheckBox();
            this.TrackPlayerStats = new System.Windows.Forms.CheckBox();
            this.AdditionalTargetInfo = new System.Windows.Forms.CheckBox();
            this.ShutdownPC = new System.Windows.Forms.CheckBox();
            this.BreadcrumbWP = new System.Windows.Forms.CheckBox();
            this.NoDrops = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.BotAura = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Gray;
            this.label7.Location = new System.Drawing.Point(122, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 17);
            this.label7.TabIndex = 30;
            this.label7.Text = "Slot";
            // 
            // HealFKey
            // 
            this.HealFKey.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HealFKey.ForeColor = System.Drawing.Color.Gray;
            this.HealFKey.Location = new System.Drawing.Point(157, 19);
            this.HealFKey.MaxLength = 1;
            this.HealFKey.Name = "HealFKey";
            this.HealFKey.Size = new System.Drawing.Size(25, 22);
            this.HealFKey.TabIndex = 29;
            this.HealFKey.Text = "0";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Gray;
            this.label6.Location = new System.Drawing.Point(153, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 15);
            this.label6.TabIndex = 28;
            this.label6.Text = "minutes";
            // 
            // StopAfterValue
            // 
            this.StopAfterValue.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopAfterValue.ForeColor = System.Drawing.Color.Gray;
            this.StopAfterValue.Location = new System.Drawing.Point(114, 72);
            this.StopAfterValue.MaxLength = 3;
            this.StopAfterValue.Name = "StopAfterValue";
            this.StopAfterValue.Size = new System.Drawing.Size(33, 22);
            this.StopAfterValue.TabIndex = 27;
            // 
            // StopBotAfter
            // 
            this.StopBotAfter.AutoSize = true;
            this.StopBotAfter.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StopBotAfter.ForeColor = System.Drawing.Color.Gray;
            this.StopBotAfter.Location = new System.Drawing.Point(33, 75);
            this.StopBotAfter.Name = "StopBotAfter";
            this.StopBotAfter.Size = new System.Drawing.Size(75, 19);
            this.StopBotAfter.TabIndex = 26;
            this.StopBotAfter.Text = "Stop After";
            this.StopBotAfter.UseVisualStyleBackColor = true;
            this.StopBotAfter.CheckedChanged += new System.EventHandler(this.StopBotAfter_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Location = new System.Drawing.Point(97, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 17);
            this.label1.TabIndex = 25;
            this.label1.Text = "%";
            // 
            // BotHealPercent
            // 
            this.BotHealPercent.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotHealPercent.ForeColor = System.Drawing.Color.Gray;
            this.BotHealPercent.Location = new System.Drawing.Point(70, 19);
            this.BotHealPercent.MaxLength = 2;
            this.BotHealPercent.Name = "BotHealPercent";
            this.BotHealPercent.Size = new System.Drawing.Size(25, 22);
            this.BotHealPercent.TabIndex = 24;
            this.BotHealPercent.Text = "58";
            // 
            // BotHeal
            // 
            this.BotHeal.AutoSize = true;
            this.BotHeal.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotHeal.ForeColor = System.Drawing.Color.Gray;
            this.BotHeal.Location = new System.Drawing.Point(15, 21);
            this.BotHeal.Name = "BotHeal";
            this.BotHeal.Size = new System.Drawing.Size(52, 21);
            this.BotHeal.TabIndex = 23;
            this.BotHeal.Text = "Heal";
            this.BotHeal.UseVisualStyleBackColor = true;
            // 
            // AttackTimeOut
            // 
            this.AttackTimeOut.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AttackTimeOut.ForeColor = System.Drawing.Color.Gray;
            this.AttackTimeOut.Location = new System.Drawing.Point(122, 19);
            this.AttackTimeOut.MaxLength = 2;
            this.AttackTimeOut.Name = "AttackTimeOut";
            this.AttackTimeOut.Size = new System.Drawing.Size(25, 22);
            this.AttackTimeOut.TabIndex = 31;
            this.AttackTimeOut.Text = "10";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Gray;
            this.label2.Location = new System.Drawing.Point(29, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 15);
            this.label2.TabIndex = 32;
            this.label2.Text = "Attack Time Out";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Gray;
            this.label3.Location = new System.Drawing.Point(153, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 15);
            this.label3.TabIndex = 33;
            this.label3.Text = "seconds";
            // 
            // NoWarpOnDeath
            // 
            this.NoWarpOnDeath.AutoSize = true;
            this.NoWarpOnDeath.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoWarpOnDeath.ForeColor = System.Drawing.Color.Gray;
            this.NoWarpOnDeath.Location = new System.Drawing.Point(13, 20);
            this.NoWarpOnDeath.Name = "NoWarpOnDeath";
            this.NoWarpOnDeath.Size = new System.Drawing.Size(82, 19);
            this.NoWarpOnDeath.TabIndex = 35;
            this.NoWarpOnDeath.Text = "No Warping";
            this.NoWarpOnDeath.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Gray;
            this.label4.Location = new System.Drawing.Point(153, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 15);
            this.label4.TabIndex = 38;
            this.label4.Text = "seconds";
            // 
            // AuraDelay
            // 
            this.AuraDelay.Font = new System.Drawing.Font("Franklin Gothic Medium", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AuraDelay.ForeColor = System.Drawing.Color.Gray;
            this.AuraDelay.Location = new System.Drawing.Point(117, 45);
            this.AuraDelay.MaxLength = 3;
            this.AuraDelay.Name = "AuraDelay";
            this.AuraDelay.Size = new System.Drawing.Size(30, 22);
            this.AuraDelay.TabIndex = 36;
            this.AuraDelay.Text = "50";
            // 
            // ShowDebug
            // 
            this.ShowDebug.AutoSize = true;
            this.ShowDebug.Checked = true;
            this.ShowDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ShowDebug.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShowDebug.ForeColor = System.Drawing.Color.Gray;
            this.ShowDebug.Location = new System.Drawing.Point(120, 42);
            this.ShowDebug.Name = "ShowDebug";
            this.ShowDebug.Size = new System.Drawing.Size(57, 19);
            this.ShowDebug.TabIndex = 39;
            this.ShowDebug.Text = "Debug";
            this.ShowDebug.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BeforeCombat);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.HealInCombat);
            this.groupBox1.Controls.Add(this.BotHeal);
            this.groupBox1.Controls.Add(this.BotHealPercent);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.HealFKey);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.ForeColor = System.Drawing.Color.Gray;
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(223, 146);
            this.groupBox1.TabIndex = 41;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Healing ";
            // 
            // BeforeCombat
            // 
            this.BeforeCombat.AutoSize = true;
            this.BeforeCombat.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BeforeCombat.ForeColor = System.Drawing.Color.Gray;
            this.BeforeCombat.Location = new System.Drawing.Point(117, 47);
            this.BeforeCombat.Name = "BeforeCombat";
            this.BeforeCombat.Size = new System.Drawing.Size(98, 19);
            this.BeforeCombat.TabIndex = 43;
            this.BeforeCombat.Text = "Before Combat";
            this.BeforeCombat.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.EndIfZeroPotions);
            this.groupBox4.Controls.Add(this.PotToFull);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.ForeColor = System.Drawing.Color.Gray;
            this.groupBox4.Location = new System.Drawing.Point(10, 72);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(200, 59);
            this.groupBox4.TabIndex = 42;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = " Potions ";
            // 
            // EndIfZeroPotions
            // 
            this.EndIfZeroPotions.AutoSize = true;
            this.EndIfZeroPotions.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndIfZeroPotions.ForeColor = System.Drawing.Color.Gray;
            this.EndIfZeroPotions.Location = new System.Drawing.Point(100, 24);
            this.EndIfZeroPotions.Name = "EndIfZeroPotions";
            this.EndIfZeroPotions.Size = new System.Drawing.Size(78, 19);
            this.EndIfZeroPotions.TabIndex = 36;
            this.EndIfZeroPotions.Text = "End If Zero";
            this.EndIfZeroPotions.UseVisualStyleBackColor = true;
            // 
            // PotToFull
            // 
            this.PotToFull.AutoSize = true;
            this.PotToFull.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PotToFull.ForeColor = System.Drawing.Color.Gray;
            this.PotToFull.Location = new System.Drawing.Point(14, 24);
            this.PotToFull.Name = "PotToFull";
            this.PotToFull.Size = new System.Drawing.Size(78, 19);
            this.PotToFull.TabIndex = 34;
            this.PotToFull.Text = "Pot To Full";
            this.PotToFull.UseVisualStyleBackColor = true;
            // 
            // HealInCombat
            // 
            this.HealInCombat.AutoSize = true;
            this.HealInCombat.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.HealInCombat.ForeColor = System.Drawing.Color.Gray;
            this.HealInCombat.Location = new System.Drawing.Point(33, 47);
            this.HealInCombat.Name = "HealInCombat";
            this.HealInCombat.Size = new System.Drawing.Size(75, 19);
            this.HealInCombat.TabIndex = 35;
            this.HealInCombat.Text = "In Combat";
            this.HealInCombat.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.IgnorePlayerKillers);
            this.groupBox2.Controls.Add(this.TrackPlayerStats);
            this.groupBox2.Controls.Add(this.AdditionalTargetInfo);
            this.groupBox2.Controls.Add(this.ShutdownPC);
            this.groupBox2.Controls.Add(this.BreadcrumbWP);
            this.groupBox2.Controls.Add(this.NoDrops);
            this.groupBox2.Controls.Add(this.NoWarpOnDeath);
            this.groupBox2.Controls.Add(this.ShowDebug);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.ForeColor = System.Drawing.Color.Gray;
            this.groupBox2.Location = new System.Drawing.Point(8, 273);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 121);
            this.groupBox2.TabIndex = 42;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = " Misc ";
            // 
            // IgnorePlayerKillers
            // 
            this.IgnorePlayerKillers.AutoSize = true;
            this.IgnorePlayerKillers.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IgnorePlayerKillers.ForeColor = System.Drawing.Color.Gray;
            this.IgnorePlayerKillers.Location = new System.Drawing.Point(13, 45);
            this.IgnorePlayerKillers.Name = "IgnorePlayerKillers";
            this.IgnorePlayerKillers.Size = new System.Drawing.Size(74, 19);
            this.IgnorePlayerKillers.TabIndex = 46;
            this.IgnorePlayerKillers.Text = "Ignore PK";
            this.IgnorePlayerKillers.UseVisualStyleBackColor = true;
            // 
            // TrackPlayerStats
            // 
            this.TrackPlayerStats.AutoSize = true;
            this.TrackPlayerStats.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TrackPlayerStats.Location = new System.Drawing.Point(13, 92);
            this.TrackPlayerStats.Name = "TrackPlayerStats";
            this.TrackPlayerStats.Size = new System.Drawing.Size(80, 19);
            this.TrackPlayerStats.TabIndex = 45;
            this.TrackPlayerStats.Text = "Track Stats";
            this.TrackPlayerStats.UseVisualStyleBackColor = true;
            // 
            // AdditionalTargetInfo
            // 
            this.AdditionalTargetInfo.AutoSize = true;
            this.AdditionalTargetInfo.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AdditionalTargetInfo.Location = new System.Drawing.Point(13, 67);
            this.AdditionalTargetInfo.Name = "AdditionalTargetInfo";
            this.AdditionalTargetInfo.Size = new System.Drawing.Size(78, 19);
            this.AdditionalTargetInfo.TabIndex = 6;
            this.AdditionalTargetInfo.Text = "Target Info";
            this.AdditionalTargetInfo.UseVisualStyleBackColor = true;
            // 
            // ShutdownPC
            // 
            this.ShutdownPC.AutoSize = true;
            this.ShutdownPC.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ShutdownPC.ForeColor = System.Drawing.Color.Gray;
            this.ShutdownPC.Location = new System.Drawing.Point(120, 92);
            this.ShutdownPC.Name = "ShutdownPC";
            this.ShutdownPC.Size = new System.Drawing.Size(89, 19);
            this.ShutdownPC.TabIndex = 42;
            this.ShutdownPC.Text = "Shutdown PC";
            this.ShutdownPC.UseVisualStyleBackColor = true;
            // 
            // BreadcrumbWP
            // 
            this.BreadcrumbWP.AutoSize = true;
            this.BreadcrumbWP.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BreadcrumbWP.ForeColor = System.Drawing.Color.Gray;
            this.BreadcrumbWP.Location = new System.Drawing.Point(120, 67);
            this.BreadcrumbWP.Name = "BreadcrumbWP";
            this.BreadcrumbWP.Size = new System.Drawing.Size(85, 19);
            this.BreadcrumbWP.TabIndex = 44;
            this.BreadcrumbWP.Text = "Breadcrumb";
            this.BreadcrumbWP.UseVisualStyleBackColor = true;
            // 
            // NoDrops
            // 
            this.NoDrops.AutoSize = true;
            this.NoDrops.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NoDrops.ForeColor = System.Drawing.Color.Gray;
            this.NoDrops.Location = new System.Drawing.Point(120, 20);
            this.NoDrops.Name = "NoDrops";
            this.NoDrops.Size = new System.Drawing.Size(70, 19);
            this.NoDrops.TabIndex = 41;
            this.NoDrops.Text = "No Drops";
            this.NoDrops.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.BotAura);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.AttackTimeOut);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.AuraDelay);
            this.groupBox3.Controls.Add(this.StopBotAfter);
            this.groupBox3.Controls.Add(this.StopAfterValue);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.ForeColor = System.Drawing.Color.Gray;
            this.groupBox3.Location = new System.Drawing.Point(8, 164);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(223, 103);
            this.groupBox3.TabIndex = 43;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = " Timing ";
            // 
            // BotAura
            // 
            this.BotAura.AutoSize = true;
            this.BotAura.Checked = true;
            this.BotAura.CheckState = System.Windows.Forms.CheckState.Checked;
            this.BotAura.Font = new System.Drawing.Font("Franklin Gothic Medium", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BotAura.Location = new System.Drawing.Point(20, 47);
            this.BotAura.Name = "BotAura";
            this.BotAura.Size = new System.Drawing.Size(94, 19);
            this.BotAura.TabIndex = 39;
            this.BotAura.Text = "Aura      Delay";
            this.BotAura.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(28)))), ((int)(((byte)(24)))));
            this.ClientSize = new System.Drawing.Size(241, 407);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form3";
            this.Text = "Bot Configuration";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox HealFKey;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox StopAfterValue;
        public System.Windows.Forms.CheckBox StopBotAfter;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox BotHealPercent;
        public System.Windows.Forms.CheckBox BotHeal;
        public System.Windows.Forms.TextBox AttackTimeOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.CheckBox NoWarpOnDeath;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox AuraDelay;
        public System.Windows.Forms.CheckBox ShowDebug;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        public System.Windows.Forms.CheckBox NoDrops;
        public System.Windows.Forms.CheckBox HealInCombat;
        public System.Windows.Forms.CheckBox ShutdownPC;
        public System.Windows.Forms.CheckBox BreadcrumbWP;
        public System.Windows.Forms.CheckBox BotAura;
        public System.Windows.Forms.CheckBox AdditionalTargetInfo;
        private System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.CheckBox EndIfZeroPotions;
        public System.Windows.Forms.CheckBox PotToFull;
        public System.Windows.Forms.CheckBox BeforeCombat;
        public System.Windows.Forms.CheckBox TrackPlayerStats;
        public System.Windows.Forms.CheckBox IgnorePlayerKillers;
    }
}