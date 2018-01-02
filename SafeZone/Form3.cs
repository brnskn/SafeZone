using StartUpLibrary;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SafeZone
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            StartUp startUp = new StartUp("SafeZone");
            startUp.RegisterOrRemove(StartUp.Action.Register);
            InitializeComponent();
            if(UserDB.UserList.Count == 0)
            {
                Text = "Hesap Oluşturun";
                girisButton.Text = "Kayıt Ol";
            }
        }

        private void girisButton_Click(object sender, EventArgs e)
        {
            if (UserDB.UserList.Count == 0)
            {
                UserModel userModel = new UserModel(usernameTextBox.Text, passwordTextBox.Text);
                userModel.id = UserDB.Insert(userModel);
                UserDB.CurrentUser = userModel;
                Hide();
            }
            else
            {
                if(UserDB.Any(usernameTextBox.Text, passwordTextBox.Text))
                {
                    UserModel userModel = new UserModel(usernameTextBox.Text, passwordTextBox.Text);
                    userModel.id = UserDB.GetUser(usernameTextBox.Text).id;
                    UserDB.CurrentUser = userModel;
                    Hide();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adı veya şifre yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            usernameTextBox.Text = String.Empty;
            passwordTextBox.Text = String.Empty;
        }
        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            Hide();
        }
    }
}
