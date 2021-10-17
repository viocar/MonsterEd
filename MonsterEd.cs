using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs; // used for the folder browser
using OriginTablets.Types;
using System.Diagnostics;
using Microsoft.Win32;

namespace MonsterEd
{
    public partial class MonsterED : Form
    {
        public MonsterED()
        {
            InitializeComponent();
        }
        private void b_LoadFile_Click(object sender, EventArgs e)
        {
            string dir = GetInitialDirectory();
            CommonOpenFileDialog d_LoadFolderDialog = new CommonOpenFileDialog()
            {
                InitialDirectory = dir,
                IsFolderPicker = true,
                Title = "Select Etrian Odyssey III Data folder"
            };
            if (d_LoadFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                enemydata_list.Clear();
                enemygraphic_list.Clear();
                enemypassive_list.Clear();
                Properties.Settings.Default.LastPath = Path.GetDirectoryName(d_LoadFolderDialog.FileName);
                Properties.Settings.Default.Save();
                string enemydata_path = d_LoadFolderDialog.FileName + "\\Enemy\\enemydata.tbl";
                string enemygraphic_path = d_LoadFolderDialog.FileName + "\\Enemy\\enemygraphic.tbl";
                string enemyhaveskilldata_path = d_LoadFolderDialog.FileName + "\\Enemy\\enemyhaveskilldata.tbl";
                string enemynametable_path = d_LoadFolderDialog.FileName + "\\Enemy\\enemynametable.tbl";
                string btlscrfiletable_path = d_LoadFolderDialog.FileName + "\\Enemy\\AI\\btlscrfiletable.tbl";
                string useitemnametable_path = d_LoadFolderDialog.FileName + "\\Item\\useitemnametable.tbl";
                string dictionary_enemy_explain_path = d_LoadFolderDialog.FileName + "\\InterfaceFile\\dictionary_enemy_explain.mbm";
                if (File.Exists(enemydata_path) && File.Exists(enemygraphic_path) && File.Exists(enemyhaveskilldata_path) && File.Exists(enemynametable_path) && File.Exists(btlscrfiletable_path) && File.Exists(useitemnametable_path) && File.Exists(dictionary_enemy_explain_path))
                {
                    //enemydata
                    int enemydata_length = (int)(new FileInfo(enemydata_path).Length);
                    enemydata_file = new byte[enemydata_length];
                    using (BinaryReader enemydata_stream = new BinaryReader(new FileStream(enemydata_path, FileMode.Open)))
                    {
                        enemydata_stream.Read(enemydata_file, 0, enemydata_length);
                    }
                    for (int x = 0; x < enemydata_length / 0x64; x++)
                    {
                        enemydata entry = new enemydata();
                        for (int y = 0; y <= 43; y++)
                        {
                            if (y <= 1)
                            {
                                entry[y] = BitConverter.ToUInt16(enemydata_file, x * 0x64 + (y * 2));
                            }
                            else if (y <= 6)
                            {
                                entry[y] = BitConverter.ToUInt32(enemydata_file, x * 0x64 + (4 + (y - 2) * 4));
                            }
                            else
                            {
                                entry[y] = BitConverter.ToUInt16(enemydata_file, x * 0x64 + (0x18 + (y - 7) * 2));
                            }
                        }
                        enemydata_list.Add(entry);
                    }
                    //enemygraphic
                    int enemygraphic_length = (int)(new FileInfo(enemygraphic_path).Length);
                    enemygraphic_file = new byte[enemygraphic_length];
                    using (BinaryReader enemygraphic_stream = new BinaryReader(new FileStream(enemygraphic_path, FileMode.Open)))
                    {
                        enemygraphic_stream.Read(enemygraphic_file, 0, enemygraphic_length);
                    }
                    for (int x = 0; x < enemygraphic_length / 0x14; x++)
                    {
                        enemygraphic entry = new enemygraphic();
                        for (int y = 0; y <= 6; y++)
                        {
                            if (y == 0)
                            {
                                entry[y] = BitConverter.ToInt32(enemygraphic_file, x * 0x14);
                            }
                            else
                            {
                                entry[y] = BitConverter.ToUInt16(enemygraphic_file, x * 0x14 + (8 + (y - 1) * 0x2));
                            }
                        }
                        enemygraphic_list.Add(entry);
                    }
                    for (int x = enemygraphic_length / 0x14; x <= 0xFF; x++)
                    { 
                        {
                            enemygraphic entry = new enemygraphic();
                            enemygraphic_list.Add(entry);
                        }
                    }
                    //enemyhaveskilldata
                    int enemyhaveskilldata_length = (int)(new FileInfo(enemyhaveskilldata_path).Length);
                    enemyhaveskilldata_file = new byte[enemyhaveskilldata_length];
                    using (BinaryReader enemyhaveskilldata_stream = new BinaryReader(new FileStream(enemyhaveskilldata_path, FileMode.Open)))
                    {
                        enemyhaveskilldata_stream.Read(enemyhaveskilldata_file, 0, enemyhaveskilldata_length);
                    }
                    for (int x = 0; x < enemyhaveskilldata_length / 0x14; x++)
                    {
                        enemypassive entry = new enemypassive();
                        for (int y = 0; y <= 9; y++)
                        {
                            entry[y] = BitConverter.ToUInt16(enemyhaveskilldata_file, x * 0x14 + (y * 0x2));
                        }
                        enemypassive_list.Add(entry);
                    }
                    //enemynametable
                    enemynametable_file = new Table(enemynametable_path, false);
                    foreach (string name in enemynametable_file)
                    {
                        tv_Monster.Nodes.Add(name);
                    }
                    //btlscrfiletable
                    int btlscrfiletable_length = (int)(new FileInfo(btlscrfiletable_path).Length);
                    btlscrfiletable_file = new byte[btlscrfiletable_length];
                    using (BinaryReader btlscrfiletable_stream = new BinaryReader(new FileStream(btlscrfiletable_path, FileMode.Open)))
                    {
                        btlscrfiletable_stream.Read(btlscrfiletable_file, 0, btlscrfiletable_length);
                        for (int x = 0; x < btlscrfiletable_length; x = x + 0x20) // need to populate an array of strings
                        {
                            btlscrfiletable_strings.Add(Encoding.ASCII.GetString(btlscrfiletable_file, x, 0x20).Replace("\0", string.Empty));
                        }
                        for (int x = btlscrfiletable_strings.Count; x <= 0x100; x++)
                        {
                            {
                                string scr_str = "scr_test000";
                                btlscrfiletable_strings.Add(scr_str);
                            }
                        }
                    }
                    //useitemnametable
                    useitemnametable_file = new Table(useitemnametable_path, false);
                    //dictionary_enemy_explain
                    dictionary_enemy_explain_file = new MBM(dictionary_enemy_explain_path);
                }
                c_Item1.DataSource = useitemnametable_file;
                c_Item2.BindingContext = new BindingContext(); //need this so it doesn't make all boxes select the same thing
                c_Item2.DataSource = useitemnametable_file;
                c_Item3.BindingContext = new BindingContext();
                c_Item3.DataSource = useitemnametable_file;
            }
        }
        private void b_SaveFile_Click(object sender, EventArgs e)
        {
            string dir = GetInitialDirectory();
            CommonOpenFileDialog d_SaveFolderDialog = new CommonOpenFileDialog()
            {
                InitialDirectory = dir,
                IsFolderPicker = true,
                Title = "Select Etrian Odyssey III Data folder"
            };
            if (d_SaveFolderDialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                byte[] enemydata_save_byte = new byte[enemydata_list.Count * 0x64];
                byte[] enemygraphic_save_byte = new byte[enemydata_list.Count * 0x14]; //we're using enemydata for each entry because if there's a desync, we want it to crash
                byte[] enemyhaveskilldata_save_byte = new byte[enemydata_list.Count * 0x14];
                byte[] btlscrfiletable_save_byte = new byte[enemydata_list.Count * 0x20];
                string enemydata_path = d_SaveFolderDialog.FileName + "\\Enemy\\enemydata.tbl";
                string enemygraphic_path = d_SaveFolderDialog.FileName + "\\Enemy\\enemygraphic.tbl";
                string enemyhaveskilldata_path = d_SaveFolderDialog.FileName + "\\Enemy\\enemyhaveskilldata.tbl";
                string enemynametable_path = d_SaveFolderDialog.FileName + "\\Enemy\\enemynametable.tbl";
                string btlscrfiletable_path = d_SaveFolderDialog.FileName + "\\Enemy\\AI\\BtlScrFileTable.tbl";
                string dictionary_enemy_explain_path = d_SaveFolderDialog.FileName + "\\InterfaceFile\\dictionary_enemy_explain.mbm";
                //enemydata
                for (int x = 0; x < enemydata_list.Count; x++)
                {
                    enemydata_list[x].MakeArray().CopyTo(enemydata_save_byte, x * 0x64);
                }
                File.WriteAllBytes(enemydata_path, enemydata_save_byte);
                //enemygraphic
                for (int x = 0; x < enemygraphic_list.Count; x++)
                {
                    enemygraphic_list[x].MakeArray().CopyTo(enemygraphic_save_byte, x * 0x14);
                }
                File.WriteAllBytes(enemygraphic_path, enemygraphic_save_byte);
                //enemyhaveskilldata
                for (int x = 0; x < enemypassive_list.Count; x++)
                {
                    enemypassive_list[x].MakeArray().CopyTo(enemyhaveskilldata_save_byte, x * 0x14);
                }
                File.WriteAllBytes(enemyhaveskilldata_path, enemyhaveskilldata_save_byte);
                //enemynametable
                enemynametable_file.WriteToFile(enemynametable_path, false);
                //btlscrfiletable
                for (int x = 0; x < enemydata_list.Count; x++)
                {
                    byte[] entry = Encoding.ASCII.GetBytes(btlscrfiletable_strings[x].PadRight(0x20, '\x00'));
                    entry.CopyTo(btlscrfiletable_save_byte, x * 0x20);
                }
                File.WriteAllBytes(btlscrfiletable_path, btlscrfiletable_save_byte);
                //dictionary_enemy_explain
                dictionary_enemy_explain_file.WriteToFile(dictionary_enemy_explain_path);
            }
        }
        private string GetInitialDirectory()
        {
            string dir = "C:\\Emulation\\EOMod\\EO3Rv2\\Data\\@Target"; //my personal data since I'm the main (only?) user of this
            if (Properties.Settings.Default.LastPath != "")
            {
                return Properties.Settings.Default.LastPath;
            }
            if (!Directory.Exists(dir))
            {
                return "%userprofile%\\Documents";
            }
            else
            {
                return dir;
            }
        }
        private void tv_Monster_AfterSelect(object sender, TreeViewEventArgs e)
        {
            PopulateBoxes(tv_Monster.SelectedNode.Index);
        }
        private void tv_Monster_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            //if (tv_Monster.SelectedNode != null)
            //{
            //    UpdateData(sender as TextBox, tv_Monster.SelectedNode.Index);
            //}
        }
        private void rt_Codex_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true; // stops the noise
                dictionary_enemy_explain_file[tv_Monster.SelectedNode.Index] = rt_Codex.Text;
            }
        }
        private void rt_Codex_Leave(object sender, EventArgs e)
        {
            dictionary_enemy_explain_file[tv_Monster.SelectedNode.Index] = rt_Codex.Text;
        }
        private void b_NewEntry_Click(object sender, EventArgs e)
        {
            string none = "NONE";
            string scr_str = "scr_test000";
            enemydata ed = new enemydata();
            enemygraphic eg = new enemygraphic();
            enemypassive ehsd = new enemypassive();
            enemydata_list.Add(ed);
            enemygraphic_list.Add(eg);
            enemypassive_list.Add(ehsd);
            enemynametable_file.Add(none);
            tv_Monster.Nodes.Add(none);
            btlscrfiletable_strings.Add(scr_str);
            dictionary_enemy_explain_file.Add(none);
            useitemnametable_file.Add(none);
        }
        private void TextBoxEntry(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true; // stops the noise
                TextBox caller = sender as TextBox;
                if (tv_Monster.SelectedNode != null)
                {
                    UpdateData(caller, tv_Monster.SelectedNode.Index);
                }
            }
        }
        private void NewBoxSelected(object sender, EventArgs e)
        {
            TextBox caller = sender as TextBox;
            if (tv_Monster.SelectedNode != null)
            {
                UpdateData(caller, tv_Monster.SelectedNode.Index);
            }
        }
        private void UpdateData(TextBox caller, int i)
        {
            int caller_tag = Convert.ToInt32(caller.Tag);
            int value;
            if (int.TryParse(caller.Text, System.Globalization.NumberStyles.HexNumber, null, out value) && (caller_tag < 45 || caller_tag > 49))
            {
                Debug.WriteLine(value);
                switch (caller_tag)
                {
                    case 2: // these use hex values. code mess
                    case 4:
                    case 14:
                    case 37:
                    case 39:
                    case 41:
                    case 43:
                    case int n when n >= 50 && n <= 59:
                        break;
                    default:
                        value = Convert.ToInt32(caller.Text); //you ever get a switch statement backwards and have to invert it in the worst way
                        break;
                }
                switch (caller_tag)
                {
                    case 1:
                        enemydata_list[i][0] = (ushort)value;
                        break;
                    case int n when n >= 2 && n <= 6:
                        enemydata_list[i][n - 1] = (uint)value;
                        break;
                    case int n when n >= 7 && n <= 35:
                        enemydata_list[i][n - 1] = (ushort)value;
                        break;
                    case int n when n == 36 || n == 37:
                        enemydata_list[i][n] = (ushort)value;
                        break;
                    case int n when n == 38 || n == 39:
                        enemydata_list[i][n + 1] = (ushort)value;
                        break;
                    case int n when n == 40 || n == 41:
                        enemydata_list[i][n + 2] = (ushort)value;
                        break;
                    case int n when n >= 43 && n <= 44:
                        enemygraphic_list[i][n - 43] = (short)value;
                        break;
                    case int n when n >= 50 && n <= 59:
                        enemypassive_list[i][n - 50] = (ushort)value;
                        break;
                    default:
                        Console.WriteLine("Invalid");
                        break;
                }
            }
            else if (caller_tag >= 45 && caller_tag <= 49) //I'm just hacking it together at this point
            {
                if (int.TryParse(caller.Text, System.Globalization.NumberStyles.Integer, null, out value))
                {
                    enemygraphic_list[i][caller_tag - 43] = (short)value;
                }
            }
            else
            {
                if (caller_tag == 0)
                {
                    enemynametable_file[i] = caller.Text;
                    tv_Monster.Nodes[i].Text = caller.Text;
                }
                else if (caller_tag == 42)
                {
                    btlscrfiletable_strings[i] = caller.Text;
                }
                else if (caller_tag == 60)
                {
                    //nothin'
                }
                else
                {
                    PopulateBoxes(i); // this might be a bad idea
                }
            }
        }
        private void UpdateItemSelection(object sender, EventArgs e)
        {
            if (tv_Monster.SelectedNode != null)
            {
                int i = tv_Monster.SelectedNode.Index;
                enemydata_list[i][35] = (uint)c_Item1.SelectedIndex + 0x39C;
                enemydata_list[i][38] = (uint)c_Item2.SelectedIndex + 0x39C;
                enemydata_list[i][41] = (uint)c_Item3.SelectedIndex + 0x39C;
            }
        }
        private void PopulateBoxes(int i)
        {
            //TextBoxes
            t_Name.Text = enemynametable_file[i];
            t_Level.Text = enemydata_list[i][0].ToString();
            t_ID.Text = enemydata_list[i][1].ToString("X");
            t_Exp.Text = enemydata_list[i][2].ToString();
            t_Flag.Text = enemydata_list[i][3].ToString("X");
            t_FloorID.Text = enemydata_list[i][4].ToString();
            t_HP.Text = enemydata_list[i][5].ToString();
            t_TP.Text = enemydata_list[i][6].ToString();
            t_STR.Text = enemydata_list[i][7].ToString();
            t_VIT.Text = enemydata_list[i][8].ToString();
            t_AGI.Text = enemydata_list[i][9].ToString();
            t_LUC.Text = enemydata_list[i][10].ToString();
            t_TEC.Text = enemydata_list[i][11].ToString();
            t_WIS.Text = enemydata_list[i][12].ToString();
            t_Type.Text = enemydata_list[i][13].ToString("x");
            t_Acc.Text = enemydata_list[i][14].ToString();
            t_Cut.Text = enemydata_list[i][15].ToString();
            t_Bash.Text = enemydata_list[i][16].ToString();
            t_Stab.Text = enemydata_list[i][17].ToString();
            t_Fire.Text = enemydata_list[i][18].ToString();
            t_Ice.Text = enemydata_list[i][19].ToString();
            t_Volt.Text = enemydata_list[i][20].ToString();
            t_Death.Text = enemydata_list[i][21].ToString();
            t_Petrify.Text = enemydata_list[i][22].ToString();
            t_Sleep.Text = enemydata_list[i][23].ToString();
            t_Panic.Text = enemydata_list[i][24].ToString();
            t_Plague.Text = enemydata_list[i][25].ToString();
            t_Poison.Text = enemydata_list[i][26].ToString();
            t_Blind.Text = enemydata_list[i][27].ToString();
            t_Curse.Text = enemydata_list[i][28].ToString();
            t_Paralyze.Text = enemydata_list[i][29].ToString();
            t_Stun.Text = enemydata_list[i][30].ToString();
            t_Head.Text = enemydata_list[i][31].ToString();
            t_Arm.Text = enemydata_list[i][32].ToString();
            t_Leg.Text = enemydata_list[i][33].ToString();
            t_Almighty.Text = enemydata_list[i][34].ToString();
            t_Chance1.Text = enemydata_list[i][36].ToString();
            t_Condition1.Text = enemydata_list[i][37].ToString("X");
            t_Chance2.Text = enemydata_list[i][39].ToString();
            t_Condition2.Text = enemydata_list[i][40].ToString("X");
            t_Chance3.Text = enemydata_list[i][42].ToString();
            t_Condition3.Text = enemydata_list[i][43].ToString("X");
            t_Script.Text = btlscrfiletable_strings[i];
            t_Graphic.Text = enemygraphic_list[i][0].ToString("X"); // 0x4 is derived from 0x0
            t_Size.Text = enemygraphic_list[i][1].ToString();
            t_MonsterX.Text = enemygraphic_list[i][2].ToString();
            t_MonsterY.Text = enemygraphic_list[i][3].ToString();
            t_ShadowX.Text = enemygraphic_list[i][4].ToString();
            t_ShadowY.Text = enemygraphic_list[i][5].ToString();
            t_HPBarSize.Text = enemygraphic_list[i][6].ToString();
            t_Passive1.Text = enemypassive_list[i][0].ToString("X");
            t_Passive2.Text = enemypassive_list[i][1].ToString("X");
            t_Passive3.Text = enemypassive_list[i][2].ToString("X");
            t_Passive4.Text = enemypassive_list[i][3].ToString("X");
            t_Passive5.Text = enemypassive_list[i][4].ToString("X");
            t_Passive6.Text = enemypassive_list[i][5].ToString("X");
            t_Passive7.Text = enemypassive_list[i][6].ToString("X");
            t_Passive8.Text = enemypassive_list[i][7].ToString("X");
            t_Passive9.Text = enemypassive_list[i][8].ToString("X");
            t_Passive10.Text = enemypassive_list[i][9].ToString("X");
            //ComboBoxes
            uint id_1 = enemydata_list[i][35];
            uint id_2 = enemydata_list[i][38];
            uint id_3 = enemydata_list[i][41];
            c_Item1.SelectedIndex = (int)(id_1 > 0 ? id_1 - 0x39C : 0); //baby's first ternary
            c_Item2.SelectedIndex = (int)(id_2 > 0 ? id_2 - 0x39C : 0);
            c_Item3.SelectedIndex = (int)(id_3 > 0 ? id_3 - 0x39C : 0);
            //RichTextBox
            rt_Codex.Text = dictionary_enemy_explain_file[i];
        }
        static private void ByteWriter<T>(T val, byte[] d_array, int start_offset)
        {
            if (val is byte || val is sbyte) // avoid using this one
            {
                d_array[start_offset] = Convert.ToByte(val);
            }
            else if (val is short)
            {
                short c_val = Convert.ToInt16(val);
                d_array[start_offset] = (byte)(c_val & 0xFF);
                d_array[start_offset + 1] = (byte)((c_val & 0xFF00) >> 8);
            }
            else if (val is ushort)
            {
                ushort c_val = Convert.ToUInt16(val);
                d_array[start_offset] = (byte)(c_val & 0xFF);
                d_array[start_offset + 1] = (byte)((c_val & 0xFF00) >> 8);
            }
            else if (val is int)
            {
                int c_val = Convert.ToInt32(val);
                d_array[start_offset] = (byte)(c_val & 0xFF);
                d_array[start_offset + 1] = (byte)((c_val & 0xFF00) >> 8);
                d_array[start_offset + 2] = (byte)((c_val & 0xFF0000) >> 16);
                d_array[start_offset + 3] = (byte)((c_val & 0xFF000000) >> 24);
            }
            else if (val is uint)
            {
                uint c_val = Convert.ToUInt32(val);
                d_array[start_offset] = (byte)(c_val & 0xFF);
                d_array[start_offset + 1] = (byte)((c_val & 0xFF00) >> 8);
                d_array[start_offset + 2] = (byte)((c_val & 0xFF0000) >> 16);
                d_array[start_offset + 3] = (byte)((c_val & 0xFF000000) >> 24);
            }
            else if (val is string)
            {
                byte[] str_bytes = Encoding.ASCII.GetBytes(Convert.ToString(val));
                for (int x = 0; x < str_bytes.Length; x++)
                {
                    d_array[start_offset + x] = str_bytes[x];
                }
            }
            else
            {
                throw new Exception("Value passed was not one of supported type: byte, sbyte, ushort, short, uint, int, string.");
            }
        }
    }
}
