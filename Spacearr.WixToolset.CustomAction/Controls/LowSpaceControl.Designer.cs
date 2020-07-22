namespace Spacearr.WixToolset.CustomAction.Controls
{
    partial class LowSpaceControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLowSpaceNotificationInterval = new System.Windows.Forms.TextBox();
            this.lblLowSpaceNotificationInterval = new System.Windows.Forms.Label();
            this.txtLowSpaceGBValue = new System.Windows.Forms.TextBox();
            this.lblLowSpaceGBValue = new System.Windows.Forms.Label();
            this.lblSettingDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSendLowSpaceNotification = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.chkSendLowSpaceNotification);
            this.panel1.Controls.Add(this.txtLowSpaceNotificationInterval);
            this.panel1.Controls.Add(this.lblLowSpaceNotificationInterval);
            this.panel1.Controls.Add(this.txtLowSpaceGBValue);
            this.panel1.Controls.Add(this.lblLowSpaceGBValue);
            this.panel1.Controls.Add(this.lblSettingDescription);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(402, 312);
            this.panel1.TabIndex = 2;
            // 
            // txtLowSpaceNotificationInterval
            // 
            this.txtLowSpaceNotificationInterval.Location = new System.Drawing.Point(196, 175);
            this.txtLowSpaceNotificationInterval.Name = "txtLowSpaceNotificationInterval";
            this.txtLowSpaceNotificationInterval.Size = new System.Drawing.Size(195, 22);
            this.txtLowSpaceNotificationInterval.TabIndex = 5;
            // 
            // lblLowSpaceNotificationInterval
            // 
            this.lblLowSpaceNotificationInterval.AutoSize = true;
            this.lblLowSpaceNotificationInterval.Location = new System.Drawing.Point(16, 178);
            this.lblLowSpaceNotificationInterval.Name = "lblLowSpaceNotificationInterval";
            this.lblLowSpaceNotificationInterval.Size = new System.Drawing.Size(181, 14);
            this.lblLowSpaceNotificationInterval.TabIndex = 4;
            this.lblLowSpaceNotificationInterval.Text = "Low Space Notification Interval:";
            // 
            // txtLowSpaceGBValue
            // 
            this.txtLowSpaceGBValue.Location = new System.Drawing.Point(196, 147);
            this.txtLowSpaceGBValue.Name = "txtLowSpaceGBValue";
            this.txtLowSpaceGBValue.Size = new System.Drawing.Size(195, 22);
            this.txtLowSpaceGBValue.TabIndex = 3;
            // 
            // lblLowSpaceGBValue
            // 
            this.lblLowSpaceGBValue.AutoSize = true;
            this.lblLowSpaceGBValue.Location = new System.Drawing.Point(16, 150);
            this.lblLowSpaceGBValue.Name = "lblLowSpaceGBValue";
            this.lblLowSpaceGBValue.Size = new System.Drawing.Size(124, 14);
            this.lblLowSpaceGBValue.TabIndex = 2;
            this.lblLowSpaceGBValue.Text = "Low Space GB Value:";
            // 
            // lblSettingDescription
            // 
            this.lblSettingDescription.AutoSize = true;
            this.lblSettingDescription.Location = new System.Drawing.Point(13, 62);
            this.lblSettingDescription.Name = "lblSettingDescription";
            this.lblSettingDescription.Size = new System.Drawing.Size(266, 14);
            this.lblSettingDescription.TabIndex = 1;
            this.lblSettingDescription.Text = "Please enter the upate the settings as desired.";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(223, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thank you for using Win App.";
            // 
            // chkSendLowSpaceNotification
            // 
            this.chkSendLowSpaceNotification.AutoSize = true;
            this.chkSendLowSpaceNotification.Location = new System.Drawing.Point(19, 102);
            this.chkSendLowSpaceNotification.Name = "chkSendLowSpaceNotification";
            this.chkSendLowSpaceNotification.Size = new System.Drawing.Size(183, 18);
            this.chkSendLowSpaceNotification.TabIndex = 6;
            this.chkSendLowSpaceNotification.Text = "Send Low Space Notification";
            this.chkSendLowSpaceNotification.UseVisualStyleBackColor = true;
            this.chkSendLowSpaceNotification.CheckedChanged += new System.EventHandler(this.chkSendLowSpaceNotification_CheckedChanged);
            // 
            // LowSpaceControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "LowSpaceControl";
            this.Size = new System.Drawing.Size(403, 314);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtLowSpaceGBValue;
        private System.Windows.Forms.Label lblLowSpaceGBValue;
        private System.Windows.Forms.Label lblSettingDescription;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLowSpaceNotificationInterval;
        private System.Windows.Forms.Label lblLowSpaceNotificationInterval;
        private System.Windows.Forms.CheckBox chkSendLowSpaceNotification;
    }
}
