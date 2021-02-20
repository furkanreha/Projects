namespace CS408_PROJECT_STEP1_CLIENT
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIp = new System.Windows.Forms.TextBox();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisconnect = new System.Windows.Forms.Button();
            this.richTextBoxLogs = new System.Windows.Forms.RichTextBox();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.buttonSend = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonListAll = new System.Windows.Forms.Button();
            this.textBoxFriendName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonAddFriend = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.checkedListBoxRequests = new System.Windows.Forms.CheckedListBox();
            this.buttonReject = new System.Windows.Forms.Button();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.checkedListBoxFriends = new System.Windows.Forms.CheckedListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonRemoveFriend = new System.Windows.Forms.Button();
            this.buttonSendFriends = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "PORT:";
            // 
            // textBoxIp
            // 
            this.textBoxIp.Location = new System.Drawing.Point(76, 56);
            this.textBoxIp.Name = "textBoxIp";
            this.textBoxIp.Size = new System.Drawing.Size(283, 22);
            this.textBoxIp.TabIndex = 2;
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(76, 94);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(283, 22);
            this.textBoxPort.TabIndex = 3;
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(389, 15);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(95, 23);
            this.buttonConnect.TabIndex = 4;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisconnect
            // 
            this.buttonDisconnect.Enabled = false;
            this.buttonDisconnect.Location = new System.Drawing.Point(389, 94);
            this.buttonDisconnect.Name = "buttonDisconnect";
            this.buttonDisconnect.Size = new System.Drawing.Size(95, 23);
            this.buttonDisconnect.TabIndex = 5;
            this.buttonDisconnect.Text = "Disconnect";
            this.buttonDisconnect.UseVisualStyleBackColor = true;
            this.buttonDisconnect.Click += new System.EventHandler(this.buttonDisconnect_Click);
            // 
            // richTextBoxLogs
            // 
            this.richTextBoxLogs.Location = new System.Drawing.Point(23, 135);
            this.richTextBoxLogs.Name = "richTextBoxLogs";
            this.richTextBoxLogs.ReadOnly = true;
            this.richTextBoxLogs.Size = new System.Drawing.Size(461, 509);
            this.richTextBoxLogs.TabIndex = 6;
            this.richTextBoxLogs.Text = "";
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Enabled = false;
            this.richTextBoxMessage.Location = new System.Drawing.Point(498, 29);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(461, 107);
            this.richTextBoxMessage.TabIndex = 7;
            this.richTextBoxMessage.Text = "";
            // 
            // buttonSend
            // 
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(589, 142);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(84, 30);
            this.buttonSend.TabIndex = 8;
            this.buttonSend.Text = "Send All";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(20, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "NAME:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(76, 16);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(283, 22);
            this.textBoxName.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(678, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "Message Box:";
            // 
            // buttonListAll
            // 
            this.buttonListAll.Enabled = false;
            this.buttonListAll.Location = new System.Drawing.Point(76, 650);
            this.buttonListAll.Name = "buttonListAll";
            this.buttonListAll.Size = new System.Drawing.Size(146, 29);
            this.buttonListAll.TabIndex = 12;
            this.buttonListAll.Text = "List All Users";
            this.buttonListAll.UseVisualStyleBackColor = true;
            this.buttonListAll.Click += new System.EventHandler(this.buttonListAll_Click);
            // 
            // textBoxFriendName
            // 
            this.textBoxFriendName.Enabled = false;
            this.textBoxFriendName.Location = new System.Drawing.Point(498, 201);
            this.textBoxFriendName.Name = "textBoxFriendName";
            this.textBoxFriendName.Size = new System.Drawing.Size(461, 22);
            this.textBoxFriendName.TabIndex = 13;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(683, 181);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 17);
            this.label5.TabIndex = 14;
            this.label5.Text = "User Name:";
            // 
            // buttonAddFriend
            // 
            this.buttonAddFriend.Enabled = false;
            this.buttonAddFriend.Location = new System.Drawing.Point(681, 229);
            this.buttonAddFriend.Name = "buttonAddFriend";
            this.buttonAddFriend.Size = new System.Drawing.Size(93, 30);
            this.buttonAddFriend.TabIndex = 15;
            this.buttonAddFriend.Text = "Add Friend";
            this.buttonAddFriend.UseVisualStyleBackColor = true;
            this.buttonAddFriend.Click += new System.EventHandler(this.buttonAddFriend_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(652, 490);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(142, 17);
            this.label6.TabIndex = 16;
            this.label6.Text = "Friendship Requests:";
            // 
            // checkedListBoxRequests
            // 
            this.checkedListBoxRequests.Enabled = false;
            this.checkedListBoxRequests.FormattingEnabled = true;
            this.checkedListBoxRequests.Location = new System.Drawing.Point(498, 510);
            this.checkedListBoxRequests.Name = "checkedListBoxRequests";
            this.checkedListBoxRequests.Size = new System.Drawing.Size(461, 157);
            this.checkedListBoxRequests.TabIndex = 17;
            // 
            // buttonReject
            // 
            this.buttonReject.Enabled = false;
            this.buttonReject.Location = new System.Drawing.Point(570, 673);
            this.buttonReject.Name = "buttonReject";
            this.buttonReject.Size = new System.Drawing.Size(75, 29);
            this.buttonReject.TabIndex = 18;
            this.buttonReject.Text = "Reject";
            this.buttonReject.UseVisualStyleBackColor = true;
            this.buttonReject.Click += new System.EventHandler(this.buttonReject_Click);
            // 
            // buttonAccept
            // 
            this.buttonAccept.Enabled = false;
            this.buttonAccept.Location = new System.Drawing.Point(791, 673);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 29);
            this.buttonAccept.TabIndex = 19;
            this.buttonAccept.Text = "Accept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // checkedListBoxFriends
            // 
            this.checkedListBoxFriends.FormattingEnabled = true;
            this.checkedListBoxFriends.Location = new System.Drawing.Point(498, 284);
            this.checkedListBoxFriends.Name = "checkedListBoxFriends";
            this.checkedListBoxFriends.Size = new System.Drawing.Size(461, 157);
            this.checkedListBoxFriends.TabIndex = 21;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(681, 264);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 17);
            this.label7.TabIndex = 22;
            this.label7.Text = "Your Friends:";
            // 
            // buttonRemoveFriend
            // 
            this.buttonRemoveFriend.Enabled = false;
            this.buttonRemoveFriend.Location = new System.Drawing.Point(655, 447);
            this.buttonRemoveFriend.Name = "buttonRemoveFriend";
            this.buttonRemoveFriend.Size = new System.Drawing.Size(139, 27);
            this.buttonRemoveFriend.TabIndex = 23;
            this.buttonRemoveFriend.Text = "Remove Friend";
            this.buttonRemoveFriend.UseVisualStyleBackColor = true;
            this.buttonRemoveFriend.Click += new System.EventHandler(this.buttonRemoveFriend_Click);
            // 
            // buttonSendFriends
            // 
            this.buttonSendFriends.Enabled = false;
            this.buttonSendFriends.Location = new System.Drawing.Point(768, 142);
            this.buttonSendFriends.Name = "buttonSendFriends";
            this.buttonSendFriends.Size = new System.Drawing.Size(108, 30);
            this.buttonSendFriends.TabIndex = 24;
            this.buttonSendFriends.Text = "Send Friends";
            this.buttonSendFriends.UseVisualStyleBackColor = true;
            this.buttonSendFriends.Click += new System.EventHandler(this.buttonSendFriends_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Enabled = false;
            this.buttonClear.Location = new System.Drawing.Point(312, 650);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 29);
            this.buttonClear.TabIndex = 25;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(971, 714);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSendFriends);
            this.Controls.Add(this.buttonRemoveFriend);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.checkedListBoxFriends);
            this.Controls.Add(this.buttonAccept);
            this.Controls.Add(this.buttonReject);
            this.Controls.Add(this.checkedListBoxRequests);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.buttonAddFriend);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBoxFriendName);
            this.Controls.Add(this.buttonListAll);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSend);
            this.Controls.Add(this.richTextBoxMessage);
            this.Controls.Add(this.richTextBoxLogs);
            this.Controls.Add(this.buttonDisconnect);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.textBoxPort);
            this.Controls.Add(this.textBoxIp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIp;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisconnect;
        private System.Windows.Forms.RichTextBox richTextBoxLogs;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonListAll;
        private System.Windows.Forms.TextBox textBoxFriendName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonAddFriend;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckedListBox checkedListBoxRequests;
        private System.Windows.Forms.Button buttonReject;
        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.CheckedListBox checkedListBoxFriends;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonRemoveFriend;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Button buttonSendFriends;
        private System.Windows.Forms.Button buttonClear;
    }
}

