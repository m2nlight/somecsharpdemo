using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EncryptWithSaltTest
{
    public partial class Form1 : Form
    {
        string plainText = "Hello, World!";    // original plaintext
        string cipherText = "";                 // encrypted text
        string passPhrase = "Pas5pr@se";        // can be any string
        string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
        const string i_passPhrase="Pas5pr@se";
        const string i_initVector = "@1B2c3D4e5F6g7H8";

        public Form1()
        {
            InitializeComponent();
            textBox2.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                passPhrase = textBox3.Text;
                if (textBox4.Text.ToCharArray().Length == 16)
                {
                    initVector = textBox4.Text;
                }
                else
                {
                    textBox4.Text = initVector;
                }

                plainText = textBox1.Text;
                RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(passPhrase, initVector);
                cipherText = rijndaelKey.Encrypt(plainText);
                textBox2.Text += String.Format("[加密] {0}\r\n", cipherText);
            }
            catch (Exception ex)
            {
                textBox2.Text += String.Format("加密出错:{0}\r\n", ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                passPhrase = textBox3.Text;
                if (textBox4.Text.ToCharArray().Length == 16)
                {
                    initVector = textBox4.Text;
                }
                else
                {
                    textBox4.Text = initVector;
                }

                cipherText = textBox1.Text.Trim();
                RijndaelEnhanced rijndaelKey = new RijndaelEnhanced(passPhrase, initVector);
                plainText = rijndaelKey.Decrypt(cipherText);
                textBox2.Text += String.Format("[解密] {0}\r\n", plainText);
            }
            catch (Exception ex)
            {
                textBox2.Text+=String.Format("解密出错:{0}\r\n",ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = String.Empty;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            textBox2.Select(textBox2.TextLength, 0);
            textBox2.ScrollToCaret();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            textBox3.Text = i_passPhrase;
            textBox4.Text = i_initVector;
        }
    }
}