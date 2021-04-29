using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab1IvanovAGBBO0118DES
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private const int sizeOfBlock = 64;
        private int sizeOfChar = 16;
        private const int shiftKey = 2;
        private int quantityOfRounds = 16;
        string[] Blocks;
        string[] newkeys;
        private readonly int[] G1_table = new int[]
        {
            57, 49, 41, 33, 25, 17, 9, 1, 58, 50, 42, 34, 26, 18,
            10, 2, 59, 51, 43, 35, 27, 19, 11, 3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15, 7, 62, 54, 46, 38, 30, 22,
            14, 6, 61, 53, 45, 37, 29, 21, 13, 5, 28, 20, 12, 4
        };
        private readonly int[] G2_table = new int[]
        {
            14, 17, 11, 24, 1, 5, 3, 28,
            15, 6, 21, 10, 23, 19, 12, 4,
            26, 8, 16, 7, 27, 20, 13, 2,
            41, 52, 31, 37, 47, 55, 30, 40,
            51, 45, 33, 48, 44, 49, 39, 56,
            34, 53, 46, 42, 50, 36, 29, 32,
        };
        private readonly int[] P_box = new int[]
        {
            16, 7, 20, 21, 29, 12, 28, 17,
            1, 15, 23, 26, 5, 18, 31, 10,
            2, 8, 24, 14, 32, 27, 3, 9,
            19, 13, 30, 6, 22, 11, 4, 25,
        };
        private readonly int[,] S1_box = new int[,]
        {
            {14, 4, 13, 1, 2, 15, 11, 8, 3, 10, 6, 12, 5, 9, 0, 7 },
            {0, 15, 7, 4, 14, 2, 13, 1, 10, 6, 12, 11, 9, 5, 3, 8 },
            {4, 1, 14, 8, 13, 6, 2, 11, 15, 12, 9, 7, 3, 10, 5, 0 },
            {15, 12, 8, 2, 4, 9, 1, 7, 5, 11, 3, 14, 10, 0, 6, 13 }
        };
        private readonly int[,] S2_box = new int[,]
        {
            {15, 1, 8, 14, 6, 11, 3, 4, 9, 7, 2, 13, 12, 0, 5, 10 },
            {3, 13, 4, 7, 15, 2, 8, 14, 12, 0, 1, 10, 6, 9, 11, 5 },
            {0, 14, 7, 11, 10, 4, 13, 1, 5, 8, 12, 6, 9, 3, 2, 15 },
            {13, 8, 10, 1, 3, 15, 4, 2, 11, 6, 7, 12, 0, 5, 14, 9 }
        };
        private readonly int[,] S3_box = new int[,]
        {
            {10, 0, 9, 14, 6, 3, 15, 5, 1, 13, 12, 7, 11, 4, 2, 8 },
            {13, 7, 0, 9, 3, 4, 6, 10, 2, 8, 5, 14, 12, 11, 15, 1 },
            {13, 6, 4, 9, 8, 15, 3, 0, 11, 1, 2, 12, 5, 10, 14, 7 },
            {1, 10, 13, 0, 6, 9, 8, 7, 4, 15, 14, 3, 11, 5, 2, 12 }
        };
        private readonly int[,] S4_box = new int[,]
        {
            {7, 13, 14, 3, 0, 6, 9, 10, 1, 2, 8, 5, 11, 12, 4, 15 },
            {13, 8, 11, 5, 6, 15, 0, 3, 4, 7, 2, 12, 1, 10, 14, 9 },
            {10, 6, 9, 0, 12, 11, 7, 13, 15, 1, 3, 14, 5, 2, 8, 4 },
            {3, 15, 0, 6, 10, 1, 13, 8, 9, 4, 5, 11, 12, 7, 2, 14 }
        };
        private readonly int[,] S5_box = new int[,]
        {
            {2, 12, 4, 1, 7, 10, 11, 6, 8, 5, 3, 15, 13, 0, 14, 9 },
            {14, 11, 2, 12, 4, 7, 13, 1, 5, 0, 15, 10, 3, 9, 8, 6 },
            {4, 2, 1, 11, 10, 13, 7, 8, 15, 9, 12, 5, 6, 3, 0, 14 },
            {11, 8, 12, 7, 1, 14, 2, 13, 6, 15, 0, 9, 10, 4, 5, 3 }
        };
        private readonly int[,] S6_box = new int[,]
        {
            {12, 1, 10, 15, 9, 2, 6, 8, 0, 13, 3, 4, 14, 7, 5, 11 },
            {10, 15, 4, 2, 7, 12, 9, 5, 6, 1, 13, 14, 0, 11, 3, 8 },
            {9, 14, 15, 5, 2, 8, 12, 3, 7, 0, 4, 10, 1, 13, 11, 6 },
            {4, 3, 2, 12, 9, 5, 15, 10, 11, 14, 1, 7, 6, 0, 8, 13 }
        };
        private readonly int[,] S7_box = new int[,]
        {
            {4, 11, 2, 14, 15, 0, 8, 13, 3, 12, 9, 7, 5, 10, 6, 1 },
            {13, 0, 11, 7, 4, 9, 1, 10, 14, 3, 5, 12, 2, 15, 8, 6 },
            {1, 4, 11, 13, 12, 3, 7, 14, 10, 15, 6, 8, 0, 5, 9, 2 },
            {6, 11, 13, 8, 1, 4, 10, 7, 9, 5, 0, 15, 14, 2, 3, 12 }
        };
        private readonly int[,] S8_box = new int[,]
        {
            {13, 2, 8, 4, 6, 15, 11, 1, 10, 9, 3, 14, 5, 0, 12, 7 },
            {1, 15, 13, 8, 10, 3, 7, 4, 12, 5, 6, 11, 0, 14, 9, 2 },
            {7, 11, 4, 1, 9, 12, 14, 2, 0, 6, 10, 13, 15, 3, 5, 8 },
            {2, 1, 14, 7, 4, 10, 8, 13, 15, 12, 9, 0, 3, 5, 6, 11 }
        };

        private string StringToRightLength(string input, bool temp)
        {
            while (((input.Length * sizeOfChar) % sizeOfBlock) != 0)
            {
                if (temp) { input += "#"; }
                else { input += " "; }
            }
            return input;
        }

        private void CutStringIntoBlocks(string input)
        {
            MessageBox.Show(input.Length.ToString());
            Blocks = new string[(input.Length * sizeOfChar) / sizeOfBlock];
            int lengthOfBlock = input.Length / Blocks.Length;
            for (int i = 0; i < Blocks.Length; i++)
            {
                Blocks[i] = input.Substring(i * lengthOfBlock, lengthOfBlock);
                Blocks[i] = StringToBinaryFormat(Blocks[i]);
            }
        }
        private string StringToBinaryFormat(string input)
        {
            string output = "";
            //string char_binary = "";
            for (int i = 0; i < input.Length; i++)
            {
            string char_binary = Convert.ToString(input[i], 2);
            //byte[] tempbitstr = Encoding.ASCII.GetBytes(input);
            //string char_binary = tempbitstr.ToString();
            //BitArray tempBA = new BitArray(tempbitstr.ToArray());
            //MessageBox.Show(Encoding.ASCII.GetString(tempBA)); 
            //MessageBox.Show(tempBA.Length.ToString());
            /*for (int i = 0; i < tempBA.Length; i++)
            {
                if(tempBA[i] == true)
                {
                    char_binary = char_binary + "1";
                }
                else
                {
                    char_binary = char_binary + "0";
                }
                MessageBox.Show(char_binary);
            }
            */
            while (char_binary.Length < sizeOfChar)
                {
                    char_binary = "0" + char_binary; // Доводит длину двоичного кода до 8-и
                    //MessageBox.Show(char_binary);
                }
            output += char_binary;
            }
            //MessageBox.Show(output);
            return output;
        }
        /*private string CorrectKeyWord(string input, int lenghKey)
        {

        }
        */
        private string StringFromBinaryToNormalFormat(string input)
        {
            string output = "";

            while (input.Length > 0)
            {
                string char_binary = input.Substring(0, sizeOfChar);
                input = input.Remove(0, sizeOfChar);

                int a = 0;
                int degree = char_binary.Length - 1;

                foreach (char c in char_binary)
                    a += Convert.ToInt32(c.ToString()) * (int)Math.Pow(2, degree--);

                output += ((char)a).ToString();
            }

            return output;
        }
        private string EncodeDES_One_Round(string input, string key)
        {
            string L = input.Substring(0, input.Length / 2);
            string R = input.Substring(input.Length / 2, input.Length / 2);

            //return (R + XOR(L, Sbox(f(Pbox_Ext(R), key))));
            return (R + XOR(L, Pbox(Sbox(f(Pbox_Ext(R), key)))));
        }
        private string Pbox_Ext(string input)
        {
            string result = "";
            for(int i=0; i<8; i++)
            {
                if (i == 0) result += input.Substring(31, 1);
                else
                { result += input.Substring((i * 4) - 1, 1); }
                result += input.Substring((i * 4), 4);
                if (i == 7) result += input.Substring(0, 1);
                else
                { result += input.Substring(((i + 1) * 4), 1); }
                            
            }
            //MessageBox.Show(input + " ||| " + result);
            return result;
        }
        private string Sbox(string input)
        {
            string[] Sboxstr = new string[8];
            string result = "";
            string tempi = "";
            string tempj = "";
            int i, j;
            //MessageBox.Show(input.Length.ToString());
            for (int q = 1; q < 9; q++)
            {
                tempi = input.Substring((q-1)*6, 1) + input.Substring(((q*6)-1), 1);
                tempj = input.Substring((((q-1)*6)+1), 4);
                i = Convert.ToInt32(tempi, 2);
                j = Convert.ToInt32(tempj, 2);
                Sboxstr[q-1]=Convert.ToString(SboxSearch(q, i, j));
                while (Sboxstr[q-1].Length < 4)
                {
                    Sboxstr[q - 1] = "0" + Sboxstr[q - 1]; 
                }
                result += Sboxstr[q - 1];
            }
            return result;
        }
        private int SboxSearch (int n, int _i, int _j)
        {
            int res = 0;
            switch (n)
            {
                case 1: 
                res = S1_box[_i, _j];
                break;
                case 2:
                res = S2_box[_i, _j];
                break;
                case 3:
                res = S3_box[_i, _j];
                break;
                case 4:
                res = S4_box[_i, _j];
                break;
                case 5:
                res = S5_box[_i, _j];
                break;
                case 6:
                res = S6_box[_i, _j];
                break;
                case 7:
                res = S7_box[_i, _j];
                break;
                case 8:
                res = S8_box[_i, _j];
                break;
                }
            return res;
        }
        private string XOR(string s1, string s2)
        {
            string result = "";
            //MessageBox.Show(s1.Length.ToString()+ "||" + s2.Length.ToString());
            for (int i = 0; i < s1.Length; i++)
            {
                bool a = Convert.ToBoolean(Convert.ToInt32(s1[i].ToString()));
                bool b = Convert.ToBoolean(Convert.ToInt32(s2[i].ToString()));

                if (a ^ b)
                    result += "1";
                else
                    result += "0";
            }
            //MessageBox.Show(result.Length.ToString());
            return result;
        }
        private string f(string s1, string s2)
        {
            return XOR(s1, s2);
        }
        private void KeysGenerator(string input)
        {
            newkeys = new string[16];
            for (int q = 0; q < quantityOfRounds; q++)
            {
                string temp = "";
                for (int i = 0; i < G1_table.Length; i++)
                {
                    temp += input.Substring(G1_table[i] - 1, 1);
                }
                string left = temp.Substring(0, 28);
                string right = temp.Substring(28, 28);
                for (int i = 0; i < shiftKey; i++)
                {
                    left = left[left.Length - 1] + left;
                    left = left.Remove(left.Length - 1);
                    right = right[right.Length - 1] + right;
                    right = right.Remove(right.Length - 1);
                }
                temp = left + right;
                for (int i = 0; i < G2_table.Length; i++)
                {
                    newkeys[q] += temp.Substring(G2_table[i] - 1, 1);
                }
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox2.Text;
            textBox2.Text = "";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if(textBox4.TextLength %8 != 0) { MessageBox.Show("Указанная длина ключа не кратна 8-и!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); goto error; }
                sizeOfChar = 8;
            }
            else 
            {
                if (textBox4.TextLength % 4 != 0) { MessageBox.Show("Указанная длина ключа не кратна 4-и!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); goto error; }
                sizeOfChar = 16; 
            }
            string mainstr = textBox1.Text;
            string key = textBox4.Text;
            quantityOfRounds = Convert.ToInt32(textBox3.Text);
            mainstr = StringToRightLength(mainstr, false);
            CutStringIntoBlocks(mainstr);
            key = StringToBinaryFormat(key);
            //for (int i=0; i<quantityOfRounds - 1; i++) key = KeyToNextRound(key);
            //MessageBox.Show("Ключ 1-й для дешифровки: " + key);
            KeysGenerator(key);
            for (int j = 0; j < quantityOfRounds; j++)
            {
                for (int i = 0; i < Blocks.Length; i++) Blocks[i] = DecodeDES_One_Round(Blocks[i], newkeys[quantityOfRounds - 1 - j]);
            }
            string result = "";
            for (int i = 0; i < Blocks.Length; i++)
                result += Blocks[i];
            textBox2.Text = StringFromBinaryToNormalFormat(result);
        error:;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if (textBox4.TextLength % 8 != 0) { MessageBox.Show("Указанная длина ключа не кратна 8-и!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); goto error; }
                sizeOfChar = 8;
            }
            else
            {
                if (textBox4.TextLength % 4 != 0) { MessageBox.Show("Указанная длина ключа не кратна 4-и!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); goto error; }
                sizeOfChar = 16;
            }
            string mainstr = textBox1.Text; // Считываем строку
            string key = textBox4.Text; // Считываем ключ
            quantityOfRounds = Convert.ToInt32(textBox3.Text);
            mainstr = StringToRightLength(mainstr, true); // Доводим длину введённой строки до кратности 8
            CutStringIntoBlocks(mainstr); // Нарезаем на блоки двоичных кодов
            //MessageBox.Show(mainstr);
            //byte[] tempbitstr = Encoding.ASCII.GetBytes(mainstr);
            //MessageBox.Show(tempbitstr[1].ToString());
            //key = CorrectKeyWord(key, mainstr.Length / (2 * Blocks.Length));
            key = StringToBinaryFormat(key);
            KeysGenerator(key);
            //MessageBox.Show("Ключ 1-й для шифровки: " + key);
            for (int j = 0; j < quantityOfRounds; j++)
            {
                for (int i = 0; i < Blocks.Length; i++) Blocks[i] = EncodeDES_One_Round(Blocks[i], newkeys[j]);
            }
            string result = "";
            for (int i = 0; i < Blocks.Length; i++)
                result += Blocks[i];
            textBox2.Text = StringFromBinaryToNormalFormat(result);

        error:;

        }
        private string DecodeDES_One_Round(string input, string key)
        {
            string L = input.Substring(0, input.Length / 2);
            string R = input.Substring(input.Length / 2, input.Length / 2);
            //MessageBox.Show(L.Length.ToString() + "|||||" + R.Length.ToString());
            //return (XOR(Sbox(f(Pbox_Ext(L), key)), R) + L);
            //MessageBox.Show(key);
            return (XOR(Pbox(Sbox(f(Pbox_Ext(L), key))), R) + L);
        }
        private string Pbox(string input)
        {
            string result ="";
            for(int i=0; i < input.Length; i++)
            {
                result += input.Substring(P_box[i] - 1, 1);
            }
            return result;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Realization of DES encryption algorithm by IvanovA (2021)\n\n- How to encrypt? \nFirst, you need to enter the text you want to encrypt in the top line. " +
                "After that, select the necessary number of rounds, as well as the password for encryption, and press the 'Encrypt' button. You will get the encrypted text in the bottom line.\nP.S. You can also choose encoding type (Unicode or ASCII)." +
                "\n\n- How to Decrypt?" +
                "\nFirst, you need to enter the text you want to decrypt in the top line. You can also use the button 'Move to the encryption line' if the bottom line contains the encrypted text that you want to decrypt. " +
                "After that, select the necessary number of rounds, as well as the password for decryption, and press the 'Decrypt' button. You will get the decrypted text in the bottom line.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }


}
