
namespace RestaurantApp.Voting.DesktopApp
{
    partial class MainVotingForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainVotingForm));
            this.RestaurantsListGroupBox = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.VoteOptionsListControl = new System.Windows.Forms.ListBox();
            this.VoteButton = new System.Windows.Forms.Button();
            this.VoteResultGroupBox = new System.Windows.Forms.GroupBox();
            this.VoteResultPanel = new System.Windows.Forms.Panel();
            this.PollClosingAlertLabel = new System.Windows.Forms.Label();
            this.MostVotedLabel = new System.Windows.Forms.Label();
            this.MonthCalendar = new System.Windows.Forms.MonthCalendar();
            this.MostVotedCadidateLabel = new System.Windows.Forms.Label();
            this.LaunchTimeCountDownLabel = new System.Windows.Forms.Label();
            this.RefreshVoteResultButton = new System.Windows.Forms.Button();
            this.LoggedUserLabel = new System.Windows.Forms.Label();
            this.LoggedUserNameLabel = new System.Windows.Forms.Label();
            this.LaunchTimeCountDownTicker = new System.Windows.Forms.Timer(this.components);
            this.AutoResultRefreshTimer = new System.Windows.Forms.Timer(this.components);
            this.RestaurantsListGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.VoteResultGroupBox.SuspendLayout();
            this.VoteResultPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // RestaurantsListGroupBox
            // 
            resources.ApplyResources(this.RestaurantsListGroupBox, "RestaurantsListGroupBox");
            this.RestaurantsListGroupBox.Controls.Add(this.panel1);
            this.RestaurantsListGroupBox.Controls.Add(this.VoteButton);
            this.RestaurantsListGroupBox.Name = "RestaurantsListGroupBox";
            this.RestaurantsListGroupBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.VoteOptionsListControl);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // VoteOptionsListControl
            // 
            resources.ApplyResources(this.VoteOptionsListControl, "VoteOptionsListControl");
            this.VoteOptionsListControl.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VoteOptionsListControl.DisplayMember = "Description";
            this.VoteOptionsListControl.FormattingEnabled = true;
            this.VoteOptionsListControl.Name = "VoteOptionsListControl";
            this.VoteOptionsListControl.ValueMember = "Name";
            // 
            // VoteButton
            // 
            resources.ApplyResources(this.VoteButton, "VoteButton");
            this.VoteButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.VoteButton.Name = "VoteButton";
            this.VoteButton.UseVisualStyleBackColor = true;
            this.VoteButton.Click += new System.EventHandler(this.OnVote);
            // 
            // VoteResultGroupBox
            // 
            resources.ApplyResources(this.VoteResultGroupBox, "VoteResultGroupBox");
            this.VoteResultGroupBox.Controls.Add(this.VoteResultPanel);
            this.VoteResultGroupBox.Name = "VoteResultGroupBox";
            this.VoteResultGroupBox.TabStop = false;
            // 
            // VoteResultPanel
            // 
            this.VoteResultPanel.Controls.Add(this.PollClosingAlertLabel);
            this.VoteResultPanel.Controls.Add(this.MostVotedLabel);
            this.VoteResultPanel.Controls.Add(this.MonthCalendar);
            this.VoteResultPanel.Controls.Add(this.MostVotedCadidateLabel);
            this.VoteResultPanel.Controls.Add(this.LaunchTimeCountDownLabel);
            resources.ApplyResources(this.VoteResultPanel, "VoteResultPanel");
            this.VoteResultPanel.Name = "VoteResultPanel";
            // 
            // PollClosingAlertLabel
            // 
            resources.ApplyResources(this.PollClosingAlertLabel, "PollClosingAlertLabel");
            this.PollClosingAlertLabel.ForeColor = System.Drawing.Color.Firebrick;
            this.PollClosingAlertLabel.Name = "PollClosingAlertLabel";
            // 
            // MostVotedLabel
            // 
            resources.ApplyResources(this.MostVotedLabel, "MostVotedLabel");
            this.MostVotedLabel.Name = "MostVotedLabel";
            // 
            // MonthCalendar
            // 
            resources.ApplyResources(this.MonthCalendar, "MonthCalendar");
            this.MonthCalendar.MaxDate = new System.DateTime(2014, 5, 24, 0, 0, 0, 0);
            this.MonthCalendar.MaxSelectionCount = 1;
            this.MonthCalendar.Name = "MonthCalendar";
            this.MonthCalendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.OnPollDateChanged);
            // 
            // MostVotedCadidateLabel
            // 
            resources.ApplyResources(this.MostVotedCadidateLabel, "MostVotedCadidateLabel");
            this.MostVotedCadidateLabel.AutoEllipsis = true;
            this.MostVotedCadidateLabel.ForeColor = System.Drawing.Color.CadetBlue;
            this.MostVotedCadidateLabel.Name = "MostVotedCadidateLabel";
            // 
            // LaunchTimeCountDownLabel
            // 
            resources.ApplyResources(this.LaunchTimeCountDownLabel, "LaunchTimeCountDownLabel");
            this.LaunchTimeCountDownLabel.ForeColor = System.Drawing.Color.Blue;
            this.LaunchTimeCountDownLabel.Name = "LaunchTimeCountDownLabel";
            // 
            // RefreshVoteResultButton
            // 
            resources.ApplyResources(this.RefreshVoteResultButton, "RefreshVoteResultButton");
            this.RefreshVoteResultButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RefreshVoteResultButton.Name = "RefreshVoteResultButton";
            this.RefreshVoteResultButton.UseVisualStyleBackColor = true;
            this.RefreshVoteResultButton.Click += new System.EventHandler(this.OnRefresh);
            // 
            // LoggedUserLabel
            // 
            this.LoggedUserLabel.AutoEllipsis = true;
            resources.ApplyResources(this.LoggedUserLabel, "LoggedUserLabel");
            this.LoggedUserLabel.Name = "LoggedUserLabel";
            // 
            // LoggedUserNameLabel
            // 
            resources.ApplyResources(this.LoggedUserNameLabel, "LoggedUserNameLabel");
            this.LoggedUserNameLabel.ForeColor = System.Drawing.Color.Blue;
            this.LoggedUserNameLabel.Name = "LoggedUserNameLabel";
            // 
            // LaunchTimeCountDownTicker
            // 
            this.LaunchTimeCountDownTicker.Interval = 1;
            this.LaunchTimeCountDownTicker.Tick += new System.EventHandler(this.OnLaunchTimeCountDownTick);
            // 
            // AutoResultRefreshTimer
            // 
            this.AutoResultRefreshTimer.Interval = 60000;
            this.AutoResultRefreshTimer.Tick += new System.EventHandler(this.OnAutoResultRefreshTimerTick);
            // 
            // MainVotingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.LoggedUserNameLabel);
            this.Controls.Add(this.RefreshVoteResultButton);
            this.Controls.Add(this.LoggedUserLabel);
            this.Controls.Add(this.VoteResultGroupBox);
            this.Controls.Add(this.RestaurantsListGroupBox);
            this.MaximizeBox = false;
            this.Name = "MainVotingForm";
            this.Title = "Votação de Restaurantes";
            this.Load += new System.EventHandler(this.MainVotingForm_Load);
            this.Controls.SetChildIndex(this.RestaurantsListGroupBox, 0);
            this.Controls.SetChildIndex(this.VoteResultGroupBox, 0);
            this.Controls.SetChildIndex(this.LoggedUserLabel, 0);
            this.Controls.SetChildIndex(this.RefreshVoteResultButton, 0);
            this.Controls.SetChildIndex(this.LoggedUserNameLabel, 0);
            this.RestaurantsListGroupBox.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.VoteResultGroupBox.ResumeLayout(false);
            this.VoteResultPanel.ResumeLayout(false);
            this.VoteResultPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.GroupBox RestaurantsListGroupBox;
        public System.Windows.Forms.Button VoteButton;
        public System.Windows.Forms.ListBox VoteOptionsListControl;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox VoteResultGroupBox;
        private System.Windows.Forms.Panel VoteResultPanel;
        private System.Windows.Forms.Label MostVotedLabel;
        public System.Windows.Forms.MonthCalendar MonthCalendar;
        public System.Windows.Forms.Label MostVotedCadidateLabel;
        public System.Windows.Forms.Button RefreshVoteResultButton;
        public System.Windows.Forms.Label PollClosingAlertLabel;
        public System.Windows.Forms.Label LaunchTimeCountDownLabel;
        private System.Windows.Forms.Label LoggedUserLabel;
        private System.Windows.Forms.Label LoggedUserNameLabel;
        private System.Windows.Forms.Timer LaunchTimeCountDownTicker;
        private System.Windows.Forms.Timer AutoResultRefreshTimer;

    }
}

