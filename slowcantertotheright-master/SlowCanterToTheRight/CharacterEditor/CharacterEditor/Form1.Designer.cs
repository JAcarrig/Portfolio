namespace CharacterEditor
{
    partial class CharacterEditorForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CharacterEditorForm));
            this.labelPlayerHealth = new System.Windows.Forms.Label();
            this.labelPlayerXSpeed = new System.Windows.Forms.Label();
            this.labelPlayerDamage = new System.Windows.Forms.Label();
            this.pictureBoxPlayer = new System.Windows.Forms.PictureBox();
            this.pictureBoxEnemy = new System.Windows.Forms.PictureBox();
            this.labelNacho = new System.Windows.Forms.Label();
            this.labelEnemy = new System.Windows.Forms.Label();
            this.labelEnemyHealth = new System.Windows.Forms.Label();
            this.labelEnemyXSpeed = new System.Windows.Forms.Label();
            this.labelEnemyDamage = new System.Windows.Forms.Label();
            this.textBoxPlayerHealth = new System.Windows.Forms.TextBox();
            this.textBoxPlayerXSpeed = new System.Windows.Forms.TextBox();
            this.textBoxPlayerDamage = new System.Windows.Forms.TextBox();
            this.textBoxEnemyHealth = new System.Windows.Forms.TextBox();
            this.textBoxEnemyXSpeed = new System.Windows.Forms.TextBox();
            this.textBoxEnemyDamage = new System.Windows.Forms.TextBox();
            this.buttonConfirm = new System.Windows.Forms.Button();
            this.labelFlyingEnemyHealth = new System.Windows.Forms.Label();
            this.labelFlyingEnemyXSpeed = new System.Windows.Forms.Label();
            this.labelFlyingEnemyDamage = new System.Windows.Forms.Label();
            this.textBoxFlyingEnemyHealth = new System.Windows.Forms.TextBox();
            this.textBoxFlyingEnemyXSpeed = new System.Windows.Forms.TextBox();
            this.textBoxFlyingEnemyDamage = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelFlyingEnemy = new System.Windows.Forms.Label();
            this.groupBoxPlayer = new System.Windows.Forms.GroupBox();
            this.textBoxPlayerYSpeed = new System.Windows.Forms.TextBox();
            this.labelPlayerYSpeed = new System.Windows.Forms.Label();
            this.groupBoxEnemy = new System.Windows.Forms.GroupBox();
            this.textBoxEnemyYSpeed = new System.Windows.Forms.TextBox();
            this.labelEnemyYSpeed = new System.Windows.Forms.Label();
            this.groupBoxFlyingEnemy = new System.Windows.Forms.GroupBox();
            this.textBoxFlyingEnemyYSpeed = new System.Windows.Forms.TextBox();
            this.labelFlyingEnemyYSpeed = new System.Windows.Forms.Label();
            this.groupBoxBoss = new System.Windows.Forms.GroupBox();
            this.textBoxBossDamage = new System.Windows.Forms.TextBox();
            this.labelBossDamage = new System.Windows.Forms.Label();
            this.labelBossYSpeed = new System.Windows.Forms.Label();
            this.textBoxBossXSpeed = new System.Windows.Forms.TextBox();
            this.labelBossXSpeed = new System.Windows.Forms.Label();
            this.textBoxBossYSpeed = new System.Windows.Forms.TextBox();
            this.labelBossHealth = new System.Windows.Forms.Label();
            this.textBoxBossHealth = new System.Windows.Forms.TextBox();
            this.pictureBoxBoss = new System.Windows.Forms.PictureBox();
            this.labelBoss = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEnemy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBoxPlayer.SuspendLayout();
            this.groupBoxEnemy.SuspendLayout();
            this.groupBoxFlyingEnemy.SuspendLayout();
            this.groupBoxBoss.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoss)).BeginInit();
            this.SuspendLayout();
            // 
            // labelPlayerHealth
            // 
            this.labelPlayerHealth.AutoSize = true;
            this.labelPlayerHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerHealth.Location = new System.Drawing.Point(18, 46);
            this.labelPlayerHealth.Name = "labelPlayerHealth";
            this.labelPlayerHealth.Size = new System.Drawing.Size(38, 13);
            this.labelPlayerHealth.TabIndex = 1;
            this.labelPlayerHealth.Text = "Health";
            // 
            // labelPlayerXSpeed
            // 
            this.labelPlayerXSpeed.AutoSize = true;
            this.labelPlayerXSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerXSpeed.Location = new System.Drawing.Point(15, 78);
            this.labelPlayerXSpeed.Name = "labelPlayerXSpeed";
            this.labelPlayerXSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelPlayerXSpeed.TabIndex = 2;
            this.labelPlayerXSpeed.Text = "XSpeed";
            // 
            // labelPlayerDamage
            // 
            this.labelPlayerDamage.AutoSize = true;
            this.labelPlayerDamage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerDamage.Location = new System.Drawing.Point(14, 148);
            this.labelPlayerDamage.Name = "labelPlayerDamage";
            this.labelPlayerDamage.Size = new System.Drawing.Size(47, 13);
            this.labelPlayerDamage.TabIndex = 3;
            this.labelPlayerDamage.Text = "Damage";
            // 
            // pictureBoxPlayer
            // 
            this.pictureBoxPlayer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxPlayer.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxPlayer.Image")));
            this.pictureBoxPlayer.Location = new System.Drawing.Point(56, 51);
            this.pictureBoxPlayer.Name = "pictureBoxPlayer";
            this.pictureBoxPlayer.Size = new System.Drawing.Size(210, 268);
            this.pictureBoxPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxPlayer.TabIndex = 4;
            this.pictureBoxPlayer.TabStop = false;
            // 
            // pictureBoxEnemy
            // 
            this.pictureBoxEnemy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxEnemy.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxEnemy.Image")));
            this.pictureBoxEnemy.Location = new System.Drawing.Point(352, 52);
            this.pictureBoxEnemy.Name = "pictureBoxEnemy";
            this.pictureBoxEnemy.Size = new System.Drawing.Size(210, 267);
            this.pictureBoxEnemy.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxEnemy.TabIndex = 5;
            this.pictureBoxEnemy.TabStop = false;
            // 
            // labelNacho
            // 
            this.labelNacho.AutoSize = true;
            this.labelNacho.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNacho.Location = new System.Drawing.Point(126, 21);
            this.labelNacho.Name = "labelNacho";
            this.labelNacho.Size = new System.Drawing.Size(71, 24);
            this.labelNacho.TabIndex = 6;
            this.labelNacho.Text = "Nacho";
            // 
            // labelEnemy
            // 
            this.labelEnemy.AutoSize = true;
            this.labelEnemy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnemy.Location = new System.Drawing.Point(420, 21);
            this.labelEnemy.Name = "labelEnemy";
            this.labelEnemy.Size = new System.Drawing.Size(75, 24);
            this.labelEnemy.TabIndex = 7;
            this.labelEnemy.Text = "Enemy";
            // 
            // labelEnemyHealth
            // 
            this.labelEnemyHealth.AutoSize = true;
            this.labelEnemyHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnemyHealth.Location = new System.Drawing.Point(19, 43);
            this.labelEnemyHealth.Name = "labelEnemyHealth";
            this.labelEnemyHealth.Size = new System.Drawing.Size(38, 13);
            this.labelEnemyHealth.TabIndex = 9;
            this.labelEnemyHealth.Text = "Health";
            // 
            // labelEnemyXSpeed
            // 
            this.labelEnemyXSpeed.AutoSize = true;
            this.labelEnemyXSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnemyXSpeed.Location = new System.Drawing.Point(16, 76);
            this.labelEnemyXSpeed.Name = "labelEnemyXSpeed";
            this.labelEnemyXSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelEnemyXSpeed.TabIndex = 10;
            this.labelEnemyXSpeed.Text = "XSpeed";
            // 
            // labelEnemyDamage
            // 
            this.labelEnemyDamage.AutoSize = true;
            this.labelEnemyDamage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnemyDamage.Location = new System.Drawing.Point(15, 148);
            this.labelEnemyDamage.Name = "labelEnemyDamage";
            this.labelEnemyDamage.Size = new System.Drawing.Size(47, 13);
            this.labelEnemyDamage.TabIndex = 11;
            this.labelEnemyDamage.Text = "Damage";
            // 
            // textBoxPlayerHealth
            // 
            this.textBoxPlayerHealth.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerHealth.Location = new System.Drawing.Point(67, 37);
            this.textBoxPlayerHealth.Name = "textBoxPlayerHealth";
            this.textBoxPlayerHealth.Size = new System.Drawing.Size(101, 26);
            this.textBoxPlayerHealth.TabIndex = 13;
            // 
            // textBoxPlayerXSpeed
            // 
            this.textBoxPlayerXSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerXSpeed.Location = new System.Drawing.Point(67, 71);
            this.textBoxPlayerXSpeed.Name = "textBoxPlayerXSpeed";
            this.textBoxPlayerXSpeed.Size = new System.Drawing.Size(101, 26);
            this.textBoxPlayerXSpeed.TabIndex = 14;
            // 
            // textBoxPlayerDamage
            // 
            this.textBoxPlayerDamage.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerDamage.Location = new System.Drawing.Point(67, 141);
            this.textBoxPlayerDamage.Name = "textBoxPlayerDamage";
            this.textBoxPlayerDamage.Size = new System.Drawing.Size(101, 26);
            this.textBoxPlayerDamage.TabIndex = 15;
            // 
            // textBoxEnemyHealth
            // 
            this.textBoxEnemyHealth.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEnemyHealth.Location = new System.Drawing.Point(68, 35);
            this.textBoxEnemyHealth.Name = "textBoxEnemyHealth";
            this.textBoxEnemyHealth.Size = new System.Drawing.Size(101, 26);
            this.textBoxEnemyHealth.TabIndex = 17;
            // 
            // textBoxEnemyXSpeed
            // 
            this.textBoxEnemyXSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEnemyXSpeed.Location = new System.Drawing.Point(68, 69);
            this.textBoxEnemyXSpeed.Name = "textBoxEnemyXSpeed";
            this.textBoxEnemyXSpeed.Size = new System.Drawing.Size(101, 26);
            this.textBoxEnemyXSpeed.TabIndex = 18;
            // 
            // textBoxEnemyDamage
            // 
            this.textBoxEnemyDamage.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEnemyDamage.Location = new System.Drawing.Point(68, 141);
            this.textBoxEnemyDamage.Name = "textBoxEnemyDamage";
            this.textBoxEnemyDamage.Size = new System.Drawing.Size(101, 26);
            this.textBoxEnemyDamage.TabIndex = 19;
            // 
            // buttonConfirm
            // 
            this.buttonConfirm.Location = new System.Drawing.Point(472, 564);
            this.buttonConfirm.Name = "buttonConfirm";
            this.buttonConfirm.Size = new System.Drawing.Size(252, 67);
            this.buttonConfirm.TabIndex = 20;
            this.buttonConfirm.Text = "Confirm Changes";
            this.buttonConfirm.UseVisualStyleBackColor = true;
            this.buttonConfirm.Click += new System.EventHandler(this.buttonConfirm_Click);
            // 
            // labelFlyingEnemyHealth
            // 
            this.labelFlyingEnemyHealth.AutoSize = true;
            this.labelFlyingEnemyHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlyingEnemyHealth.Location = new System.Drawing.Point(27, 43);
            this.labelFlyingEnemyHealth.Name = "labelFlyingEnemyHealth";
            this.labelFlyingEnemyHealth.Size = new System.Drawing.Size(38, 13);
            this.labelFlyingEnemyHealth.TabIndex = 21;
            this.labelFlyingEnemyHealth.Text = "Health";
            // 
            // labelFlyingEnemyXSpeed
            // 
            this.labelFlyingEnemyXSpeed.AutoSize = true;
            this.labelFlyingEnemyXSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlyingEnemyXSpeed.Location = new System.Drawing.Point(24, 75);
            this.labelFlyingEnemyXSpeed.Name = "labelFlyingEnemyXSpeed";
            this.labelFlyingEnemyXSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelFlyingEnemyXSpeed.TabIndex = 22;
            this.labelFlyingEnemyXSpeed.Text = "XSpeed";
            // 
            // labelFlyingEnemyDamage
            // 
            this.labelFlyingEnemyDamage.AutoSize = true;
            this.labelFlyingEnemyDamage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlyingEnemyDamage.Location = new System.Drawing.Point(23, 148);
            this.labelFlyingEnemyDamage.Name = "labelFlyingEnemyDamage";
            this.labelFlyingEnemyDamage.Size = new System.Drawing.Size(47, 13);
            this.labelFlyingEnemyDamage.TabIndex = 23;
            this.labelFlyingEnemyDamage.Text = "Damage";
            // 
            // textBoxFlyingEnemyHealth
            // 
            this.textBoxFlyingEnemyHealth.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFlyingEnemyHealth.Location = new System.Drawing.Point(71, 35);
            this.textBoxFlyingEnemyHealth.Name = "textBoxFlyingEnemyHealth";
            this.textBoxFlyingEnemyHealth.Size = new System.Drawing.Size(101, 26);
            this.textBoxFlyingEnemyHealth.TabIndex = 24;
            // 
            // textBoxFlyingEnemyXSpeed
            // 
            this.textBoxFlyingEnemyXSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFlyingEnemyXSpeed.Location = new System.Drawing.Point(71, 69);
            this.textBoxFlyingEnemyXSpeed.Name = "textBoxFlyingEnemyXSpeed";
            this.textBoxFlyingEnemyXSpeed.Size = new System.Drawing.Size(100, 26);
            this.textBoxFlyingEnemyXSpeed.TabIndex = 25;
            // 
            // textBoxFlyingEnemyDamage
            // 
            this.textBoxFlyingEnemyDamage.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFlyingEnemyDamage.Location = new System.Drawing.Point(71, 141);
            this.textBoxFlyingEnemyDamage.Name = "textBoxFlyingEnemyDamage";
            this.textBoxFlyingEnemyDamage.Size = new System.Drawing.Size(101, 26);
            this.textBoxFlyingEnemyDamage.TabIndex = 26;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(639, 51);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(210, 268);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            // 
            // labelFlyingEnemy
            // 
            this.labelFlyingEnemy.AutoSize = true;
            this.labelFlyingEnemy.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlyingEnemy.Location = new System.Drawing.Point(675, 21);
            this.labelFlyingEnemy.Name = "labelFlyingEnemy";
            this.labelFlyingEnemy.Size = new System.Drawing.Size(138, 24);
            this.labelFlyingEnemy.TabIndex = 28;
            this.labelFlyingEnemy.Text = "Flying Enemy";
            // 
            // groupBoxPlayer
            // 
            this.groupBoxPlayer.BackColor = System.Drawing.Color.LightSlateGray;
            this.groupBoxPlayer.Controls.Add(this.textBoxPlayerYSpeed);
            this.groupBoxPlayer.Controls.Add(this.labelPlayerYSpeed);
            this.groupBoxPlayer.Controls.Add(this.textBoxPlayerDamage);
            this.groupBoxPlayer.Controls.Add(this.textBoxPlayerXSpeed);
            this.groupBoxPlayer.Controls.Add(this.textBoxPlayerHealth);
            this.groupBoxPlayer.Controls.Add(this.labelPlayerDamage);
            this.groupBoxPlayer.Controls.Add(this.labelPlayerXSpeed);
            this.groupBoxPlayer.Controls.Add(this.labelPlayerHealth);
            this.groupBoxPlayer.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPlayer.Location = new System.Drawing.Point(56, 349);
            this.groupBoxPlayer.Name = "groupBoxPlayer";
            this.groupBoxPlayer.Size = new System.Drawing.Size(210, 211);
            this.groupBoxPlayer.TabIndex = 29;
            this.groupBoxPlayer.TabStop = false;
            this.groupBoxPlayer.Text = "Player Stats";
            // 
            // textBoxPlayerYSpeed
            // 
            this.textBoxPlayerYSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxPlayerYSpeed.Location = new System.Drawing.Point(67, 106);
            this.textBoxPlayerYSpeed.Name = "textBoxPlayerYSpeed";
            this.textBoxPlayerYSpeed.Size = new System.Drawing.Size(101, 26);
            this.textBoxPlayerYSpeed.TabIndex = 17;
            // 
            // labelPlayerYSpeed
            // 
            this.labelPlayerYSpeed.AutoSize = true;
            this.labelPlayerYSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPlayerYSpeed.Location = new System.Drawing.Point(15, 113);
            this.labelPlayerYSpeed.Name = "labelPlayerYSpeed";
            this.labelPlayerYSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelPlayerYSpeed.TabIndex = 16;
            this.labelPlayerYSpeed.Text = "YSpeed";
            // 
            // groupBoxEnemy
            // 
            this.groupBoxEnemy.BackColor = System.Drawing.Color.LightSlateGray;
            this.groupBoxEnemy.Controls.Add(this.textBoxEnemyYSpeed);
            this.groupBoxEnemy.Controls.Add(this.labelEnemyYSpeed);
            this.groupBoxEnemy.Controls.Add(this.textBoxEnemyDamage);
            this.groupBoxEnemy.Controls.Add(this.textBoxEnemyXSpeed);
            this.groupBoxEnemy.Controls.Add(this.textBoxEnemyHealth);
            this.groupBoxEnemy.Controls.Add(this.labelEnemyDamage);
            this.groupBoxEnemy.Controls.Add(this.labelEnemyXSpeed);
            this.groupBoxEnemy.Controls.Add(this.labelEnemyHealth);
            this.groupBoxEnemy.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxEnemy.Location = new System.Drawing.Point(352, 350);
            this.groupBoxEnemy.Name = "groupBoxEnemy";
            this.groupBoxEnemy.Size = new System.Drawing.Size(210, 208);
            this.groupBoxEnemy.TabIndex = 30;
            this.groupBoxEnemy.TabStop = false;
            this.groupBoxEnemy.Text = "Enemy Stats";
            // 
            // textBoxEnemyYSpeed
            // 
            this.textBoxEnemyYSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxEnemyYSpeed.Location = new System.Drawing.Point(68, 106);
            this.textBoxEnemyYSpeed.Name = "textBoxEnemyYSpeed";
            this.textBoxEnemyYSpeed.Size = new System.Drawing.Size(100, 26);
            this.textBoxEnemyYSpeed.TabIndex = 21;
            // 
            // labelEnemyYSpeed
            // 
            this.labelEnemyYSpeed.AutoSize = true;
            this.labelEnemyYSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnemyYSpeed.Location = new System.Drawing.Point(16, 113);
            this.labelEnemyYSpeed.Name = "labelEnemyYSpeed";
            this.labelEnemyYSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelEnemyYSpeed.TabIndex = 20;
            this.labelEnemyYSpeed.Text = "YSpeed";
            // 
            // groupBoxFlyingEnemy
            // 
            this.groupBoxFlyingEnemy.BackColor = System.Drawing.Color.LightSlateGray;
            this.groupBoxFlyingEnemy.Controls.Add(this.textBoxFlyingEnemyYSpeed);
            this.groupBoxFlyingEnemy.Controls.Add(this.labelFlyingEnemyYSpeed);
            this.groupBoxFlyingEnemy.Controls.Add(this.textBoxFlyingEnemyDamage);
            this.groupBoxFlyingEnemy.Controls.Add(this.labelFlyingEnemyHealth);
            this.groupBoxFlyingEnemy.Controls.Add(this.textBoxFlyingEnemyHealth);
            this.groupBoxFlyingEnemy.Controls.Add(this.textBoxFlyingEnemyXSpeed);
            this.groupBoxFlyingEnemy.Controls.Add(this.labelFlyingEnemyXSpeed);
            this.groupBoxFlyingEnemy.Controls.Add(this.labelFlyingEnemyDamage);
            this.groupBoxFlyingEnemy.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxFlyingEnemy.Location = new System.Drawing.Point(639, 350);
            this.groupBoxFlyingEnemy.Name = "groupBoxFlyingEnemy";
            this.groupBoxFlyingEnemy.Size = new System.Drawing.Size(210, 208);
            this.groupBoxFlyingEnemy.TabIndex = 31;
            this.groupBoxFlyingEnemy.TabStop = false;
            this.groupBoxFlyingEnemy.Text = "Flying Enemy Stats";
            // 
            // textBoxFlyingEnemyYSpeed
            // 
            this.textBoxFlyingEnemyYSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxFlyingEnemyYSpeed.Location = new System.Drawing.Point(71, 105);
            this.textBoxFlyingEnemyYSpeed.Name = "textBoxFlyingEnemyYSpeed";
            this.textBoxFlyingEnemyYSpeed.Size = new System.Drawing.Size(101, 26);
            this.textBoxFlyingEnemyYSpeed.TabIndex = 28;
            // 
            // labelFlyingEnemyYSpeed
            // 
            this.labelFlyingEnemyYSpeed.AutoSize = true;
            this.labelFlyingEnemyYSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlyingEnemyYSpeed.Location = new System.Drawing.Point(24, 110);
            this.labelFlyingEnemyYSpeed.Name = "labelFlyingEnemyYSpeed";
            this.labelFlyingEnemyYSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelFlyingEnemyYSpeed.TabIndex = 27;
            this.labelFlyingEnemyYSpeed.Text = "YSpeed";
            // 
            // groupBoxBoss
            // 
            this.groupBoxBoss.BackColor = System.Drawing.Color.LightSlateGray;
            this.groupBoxBoss.Controls.Add(this.textBoxBossDamage);
            this.groupBoxBoss.Controls.Add(this.labelBossDamage);
            this.groupBoxBoss.Controls.Add(this.labelBossYSpeed);
            this.groupBoxBoss.Controls.Add(this.textBoxBossXSpeed);
            this.groupBoxBoss.Controls.Add(this.labelBossXSpeed);
            this.groupBoxBoss.Controls.Add(this.textBoxBossYSpeed);
            this.groupBoxBoss.Controls.Add(this.labelBossHealth);
            this.groupBoxBoss.Controls.Add(this.textBoxBossHealth);
            this.groupBoxBoss.Font = new System.Drawing.Font("Franklin Gothic Heavy", 14.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxBoss.Location = new System.Drawing.Point(926, 349);
            this.groupBoxBoss.Name = "groupBoxBoss";
            this.groupBoxBoss.Size = new System.Drawing.Size(210, 209);
            this.groupBoxBoss.TabIndex = 32;
            this.groupBoxBoss.TabStop = false;
            this.groupBoxBoss.Text = "Boss Stats";
            // 
            // textBoxBossDamage
            // 
            this.textBoxBossDamage.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBossDamage.Location = new System.Drawing.Point(67, 142);
            this.textBoxBossDamage.Name = "textBoxBossDamage";
            this.textBoxBossDamage.Size = new System.Drawing.Size(100, 26);
            this.textBoxBossDamage.TabIndex = 40;
            // 
            // labelBossDamage
            // 
            this.labelBossDamage.AutoSize = true;
            this.labelBossDamage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBossDamage.Location = new System.Drawing.Point(14, 149);
            this.labelBossDamage.Name = "labelBossDamage";
            this.labelBossDamage.Size = new System.Drawing.Size(47, 13);
            this.labelBossDamage.TabIndex = 36;
            this.labelBossDamage.Text = "Damage";
            // 
            // labelBossYSpeed
            // 
            this.labelBossYSpeed.AutoSize = true;
            this.labelBossYSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBossYSpeed.Location = new System.Drawing.Point(18, 113);
            this.labelBossYSpeed.Name = "labelBossYSpeed";
            this.labelBossYSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelBossYSpeed.TabIndex = 35;
            this.labelBossYSpeed.Text = "YSpeed";
            // 
            // textBoxBossXSpeed
            // 
            this.textBoxBossXSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBossXSpeed.Location = new System.Drawing.Point(67, 73);
            this.textBoxBossXSpeed.Name = "textBoxBossXSpeed";
            this.textBoxBossXSpeed.Size = new System.Drawing.Size(100, 26);
            this.textBoxBossXSpeed.TabIndex = 38;
            // 
            // labelBossXSpeed
            // 
            this.labelBossXSpeed.AutoSize = true;
            this.labelBossXSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBossXSpeed.Location = new System.Drawing.Point(16, 78);
            this.labelBossXSpeed.Name = "labelBossXSpeed";
            this.labelBossXSpeed.Size = new System.Drawing.Size(45, 13);
            this.labelBossXSpeed.TabIndex = 34;
            this.labelBossXSpeed.Text = "XSpeed";
            // 
            // textBoxBossYSpeed
            // 
            this.textBoxBossYSpeed.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBossYSpeed.Location = new System.Drawing.Point(67, 107);
            this.textBoxBossYSpeed.Name = "textBoxBossYSpeed";
            this.textBoxBossYSpeed.Size = new System.Drawing.Size(100, 26);
            this.textBoxBossYSpeed.TabIndex = 39;
            // 
            // labelBossHealth
            // 
            this.labelBossHealth.AutoSize = true;
            this.labelBossHealth.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBossHealth.Location = new System.Drawing.Point(23, 46);
            this.labelBossHealth.Name = "labelBossHealth";
            this.labelBossHealth.Size = new System.Drawing.Size(38, 13);
            this.labelBossHealth.TabIndex = 33;
            this.labelBossHealth.Text = "Health";
            // 
            // textBoxBossHealth
            // 
            this.textBoxBossHealth.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxBossHealth.Location = new System.Drawing.Point(67, 39);
            this.textBoxBossHealth.Name = "textBoxBossHealth";
            this.textBoxBossHealth.Size = new System.Drawing.Size(100, 26);
            this.textBoxBossHealth.TabIndex = 37;
            // 
            // pictureBoxBoss
            // 
            this.pictureBoxBoss.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxBoss.BackgroundImage")));
            this.pictureBoxBoss.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBoxBoss.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxBoss.Location = new System.Drawing.Point(926, 52);
            this.pictureBoxBoss.Name = "pictureBoxBoss";
            this.pictureBoxBoss.Size = new System.Drawing.Size(210, 267);
            this.pictureBoxBoss.TabIndex = 33;
            this.pictureBoxBoss.TabStop = false;
            // 
            // labelBoss
            // 
            this.labelBoss.AutoSize = true;
            this.labelBoss.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBoss.Location = new System.Drawing.Point(1003, 21);
            this.labelBoss.Name = "labelBoss";
            this.labelBoss.Size = new System.Drawing.Size(55, 24);
            this.labelBoss.TabIndex = 34;
            this.labelBoss.Text = "Boss";
            // 
            // CharacterEditorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1197, 641);
            this.Controls.Add(this.labelBoss);
            this.Controls.Add(this.pictureBoxBoss);
            this.Controls.Add(this.groupBoxBoss);
            this.Controls.Add(this.groupBoxFlyingEnemy);
            this.Controls.Add(this.groupBoxEnemy);
            this.Controls.Add(this.groupBoxPlayer);
            this.Controls.Add(this.labelFlyingEnemy);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonConfirm);
            this.Controls.Add(this.labelEnemy);
            this.Controls.Add(this.labelNacho);
            this.Controls.Add(this.pictureBoxEnemy);
            this.Controls.Add(this.pictureBoxPlayer);
            this.Name = "CharacterEditorForm";
            this.Text = "Character Editor";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPlayer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEnemy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBoxPlayer.ResumeLayout(false);
            this.groupBoxPlayer.PerformLayout();
            this.groupBoxEnemy.ResumeLayout(false);
            this.groupBoxEnemy.PerformLayout();
            this.groupBoxFlyingEnemy.ResumeLayout(false);
            this.groupBoxFlyingEnemy.PerformLayout();
            this.groupBoxBoss.ResumeLayout(false);
            this.groupBoxBoss.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoss)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelPlayerHealth;
        private System.Windows.Forms.Label labelPlayerXSpeed;
        private System.Windows.Forms.Label labelPlayerDamage;
        private System.Windows.Forms.PictureBox pictureBoxPlayer;
        private System.Windows.Forms.PictureBox pictureBoxEnemy;
        private System.Windows.Forms.Label labelNacho;
        private System.Windows.Forms.Label labelEnemy;
        private System.Windows.Forms.Label labelEnemyHealth;
        private System.Windows.Forms.Label labelEnemyXSpeed;
        private System.Windows.Forms.Label labelEnemyDamage;
        private System.Windows.Forms.TextBox textBoxPlayerHealth;
        private System.Windows.Forms.TextBox textBoxPlayerXSpeed;
        private System.Windows.Forms.TextBox textBoxPlayerDamage;
        private System.Windows.Forms.TextBox textBoxEnemyHealth;
        private System.Windows.Forms.TextBox textBoxEnemyXSpeed;
        private System.Windows.Forms.TextBox textBoxEnemyDamage;
        private System.Windows.Forms.Button buttonConfirm;
        private System.Windows.Forms.Label labelFlyingEnemyHealth;
        private System.Windows.Forms.Label labelFlyingEnemyXSpeed;
        private System.Windows.Forms.Label labelFlyingEnemyDamage;
        private System.Windows.Forms.TextBox textBoxFlyingEnemyHealth;
        private System.Windows.Forms.TextBox textBoxFlyingEnemyXSpeed;
        private System.Windows.Forms.TextBox textBoxFlyingEnemyDamage;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label labelFlyingEnemy;
        private System.Windows.Forms.GroupBox groupBoxPlayer;
        private System.Windows.Forms.GroupBox groupBoxEnemy;
        private System.Windows.Forms.GroupBox groupBoxFlyingEnemy;
        private System.Windows.Forms.Label labelFlyingEnemyYSpeed;
        private System.Windows.Forms.TextBox textBoxFlyingEnemyYSpeed;
        private System.Windows.Forms.TextBox textBoxEnemyYSpeed;
        private System.Windows.Forms.Label labelEnemyYSpeed;
        private System.Windows.Forms.TextBox textBoxPlayerYSpeed;
        private System.Windows.Forms.Label labelPlayerYSpeed;
        private System.Windows.Forms.GroupBox groupBoxBoss;
        private System.Windows.Forms.TextBox textBoxBossDamage;
        private System.Windows.Forms.Label labelBossDamage;
        private System.Windows.Forms.Label labelBossYSpeed;
        private System.Windows.Forms.TextBox textBoxBossXSpeed;
        private System.Windows.Forms.Label labelBossXSpeed;
        private System.Windows.Forms.TextBox textBoxBossYSpeed;
        private System.Windows.Forms.Label labelBossHealth;
        private System.Windows.Forms.TextBox textBoxBossHealth;
        private System.Windows.Forms.PictureBox pictureBoxBoss;
        private System.Windows.Forms.Label labelBoss;
    }
}

