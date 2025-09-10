namespace CefSharp.MinimalExample.WinForms
{
    partial class BrowserForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowserForm));
            toolStripContainer = new System.Windows.Forms.ToolStripContainer();
            statusLabel = new System.Windows.Forms.Label();
            outputLabel = new System.Windows.Forms.Label();
            toolStrip1 = new System.Windows.Forms.ToolStrip();
            backButton = new System.Windows.Forms.ToolStripButton();
            forwardButton = new System.Windows.Forms.ToolStripButton();
            urlTextBox = new System.Windows.Forms.ToolStripTextBox();
            goButton = new System.Windows.Forms.ToolStripButton();
            menuStrip1 = new System.Windows.Forms.MenuStrip();
            fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            showDevToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripContainer.ContentPanel.SuspendLayout();
            toolStripContainer.TopToolStripPanel.SuspendLayout();
            toolStripContainer.SuspendLayout();
            toolStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // toolStripContainer
            // 
            // 
            // toolStripContainer.ContentPanel
            // 
            toolStripContainer.ContentPanel.Controls.Add(statusLabel);
            toolStripContainer.ContentPanel.Controls.Add(outputLabel);
            toolStripContainer.ContentPanel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            toolStripContainer.ContentPanel.Size = new System.Drawing.Size(973, 754);
            toolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            toolStripContainer.LeftToolStripPanelVisible = false;
            toolStripContainer.Location = new System.Drawing.Point(0, 0);
            toolStripContainer.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            toolStripContainer.Name = "toolStripContainer";
            toolStripContainer.RightToolStripPanelVisible = false;
            toolStripContainer.Size = new System.Drawing.Size(973, 754);
            toolStripContainer.TabIndex = 0;
            toolStripContainer.Text = "toolStripContainer1";
            // 
            // toolStripContainer.TopToolStripPanel
            // 
            toolStripContainer.TopToolStripPanel.Controls.Add(toolStrip1);
            // 
            // statusLabel
            // 
            statusLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            statusLabel.Location = new System.Drawing.Point(0, 714);
            statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new System.Drawing.Size(973, 20);
            statusLabel.TabIndex = 1;
            statusLabel.Visible = false;
            // 
            // outputLabel
            // 
            outputLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            outputLabel.Location = new System.Drawing.Point(0, 734);
            outputLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            outputLabel.Name = "outputLabel";
            outputLabel.Size = new System.Drawing.Size(973, 20);
            outputLabel.TabIndex = 0;
            outputLabel.Visible = false;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { backButton, forwardButton, urlTextBox, goButton });
            toolStrip1.Location = new System.Drawing.Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            toolStrip1.Size = new System.Drawing.Size(707, 27);
            toolStrip1.Stretch = true;
            toolStrip1.TabIndex = 0;
            toolStrip1.Visible = false;
            toolStrip1.Layout += HandleToolStripLayout;
            // 
            // backButton
            // 
            backButton.Enabled = false;
            backButton.Image = Properties.Resources.nav_left_green;
            backButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            backButton.Name = "backButton";
            backButton.Size = new System.Drawing.Size(64, 24);
            backButton.Text = "Back";
            backButton.Click += BackButtonClick;
            // 
            // forwardButton
            // 
            forwardButton.Enabled = false;
            forwardButton.Image = Properties.Resources.nav_right_green;
            forwardButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            forwardButton.Name = "forwardButton";
            forwardButton.Size = new System.Drawing.Size(87, 24);
            forwardButton.Text = "Forward";
            forwardButton.Click += ForwardButtonClick;
            // 
            // urlTextBox
            // 
            urlTextBox.AutoSize = false;
            urlTextBox.Name = "urlTextBox";
            urlTextBox.Size = new System.Drawing.Size(500, 25);
            urlTextBox.KeyUp += UrlTextBoxKeyUp;
            // 
            // goButton
            // 
            goButton.Image = Properties.Resources.nav_plain_green;
            goButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            goButton.Name = "goButton";
            goButton.Size = new System.Drawing.Size(52, 24);
            goButton.Text = "Go";
            goButton.Click += GoButtonClick;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new System.Drawing.Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new System.Windows.Forms.Padding(8, 3, 0, 3);
            menuStrip1.Size = new System.Drawing.Size(973, 30);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { showDevToolsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // showDevToolsToolStripMenuItem
            // 
            showDevToolsToolStripMenuItem.Name = "showDevToolsToolStripMenuItem";
            showDevToolsToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            showDevToolsToolStripMenuItem.Text = "Show DevTools";
            showDevToolsToolStripMenuItem.Click += ShowDevToolsMenuItemClick;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new System.Drawing.Size(193, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += ExitMenuItemClick;
            // 
            // BrowserForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(973, 754);
            Controls.Add(toolStripContainer);
            Controls.Add(menuStrip1);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            Name = "BrowserForm";
            Text = "BrowserForm";
            Load += BrowserForm_Load;
            toolStripContainer.ContentPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.ResumeLayout(false);
            toolStripContainer.TopToolStripPanel.PerformLayout();
            toolStripContainer.ResumeLayout(false);
            toolStripContainer.PerformLayout();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton backButton;
        private System.Windows.Forms.ToolStripButton forwardButton;
        private System.Windows.Forms.ToolStripTextBox urlTextBox;
        private System.Windows.Forms.ToolStripButton goButton;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Label statusLabel;
		private System.Windows.Forms.ToolStripMenuItem showDevToolsToolStripMenuItem;

    }
}