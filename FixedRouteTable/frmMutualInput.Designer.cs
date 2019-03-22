namespace FixedRouteTable
{
    partial class frmMutualInput
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtTopoSize = new System.Windows.Forms.TextBox();
            this.btnBegin = new System.Windows.Forms.Button();
            this.grbDetails = new System.Windows.Forms.GroupBox();
            this.lstTopoNodes = new System.Windows.Forms.ListBox();
            this.grbRelativeInput = new System.Windows.Forms.GroupBox();
            this.cbbLeftNodes = new System.Windows.Forms.ComboBox();
            this.cbbRightNodes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCost_left = new System.Windows.Forms.TextBox();
            this.btnAddRelative = new System.Windows.Forms.Button();
            this.grbFeatures = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCost_right = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.grbDetails.SuspendLayout();
            this.grbRelativeInput.SuspendLayout();
            this.grbFeatures.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grbFeatures);
            this.groupBox1.Controls.Add(this.grbRelativeInput);
            this.groupBox1.Controls.Add(this.btnBegin);
            this.groupBox1.Controls.Add(this.txtTopoSize);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 425);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dữ liệu cơ bản";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Số Node:";
            // 
            // txtTopoSize
            // 
            this.txtTopoSize.Location = new System.Drawing.Point(83, 31);
            this.txtTopoSize.Name = "txtTopoSize";
            this.txtTopoSize.Size = new System.Drawing.Size(141, 20);
            this.txtTopoSize.TabIndex = 1;
            // 
            // btnBegin
            // 
            this.btnBegin.Location = new System.Drawing.Point(230, 29);
            this.btnBegin.Name = "btnBegin";
            this.btnBegin.Size = new System.Drawing.Size(111, 23);
            this.btnBegin.TabIndex = 2;
            this.btnBegin.Text = "Bắt đầu";
            this.btnBegin.UseVisualStyleBackColor = true;
            this.btnBegin.Click += new System.EventHandler(this.btnBegin_Click);
            // 
            // grbDetails
            // 
            this.grbDetails.Controls.Add(this.lstTopoNodes);
            this.grbDetails.Location = new System.Drawing.Point(366, 13);
            this.grbDetails.Name = "grbDetails";
            this.grbDetails.Size = new System.Drawing.Size(422, 425);
            this.grbDetails.TabIndex = 1;
            this.grbDetails.TabStop = false;
            this.grbDetails.Text = "Dữ liệu mô hình";
            // 
            // lstTopoNodes
            // 
            this.lstTopoNodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstTopoNodes.FormattingEnabled = true;
            this.lstTopoNodes.Location = new System.Drawing.Point(3, 16);
            this.lstTopoNodes.Name = "lstTopoNodes";
            this.lstTopoNodes.Size = new System.Drawing.Size(416, 406);
            this.lstTopoNodes.TabIndex = 0;
            // 
            // grbRelativeInput
            // 
            this.grbRelativeInput.Controls.Add(this.label7);
            this.grbRelativeInput.Controls.Add(this.label6);
            this.grbRelativeInput.Controls.Add(this.btnAddRelative);
            this.grbRelativeInput.Controls.Add(this.txtCost_right);
            this.grbRelativeInput.Controls.Add(this.txtCost_left);
            this.grbRelativeInput.Controls.Add(this.label8);
            this.grbRelativeInput.Controls.Add(this.label4);
            this.grbRelativeInput.Controls.Add(this.label3);
            this.grbRelativeInput.Controls.Add(this.label2);
            this.grbRelativeInput.Controls.Add(this.cbbRightNodes);
            this.grbRelativeInput.Controls.Add(this.cbbLeftNodes);
            this.grbRelativeInput.Controls.Add(this.label5);
            this.grbRelativeInput.Enabled = false;
            this.grbRelativeInput.Location = new System.Drawing.Point(7, 70);
            this.grbRelativeInput.Name = "grbRelativeInput";
            this.grbRelativeInput.Size = new System.Drawing.Size(334, 220);
            this.grbRelativeInput.TabIndex = 3;
            this.grbRelativeInput.TabStop = false;
            this.grbRelativeInput.Text = "Nhập quan hệ giữa các NODE";
            // 
            // cbbLeftNodes
            // 
            this.cbbLeftNodes.FormattingEnabled = true;
            this.cbbLeftNodes.Location = new System.Drawing.Point(7, 112);
            this.cbbLeftNodes.Name = "cbbLeftNodes";
            this.cbbLeftNodes.Size = new System.Drawing.Size(121, 21);
            this.cbbLeftNodes.TabIndex = 0;
            this.cbbLeftNodes.SelectedIndexChanged += new System.EventHandler(this.cbbCurrentNodes_SelectedIndexChanged);
            // 
            // cbbRightNodes
            // 
            this.cbbRightNodes.FormattingEnabled = true;
            this.cbbRightNodes.Location = new System.Drawing.Point(182, 112);
            this.cbbRightNodes.Name = "cbbRightNodes";
            this.cbbRightNodes.Size = new System.Drawing.Size(121, 21);
            this.cbbRightNodes.TabIndex = 0;
            this.cbbRightNodes.SelectedIndexChanged += new System.EventHandler(this.cbbDirectedNodes_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "NODE:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(259, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "NODE:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(43, 138);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 21);
            this.label4.TabIndex = 2;
            this.label4.Text = "---->";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(14, 161);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(286, 19);
            this.label5.TabIndex = 3;
            this.label5.Text = "COST";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtCost_left
            // 
            this.txtCost_left.Location = new System.Drawing.Point(7, 160);
            this.txtCost_left.Name = "txtCost_left";
            this.txtCost_left.Size = new System.Drawing.Size(121, 20);
            this.txtCost_left.TabIndex = 4;
            this.txtCost_left.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btnAddRelative
            // 
            this.btnAddRelative.Location = new System.Drawing.Point(87, 187);
            this.btnAddRelative.Name = "btnAddRelative";
            this.btnAddRelative.Size = new System.Drawing.Size(130, 23);
            this.btnAddRelative.TabIndex = 5;
            this.btnAddRelative.Text = "Thêm quan hệ";
            this.btnAddRelative.UseVisualStyleBackColor = true;
            this.btnAddRelative.Click += new System.EventHandler(this.btnAddRelative_Click);
            // 
            // grbFeatures
            // 
            this.grbFeatures.Controls.Add(this.button1);
            this.grbFeatures.Controls.Add(this.button3);
            this.grbFeatures.Location = new System.Drawing.Point(7, 296);
            this.grbFeatures.Name = "grbFeatures";
            this.grbFeatures.Size = new System.Drawing.Size(334, 113);
            this.grbFeatures.TabIndex = 4;
            this.grbFeatures.TabStop = false;
            this.grbFeatures.Text = "Chức năng";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(223, 19);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(105, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Kết thúc";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label6
            // 
            this.label6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(7, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(321, 19);
            this.label6.TabIndex = 7;
            this.label6.Text = "* Các NODE không kết nối với nhau Cost = -1 ( không cần nhập )";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label7.Location = new System.Drawing.Point(7, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(321, 38);
            this.label7.TabIndex = 7;
            this.label7.Text = "* Cost = 0 thể hiện cho cùng một NODE ( dữ liệu này tự sinh có thể bỏ qua )";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtCost_right
            // 
            this.txtCost_right.Location = new System.Drawing.Point(182, 160);
            this.txtCost_right.Name = "txtCost_right";
            this.txtCost_right.Size = new System.Drawing.Size(121, 20);
            this.txtCost_right.TabIndex = 4;
            this.txtCost_right.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(220, 136);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 21);
            this.label8.TabIndex = 2;
            this.label8.Text = "<----";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(223, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "Huỷ bỏ tác vụ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmMutualInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.grbDetails);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMutualInput";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nhập dữ liệu cho mô hình";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMutualInput_FormClosing);
            this.Load += new System.EventHandler(this.frmMutualInput_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grbDetails.ResumeLayout(false);
            this.grbRelativeInput.ResumeLayout(false);
            this.grbRelativeInput.PerformLayout();
            this.grbFeatures.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBegin;
        private System.Windows.Forms.TextBox txtTopoSize;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grbDetails;
        private System.Windows.Forms.ListBox lstTopoNodes;
        private System.Windows.Forms.GroupBox grbRelativeInput;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbbRightNodes;
        private System.Windows.Forms.ComboBox cbbLeftNodes;
        private System.Windows.Forms.Button btnAddRelative;
        private System.Windows.Forms.TextBox txtCost_left;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox grbFeatures;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCost_right;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button button1;
    }
}