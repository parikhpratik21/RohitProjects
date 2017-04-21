using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace productionmanagment
{
    public partial class PasswordBox : Form
    {
        public bool Result = false;
        string strPassword = "NAKALANK@HMSI";
        public PasswordBox()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Result = false;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if(txtPassWord.Text == strPassword)
            {
                Result = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Please enter correct password");
            }
        }

        private void PasswordBox_Load(object sender, EventArgs e)
        {
            txtPassWord.Focus();
        }
    }
}
