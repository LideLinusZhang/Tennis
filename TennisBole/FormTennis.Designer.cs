
namespace TennisBole
{
    partial class FormTennis
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listViewPlayers = new System.Windows.Forms.ListView();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderAge = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderGender = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNationality = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderUTR = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderATP = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderMyRank = new System.Windows.Forms.ColumnHeader();
            this.buttonAgeLimits = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewPlayers
            // 
            this.listViewPlayers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderAge,
            this.columnHeaderGender,
            this.columnHeaderNationality,
            this.columnHeaderUTR,
            this.columnHeaderATP,
            this.columnHeaderMyRank});
            this.listViewPlayers.HideSelection = false;
            this.listViewPlayers.Location = new System.Drawing.Point(212, 12);
            this.listViewPlayers.Name = "listViewPlayers";
            this.listViewPlayers.Size = new System.Drawing.Size(617, 399);
            this.listViewPlayers.TabIndex = 0;
            this.listViewPlayers.UseCompatibleStateImageBehavior = false;
            this.listViewPlayers.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Name = "columnHeaderName";
            this.columnHeaderName.Text = "Name";
            this.columnHeaderName.Width = 150;
            // 
            // columnHeaderAge
            // 
            this.columnHeaderAge.Name = "columnHeaderAge";
            this.columnHeaderAge.Text = "Age";
            // 
            // columnHeaderGender
            // 
            this.columnHeaderGender.Name = "columnHeaderGender";
            this.columnHeaderGender.Text = "Gender";
            // 
            // columnHeaderNationality
            // 
            this.columnHeaderNationality.Name = "columnHeaderNationality";
            this.columnHeaderNationality.Text = "Nationality";
            this.columnHeaderNationality.Width = 150;
            // 
            // columnHeaderUTR
            // 
            this.columnHeaderUTR.Name = "columnHeaderUTR";
            this.columnHeaderUTR.Text = "UTR";
            // 
            // columnHeaderATP
            // 
            this.columnHeaderATP.Name = "columnHeaderATP";
            this.columnHeaderATP.Text = "ATP";
            // 
            // columnHeaderMyRank
            // 
            this.columnHeaderMyRank.Name = "columnHeaderMyRank";
            this.columnHeaderMyRank.Text = "MyRank";
            // 
            // buttonAgeLimits
            // 
            this.buttonAgeLimits.Location = new System.Drawing.Point(12, 12);
            this.buttonAgeLimits.Name = "buttonAgeLimits";
            this.buttonAgeLimits.Size = new System.Drawing.Size(194, 36);
            this.buttonAgeLimits.TabIndex = 1;
            this.buttonAgeLimits.Text = "Age Limits Setting";
            this.buttonAgeLimits.UseVisualStyleBackColor = true;
            this.buttonAgeLimits.Click += new System.EventHandler(this.buttonAgeLimits_Click);
            // 
            // FormTennis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(841, 423);
            this.Controls.Add(this.buttonAgeLimits);
            this.Controls.Add(this.listViewPlayers);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormTennis";
            this.Text = "Tennis Bole";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewPlayers;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.ColumnHeader columnHeaderAge;
        private System.Windows.Forms.ColumnHeader columnHeaderGender;
        private System.Windows.Forms.ColumnHeader columnHeaderNationality;
        private System.Windows.Forms.ColumnHeader columnHeaderUTR;
        private System.Windows.Forms.ColumnHeader columnHeaderATP;
        private System.Windows.Forms.ColumnHeader columnHeaderMyRank;
        private System.Windows.Forms.Button buttonAgeLimits;
    }
}

