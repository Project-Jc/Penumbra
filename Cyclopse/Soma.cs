using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Diagnostics;
using MemoryEditor;
using System.Timers;

namespace Soma
{
    public class Entity
    {
        private Memory oMemory;
        private Mouse somaMouse;
        private Player tmpPlayer;

        private static int SelectedEntity,
                           tmpSelectedEntity;

        private int EntityArrayBase;

        bool tmpEntity;

        private const int HP_Max = 0x244,
                            HP_Cur = 0x240,
                            MP_Max = 0x24c,
                            MP_Cur = 0x248,
                            Type_ID = 0x164,
                            eID = 0x20c,
                            X = 0x1d4,
                            Y = 0x1d8,
                            Dead = 0x38,
                            MotionStep = 0x200,
                            eDirectionFacing = 0x50,
                            eWeakened = 0xDc,
                            eStr = 0x13e8,
                            eInt = 0x13ea,
                            eDex = 0x13ec,
                            eWis = 0x13ee,
                            eCon = 0x13f2,
                            rSpeed = 0x1438,
                            rSpeedMax = 0x1444,
                            eName = 0x58,
                            eGreyStatus = 0x19c,
                            eAction = 0x48,
                            ePreviousAction = 0x4C;

        public Entity(bool UseTmpEntity)
        {
            if (UseTmpEntity)
                tmpEntity = true;
            oMemory = new Memory();
            EntityArrayBase = oMemory.CalculateStaticAddress(0x2e6710);
        }

        // Entity array & address stuff
        public void FindAddress(int ArrayPlace)
        {
            if (tmpEntity)
                tmpSelectedEntity = (int)oMemory.ReadInt(EntityArrayBase, new int[] { ArrayPlace });
            else
                SelectedEntity = (int)oMemory.ReadInt(EntityArrayBase, new int[] { ArrayPlace });
        }
        public bool SetSelectedEntityByID(int tmpID)
        {
            int IterateMe = 0x278;
            while (IterateMe != 0x300)
            {
                FindAddress(IterateMe);
                //tmpSelectedEntity = (int)oMemory.ReadInt(EntityArrayBase, new int[] { IterateMe });
                if (oMemory.ReadInt(SelectedEntity + eID) == tmpID)
                    return true;  
                IterateMe += 0x4;
            }
            return false;
        }
        public void SetAddress(int Address)
        {
            SelectedEntity = Address;
        }
        public int ReturnAddress()
        {
            return SelectedEntity;
        }
        public bool FoundEntityWithMouse()
        {
            somaMouse = new Mouse();
            int IterateMe = 0x278;
            while (IterateMe != 0x300)
            {
                FindAddress(IterateMe);
                if (oMemory.ReadInt(SelectedEntity + eID) == somaMouse.MousePTR())
                    return true;
                IterateMe += 0x4;
            }
            return false;
        }

        // Health & Mana
        public int CurrentHP()
        {
            return (int)oMemory.ReadInt(SelectedEntity + HP_Cur);
        }
        public int MaximumHP()
        {
            return (int)oMemory.ReadInt(SelectedEntity + HP_Max);
        }
        public int CurrentMP()
        {
            return (int)oMemory.ReadInt(SelectedEntity + MP_Cur);
        }
        public int MaximumMP()
        {
            return (int)oMemory.ReadInt(SelectedEntity + MP_Max);
        }
        public bool HealthBelowPercent(int iPercent)
        {
            if (CurrentHP() < MaximumHP() * iPercent / 100)
                return true;
            return false;
        }

        // Misc 
        public bool IsGrey()
        {
            if (oMemory.ReadByte(SelectedEntity + eGreyStatus) == 1 || oMemory.ReadByte(SelectedEntity + eGreyStatus) == 2)
                return true;
            return false;
        }
        public string Name()
        {
            //return SelectedEntity.ToString("X");
            //int nAddr = SelectedEntity + 0x58;       
            //return nAddr.ToString("X");
            
            string nName;
            if (tmpEntity)
                nName = oMemory.ReadText(tmpSelectedEntity + 0X58, new int[] { 0X0 }, 20, 0);
            else
                nName = oMemory.ReadText(SelectedEntity + 0X58, new int[] { 0X0 }, 20, 0);

            //return nName;

            string rName = string.Empty;

            foreach (char cChar in nName)
            {
                if (char.IsLower(cChar))    // Break on lowercase indicating the end of the monster name
                    break;
                if (char.IsWhiteSpace(cChar))
                    break;
                if (Convert.ToInt32(cChar) == 0)    // Null character
                    break;
                if (char.IsLetter(cChar))
                    rName += cChar;
                //rName += " " + Convert.ToInt32(cChar).ToString() + " " ;
            }
            //if (rName != string.Empty)
               // if (rName[rName.Length - 1] == 'S')
                   // return rName.Remove(rName.Length - 1, 1);
            return rName;
        }
        public bool IsWeakened()
        {
            if (oMemory.ReadByte(SelectedEntity + eWeakened) == 0)
                return false;
            return true;
        }
        public bool IsPlayer()
        {
            tmpPlayer = new Player();
            if (tmpPlayer.Address() != ReturnAddress())
                if (oMemory.ReadInt(SelectedEntity + eID) <= 10000)
                    return true;
            return false;
        }
        public bool IsDead()
        {
            if (oMemory.ReadByte(SelectedEntity + Dead) == 0)
                return false;
            return true;
        }
        public string IsAttacking()
        {
            // Skip anything that's weakened
            if (IsWeakened())
                return "UNKNOWN";

            // Entity is not moving or attacking. Hasn't been hit at all yet. Perfect.
            if (Motion() == 0 && CurrentHP() == 0 && MaximumHP() == 0)
                return "NOT ATTACKING";

            // Zero means the entity is just standing still
            if (Motion() == 0)
                return "NOT ATTACKING";

            // Anything other than zero implies the entity is either attacking or moving
            if (Motion() > 0)
            {
                // RunSpeed has not yet been set.
                if (RunSpeed() == 0)
                {
                    if ( (Action(false) == 0 && Action(true) == 1) || (Action(false) == 1 && Action(true) == 0) )
                        return "NOT ATTACKING";
                    else if ( (Action(false) == 2 && Action(true) == 0) || (Action(false) == 0 && Action(true) == 0) )
                        return "ATTACKING";
                    else
                        return "UNKNOWN";
                }
                else
                {
                    // Entity is standing still
                    if (RunSpeed() >= oMemory.ReadInt(SelectedEntity + rSpeedMax))
                        return "ATTACKING";
                    // If the entities run speed is less than the maximum this means the entity is moving
                    else
                    {
                        return "NOT ATTACKING";
                    }
                }
            }
            return "UNKNOWN";
            /* * * * * *
             *  Notes  *
             * * * * * *
             * 
             * Motion() = 0 (If standing still)
             * Motion() > 0 (If either moving or attacking)
             * 
             * RunSpeed() = 0 (Means the value simply hasn't been loaded yet) 
             * RunSpeed() >= 750 (A static value) (Means the monster is standing still)
             * RunSpeed() <= 700 (Monster is moving)
             * 
             * Health = 0/0 (For new monsters)
             * Health = 1/0 (For monsters attacking?)
             * 
             * Action(false) = 0 && Action(true) = 1    (Means the entity has just moved and is not attacking)
             * Action(false) = 1 && Action(true) = 0    (Means the entity is currently moving)
             * Action(false) = 2 && Action(true) = 0    (Means the entity is currently attacking)
             * Action(false) = 0 && Action(true) = 0    (Means the entity is currently attacking)
             * 
             * Sometimes an entity which is either attacking or not attacking will show it's actual current health. As opposed to the usual display of zero. EG: cHP: 1400 mHP: 0
             * 
             * 
             */
        }
        public int Motion()
        {
            return (int)oMemory.ReadInt(SelectedEntity + MotionStep);
        }
        public byte Action(Boolean ReturnPrevious)
        {
            if (ReturnPrevious)
                return oMemory.ReadByte(SelectedEntity + ePreviousAction);
            else
                return oMemory.ReadByte(SelectedEntity + eAction);
        }

        public int RunSpeed()
        {  
            return (int)oMemory.ReadInt(SelectedEntity + rSpeed);
        }
        public int[] ReturnEntityRange()
        {
            tmpPlayer = new Player();
            int[] Distance = new int[2];
            if (XLocation() <= tmpPlayer.XLocation() && YLocation() <= tmpPlayer.YLocation())
            {
                Distance[0] = tmpPlayer.XLocation() - XLocation();
                Distance[1] = tmpPlayer.YLocation() - YLocation();
            }
            if (XLocation() >= tmpPlayer.XLocation() && YLocation() <= tmpPlayer.YLocation())
            {
                Distance[0] = XLocation() - tmpPlayer.XLocation();
                Distance[1] = tmpPlayer.YLocation() - YLocation();
            }
            if (XLocation() <= tmpPlayer.XLocation() && YLocation() >= tmpPlayer.YLocation())
            {
                Distance[0] = tmpPlayer.XLocation() - XLocation();
                Distance[1] = YLocation() - tmpPlayer.YLocation();
            }
            if (XLocation() >= tmpPlayer.XLocation() && YLocation() >= tmpPlayer.YLocation())
            {
                Distance[0] = XLocation() - tmpPlayer.XLocation();
                Distance[1] = YLocation() - tmpPlayer.YLocation();
            }
            return Distance;
        }

        public bool IsWithinRange(int iRange)
        {
            int[] Range = ReturnEntityRange();
            switch (TypeID())
            {
                case 115:               // Wraith
                    iRange = 4;
                    break;
                case 808:               // Lich Lord
                    iRange = 6;
                    break;
                case 701:               // Prismatic Shadow
                    iRange = 8;
                    break;
                case 807:               // Nightmare
                    iRange = 16;
                    break;
            }
            if (Range[0] + Range[1] <= iRange)
                return true;
            return false;
            /*
            switch (iRange)
            {
                case 1:
                    if (Range[0] == 1 && Range[1] == 1)
                        return true;                 
                    else if (Range[0] == 2 && Range[1] == 0)
                        return true;            
                    else if (Range[0] == 0 && Range[1] == 2)
                        return true;      
                    break;
                case 2:
                    if (Range[0] <= 3 && Range[1] <= 3)
                        return true;
                    else if (Range[0] <= 4 && Range[1] == 0)
                        return true;
                    else if (Range[0] == 0 && Range[1] <= 4)
                        return true;
                    break;
                case 6:
                    if (Range[0] <= 5 && Range[1] <= 5)
                        return true;
                    else if (Range[0] <= 6 && Range[1] == 0)
                        return true;
                    else if (Range[0] == 0 && Range[1] <= 6)
                        return true;
                    break;
                case 7:
                    if (Range[0] <= 6 && Range[1] <= 6)
                        return true;
                    else if (Range[0] <= 7 && Range[1] == 0)
                        return true;
                    else if (Range[0] == 0 && Range[1] <= 7)
                        return true;
                    break;
                case 8:
                    if (Range[0] <= 7 && Range[1] <= 7)
                        return true;
                    else if (Range[0] <= 8 && Range[1] == 0)
                        return true;
                    else if (Range[0] == 0 && Range[1] <= 8)
                        return true;
                    break;
                case 10:
                    if (Range[0] <= 9 && Range[1] <= 9)
                        return true;
                    else if (Range[0] <= 10 && Range[1] == 0)
                        return true;
                    else if (Range[0] == 0 && Range[1] <= 10)
                        return true;
                    break;
            } */
            return false;
        }
        public bool IsInRange(int RangeX, int RangeY)
        {
            int[] Range = ReturnEntityRange();
            if (Range[0] <= RangeX && Range[1] <= RangeY)
            {
                return true;
            }
            return false;
        }
        public short[] Stats()
        {
            /*
            string rtrnMe = string.Empty; short cStat;
            if ((short)oMemory.ReadShort(SelectedEntity + eStr) != 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    switch (i)
                    {
                        case 0:
                            cStat = (short)oMemory.ReadShort(SelectedEntity + eStr);

                            break;
                    }
                }
            }
             */
            return new short[] { (short)oMemory.ReadShort(SelectedEntity + eStr), (short)oMemory.ReadShort(SelectedEntity + eInt), (short)oMemory.ReadShort(SelectedEntity + eDex), (short)oMemory.ReadShort(SelectedEntity + eWis), (short)oMemory.ReadShort(SelectedEntity + eCon) };
            //return (short)oMemory.ReadShort(SelectedEntity + eStr);
        }
        public int TypeID()
        {
            return (int)oMemory.ReadInt(SelectedEntity + Type_ID);
        }
        public uint ID()
        {
            return oMemory.ReadInt(SelectedEntity + eID);
        }
        public string DirectionFacing()
        {
            switch (oMemory.ReadByte(SelectedEntity + eDirectionFacing))
            {
                case 0:
                    return "Down";
                case 1:
                    return "Down Left";
                case 2:
                    return "Left";
                case 3:
                    return "Up Left";
                case 4:
                    return "Up";
                case 5:
                    return "Up Right";
                case 6:
                    return "Right";
                case 7:
                    return "Down Right";
            }
            return string.Empty;
        }
        public bool IsFacingPlayer()
        {
            tmpPlayer = new Player();
            // Top Left
            if (XLocation() < tmpPlayer.XLocation() && YLocation() < tmpPlayer.YLocation())
            {
                //MessageBox.Show("TL");
                if (DirectionFacing() == "Down Right")
                    return true;
            }
            // Above
            if (XLocation() == tmpPlayer.XLocation() && YLocation() < tmpPlayer.YLocation())
            {
                //MessageBox.Show("A");
                if (DirectionFacing() == "Down")
                    return true;
            }
            // Top Right
            if (XLocation() > tmpPlayer.XLocation() && YLocation() < tmpPlayer.YLocation())
            {
                //MessageBox.Show("TR");
                if (DirectionFacing() == "Down Left")
                    return true;
            }
            // Right
            if (XLocation() > tmpPlayer.XLocation() && YLocation() == tmpPlayer.YLocation())
            {
                //MessageBox.Show("R");
                if (DirectionFacing() == "Left")
                    return true;
            }
            // Bottom Left
            if (XLocation() < tmpPlayer.XLocation() && YLocation() > tmpPlayer.YLocation())
            {
                //MessageBox.Show("BL");
                if (DirectionFacing() == "Up Right")
                    return true;
            }
            // Below
            if (XLocation() == tmpPlayer.XLocation() && YLocation() > tmpPlayer.YLocation())
            {
                //MessageBox.Show("B");
                if (DirectionFacing() == "Up")
                    return true;
            }
            // Bottom Right
            if (XLocation() > tmpPlayer.XLocation() && YLocation() > tmpPlayer.YLocation())
            {
                //MessageBox.Show("BR");
                if (DirectionFacing() == "Up Left")
                    return true;
            }
            // Left
            if (XLocation() < tmpPlayer.XLocation() && YLocation()  == tmpPlayer.YLocation())
            {
                //MessageBox.Show("L");
                if (DirectionFacing() == "Right")
                    return true;
            }
            return false;
        }

        // Location
        public int XLocation()
        {
            return (int)oMemory.ReadInt(SelectedEntity + X);
        }
        public int YLocation()
        {
            return (int)oMemory.ReadInt(SelectedEntity + Y);
        }
        public string Quadrant()
        {
            tmpPlayer = new Player();
            // If entity is located in the top left quadrant of the game window
            if (XLocation() <= tmpPlayer.XLocation() && YLocation() <= tmpPlayer.YLocation())
            {
                return "Top Left";
            }
            // If entity is located in the top right quadrant of the game window
            if (XLocation() >= tmpPlayer.XLocation() && YLocation() <= tmpPlayer.YLocation())
            {
                return "Top Right";
            }
            // If entity is located in the bottom left quadrant of the game window
            if (XLocation() <= tmpPlayer.XLocation() && YLocation() >= tmpPlayer.YLocation())
            {
                return "Bottom Left";
            }
            // If entity is located in the bottom right quadrant of the game window
            if (XLocation() >= tmpPlayer.XLocation() && YLocation() >= tmpPlayer.YLocation())
            {
                return "Bottom Right";
            }
            return string.Empty;
        }
    }

    public class Player
    {
        private Memory oMemory;
        private Mouse somaMouse;

        private int PlayerBase,
                    HP_Max,
                    HP_Cur,
                    MP_Max,
                    MP_Cur,
                    BW_Max,
                    BW_Cur,
                    Stam_Max,
                    Stam_Cur,
                    EXP_Max,
                    EXP_Cur,
                    Barr,
                    Level,
                    pID,
                    X,
                    Y,
                    iZone,
                    Posture,
                    pUserMotion,
                    Target_PTR,
                    pWarping,
                    Dead,
                    StaminaCheck,
                    AutoAttack,
                    cWeaponType,
                    Race,
                    eMoving,
                    cWeaponDura,
                    cWeaponDuraMax,
                    cWeaponDuraMaxOriginal,
                    cWeaponIndex,
                    cHelmDura,
                    cHelmDuraMax,
                    cHelmDuraMaxOriginal,
                    cHelmIndex,
                    invItemIndex,
                    invItemIndexOne,
                    invItemDura,
                    invItemDuraMax,
                    invItemDuraMaxOriginal,
                    InventoryScrollIndex,
                    InventoryScrollIndexOffset,
                    WH_SelectedIndex,
                    WH_InventSelectedIndex,
                    CraftType,
                    DialogueWindowIndex,
                    CraftSlot_1,
                    CraftSlot_2,
                    CraftSlot_3,
                    ExtractDialogue,
                    ShopInventorySelectedIndex,
                    ShopInventoryCount,
                    ShopSelectedIndex,
                    SelectedSpellIndex,
                    PotionSkillOffset,
                    ZonePenalty,

                    // Stats
                    pSTR,
                    pDEX,
                    pINT,

                    // Magic
                    MagicBeltArrayStart,
                    MagicDistance,
                    MagicUsageMP,
                    MagicName,
                    MagicPower;

        public Player()
        {
            oMemory = new Memory();
            PlayerBase = oMemory.CalculateStaticAddress(0x2D8600);
            HP_Max = 0x244;
            HP_Cur = 0x240;
            MP_Max = 0x24c;
            MP_Cur = 0x248;
            BW_Max = 0x25c;
            BW_Cur = 0x260;
            Stam_Max = 0x250;
            Stam_Cur = 0x254;
            EXP_Max = 0x234;
            EXP_Cur = 0x238;
            Barr = 0x268;
            Level = 0x22c;
            pID = 0x20c;
            X = 0x1d4;
            Y = 0x1d8;
            iZone = oMemory.CalculateStaticAddress(0x1316B4);
            Posture = 0x2b8;
            pUserMotion = 0x34;
            Target_PTR = 0x1440;
            pWarping = 0x60;
            Dead = 0x38;
            StaminaCheck = oMemory.CalculateStaticAddress(0x136780);
            AutoAttack = 0x1448;
            cWeaponType = 0x2b0;
            Race = oMemory.CalculateStaticAddress(0x1362F4);
            eMoving = 0x48;

            // Stats
            pSTR = 0x13e8;
            pDEX = 0x13ec;
            pINT = 0x13ea;

            // Inventory
            cWeaponDura = 0x4d6;
            cWeaponDuraMax = 0x4d8;
            cWeaponDuraMaxOriginal = 0x4da;
            cWeaponIndex = 0x4ba;
            cHelmDura = 0x30E;
            cHelmDuraMax = 0x310;
            cHelmDuraMaxOriginal = 0x312;
            cHelmIndex = 0;
            invItemIndexOne = 0x5e8;
            invItemIndex = 0x5ea;
            invItemDura = 0x606;
            invItemDuraMax = 0x608;
            invItemDuraMaxOriginal = 0x60a;
            InventoryScrollIndex = oMemory.CalculateStaticAddress(0x135B88);
            InventoryScrollIndexOffset = 0x44;

            // Warehouse
            WH_SelectedIndex = oMemory.CalculateStaticAddress(0x135990);
            WH_InventSelectedIndex = oMemory.CalculateStaticAddress(0x13598c);

            // Crafting
            CraftType = oMemory.CalculateStaticAddress(0x1356AC);           
            CraftSlot_1 = oMemory.CalculateStaticAddress(0x13552C);
            CraftSlot_2 = oMemory.CalculateStaticAddress(0x135578);
            CraftSlot_3 = oMemory.CalculateStaticAddress(0x1355C4);
            
            // Shop
            ShopInventorySelectedIndex = oMemory.CalculateStaticAddress(0x13626C);
            ShopInventoryCount = oMemory.CalculateStaticAddress(0x136248);
            ShopSelectedIndex = oMemory.CalculateStaticAddress(0x136270);

            SelectedSpellIndex = oMemory.CalculateStaticAddress(0x1366AC);
            PotionSkillOffset = 0x1418;
            ZonePenalty = oMemory.CalculateStaticAddress(0x2D9FB8);
            DialogueWindowIndex = oMemory.CalculateStaticAddress(0x132850);
            ExtractDialogue = oMemory.CalculateStaticAddress(0x1325D8);

            // Magic Belt
            MagicBeltArrayStart = oMemory.CalculateStaticAddress(0x136678);
            MagicDistance = 0x20;
            MagicUsageMP = 0x22;
            MagicName = 0x18;
            MagicPower = 0x30;
        }

        // Magic
        public short GetSpellDistance(int Index)
        {
            return (short)oMemory.ReadShort(MagicBeltArrayStart + (Index * 0x4), new int[] { MagicDistance });    
        }
        public short GetSpellUsageMP(int Index)
        {
            return (short)oMemory.ReadShort(MagicBeltArrayStart + (Index * 0x4), new int[] { MagicUsageMP });    
        }
        public string GetSpellName(int Index)
        {
            // Get the address of the spells name with this pointer
            int vMagicNameAddress = (int)oMemory.ReadInt(MagicBeltArrayStart + (Index * 0x4), new int[] { MagicName });

            // Get the spell name
            char[] SpellName = oMemory.ReadString(vMagicNameAddress, 18, 0).ToCharArray();
            
            // Remove the NULL chars
            String NewSpellName = String.Empty;
            for (int i = 0; i < SpellName.Length; i++)  {
                if (char.IsDigit(SpellName[i])){
                    NewSpellName += SpellName[i];
                    break;
                }
                if (char.IsLetter(SpellName[i]) || char.IsWhiteSpace(SpellName[i]))  {
                    NewSpellName += SpellName[i];
                }
            }

            return NewSpellName;
        }
        public short GetSpellPower(int Index)
        {
            return (short)oMemory.ReadShort(MagicBeltArrayStart + (Index * 0x4), new int[] { MagicPower });
        }

        // Health, Mana, Stamin, Bagweight etc
        public int CurrentHP()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { HP_Cur });
        }
        private int MaximumHP()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { HP_Max });
        }
        private int CurrentMP()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { MP_Cur });
        }
        private int MaximumMP()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { MP_Max });
        }
        public int CurrentBagWeight()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { BW_Cur });
        }
        public int MaximumBagWeight()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { BW_Max });
        }
        public int MaximumEXP()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { EXP_Max });
        }
        public int CurrentEXP()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { EXP_Cur });
        }
        public bool CurrentHPBelowPercent(int HealthPercent)
        {
            if (CurrentHP() <= MaximumHP() * HealthPercent / 100)
                return true;
            return false;
        }
        public bool CurrentMPBelowPercent(int ManaPercent)
        {
            if (CurrentMP() <= MaximumMP() * ManaPercent / 100)
                return true;
            return false;
        }
        public bool HasFreeBW()
        {
            if ((oMemory.ReadInt(PlayerBase, new int[] { BW_Max }) - oMemory.ReadInt(PlayerBase, new int[] { BW_Cur })) >= 10)
                return true;
            return false;
        }
        public bool StaminaAbovePercent(int iPercent)
        {
            if (oMemory.ReadInt(PlayerBase, new int[] { Stam_Cur }) >= oMemory.ReadInt(PlayerBase, new int[] { Stam_Max }) * iPercent / 100)
                return true;
            return false;
        }
        public int CurrentBarr()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { Barr });
        }
        public void HideLevel()
        {
            if (oMemory.ReadInt(PlayerBase, new int[] { Level }) != 9999)
                oMemory.Write(PlayerBase, new int[] { Level }, 9999);
        }
        public ushort GetStat(string iType)
        {
            switch (iType.ToUpper())
            {
                case "STR":
                    return oMemory.ReadShort(PlayerBase, new int[] { pSTR });
                case "DEX":
                    return oMemory.ReadShort(PlayerBase, new int[] { pDEX });
                case "INT":
                    return oMemory.ReadShort(PlayerBase, new int[] { pINT });
            }
            return 0;
        }

        // Misc checks
        public bool IsDead()
        {
            if (oMemory.ReadByte(PlayerBase, new int[] { Dead }) == 1)
                return true;
            return false;
        }
        public bool IsAutoAttacking()
        {
            if (oMemory.ReadInt(PlayerBase, new int[] { AutoAttack }) == 16777216)
                return true;
            return false;
        }
        public bool IsWarping()
        {
            if (oMemory.ReadInt(PlayerBase, new int[] { pWarping }) == 1)
                return true;
            return false;
        }
        public bool IsMoving()
        {
            if (oMemory.ReadInt(PlayerBase, new int[] { eMoving }) == 1 || oMemory.ReadInt(PlayerBase, new int[] { eMoving }) == 6)
                return true;
            return false;
        }
        public bool IsWalking()
        {
            if (oMemory.ReadInt(PlayerBase, new int[] { eMoving }) == 1)
                return true;
            return false;
        }
        public bool IsRunning()
        {
            if (oMemory.ReadInt(PlayerBase, new int[] { eMoving }) == 6)
                return true;
            return false;
        }
        public bool IsDevil()
        {
            // Zero for human. >= 10 for devil
            if (oMemory.ReadInt(Race) == 0)
                return false;
            return true;
        }
        public int Test()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { invItemDuraMaxOriginal });
        }
        public string Stance()
        {
            switch (oMemory.ReadByte(PlayerBase, new int[] { Posture }))
            {
                case 0:
                    return "Peace";
                case 1:
                    return "Battle";
                case 2:
                    return "PK";
            }
            return string.Empty;
        }
        public int TargetPTR()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { Target_PTR });
        }
        public bool HasTarget()
        {
            //if (oMemory.ReadInt(PlayerBase, new int[] { Target_PTR }) == 4294967295 || oMemory.ReadInt(PlayerBase, new int[] { Target_PTR }) == 0)
            if ((int)oMemory.ReadInt(PlayerBase, new int[] { Target_PTR }) == -1)
                return false;
            return true;
        }
        public string Zone()
        {
            switch (oMemory.ReadInt(iZone))
            {
                case 1:
                    return "TYT";
                case 4:
                    return "Merc";
                case 5:
                    return "Abias";
                case 6:
                    return "D4";
                case 9:
                    return "Pandi";
                case 11:
                    return "IC";
                case 12:
                    return "VoD";
                case 14:
                    return "Castle";
                case 16:
                    return "Castle Crafting Room";
                case 17:
                    return "Conti";
                case 18:
                    return "Hwan";
            }
            return string.Empty;
        }
        public int Address()
        {
            return (int)oMemory.ReadInt(PlayerBase);
        }
        public int UserMotion()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { pUserMotion });
        }
        public bool HasZonePenalty()
        {
            if (oMemory.ReadInt(ZonePenalty) == 1)
                return true;
            return false;
        }
        public bool InventoryFull()
        {
            int tmpIndexOffset = invItemIndex;
            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndexOffset }) == 0)
                {
                    return false;
                }
                tmpIndexOffset += 0x4c;
            }
            return true;
        }
        
        // Misc actions
        public void Attack(int MobPTR)
        {
            oMemory.Write(PlayerBase, new int[] { Target_PTR }, MobPTR);
            oMemory.Write(PlayerBase, new int[] { AutoAttack }, 16777217);
            Thread.Sleep(100);
            oMemory.Write(PlayerBase, new int[] { AutoAttack }, 16777216);
        }
        public void SendRevive()
        {
            somaMouse = new Mouse();
            while (IsDead())
            {
                if (IsDevil())
                    somaMouse.SetCoordinates(520, 740);
                else
                    somaMouse.SetCoordinates(550, 720);
                somaMouse.SimulateLeftClick();
                Thread.Sleep(200);
            }
        }
        public void Heal()
        {
            somaMouse = new Mouse();
            int TimeOut = 0,
                pIndex = (int)oMemory.ReadInt(SelectedSpellIndex);
            while (CurrentHPBelowPercent(85))
            {
                somaMouse.SetCoordinates(9999, 9999);
                somaMouse.SimulateRightClick();
                Thread.Sleep(100);
                TimeOut++;
                if (TimeOut >= 120 || IsDead() || CurrentMPBelowPercent(8))  // 10 = 1000ms; 100 = 10,000ms; 
                    break;
            }
            //return true;
        }
        public bool Heal(int SpellIndex)
        {
            somaMouse = new Mouse();
            int TimeOut = 0,
                pIndex = (int)oMemory.ReadInt(SelectedSpellIndex);
            if (SpellIndex <= 11)
                SetSpellKey(SpellIndex);
            while (CurrentHPBelowPercent(85))
            {
                somaMouse.SetCoordinates(9999, 9999);
                somaMouse.SimulateRightClick();
                Thread.Sleep(100);
                TimeOut++;
                if (IsDead())
                    return false;
                if (TimeOut >= 120 || CurrentMPBelowPercent(8))  // 10 = 1000ms; 100 = 10,000ms; 
                    break;

            }
            SetSpellKey(pIndex);
            return true;
        }
        public void SetSpellKey(int SpellIndex)
        {
            /*
            if (SpellIndex == 12)
                SpellIndex = 0;
            if (SpellIndex == -1)
                SelectedSpellIndex = 11;
             */
            oMemory.Write(SelectedSpellIndex, SpellIndex);           
        }
        public int GetSpellKey()
        {
            return (int)oMemory.ReadInt(SelectedSpellIndex);
        }
        public void ResetAutoAttack()
        {
            if (oMemory.ReadInt(PlayerBase, new int[] {  AutoAttack }) != 0)
                oMemory.Write(PlayerBase, new int[] { AutoAttack }, 0);
        }
        public void ToggleStamina(bool On, bool ThroughMemory)
        {
            if (ThroughMemory)
            {
                if (On)
                {
                    while (oMemory.ReadInt(StaminaCheck) == 0)
                    {
                        oMemory.Write(StaminaCheck, 1);
                        Thread.Sleep(250);
                    }
                }
                else
                {
                    while (oMemory.ReadInt(StaminaCheck) == 1)
                    {
                        oMemory.Write(StaminaCheck, 0);
                        Thread.Sleep(250);
                    }
                }
            }
            else
            {
                if (On)
                {
                    while (oMemory.ReadInt(StaminaCheck) == 0)
                    {
                        PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.ControlKey, 0);
                        PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.ControlKey, 0);
                        Thread.Sleep(200);
                    }
                }
                else
                {
                    while (oMemory.ReadInt(StaminaCheck) == 1)
                    {
                        PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.ControlKey, 0);
                        PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.ControlKey, 0);
                        Thread.Sleep(200);
                    }
                }
            }
        }
        public void SetStance(string iStance)
        {
            switch (iStance)
            {
                case "Peace":
                    while (oMemory.ReadInt(PlayerBase, new int[] { Posture }) != 0)
                    {
                        PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.Tab, 0);
                        PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.Tab, 0);
                        Thread.Sleep(500);
                    }
                    //oMemory.Write(PlayerBase, new int[] { Posture }, 0);
                    break;
                case "Battle":
                    while (oMemory.ReadInt(PlayerBase, new int[] { Posture }) != 1)
                    {
                        PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.Tab, 0);
                        PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.Tab, 0);
                        Thread.Sleep(500);
                    }
                    //oMemory.Write(PlayerBase, new int[] { Posture }, 1);
                    break;
                case "PK":
                    while (oMemory.ReadInt(PlayerBase, new int[] { Posture }) != 2)
                    {
                        PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.Tab, 0);
                        PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.Tab, 0);
                        Thread.Sleep(500);
                    }
                    //oMemory.Write(PlayerBase, new int[] { Posture }, 2);
                    break;
            }
        }
        public void Shift()
        {        
            //if (ShiftOn)
            {
                // Press the Control key.
                //PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.ShiftKey, 0x402A0001); 
               // PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.ShiftKey, 0);  
                //SendKeys.SendWait("+");
            }
            //else
            {
                //SendKeys.SendWait("+");
               // PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.ShiftKey, 0);
            }
        }
        public bool HasItem(string gItem)
        {
            int gItemIndex = 0,
                tmpIndexOffset = invItemIndex;

            gItem = gItem.ToLower();

            switch (gItem)
            {
                case "frying pan":
                    gItemIndex = 2569;
                    break;
                case "staff":
                    gItemIndex = 2053;
                    break;
            }
            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndexOffset }) == gItemIndex)
                {
                    //MessageBox.Show("Found " + gItem + " at index: " + i.ToString());
                    pItemIndex = i;
                    if (gItem == "staff")
                        NewWeaponIndex = pItemIndex;
                    return true;
                }
                tmpIndexOffset += 0x4c;
            }
            return false;
        }
        public bool UseItem(string gItem)
        {
            somaMouse = new Mouse();
            int NewX = 330,
                NewY = 560,
                ScrollIndex = 0;

            if (pItemIndex >= 10 && pItemIndex <= 19)
            {
                pItemIndex -= 10;
                ScrollIndex = 2;
            }
            else if (pItemIndex >= 20 && pItemIndex <= 29)
            {
                pItemIndex -= 20;
                ScrollIndex = 4;
            }
            else if (pItemIndex >= 30 && pItemIndex <= 39)
            {
                pItemIndex -= 30;
                ScrollIndex = 6;
            }
            // Inventory has to be open for the scroll index to be changed through memory
            while (!WindowOpen("Inventory"))
            {
                SendKeys.SendWait("{F10}");
                Thread.Sleep(200);
            }
            oMemory.Write(InventoryScrollIndex, new int[] { InventoryScrollIndexOffset }, ScrollIndex);
            if (IsDevil())
            {
                NewX = 290;
                NewY = 540;
            }
            if (pItemIndex >= 5)
            {
                // Second inventory row
                NewY += 65;
                NewX = NewX + ((pItemIndex - 5) * 55);
            }
            else
            {
                // First inventory row
                NewX = NewX + (pItemIndex * 55);
            }
            Thread.Sleep(200);
            switch (gItem.ToLower())
            {
                case "frying pan":
                    do
                    {
                        somaMouse.SetCoordinates(NewX, NewY);
                        somaMouse.SimulateRightClick();
                        Thread.Sleep(1000);
                    }
                    while (!WindowOpen("Craft"));
                    return true;
            }
            /*
            while (WindowOpen("Inventory"))
            {
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
            */
            return false;
        }

        // Crafting stuff
        private int CraftItemCountOffset, pItemIndex;
        public bool WindowOpen(string iWindow)
        {
            if (oMemory.ReadInt(DialogueWindowIndex) != 0)
            {
                switch (iWindow.ToLower())
                {
                    case "warehouse":
                        if (oMemory.ReadInt(DialogueWindowIndex) == 5462160)
                            return true;
                        break;
                    case "input":
                        if (oMemory.ReadInt(DialogueWindowIndex) == 5458056 || oMemory.ReadInt(DialogueWindowIndex) == 5458280 || oMemory.ReadInt(DialogueWindowIndex) == 5457832)
                            return true;
                        break;
                    case "vendor":
                        if (oMemory.ReadInt(DialogueWindowIndex) == 5464416)
                            return true;
                        break;
                    case "dialogueselection":
                        if (oMemory.ReadInt(DialogueWindowIndex) == 5449520)
                            return true;
                        break;
                    case "craft":
                        if (oMemory.ReadInt(DialogueWindowIndex) == 5461016)
                            return true;
                        break;
                    case "inventory":
                        if (oMemory.ReadInt(DialogueWindowIndex) == 5462696)
                            return true;
                        break;
                    case "extract":
                        if (oMemory.ReadInt(ExtractDialogue) == 1 || oMemory.ReadInt(DialogueWindowIndex) == 5448992)
                            return true;
                        break;
                }
            }
            return false;
        }
        public bool HasWindowOpen()
        {
            if (oMemory.ReadInt(DialogueWindowIndex) == 0)
                return false;
            return true;
        }
        public bool OpenNPC(int npcX, int npcY, int npcPtr, string iNPC)
        {
            somaMouse = new Mouse();
            int TimeOut = 0; 
            while (IsMoving())
            {
                Thread.Sleep(1000);
            }          
            while (!WindowOpen(iNPC))
            {
                somaMouse.MoveMouseToPoint(npcX, npcY);
                if (somaMouse.MousePTR() == npcPtr)
                    somaMouse.SimulateLeftClick();
                TimeOut++;
                if (TimeOut >= 10)
                    return false;
                Thread.Sleep(200);
            }
            // Extra wait just to make sure the window is up
            Thread.Sleep(1500);
            return true;
        }
        public bool OpenNPC(int npcX, int npcY, int npcPtr, string iNPC, int DialogueIndex)
        {
            somaMouse = new Mouse();
            int TimeOut = 0;
            string pNPC = iNPC;
            iNPC = "DialogueSelection";
            while (IsMoving())
            {
                Thread.Sleep(250);
            }
            Thread.Sleep(2000);
            while (!WindowOpen(iNPC))
            {
                Thread.Sleep(200);
                somaMouse.MoveMouseToPoint(npcX, npcY);
                if (somaMouse.MousePTR() == npcPtr)
                    somaMouse.SimulateLeftClick();
                TimeOut++;
                if (TimeOut >= 10)
                    return false;
            }
            Thread.Sleep(2000);
            iNPC = pNPC; TimeOut = 0;
            int Y = 195 + (DialogueIndex * 17);
            while (!WindowOpen(iNPC))
            {
                Thread.Sleep(200);
                somaMouse.SetCoordinates(530, Y);
                somaMouse.SimulateLeftClick();              
                if (TimeOut >= 10)
                    return false;
            }
            // Extra wait just to make sure the window is up
            Thread.Sleep(2500);
            return true;
        }
        public bool WithDraw(int Index, int Amount, bool CloseDialogue)
        {
            somaMouse = new Mouse();
            int TimeOut = 0;
            while (oMemory.ReadInt(WH_SelectedIndex) != Index)
            {
                oMemory.Write(WH_SelectedIndex, Index);
                Thread.Sleep(200);
            }
            while (!WindowOpen("Input"))
            {
                somaMouse.SetCoordinates(450, 490);
                somaMouse.SimulateLeftClick();
                Thread.Sleep(200);
            }
            SendKeys.SendWait(Amount.ToString());           
            while (WindowOpen("Input"))
            {
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(500);
                TimeOut++;
                if (TimeOut >= 10)
                    return false;               
            }
            if (CloseDialogue)
            {
                while (WindowOpen("Warehouse"))
                {
                    SendKeys.SendWait("{ESC}");
                    Thread.Sleep(200);
                }
            }
            return true;
        }
        public bool Deposit(int InventoryPosition, bool CloseDialogue)
        {
            somaMouse = new Mouse();
            int TimeOut = 0;
            while (oMemory.ReadInt(WH_InventSelectedIndex) != InventoryPosition)
            {
                oMemory.Write(WH_InventSelectedIndex, InventoryPosition);
                Thread.Sleep(200);
            }
            while (!WindowOpen("Input"))
            {
                somaMouse.SetCoordinates(450, 390);
                somaMouse.SimulateLeftClick();
                Thread.Sleep(200);
            }
            while (WindowOpen("Input"))
            {
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(500);
                TimeOut++;
                if (TimeOut >= 10)
                    return false;
            }
            if (CloseDialogue)
            {
                while (WindowOpen("Warehouse"))
                {
                    SendKeys.SendWait("{ESC}");
                    Thread.Sleep(200);
                }
            }
            return true;
        }
        public void SellAllItems(int InventoryPosition)
        {
            somaMouse = new Mouse();
            while (oMemory.ReadInt(ShopInventoryCount) > 3)
            {             
                //if (oMemory.ReadByte(ShopInventorySelectedIndex) == 255)
                {
                    oMemory.Write(ShopInventorySelectedIndex, InventoryPosition);
                    somaMouse.SetCoordinates(455, 410);
                    somaMouse.SimulateLeftClick();
                }
                Thread.Sleep(10);
            }
            while (WindowOpen("Vendor"))
            {
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
        }
        public bool BuyItem(int Index, int Amount)
        {
            somaMouse = new Mouse();
            int TimeOut = 0,
                pBagweight = CurrentBagWeight();

            while (oMemory.ReadInt(ShopSelectedIndex) != Index)
            {
                oMemory.Write(ShopSelectedIndex, Index);
                Thread.Sleep(200);
            }
            while (!WindowOpen("Input"))
            {
                somaMouse.SetCoordinates(450, 510);
                somaMouse.SimulateLeftClick();
                Thread.Sleep(200);
            }
            SendKeys.SendWait(Amount.ToString());
            while (WindowOpen("Input"))
            {
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(500);
                TimeOut++;
                if (TimeOut >= 10)
                    return false;
            }
            if (pBagweight == CurrentBagWeight())
                return false;
            while (WindowOpen("Vendor"))
            {
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
            return true;
        }
        public void EnterCraftItemName()
        {
            somaMouse = new Mouse();
            string ItemName = System.IO.Path.GetRandomFileName();
            ItemName = ItemName.Replace(".", "");
            var rVar = new Random();
            int rNum = rVar.Next(1, ItemName.Length);
            ItemName = ItemName.Remove(0, rNum);

            while (!WindowOpen("Input"))
            {
                somaMouse.SetCoordinates(330, 425);
                somaMouse.SimulateLeftClick();
                Thread.Sleep(200);
            }
            SendKeys.SendWait(ItemName);
            while (WindowOpen("Input"))
            {               
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(500);
            }
        }
        public void SetCraftType(string iType)
        {
            int WriteMe = 0;
            switch (iType.ToLower())
            {
                case "shoes":
                    WriteMe = 15;
                    break;
                case "cook":
                    WriteMe = 21;
                    break;
                case "knux":
                    WriteMe = 7;
                    break;
            }
            while (oMemory.ReadInt(CraftType) != WriteMe)
            {
                oMemory.Write(CraftType, WriteMe);
                Thread.Sleep(200);
            }
        }
        public void InputCraftMaterial(string FirstItemAmount, string SecondItemAmount, string ThirdItemAmount)
        {
            while (oMemory.ReadInt(CraftSlot_1) != int.Parse(FirstItemAmount))
            {
                Thread.Sleep(100);
                oMemory.Write(CraftSlot_1, int.Parse(FirstItemAmount));
            }
            if (SecondItemAmount != string.Empty)
            {
                while (oMemory.ReadInt(CraftSlot_2) != int.Parse(SecondItemAmount))
                {
                    Thread.Sleep(100);
                    oMemory.Write(CraftSlot_2, int.Parse(SecondItemAmount));
                }
            }
            if (ThirdItemAmount != string.Empty)
            {

                while (oMemory.ReadInt(CraftSlot_3) != int.Parse(ThirdItemAmount))
                {
                    Thread.Sleep(100);
                    oMemory.Write(CraftSlot_3, int.Parse(ThirdItemAmount));
                }
            }
        }
        public void InputCraftMaterial(int FirstItemAmount, int SecondItemAmount)
        {
            while (oMemory.ReadInt(CraftSlot_1) != FirstItemAmount)
            {
                Thread.Sleep(100);
                oMemory.Write(CraftSlot_1, FirstItemAmount);
            }
            while (oMemory.ReadInt(CraftSlot_2) != SecondItemAmount)
            {
                Thread.Sleep(100);
                oMemory.Write(CraftSlot_2, SecondItemAmount);
            }
        }
        public void InputCraftMaterial(int FirstItemAmount)
        {
            while (oMemory.ReadInt(CraftSlot_1) != FirstItemAmount)
            {
                Thread.Sleep(250);
                oMemory.Write(CraftSlot_1, FirstItemAmount);
            }   
        }
        public bool StillHasMats(int Threshold)
        {
            if (oMemory.ReadShort(PlayerBase, new int[] { CraftItemCountOffset }) > Threshold)
                return true;
            return false;
        }
        public string Craft(int GoUntil)
        {
            ToggleStamina(false, false);
            somaMouse = new Mouse();
            int tmpIndexOffset = invItemIndex;
            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndexOffset }) != 0)
                {
                    //MessageBox.Show("Found item at index: " + i.ToString());
                    CraftItemCountOffset = (invItemDura += (0x4c * i));
                    //MessageBox.Show(oMemory.ReadShort(PlayerBase, new int[] { CraftItemCountOffset }).ToString());
                    break;
                }
                tmpIndexOffset += 0x4c;
            }
            do
            {
                somaMouse.SetCoordinates(565, 425);
                somaMouse.SimulateLeftClick();
                Thread.Sleep(250);
                if (InventoryFull())
                    break;
            } while (oMemory.ReadShort(PlayerBase, new int[] { CraftItemCountOffset }) > GoUntil);         
            Thread.Sleep(1000);
            ToggleStamina(true, false);
            while (WindowOpen("Craft"))
            {
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
            return "Done";
        }     

        // Extraction stuff
        public void Extract()
        {
            somaMouse = new Mouse();
            int tmpIndexOffset = invItemIndex;
            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndexOffset }) != 0)
                {
                    //MessageBox.Show("Found item at index: " + i.ToString());
                    CraftItemCountOffset = (invItemDura += (0x4c * i));
                    //MessageBox.Show(oMemory.ReadShort(PlayerBase, new int[] { CraftItemCountOffset }).ToString());
                    break;
                }
                tmpIndexOffset += 0x4c;
            }
            while (IsMoving())
            {
                Thread.Sleep(200);
            }
            ToggleStamina(false, false);
            while (oMemory.ReadShort(PlayerBase, new int[] { CraftItemCountOffset }) > 1)
            {
                somaMouse.MoveMouseToPoint(18, 30);
                if (somaMouse.MousePTR() == 26403)
                    somaMouse.SimulateLeftClick();
                if (WindowOpen("extract"))
                    oMemory.Write(ExtractDialogue, 0);
                Thread.Sleep(10);
            }
            somaMouse.MoveMouseToPoint(18, 40);
            while (WindowOpen("extract"))
            {
                SendKeys.SendWait("{F10}");
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
            ToggleStamina(true, false);
        }
        public void LocateExtractionItem(int ItemIndex)
        {
            int tmpIndexOffset = invItemIndex;
            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndexOffset }) == ItemIndex)
                {
                    //MessageBox.Show("Found item at index: " + i.ToString());
                    CraftItemCountOffset = (invItemDura += (0x4c * i));
                    //MessageBox.Show("Count: " + oMemory.ReadShort(PlayerBase, new int[] { CraftItemCountOffset }).ToString());
                    break;
                }
                tmpIndexOffset += 0x4c;
            }
        }
    
        // Weapon stuff
        private int NewWeaponIndex;
        public string WeaponType()
        {
            switch (oMemory.ReadInt(PlayerBase, new int[] { cWeaponType }))
            {
                case 0:
                    return "Fist";
                case 1:
                    return "Sword";
                case 2:
                    return "Sword";
                case 3:
                    return "Spear";
                case 4:
                    return "Bow";
                case 5:
                    return "Axe";
                case 6:
                    return "Staff";
                case 7:
                    return "Bow";
                case 9:
                    return "Axe";
            }
            return string.Empty;
        }
        public bool HasWeapon()
        {
            if (oMemory.ReadShort(PlayerBase, new int[] { cWeaponIndex }) == 0)
                return false;
            return true;
        }
        public bool WeaponDuraLow()
        {
            if (oMemory.ReadShort(PlayerBase, new int[] { cWeaponDura }) < oMemory.ReadShort(PlayerBase, new int[] { cWeaponDuraMaxOriginal }) * 13 / 100)
                return true;
            return false;
        }
        public bool HasNewWeapon()
        {
            int tmpIndex = invItemIndex,
                tmpDura = invItemDura,
                tmpDuraMaxOriginal = invItemDuraMaxOriginal;

            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndex }) == oMemory.ReadShort(PlayerBase, new int[] { cWeaponIndex }))
                {
                    //MessageBox.Show("Found new weapon at index: " + i.ToString());
                    if (oMemory.ReadShort(PlayerBase, new int[] { tmpDura }) > oMemory.ReadShort(PlayerBase, new int[] { tmpDuraMaxOriginal }) * 13 / 100)
                    {
                        //MessageBox.Show("New weapon dura ok");
                        NewWeaponIndex = i;
                        return true;
                    }
                }
                tmpIndex += 0x4c;
                tmpDura += 0x4c;
                tmpDuraMaxOriginal += 0x4c;
            }
            return false;
        }
        public void ChangeWeapon(bool ChangingStaff, bool CloseDialogue)
        {
            somaMouse = new Mouse();
            int NewX = 330,
                NewY = 560,
                ScrollIndex = 0,
                previousDura = oMemory.ReadShort(PlayerBase, new int[] { cWeaponDura }),
                previousIndex = oMemory.ReadShort(PlayerBase, new int[] { cWeaponIndex });

            //MessageBox.Show(oMemory.ReadInt(InventoryScrollIndex, new int[] { InventoryScrollIndexOffset }).ToString());
            if (NewWeaponIndex >= 10 && NewWeaponIndex <= 19)
            {
                NewWeaponIndex -= 10;
                ScrollIndex = 2;
            }
            else if (NewWeaponIndex >= 20 && NewWeaponIndex <= 29)
            {
                NewWeaponIndex -= 20;
                ScrollIndex = 4;
            }
            else if (NewWeaponIndex >= 30 && NewWeaponIndex <= 39)
            {
                NewWeaponIndex -= 30;
                ScrollIndex = 6;
            }
            HideLevel();
            // Inventory has to be open for the scroll index to be changed through memory
            while (!WindowOpen("Inventory"))
            {
                SendKeys.SendWait("{F10}");
                Thread.Sleep(200);
            }
            oMemory.Write(InventoryScrollIndex, new int[] { InventoryScrollIndexOffset }, ScrollIndex);           
            if (IsDevil())
            {
                NewX = 290;
                NewY = 540;
            }
            if (NewWeaponIndex >= 5)
            {
                // Second inventory row
                NewY += 65;
                NewX = NewX + ( (NewWeaponIndex - 5) * 55);
            }
            else
            {
                // First inventory row
                NewX = NewX + (NewWeaponIndex * 55);   
            }     
            Thread.Sleep(200);
            if (!ChangingStaff)
            {
                do
                {                   
                    somaMouse.SetCoordinates(NewX, NewY);
                    somaMouse.SimulateRightClick();
                    Thread.Sleep(1000);
                }
                while (oMemory.ReadShort(PlayerBase, new int[] { cWeaponDura }) == previousDura);
            }
            else
            {
                do
                {
                    somaMouse.SetCoordinates(NewX, NewY);
                    somaMouse.SimulateRightClick();
                    Thread.Sleep(1000);
                }
                while (oMemory.ReadShort(PlayerBase, new int[] { cWeaponIndex }) == previousIndex);
            }
            HideLevel();
            if (CloseDialogue)
            {
                while (WindowOpen("Inventory"))
                {
                    SendKeys.SendWait("{ESC}");
                    Thread.Sleep(200);
                }
            }
        }

        // Armour stuff
        public int cArmourIndex, cArmourDuraOffset;
        public string ArmourDuraLow()
        {
            // Armour
            int cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura;
                cArmourIndex = 7;
                return "Helm";
            }
            cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + (0x4c * 3) }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal + (0x4c * 3) }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura + (0x4c * 3);
                cArmourIndex = 775;
                return "Top";
            }
            cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + (0x4c * 4) }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal + (0x4c * 4) }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura + (0x4c * 4);
                cArmourIndex = 1031;
                return "Pads";
            }
            cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + (0x4c * 5) }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal + (0x4c * 5) }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura + (0x4c * 5);
                cArmourIndex = 1287;
                return "Boots";
            }

            // Accessories
            cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + (0x4c * 8) }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal + (0x4c * 8) }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura + (0x4c * 8);
                cArmourIndex = 2312;
                return "LRing";
            }
            cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + (0x4c * 9) }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal + (0x4c * 9) }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura + (0x4c * 9);
                cArmourIndex = 2312;
                return "RRing";
            }
            cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + 0x4c }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal + 0x4c }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura + 0x4c;
                cArmourIndex = 264;
                return "Ear";
            }
            cDura = oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + (0x4c * 2) }); if (cDura < oMemory.ReadShort(PlayerBase, new int[] { cHelmDuraMaxOriginal + (0x4c * 2) }) * 13 / 100 && cDura != 0)
            {
                cArmourDuraOffset = cHelmDura + (0x4c * 2);
                cArmourIndex = 520;
                return "Neck";
            }
            return string.Empty;
        }
        public bool HasShield()
        {
            if (oMemory.ReadShort(PlayerBase, new int[] { cHelmDura + (0x4c * 7) }) == 0)
                return false;

            int tmpIndexOffset = invItemIndex;
            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndexOffset }) == 0)
                {
                    //MessageBox.Show("Shield will be dropped at index: " + i.ToString());
                    cArmourIndex = i;
                    break;
                }
                tmpIndexOffset += 0x4c;
            }
            cArmourDuraOffset = cHelmDura + (0x4c * 7);
            return true;
        }
        public bool EquipShield()
        {
            somaMouse = new Mouse();
            int NewX = 330,
                NewY = 560,
                ScrollIndex = 0,
                TimeOut = 0,
                previousDura = oMemory.ReadShort(PlayerBase, new int[] { cArmourDuraOffset });

            if (cArmourIndex >= 10 && cArmourIndex <= 19)
            {
                cArmourIndex -= 10;
                ScrollIndex = 2;
            }
            else if (cArmourIndex >= 20 && cArmourIndex <= 29)
            {
                cArmourIndex -= 20;
                ScrollIndex = 4;
            }
            else if (cArmourIndex >= 30 && cArmourIndex <= 39)
            {
                cArmourIndex -= 30;
                ScrollIndex = 6;
            }
            // Inventory has to be open for the scroll index to be changed through memory
            while (!WindowOpen("inventory"))
            {
                SendKeys.SendWait("{F10}");
                Thread.Sleep(200);
            }
            oMemory.Write(InventoryScrollIndex, new int[] { InventoryScrollIndexOffset }, ScrollIndex);

            if (IsDevil())
            {
                NewX = 290;
                NewY = 540;
            }
            if (cArmourIndex >= 5)
            {
                // Second inventory row
                NewY += 65;
                NewX = NewX + ((cArmourIndex - 5) * 55);
            }
            else
            {
                // First inventory row
                NewX = NewX + (cArmourIndex * 55);
            }
            Thread.Sleep(200);
            do
            {
                somaMouse.SetCoordinates(NewX, NewY);
                somaMouse.SimulateRightClick();
                Thread.Sleep(1000);
                TimeOut++;
                if (TimeOut >= 8)
                    return false;
            }
            while (oMemory.ReadShort(PlayerBase, new int[] { cArmourDuraOffset }) == previousDura);
            while (WindowOpen("Inventory"))
            {
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
            return true;
        }
        public bool HasNewArmour()
        {
            int tmpIndex = invItemIndex,
                tmpDura = invItemDura,
                tmpDuraMaxOriginal = invItemDuraMaxOriginal;

            for (int i = 0; i < 40; i++)
            {
                if (oMemory.ReadShort(PlayerBase, new int[] { tmpIndex }) == cArmourIndex)
                {
                    if (oMemory.ReadShort(PlayerBase, new int[] { tmpDura }) > oMemory.ReadShort(PlayerBase, new int[] { tmpDuraMaxOriginal }) * 13 / 100)
                    {
                        cArmourIndex = i;
                        return true;
                    }
                }
                tmpIndex += 0x4c;
                tmpDura += 0x4c;
                tmpDuraMaxOriginal += 0x4c;
            }
            return false;
        }
        public bool ChangeArmour()
        {
            somaMouse = new Mouse();
            int NewX = 330,
                NewY = 560,
                ScrollIndex = 0,
                TimeOut = 0,
                previousDura = oMemory.ReadShort(PlayerBase, new int[] { cArmourDuraOffset });

            if (cArmourIndex >= 10 && cArmourIndex <= 19)
            {
                cArmourIndex -= 10;
                ScrollIndex = 2;
            }
            else if (cArmourIndex >= 20 && cArmourIndex <= 29)
            {
                cArmourIndex -= 20;
                ScrollIndex = 4;
            }
            else if (cArmourIndex >= 30 && cArmourIndex <= 39)
            {
                cArmourIndex -= 30;
                ScrollIndex = 6;
            }
            // Inventory has to be open for the scroll index to be changed through memory
            while (!WindowOpen("inventory"))
            {
                SendKeys.SendWait("{F10}");
                Thread.Sleep(200);
            }
            oMemory.Write(InventoryScrollIndex, new int[] { InventoryScrollIndexOffset }, ScrollIndex);

            if (IsDevil())
            {
                NewX = 290;
                NewY = 540;
            }
            if (cArmourIndex >= 5)
            {
                // Second inventory row
                NewY += 65;
                NewX = NewX + ((cArmourIndex - 5) * 55);
            }
            else
            {
                // First inventory row
                NewX = NewX + (cArmourIndex * 55);
            }
            Thread.Sleep(200);            
            do
            {
                somaMouse.SetCoordinates(NewX, NewY);
                somaMouse.SimulateRightClick();
                Thread.Sleep(1000);
                TimeOut++;
                if (TimeOut >= 6)
                    return false;
            }
            while (oMemory.ReadShort(PlayerBase, new int[] { cArmourDuraOffset }) == previousDura);             
            //SendKeys.SendWait("{ESC}");
            return true;
        }
        public bool RemoveArmour(string iPiece)
        {
            somaMouse = new Mouse();
            int aX = 340, aY = 400, // Default location is LRing
                TimeOut = 0,
                previousDura = oMemory.ReadShort(PlayerBase, new int[] { cArmourDuraOffset });

            if (IsDevil())
            {
                aX = 290; aY = 375;
            }
            switch (iPiece.ToLower())
            {
                case "shield":
                    if (IsDevil())
                    {
                        aX += 225;
                        aY -= 60;
                    }
                    else
                    {
                        aX += 210;
                        aY -= 60;
                    }
                    break;
                case "rring":
                    if (IsDevil())
                        aX += 225;
                    else
                        aX += 210;
                    break;
                case "ear":
                    aY -= 110;
                    break;
                case "neck":
                    aY -= 110;
                    if (IsDevil())
                        aX += 225;
                    else
                        aX += 200;
                    break;
            }
            while (!WindowOpen("Inventory"))
            {
                SendKeys.SendWait("{F10}");
                Thread.Sleep(200);
            }
            do
            {
                somaMouse.SetCoordinates(aX, aY);
                somaMouse.SimulateRightClick();
                Thread.Sleep(200);
                TimeOut++;
                if (TimeOut >= 10)
                    return false;
            }
            while (oMemory.ReadShort(PlayerBase, new int[] { cArmourDuraOffset }) == previousDura);
            /*
            while (WindowOpen("Inventory"))
            {
                SendKeys.SendWait("{ESC}");
                Thread.Sleep(200);
            }
             */
            return true;
        }

        // Movement
        public bool MoveToPoint(int X, int Y, int DestinationRange, int TimeOutAsSeconds)
        {
            somaMouse = new Mouse();
            int CenterPosX = 640,
                CenterPosY = 384,
                TimeOut = 0,
                tUnstuck = 0;

            ArrayList xStuck = new ArrayList();
            ArrayList yStuck = new ArrayList();

            if (TimeOutAsSeconds == 0)
                TimeOutAsSeconds = 500;
            else
                TimeOutAsSeconds = int.Parse(Convert.ToString(TimeOutAsSeconds + "000"));

            while (ReturnPointRange(X, Y)[0] >= DestinationRange || ReturnPointRange(X, Y)[1] >= DestinationRange)
            {
                if (!IsMoving())
                {
                    if (X <= XLocation() && Y <= YLocation())
                    {
                        somaMouse.SetCoordinates(CenterPosX - (XLocation() - X) * 50, CenterPosY - (YLocation() - Y) * 20);
                    }
                    if (X >= XLocation() && Y <= YLocation())
                    {
                        somaMouse.SetCoordinates(CenterPosX + (X - XLocation()) * 50, CenterPosY - (YLocation() - Y) * 20);
                    }
                    if (X <= XLocation() && Y >= YLocation())
                    {
                        somaMouse.SetCoordinates(CenterPosX - (XLocation() - X) * 50, CenterPosY - (YLocation() - Y) * 20);
                    }
                    if (X >= XLocation() && Y >= YLocation())
                    {
                        somaMouse.SetCoordinates(CenterPosX + (X - XLocation()) * 50, CenterPosY - (YLocation() - Y) * 20);
                    }
                    if (somaMouse.State() == 0)
                    {
                        somaMouse.SimulateLeftClick();
                    }
                    xStuck.Add(XLocation());
                    //yStuck.Add(XLocation());
                   
                    if (xStuck.Count == 4)
                    {
                        //MessageBox.Show("xStuck is greater than 4");                      
                        int cStuck = 0;
                        foreach (int iCoord in xStuck)
                        {
                            //MessageBox.Show(iCoord.ToString());
                            if (iCoord == XLocation())
                                cStuck++;
                        }
                        //MessageBox.Show("cStuck: " + cStuck.ToString());
                        if (cStuck >= 2)
                        {
                            TimeOut = 0;
                            tUnstuck++;
                            if (tUnstuck >= 2)
                                return false;
                            MoveToPoint(XLocation() + 4, YLocation() + 4, 2, 0);
                        }
                        xStuck.Clear();
                    }
                }              
                if (IsWalking())
                {
                    if (StaminaAbovePercent(5))
                    {
                        ToggleStamina(true, false);
                        Thread.Sleep(1000);
                    }
                }
                //MessageBox.Show(TimeOut.ToString() + "  " + TimeOutAsSeconds.ToString());
                Thread.Sleep(250);
                TimeOut += 250;
                if (TimeOut >= TimeOutAsSeconds)
                    return false;             
            }
            return true;
        }
        public int[] ReturnPointRange(int X, int Y)
        {
            int[] Distance = new int[2];
            if (X <= XLocation() && Y <= YLocation())
            {
                Distance[0] = XLocation() - X;
                Distance[1] = YLocation() - Y;
            }
            if (X >= XLocation() && Y <= YLocation())
            {
                Distance[0] = X - XLocation();
                Distance[1] = YLocation() - Y;
            }
            if (X <= XLocation() && Y >= YLocation())
            {
                Distance[0] = XLocation() - X;
                Distance[1] = Y - YLocation();
            }
            if (X >= XLocation() && Y >= YLocation())
            {
                Distance[0] = X - XLocation();
                Distance[1] = Y - YLocation();
            }
            return Distance;
        }
        public int XLocation()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { X });
        }
        public int YLocation()
        {
            return (int)oMemory.ReadInt(PlayerBase, new int[] { Y });
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x0101;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
    }

    public class Belt
    {
        private Memory oMemory;
        private Player somaPlayer;

        private int BeltBase,
                    BeltItemType,
                    BeltItemCount;

        public int FKeyTP,
                   FKeyHP,
                   FKeyMP;

        public Belt()
        {
            oMemory = new Memory();
            BeltBase = oMemory.CalculateStaticAddress(0x2D8600);
            BeltItemType = 0x11ec;
            BeltItemCount = 0x11e6;
        }

        // TP
        public bool HasTP()
        {
            int itrBeltItemType = BeltItemType,
                itrBeltItemCount = BeltItemCount;
            for (int i = 1; i < 5; i++)
            {
                if (oMemory.ReadShort(BeltBase, new int[] { itrBeltItemCount }) > 0)
                {
                    switch (oMemory.ReadShort(BeltBase, new int[] { itrBeltItemType }))
                    {
                        // Regular town portal scroll
                        case 320:
                            FKeyTP = i;
                            return true;
                        // Regular town portal scroll (Hsoma)
                        case 200:
                            FKeyTP = i;
                            return true;
                        // Infernal caves guild village portal scroll
                        case 1300:
                            FKeyTP = i;
                            return true;
                        // Pandemonium portal scroll
                        case 1600:
                            FKeyTP = i;
                            return true;
                    }
                }
                itrBeltItemType += 0x4c;
                itrBeltItemCount += 0x4c;
            }
            return false;
        }
        public void UseTP()
        {
            somaPlayer = new Player();
            while (!somaPlayer.IsWarping())
            {
                PostMessage(Game.hWndChild, WM_KEYDOWN, ToKeyCode(FKeyTP), 0);
                PostMessage(Game.hWndChild, WM_KEYUP, ToKeyCode(FKeyTP), 0);
                Thread.Sleep(200);
            }
        }

        // Health Potions
        public bool HasHP()
        {
            int itrBeltItemType = BeltItemType,
                itrBeltItemCount = BeltItemCount;
            for (int i = 1; i < 5; i++)
            {
                //MessageBox.Show(oMemory.ReadShort(BeltBase, new int[] { itrBeltItemCount }).ToString());
                if (oMemory.ReadShort(BeltBase, new int[] { itrBeltItemCount }) > 0)
                {
                    //MessageBox.Show(oMemory.ReadShort(BeltBase, new int[] { itrBeltItemType }).ToString());
                    switch (oMemory.ReadShort(BeltBase, new int[] { itrBeltItemType }))
                    {
                        // +10 HP (Hsoma)
                        case 10:
                            FKeyHP = i;
                            return true;
                        // +10 HP (Dsoma)
                        case 13:
                            FKeyHP = i;
                            return true;
                        // +25 HP (Hsoma)
                        case 30:
                            FKeyHP = i;
                            return true;
                        // +20 HP (Dsoma)
                        case 32:
                            FKeyHP = i;
                            return true;
                        // +25 HP (Dsoma)
                        case 39:
                            FKeyHP = i;
                            return true;
                         // +30 HP & MP Energizer Tonic
                        case 80:
                            FKeyHP = i;
                            return true;
                        // +50 HP (Dsoma and Hsoma)
                        case 110:
                            FKeyHP = i;
                            return true;
                        // +100 HP (Dsoma and Hsoma)
                        case 330:
                            FKeyHP = i;
                            return true;
                    }
                }
                itrBeltItemType += 0x4c;
                itrBeltItemCount += 0x4c;
            }
            return false;
        }
        public void UseHP()
        {
            PostMessage(Game.hWndChild, WM_KEYDOWN, ToKeyCode(FKeyHP), 0);
            PostMessage(Game.hWndChild, WM_KEYUP, ToKeyCode(FKeyHP), 0);
        }

        // Mana Potions
        public bool HasMP()
        {
            int itrBeltItemType = BeltItemType,
                itrBeltItemCount = BeltItemCount;
            for (int i = 1; i < 5; i++)
            {
                if (oMemory.ReadShort(BeltBase, new int[] { itrBeltItemCount }) > 0)
                {
                    switch (oMemory.ReadShort(BeltBase, new int[] { itrBeltItemType }))
                    {
                        // +10 MP (Hsoma)
                        case 15:
                            FKeyMP = i;
                            return true;
                        // +15 MP (Dsoma)
                        case 20:
                            FKeyMP = i;
                            return true;
                        // +30 MP (Dsoma)
                        case 37:
                            FKeyMP = i;
                            return true;
                        // +25 MP (Hsoma)
                        case 40:
                            FKeyMP = i;
                            return true;
                        // +37 MP (Dsoma)
                        case 52:
                            FKeyMP = i;
                            return true;
                        // +50 MP (Hsoma)
                        case 130:
                            FKeyMP = i;
                            return true;
                    }
                }
                itrBeltItemType += 0x4c;
                itrBeltItemCount += 0x4c;
            }
            return false;
        }
        public void UseMP()
        {
            PostMessage(Game.hWndChild, WM_KEYDOWN, ToKeyCode(FKeyMP), 0);
            PostMessage(Game.hWndChild, WM_KEYUP, ToKeyCode(FKeyMP), 0);
        }

        // Other
        private int ToKeyCode(int FKey)
        {
            switch (FKey)
            {
                case 1:
                    return (int)Keys.F1;
                case 2:
                    return (int)Keys.F2;
                case 3:
                    return (int)Keys.F3;
                case 4:
                    return (int)Keys.F4;
            }
            return 0;
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
        const int WM_KEYDOWN = 0x100; const int WM_KEYUP = 0x0101;
    }

    public class Mouse
    {
        private Memory oMemory;
        private Player mPlayer;
        private Entity tmpEntity;

        private const int   tmpMouseY = 20,
                            tmpMouseX = 50,
                            CenterPosX = 640,
                            CenterPosY = 384;

        public int CurrentXPosition,
                   CurrentYPosition;

        private int MouseBase,
                    MouseStateBase,
                    MousePTRBase;

        private enum mOffSets : int
        {
            MouseStateOffset = 0x18,
            MouseX = 0x24,
            MouseY = 0x28,
            MouseLeftClick = 0x14,
            MouseRightClick = 0x18,
            MousePTRoff = 0x270
        }

        public Mouse()
        {
            oMemory = new Memory();
            mPlayer = new Player();
            tmpEntity = new Entity(false);
            MouseBase = oMemory.CalculateStaticAddress(0x2DC254);
            MouseStateBase = oMemory.CalculateStaticAddress(0x2DD468);
            MousePTRBase = oMemory.CalculateStaticAddress(0x2e6710);
        }

        // Left Mouse
        public void LeftClick()
        {
            oMemory.Write(MouseBase, new int[] { (int)mOffSets.MouseLeftClick }, 7);
            Thread.Sleep(50);          
        }
        public void SimulateLeftClick()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        // Right Mouse
        public void RightClick()
        {
            oMemory.Write(MouseBase, new int[] { (int)mOffSets.MouseRightClick }, 2);
            Thread.Sleep(50);
            oMemory.Write(MouseBase, new int[] { (int)mOffSets.MouseRightClick }, 0);
        }
        public void SimulateRightClick()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
            Thread.Sleep(10);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }
        public void HoldMouse(string LeftorRight)
        {
            switch (LeftorRight.ToLower())
            {
                case "left":
                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                    break;

                case "right":
                    mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
                    break;
            }            
        }
        public void ReleaseMouse()
        {
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
            Thread.Sleep(50);
            mouse_event(MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
        }

        // Misc
        public int MousePTR()
        {
            return (int)oMemory.ReadInt(MousePTRBase, new int[] { (int)mOffSets.MousePTRoff });
        }
        public int State()
        {
            return (int)oMemory.ReadInt(MouseStateBase, new int[] { (int)mOffSets.MouseStateOffset });
        }
        public void SetCoordinates(int X, int Y)
        {
            oMemory.Write(MouseBase, new int[] { (int)mOffSets.MouseX }, X);
            oMemory.Write(MouseBase, new int[] { (int)mOffSets.MouseY }, Y);
        }
        public void MoveMouseToPoint(int X, int Y)
        {
            if (X <= mPlayer.XLocation() && Y <= mPlayer.YLocation())
            {
                SetCoordinates(CenterPosX - (mPlayer.XLocation() - X) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - Y) * tmpMouseY);
            }
            if (X >= mPlayer.XLocation() && Y <= mPlayer.YLocation())
            {
                SetCoordinates(CenterPosX + (X - mPlayer.XLocation()) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - Y) * tmpMouseY);
            }
            if (X <= mPlayer.XLocation() && Y >= mPlayer.YLocation())
            {
                SetCoordinates(CenterPosX - (mPlayer.XLocation() - X) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - Y) * tmpMouseY);
            }
            if (X >= mPlayer.XLocation() && Y >= mPlayer.YLocation())
            {
                SetCoordinates(CenterPosX + (X - mPlayer.XLocation()) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - Y) * tmpMouseY);
            }
        }
        private void PositionMouseAtEntity()
        {
            
            // If entity is located in the top left quadrant of the game window
            if (tmpEntity.XLocation() <= mPlayer.XLocation() && tmpEntity.YLocation() <= mPlayer.YLocation())
            {
                //MessageBox.Show("TL");
                //SetCoordinates(somaGame.CenterPosX - (mPlayer.XLocation() - tmpEntity.XLocation()) * tmpMouseX, somaGame.CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                SetCoordinates(CenterPosX - (mPlayer.XLocation() - tmpEntity.XLocation()) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                //SetCoordinates(CurrentLocation()[0] - 20, CurrentLocation()[1] - 25);
                SetCoordinates(CurrentLocation()[0] - 5, CurrentLocation()[1] - 25);
            }
            // If entity is located in the top right quadrant of the game window
            if (tmpEntity.XLocation() >= mPlayer.XLocation() && tmpEntity.YLocation() <= mPlayer.YLocation())
            {
                //MessageBox.Show("TR");
                //SetCoordinates(somaGame.CenterPosX + (tmpEntity.XLocation() - mPlayer.XLocation()) * tmpMouseX, somaGame.CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                SetCoordinates(CenterPosX + (tmpEntity.XLocation() - mPlayer.XLocation()) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                SetCoordinates(CurrentLocation()[0] - 5, CurrentLocation()[1] - 25);
            }
            // If entity is located in the bottom left quadrant of the game window
            if (tmpEntity.XLocation() <= mPlayer.XLocation() && tmpEntity.YLocation() >= mPlayer.YLocation())
            {
                //SetCoordinates(somaGame.CenterPosX - (mPlayer.XLocation() - tmpEntity.XLocation()) * tmpMouseX, somaGame.CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                SetCoordinates(CenterPosX - (mPlayer.XLocation() - tmpEntity.XLocation()) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                SetCoordinates(CurrentLocation()[0] - 5, CurrentLocation()[1] + 25);
            }
            // If entity is located in the bottom right quadrant of the game window
            if (tmpEntity.XLocation() >= mPlayer.XLocation() && tmpEntity.YLocation() >= mPlayer.YLocation())
            {
                //SetCoordinates(somaGame.CenterPosX + (tmpEntity.XLocation() - mPlayer.XLocation()) * tmpMouseX, somaGame.CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                SetCoordinates(CenterPosX + (tmpEntity.XLocation() - mPlayer.XLocation()) * tmpMouseX, CenterPosY - (mPlayer.YLocation() - tmpEntity.YLocation()) * tmpMouseY);
                SetCoordinates(CurrentLocation()[0] - 5, CurrentLocation()[1] + 25);
            }
        }
        public bool FoundEntity()
        {
            int xEntity,
                yEntity,
                oyPos;

        Repeat:
            xEntity = tmpEntity.XLocation();
            yEntity = tmpEntity.YLocation();
            //PositionMouseAtEntity();
            oyPos = CurrentLocation()[1];
            for (int b = 0; b < 4; b++)
            {
                for (int i = 0; i < 50; i++)
                {
                    if (xEntity != tmpEntity.XLocation() || yEntity != tmpEntity.YLocation())
                        goto Repeat;
                    SetCoordinates(CurrentLocation()[0], CurrentLocation()[1] - 1);
                    Thread.Sleep(10);
                    if (State() == 1)
                        if (MousePTR() == tmpEntity.ID())
                        {
                            CurrentXPosition = CurrentLocation()[0];
                            CurrentYPosition = CurrentLocation()[1];
                            return true;
                        }
                }
                SetCoordinates(CurrentLocation()[0] + 10, oyPos);
            }       
            return false;
        }
        public int[] CurrentLocation()
        {
            //return new int[] { (int)oMemory.ReadInt(MouseBase, new int[] { (int)mOffSets.MouseX }) };
            return new int[] { (int)oMemory.ReadInt(MouseBase, new int[] { (int)mOffSets.MouseX }), (int)oMemory.ReadInt(MouseBase, new int[] { (int)mOffSets.MouseY }) };
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        private const int   MOUSEEVENTF_ABSOLUTE = 0x8000,
                            MOUSEEVENTF_LEFTDOWN = 0x0002,
                            MOUSEEVENTF_LEFTUP = 0x0004,
                            MOUSEEVENTF_RIGHTDOWN = 0x0008,
                            MOUSEEVENTF_RIGHTUP = 0x0010;
    }

    public class Drops
    {
        private Memory oMemory;
        private Entity somaEntity;
        private Mouse somaMouse;
        private Player somaPlayer;

        private int ItemOnFloorCount,
                    ItemOnFloorCountOffset,
                    ItemOnFloorID;

        private byte total_Drops,
                     amount_Dropped;

        private int EntityX,
                    EntityY;

        public Drops()
        {
            oMemory = new Memory();
            somaEntity = new Entity(false);
            somaMouse = new Mouse();
            ItemOnFloorCount = oMemory.CalculateStaticAddress(0x15950);
            ItemOnFloorCountOffset = 0xa8;
            ItemOnFloorID = oMemory.CalculateStaticAddress(0x131998);
            total_Drops = 0;
            amount_Dropped = 0;     
        }
        public void GetTotalDrops()
        {
            EntityX = somaEntity.XLocation();
            EntityY = somaEntity.YLocation();
            total_Drops = DropsCount();
        }
        public bool NewDropsFound()
        {
            if (DropsCount() > total_Drops)
            {
                amount_Dropped = (byte)(DropsCount() - total_Drops);
                return true;
            }
            return false;
        }
        public byte AmountDropped()
        {
            return amount_Dropped;
        }
        public void PickUp(bool ExtraTimeOutRequired)
        {
            somaPlayer = new Player();
            int TimeOut = 0,
                Threshold = 10,
                pBagweight = 0,
                pBarr = 0,
                Delay = 50,
                CurrentItem = 1;

            if ( (somaPlayer.ReturnPointRange(EntityX, EntityY)[0] + somaPlayer.ReturnPointRange(EntityX, EntityY)[1]) >= 6)
                Threshold += 10;
            if (ExtraTimeOutRequired)
                Threshold += 10;                        // Extra seconds allocated to the time out because we have monsters attacking. Potentially interrupting movement.

        DoOver:
            TimeOut = 0;
            pBagweight = somaPlayer.CurrentBagWeight();
            pBarr = somaPlayer.CurrentBarr();
            while (!HasPlayerPickedUp(pBagweight, pBarr))
            {
                if (!somaPlayer.IsMoving())
                {
                    somaMouse.MoveMouseToPoint(EntityX, EntityY);
                    if (somaMouse.State() == 2)
                    {
                        somaMouse.SimulateLeftClick();
                    }
                }
                TimeOut++;
                if (TimeOut >= Threshold)
                {
                    if (CurrentItem == 2 && amount_Dropped == 2)         // If picking up the second item timed out
                        amount_Dropped = 3;                              // Check to see if it's been dropped in the third item location
                    if (CurrentItem == 4 && amount_Dropped == 4)         
                        amount_Dropped = 5;                              
                    break;
                }
                Thread.Sleep(Delay);
            }
            CurrentItem++;
            if (CurrentItem > amount_Dropped)
            {
                return;
            }
            else
            {
                switch (CurrentItem)
                {
                    case 2:
                        Threshold = 5;
                        EntityY += 2;
                        goto DoOver;
                    case 3:
                        EntityX -= 1;
                        EntityY -= 1;
                        goto DoOver;
                    case 4:
                        EntityX -= 1;
                        EntityY -= 1;
                        goto DoOver;
                    case 5:
                        EntityX += 1;
                        EntityY -= 1;
                        goto DoOver;
                }
            }
        }
                 
        private byte DropsCount()
        {
            return oMemory.ReadByte(ItemOnFloorCount, new int[] { ItemOnFloorCountOffset });
        }
        public bool HasPlayerPickedUp(int pBagweight, int pBarr)
        {
            somaPlayer = new Player();
            if (somaPlayer.CurrentBagWeight() > pBagweight)
                return true;
            else if (somaPlayer.CurrentBarr() > pBarr)
                return true;
            return false;
        }
        private bool DropExcluded()
        {
            //if (oMemory.ReadByte(ItemOnFloorID))
            switch (oMemory.ReadByte(ItemOnFloorID))
            {
                //case 24:           // Troll Ess
                    //return true;
                //case 160:           // Corpse
                    //return true;
                case 192:           // Corpse
                    return true;
                //case 152:           // Metal
                    //return true;    
                //case 224:           // Essences
                    //return true;

            }
            return false;
        }
    }

    public class Game
    {
        private Memory oMemory;
        private Player somaPlayer;

        public static IntPtr hWndParent,
                             hWndChild,
                             hWndChildAfter;

        private static int WindowWidth,
                           WindowHeight,
                           somaProcess;

        public static string dirLocation;

        public Game()
        {
            //dirLocation = string.Empty;
        }
        public void RunSoma()
        {
            using (Process newGame = Process.Start(@dirLocation + "SomaWindow.exe", "192.99.150.23"))
            {
                newGame.WaitForInputIdle();
                hWndParent = newGame.MainWindowHandle;
                somaProcess = newGame.Id;
            }
        }
        public bool IsRunning()
        {
            return false;
        }
        public void ToggleUI(string ShowOrHide)
        {
            oMemory = new Memory();
            somaPlayer = new Player();
            int UIOffset; int UIDisplay;
            if (somaPlayer.IsDevil())
            {
                UIOffset = oMemory.CalculateStaticAddress(0x13650c);
                if (ShowOrHide.ToLower() == "hide")
                {
                    oMemory.Write(UIOffset, 2000);
                    Thread.Sleep(200);
                }
                else
                {
                    oMemory.Write(UIOffset, 240);
                    Thread.Sleep(200);
                }
                /*
                switch (oMemory.ReadInt(UIOffset))
                {
                    case 240:
                        oMemory.Write(UIOffset, 2000);
                        break;

                    case 2000:
                        oMemory.Write(UIOffset, 240);
                        break;
                }
                 */
            }
            else
            {
                UIOffset = oMemory.CalculateStaticAddress(0x136430);
                UIDisplay = oMemory.CalculateStaticAddress(0x1316CC);
                if (ShowOrHide.ToLower() == "hide")
                {
                    oMemory.Write(UIDisplay, 0);
                    Thread.Sleep(200);
                    while (oMemory.ReadInt(UIDisplay) == 0)
                    {
                        oMemory.Write(UIDisplay, 1);
                        Thread.Sleep(200);
                        oMemory.Write(UIOffset, 2000);
                    }
                }
                else
                {
                    while (oMemory.ReadInt(UIDisplay) == 1)
                    {
                        oMemory.Write(UIDisplay, 0);
                        Thread.Sleep(200);
                    }
                }
                    /*
                switch (oMemory.ReadInt(UIDisplay))
                {
                    case 0:
                        oMemory.Write(UIDisplay, 1);
                        Thread.Sleep(200);
                        oMemory.Write(UIOffset, 2000);
                        break;

                    case 1:
                        oMemory.Write(UIDisplay, 0);
                        break;
                }
                     **/
            }
        }
        public void Embed(IntPtr newHandle)
        {        
            SetWindowLong(hWndParent, GWL_STYLE, WS_VISIBLE);              // Hide the program task bar (Bar at the top of the program)
            SetParent(hWndParent, newHandle);                              // Bind the program to our panel handle
            ShowWindowAsync(hWndParent, 3);                                // Maximize the program so it fits in nicely
        }
        public void Release()
        {
            SetParent(hWndParent, IntPtr.Zero);
            SetWindowLong(hWndParent, GWL_STYLE, (0x00800000));
        }
        public bool SetDirectoryLocation()
        {
            string uninstallKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(uninstallKey))
            {
                foreach (string skName in rk.GetSubKeyNames())
                {
                    if (skName == "Myth of Soma")
                    {
                        RegistryKey skSoma = rk.OpenSubKey("Myth of Soma");
                        dirLocation = skSoma.GetValue("InstallDir").ToString() + "\\";
                    }
                }
            }
            if (dirLocation == string.Empty)
                return false;
            return true;
        }
        public bool SetGameWindowSize()
        {
            string cfgLocation = @dirLocation + "SomaDevLauncher.ini";
            if (System.IO.File.Exists(cfgLocation))
            {
                foreach (string Line in System.IO.File.ReadAllLines(cfgLocation))
                {
                    string[] result = Line.Split('=');
                    switch (result[0])
                    {
                        case "resolution_height":
                            WindowHeight = int.Parse(result[1]);
                            break;

                        case "resolution_width":
                            WindowWidth = int.Parse(result[1]);
                            break;
                    }
                }
            }
            if (WindowHeight != 768 || WindowWidth != 1280)
            {
                var fileContents = System.IO.File.ReadAllText(cfgLocation);
                fileContents = fileContents.Replace(WindowHeight.ToString(), "768");
                fileContents = fileContents.Replace(WindowWidth.ToString(), "1280");
                System.IO.File.WriteAllText(cfgLocation, fileContents);
                WindowHeight = 768; WindowWidth = 1280;
                return false;
            }
            return true;
        }
        public void GiveFocus()
        {
            SetForegroundWindow(hWndParent);
        }
        public bool IsActive()
        {
            if (GetForegroundWindow() == hWndParent)
                return true;
            return false;
        }
        public bool FindProcess(bool UserLaunched)
        {
            hWndChild = IntPtr.Zero;
            oMemory = new Memory();
            if (hWndParent == IntPtr.Zero)
            { 
                hWndParent = FindWindow("Afx:400000:0", null);
            }
            if (hWndParent != IntPtr.Zero)
            {                
                hWndChild = FindWindowEx(hWndParent, IntPtr.Zero, "Afx:400000:b:10003:6:0", null);

                if (hWndChild == IntPtr.Zero)
                    hWndChild = FindWindowEx(hWndParent, IntPtr.Zero, "Afx:400000:b:10005:6:0", null);

                if (hWndChild == IntPtr.Zero)
                    hWndChild = FindWindowEx(hWndParent, IntPtr.Zero, "Afx:400000:b:10011:6:0", null);
                
                if (hWndChild == IntPtr.Zero)
                    hWndChild = FindWindowEx(hWndParent, IntPtr.Zero, "Afx:400000:b:10007:6:0", null);
                
                if (hWndChild != IntPtr.Zero)
                {
                    if (UserLaunched)
                    {
                        if (oMemory.OpenProcess(somaProcess)) {}
                           // MessageBox.Show("Open Process OK");
                        //else
                            //MessageBox.Show("Open Process Fail");
                    }
                    else
                    {
                        if (oMemory.OpenProcess("SomaWindow")) { }
                            //MessageBox.Show("Open Process OK");
                        //else
                            //MessageBox.Show("Open Process Fail"); 
                    }
                    return true;
                }
            }
            return false;
        }
        public void Kill()
        {
            //SendMessage(hWndParent.ToInt32(), 0x0112, 0xF060, 0);
            Process SomaProcess = Process.GetProcessById(oMemory.ReturnmReadProcess());
            SomaProcess.Kill();
        }
        public void Login(string Username, string Password)
        {
            somaPlayer = new Player();
            GiveFocus();
            Thread.Sleep(500); SendKeys.Send(Username);
            Thread.Sleep(250); PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.Tab, 0); PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.Tab, 0);
            Thread.Sleep(500); SendKeys.Send(Password);
            Thread.Sleep(250);
            while (somaPlayer.CurrentHP() == 0)
            {
                PostMessage(Game.hWndChild, WM_KEYDOWN, (int)Keys.Enter, 0);
                PostMessage(Game.hWndChild, WM_KEYUP, (int)Keys.Enter, 0);
                Thread.Sleep(200);
            }            
            SendKeys.SendWait("{ESC}");
        }

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);
        const int WM_KEYDOWN = 0x100; const int WM_KEYUP = 0x0101;

        [DllImport("user32.DLL")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

        const int GWL_STYLE = (-16);
        const UInt32 WS_VISIBLE = 0x10000000;

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);
    }
}
