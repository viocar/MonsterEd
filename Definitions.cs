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
using OriginTablets.Types;

namespace MonsterEd
{
    public partial class MonsterED : Form
    {
        byte[] enemydata_file;
        byte[] enemygraphic_file;
        byte[] enemyhaveskilldata_file;
        Table enemynametable_file;
        byte[] btlscrfiletable_file;
        List<string> btlscrfiletable_strings = new List<string>();
        Table useitemnametable_file;
        MBM dictionary_enemy_explain_file; // this one has underscores in the actual file so it has underscores here
        List<enemydata> enemydata_list = new List<enemydata>();
        List<enemygraphic> enemygraphic_list = new List<enemygraphic>();
        List<enemypassive> enemypassive_list = new List<enemypassive>();
        class enemydata
        {
            ushort level { set; get; } = 0;
            ushort id { set; get; } = 0;
            uint exp { set; get; } = 0;
            uint flag { set; get; } = 0;
            uint floor_id { set; get; } = 0;
            uint hp { set; get; } = 0;
            uint tp { set; get; } = 0;
            ushort str { set; get; } = 0;
            ushort vit { set; get; } = 0;
            ushort agi { set; get; } = 0;
            ushort luc { set; get; } = 0;
            ushort tec { set; get; } = 0;
            ushort wis { set; get; } = 0;
            ushort type { set; get; } = 0;
            ushort acc { set; get; } = 0;
            ushort cut { set; get; } = 0;
            ushort bash { set; get; } = 0;
            ushort stab { set; get; } = 0;
            ushort fire { set; get; } = 0;
            ushort ice { set; get; } = 0;
            ushort volt { set; get; } = 0;
            ushort death { set; get; } = 0;
            ushort petrify { set; get; } = 0;
            ushort sleep { set; get; } = 0;
            ushort panic { set; get; } = 0;
            ushort plague { set; get; } = 0;
            ushort poison { set; get; } = 0;
            ushort blind { set; get; } = 0;
            ushort curse { set; get; } = 0;
            ushort paralyze { set; get; } = 0;
            ushort stun { set; get; } = 0;
            ushort head { set; get; } = 0;
            ushort arm { set; get; } = 0;
            ushort leg { set; get; } = 0;
            ushort almighty { set; get; } = 0;
            ushort drop_1 { set; get; } = 0;
            ushort chance_1 { set; get; } = 0;
            ushort condition_1 { set; get; } = 0;
            ushort drop_2 { set; get; } = 0;
            ushort chance_2 { set; get; } = 0;
            ushort condition_2 { set; get; } = 0;
            ushort drop_3 { set; get; } = 0;
            ushort chance_3 { set; get; } = 0;
            ushort condition_3 { set; get; } = 0;
            public uint this[int i]
            {
                get
                {
                    switch (i)
                    {
                        case 0: return level;
                        case 1: return id;
                        case 2: return exp;
                        case 3: return flag;
                        case 4: return floor_id;
                        case 5: return hp;
                        case 6: return tp;
                        case 7: return str;
                        case 8: return vit;
                        case 9: return agi;
                        case 10: return luc;
                        case 11: return tec;
                        case 12: return wis;
                        case 13: return type;
                        case 14: return acc;
                        case 15: return cut;
                        case 16: return bash;
                        case 17: return stab;
                        case 18: return fire;
                        case 19: return ice;
                        case 20: return volt;
                        case 21: return death;
                        case 22: return petrify;
                        case 23: return sleep;
                        case 24: return panic;
                        case 25: return plague;
                        case 26: return poison;
                        case 27: return blind;
                        case 28: return curse;
                        case 29: return paralyze;
                        case 30: return stun;
                        case 31: return head;
                        case 32: return arm;
                        case 33: return leg;
                        case 34: return almighty;
                        case 35: return drop_1;
                        case 36: return chance_1;
                        case 37: return condition_1;
                        case 38: return drop_2;
                        case 39: return chance_2;
                        case 40: return condition_2;
                        case 41: return drop_3;
                        case 42: return chance_3;
                        case 43: return condition_3;
                        default: throw new ArgumentException("enemydata getter out of range: " + i.ToString() + " (maximum: 43)");
                    }
                }
                set
                {
                    switch (i)
                    {
                        case 0: level = (ushort)value; break;
                        case 1: id = (ushort)value; break;
                        case 2: exp = value; break;
                        case 3: flag = value; break;
                        case 4: floor_id = value; break;
                        case 5: hp = value; break;
                        case 6: tp = value; break;
                        case 7: str = (ushort)value; break;
                        case 8: vit = (ushort)value; break;
                        case 9: agi = (ushort)value; break;
                        case 10: luc = (ushort)value; break;
                        case 11: tec = (ushort)value; break;
                        case 12: wis = (ushort)value; break;
                        case 13: type = (ushort)value; break;
                        case 14: acc = (ushort)value; break;
                        case 15: cut = (ushort)value; break;
                        case 16: bash = (ushort)value; break;
                        case 17: stab = (ushort)value; break;
                        case 18: fire = (ushort)value; break;
                        case 19: ice = (ushort)value; break;
                        case 20: volt = (ushort)value; break;
                        case 21: death = (ushort)value; break;
                        case 22: petrify = (ushort)value; break;
                        case 23: sleep = (ushort)value; break;
                        case 24: panic = (ushort)value; break;
                        case 25: plague = (ushort)value; break;
                        case 26: poison = (ushort)value; break;
                        case 27: blind = (ushort)value; break;
                        case 28: curse = (ushort)value; break;
                        case 29: paralyze = (ushort)value; break;
                        case 30: stun = (ushort)value; break;
                        case 31: head = (ushort)value; break;
                        case 32: arm = (ushort)value; break;
                        case 33: leg = (ushort)value; break;
                        case 34: almighty = (ushort)value; break;
                        case 35: drop_1 = (ushort)value; break;
                        case 36: chance_1 = (ushort)value; break;
                        case 37: condition_1 = (ushort)value; break;
                        case 38: drop_2 = (ushort)value; break;
                        case 39: chance_2 = (ushort)value; break;
                        case 40: condition_2 = (ushort)value; break;
                        case 41: drop_3 = (ushort)value; break;
                        case 42: chance_3 = (ushort)value; break;
                        case 43: condition_3 = (ushort)value; break;
                        default: throw new ArgumentException("enemydata setter out of range: " + i.ToString() + " (maximum: 43)");
                    }
                }
            }
            public byte[] MakeArray()
            {
                byte[] b = new byte[0x64];
                ushort zero = 0;
                ByteWriter(level, b, 0x0);
                ByteWriter(id, b, 0x2);
                ByteWriter(exp, b, 0x4);
                ByteWriter(flag, b, 0x8);
                ByteWriter(floor_id, b, 0xC);
                ByteWriter(hp, b, 0x10);
                ByteWriter(tp, b, 0x14);
                ByteWriter(str, b, 0x18);
                ByteWriter(vit, b, 0x1A);
                ByteWriter(agi, b, 0x1C);
                ByteWriter(luc, b, 0x1E);
                ByteWriter(tec, b, 0x20);
                ByteWriter(wis, b, 0x22);
                ByteWriter(type, b, 0x24);
                ByteWriter(acc, b, 0x26);
                ByteWriter(cut, b, 0x28);
                ByteWriter(bash, b, 0x2A);
                ByteWriter(stab, b, 0x2C);
                ByteWriter(fire, b, 0x2E);
                ByteWriter(ice, b, 0x30);
                ByteWriter(volt, b, 0x32);
                ByteWriter(death, b, 0x34);
                ByteWriter(petrify, b, 0x36);
                ByteWriter(sleep, b, 0x38);
                ByteWriter(panic, b, 0x3A);
                ByteWriter(plague, b, 0x3C);
                ByteWriter(poison, b, 0x3E);
                ByteWriter(blind, b, 0x40);
                ByteWriter(curse, b, 0x42);
                ByteWriter(paralyze, b, 0x44);
                ByteWriter(stun, b, 0x46);
                ByteWriter(head, b, 0x48);
                ByteWriter(arm, b, 0x4A);
                ByteWriter(leg, b, 0x4C);
                ByteWriter(almighty, b, 0x4E);
                if (drop_1 == 0x39C)
                {
                    ByteWriter((ushort)0, b, 0x50); //we don't want to write the literal NONE item here
                }
                else
                {
                    ByteWriter(drop_1, b, 0x50);
                }
                ByteWriter(chance_1, b, 0x52);
                ByteWriter(condition_1, b, 0x54);
                if (drop_2 == 0x39C)
                {
                    ByteWriter((ushort)0, b, 0x56);
                }
                else
                {
                    ByteWriter(drop_2, b, 0x56);
                }
                ByteWriter(chance_2, b, 0x58);
                ByteWriter(condition_2, b, 0x5A);
                if (drop_3 == 0x39C)
                {
                    ByteWriter((ushort)0, b, 0x5C);
                }
                else
                {
                    ByteWriter(drop_3, b, 0x5C);
                }
                ByteWriter(chance_3, b, 0x5E);
                ByteWriter(condition_3, b, 0x60);
                ByteWriter(zero, b, 0x62);
                return b;
            }
        }
        class enemygraphic
        {
            int graphic { set; get; } = 0;
            private int ttd { set; get; } = 0; // inaccessible from the outside
            short sprite { set; get; } = 0;
            short mx { set; get; } = 0;
            short my { set; get; } = 0;
            short sx { set; get; } = 0;
            short sy { set; get; } = 0;
            short hp_bar { set; get; } = 0;
            public int this[int i]
            {
                get
                {
                    switch (i)
                    {
                        case 0: return graphic;
                        case 1: return sprite;
                        case 2: return mx;
                        case 3: return my;
                        case 4: return sx;
                        case 5: return sy;
                        case 6: return hp_bar;
                        default: throw new ArgumentException("enemygraphic getter out of range: " + i.ToString() + " (maximum: 6)");
                    }
                }
                set
                {
                    switch (i)
                    {
                        case 0:
                            graphic = value;
                            ttd = value * 1000;
                            break;
                        case 1: sprite = (short)value; break;
                        case 2: mx = (short)value; break;
                        case 3: my = (short)value; break;
                        case 4: sx = (short)value; break;
                        case 5: sy = (short)value; break;
                        case 6: hp_bar = (short)value; break;
                        default: throw new ArgumentException("enemygraphic setter out of range: " + i.ToString() + " (maximum: 6)");
                    }
                }
            }
            public byte[] MakeArray()
            {
                byte[] b = new byte[0x14];
                ByteWriter(graphic, b, 0x0);
                ByteWriter(ttd, b, 0x4);
                ByteWriter(sprite, b, 0x8);
                ByteWriter(mx, b, 0xA);
                ByteWriter(my, b, 0xC);
                ByteWriter(sx, b, 0xE);
                ByteWriter(sy, b, 0x10);
                ByteWriter(hp_bar, b, 0x12);
                return b;
            }
        }
        class enemypassive
        {
            ushort passive_1 { set; get; } = 0;
            ushort passive_2 { set; get; } = 0;
            ushort passive_3 { set; get; } = 0;
            ushort passive_4 { set; get; } = 0;
            ushort passive_5 { set; get; } = 0;
            ushort passive_6 { set; get; } = 0;
            ushort passive_7 { set; get; } = 0;
            ushort passive_8 { set; get; } = 0;
            ushort passive_9 { set; get; } = 0;
            ushort passive_10 { set; get; } = 0;
            public ushort this[int i]
            {
                get
                {
                    switch (i)
                    {
                        case 0: return passive_1;
                        case 1: return passive_2;
                        case 2: return passive_3;
                        case 3: return passive_4;
                        case 4: return passive_5;
                        case 5: return passive_6;
                        case 6: return passive_7;
                        case 7: return passive_8;
                        case 8: return passive_9;
                        case 9: return passive_10;
                        default: throw new ArgumentException("enemypassive getter out of range: " + i.ToString() + " (maximum: 9)");

                    }
                }
                set
                {
                    switch (i)
                    {
                        case 0: passive_1 = value; break;
                        case 1: passive_2 = value; break;
                        case 2: passive_3 = value; break;
                        case 3: passive_4 = value; break;
                        case 4: passive_5 = value; break;
                        case 5: passive_6 = value; break;
                        case 6: passive_7 = value; break;
                        case 7: passive_8 = value; break;
                        case 8: passive_9 = value; break;
                        case 9: passive_10 = value; break;
                        default: throw new ArgumentException("enemypassive setter out of range: " + i.ToString() + " (maximum: 9)");
                    }
                }
            }
            public byte[] MakeArray()
            {
                byte[] b = new byte[0x14];
                ByteWriter(passive_1, b, 0x0);
                ByteWriter(passive_2, b, 0x2);
                ByteWriter(passive_3, b, 0x4);
                ByteWriter(passive_4, b, 0x6);
                ByteWriter(passive_5, b, 0x8);
                ByteWriter(passive_6, b, 0xA);
                ByteWriter(passive_7, b, 0xC);
                ByteWriter(passive_8, b, 0xE);
                ByteWriter(passive_9, b, 0x10);
                ByteWriter(passive_10, b, 0x12);
                return b;
            }
        }
    }
}