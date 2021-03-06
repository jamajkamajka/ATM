﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Resources;
using System.Reflection;
using System.Globalization;

namespace ATM_Final
{
    public partial class WithdrawForm : Form
    {
        GlobalDBConnection gdc = new GlobalDBConnection();
        Show show = new Show();

        public WithdrawForm()
        {
            InitializeComponent();
        }

        private void getRes(CultureInfo ci)
        {
            Assembly a = Assembly.Load("ATM_Final");
            ResourceManager rm = new ResourceManager("ATM_Final.Lang.langres", a);

            lbl_Please.Text = rm.GetString("please", ci);
            btn_Back.Text = rm.GetString("back", ci);
            btn_Confirm.Text = rm.GetString("confirm", ci);
            btn_Exit.Text = rm.GetString("exit", ci);
            en_flag.Image = (Image)rm.GetObject("flag", ci);
        }

        private void WithdrawForm_Load(object sender, EventArgs e)
        {

        }

        private void btn_Confirm_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(gdc.Con);
            SqlCommand cmd = new SqlCommand("Command String", con);
            SqlDataReader readdata;

            try
            {
                con.Open();
                double balance = double.Parse(txt_Withdraw.Text);
                double balanceDB = 0;
                double max = 1000.00;

                cmd.CommandText = "SELECT * FROM [Table] WHERE AccNo = '" + show.AccNo + "'";
                readdata = cmd.ExecuteReader();
                while (readdata.Read())
                {
                    balanceDB = double.Parse(readdata["Balance"].ToString());
                    
                }
                con.Close();

                con = new SqlConnection(gdc.Con);
                SqlCommand cmda = new SqlCommand("Command String", con);
                con.Open();

                if (balance < max)
                {

                    if (balanceDB >= balance)
                    {
                        balanceDB -= balance;
                        cmda.CommandText = "UPDATE [Table] SET Balance = '" + balanceDB + "' WHERE AccNo = '" + show.AccNo + "'";
                        cmda.ExecuteNonQuery();
                        con.Close();
                        con.Dispose();
                        MessageBox.Show("Withdrawal success!");

                        WelcomeForm previousForm = new WelcomeForm();
                        previousForm.Show();
                        this.Hide();
                    }
                    else if (balanceDB < balance)
                    {
                        MessageBox.Show("Insufficient fund");
                        txt_Withdraw.Clear();
                        con.Close();
                        con.Dispose();
                    }
                }
                    else
                    {
                        MessageBox.Show("You have exaggerated your limit!" + "\n" + "Please try again!");
                        txt_Withdraw.Clear();
                        con.Close();
                        con.Dispose();
                     }
                
            }

            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Back_Click(object sender, EventArgs e)
        {
            WelcomeForm previousForm = new WelcomeForm();
            previousForm.Show();
            this.Hide();
        }

        private void btn_Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btn_enflag_Click(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("en-US");
            getRes(ci);
        }

        private void btn_bsflag_Click(object sender, EventArgs e)
        {
            CultureInfo ci = new CultureInfo("hr-HR");
            getRes(ci);
        }
    }
}
