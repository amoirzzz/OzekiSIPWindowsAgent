﻿using onvif.services;
using sipservice;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using static sipservice.SIPService;

namespace SIPWindowsAgent
{
    public partial class IncomingCallForm : Form
    {
        public event EventHandler AcceptButtonClicked;
        public event EventHandler RejectButtonClicked;
        private SIPService sipService;

        public IncomingCallForm(string CallerID, List<CallerData> callerInfo, SIPService sipService, string userToken)
        {

            InitializeComponent();
            this.sipService = sipService;
            var config = SettingsManager.Instance.LoadSettings();
            timer1.Interval = config.CloseFormInterval * 1000;
            timer1.Enabled = true;
            // Set the caller information in the form
            try
            {
                if (callerInfo != null)
                {
                    var height = this.Height - ctlCallInfoList.Height;
                    ShowData(callerInfo, CallerID, true, userToken, async (s, s1, s2, s3) => await sipService.CallRedirectAPI(s, s1, s2, s3, true));
                    this.Height = height + ctlCallInfoList.Height;

                    //txtLog.AppendText($"Incoming Call from: {callerInfo.Items[0].Label}" + Environment.NewLine);
                    //TitleBarcaCaller.Text = callerInfo.Items[0].Label != "Unknown" ? callerInfo.Items[0].Label : CallerID;
                    //BarcaCallerFormID.Text = callerInfo.Items[0].Id;
                    //foreach (CallerDataItem key in callerInfo.Items)
                    //{
                    //    DescriptionBarcaCaller.Text += key.Text + Environment.NewLine;
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void ShowData(List<CallerData> data, string callerNumber, bool isInput, string userToken, OpenMethodDelegate openMethod)
        {
            ctlCallInfoList.ShowData(data, callerNumber, isInput, userToken, openMethod, this.sipService);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }
        public void UpdateLabelText(string newText)
        {
            //ctlCallInfoList.AddLog(newText);
        }



        private void IncomingCallForm_Load(object sender, EventArgs e)
        {
            int screenWorkingAreaWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenWorkingAreaHeight = Screen.PrimaryScreen.WorkingArea.Height;

            this.Left = screenWorkingAreaWidth - this.Width;
            this.Top = screenWorkingAreaHeight - this.Height;
        }


        private void button2_Click_1(object sender, EventArgs e)
        {
            sipService.AnswerCall();
            btnAnswer.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sipService?.RejectCall();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            this.Close();

        }
    }
}
