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
                string body = string.Format(@"
<p>
    DCR: {0} <br />
    Server: {1} <br />
    Db: {2} <br />
    User: {3}
</p>
<p>
    Notes: <br />
    {4}
</p>
<p>
    Error: <br />
    {5}
</p>
", txtDcr.Text, txtServer.Text, txtDb.Text, txtUser.Text, txtNotes.Text.Replace("\n", "<br />"), txtError.Text.Replace("\n", "<br />"));

                string[] tos = new string[] { "to@email.com" };
                string subject = string.Format("DCR Error - {0}", txtDcr.Text);

                service.SendDbMail(tos, "", subject, body);
            }
        }

        private void Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
