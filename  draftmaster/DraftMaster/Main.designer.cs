namespace MTG_Drafter
{
    partial class Main
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
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("Picked", System.Windows.Forms.HorizontalAlignment.Left);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.panelPackInfo = new System.Windows.Forms.Panel();
            this.buttonAddSet = new System.Windows.Forms.Button();
            this.comboSets = new System.Windows.Forms.ComboBox();
            this.upDownBoosters = new System.Windows.Forms.NumericUpDown();
            this.buttonNewGame = new System.Windows.Forms.Button();
            this.panelPlayerInfo = new System.Windows.Forms.Panel();
            this.listViewPack = new System.Windows.Forms.ListView();
            this.columnPackDisp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewPicked = new System.Windows.Forms.ListView();
            this.drafted = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cardImage = new System.Windows.Forms.PictureBox();
            this.buttonPick = new System.Windows.Forms.Button();
            this.labelTips = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.panelPackInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPackInfo
            // 
            this.panelPackInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPackInfo.BackColor = System.Drawing.Color.White;
            this.panelPackInfo.Controls.Add(this.buttonAddSet);
            this.panelPackInfo.Controls.Add(this.comboSets);
            this.panelPackInfo.Controls.Add(this.upDownBoosters);
            this.panelPackInfo.Controls.Add(this.buttonNewGame);
            this.panelPackInfo.Location = new System.Drawing.Point(12, 12);
            this.panelPackInfo.Name = "panelPackInfo";
            this.panelPackInfo.Size = new System.Drawing.Size(158, 638);
            this.panelPackInfo.TabIndex = 0;
            this.panelPackInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPackInfo_Paint);
            // 
            // buttonAddSet
            // 
            this.buttonAddSet.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAddSet.Enabled = false;
            this.buttonAddSet.Location = new System.Drawing.Point(109, 57);
            this.buttonAddSet.Name = "buttonAddSet";
            this.buttonAddSet.Size = new System.Drawing.Size(48, 20);
            this.buttonAddSet.TabIndex = 3;
            this.buttonAddSet.Text = "Add";
            this.buttonAddSet.UseVisualStyleBackColor = true;
            this.buttonAddSet.Click += new System.EventHandler(this.buttonAddSet_Click);
            // 
            // comboSets
            // 
            this.comboSets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.comboSets.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSets.Enabled = false;
            this.comboSets.FormattingEnabled = true;
            this.comboSets.Location = new System.Drawing.Point(3, 57);
            this.comboSets.Name = "comboSets";
            this.comboSets.Size = new System.Drawing.Size(103, 21);
            this.comboSets.TabIndex = 2;
            // 
            // upDownBoosters
            // 
            this.upDownBoosters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.upDownBoosters.Location = new System.Drawing.Point(112, 33);
            this.upDownBoosters.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.upDownBoosters.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.upDownBoosters.Name = "upDownBoosters";
            this.upDownBoosters.Size = new System.Drawing.Size(46, 20);
            this.upDownBoosters.TabIndex = 1;
            this.upDownBoosters.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // buttonNewGame
            // 
            this.buttonNewGame.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNewGame.Location = new System.Drawing.Point(38, 29);
            this.buttonNewGame.Name = "buttonNewGame";
            this.buttonNewGame.Size = new System.Drawing.Size(68, 26);
            this.buttonNewGame.TabIndex = 0;
            this.buttonNewGame.Text = "New Draft";
            this.buttonNewGame.UseVisualStyleBackColor = true;
            this.buttonNewGame.Click += new System.EventHandler(this.buttonNewGame_Click);
            // 
            // panelPlayerInfo
            // 
            this.panelPlayerInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelPlayerInfo.BackColor = System.Drawing.Color.White;
            this.panelPlayerInfo.Location = new System.Drawing.Point(176, 565);
            this.panelPlayerInfo.Name = "panelPlayerInfo";
            this.panelPlayerInfo.Size = new System.Drawing.Size(596, 86);
            this.panelPlayerInfo.TabIndex = 1;
            this.panelPlayerInfo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelPlayerInfo_Paint);
            // 
            // listViewPack
            // 
            this.listViewPack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewPack.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnPackDisp,
            this.columnHeader1});
            this.listViewPack.Location = new System.Drawing.Point(176, 12);
            this.listViewPack.MultiSelect = false;
            this.listViewPack.Name = "listViewPack";
            this.listViewPack.Size = new System.Drawing.Size(325, 338);
            this.listViewPack.TabIndex = 2;
            this.listViewPack.UseCompatibleStateImageBehavior = false;
            this.listViewPack.View = System.Windows.Forms.View.Details;
            this.listViewPack.SelectedIndexChanged += new System.EventHandler(this.listViewPack_SelectedIndexChanged);
            // 
            // columnPackDisp
            // 
            this.columnPackDisp.Text = "Name";
            this.columnPackDisp.Width = 150;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Rarity";
            this.columnHeader1.Width = 150;
            // 
            // listViewPicked
            // 
            this.listViewPicked.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewPicked.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.drafted,
            this.columnHeader2});
            listViewGroup1.Header = "Picked";
            listViewGroup1.Name = "listViewGroup1";
            this.listViewPicked.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1});
            this.listViewPicked.Location = new System.Drawing.Point(176, 356);
            this.listViewPicked.MultiSelect = false;
            this.listViewPicked.Name = "listViewPicked";
            this.listViewPicked.Size = new System.Drawing.Size(325, 203);
            this.listViewPicked.TabIndex = 3;
            this.listViewPicked.UseCompatibleStateImageBehavior = false;
            this.listViewPicked.View = System.Windows.Forms.View.Details;
            this.listViewPicked.SelectedIndexChanged += new System.EventHandler(this.listViewPicked_SelectedIndexChanged);
            // 
            // drafted
            // 
            this.drafted.Text = "Name";
            this.drafted.Width = 150;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Rarity";
            this.columnHeader2.Width = 150;
            // 
            // cardImage
            // 
            this.cardImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cardImage.BackColor = System.Drawing.Color.Black;
            this.cardImage.Location = new System.Drawing.Point(507, 12);
            this.cardImage.Name = "cardImage";
            this.cardImage.Size = new System.Drawing.Size(265, 370);
            this.cardImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.cardImage.TabIndex = 4;
            this.cardImage.TabStop = false;
            // 
            // buttonPick
            // 
            this.buttonPick.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonPick.Location = new System.Drawing.Point(401, 303);
            this.buttonPick.Name = "buttonPick";
            this.buttonPick.Size = new System.Drawing.Size(100, 47);
            this.buttonPick.TabIndex = 6;
            this.buttonPick.Text = "Pick";
            this.buttonPick.UseVisualStyleBackColor = true;
            this.buttonPick.Visible = false;
            this.buttonPick.Click += new System.EventHandler(this.buttonPick_Click);
            // 
            // labelTips
            // 
            this.labelTips.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labelTips.BackColor = System.Drawing.Color.White;
            this.labelTips.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTips.Location = new System.Drawing.Point(507, 385);
            this.labelTips.Name = "labelTips";
            this.labelTips.Size = new System.Drawing.Size(265, 174);
            this.labelTips.TabIndex = 7;
            this.labelTips.Text = "TIPS";
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.Location = new System.Drawing.Point(401, 303);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(100, 47);
            this.buttonSave.TabIndex = 8;
            this.buttonSave.Text = "Save Picks";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Visible = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(784, 662);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.labelTips);
            this.Controls.Add(this.buttonPick);
            this.Controls.Add(this.cardImage);
            this.Controls.Add(this.listViewPicked);
            this.Controls.Add(this.listViewPack);
            this.Controls.Add(this.panelPlayerInfo);
            this.Controls.Add(this.panelPackInfo);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Draftmaster";
            this.panelPackInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPackInfo;
        private System.Windows.Forms.Panel panelPlayerInfo;
        private System.Windows.Forms.ListView listViewPack;
        private System.Windows.Forms.ListView listViewPicked;
        private System.Windows.Forms.PictureBox cardImage;
        private System.Windows.Forms.ColumnHeader columnPackDisp;
        private System.Windows.Forms.ColumnHeader drafted;
        private System.Windows.Forms.Button buttonPick;
        private System.Windows.Forms.Label labelTips;
        private System.Windows.Forms.NumericUpDown upDownBoosters;
        private System.Windows.Forms.Button buttonNewGame;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ComboBox comboSets;
        private System.Windows.Forms.Button buttonAddSet;
    }
}

