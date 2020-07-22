namespace Spacearr.WixToolset.CustomAction.Controls
{
    partial class UpdateAppControl
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
            this.txtUpdateAppInterval = new System.Windows.Forms.TextBox();
            this.lblUpdateAppInterval = new System.Windows.Forms.Label();
            this.lblSettingDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txtUpdateAppInterval);
            this.panel1.Controls.Add(this.lblUpdateAppInterval);
            this.panel1.Controls.Add(this.lblSettingDescription);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(349, 312);
            this.panel1.TabIndex = 2;
            // 
            // txtUpdateAppInterval
            // 
            this.txtUpdateAppInterval.Location = new System.Drawing.Point(144, 117);
            this.txtUpdateAppInterval.Name = "txtUpdateAppInterval";
            this.txtUpdateAppInterval.Size = new System.Drawing.Size(195, 22);
            this.txtUpdateAppInterval.TabIndex = 3;
            // 
            // lblUpdateAppInterval
            // 
            this.lblUpdateAppInterval.AutoSize = true;
            this.lblUpdateAppInterval.Location = new System.Drawing.Point(16, 120);
            this.lblUpdateAppInterval.Name = "lblUpdateAppInterval";
            this.lblUpdateAppInterval.Size = new System.Drawing.Size(122, 14);
            this.lblUpdateAppInterval.TabIndex = 2;
            this.lblUpdateAppInterval.Text = "Update App Interval:";
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
            // UpdateAppControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "UpdateAppControl";
            this.Size = new System.Drawing.Size(351, 314);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtUpdateAppInterval;
        private System.Windows.Forms.Label lblUpdateAppInterval;
        private System.Windows.Forms.Label lblSettingDescription;
        private System.Windows.Forms.Label label1;
    }
}
