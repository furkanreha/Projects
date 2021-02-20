using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace CS408_PROJECT_STEP1_CLIENT
{
    public partial class Form1 : Form
    {
        bool terminating = false;
        bool connected = false;
        Socket clientSocket;
        string name;
        //Define loop invariants, Socket for Client and String for name of user connected
        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
            // Close the GUI 
        }
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
            // Initialize the form1

        }

        private void buttonConnect_Click(object sender, EventArgs e) // When ConnectButton Clicked
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            string IP = textBoxIp.Text;
            name = textBoxName.Text;
            int portNum;

            if (Int32.TryParse(textBoxPort.Text, out portNum)) // parse port to intger
            {
                try
                {
                    if (IP == "")
                    {
                        richTextBoxLogs.AppendText("IP cannot be empty!\n");
                    }
                    else
                    {
                        clientSocket.Connect(IP, portNum); // Connect to the Ip and Port Number

                        if (name != "" && name.Length <= 16384)
                        {
                            Byte[] buffer1 = new Byte[16384];
                            buffer1 = new Byte[16384];
                            buffer1 = Encoding.Default.GetBytes(name);
                            clientSocket.Send(buffer1); // Send the user name entered
                            Thread.Sleep(75);
                        }

                        Byte[] buffer2 = new Byte[16384];
                        clientSocket.Receive(buffer2); // Recieve the response of the server

                        string incomingMessage = Encoding.Default.GetString(buffer2);
                        incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                        richTextBoxLogs.AppendText(incomingMessage); // Print the server response in logs

                        if (incomingMessage.Contains("connected")) // If message includes connected, user is connected
                        {
                            buttonConnect.Enabled = false;
                            buttonDisconnect.Enabled = true;
                            textBoxIp.Enabled = false;
                            textBoxName.Enabled = false;
                            textBoxPort.Enabled = false;
                            richTextBoxMessage.Enabled = true;
                            buttonSend.Enabled = true;
                            buttonListAll.Enabled = true;
                            textBoxFriendName.Enabled = true;
                            buttonAddFriend.Enabled = true;
                            checkedListBoxRequests.Enabled = true;
                            buttonAccept.Enabled = true;
                            buttonReject.Enabled = true;
                            connected = true;
                            buttonRemoveFriend.Enabled = true;
                            buttonSend.Enabled = true;
                            buttonSendFriends.Enabled = true;
                            buttonClear.Enabled = true;
                            //fix buttons and loop invariants
                            Thread receiveThread = new Thread(Receive); // Start recieve thread to receive messages from other users
                            receiveThread.Start();
                        }
                    }
                }
                catch // Could not connected to the ip and port
                {
                    richTextBoxLogs.AppendText("Could not connect to the server!\n");
                }
            }
            else // Port number cannot be converted into integer
            {
                richTextBoxLogs.AppendText("Check the port spelling\n");
            }

        }

        private void buttonDisconnect_Click(object sender, EventArgs e) // When disconnect button clicked
        {
            connected = false;
            clientSocket.Close();
            // Connected will be false, and clientSocker for the user must be closed
        }

        private void buttonSend_Click(object sender, EventArgs e) // When Send clicked
        {
            string message = "typeMes:" + richTextBoxMessage.Text; // Get the message from user

            if (message.Length > 8 && message.Length <= 16384) // If not empty
            {
                Byte[] buffer = new Byte[16384];
                buffer = Encoding.Default.GetBytes(message);          
                richTextBoxLogs.AppendText("Message sent: " + message.Substring(8) + "\n"); // Print sended message to logs
                clientSocket.Send(buffer); // Send message to Server
                Thread.Sleep(75);
            }
            richTextBoxMessage.Clear();
        }

        private void Receive() // Receive messages across server 
        {
            while (connected)
            {
                try
                {
                    Byte[] buffer1 = new Byte[16384];
                    clientSocket.Receive(buffer1); // recive message from server

                    string incomingMessage = Encoding.Default.GetString(buffer1);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));

                    if (incomingMessage.Substring(0, 8) == "typeMes:") // normal message type
                    {
                        richTextBoxLogs.AppendText(incomingMessage.Substring(8) + "\n");
                    }
                    else if (incomingMessage.Substring(0, 8) == "typeAdR:") // add request to the friendship request list
                    {
                        checkedListBoxRequests.Items.Add(incomingMessage.Substring(8));
                    }
                    else if (incomingMessage.Substring(0, 8) == "typeAlR:") // chosen user names from the list will be rejected
                    {
                        List<string> result = incomingMessage.Substring(8).Split('@').ToList();
                        foreach (string dname in result)
                        {
                            checkedListBoxRequests.Items.Add(dname);
                        }
                    }
                    else if (incomingMessage.Substring(0, 8) == "typeAlP:") // chosen user names from the list will be accepted
                    {
                        List<string> result = incomingMessage.Substring(8).Split('@').ToList();
                        foreach (string response in result)
                        {
                            richTextBoxLogs.AppendText(response + "\n");
                        }
                    }
                    else if (incomingMessage.Substring(0, 8) == "typeAlF:")
                    {
                        List<string> result = incomingMessage.Substring(8).Split('@').ToList();
                        foreach (string dname in result)
                        {
                            checkedListBoxFriends.Items.Add(dname);
                        }
                    }
                    else if (incomingMessage.Substring(0, 8) == "typeAdF:")
                    {
                        checkedListBoxFriends.Items.Add(incomingMessage.Substring(8));
                    }

                    else if (incomingMessage.Substring(0, 8) == "typeFrR:")
                    {
                        checkedListBoxFriends.Items.Remove(incomingMessage.Substring(8));
                    }

                    // print the message
                }
                catch
                {
                    if (!terminating) // if not terminating the window
                    {
                        buttonConnect.Enabled = true;
                        richTextBoxMessage.Enabled = false;
                        buttonSend.Enabled = false;
                        textBoxIp.Enabled = true;
                        textBoxName.Enabled = true;
                        textBoxPort.Enabled = true;
                        buttonDisconnect.Enabled = false;
                        buttonListAll.Enabled = false;
                        textBoxFriendName.Enabled = false;
                        buttonAddFriend.Enabled = false;
                        checkedListBoxRequests.Items.Clear();
                        checkedListBoxFriends.Items.Clear();
                        checkedListBoxRequests.Enabled = false;
                        buttonReject.Enabled = false;
                        buttonAccept.Enabled = false;
                        buttonRemoveFriend.Enabled = false;
                        buttonSend.Enabled = false;
                        buttonSendFriends.Enabled = false;
                        buttonClear.Enabled = false;
                        // fix variables to initial state
                        if (connected) // if we connected and not terminating then the server must be disconnected
                        {
                            richTextBoxLogs.AppendText("The server has disconnected.\n");
                        }
                        else // User is disconnected
                        {
                            richTextBoxLogs.AppendText("Disconnected.\n");
                        }

                    }
                    connected = false; // In every case, we are now disconnected
                    clientSocket.Close(); // Therefore close the socket for User
                }
            }
        }

        private void buttonListAll_Click(object sender, EventArgs e)
        {
            string message = "typeAll:"; // Get the message from user

            Byte[] buffer = new Byte[16384];
            buffer = Encoding.Default.GetBytes(message);     
            richTextBoxLogs.AppendText("All User Names Requested! " + "\n"); // Print sended message to logs
            clientSocket.Send(buffer); // Send message to Server
            Thread.Sleep(75);

        }

        private void buttonAddFriend_Click(object sender, EventArgs e)
        {
            string message = "typeReq:" + textBoxFriendName.Text; // Get the message from user

            if (message.Length > 8 && message.Length <= 16384) // If not empty
            {
                Byte[] buffer = new Byte[16384];
                buffer = Encoding.Default.GetBytes(message);
                clientSocket.Send(buffer); // Send message to Server

                if (checkedListBoxRequests.Items.Contains(message.Substring(8)))
                {
                    richTextBoxLogs.AppendText("You have accepted friendship request from " + message.Substring(8) + ".\n");
                    checkedListBoxRequests.Items.Remove(message.Substring(8));
                    checkedListBoxFriends.Items.Add(message.Substring(8));
                }
                Thread.Sleep(75);
            }
            textBoxFriendName.Clear();
        }

        private void buttonReject_Click(object sender, EventArgs e) // reject the friend requests of selected ones
        {
            string to_be_sent = "typeRjR:";
            List<object> iterator = new List<object>();
            foreach (object x in checkedListBoxRequests.CheckedItems)
            {
                to_be_sent += x.ToString() + "@";
                richTextBoxLogs.AppendText("You have rejected friendship request from " + x.ToString() + ".\n");
                iterator.Add(x);
            }

            foreach (object x in iterator)
            {
                checkedListBoxRequests.Items.Remove(x);
            }

            if (to_be_sent.Length > 8 && to_be_sent.Length <= 16384)
            {
                Byte[] buffer = new Byte[16384];
                buffer = Encoding.Default.GetBytes(to_be_sent.Substring(0, to_be_sent.Length - 1));
                clientSocket.Send(buffer);
                Thread.Sleep(75);
            }
        }

        private void buttonAccept_Click(object sender, EventArgs e) // accept the friend request of selected ones
        {
            string to_be_sent = "typeAcR:";
            List<object> iterator = new List<object>();
            foreach (object x in checkedListBoxRequests.CheckedItems)
            {
                to_be_sent += x.ToString() + "@";
                richTextBoxLogs.AppendText("You have accepted friendship request from " + x.ToString() + ".\n");
                iterator.Add(x);
            }
            foreach (object x in iterator)
            {
                checkedListBoxRequests.Items.Remove(x);
            }

            if (to_be_sent.Length > 8 && to_be_sent.Length <= 16384)
            {
                Byte[] buffer = new Byte[16384];
                buffer = Encoding.Default.GetBytes(to_be_sent.Substring(0, to_be_sent.Length - 1));
                clientSocket.Send(buffer);
                Thread.Sleep(75);
            }
        }

        private void buttonRemoveFriend_Click(object sender, EventArgs e)
        {
            string to_be_sent = "typeFrR:";
            List<object> iterator = new List<object>();
            foreach (object x in checkedListBoxFriends.CheckedItems)
            {
                to_be_sent += x.ToString() + "@";
                richTextBoxLogs.AppendText("You have removed " + x.ToString() + " from your friends!\n");
                iterator.Add(x);
            }
            foreach (object x in iterator)
            {
                checkedListBoxFriends.Items.Remove(x);
            }

            if (to_be_sent.Length > 8 && to_be_sent.Length <= 16384)
            {
                Byte[] buffer = new Byte[16384];
                buffer = Encoding.Default.GetBytes(to_be_sent.Substring(0, to_be_sent.Length - 1));
                clientSocket.Send(buffer);
                Thread.Sleep(75);
            }
        }

        private void buttonSendFriends_Click(object sender, EventArgs e)
        {
            string message = "typeFrM:" + richTextBoxMessage.Text; // Get the message from user

            if (message.Length > 8 && message.Length <= 16384) // If not empty
            {
                Byte[] buffer = new Byte[16384];
                buffer = Encoding.Default.GetBytes(message);
                richTextBoxLogs.AppendText("Message sent to friends: " + message.Substring(8) + "\n"); // Print sended message to logs
                clientSocket.Send(buffer); // Send message to Server
                Thread.Sleep(75);
            }
            richTextBoxMessage.Clear();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxLogs.Clear();
        }
    }
}
