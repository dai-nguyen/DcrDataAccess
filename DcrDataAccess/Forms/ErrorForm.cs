/** 
 * This file is part of the DcrDataAccess project.
 * Copyright (c) 2014 Dai Nguyen
 * Author: Dai Nguyen
**/

using DcrDataAccess.Models;
using System;
using System.Windows.Forms;

namespace DcrDataAccess.Forms
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(string dcr, SessionInfo info, string error)
        {
            InitializeComponent();

            txtDcr.Text = dcr;            
            txtServer.Text = info.Server;
            txtDb.Text = info.Db;
            txtUser.Text = info.User;
            txtError.Text = error;
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            using (DataService service = new DataService(txtServer.Text, txtDb.Text))
            {
                service.SendErrorEmail(txtDcr.Text,
                    new SessionInfo(txtServer.Text, txtDb.Text, txtUser.Text),
                    txtError.Text, txtNotes.Text);
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
