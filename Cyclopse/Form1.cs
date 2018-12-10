using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Media;
using System.Net;
using System.Web;

using KeyboardHooker;
using MouseKeyboardLibrary;
using MemoryEditor;
using Soma;

namespace Penumbra
{
    public partial class Form1 : Form
    {
        MouseHook mouseHook;
        KeyboardHookAlt hook;
        Form2 CraftForm;
        Form3 AdvancedForm;

        Game somaGame;
        Entity somaEntity;
        Player myPlayer;
        Mouse somaMouse;
        Belt playerBelt;
        Drops PlayerDrops;

        ArrayList Waypoint_X = new ArrayList();
        ArrayList Waypoint_Y = new ArrayList();
        ArrayList EntityType = new ArrayList();
        ArrayList BadEntity = new ArrayList();
        int CurrentWaypoint = 0;

        public Form1()
        {
            InitializeComponent();

            mouseHook = new MouseHook();
            hook = new KeyboardHookAlt();

            // Mouse hook stuff
            mouseHook.MouseWheel += new MouseEventHandler(mouseHook_MouseWheel);
            mouseHook.Start();
            
            // Register the event that is fired after the key press
            hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);
            
            // Register keys
            hook.RegisterHotKey(ModifierKeysEx.Nomod, Keys.Home);
            hook.RegisterHotKey(ModifierKeysEx.Nomod, Keys.Insert);
            hook.RegisterHotKey(ModifierKeysEx.Alt, Keys.Insert);
            hook.RegisterHotKey(ModifierKeysEx.Nomod, Keys.End);
            hook.RegisterHotKey(ModifierKeysEx.Alt, Keys.End);
            hook.RegisterHotKey(ModifierKeysEx.Nomod, Keys.PageDown);
            hook.RegisterHotKey(ModifierKeysEx.Nomod, Keys.Left);
            hook.RegisterHotKey(ModifierKeysEx.Nomod, Keys.Up);
            hook.RegisterHotKey(ModifierKeysEx.Shift, Keys.None);
            hook.RegisterHotKey(ModifierKeysEx.Alt, Keys.D);
            hook.RegisterHotKey(ModifierKeysEx.Alt, Keys.C);
            hook.RegisterHotKey(ModifierKeysEx.Alt, Keys.S);
            hook.RegisterHotKey(ModifierKeysEx.Alt, Keys.I);
        }

        void mouseHook_MouseWheel(object sender, MouseEventArgs e)
        {
            if (somaGame.IsActive())
            {
                if (!myPlayer.HasWindowOpen())
                {
                    int WriteMe = myPlayer.GetSpellKey();
                    if (e.Delta == 120)
                    {
                        WriteMe++;
                        if (WriteMe == 12)
                            WriteMe = 0;
                        myPlayer.SetSpellKey(WriteMe);
                    }
                    else
                    {
                        WriteMe--;
                        if (WriteMe == -1)
                            WriteMe = 11;
                        myPlayer.SetSpellKey(WriteMe);
                    }
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Generate random program name
            //string RndName = System.IO.Path.GetRandomFileName();
            //RndName = RndName.Replace(".", "");
            //this.Text = RndName;

            //bool Exit = true;
            //string cpuID = string.Empty,
            //       hddID = string.Empty,
            //       combinedID;

            //string[] userID = new string[] {     

            //    "34F00100FFBFB871  F943C7A8", "16604000FFBFBEFB  8E6BF529", "36F00100FFBFB871  BD8B9E2B", "7A602000FFBFBEFB  4E2FDAE7",
            //    "9A603000FFBFBEFB  1F9329AF", "A7601000FFBFBEFB  3211AD21", "55602000FFBFBEFB  F97990C3", "DF600000FFBFBEFB  E4303D8C", 
            //    "9A603000FFBFBEFB  934F6DAF", "7A602000FFBFBEFB  D6EC4129", "24F00100FFBFB871  A67E79A0", "A7601000FFBFBEFB  39E2FF0A",
            //    "3C603000FFBFBEFB  C7E21CE2", "36F00100FFBFB871  CE7136CD", "55602000FFBFBEFB  015A5861", "25F00100FFBFB871  AAA75F45",
            //    "8E600000FFBF9EFB  238DD5E1", "15604000FFBFBEFB  DEB8D8AC", "9A603000FFBFBEFB  BEFBFF67", "9A603000FFBFBEFB  5DC1FDA6",
            //    "7A602000FFBFBEFB  D41F61AA", "CE600000FFBF9EFB  4AC0368D", "10F01600FFBFB871  A293DC24", "02F00500FFBFB871  C2722162",
            //    "3C603000FFBFBEFB  07EAC648", "13F01600FFBFB871  29EE84A1", "55602000FFBFBEFB  4D68E4C7", "7A602000FFBFBEFB  9E027CCD",
            //    "DF600000FFBFBEFB  6B72C2E7", "28F06000FFBFB871  71456709", "9A603000FFBFBEFB  4FDF7821", "7A602000FFBFBEFB  EEE76C66",
            //    "35F00100FFBFB871  A9CFFBC7", "3C603000FFBFBEFB  069903AC"
            //};

            //string[] secID = new string[] {

            //    "34F9A8", "168E29", "36BD2B", "7A4EE7", 
            //    "9A1FAF", "A73221", "55F9C3", "DFE48C", 
            //    "9A93AF", "7AD629", "24A6A0", "A7390A", 
            //    "3CC7E2", "36CECD", "550161", "25AA45", 
            //    "BE23E1", "15DEAC", "9ABE67", "9A5DA6",
            //    "7AD4AA", "CE4A8D", "10A224", "02C262", 
            //    "3C0748", "1329A1", "554DC7", "7A9ECD",
            //    "DF6BE7", "287109", "9A4F21", "7AEE66",
            //    "35A9C7", "3C06AC"
            //};

            //// Get the HDD ID
            //ManagementObject dsk = new ManagementObject(@"win32_logicaldisk.deviceid=""" + "C" + @":""");
            //dsk.Get();
            //hddID = dsk["VolumeSerialNumber"].ToString();

            //// Get the CPU ID
            //ManagementClass mClass = new ManagementClass("win32_processor");
            //ManagementObjectCollection mOBJCOLLECTION = mClass.GetInstances();
            //foreach (ManagementObject mOBJ in mOBJCOLLECTION)   {
            //    cpuID = mOBJ.Properties["processorID"].Value.ToString();
            //}

            //// Combine them
            //combinedID = hddID + "  " + cpuID;
            ////MessageBox.Show(combinedID);

            //char[] charArray = combinedID.ToCharArray();
            //Array.Reverse(charArray);
            //combinedID = new string(charArray);
            ////MessageBox.Show(combinedID);

            //for (int i = 0; i < userID.Length; i++)
            //{
            //    if (combinedID == userID[i])
            //    {
            //        string secString = string.Empty;
            //        char[] secArray = combinedID.ToCharArray();

            //        secString += secArray[0];
            //        secString += secArray[1];
            //        secString += secArray[18];
            //        secString += secArray[19];
            //        secString += secArray[24];
            //        secString += secArray[25];

            //        if (secID[i] == secString)
            //        {
            //            Exit = false;
            //            LogWrite("      Master, I am here to serve... ", true);
            //            break;
            //        }
            //    }
            //}
            //if (Exit)
            //{
            //    LogWrite("Illegal user detected");
            //    Environment.Exit(0);
            //}
           
            CraftForm = new Form2();
            AdvancedForm = new Form3();
            somaGame = new Game();

            if (!somaGame.SetDirectoryLocation())
            {
                Application.Exit();
            }
            else
            {
                LogWrite("Detecting game window size", true);
                //if (!somaGame.SetGameWindowSize())
                //{
                //    LogWrite("      Wrong window size");
                //    LogWrite("      Modified game window size correctly");
                //}
                //else
                //{
                //    LogWrite("      Window size okay");
                //}
                LogWrite("Searching for process", true);
                if (!somaGame.FindProcess(false))
                {
                    LogWrite("      Could not find soma");
                }
                else
                {
                    LogWrite("      pHandle: " + Game.hWndParent.ToString("X") + "   cHandle: " + Game.hWndChild.ToString("X"));

                    somaGame.GiveFocus();

                    playerBelt = new Belt();
                    somaEntity = new Entity(false);
                    myPlayer = new Player();
                    PlayerDrops = new Drops();
                    somaMouse = new Mouse();
                    aEntity = new Entity(true); // For GM Tracking

                    SetStats();
                    if (pStat[0] != 0)   {
                        LogWrite("      Stat tracking ready");
                        StatTrackTick.Start();
                    }
                    else
                        LogWrite("      Could not start stat tracking");
                    
                    LogWrite("      Target tracking enabled");
                    TargetTick.Start();
                }
            }

            LoadConfig();

            TargetHP.Text = string.Empty;
            TargetHPMax.Text = string.Empty;
            StatsLabel.Text = string.Empty;
            label1.Hide();
            label6.Hide();
            label7.Hide();
        }

        public void LoadConfig()
        {
            if (System.IO.File.Exists(Application.StartupPath + "\\Config.ini"))
            {
                LogWrite("      Loaded Config.ini");
                foreach (string Line in System.IO.File.ReadAllLines(Application.StartupPath + "\\Config.ini"))
                {
                    if (Line != "")
                    {
                        string[] result = Line.Split('=');
                        switch (result[0])
                        {
                            case "EntityTypeID":
                                if (result[1] != string.Empty)
                                {
                                    string[] EntityTypes = result[1].Split(',');
                                    for (int i = 0; i < EntityTypes.Count(); i++)
                                    {
                                        EntityType.Add(EntityTypes[i]);
                                        //LogWrite(i.ToString() + " " + EntityTypes[i]);
                                    }
                                    LogWrite("      Set entity Type ID: " + result[1]);
                                }
                                break;
                            // Crafting
                            case "WarehouseSlotOne":
                                CraftForm.WhSlotOne.Text = result[1];
                                break;
                            case "WarehouseSlotTwo":
                                CraftForm.WhSlotTwo.Text = result[1];
                                break;
                            case "WarehouseSlotThree":
                                CraftForm.WhSlotThree.Text = result[1];
                                break;
                            case "WarehouseSlotFour":
                                CraftForm.WhSlotFour.Text = result[1];
                                break;
                            case "CustomInputOne":
                                CraftForm.CustomInputOne.Text = result[1];
                                break;
                            case "CustomInputTwo":
                                CraftForm.CustomInputTwo.Text = result[1];
                                break;
                            case "CustomInputThree":
                                CraftForm.CustomInputThree.Text = result[1];
                                break;
                            case "CustomInputFour":
                                CraftForm.CustomInputFour.Text = result[1];
                                break;

                            // Healing
                            case "Heal":
                                AdvancedForm.BotHeal.Checked = bool.Parse(result[1]);
                                break;
                            case "HealInCombat":
                                AdvancedForm.HealInCombat.Checked = bool.Parse(result[1]);
                                break;
                            case "Heal%":
                                AdvancedForm.BotHealPercent.Text = result[1];
                                break;
                            case "HealSlot":
                                AdvancedForm.HealFKey.Text = result[1];
                                break;

                            // Pots
                            case "potHP%":
                                PotHealthPercent.Text = result[1];
                                break;
                            case "potMP%":
                                PotManaPercent.Text = result[1];
                                break;
                            case "StopAfter":
                                AdvancedForm.StopBotAfter.Checked = bool.Parse(result[1]);
                                break;
                            case "StopAfterValue":
                                AdvancedForm.StopAfterValue.Text = result[1];
                                break;
                            case "AttackTimeOut":
                                AdvancedForm.AttackTimeOut.Text = result[1];
                                break;

                            // Aura
                            case "Aura":
                                AdvancedForm.BotAura.Checked = bool.Parse(result[1]);
                                //BotAura.Checked = bool.Parse(result[1]);
                                break;
                            case "AuraDelay":
                                AdvancedForm.AuraDelay.Text = result[1];
                                break;
                        }
                    }
                }
            }
            else
            {
                LogWrite("      No Config.ini found");
            }
        }
        
        public void hook_KeyPressed(object sender, KeyPressedEventArgs e)
        {
            //LogWrite(e.Modifier.ToString() + " + " + e.Key.ToString());
            switch (e.Key)
            {
                case Keys.D:
                    if (e.Modifier == ModifierKeysEx.Alt)
                    {
                        if (Logger.Size.Height >= 500)
                        {
                            Logger.Size = new Size(Logger.Size.Width, 238);
                            TargetBox.Show();
                            ClickBox.Show();
                            PotBox.Show();
                        }
                        else
                        {
                            //LogWrite("Debug");
                            TargetBox.Hide();
                            ClickBox.Hide();
                            PotBox.Hide();
                            Logger.Size = new Size(Logger.Size.Width, 520);
                        }
                    }
                    break;
                case Keys.S:
                    if (e.Modifier == ModifierKeysEx.Alt)
                    {
                        using (System.IO.TextWriter log = new System.IO.StreamWriter(Application.StartupPath + "\\Out.log"))
                        {
                            LogWrite("Saving log", true);
                            for (int i = 0; i < Logger.Items.Count; i++)
                            {
                                log.WriteLine(Logger.Items[i]);
                            }
                            Logger.Items.Clear();
                        }
                    }
                    break;
                case Keys.I:
                    if (e.Modifier == ModifierKeysEx.Alt)
                    {                   
                        if (LevelingInt)
                        {
                            LogWrite("Disabled int hunting", true);
                            LevelingInt = false;
                        }
                        else
                        {
                            if (myPlayer.GetSpellKey() == -1)
                            {
                                LogWrite("No spell indexed", true);
                                LogWrite("  Select a spell first");
                                return;
                            }
                            else
                            {
                                if (myPlayer.GetSpellPower(myPlayer.GetSpellKey()) > 2)
                                {
                                    AttackSpellKey = myPlayer.GetSpellKey();
                                    MaximumSpellRange = myPlayer.GetSpellDistance(AttackSpellKey);
                                    //LogWrite("      Spell: " + myPlayer.GetSpellName(AttackSpellKey), true);
                                    //LogWrite("      Power: " + myPlayer.GetSpellPower(AttackSpellKey));
                                    //LogWrite("      Range: " + MaximumSpellRange);
                                    //LogWrite("      MP Usage: " + myPlayer.GetSpellUsageMP(AttackSpellKey));
                                    LogWrite("Spell List", true);
                                    LogWrite("  " + myPlayer.GetSpellName(AttackSpellKey));
                                    if (AdvancedForm.ShowDebug.Checked)
                                    {
                                        LogWrite("      Key: " + AttackSpellKey);
                                        LogWrite("      Power: " + myPlayer.GetSpellPower(AttackSpellKey));
                                        LogWrite("      Range: " + MaximumSpellRange);
                                        LogWrite("      MP Usage: " + myPlayer.GetSpellUsageMP(AttackSpellKey));
                                    }

                                    // Detect the users spells (Heal, Weakening, Aura)
                                    bool WeakenSet = false, HealSet = false, AuraSet = false;
                                    for (int i = 0; i < 12; i++)
                                    {
                                        if (HealSet == false)
                                        {
                                            if (myPlayer.GetSpellName(i).Contains("Healing"))
                                            {
                                                HealSpellKey = i;
                                                //LogWrite("Healing detected  [Key: " + HealSpellKey + "]");
                                                LogWrite("  " + myPlayer.GetSpellName(HealSpellKey));
                                                if (AdvancedForm.ShowDebug.Checked)
                                                    LogWrite("      Key: " + HealSpellKey);
                                                HealSet = true;
                                            }
                                        }
                                        if (AuraSet == false)
                                        {
                                            if (myPlayer.GetSpellName(i).Contains("MP Saver"))
                                            {
                                                AuraSpellKey = i;
                                                LogWrite("  " + myPlayer.GetSpellName(AuraSpellKey));
                                                if (AdvancedForm.ShowDebug.Checked)
                                                    LogWrite("      Key: " + AuraSpellKey);
                                                AuraSet = true;
                                            }
                                            if (myPlayer.GetSpellName(i).Contains("Rapid MP Recovery"))
                                            {
                                                AuraSpellKey = i;
                                                LogWrite("  " + myPlayer.GetSpellName(AuraSpellKey));
                                                if (AdvancedForm.ShowDebug.Checked) 
                                                    LogWrite("      Key: " + AuraSpellKey);
                                                AuraSet = true;
                                            }  
                                        }
                                        if (WeakenSet == false)
                                        {
                                            if (myPlayer.GetSpellName(i).Contains("Weakening"))
                                            {
                                                WeakenSpellKey = i;
                                                MaximumSpellRange = myPlayer.GetSpellDistance(WeakenSpellKey);
                                                LogWrite("  " + myPlayer.GetSpellName(WeakenSpellKey));
                                                if (AdvancedForm.ShowDebug.Checked)
                                                {
                                                    LogWrite("      Key: " + WeakenSpellKey);
                                                    LogWrite("      New range: " + MaximumSpellRange);
                                                }
                                                WeakenSet = true;
                                            }
                                        }
                                    }

                                    // Verify the user has the necessary spells
                                    if (HealSet == false)
                                    {
                                        LogWrite("No healing spell detected", true);
                                        return;
                                    }
                                    LogWrite("Int hunting successfully enabled", true);
                                    LevelingInt = true;
                                }
                                else
                                {
                                    LogWrite("Selected spell is not attack spell", true);
                                }
                            }                     
                        }
                    }
                    break;
                case Keys.C:
                    if (e.Modifier == ModifierKeysEx.Alt)
                    {
                        if (Craft)
                        {
                            if (!CraftForm.Visible)
                                CraftForm.ShowDialog();
                            else
                                CraftForm.Visible = false;
                        }
                        else
                        {
                            if (!AdvancedForm.Visible)
                                AdvancedForm.ShowDialog();
                            else
                                AdvancedForm.Visible = false;
                        }
                    }
                    break;
                case Keys.Home:
                    Waypoint_X.Add(myPlayer.XLocation());
                    Waypoint_Y.Add(myPlayer.YLocation());
                    LogWrite("Added WP @ X: " + Waypoint_X[Waypoint_X.Count - 1].ToString() + "    Y: " + Waypoint_Y[Waypoint_X.Count - 1].ToString(), true);
                    break;
                case Keys.PageDown:
                    somaGame.GiveFocus();
                    string ItemName = System.IO.Path.GetRandomFileName();
                    ItemName = ItemName.Replace(".", "");
                    int rNum = ReturnRandomNumber(1, ItemName.Length);
                    ItemName = ItemName.Remove(0, rNum);

                    LogWrite(ItemName);
                    break;
                case Keys.End:
                    if (e.Modifier == ModifierKeysEx.Alt)
                    {
                        if (TargetTick.Enabled)
                            ToggleTimer("TargetTracker");
                        if (StatTrackTick.Enabled)
                            StatTrackTick.Stop();

                        LogWrite("Killing client: " + Game.hWndParent.ToString("X"), true);
                        somaGame.Kill();

                        if (this.Size.Width >= 1500)
                        {
                            this.Size = new System.Drawing.Size(250, 750);
                        }
                    }
                    else
                    {
                        Application.Exit();
                    }
                    break;
                case Keys.Insert:
                    if (e.Modifier == ModifierKeysEx.Alt)
                    {
                        LoadGame.PerformClick();
                    }
                    else
                    {
                        if (Craft)
                        {
                            if ( (CraftForm.WhSlotOne.Text != string.Empty || CraftForm.CustomInputOne.Text != string.Empty) || CraftForm.Restock)
                            {
                                if (!Craftbot.IsBusy)
                                {
                                    BottingBox.Text = " Crafting ";
                                    BottingBox.ForeColor = Color.WhiteSmoke;
                                    foreach (Control c in BottingBox.Controls)
                                    {
                                        c.ForeColor = Color.Gray;
                                    }
                                    Botting = true;
                                    Craftbot.RunWorkerAsync();
                                }
                                else
                                {
                                    BottingBox.Text = " Bot ";
                                    BottingBox.ForeColor = Color.Gray;
                                    LogWrite("Sending kill signal to craft bot");
                                    LogWrite("  Waiting for thread to close...");
                                    Craftbot.CancelAsync();
                                }
                            }
                            else
                            {
                                LogWrite("Declare crafting slots first", true);
                            }
                        }
                        else
                        {
                            if (!Bot.IsBusy)
                            {
                                LogWrite("Bot starting...", true);
                                if (EntityType.Count == 0)
                                {
                                    LogWrite("      Entity Type ID not set");
                                    return;
                                }
                                if (Waypoint_X.Count == 0)
                                {
                                    LogWrite("      No waypoints set");
                                    return;
                                }
                                if (bool.Parse(AdvancedForm.StopBotAfter.Checked.ToString()))
                                {
                                    if (AdvancedForm.StopAfterValue.Text != string.Empty)
                                    {
                                        LogWrite("      Will stop botting after " + AdvancedForm.StopAfterValue.Text + "minutes");
                                    }
                                    else
                                    {
                                        LogWrite("      Set a value to stop botting after!");
                                        return;
                                    }
                                }
                                if (myPlayer.Stance() != "Battle")
                                {
                                    LogWrite("      " + myPlayer.Stance() + " mode detected");
                                    LogWrite("          Changing stance to battle mode");
                                    myPlayer.SetStance("Battle");
                                }
                                if (!playerBelt.HasTP())
                                {
                                    LogWrite("      No TP found on belt!");
                                    LogWrite("          Will quicklog instead");
                                }
                                else
                                {
                                    LogWrite("      TP on: F" + playerBelt.FKeyTP.ToString());
                                }
                                if (LevelingInt)
                                {
                                    if (!playerBelt.HasMP())
                                    {
                                        LogWrite("      No MP potions found on belt!");
                                        return;
                                    }
                                    else
                                    {
                                        LogWrite("      MP pots on: F" + playerBelt.FKeyHP.ToString());
                                        if (!PotTick.Enabled)
                                        {
                                            ToggleTimer("Pot");
                                        }
                                    }
                                }
                                else
                                {
                                    if (!playerBelt.HasHP())
                                    {
                                        LogWrite("      No HP potions found on belt!");
                                        LogWrite("          Botting without potions");
                                    }
                                    else
                                    {
                                        LogWrite("      HP pots on: F" + playerBelt.FKeyHP.ToString());
                                        if (!PotTick.Enabled)
                                        {
                                            ToggleTimer("Pot");
                                        }
                                    }
                                }
                                Clock.Start();

                                LogWrite("      Enabling GM detection");
                                FoundAdmin = false;
                                gmDetection.Start();

                                LogWrite("      Botting on zone: " + myPlayer.Zone());
                                BottingBox.Text = " Botting ";
                                BottingBox.ForeColor = Color.WhiteSmoke;
                                foreach (Control c in BottingBox.Controls)
                                {
                                    c.ForeColor = Color.Gray;
                                }
                                Botting = true;
                                Bot.RunWorkerAsync();
                            }
                            else
                            {
                                BottingBox.Text = " Bot ";
                                BottingBox.ForeColor = Color.Gray;
                                LogWrite("Sending kill signal to bot");
                                LogWrite("  Waiting for thread to close...");
                                Bot.CancelAsync();
                            }
                        }
                    }
                    break;
                case Keys.Left:
                    ToggleTimer("Click");
                    break;
                case Keys.Up:
                    ToggleTimer("Pot");
                    break;
            }
        }


        // Bots
        bool Botting = false, LevelingInt = false; int AttackSpellKey, WeakenSpellKey = -1, MaximumSpellRange, HealSpellKey, AuraSpellKey;
        private void Bot_DoWork(object sender, DoWorkEventArgs e)
        {
            int HealOutOfCombatCount = 0,           // For randomized healing when no new entities have been found for a while
                DetectEntiyFailureCount = 0,        // For counting ScanArray "failures"  
                MoveToWaypointFailureCount = 0,
                HealAttemptsInCombatCount = 0;

            AuraDelay = int.Parse(AdvancedForm.AuraDelay.Text);

            string CurrentZone = myPlayer.Zone();

            bool TimeToWarp = false,
                 PlayerDead = false,
                 BeingAttacked = false,
                 Pause = true;

            if (!bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                somaGame.ToggleUI("Hide");

            while (!Bot.CancellationPending)
            {
                switch (ScanArray(true, false))
                {
                    case "PK":
                        if (bool.Parse(AdvancedForm.IgnorePlayerKillers.Checked.ToString()) == false)
                        {
                            ScreenShot("PK.png");
                            TimeToWarp = true;
                        }
                        goto case "End";

                    case "Ambush":
                        if (bool.Parse(AdvancedForm.BotHeal.Checked.ToString()))
                        {
                            if (bool.Parse(AdvancedForm.HealInCombat.Checked.ToString()))
                            {
                                if (myPlayer.CurrentHPBelowPercent(int.Parse(AdvancedForm.BotHealPercent.Text)))
                                {
                                    RunHeal("Ambush");
                                }
                            }
                        }
                        if (LevelingInt)
                            goto case "Int";
                        else
                            goto case "Melee";

                    case "Entity Found":
                        if (bool.Parse(AdvancedForm.BotHeal.Checked.ToString()))
                        {
                            if (bool.Parse(AdvancedForm.BeforeCombat.Checked.ToString()))
                            {
                                int chkHP = int.Parse(AdvancedForm.BotHealPercent.Text) + 25;
                                if (chkHP >= 85)
                                    chkHP = int.Parse(AdvancedForm.BotHealPercent.Text);

                                if (myPlayer.CurrentHPBelowPercent(chkHP))
                                {
                                    RunHeal("Entity Found");
                                    int GoodEntityAddress = somaEntity.ReturnAddress();
                                    if (ScanArray(false, false) == "Ambush")
                                    {
                                        if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                        {
                                            LogWrite("Ambushed after healing", true);
                                            LogWrite(string.Empty);
                                        }
                                    }
                                    else
                                    {
                                        // Monster has begun attacking another player?
                                        if (somaEntity.IsAttacking() == "ATTACKING" && somaEntity.IsInRange(2, 2) && !somaEntity.IsFacingPlayer())
                                        {
                                            if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                                LogWrite("Entity: " + GoodEntityAddress.ToString("X") + " was found to be attacking after healing... Ignoring");
                                            BadEntity.Add(GoodEntityAddress);
                                            goto case "End";
                                        }
                                        somaEntity.SetAddress(GoodEntityAddress);
                                    }
                                }
                            }
                        }
                        if (LevelingInt)
                            goto case "Int";
                        else
                            goto case "Melee";

                    case "Nothing Found":
                        DetectEntiyFailureCount++;
                        if (bool.Parse(AdvancedForm.BotHeal.Checked.ToString()))
                        {
                            if (myPlayer.CurrentHPBelowPercent(int.Parse(AdvancedForm.BotHealPercent.Text)))
                            {
                                RunHeal("Nothing Found");
                            }
                            else
                            {
                                if (DetectEntiyFailureCount == (ReturnRandomNumber(5, 25)))
                                {
                                    int vHeal = int.Parse(AdvancedForm.BotHealPercent.Text);
                                    if (vHeal <= 60)
                                    {
                                        vHeal += 20;
                                    }
                                    if (myPlayer.CurrentHPBelowPercent(vHeal))
                                    {
                                        RunHeal("Nothing Found");
                                        HealOutOfCombatCount++;
                                    }
                                }
                                if (DetectEntiyFailureCount >= 25)
                                {
                                    DetectEntiyFailureCount = 0;
                                }
                            }
                        }
                        int xDistance = 0, yDistance = 0;
                        if ((int)Waypoint_X[CurrentWaypoint] >= myPlayer.XLocation())
                            xDistance = (int)Waypoint_X[CurrentWaypoint] - myPlayer.XLocation();
                        else
                            xDistance = myPlayer.XLocation() - (int)Waypoint_X[CurrentWaypoint];
                        if ((int)Waypoint_Y[CurrentWaypoint] >= myPlayer.YLocation())
                            yDistance = (int)Waypoint_Y[CurrentWaypoint] - myPlayer.YLocation();
                        else
                            yDistance = myPlayer.YLocation() - (int)Waypoint_Y[CurrentWaypoint];
                        if (xDistance >= 20 || yDistance >= 20)
                        {
                            if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                            {
                                LogWrite("Player has wandered", true);
                                LogWrite("  Searching for nearest WP");
                            }
                            FindNearestWP();
                        }
                        LogWrite("Moving to WP @ X: " + Waypoint_X[CurrentWaypoint].ToString() + "  Y: " + Waypoint_Y[CurrentWaypoint].ToString(), true);
                        if (myPlayer.MoveToPoint((int)Waypoint_X[CurrentWaypoint], (int)Waypoint_Y[CurrentWaypoint], 4, 20))
                        {
                            //if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                //LogWrite("  Arrived at point");
                            if (MovingForward)
                                CurrentWaypoint++;
                            else
                                CurrentWaypoint--;
                            VerifyWP(CurrentWaypoint);
                            MoveToWaypointFailureCount = 0;
                            if (!myPlayer.StaminaAbovePercent(10))
                            {
                                if (!myPlayer.HasZonePenalty())
                                {
                                    LogWrite("Getting some stamina", true);
                                    myPlayer.ToggleStamina(false, false);
                                    while (!myPlayer.StaminaAbovePercent(55))
                                    {
                                        Thread.Sleep(500);
                                        if (ScanArray(false, false) == "Ambush")
                                        {
                                            if (LevelingInt)
                                                goto case "Int";
                                            else
                                                goto case "Melee";
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            LogWrite("  Move to WP failed");
                            MoveToWaypointFailureCount++;
                            if (MoveToWaypointFailureCount >= 2)
                            {
                                LogWrite("  Failed moving to WP 2 times");
                                ScreenShot("Stuck.png");
                                TimeToWarp = true;
                            }
                        }
                        goto case "End";

                    case "Int":
                        Pause = true;
                        if (AdvancedForm.ShowDebug.Checked)
                        {
                            //LogWrite("      ePosition: " + somaEntity.Quadrant());
                            //LogWrite("      X: " + somaEntity.XLocation() + "    Y: " + somaEntity.YLocation());
                        }
                        if (!somaEntity.IsInRange(MaximumSpellRange, MaximumSpellRange))
                        {
                            int NewXPos = somaEntity.XLocation(),
                                NewYPos = somaEntity.YLocation(),
                                NewPosRange = ReturnRandomNumber(MaximumSpellRange / 2, MaximumSpellRange);
                                //NewPosRange = MaximumSpellRange;
                                
                            switch (somaEntity.Quadrant().ToLower())
                            {
                                case "top left":
                                    NewXPos += NewPosRange;
                                    NewYPos += NewPosRange;
                                    break;
                                case "top right":
                                    NewXPos -= NewPosRange;
                                    NewYPos += NewPosRange;
                                    break;
                                case "bottom left":
                                    NewXPos += NewPosRange;
                                    NewYPos -= NewPosRange;
                                    break;
                                case "bottom right":
                                    NewXPos -= NewPosRange;
                                    NewYPos -= NewPosRange;
                                    break;
                            }
                            //LogWrite("          Moving to: " + NewXPos + "    Y: " + NewYPos);
                            myPlayer.MoveToPoint(NewXPos, NewYPos, 2, 20);
                        }
                        while (myPlayer.IsMoving())
                        {
                            //if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))  LogWrite("      Waiting for player to stop moving");
                            Thread.Sleep(200);
                        }
                        Thread.Sleep(500);
                        myPlayer.ToggleStamina(false, false);
                    RedoInt:

                        // Attempt to weaken
                        myPlayer.SetSpellKey(WeakenSpellKey);
                        bool CastingWeaken = false; 
                        int WeakenTimeOut = 0;
                        TimeOut = 0;

                        // But only if the player has it...
                        if (WeakenSpellKey != -1)
                        {
                            LogWrite("      Weakening", true);
                            while (!somaEntity.IsWeakened())
                            {
                                if (!CastingWeaken)
                                {
                                    if (somaMouse.MousePTR() != somaEntity.ID())
                                    {
                                        if (somaMouse.FoundEntity())
                                        {
                                            for (int i = 0; i < 3; i++)
                                            {
                                                Thread.Sleep(100);
                                                somaMouse.SimulateRightClick();
                                            }
                                            CastingWeaken = true;
                                        }
                                    }
                                    else
                                    {
                                        somaMouse.SimulateRightClick();
                                        CastingWeaken = true;
                                    }
                                }
                                else
                                {
                                    Thread.Sleep(100);
                                    WeakenTimeOut++;    // 2800ms total cast?
                                    if (WeakenTimeOut >= 34)
                                    {
                                        CastingWeaken = false;
                                        WeakenTimeOut = 0;
                                    }
                                }  
                                //if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                    //LogWrite("      Timeout: " + TimeOut.ToString());
                                if (TimeOut >= (int.Parse(AdvancedForm.AttackTimeOut.Text)))
                                {
                                    LogWrite("      Ignoring entity: " + somaEntity.ReturnAddress().ToString("X"));
                                    BadEntity.Add(somaEntity.ID());
                                    goto case "End";
                                }
                                if (Bot.CancellationPending)
                                {
                                    goto case "Quit";
                                }
                            }
                        }

                        // Begin attacking
                        if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                            LogWrite("      Attacking");
                        
                        myPlayer.SetSpellKey(AttackSpellKey);
                        while (!somaEntity.IsDead())
                        {
                            if (FoundAdmin)
                            {
                                TimeToWarp = true;
                                goto case "End";
                            }
                            if (somaMouse.MousePTR() != somaEntity.ID())
                            {
                                if (somaMouse.FoundEntity())
                                {
                                    somaMouse.SimulateRightClick();
                                }
                            }
                            else
                            {
                                somaMouse.SimulateRightClick();
                            }
                            if (!somaEntity.HealthBelowPercent(25))
                            {
                                if (bool.Parse(AdvancedForm.HealInCombat.Checked.ToString()))
                                {
                                    if (bool.Parse(AdvancedForm.BotHeal.Checked.ToString()))
                                    {
                                        if (myPlayer.CurrentHPBelowPercent(int.Parse(AdvancedForm.BotHealPercent.Text)))
                                        {
                                            RunHeal("Ambush");
                                            goto RedoInt;
                                        }
                                    }
                                }
                            }
                            if (bool.Parse(AdvancedForm.BotAura.Checked.ToString()))
                            {
                                if (AuraDelay >= int.Parse(AdvancedForm.AuraDelay.Text))
                                {
                                    myPlayer.SetSpellKey(AuraSpellKey);
                                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                        LogWrite("      Casting aura");
                                    for (int i = 0; i < 5; i++)
                                    {
                                        Thread.Sleep(200);
                                        somaMouse.SimulateRightClick();
                                    }
                                    AuraDelay = 0;
                                    myPlayer.SetSpellKey(AttackSpellKey);
                                }
                            }
                            if (somaEntity.HealthBelowPercent(75))
                            {
                                PlayerDrops.GetTotalDrops();
                            }
                            if (!somaEntity.HealthBelowPercent(20))
                            {
                                if (WeakenSpellKey != -1)
                                {
                                    if (!somaEntity.IsWeakened())
                                    {
                                        goto RedoInt;
                                    }
                                }
                            }
                            if (!playerBelt.HasMP())
                            {
                                TimeToWarp = true;
                                goto case "End";
                            }
                            if (Bot.CancellationPending)
                            {
                                goto case "Quit";
                            }
                            Thread.Sleep(200);
                        }
                        LogWrite("      Target dead");
                        myPlayer.ToggleStamina(true, false);
                        goto case "Loot";

                    case "Melee":
                        HealAttemptsInCombatCount = 0;
                        Pause = true;
                        LogWrite("      Auto attacking", true);
                    RedoMelee:
                        TimeOut = 0;
                        LogWrite("  Here", true);
                        myPlayer.ResetAutoAttack();
                        LogWrite("  Here1", true);
                        while (!myPlayer.IsAutoAttacking())
                        {
                            LogWrite("  Here2", true);
                            if (somaEntity.IsDead())
                            {
                                goto case "End";
                            }
                            LogWrite("  Here3", true);
                            if (myPlayer.IsDead())
                            {
                                PlayerDead = true;
                                goto case "End";
                            }
                            LogWrite("  Here4", true);
                            if (!myPlayer.IsMoving())
                            {
                                if (somaMouse.FoundEntity())
                                {
                                    //somaMouse.SimulateLeftClick();
                                }
                            }

                            //if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                            //{
                            //    LogWrite("      Timeout: " + TimeOut.ToString());
                            //}
                            //if (TimeOut >= (int.Parse(AdvancedForm.AttackTimeOut.Text)))
                            //{
                            //    LogWrite("      Ignoring entity: " + somaEntity.ReturnAddress().ToString("X"));
                            //    BadEntity.Add(somaEntity.ID());                   
                            //    goto case "End";
                            //}
                            //if (Bot.CancellationPending)
                            //    goto case "Quit";
                            Thread.Sleep(250);
                        }
                        myPlayer.ToggleStamina(false, false);
                        TimeOut = 0;
                        if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                        {
                            LogWrite("      Auto attack enabled");
                        }
                        while (myPlayer.HasTarget())
                        {
                            if (somaEntity.MaximumHP() == 0)
                            {
                                // Extra time out for bugged mobs or mobs hit through walls etc
                                if (somaEntity.TypeID() == 115 || somaEntity.TypeID() == 87 || somaEntity.TypeID() == 808)
                                {
                                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                        LogWrite("      Timeout Two: " + TimeOut.ToString() + " - 12");
                                    if (TimeOut >= 10)
                                    {
                                        LogWrite("      Ignoring entity: " + somaEntity.ReturnAddress().ToString("X"));
                                        BadEntity.Add(somaEntity.ReturnAddress());
                                        goto case "End";
                                    }
                                }
                            }
                            if (FoundAdmin)
                            {
                                TimeToWarp = true;
                                goto case "End";
                            }
                            if (CurrentZone != myPlayer.Zone())
                            {
                                TimeToWarp = true;
                                goto case "End";
                            }
                            if (myPlayer.IsDead())
                            {
                                PlayerDead = true;
                                goto case "End";
                            }
                            if (bool.Parse(AdvancedForm.EndIfZeroPotions.Checked.ToString()))
                            {
                                if (!playerBelt.HasHP())
                                {
                                    TimeToWarp = true;
                                    goto case "End";
                                }
                            }
                            PlayerDrops.GetTotalDrops();
                            if (bool.Parse(AdvancedForm.BotAura.Checked.ToString()))
                            {
                                if (AuraDelay >= int.Parse(AdvancedForm.AuraDelay.Text))
                                {
                                    somaMouse.FoundEntity();
                                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                        LogWrite("      Casting aura");
                                    for (int i = 0; i < 4; i++)
                                    {
                                        Thread.Sleep(250);
                                        //somaMouse.RightClick();
                                        somaMouse.SimulateRightClick();
                                    }
                                    AuraDelay = 0;
                                }
                            }
                            if (HealAttemptsInCombatCount <= 3)
                            {
                                if (!somaEntity.HealthBelowPercent(25))
                                {
                                    if (bool.Parse(AdvancedForm.HealInCombat.Checked.ToString()))
                                    {
                                        if (bool.Parse(AdvancedForm.BotHeal.Checked.ToString()))
                                        {
                                            if (myPlayer.CurrentHPBelowPercent(int.Parse(AdvancedForm.BotHealPercent.Text)))
                                            {
                                                myPlayer.ResetAutoAttack();
                                                if (!RunHeal("Ambush"))
                                                {
                                                    PlayerDead = true;
                                                    goto case "End";
                                                }
                                                HealAttemptsInCombatCount++;
                                                goto RedoMelee;
                                            }
                                        }
                                    }
                                }
                            }
                            if (Bot.CancellationPending)
                                goto case "Quit";
                            Thread.Sleep(250);
                        }
                        LogWrite("      Target dead");
                        myPlayer.ToggleStamina(true, false);
                        goto case "Loot";

                    case "Loot":
                        BeingAttacked = false;
                        if (!bool.Parse(AdvancedForm.NoDrops.Checked.ToString()))
                        {
                            if (PlayerDrops.NewDropsFound())
                            {
                                LogWrite("      Target dropped " + PlayerDrops.AmountDropped() + " item(s)");
                                if (myPlayer.HasFreeBW())
                                {
                                    if (ScanArray(false, false) == "Ambush")
                                        BeingAttacked = true;
                                    else
                                        Pause = false;
                                    PlayerDrops.PickUp(BeingAttacked);
                                }
                                else
                                {
                                    LogWrite("      Player has no free BW");
                                }
                            }
                        }
                        goto case "Durability Check";

                    case "Durability Check":
                        if (!LevelingInt)
                        {
                            if (myPlayer.HasWeapon())
                            {
                                if (myPlayer.WeaponDuraLow())
                                {
                                    LogWrite("Weapon dura low", true);
                                    LogWrite("  Searching for a new weapon...");
                                    if (myPlayer.HasNewWeapon())
                                    {
                                        LogWrite("      New weapon found");
                                        LogWrite("      Changing weapon");
                                        myPlayer.ChangeWeapon(false, true);
                                        somaGame.ToggleUI("Hide");
                                        Pause = false;
                                    }
                                    else
                                    {
                                        LogWrite("      No new weapon found");
                                        TimeToWarp = true;
                                        goto case "End";
                                    }
                                }
                            }
                        }
                        string ArmourPeice = myPlayer.ArmourDuraLow();
                        while (ArmourPeice != string.Empty)
                        {
                            LogWrite(ArmourPeice + " dura low", true);

                            if (ArmourPeice == "LRing" || ArmourPeice == "RRing" || ArmourPeice == "Ear" || ArmourPeice == "Neck")
                            {
                                LogWrite("  Removing " + ArmourPeice);
                                if (!myPlayer.RemoveArmour(ArmourPeice))
                                {
                                    LogWrite("          Could not remove " + ArmourPeice);
                                    TimeToWarp = true;
                                    goto case "End";
                                }
                                if (myPlayer.HasNewArmour())
                                {
                                    LogWrite("  Found new " + ArmourPeice);
                                    LogWrite("      Equipping");
                                    if (!myPlayer.ChangeArmour())
                                    {
                                        if (ArmourPeice == "RRing") // Custom condition for when the player has no left ring equipped and the right ring goes into the left slot...
                                        {
                                            //LogWrite("RRing failed");
                                            myPlayer.cArmourIndex = 2312;
                                            if (myPlayer.HasNewArmour())
                                            {
                                                //LogWrite("New ring found though");
                                                if (!myPlayer.ChangeArmour())
                                                {
                                                    LogWrite("          Failed");
                                                }
                                            }
                                        } 
                                    }
                                    else
                                    {
                                        LogWrite("          Failed");
                                    }
                                }
                                else
                                {
                                    LogWrite("  No new " + ArmourPeice + " found");
                                }
                            }
                            else
                            {
                                if (myPlayer.HasNewArmour())
                                {
                                    LogWrite("  Found new " + ArmourPeice.ToLower());
                                    LogWrite("      Changing out");
                                    if (!myPlayer.ChangeArmour())
                                    {
                                        LogWrite("          Failed");
                                        TimeToWarp = true;
                                        goto case "End";
                                    }
                                }
                                else
                                {
                                    LogWrite("  No new " + ArmourPeice.ToLower() + " found");
                                    TimeToWarp = true;
                                    goto case "End";
                                }
                            }
                            ArmourPeice = myPlayer.ArmourDuraLow();
                        }
                        while (myPlayer.WindowOpen("inventory"))
                        {
                            SendKeys.SendWait("{F10}");
                            Thread.Sleep(200);
                        }
                        if (BeingAttacked)
                        {
                            LogWrite("  " + somaEntity.ReturnAddress().ToString("X") + " is attacking player", true);
                            PrintEntityDebug();
                            goto case "Ambush";
                        }
                        if (Pause)
                            Sleep(250, 1000, true);
                        else
                            Thread.Sleep(200);
                        goto case "End";

                    case "End":
                        if (!LevelingInt)
                        {
                            if (bool.Parse(AdvancedForm.EndIfZeroPotions.Checked.ToString()))
                            {
                                if (!playerBelt.HasHP())
                                {
                                    TimeToWarp = true;
                                }
                            }
                        }
                        else
                        {
                            if (!playerBelt.HasMP())
                            {
                                TimeToWarp = true;
                            }
                        }
                        if (bool.Parse(AdvancedForm.StopBotAfter.Checked.ToString()))
                        {
                            if (ClockTick >= int.Parse(AdvancedForm.StopAfterValue.Text) * 60)
                            {
                                LogWrite("Time limited reached");
                                LogWrite("      Shutting down");
                                TimeToWarp = true;
                            }
                        }
                        if (CurrentZone != myPlayer.Zone())
                        {
                            TimeToWarp = true;
                        }
                        if (PlayerDead)
                        {
                            LogWrite("      Player died");
                            Sleep(5000, 9000, true);
                            ScreenShot("Death.png");
                            myPlayer.ResetAutoAttack();
                            LogWrite("          Reviving");
                            myPlayer.SendRevive();
                            if (bool.Parse(AdvancedForm.NoWarpOnDeath.Checked.ToString()))
                            {
                                TimeToWarp = false;
                                PlayerDead = false;
                                myPlayer.ToggleStamina(true, false);
                            }
                            else
                            {
                                TimeToWarp = true;
                                PlayerDead = false;
                            }
                        }
                        if (TimeToWarp)
                        {
                            LogWrite("      Attempting to warp...");
                            Sleep(200, 500, false);
                            //if (FoundAdmin)
                            //{
                            //    if (playerBelt.HasTP())
                            //    {
                            //        LogWrite("          Warping");
                            //        playerBelt.UseTP();
                            //    }
                            //}
                            //else
                            //{
                                SoundAlert();
                                if (playerBelt.HasTP())
                                {
                                    LogWrite("          Warping");
                                    playerBelt.UseTP();
                                    Sleep(5000, 10000, true);
                                    LogWrite("          Killing client");
                                }
                                else
                                {
                                    LogWrite("          Failed finding a TP");
                                    Thread.Sleep(500);
                                    LogWrite("          Killing client instead");
                                }
                                somaGame.Kill();
                            //}
                            Bot.CancelAsync();
                        }
                        break;

                    case "Quit":
                        break;
                }
                Thread.Sleep(10);
                if (Bot.CancellationPending)
                    break;
            }
            LogWrite("Bot ended");
            somaGame.ToggleUI("Show");
            if (PotTick.Enabled)
                ToggleTimer("Pot");
            if (Clock.Enabled)
                ToggleTimer("Clock");
            Botting = false;
            if (!Bot.CancellationPending)
            {
                if (bool.Parse(AdvancedForm.ShutdownPC.Checked.ToString()))
                {
                    Process.Start("shutdown", "/s /t 0");
                }
            }
        }

        string PreviousPosition = "Craft";
        private void Craftbot_DoWork(object sender, DoWorkEventArgs e)
        {
            int InventoryDepositPosition = 1,      // Position to select in the inventory when selling items 
                MaterialThreshold = 2,             // Amount of crafting material to leave in the inventory when finished crafting (Prevents issues with selling items)
                SuccessCount = 0,                  // Variable for counting the amount of successful runs
                MaterialContintueAmount = 0,       // Minimum amount of mats needed to continue after vendoring
                FailureCount = 0,
                ShopRestockPosition = CraftForm.ShopRestockPosition;           // Position of the material in the shop for restocking

            bool Sell = false,
                 FryingPan = false,
                 Restock = CraftForm.Restock;

            if (CraftForm.CraftCook.Checked && CraftForm.WhSlotThree.Text != string.Empty && CraftForm.WhSlotFour.Text == "fry" && CraftForm.CustomInputThree.Text == string.Empty)
            {
               LogWrite("Unlocking frying pan", true); PreviousPosition = "Frying Pan"; FryingPan = true;
            }

            ArrayList gotoX = new ArrayList(); ArrayList gotoY = new ArrayList();

            int[] vndrVariables = new int[] { 266, 114, 10000 };             // Vendor; X, Y, PTR                                       // Default is the potion lady
            int[] whVariables = new int[] { 279, 135, 11189, 25 };          // Warehouse; X, Y, PTR, TimeOut for MoveToPoint            // Default is the 2nd Warehouse from the potion vendor
            int[] craftMachineVariables = new int[] { 0, 0, 0 };            // Craft objects; X, Y, PTR
            int[] WithDrawAmount = new int[] { 0, 0, 0 };
            string CraftItem = string.Empty;

            if (Restock)
            {
                InventoryDepositPosition = 0;
                if (PreviousPosition == "Craft")
                    PreviousPosition = "Abias Restock";
                whVariables[0] = 95; whVariables[1] = 835; whVariables[2] = 13240;
                if (CraftForm.RestockCopper.Checked)
                {
                    vndrVariables[0] = 93; vndrVariables[1] = 877; vndrVariables[2] = 13238;
                }
                else
                {
                    vndrVariables[0] = 56; vndrVariables[1] = 812; vndrVariables[2] = 13239; 
                }
            }
            else
            {
                if (CraftForm.CraftWeapon.Checked || CraftForm.CraftArmor.Checked)
                {
                    InventoryDepositPosition = 2; Sell = true;
                    whVariables[0] = 306; whVariables[1] = 130; whVariables[2] = 11191;
                    vndrVariables[0] = 340; vndrVariables[1] = 94; vndrVariables[2] = 10011;
                    // Weapon
                    if (CraftForm.CraftWeapon.Checked)
                    {
                        craftMachineVariables[0] = 346; craftMachineVariables[1] = 81; craftMachineVariables[2] = 11322;
                        gotoX.Add(343); gotoY.Add(89); gotoX.Add(344); gotoY.Add(90); gotoX.Add(341); gotoY.Add(87);
                        if (CraftForm.CustomInputOne.Text == "4" && CraftForm.CustomInputTwo.Text == "3" && CraftForm.CustomInputThree.Text == "2")
                        {
                            CraftItem = "Zinc Glove"; MaterialThreshold = 8; MaterialContintueAmount = 80; InventoryDepositPosition = 3;
                        }
                        else
                        {
                            CraftItem = "Daggers"; MaterialThreshold = 9; MaterialContintueAmount = 60;
                        }
                    }
                    // Armour 
                    else if (CraftForm.CraftArmor.Checked)
                    {
                        craftMachineVariables[0] = 326; craftMachineVariables[1] = 69; craftMachineVariables[2] = 11323;
                        gotoX.Add(324); gotoY.Add(76); gotoX.Add(327); gotoY.Add(75); gotoX.Add(329); gotoY.Add(73);
                        if (CraftForm.CustomInputOne.Text == "3" && CraftForm.CustomInputTwo.Text == "3")
                        {
                            CraftItem = "Shaman Shoes"; MaterialThreshold = 9; MaterialContintueAmount = 60;
                        }
                        else
                        {
                            CraftItem = "Cheap Shoes"; MaterialThreshold = 2; MaterialContintueAmount = 30;
                        }
                    }
                }
                // Access
                else if (CraftForm.CraftAccess.Checked)
                {
                    CraftItem = "Ring";
                    InventoryDepositPosition = 1; MaterialThreshold = 9; MaterialContintueAmount = 60; Sell = true;
                    craftMachineVariables[0] = 250; craftMachineVariables[1] = 78; craftMachineVariables[2] = 11324;
                    gotoX.Add(250); gotoY.Add(84); gotoX.Add(249); gotoY.Add(83); gotoX.Add(247); gotoY.Add(81);
                    whVariables[0] = 274; whVariables[1] = 128; whVariables[2] = 10006; whVariables[3] = 20;            // Closest WH to the potion vendor
                    vndrVariables[0] = 247; vndrVariables[1] = 83; vndrVariables[2] = 10001;    // NPC next to the anvil
                }
                // Potion
                else if (CraftForm.CraftPot.Checked)
                {
                    InventoryDepositPosition = 1; MaterialThreshold = 9; MaterialContintueAmount = 60;
                    craftMachineVariables[0] = 266; craftMachineVariables[1] = 106; craftMachineVariables[2] = 11324;
                    gotoX.Add(267); gotoY.Add(105); gotoX.Add(268); gotoY.Add(105); gotoX.Add(269); gotoY.Add(109);
                }
                // Cook
                else if (CraftForm.CraftCook.Checked)
                {
                    if (CraftForm.CustomInputOne.Text == "1" && CraftForm.CustomInputTwo.Text == "1")
                    {
                        CraftItem = "Red Bean Rice Cake";
                    }
                    else if (CraftForm.CustomInputOne.Text == "2" && CraftForm.CustomInputTwo.Text == "1")
                    {
                        CraftItem = "Rice Cake";
                    }
                    else
                    {
                        CraftItem = "JerkeyPile";
                    }
                    if (CraftForm.CustomInputTwo.Text != string.Empty)
                        InventoryDepositPosition = 2;
                    else
                        InventoryDepositPosition = 1;

                    MaterialThreshold = 5;
                    whVariables[0] = 274; whVariables[1] = 128; whVariables[2] = 10006; whVariables[3] = 20;            // Closest WH to the potion vendor
                    craftMachineVariables[0] = 244; craftMachineVariables[1] = 100; craftMachineVariables[2] = 11326;
                    gotoX.Add(243); gotoY.Add(106); gotoX.Add(245); gotoY.Add(106); gotoX.Add(246); gotoY.Add(106);
                }
                // Extraction
                else if (CraftForm.Extraction.Checked)
                {
                    CraftItem = "Extraction"; InventoryDepositPosition = 0;
                    whVariables[0] = 18; whVariables[1] = 48; whVariables[2] = 26490; whVariables[3] = 25;
                }
            }
            somaGame.ToggleUI("Hide");
            if (AdvancedForm.ShowDebug.Checked)
                LogWrite("Previous State: " + PreviousPosition);
        Restart:

            int cIndex = ReturnRandomNumber(0, 3);
            switch (PreviousPosition)
            {
                case "Abias Restock":
                    PreviousPosition = "Vendor";
                    if (Craftbot.CancellationPending)
                        goto case "Quit";

                    LogWrite("Moving to Vendor... ", true);
                    if (myPlayer.MoveToPoint(vndrVariables[0], vndrVariables[1], 4, 15))
                    {
                        LogWrite("Arrived.", "append");
                    }
                    LogWrite("  Opening... ");
                    if (CraftForm.RestockType == "Leather" || CraftForm.RestockType == "Linen")
                    {
                        if (!myPlayer.OpenNPC(vndrVariables[0], vndrVariables[1] - 2, vndrVariables[2], "Vendor", 1))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                    }
                    else
                    {
                        if (!myPlayer.OpenNPC(vndrVariables[0], vndrVariables[1] - 2, vndrVariables[2], "Vendor", 3))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                    }
                    myPlayer.ToggleStamina(false, false);
                    LogWrite("Ok.", "append");
                    LogWrite("Buying... ", true);
                    if (!myPlayer.BuyItem(ShopRestockPosition, myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight()))
                    {
                        LogWrite("Failed.", "append"); goto case "Quit";
                    }
                    SuccessCount++;
                    LogWrite("Ok.", "append");
                    myPlayer.ToggleStamina(true, false);
                    int NewXPostion,
                        NewYPosition;
                    if (CraftForm.RestockType == "Copper")
                    {
                        NewXPostion = ReturnRandomNumber(81, 84);
                        NewYPosition = ReturnRandomNumber(867, 874);
                        if (ReturnRandomNumber(0, 10) <= 5)
                        {
                            NewXPostion = ReturnRandomNumber(102, 104);
                            NewYPosition = ReturnRandomNumber(870, 872);
                        }
                        myPlayer.MoveToPoint(NewXPostion, NewYPosition, 4, whVariables[3]);
                    }
                    else
                    {
                        if (ReturnRandomNumber(0, 10) <= 7)
                        {
                            NewXPostion = ReturnRandomNumber(66, 76);
                            NewYPosition = ReturnRandomNumber(821, 833);
                            myPlayer.MoveToPoint(NewXPostion, NewYPosition, 4, whVariables[3]);
                        } 
                    }
                    somaGame.ToggleUI("Hide");
                    goto case "Warehouse";

                case "Deposit":
                    PreviousPosition = "Deposit";
                    if (Craftbot.CancellationPending)
                        goto case "Quit";

                    LogWrite("Depositing... ", true);
                    if (!myPlayer.Deposit(InventoryDepositPosition, Restock))
                    {
                        LogWrite("Failed.", "append"); goto case "Quit";
                    }
                    else
                    {
                        myPlayer.ToggleStamina(true, false);
                        LogWrite("Done.", "append");
                        if (Restock)
                            goto case "Abias Restock";
                        else
                            goto case "Withdraw";
                    }

                case "Quit":
                    bool End = false;
                    if (Craftbot.CancellationPending)
                    {
                        LogWrite("      Kill signal received");
                        LogWrite("          Closing");
                        End = true;
                    }
                    else
                    {
                        if (!AdvancedForm.ShowDebug.Checked)
                            SoundAlert();
                        FailureCount++;
                        if (FailureCount >= 2)
                        {
                            End = true;
                        }
                    }
                    if (End)
                    {
                        Botting = false;
                        LogWrite("Craft bot ended", true);
                        LogWrite("  Did " + SuccessCount.ToString() + " successful runs");
                        somaGame.ToggleUI("Show");
                        return;
                    }
                    else
                        goto Restart;

                case "Extract":
                    LogWrite("Extracting... ", true);
                    myPlayer.Extract();
                    SuccessCount++;
                    LogWrite("Done.", "append");
                    goto case "Warehouse";

                case "Craft":
                    PreviousPosition = "Craft";
                    if (Craftbot.CancellationPending)
                        goto case "Quit";

                    if (CraftForm.Extraction.Checked)
                        goto case "Extract";

                    LogWrite("Crafting... ", true);
                    myPlayer.Craft(MaterialThreshold);
                    //myPlayer.Craft(999);
                    LogWrite("Done.", "append");
                    SuccessCount++;
                    if (bool.Parse(CraftForm.UseDelay.Checked.ToString()))
                        Sleep(5000, 12500, true);
                    somaGame.ToggleUI("Hide");
                    if (Sell)
                        goto case "Vendor";
                    else if (FryingPan)
                        goto case "Open Warehouse";
                    else
                        goto case "Warehouse";

                case "Vendor":
                    PreviousPosition = "Vendor";
                    if (Craftbot.CancellationPending)
                        goto case "Quit";

                    LogWrite("Moving to Vendor... ", true);
                    if (myPlayer.MoveToPoint(vndrVariables[0], vndrVariables[1], 4, 10))
                    {
                        LogWrite("Arrived.", "append");
                    }
                    LogWrite("  Opening... ");
                    if (!myPlayer.OpenNPC(vndrVariables[0], vndrVariables[1] - 2, vndrVariables[2], "Vendor", 1))
                    {
                        LogWrite("Failed.", "append"); goto case "Quit";
                    }
                    LogWrite("Ok.", "append");
                    myPlayer.SellAllItems(InventoryDepositPosition);
                    if (bool.Parse(CraftForm.UseDelay.Checked.ToString()))
                        Sleep(2500, 7500, true);
                    somaGame.ToggleUI("Hide");
                    if (myPlayer.StillHasMats(MaterialContintueAmount))
                        goto case "Move";
                    else
                        goto case "Warehouse";

                case "Warehouse":
                    PreviousPosition = "Warehouse";
                    if (Craftbot.CancellationPending)
                        goto case "Quit";

                    LogWrite("Moving to Warehouse... ", true);
                    if (myPlayer.MoveToPoint(whVariables[0], whVariables[1], 4, whVariables[3]))
                    {
                        LogWrite("Arrived.", "append");
                    }
                    else
                    {
                        LogWrite("Failed.", "append"); goto case "Quit";
                    }
                    goto case "Open Warehouse";

                case "Open Warehouse":
                    myPlayer.ToggleStamina(false, false);
                    LogWrite("  Opening... ");
                    if (CraftForm.Extraction.Checked)
                    {
                        if (!myPlayer.OpenNPC(whVariables[0], whVariables[1] - 2, whVariables[2], "Warehouse", 2))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                    }
                    else
                    {
                        if (!myPlayer.OpenNPC(whVariables[0], whVariables[1] - 2, whVariables[2], "Warehouse"))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                    }
                    LogWrite("Ok.", "append");
                    if (CraftForm.CraftCook.Checked || CraftForm.CraftPot.Checked || CraftForm.Extraction.Checked || Restock)
                        goto case "Deposit";
                    else
                        goto case "Withdraw";

                case "Withdraw":
                    PreviousPosition = "Withdraw";
                    LogWrite("Withdrawing... ", true);
                    switch (CraftItem)
                    {
                        // Cooking
                        case "Red Bean Rice Cake":
                            WithDrawAmount[0] = (myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 10) / 2;
                            WithDrawAmount[1] = WithDrawAmount[0];
                            break;
                        case "Rice Cake":
                            WithDrawAmount[0] = (myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 10) / 3 * 2;
                            WithDrawAmount[1] = WithDrawAmount[0] / 2;
                            break;
                        case "JerkeyPile":
                            WithDrawAmount[0] = myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 10;
                            break;

                        // Weapon
                        case "Zinc Glove":
                            WithDrawAmount[0] = ((myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 25) / 9) * 4;
                            WithDrawAmount[1] = (WithDrawAmount[0] / 4) * 3;
                            WithDrawAmount[2] = WithDrawAmount[0] / 2;
                            break;
                        case "Daggers":
                            WithDrawAmount[0] = ((myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 25) / 4) * 3;
                            WithDrawAmount[1] = WithDrawAmount[0] / 3;
                            break;

                        // Armour
                        case "Shaman Shoes":
                            WithDrawAmount[0] = (myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 25) / 2;
                            WithDrawAmount[1] = WithDrawAmount[0];
                            break;
                        case "Cheap Shoes":
                            WithDrawAmount[0] = (myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 25) / 3;
                            WithDrawAmount[1] = WithDrawAmount[0] * 2;
                            break;

                        // Access
                        case "Ring":
                            WithDrawAmount[0] = ((myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight()) - 25);
                            break;

                        // Extraction
                        case "Extraction":
                            WithDrawAmount[0] = (myPlayer.MaximumBagWeight() - myPlayer.CurrentBagWeight() - 2) / 5;
                            break;
                    }
                    if (FryingPan)
                    {
                        if (!myPlayer.WithDraw(int.Parse(CraftForm.WhSlotOne.Text), WithDrawAmount[0], false))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                        if (CraftForm.CustomInputTwo.Text != string.Empty)
                        {
                            if (!myPlayer.WithDraw(int.Parse(CraftForm.WhSlotTwo.Text), WithDrawAmount[1], false))
                            {
                                LogWrite("Failed.", "append"); goto case "Quit";
                            }
                        }
                        if (!myPlayer.WithDraw(int.Parse(CraftForm.WhSlotThree.Text), 1, true))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                        LogWrite("Ok.", "append");
                        goto case "Frying Pan";
                    }
                    else
                    {
                        if (!myPlayer.WithDraw(int.Parse(CraftForm.WhSlotOne.Text), WithDrawAmount[0], CraftForm.WhSlotTwo.Text == string.Empty))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                        if (CraftForm.WhSlotTwo.Text != string.Empty)
                        {
                            if (!myPlayer.WithDraw(int.Parse(CraftForm.WhSlotTwo.Text), WithDrawAmount[1], CraftForm.WhSlotThree.Text == string.Empty))
                            {
                                LogWrite("  Failed", "append"); goto case "Quit";
                            }
                        }
                        if (CraftForm.WhSlotThree.Text != string.Empty)
                        {
                            if (!myPlayer.WithDraw(int.Parse(CraftForm.WhSlotThree.Text), WithDrawAmount[2], true))
                            {
                                LogWrite("  Failed", "append"); goto case "Quit";
                            }
                        }
                    }
                    if (bool.Parse(CraftForm.UseDelay.Checked.ToString()))
                        Sleep(5000, 7500, true);
                    LogWrite("Ok.", "append");
                    myPlayer.ToggleStamina(false, false);
                    goto case "Move";

                case "Frying Pan":
                    PreviousPosition = "Frying Pan";
                    if (myPlayer.HasItem("Frying Pan"))
                    {
                        myPlayer.UseItem("Frying Pan");
                    }
                    else
                    {
                        LogWrite("No frying pan found"); goto case "Quit";
                    }
                    goto case "Input";

                case "Input":
                    if (FryingPan)
                        myPlayer.SetCraftType("Cook");

                    myPlayer.InputCraftMaterial(CraftForm.CustomInputOne.Text, CraftForm.CustomInputTwo.Text, CraftForm.CustomInputThree.Text);

                    if (CraftForm.CraftArmor.Checked || CraftForm.CraftWeapon.Checked || CraftForm.CraftAccess.Checked)
                    {
                        myPlayer.EnterCraftItemName();
                        switch (CraftItem)
                        {
                            case "Cheap Shoes":
                                myPlayer.SetCraftType("Shoes");
                                break;

                            case "Zinc Glove":
                                myPlayer.SetCraftType("Knux");
                                break;
                        }
                    }
                    goto case "Craft";

                case "Open":
                    PreviousPosition = "Open";
                    LogWrite("  Opening... ");
                    if (CraftForm.CraftCook.Checked || CraftForm.CraftWeapon.Checked || CraftForm.CraftAccess.Checked)
                    {
                        if (CraftItem == "Zinc Glove")
                        {
                            if (!myPlayer.OpenNPC(craftMachineVariables[0], craftMachineVariables[1], craftMachineVariables[2], "Craft", 1))
                            {
                                LogWrite("  Failed"); goto case "Quit";
                            }
                        }
                        else if (!myPlayer.OpenNPC(craftMachineVariables[0], craftMachineVariables[1], craftMachineVariables[2], "Craft", 0))
                        {
                            LogWrite("  Failed"); goto case "Quit";
                        }
                    }
                    else
                    {
                        if (!myPlayer.OpenNPC(craftMachineVariables[0], craftMachineVariables[1], craftMachineVariables[2], "Craft"))
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                    }
                    LogWrite("Ok.", "append");
                    goto case "Input";

                case "Move":
                    PreviousPosition = "Move";
                    if (Craftbot.CancellationPending)
                        goto case "Quit";

                    if (CraftForm.Extraction.Checked)
                    {
                        LogWrite("Moving to Extraction NPC... ", true);
                        if (myPlayer.MoveToPoint(18, 34, 2, 10))
                        {
                            LogWrite("Arrived.", "append");
                        }
                        else
                        {
                            LogWrite("Failed.", "append");
                            goto case "Quit";
                        }
                        goto case "Craft";
                    }
                    else
                    {
                        LogWrite("Running to craft spot... ", true);
                        if (myPlayer.MoveToPoint((int)gotoX[cIndex], (int)gotoY[cIndex], 3, 25))
                        {
                            LogWrite("Arrived.", "append");
                        }
                        else
                        {
                            LogWrite("Failed.", "append"); goto case "Quit";
                        }
                    }
                    goto case "Open";
            }
        }


        // Functions
        public void SoundAlert()
        {
            SoundPlayer sndplayr = new SoundPlayer(Properties.Resources.alert);
            sndplayr.Play();
        }
   
        public bool RunHeal(string iState)
        {
            if (!myPlayer.CurrentMPBelowPercent(10))
            {
                switch (iState.ToLower())
                {
                    case "entity found":
                        LogWrite("Healing before engaging", true);
                        break;
                    case "ambush":
                        LogWrite("Healing while engaged", true);
                        break;
                    case "nothing found":
                        LogWrite("Attempting to heal", true);
                        break;
                }
                if (!myPlayer.HasWeapon())
                {
                    LogWrite("  Running");
                    Sleep(100, 300, false);
                    myPlayer.Heal();
                    LogWrite("      Done");
                }
                else if (myPlayer.WeaponType() == "Staff")
                {
                    LogWrite("  Running");
                    if (LevelingInt)
                        myPlayer.Heal(HealSpellKey);
                    else
                        myPlayer.Heal(int.Parse(AdvancedForm.HealFKey.Text));
                    LogWrite("      Done");
                }
                else
                {
                    if (myPlayer.HasItem("Staff"))
                    {
                        bool UsingShield = myPlayer.HasShield();
                        if (UsingShield)
                            myPlayer.RemoveArmour("Shield");
                        LogWrite("  Switching to staff");
                        myPlayer.ChangeWeapon(true, true);
                        LogWrite("      Running");
                        if (myPlayer.Heal(int.Parse(AdvancedForm.HealFKey.Text)))
                            LogWrite("          Done");
                        else
                            return false;
                        myPlayer.ChangeWeapon(true, !UsingShield);
                        if (UsingShield)
                            myPlayer.EquipShield();                       
                        AuraDelay = int.Parse(AdvancedForm.AuraDelay.Text);
                        somaGame.ToggleUI("Hide");
                    }
                    else
                    {
                        LogWrite("No staff found");
                    }
                }
            }
            else
            {
                LogWrite("Not enough mana to heal", true);
            }
            return true;
        }

        public void Sleep(int Min, int Max, bool PrintTime)
        {
            var rVar = new Random();
            int Time = rVar.Next(Min, Max);
            if (PrintTime)
                LogWrite("      Waiting for: " + Time.ToString() + "ms");
            Thread.Sleep(Time);
        }

        public void FindNearestWP()
        {
            int IterateDistance = 0;
            while (IterateDistance != 60)
            {
                for (int i = 1; i < Waypoint_X.Count; i++)
                {
                    // If both X and Y are minus
                    if ((myPlayer.XLocation() - (int)Waypoint_X[i]) <= 0 && (myPlayer.YLocation() - (int)Waypoint_Y[i]) <= 0)
                    {
                        // Convert both X and Y to positive and check range
                        if (((myPlayer.XLocation() - (int)Waypoint_X[i]) * -1) <= IterateDistance && ((myPlayer.YLocation() - (int)Waypoint_Y[i]) * -1) <= IterateDistance)
                        {
                            VerifyWP(i);
                            return;
                        }
                    }
                    // If X is minus but Y isn't
                    else if ((myPlayer.XLocation() - (int)Waypoint_X[i]) < 0 && (myPlayer.YLocation() - (int)Waypoint_Y[i]) >= 0)
                    {
                        // Convert X to positive and check range
                        if (((myPlayer.XLocation() - (int)Waypoint_X[i]) * -1) <= IterateDistance && (myPlayer.YLocation() - (int)Waypoint_Y[i]) <= IterateDistance)
                        {
                            VerifyWP(i);
                            return;
                        }
                    }
                    // If Y is minus but X isn't
                    else if ((myPlayer.XLocation() - (int)Waypoint_X[i]) >= 0 && (myPlayer.YLocation() - (int)Waypoint_Y[i]) < 0)
                    {
                        // Convert Y to positive and check range
                        if ((myPlayer.XLocation() - (int)Waypoint_X[i]) <= IterateDistance && ((myPlayer.YLocation() - (int)Waypoint_Y[i]) * -1) <= IterateDistance)
                        {
                            VerifyWP(i);
                            return;
                        }
                    }
                    // If X and Y aren't minus
                    else if ((myPlayer.XLocation() - (int)Waypoint_X[i]) >= 0 && (myPlayer.YLocation() - (int)Waypoint_Y[i]) >= 0)
                    {
                        // Check range
                        if ((myPlayer.XLocation() - (int)Waypoint_X[i]) <= IterateDistance && (myPlayer.YLocation() - (int)Waypoint_Y[i]) <= IterateDistance)
                        {
                            VerifyWP(i);
                            return;
                        }
                    }
                }
                IterateDistance++;
            }
        }

        bool MovingForward = true;
        public void VerifyWP(int NewWaypoint)
        {
            CurrentWaypoint = NewWaypoint;
            if (CurrentWaypoint == Waypoint_X.Count)
            {
                if (AdvancedForm.BreadcrumbWP.Checked)
                {
                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                    {
                        LogWrite("Current Waypoint is equal to WP array size");
                        LogWrite("Iterating backwards");
                    }
                    CurrentWaypoint -= 2;
                    MovingForward = false;        
                }
                else
                {
                    CurrentWaypoint = 0;
                }
                LogWrite("Flushing the blacklist", true);
                for (int i = 0; i < BadEntity.Count; i++)
                {
                    BadEntity[i] = 0;
                }
            }
            else if (CurrentWaypoint == 0)
            {
                if (AdvancedForm.BreadcrumbWP.Checked)
                {
                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                    {
                        LogWrite("CurrentWaypoint is 0");
                        LogWrite("Iterating forwards");
                    }
                    MovingForward = true;
                }
                if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                    LogWrite("Flushing the blacklist");

                for (int i = 0; i < BadEntity.Count; i++)
                {
                    BadEntity[i] = 0;
                }
            }
            //if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                //LogWrite("      Next WP @ X: " + Waypoint_X[CurrentWaypoint].ToString() + "  Y: " + Waypoint_Y[CurrentWaypoint].ToString());
        }

        public void PrintEntityDebug()
        {
            LogWrite("  Debug");
            LogWrite("      cHP: " + somaEntity.CurrentHP() + "     mHP: " + somaEntity.MaximumHP());
            LogWrite("      Motion: " + somaEntity.Motion());
            LogWrite("      RunSpeed: " + somaEntity.RunSpeed());
            LogWrite("      Quadrant: " + somaEntity.Quadrant());
            LogWrite("      Facing: " + somaEntity.DirectionFacing());
            LogWrite("      Facing player: " + somaEntity.IsFacingPlayer());
            LogWrite("      Weakened: " + somaEntity.IsWeakened());
            LogWrite("      Action: " + somaEntity.Action(false) + "    Previous: " + somaEntity.Action(true));
            int[] Range = somaEntity.ReturnEntityRange();
            LogWrite("      Range:  X: " + Range[0].ToString() + "  Y: " + Range[1].ToString() + " C: " + (Range[0] + Range[1]).ToString());
            //LogWrite("      In Range: " + somaEntity.IsWithinRange(2));
            bool AttackingPlayer = (somaEntity.IsWithinRange(2) && somaEntity.IsFacingPlayer());
            LogWrite("      Attacking Player: " + AttackingPlayer.ToString());
            bool BlackListed = false;
            for (int i = 0; i < BadEntity.Count; i++)
            {
                if (Convert.ToInt32(BadEntity[i]) == somaEntity.ID()) 
                {
                    BlackListed = true;
                }
            }
            LogWrite("      Blacklisted: " + BlackListed.ToString());
            LogWrite(string.Empty);
        }

        public string ScanArray(bool ShowOutput, bool ReturnAmbushNumber)
        {
            //somaEntity = new Entity(false);
            ArrayList gMonsterAddress = new ArrayList();
            ArrayList gMonsterRangeX = new ArrayList();
            ArrayList gMonsterRangeY = new ArrayList();
            ArrayList gMonsterAttackingAddress = new ArrayList();

            if (ShowOutput)
                LogWrite("Scanning array", true);

            bool Skip = false, CorrectType = false, Blacklisted = false;

            int RangeX = 12, RangeY = 12, BlackListPos = 0;

            for (int IterateMe = 0x278; IterateMe < 0x300; IterateMe += 0x4)
            {
                somaEntity.FindAddress(IterateMe);
                for (int i = 0; i < gMonsterAddress.Count; i++)
                {
                    if ((int)gMonsterAddress[i] == somaEntity.ReturnAddress())  // Skip monsters already found
                    {
                        Skip = true; break;
                    }
                }
                for (int i = 0; i < gMonsterAttackingAddress.Count; i++)
                {
                    if ((int)gMonsterAttackingAddress[i] == somaEntity.ReturnAddress()) // Skip monsters already found
                    {
                        Skip = true; break;
                    }
                }
                for (int i = 0; i < BadEntity.Count; i++)
                {
                    if (Convert.ToInt32(BadEntity[i]) == somaEntity.ID())   // Flag the blacklist bool if the mob is on it
                    {
                        Blacklisted = true; BlackListPos = i; break;
                    }
                }
                if (somaEntity.ReturnAddress() == myPlayer.Address())   // Skip the player if found
                {
                    Skip = true;
                }
                if (somaEntity.IsDead())    // Skip dead stuff
                {
                    Skip = true;
                }
                if (!Skip)
                {
                    for (int i = 0; i < EntityType.Count; i++)
                    {
                        if (Convert.ToInt32(EntityType[i]) == somaEntity.TypeID())  // Determine if entity type is correct
                        {
                            CorrectType = true; break;
                        }
                    }
                    if (CorrectType)
                    {
                        if ((myPlayer.Zone() == "VoD" && somaEntity.XLocation() >= 453) || (myPlayer.Zone() == "Hwan" && somaEntity.XLocation() <= 13))
                        {
                            if (ShowOutput)
                                LogWrite("  " + somaEntity.ReturnAddress().ToString("X") + " is out of bounds");
                        }
                        else
                        {
                            if (somaEntity.IsInRange(RangeX, RangeY))
                            {
                                if (somaEntity.IsAttacking() == "ATTACKING")
                                {
                                    if (ShowOutput)
                                    {
                                        LogWrite("  " + somaEntity.ReturnAddress().ToString("X") + " is already attacking");
                                        if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                        {
                                            PrintEntityDebug();
                                        }
                                    }
                                    if (somaEntity.IsWithinRange(2))
                                    {
                                        if (somaEntity.IsFacingPlayer())
                                        {
                                            if (Blacklisted)
                                            {
                                                BadEntity[BlackListPos] = 0;
                                            }
                                            gMonsterAttackingAddress.Add(somaEntity.ReturnAddress());
                                        }
                                        else
                                        {
                                            if (!Blacklisted)
                                            {
                                                if (somaEntity.TypeID() == 807) // Nightmare
                                                {
                                                    gMonsterAddress.Add(somaEntity.ReturnAddress());
                                                    gMonsterRangeX.Add(somaEntity.ReturnEntityRange()[0]);
                                                    gMonsterRangeY.Add(somaEntity.ReturnEntityRange()[1]);
                                                }
                                            }
                                        }
                                    } 
                                    else
                                    {
                                        if (!Blacklisted)
                                        {
                                            //if (!LevelingInt)
                                            {
                                                BadEntity.Add(somaEntity.ID());
                                            }
                                        }
                                    } 
                                }
                                else if (somaEntity.IsAttacking() == "NOT ATTACKING")
                                {
                                    if (Blacklisted)
                                    {
                                        if (ShowOutput)
                                        {
                                            if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                            {
                                                LogWrite("  " + somaEntity.ReturnAddress().ToString("X") + " is blacklisted");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        gMonsterAddress.Add(somaEntity.ReturnAddress());
                                        gMonsterRangeX.Add(somaEntity.ReturnEntityRange()[0]);
                                        gMonsterRangeY.Add(somaEntity.ReturnEntityRange()[1]);
                                    }
                                }
                                else // somaEntity.IsAttacking() == "UNKNOWN" (Some values haven't been loaded yet so proper checks can't be completed
                                {

                                }
                            }
                        }
                    }
                    else
                    {
                        if (somaEntity.IsPlayer())
                        {
                            if (somaEntity.IsGrey())
                            {
                                if (ShowOutput)
                                {
                                    LogWrite("  " + somaEntity.ReturnAddress().ToString("X") + " is pking");
                                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                    {
                                        PrintEntityDebug();
                                    }
                                }
                                return "PK";
                            }
                        }
                        else
                        {
                            if (somaEntity.IsAttacking() == "ATTACKING")
                            {
                                if (somaEntity.IsWithinRange(2) && somaEntity.IsFacingPlayer())
                                {
                                    if (ShowOutput)
                                    {
                                        LogWrite("  " + somaEntity.ReturnAddress().ToString("X") + " foreign entity is attacking");
                                    }
                                    if (Blacklisted)
                                    {
                                        if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                        {
                                            LogWrite("  " + somaEntity.ReturnAddress().ToString("X") + " is blacklisted");
                                        }
                                        BadEntity[BlackListPos] = 0;
                                    }
                                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                    {
                                        PrintEntityDebug();
                                    }
                                    return "Ambush";
                                }
                            }
                        }
                    }
                }
                Skip = false;
                CorrectType = false;
                Blacklisted = false;
            }
            if (gMonsterAttackingAddress.Count > 0)
            {
                if (ReturnAmbushNumber)
                {
                    return gMonsterAttackingAddress.Count.ToString();
                }
                if (ShowOutput)
                {
                    LogWrite("  " + gMonsterAttackingAddress.Count.ToString() + " entity(s) are attacking");
                }
                somaEntity.SetAddress((int)gMonsterAttackingAddress[0]);
                if (ShowOutput)
                {
                    LogWrite("      Nearest attacking entity: " + somaEntity.ReturnAddress().ToString("X"));
                    if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                    {
                        PrintEntityDebug();
                    }
                }
                return "Ambush";
            }
            if (gMonsterAddress.Count > 0)
            {
                if (ShowOutput)
                {
                    LogWrite("  Found: " + gMonsterAddress.Count.ToString() + " good entities");
                }
                for (int tmpVar = 0; tmpVar < 13; tmpVar++)
                {
                    for (int i = 0; i < gMonsterAddress.Count; i++)
                    {
                        if ((int)gMonsterRangeX[i] <= tmpVar && (int)gMonsterRangeY[i] <= tmpVar)
                        {
                            somaEntity.SetAddress((int)gMonsterAddress[i]);
                            if (ShowOutput)
                            {
                                LogWrite("      Nearest entity: " + somaEntity.ReturnAddress().ToString("X"));
                                if (bool.Parse(AdvancedForm.ShowDebug.Checked.ToString()))
                                {
                                    PrintEntityDebug();
                                }
                            }
                            return "Entity Found";
                        }
                    }
                }
            }
            if (ShowOutput)
                LogWrite("      Found no relevant entities");
            return "Nothing Found";
        }

        public void LogWrite(string Message)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    Logger.Items.Add(Message);
                    Logger.TopIndex = Logger.Items.Count - 1;
                }));
            }
            else
            {
                Logger.Items.Add(Message);
                Logger.TopIndex = Logger.Items.Count - 1;
            }
        }

        public void LogWrite(string Message, string AmendOrAppend)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    switch (AmendOrAppend.ToLower())
                    {
                        case "append":
                            LogWrite(Message.Insert(0, Logger.Items[Logger.Items.Count - 1].ToString()));
                            Logger.Items.RemoveAt(Logger.Items.Count - 2);
                            break;
                    }
                    Logger.TopIndex = Logger.Items.Count - 1;
                }));
            }
            else
            {
                switch (AmendOrAppend.ToLower())
                {
                    case "append":                  
                        LogWrite(Message.Insert(0, Logger.Items[Logger.Items.Count - 1].ToString()));
                        Logger.Items.RemoveAt(Logger.Items.Count - 2);
                        break;
                }           
                Logger.TopIndex = Logger.Items.Count - 1;
            }
        }

        public void LogWrite(string Message, bool PrintNewLine)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    if (PrintNewLine)
                        Logger.Items.Add(string.Empty);
                    Logger.Items.Add(Message);
                    Logger.TopIndex = Logger.Items.Count - 1;
                }));
            }
            else
            {
                if (PrintNewLine)
                    Logger.Items.Add(string.Empty);
                Logger.Items.Add(Message);
                Logger.TopIndex = Logger.Items.Count - 1;
            }
        }

        public void ToggleTimer(string WhichTimer)
        {
            if (this.InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    switch (WhichTimer.ToLower())
                    {
                        case "pot":
                            if (!PotTick.Enabled)
                            {
                                PotBox.Text = " Potting ";
                                PotBox.ForeColor = Color.WhiteSmoke;
                                foreach (Control c in PotBox.Controls)
                                {
                                    c.ForeColor = Color.Gray;
                                }
                                hpOut = false; mpOut = false;
                                PotTick.Start();
                            }
                            else
                            {
                                PotBox.Text = " Pot ";
                                PotBox.ForeColor = Color.Gray;
                                PotTick.Stop();
                            }
                            break;
                        case "click":
                            if (!ClickTick.Enabled)
                            {
                                ClickBox.Text = " Clicking ";
                                ClickBox.ForeColor = Color.WhiteSmoke;
                                foreach (Control c in ClickBox.Controls)
                                {
                                    c.ForeColor = Color.Gray;
                                }
                                ClickTick.Start();
                            }
                            else
                            {
                                ClickBox.Text = " Click ";
                                ClickBox.ForeColor = Color.Gray;
                                ClickTick.Stop();
                            }
                            break;
                        case "targettracker":
                            if (!TargetTick.Enabled)
                            {
                                LogWrite("Tracking targets", true);
                                TargetBox.ForeColor = Color.WhiteSmoke;
                                foreach (Control c in TargetBox.Controls)
                                {
                                    c.ForeColor = Color.Gray;
                                }
                                TargetTick.Start();
                            }
                            else
                            {
                                TargetBox.ForeColor = Color.Gray;
                                TargetTick.Stop();
                            }
                            break;
                        case "clock":
                            if (!Clock.Enabled)
                                Clock.Start();
                            else
                                Clock.Stop();
                            break;
                        case "gmdetect":
                            if (!gmDetection.Enabled)
                                gmDetection.Start();
                            else
                                gmDetection.Stop();
                            break;
                    }
                }));
            }
            else
            {
                switch (WhichTimer.ToLower())
                {
                    case "pot":
                        if (!PotTick.Enabled)
                        {
                            PotBox.Text = " Potting ";
                            PotBox.ForeColor = Color.WhiteSmoke;
                            foreach (Control c in PotBox.Controls)
                            {
                                c.ForeColor = Color.Gray;
                            }
                            hpOut = false; mpOut = false;
                            PotTick.Start();
                        }
                        else
                        {
                            PotBox.Text = " Pot ";
                            PotBox.ForeColor = Color.Gray;
                            PotTick.Stop();
                        }
                        break;
                    case "click":
                        if (!ClickTick.Enabled)
                        {
                            ClickBox.Text = " Clicking ";
                            ClickBox.ForeColor = Color.WhiteSmoke;
                            foreach (Control c in ClickBox.Controls)
                            {
                                c.ForeColor = Color.Gray;
                            }
                            ClickTick.Start();
                        }
                        else
                        {
                            ClickBox.Text = " Click ";
                            ClickBox.ForeColor = Color.Gray;
                            ClickTick.Stop();
                        }
                        break;
                    case "targettracker":
                        if (!TargetTick.Enabled)
                        {
                            //LogWrite("Tracking targets", true);
                            TargetBox.ForeColor = Color.WhiteSmoke;
                            foreach (Control c in TargetBox.Controls)
                            {
                                c.ForeColor = Color.Gray;
                            }
                            TargetTick.Start();
                        }
                        else
                        {
                            TargetBox.ForeColor = Color.Gray;
                            TargetTick.Stop();
                        }
                        break;
                    case "clock":
                        if (!Clock.Enabled)
                            Clock.Start();
                        else
                            Clock.Stop();
                        break;
                    case "gmdetect":
                        if (!gmDetection.Enabled)
                            gmDetection.Start();
                        else
                            gmDetection.Stop();
                        break;
                }
            }
        }

        public int ReturnRandomNumber(int Min, int Max)
        {
            // This function will always return -1 less than the max entered
            // E.G
            // Min = 0, Max = 5
            // Could return anything between 0 - 4
            var rVar = new Random();
            int ReturnThis = rVar.Next(Min, Max);
            return ReturnThis;
        }

        public void ResetTarget()
        {
            TargetSet = false;
            label1.Location = new Point(19, label1.Location.Y);
            TargetHP.Location = new Point(26, TargetHP.Location.Y);
            TargetDamageBox.Items.Clear();
            TargetBox.ForeColor = Color.Gray;
            TargetHP.Text = string.Empty;
            TargetHPMax.Text = string.Empty;
            StatsLabel.Text = string.Empty;
            label1.Hide();
            label6.Hide();
            label7.Hide();
        }

        public void IniateTarget()
        {
            ceHP = somaEntity.CurrentHP();
            Exp = myPlayer.CurrentEXP();
            pTargetID = somaEntity.ID();
            TargetBox.ForeColor = Color.WhiteSmoke;
            TargetHP.ForeColor = Color.OrangeRed;
            TargetHPMax.ForeColor = Color.OrangeRed;
            StatsLabel.ForeColor = Color.Gray;
            TargetDamageBox.ForeColor = Color.Red;
            label1.ForeColor = Color.Gray;
            label6.ForeColor = Color.Gray;
            label7.ForeColor = Color.Gray;
            label1.Show();
            label6.Show();
            label7.Show();
            Elapsed = 0;
            TargetSet = true;
        }

        public void ResetBot()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(delegate
                {
                    BottingBox.Text = " Bot ";
                    BottingBox.ForeColor = Color.Gray;
                }));
            }
            else
            {
                BottingBox.Text = " Bot ";
                BottingBox.ForeColor = Color.Gray;
            }
        }

        public void ScreenShot(string FileName)
        {
            Bitmap bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size);
                bmp.Save(FileName);
            }
        }


        // Timers
        bool TargetSet = false; uint pTargetID; double Elapsed = 0; int Exp = 0, ceHP = 0, ekCount = 1;
        private void TargetTick_Tick(object sender, EventArgs e)
        {
            if (TargetSet)
            {
                if (ceHP != somaEntity.CurrentHP())
                {
                    //LogWrite("cehp != currentHP");
                    if (ceHP > somaEntity.CurrentHP())
                    {
                        if (TargetDamageBox.Items.Count == 4)
                        {
                            TargetDamageBox.Items.RemoveAt(0);
                        }
                        TargetDamageBox.Items.Add("- " + (ceHP - somaEntity.CurrentHP()).ToString());
                    }
                    TargetDamageBox.TopIndex = TargetDamageBox.Items.Count - 1;
                }
                ceHP = somaEntity.CurrentHP();
                TargetHP.Text = somaEntity.MaximumHP().ToString();
                TargetHPMax.Text = somaEntity.CurrentHP().ToString();
                /*
                if (somaEntity.IsPlayer())
                {
                    StatsLabel.Text = "Str: " + somaEntity.Stats()[0].ToString() +
                                        "   Dex: " + somaEntity.Stats()[2].ToString() +
                                        "   Int: " + somaEntity.Stats()[1].ToString();

                }
                */
                if (somaEntity.IsDead())
                {
                    if (AdvancedForm.AdditionalTargetInfo.Checked)
                    {
                        string IAmAString = Elapsed.ToString();
                        double TimeTaken = double.Parse(IAmAString.Insert(IAmAString.Length - 1, "."));
                        int ExpGained = myPlayer.CurrentEXP() - Exp;
                        LogWrite("Target Information Report" + " [" + ekCount.ToString() + "]", true);
                        LogWrite(string.Empty);
                        LogWrite("  Elapsed Time: " + TimeTaken.ToString() + " Seconds");
                        LogWrite(string.Empty);

                        LogWrite("  Current Exp: " + myPlayer.CurrentEXP());
                        LogWrite("    Maximum Exp: " + myPlayer.MaximumEXP());
                        LogWrite(string.Empty);

                        LogWrite("  Exp Gained: " + ExpGained);
                        double Percentage = Math.Round(100.0 * ExpGained / myPlayer.MaximumEXP(), 3, MidpointRounding.AwayFromZero);
                        LogWrite("      As Percent: 0" + Percentage + "%" + " - 0"+ Math.Round(Percentage, 2, MidpointRounding.AwayFromZero) + "%");
                        //LogWrite("          Rounded: 0" + Math.Round(Percentage, 2, MidpointRounding.AwayFromZero) + "%");
                        LogWrite(string.Empty);
                        LogWrite("  Mobs Per Hour: " + Convert.ToInt32( (3600 / TimeTaken) ).ToString());
                        LogWrite("  Exp Per Hour: " + Convert.ToInt32( ( (3600 / TimeTaken) * (myPlayer.CurrentEXP() - Exp) ) ).ToString());
                        Percentage = Percentage * Convert.ToInt32((3600 / TimeTaken));
                        LogWrite("      As percent: " + Percentage + "%" + " - " + Math.Round(Percentage, 2, MidpointRounding.AwayFromZero) + "%");
                        //LogWrite("          Rounded: " + Math.Round(Percentage, 2, MidpointRounding.AwayFromZero) + "%");
                        ekCount++;
                    }
                    ResetTarget();
                }
                if (myPlayer.TargetPTR() != -1 && myPlayer.TargetPTR() != pTargetID)
                {
                    ResetTarget();
                }
                Elapsed++;
            }
            else
            {
                if (myPlayer.HasTarget())
                {
                    if (!Botting)
                    {
                        if (somaEntity.SetSelectedEntityByID(myPlayer.TargetPTR()))
                        {
                            IniateTarget();
                        }
                        else
                        {
                            //LogWrite("Couldn't find entity by ID");
                        }
                    }
                    else
                    {
                        IniateTarget();
                    }
                }
            }
        }

        private void LClickTick_Tick(object sender, EventArgs e)
        {
            if (ClickDrops.Checked)
            {
                if (somaMouse.State() == 2)
                {
                    LogWrite("Picking up drop");
                    somaMouse.SimulateLeftClick();
                }
            }
            /*
            if (ClickPK.Checked)
            {
                if (somaMouse.State() == 3)
                {
                    //LogWrite("Attacking entity");
                    while (!myPlayer.IsAutoAttacking())
                    {
                        somaMouse.SimulateLeftClick();
                    }
                }
            }
             */
            if (ClickRight.Checked)
            {
                somaMouse.SimulateRightClick();
            }
            if (ClickLeft.Checked)
            {
                /*
                if (!myPlayer.HasTarget())
                {
                    if (!myPlayer.IsAutoAttacking())
                    {
                        if (somaMouse.State() == 1)
                        {
                            while (!myPlayer.IsAutoAttacking())
                            {
                                somaMouse.SimulateLeftClick();
                            }
                        }
                    }
                    else
                    {
                        if (!myPlayer.HasTarget())
                            myPlayer.ResetAutoAttack();
                    }
                }
                */
            }
        }

        bool potHP; bool potMP; bool mpOut; bool hpOut;
        private void PotTick_Tick(object sender, EventArgs e)
        {
            if (!hpOut)
            {
                if (potHP)
                {
                    if (bool.Parse(AdvancedForm.PotToFull.Checked.ToString()))
                    {
                        if (myPlayer.CurrentHPBelowPercent(96))
                            playerBelt.UseHP();
                        else
                            potHP = false;
                    }
                    else
                    {
                        if (myPlayer.CurrentHPBelowPercent(int.Parse(PotHealthPercent.Text) + 20))
                            playerBelt.UseHP();
                        else
                            potHP = false;
                    }
                }
                else
                {
                    if (playerBelt.HasHP())
                    {
                        if (PotHealthPercent.Text != string.Empty && myPlayer.CurrentHPBelowPercent(int.Parse(PotHealthPercent.Text)))
                        {
                            LogWrite("Potting HP up");
                            potHP = true;
                        }
                    }
                    else
                    {
                        LogWrite("No HP potions found", true);
                        LogWrite("  Disabling HP potting");
                        hpOut = true;
                    }
                }
            }
            if (!mpOut)
            {
                if (potMP && !potHP)
                {
                    if (myPlayer.CurrentMPBelowPercent(96))
                        playerBelt.UseMP();
                    else
                        potMP = false;
                }
                else
                {
                    if (playerBelt.HasMP())
                    {
                        if (PotManaPercent.Text != string.Empty && myPlayer.CurrentMPBelowPercent(int.Parse(PotManaPercent.Text)))
                        {
                            LogWrite("Potting MP up");
                            potMP = true;
                        }
                    }
                    else
                    {
                        LogWrite("No MP potions found", true);
                        LogWrite("  Disabling MP potting");
                        mpOut = true;
                    }
                }
            }
            if (hpOut && mpOut)
            {
                hpOut = false; mpOut = false;
                LogWrite("HP & MP Macro stopped");
                ToggleTimer("Pot");
            }
        }


        // Form stuff
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            using (System.IO.TextWriter log = new System.IO.StreamWriter(Application.StartupPath + "\\Out.log"))
            {
                LogWrite("Saving log");
                for (int i = 0; i < Logger.Items.Count; i++)
                {
                    log.WriteLine(Logger.Items[i]);
                }
            }
            using (System.IO.TextWriter log = new System.IO.StreamWriter(Application.StartupPath + "\\Config.ini"))
            {
                LogWrite("Saving CFG");
                if (EntityType.Count != 0)
                {
                    if (EntityType.Count == 1)
                    {
                        log.WriteLine("EntityTypeID=" + EntityType[0].ToString());
                    }
                    else
                    {
                        string NewLine = "EntityTypeID=" + EntityType[0].ToString();
                        for (int i = 1; i < EntityType.Count; i++)
                        {
                            NewLine += "," + EntityType[i].ToString();
                        }
                        log.WriteLine(NewLine);
                    }
                }
                else
                {
                    log.WriteLine("EntityTypeID=");
                }
                // Crafting
                log.WriteLine("WarehouseSlotOne=" + CraftForm.WhSlotOne.Text);
                log.WriteLine("WarehouseSlotTwo=" + CraftForm.WhSlotTwo.Text);
                log.WriteLine("WarehouseSlotThree=" + CraftForm.WhSlotThree.Text);
                log.WriteLine("WarehouseSlotFour=" + CraftForm.WhSlotFour.Text);
                log.WriteLine("CustomInputOne=" + CraftForm.CustomInputOne.Text);
                log.WriteLine("CustomInputTwo=" + CraftForm.CustomInputTwo.Text);
                log.WriteLine("CustomInputThree=" + CraftForm.CustomInputThree.Text);
                log.WriteLine("CustomInputFour=" + CraftForm.CustomInputFour.Text);

                // Healing
                if (AdvancedForm.BotHeal.Checked)
                    log.WriteLine("Heal=" + AdvancedForm.BotHeal.Checked.ToString());
                else
                    log.WriteLine("Heal=False");
                if (AdvancedForm.HealInCombat.Checked)
                    log.WriteLine("HealInCombat=" + AdvancedForm.HealInCombat.Checked.ToString());
                else
                    log.WriteLine("HealInCombat=False");
                if (AdvancedForm.BotHealPercent.Text != string.Empty)
                    log.WriteLine("Heal%=" + AdvancedForm.BotHealPercent.Text);
                else
                    log.WriteLine("Heal%=55");
                if (AdvancedForm.HealFKey.Text != string.Empty)
                    log.WriteLine("HealSlot=" + AdvancedForm.HealFKey.Text);
                else
                    log.WriteLine("HealSlot=0");

                // HP & MP
                if (PotHealthPercent.Text != string.Empty)
                    log.WriteLine("potHP%=" + PotHealthPercent.Text);
                else
                    log.WriteLine("potHP%=20");
                if (PotManaPercent.Text != string.Empty)
                    log.WriteLine("potMP%=" + PotManaPercent.Text);
                else
                    log.WriteLine("potMP%=10");

                // Stop After Stuff
                if (AdvancedForm.StopBotAfter.Checked)
                    log.WriteLine("StopAfter=" + AdvancedForm.StopBotAfter.Checked);
                else
                    log.WriteLine("StopAfter=False");
                if (AdvancedForm.StopAfterValue.Text != string.Empty)
                    log.WriteLine("StopAfterValue=" + AdvancedForm.StopAfterValue.Text);
                else
                    log.WriteLine("StopAfterValue=60");

                // Attack
                if (AdvancedForm.AttackTimeOut.Text != string.Empty)
                    log.WriteLine("AttackTimeOut=" + AdvancedForm.AttackTimeOut.Text);
                else
                    log.WriteLine("AttackTimeOut=" + 8);

                // Aura
                if (AdvancedForm.BotAura.Checked)
                    log.WriteLine("Aura=" + AdvancedForm.BotAura.Checked.ToString());
                else
                    log.WriteLine("Aura=False");
                if (AdvancedForm.AuraDelay.Text != string.Empty)
                    log.WriteLine("AuraDelay=" + AdvancedForm.AuraDelay.Text);
                else
                    log.WriteLine("AuraDelay=30");
            }
        }

        private void SetEntityTypeID_Click(object sender, EventArgs e)
        {
            LogWrite("Mouse over a monster...", true);
            somaGame.GiveFocus();
            if (!EntityIDWorker.IsBusy)
                EntityIDWorker.RunWorkerAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!EntityIDWorker.CancellationPending)
            {
                while (!somaEntity.FoundEntityWithMouse())
                {
                    Thread.Sleep(200);
                }
                if (EntityType.Count == 0)
                {
                    LogWrite("      Set entity Type ID: " + somaEntity.TypeID().ToString());
                    LogWrite("      Name:   " + somaEntity.Name());
                    EntityType.Add(somaEntity.TypeID());
                }
                else
                {
                    LogWrite("      Added new entity Type ID: " + somaEntity.TypeID().ToString());
                    //string cNAme = somaEntity.Name();
                    //LogWrite(cNAme.Length.ToString());
                    LogWrite("      Name:   " + somaEntity.Name());
                    EntityType.Add(somaEntity.TypeID());
                }
                EntityIDWorker.CancelAsync();
            }
        }

        private void LoadWaypoints_Click(object sender, EventArgs e)
        {
            Waypoint_X.Clear();
            Waypoint_Y.Clear();
            LogWrite("Cleared waypoints", true);
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.RestoreDirectory = true;
            openDialog.Filter = "Waypoint file (*.wpf)|*.wpf";
            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string Line in System.IO.File.ReadAllLines(openDialog.FileName))
                {
                    if (Line != "")
                    {
                        string[] result = Line.Split(' ');
                        Waypoint_X.Add(int.Parse(result[0]));
                        Waypoint_Y.Add(int.Parse(result[1]));
                    }
                }
            }
            LogWrite(Waypoint_X.Count.ToString() + " waypoints loaded", true);
        }

        private void SaveWaypoints_Click(object sender, EventArgs e)
        {
            if (Waypoint_X.Count == 0)
            {
                LogWrite("No waypoints found", true);
            }
            else
            {
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.DefaultExt = "wpf";
                saveDialog.AddExtension = true;
                saveDialog.RestoreDirectory = true;
                saveDialog.Filter = "Waypoint file (*.wpf)|*.wpf";
                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.TextWriter WriteText = new System.IO.StreamWriter(saveDialog.FileName);
                    for (int i = 0; i < Waypoint_X.Count; i++)
                    {
                        WriteText.WriteLine(Waypoint_X[i] + " " + Waypoint_Y[i]);
                    }
                    LogWrite("Waypoints saved", true);
                    WriteText.Close();
                }
                else
                {
                    LogWrite("Waypoints not saved", true);
                }
            }
        }

        private void ClearWaypoints_Click(object sender, EventArgs e)
        {
            Waypoint_X.Clear();
            Waypoint_Y.Clear();
            LogWrite("Cleared waypoints", true);
            EntityType.Clear();
            LogWrite("Cleared entities", true);
        }

        int ClockTick = 0, AuraDelay = 0, TimeOut = 0, BlackListFlush = 0;
        private void Clock_Tick(object sender, EventArgs e)
        {
            //LogWrite(ClockTick.ToString());
            ClockTick++;
            AuraDelay++;
            TimeOut++;
            BlackListFlush++;
        }

        private void EmbedSoma_Click(object sender, EventArgs e)
        {
            if (myPlayer.CurrentHP() > 0)
            {
                this.Size = new System.Drawing.Size(1550, 835);
                EmbedSoma.Enabled = false;
                somaGame.Embed(this.SomaPanel.Handle);
                /*
                if (this.Size.Width >= 1500)
                {
                    this.Size = new System.Drawing.Size(250, 750);
                    EmbedSoma.Text = "Embed";
                    somaGame.Release();
                }
                else
                {
                    this.Size = new System.Drawing.Size(1550, 835);
                    EmbedSoma.Text = "Release";
                    somaGame.Embed(this.SomaPanel.Handle);
                }
                */
            }
            else
                LogWrite("Must be logged in to embed", true);
        }

        private void BotAdvanced_Click(object sender, EventArgs e)
        {
            if (Craft)
                CraftForm.ShowDialog();
            else
                AdvancedForm.ShowDialog();
        }
        
        private void LoadGame_Click(object sender, EventArgs e)
        {
            LogWrite("Loading Soma", true);

            somaGame.RunSoma();

            LogWrite("  Searching for process");

            if (!somaGame.FindProcess(true))
            {
                LogWrite("      Could not find soma");
            }
            else
            {
                LogWrite("      pHandle: " + Game.hWndParent.ToString("X") + "   cHandle: " + Game.hWndChild.ToString("X"));

                somaGame.GiveFocus();

                playerBelt = new Belt();
                somaEntity = new Entity(false);
                myPlayer = new Player();
                PlayerDrops = new Drops();
                somaMouse = new Mouse();
                aEntity = new Entity(true); // For GM Tracking

                SetStats();
                if (pStat[0] != 0) {
                    LogWrite("      Stat tracking ready");
                    StatTrackTick.Start();
                }
                else
                    LogWrite("      Could not start stat tracking");

                if (!TargetTick.Enabled)
                {
                    LogWrite("      Target tracking enabled");
                    TargetTick.Start();
                }
            }
        }

        int SleepH = 0;
        private void ShiftTimer_Tick(object sender, EventArgs e)
        {
            SleepH += 100;
            LogWrite(SleepH.ToString());
        }

        bool Craft = false;
        private void CraftButton_Click(object sender, EventArgs e)
        {
            if (Craft)
            {
                LogWrite("Crafting disabled", true);
                Craft = false;
            }
            else
            {
                LogWrite("Crafting enabled", true);
                Craft = true;
            }
        }

        Entity aEntity; bool FoundAdmin = false;
        private void gmDetection_Tick(object sender, EventArgs e)
        {
            //LogWrite("Checking for GM... ");                             
            for (int IterateMe = 0x278; IterateMe < 0x300; IterateMe += 0x4)
            {
                aEntity.FindAddress(IterateMe);
                if (aEntity.Name() != string.Empty)
                {
                    //LogWrite(aEntity.Name());                    
                    switch (aEntity.Name())
                    {
                        case "ISYLVER":
                            FoundAdmin = true;
                            break;
                        case "FINITO":
                            FoundAdmin = true;
                            break;
                        case "GHOSTLORD":
                            FoundAdmin = true;
                            break;
                        case "DISYLVER":
                            FoundAdmin = true;
                            break;
                        case "MACHEONRU":
                           FoundAdmin = true;
                           break;
                    }
                    if (myPlayer.Zone() == "Conti" || myPlayer.Zone() == "Castle")
                    {
                        if (aEntity.IsPlayer())
                        {

                        }
                    }
                }
            }
            if (FoundAdmin)
            {
                LogWrite("GM DETECTED!");
                if (Botting)
                {
                    SoundAlert();
                    Craftbot.CancelAsync();
                }
                gmDetection.Stop();
            } 
        }

        public void SetStats()
        {
            pStat[0] = myPlayer.GetStat("STR");
            pStat[1] = myPlayer.GetStat("DEX");
            pStat[2] = myPlayer.GetStat("INT");

            for (int i = 0; i < pExp.Length; i++) {
                pExp[i] = myPlayer.CurrentEXP();
            }
        }

        int[] pStat = new int[] { 0, 0, 0 };
        int[] pExp = new int[] { 0, 0, 0 };
        int[] pTime = new int[] { 0, 0, 0 };

        private void StatTrackTick_Tick(object sender, EventArgs e)
        {
            if (AdvancedForm.TrackPlayerStats.Checked)
            {
                for (int i = 0; i < pTime.Length; i++)
                {
                    pTime[i]++;
                }
                if (myPlayer.GetStat("STR") > pStat[0])
                {
                    // 0.1 STR gained
                    LogWrite("Str gained", true);

                    // Show much percent it took
                    double Percentage = Math.Round(100.0 * (myPlayer.CurrentEXP() - pExp[0]) / myPlayer.MaximumEXP(), 3, MidpointRounding.AwayFromZero);
                    LogWrite("  Percent Taken: " + Percentage + "%" + " - " + Math.Round(Percentage, 2, MidpointRounding.AwayFromZero) + "%");

                    // Show how long it took
                    string ShowMe = "   Time Taken: ";
                    if (pTime[0] >= 60)
                    {
                        int Minutes = pTime[0] / 60,
                            Seconds = pTime[0] - (60 * Minutes);
                        ShowMe += Minutes.ToString() + " Minutes " + Seconds.ToString() + " Seconds";
                    }
                    else
                    {
                        ShowMe += pTime[0].ToString() + " Seconds";
                    }
                    LogWrite(ShowMe);

                    // Update previous stats
                    pExp[0] = myPlayer.CurrentEXP();
                    pStat[0] = myPlayer.GetStat("STR");
                    pTime[0] = 0;
                }
                if (myPlayer.GetStat("DEX") > pStat[1])
                {
                    LogWrite("Dex gained", true);
                    double Percentage = Math.Round(100.0 * (myPlayer.CurrentEXP() - pExp[1]) / myPlayer.MaximumEXP(), 3, MidpointRounding.AwayFromZero);
                    LogWrite("  Percent Taken: " + Percentage + "%" + " - " + Math.Round(Percentage, 2, MidpointRounding.AwayFromZero) + "%");

                    string ShowMe = "   Time Taken: ";
                    if (pTime[1] >= 60)
                    {
                        int Minutes = pTime[1] / 60,
                            Seconds = pTime[1] - (60 * Minutes);
                        ShowMe += Minutes.ToString() + " Minutes " + Seconds.ToString() + " Seconds";
                    }
                    else
                    {
                        ShowMe += pTime[1].ToString() + " Seconds";
                    }
                    LogWrite(ShowMe);

                    pExp[1] = myPlayer.CurrentEXP();
                    pStat[1] = myPlayer.GetStat("DEX");
                    pTime[1] = 0;
                }
                if (pStat[2] > myPlayer.GetStat("INT"))
                {
                    LogWrite("Int gained", true);
                    double Percentage = Math.Round(100.0 * (myPlayer.CurrentEXP() - pExp[2]) / myPlayer.MaximumEXP(), 3, MidpointRounding.AwayFromZero);
                    LogWrite("  Percent Taken: " + Percentage + "%" + " - " + Math.Round(Percentage, 2, MidpointRounding.AwayFromZero) + "%");

                    string ShowMe = "   Time Taken: ";
                    if (pTime[2] >= 60)
                    {
                        int Minutes = pTime[2] / 60,
                            Seconds = pTime[2] - (60 * Minutes);
                        ShowMe += Minutes.ToString() + " Minutes " + Seconds.ToString() + " Seconds";
                    }
                    else
                    {
                        ShowMe += pTime[2].ToString() + " Seconds";
                    }
                    LogWrite(ShowMe);

                    pExp[2] = myPlayer.CurrentEXP();
                    pStat[2] = myPlayer.GetStat("DEX");
                    pTime[2] = 0;
                }
            }
        }
    }
}
