namespace FixedRouteTable
{
    partial class RouterControlUI
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
            this.lblRID = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblRID
            // 
            this.lblRID.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblRID.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRID.Location = new System.Drawing.Point(3, 9);
            this.lblRID.Name = "lblRID";
            this.lblRID.Size = new System.Drawing.Size(34, 23);
            this.lblRID.TabIndex = 0;
            this.lblRID.Text = "0";
            this.lblRID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblRID.Click += new System.EventHandler(this.lblRID_Click);
            // 
            // RouterControlUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Controls.Add(this.lblRID);
            this.Name = "RouterControlUI";
            this.Size = new System.Drawing.Size(40, 40);
            this.MouseHover += new System.EventHandler(this.RouterControlUI_MouseHover);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblRID;
    }
}
