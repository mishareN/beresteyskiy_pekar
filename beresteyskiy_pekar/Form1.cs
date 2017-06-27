using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data.Types;
using MySql.Data;
using System.Security.Cryptography;

namespace beresteyskiy_pekar
{
    public partial class Form1 : Form
    {
        MySqlConnection con = new MySqlConnection(@"Data Source=localhost;port=3306;Initial Catalog=productpekar; User Id=root;password=root");
        private String ps, hps;
        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (ReferenceEquals(a, b))
            {
                return true;
            }

            if (a == null || b == null || a.Length != b.Length)
            {
                return false;
            }

            var areSame = true;
            for (var i = 0; i < a.Length; i++)
            {
                areSame &= (a[i] == b[i]);
            }
            return areSame;
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            hps = string.Empty;
            con.Open();
            MySqlCommand cmd = con.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT password FROM user WHERE username = @username";
            cmd.Parameters.AddWithValue("@username", textBox1.Text);
            
            MySqlDataReader reader;

            reader = cmd.ExecuteReader();
            reader.Read();

            if (!reader.HasRows)
            {
                label4.Text = "Пользователя с данным логином не существует!";
            }
            else 
            {
                hps = reader.GetString("password");
                ps = textBox2.ToString();
                if (VerifyHashedPassword(hps, ps))
                {
                    this.Hide();
                    Form2 fm2 = new Form2();
                    fm2.Show();
                }
                else label4.Text = "Пароли не совпадают!";
            }

            con.Close();
        }
    }
}
