namespace Spacearr.WixToolset.CustomAction.Controls
{
    partial class LowComputerDriveGBValueControl
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
            this.txtLowComputerDriveGBValue = new System.Windows.Forms.TextBox();
            this.lblLowComputerDriveGBValue = new System.Windows.Forms.Label();
            this.lblSettingDescription = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.txtLowComputerDriveGBValue);
            this.panel1.Controls.Add(this.lblLowComputerDriveGBValue);
            this.panel1.Controls.Add(this.lblSettingDescription);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(402, 312);
            this.panel1.TabIndex = 2;
            // 
            // txtLowComputerDriveGBValue
            // 
            this.txtLowComputerDriveGBValue.Location = new System.Drawing.Point(196, 117);
            this.txtLowComputerDriveGBValue.Name = "txtLowComputerDriveGBValue";
            this.txtLowComputerDriveGBValue.Size = new System.Drawing.Size(195, 22);
            this.txtLowComputerDriveGBValue.TabIndex = 3;
            // 
            // lblLowComputerDriveGBValue
            // 
            this.lblLowComputerDriveGBValue.AutoSize = true;
            this.lblLowComputerDriveGBValue.Location = new System.Drawing.Point(16, 120);
            this.lblLowComputerDriveGBValue.Name = "lblLowComputerDriveGBValue";
            this.lblLowComputerDriveGBValue.Size = new System.Drawing.Size(176, 14);
            this.lblLowComputerDriveGBValue.TabIndex = 2;
            this.lblLowComputerDriveGBValue.Text = "Low Computer Drive GB Value:";
            // 
            // lblSettingDescription
            // 
            this.lblSettingDescription.AutoSize = true;
            this.lblSettingDescription.Location = new System.Drawing.Point(13, 62);
            this.lblSettingDescription.Name = "lblSettingDescription";
            this.lblSettingDescription.Size = new System.Drawing.Size(385, 28);
            this.lblSettingDescription.TabIndex = 1;
            this.lblSettingDescription.Text = "Please enter the value for LowComputerDriveGBValue in the\r\nAppSetting.json. This " +
    "value is used to send a low space notifications.";
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
            // LowComputerDriveGBValueControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Name = "LowComputerDriveGBValueControl";
            this.Size = new System.Drawing.Size(403, 314);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtLowComputerDriveGBValue;
        private System.Windows.Forms.Label lblLowComputerDriveGBValue;
        private System.Windows.Forms.Label lblSettingDescription;
        private System.Windows.Forms.Label label1;
    }
}
