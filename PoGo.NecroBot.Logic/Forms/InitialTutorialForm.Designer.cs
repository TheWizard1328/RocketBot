namespace PoGo.NecroBot.Logic.Forms
{
    partial class InitialTutorialForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitialTutorialForm));
            this.wizardControl1 = new AeroWizard.WizardControl();
            this.Step1 = new AeroWizard.WizardPage();
            this.rdoFemale = new System.Windows.Forms.RadioButton();
            this.rdoMale = new System.Windows.Forms.RadioButton();
            this.SavingGender_Page = new AeroWizard.WizardPage();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Step2 = new AeroWizard.WizardPage();
            this.rdoCharmander = new System.Windows.Forms.RadioButton();
            this.rdoSquirtle = new System.Windows.Forms.RadioButton();
            this.rdoBulbasaur = new System.Windows.Forms.RadioButton();
            this.Step2_2 = new AeroWizard.WizardPage();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Step3 = new AeroWizard.WizardPage();
            this.lblNameError = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNick = new System.Windows.Forms.TextBox();
            this.Step3_3 = new AeroWizard.WizardPage();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).BeginInit();
            this.Step1.SuspendLayout();
            this.SavingGender_Page.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.Step2.SuspendLayout();
            this.Step2_2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.Step3.SuspendLayout();
            this.Step3_3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // wizardControl1
            // 
            this.wizardControl1.BackColor = System.Drawing.Color.White;
            this.wizardControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.wizardControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardControl1.Location = new System.Drawing.Point(0, 0);
            this.wizardControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.wizardControl1.Name = "wizardControl1";
            this.wizardControl1.Pages.Add(this.Step1);
            this.wizardControl1.Pages.Add(this.SavingGender_Page);
            this.wizardControl1.Pages.Add(this.Step2);
            this.wizardControl1.Pages.Add(this.Step2_2);
            this.wizardControl1.Pages.Add(this.Step3);
            this.wizardControl1.Pages.Add(this.Step3_3);
            this.wizardControl1.Size = new System.Drawing.Size(561, 298);
            this.wizardControl1.TabIndex = 0;
            this.wizardControl1.Text = "Setting";
            this.wizardControl1.Title = "Beginner Tutorial";
            // 
            // Step1
            // 
            this.Step1.Controls.Add(this.rdoFemale);
            this.Step1.Controls.Add(this.rdoMale);
            this.Step1.Name = "Step1";
            this.Step1.Size = new System.Drawing.Size(514, 111);
            this.Step1.TabIndex = 0;
            this.Step1.Text = "Select  Gender";
            this.Step1.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.WizardPage1_Initialize);
            // 
            // rdoFemale
            // 
            this.rdoFemale.AutoSize = true;
            this.rdoFemale.Location = new System.Drawing.Point(236, 48);
            this.rdoFemale.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoFemale.Name = "rdoFemale";
            this.rdoFemale.Size = new System.Drawing.Size(78, 24);
            this.rdoFemale.TabIndex = 1;
            this.rdoFemale.Text = "Female";
            this.rdoFemale.UseVisualStyleBackColor = true;
            // 
            // rdoMale
            // 
            this.rdoMale.AutoSize = true;
            this.rdoMale.Checked = true;
            this.rdoMale.Location = new System.Drawing.Point(119, 48);
            this.rdoMale.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoMale.Name = "rdoMale";
            this.rdoMale.Size = new System.Drawing.Size(63, 24);
            this.rdoMale.TabIndex = 0;
            this.rdoMale.TabStop = true;
            this.rdoMale.Text = "Male";
            this.rdoMale.UseVisualStyleBackColor = true;
            // 
            // SavingGender_Page
            // 
            this.SavingGender_Page.AllowBack = false;
            this.SavingGender_Page.AllowNext = false;
            this.SavingGender_Page.Controls.Add(this.pictureBox3);
            this.SavingGender_Page.Controls.Add(this.label2);
            this.SavingGender_Page.Name = "SavingGender_Page";
            this.SavingGender_Page.ShowNext = false;
            this.SavingGender_Page.Size = new System.Drawing.Size(499, 108);
            this.SavingGender_Page.TabIndex = 3;
            this.SavingGender_Page.Text = "Working in progress";
            this.SavingGender_Page.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.WizardPage4_Initialize);
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::PoGo.NecroBot.Logic.Properties.Resources.ajax_loader;
            this.pictureBox3.Location = new System.Drawing.Point(131, 34);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(39, 41);
            this.pictureBox3.TabIndex = 3;
            this.pictureBox3.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(192, 34);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Please wait........";
            // 
            // Step2
            // 
            this.Step2.AllowBack = false;
            this.Step2.AllowCancel = false;
            this.Step2.Controls.Add(this.rdoCharmander);
            this.Step2.Controls.Add(this.rdoSquirtle);
            this.Step2.Controls.Add(this.rdoBulbasaur);
            this.Step2.Name = "Step2";
            this.Step2.ShowCancel = false;
            this.Step2.Size = new System.Drawing.Size(499, 108);
            this.Step2.TabIndex = 1;
            this.Step2.Text = "First Catch";
            this.Step2.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.WizardPage2_Initialize);
            // 
            // rdoCharmander
            // 
            this.rdoCharmander.AutoSize = true;
            this.rdoCharmander.Location = new System.Drawing.Point(175, 42);
            this.rdoCharmander.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoCharmander.Name = "rdoCharmander";
            this.rdoCharmander.Size = new System.Drawing.Size(111, 24);
            this.rdoCharmander.TabIndex = 2;
            this.rdoCharmander.Text = "Charmander";
            this.rdoCharmander.UseVisualStyleBackColor = true;
            // 
            // rdoSquirtle
            // 
            this.rdoSquirtle.AutoSize = true;
            this.rdoSquirtle.Location = new System.Drawing.Point(333, 42);
            this.rdoSquirtle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoSquirtle.Name = "rdoSquirtle";
            this.rdoSquirtle.Size = new System.Drawing.Size(81, 24);
            this.rdoSquirtle.TabIndex = 1;
            this.rdoSquirtle.Text = "Squirtle";
            this.rdoSquirtle.UseVisualStyleBackColor = true;
            // 
            // rdoBulbasaur
            // 
            this.rdoBulbasaur.AutoSize = true;
            this.rdoBulbasaur.Checked = true;
            this.rdoBulbasaur.Location = new System.Drawing.Point(25, 42);
            this.rdoBulbasaur.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rdoBulbasaur.Name = "rdoBulbasaur";
            this.rdoBulbasaur.Size = new System.Drawing.Size(95, 24);
            this.rdoBulbasaur.TabIndex = 0;
            this.rdoBulbasaur.TabStop = true;
            this.rdoBulbasaur.Text = "Bulbasaur";
            this.rdoBulbasaur.UseVisualStyleBackColor = true;
            // 
            // Step2_2
            // 
            this.Step2_2.AllowBack = false;
            this.Step2_2.AllowNext = false;
            this.Step2_2.Controls.Add(this.pictureBox2);
            this.Step2_2.Controls.Add(this.label3);
            this.Step2_2.Name = "Step2_2";
            this.Step2_2.Size = new System.Drawing.Size(499, 108);
            this.Step2_2.TabIndex = 2;
            this.Step2_2.Text = "Working in progress";
            this.Step2_2.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.WizardPage3_Initialize);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::PoGo.NecroBot.Logic.Properties.Resources.ajax_loader;
            this.pictureBox2.Location = new System.Drawing.Point(124, 39);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(39, 33);
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(189, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(107, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Please wait........";
            // 
            // Step3
            // 
            this.Step3.Controls.Add(this.lblNameError);
            this.Step3.Controls.Add(this.label1);
            this.Step3.Controls.Add(this.txtNick);
            this.Step3.Name = "Step3";
            this.Step3.Size = new System.Drawing.Size(499, 108);
            this.Step3.TabIndex = 4;
            this.Step3.Text = "Select Nickname";
            this.Step3.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.WizardPage5_Initialize);
            // 
            // lblNameError
            // 
            this.lblNameError.AutoSize = true;
            this.lblNameError.ForeColor = System.Drawing.Color.Red;
            this.lblNameError.Location = new System.Drawing.Point(13, 59);
            this.lblNameError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNameError.Name = "lblNameError";
            this.lblNameError.Size = new System.Drawing.Size(41, 20);
            this.lblNameError.TabIndex = 2;
            this.lblNameError.Text = "error";
            this.lblNameError.UseWaitCursor = true;
            this.lblNameError.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nickname";
            this.label1.UseWaitCursor = true;
            // 
            // txtNick
            // 
            this.txtNick.Location = new System.Drawing.Point(103, 20);
            this.txtNick.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.txtNick.Name = "txtNick";
            this.txtNick.Size = new System.Drawing.Size(329, 27);
            this.txtNick.TabIndex = 0;
            // 
            // Step3_3
            // 
            this.Step3_3.AllowBack = false;
            this.Step3_3.Controls.Add(this.pictureBox1);
            this.Step3_3.Controls.Add(this.label4);
            this.Step3_3.Name = "Step3_3";
            this.Step3_3.Size = new System.Drawing.Size(499, 108);
            this.Step3_3.TabIndex = 5;
            this.Step3_3.Text = "Working ‌ in progress";
            this.Step3_3.Initialize += new System.EventHandler<AeroWizard.WizardPageInitEventArgs>(this.WizardPage6_Initialize);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PoGo.NecroBot.Logic.Properties.Resources.ajax_loader;
            this.pictureBox1.Location = new System.Drawing.Point(115, 15);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(39, 41);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(161, 15);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(107, 20);
            this.label4.TabIndex = 1;
            this.label4.Text = "Please wait........";
            // 
            // InitialTutorialForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(561, 298);
            this.Controls.Add(this.wizardControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "InitialTutorialForm";
            this.Text = "Initial Account Tutorial";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.wizardControl1)).EndInit();
            this.Step1.ResumeLayout(false);
            this.Step1.PerformLayout();
            this.SavingGender_Page.ResumeLayout(false);
            this.SavingGender_Page.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.Step2.ResumeLayout(false);
            this.Step2.PerformLayout();
            this.Step2_2.ResumeLayout(false);
            this.Step2_2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.Step3.ResumeLayout(false);
            this.Step3.PerformLayout();
            this.Step3_3.ResumeLayout(false);
            this.Step3_3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AeroWizard.WizardControl wizardControl1;
        private AeroWizard.WizardPage Step1;
        private System.Windows.Forms.RadioButton rdoFemale;
        private System.Windows.Forms.RadioButton rdoMale;
        private AeroWizard.WizardPage Step2;
        private AeroWizard.WizardPage Step2_2;
        private AeroWizard.WizardPage SavingGender_Page;
        private System.Windows.Forms.RadioButton rdoCharmander;
        private System.Windows.Forms.RadioButton rdoSquirtle;
        private System.Windows.Forms.RadioButton rdoBulbasaur;
        private AeroWizard.WizardPage Step3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNick;
        private AeroWizard.WizardPage Step3_3;
        private System.Windows.Forms.Label lblNameError;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}