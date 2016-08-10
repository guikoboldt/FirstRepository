namespace RestaurantApp.Voting.DesktopApp
{
    partial class LoginForm
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
            this.UserOptionListControl = new System.Windows.Forms.ComboBox();
            this.credentialsGroupBox = new System.Windows.Forms.GroupBox();
            this.ExitButton = new System.Windows.Forms.Button();
            this.LoginBotton = new System.Windows.Forms.Button();
            this.userPasswordLabel = new System.Windows.Forms.Label();
            this.userLoginLabel = new System.Windows.Forms.Label();
            this.UserPasswordTextInput = new System.Windows.Forms.TextBox();
            this.credentialsGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserOptionListControl
            // 
            this.UserOptionListControl.DisplayMember = "Name";
            this.UserOptionListControl.FormattingEnabled = true;
            this.UserOptionListControl.Location = new System.Drawing.Point(80, 33);
            this.UserOptionListControl.Name = "UserOptionListControl";
            this.UserOptionListControl.Size = new System.Drawing.Size(195, 21);
            this.UserOptionListControl.TabIndex = 0;
            this.UserOptionListControl.ValueMember = "Name";
            // 
            // credentialsGroupBox
            // 
            this.credentialsGroupBox.Controls.Add(this.ExitButton);
            this.credentialsGroupBox.Controls.Add(this.LoginBotton);
            this.credentialsGroupBox.Controls.Add(this.userPasswordLabel);
            this.credentialsGroupBox.Controls.Add(this.userLoginLabel);
            this.credentialsGroupBox.Controls.Add(this.UserPasswordTextInput);
            this.credentialsGroupBox.Controls.Add(this.UserOptionListControl);
            this.credentialsGroupBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.credentialsGroupBox.Location = new System.Drawing.Point(28, 36);
            this.credentialsGroupBox.Name = "credentialsGroupBox";
            this.credentialsGroupBox.Size = new System.Drawing.Size(301, 159);
            this.credentialsGroupBox.TabIndex = 1;
            this.credentialsGroupBox.TabStop = false;
            this.credentialsGroupBox.Text = "Credenciais";
            // 
            // ExitButton
            // 
            this.ExitButton.Location = new System.Drawing.Point(189, 113);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(86, 23);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "Cancelar";
            this.ExitButton.UseVisualStyleBackColor = true;
            this.ExitButton.Click += new System.EventHandler(this.OnExit);
            // 
            // LoginBotton
            // 
            this.LoginBotton.Location = new System.Drawing.Point(80, 113);
            this.LoginBotton.Name = "LoginBotton";
            this.LoginBotton.Size = new System.Drawing.Size(86, 23);
            this.LoginBotton.TabIndex = 4;
            this.LoginBotton.Text = "Entrar";
            this.LoginBotton.UseVisualStyleBackColor = true;
            this.LoginBotton.Click += new System.EventHandler(this.OnLogin);
            // 
            // userPasswordLabel
            // 
            this.userPasswordLabel.AutoSize = true;
            this.userPasswordLabel.Location = new System.Drawing.Point(26, 77);
            this.userPasswordLabel.Name = "userPasswordLabel";
            this.userPasswordLabel.Size = new System.Drawing.Size(47, 13);
            this.userPasswordLabel.TabIndex = 3;
            this.userPasswordLabel.Text = "Senha:";
            // 
            // userLoginLabel
            // 
            this.userLoginLabel.AutoSize = true;
            this.userLoginLabel.Location = new System.Drawing.Point(19, 36);
            this.userLoginLabel.Name = "userLoginLabel";
            this.userLoginLabel.Size = new System.Drawing.Size(54, 13);
            this.userLoginLabel.TabIndex = 2;
            this.userLoginLabel.Text = "Usuário:";
            // 
            // UserPasswordTextInput
            // 
            this.UserPasswordTextInput.Location = new System.Drawing.Point(80, 74);
            this.UserPasswordTextInput.Name = "UserPasswordTextInput";
            this.UserPasswordTextInput.Size = new System.Drawing.Size(195, 20);
            this.UserPasswordTextInput.TabIndex = 1;
            this.UserPasswordTextInput.UseSystemPasswordChar = true;
            // 
            // LoginForm
            // 
            this.AcceptButton = this.LoginBotton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(357, 217);
            this.Controls.Add(this.credentialsGroupBox);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(373, 255);
            this.Name = "LoginForm";
            this.Opacity = 0.95D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.SubTitle = "Login";
            this.Text = "Votação de Restaurante - Login";
            this.Title = "Votação de Restaurante";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.Controls.SetChildIndex(this.credentialsGroupBox, 0);
            this.credentialsGroupBox.ResumeLayout(false);
            this.credentialsGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox UserOptionListControl;
        private System.Windows.Forms.GroupBox credentialsGroupBox;
        private System.Windows.Forms.Label userPasswordLabel;
        private System.Windows.Forms.Label userLoginLabel;
        private System.Windows.Forms.TextBox UserPasswordTextInput;
        private System.Windows.Forms.Button LoginBotton;
        private System.Windows.Forms.Button ExitButton;
    }
}