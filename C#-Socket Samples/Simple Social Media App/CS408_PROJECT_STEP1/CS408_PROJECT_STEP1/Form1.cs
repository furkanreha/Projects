using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CS408_PROJECT_STEP1
{
    public partial class Form1 : Form
    {
        Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        List<string> databaseNames = new List<string>();
        IDictionary<string, Socket> onlineClients = new Dictionary<string, Socket>();
        IDictionary<string, List<string>> requestList = new Dictionary<string, List<string>>();
        IDictionary<string, List<string>> responseList = new Dictionary<string, List<string>>();
        IDictionary<string, List<string>> friendList = new Dictionary<string, List<string>>();
        IDictionary<string, List<string>> friendMessages = new Dictionary<string, List<string>>();


        // Define Socket for Server, List for Names in the Database and List of Sockets for Online Users

        bool terminating = false;
        bool listening = false;
        //initialize loop variants

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listening = false;
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

        private void buttonListen_Click(object sender, EventArgs e) // When ListenButton Clicked
        {
            string path = Directory.GetCurrentDirectory(); // Get current directory path
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(path + "\\user_db.txt"); // Read names for validation
            while ((line = file.ReadLine()) != null) // Until the end of file
            {
                databaseNames.Add(line); // add names to the list 
            }

            int serverPort;
            if (Int32.TryParse(textBoxPort.Text, out serverPort)) // parse port to intger
            {
                IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, serverPort);
                serverSocket.Bind(endPoint); // Bind to the Ip adress
                serverSocket.Listen(Int32.MaxValue); // Start to listen

                listening = true;
                terminating = false;
                buttonListen.Enabled = false;
                buttonClear.Enabled = true;
                textBoxPort.ReadOnly = true;
                //Fix the buttons and loop variant values
                foreach (string dname in databaseNames)
                {
                    requestList.Add(dname, new List<string>());
                    responseList.Add(dname, new List<string>());
                    friendList.Add(dname, new List<string>());
                    friendMessages.Add(dname, new List<string>());
                }
                Thread acceptThread = new Thread(Accept);
                acceptThread.Start(); // Create and start accept thread to connect users from now on

                richTextBoxLogs.AppendText("Started listening on port: " + serverPort + "\n");
            }
            else // Port Number has some spelling error
            {
                richTextBoxLogs.AppendText("Please check port number spelling!!\n");
            }
        }

        private void Accept() // Decleration of Accept Thread
        {
            while (listening) // While listening on spesific port and Ip adress
            {
                try
                {
                    Socket newClient = serverSocket.Accept(); // Accept the new user
                    Byte[] buffer = new Byte[16384];
                    newClient.Receive(buffer); // Recieve message from User

                    string name = Encoding.Default.GetString(buffer); // Get the name of user
                    name = name.Substring(0, name.IndexOf("\0"));
                    String message = name;

                    if (onlineClients.ContainsKey(name)) // If name is in the online list. Don't Accept
                    {
                        Byte[] buffer1 = new Byte[16384];
                        message += " is already online.";
                        richTextBoxLogs.AppendText(message + "\n");
                        buffer1 = Encoding.Default.GetBytes(message + " Try another name!\n");
                        newClient.Send(buffer1);
                        Thread.Sleep(75);
                    }
                    else if (databaseNames.Contains(name)) // If else name is in the database. Accept
                    {
                        message += " connected to the server.";
                        Byte[] buffer2 = new Byte[16384];
                        richTextBoxLogs.AppendText(message + "\n");
                        buffer2 = Encoding.Default.GetBytes(message + " Welcome to Social Media!\n");
                        newClient.Send(buffer2);
                        Thread.Sleep(75);
                        onlineClients.Add(name, newClient);


                        Thread recieveThread = new Thread(recieve); // Starts direct message thread to forward messages from user
                        recieveThread.Start(name); // give name to the thread to know the name of user 

                    }
                    else // else means the name not in the database. Reject 
                    {
                        Byte[] buffer3 = new Byte[16384];
                        message += " not in the database!";
                        richTextBoxLogs.AppendText(message + " Request rejected!\n");
                        buffer3 = Encoding.Default.GetBytes(message + " Try valid name!\n");
                        newClient.Send(buffer3);
                        Thread.Sleep(75);
                    }

                }

                catch
                {
                    if (terminating) // If server terminating means also server is not listening
                    {
                        listening = false;
                    }
                    else // The reason of disconnection is not server. Something happens to socket
                    {
                        richTextBoxLogs.AppendText("The socket stopped working.\n");
                    }
                }
            }
        }
        private void recieve(object name)
        {
            string client_name = name.ToString();
            Socket thisClient = onlineClients[client_name];
            bool connected = true;
            Byte[] buffer = new Byte[16384];

            string allRequests = "typeAlR:";
            foreach (string dname in requestList[client_name])
            {
                allRequests += dname + "@";
            }

            requestList[client_name].Clear();

            if (allRequests.Length > 8)
            {
                buffer = Encoding.Default.GetBytes(allRequests.Substring(0, allRequests.Length - 1));
                thisClient.Send(buffer);
                Thread.Sleep(75);
            }

            string allFriendsMessages = "typeAlP:";
            foreach (string message in friendMessages[client_name])
            {
                allFriendsMessages += message + "@";
            }
            friendMessages[client_name].Clear();
            if (allFriendsMessages.Length > 8)
            {
                buffer = Encoding.Default.GetBytes(allFriendsMessages.Substring(0, allFriendsMessages.Length - 1));
                thisClient.Send(buffer);
                Thread.Sleep(75);
            }

            string allResponses = "typeAlP:";
            foreach (string dname in responseList[client_name])
            {
                allResponses += dname + "@";
            }
            responseList[client_name].Clear();
            if (allResponses.Length > 8)
            {
                buffer = Encoding.Default.GetBytes(allResponses.Substring(0, allResponses.Length - 1));
                thisClient.Send(buffer);
                Thread.Sleep(75);
            }

            string allFriendsToSend = "typeAlF:";
            foreach (string dname in friendList[client_name])
            {
                allFriendsToSend += dname + "@";
            }

            if (allFriendsToSend.Length > 8)
            {
                buffer = Encoding.Default.GetBytes(allFriendsToSend.Substring(0, allFriendsToSend.Length - 1));
                thisClient.Send(buffer);
                Thread.Sleep(75);
            }

            

            while (connected && !terminating) // While user connected and server working
            {
                try
                {
                    buffer = new Byte[16384];
                    thisClient.Receive(buffer); // Recieve message from user

                    string incomingMessage = Encoding.Default.GetString(buffer);
                    incomingMessage = incomingMessage.Substring(0, incomingMessage.IndexOf("\0"));
                    string actualMessage = incomingMessage.Substring(8);

                    if (incomingMessage.Length > 8 && incomingMessage.Length <= 16384 && incomingMessage.Substring(0, 8) == "typeMes:")
                    {
                        buffer = Encoding.Default.GetBytes("typeMes:" + client_name + "'s message: " + actualMessage);
                        richTextBoxLogs.AppendText(client_name + " 's message forwarded: " + actualMessage + " " + "\n");
                        foreach (String key in onlineClients.Keys) // forward message to every online user
                        {

                            if (key != client_name) // If the user name same as the sending user name we don't send the message
                            {
                                try
                                {
                                    onlineClients[key].Send(buffer);
                                    Thread.Sleep(75);

                                }
                                catch // Some connection error while forwarding the message to the other users
                                {
                                    richTextBoxLogs.AppendText("There is a problem! Check the connection...\n");
                                    terminating = true;
                                    buttonListen.Enabled = true;
                                    textBoxPort.Enabled = true;
                                    buttonClear.Enabled = false;
                                    serverSocket.Close();
                                    break;
                                    // handle the loop variants, close server Socket and fix buttons.
                                }

                            }
                        }
                    }
                    else if (incomingMessage == "typeAll:") // All user names requested
                    {
                        string allNames = "typeMes:All users in the database: ";
                        foreach (string dname in databaseNames)
                        {
                            allNames += "\n" + dname;
                        }
                        try
                        {
                            buffer = Encoding.Default.GetBytes(allNames);
                            thisClient.Send(buffer);
                            Thread.Sleep(75);
                            richTextBoxLogs.AppendText(client_name + " requests all users in the database.\n");
                        }
                        catch
                        {
                            richTextBoxLogs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            buttonListen.Enabled = true;
                            textBoxPort.Enabled = true;
                            buttonClear.Enabled = false;
                            serverSocket.Close();
                            break;
                        }
                    }
                    else if (incomingMessage.Substring(0, 8) == "typeReq:") // corresponding user tries to send a friendship request
                    {
                        try
                        {
                            buffer = new Byte[16384];
                            if (databaseNames.Contains(actualMessage))
                            {
                                if (client_name != actualMessage)
                                {
                                    if (requestList[actualMessage].Contains(client_name)) // The request is already sent
                                    {
                                        richTextBoxLogs.AppendText("Friendship request of " + client_name + " to " + actualMessage + " is not valid!\n");
                                        buffer = Encoding.Default.GetBytes("typeMes:You have already sent friendship request to " + actualMessage + ".");
                                        thisClient.Send(buffer);
                                        Thread.Sleep(75);
                                    }
                                    else
                                    {
                                        if (friendList[client_name].Contains(actualMessage)) // They are already friends
                                        {
                                            richTextBoxLogs.AppendText("Friendship request of " + client_name + " to " + actualMessage + " is not valid!\n");
                                            buffer = Encoding.Default.GetBytes("typeMes:You are already friend with " + actualMessage + ".");
                                            thisClient.Send(buffer);
                                            Thread.Sleep(75);
                                        }
                                        else
                                        {
                                            if (requestList[client_name].Contains(actualMessage)) // Same case as accepting friend request from the same user, since there is already a friend request from that user
                                            {
                                                richTextBoxLogs.AppendText("Friendship request of " + actualMessage + " is accepted by " + client_name + "!\n");
                                                string response = "Your friendship request is accepted by " + client_name + "!";
                                                friendList[actualMessage].Add(client_name);
                                                friendList[client_name].Add(actualMessage);
                                                requestList[client_name].Remove(actualMessage);
                                                if (onlineClients.Keys.Contains(actualMessage))
                                                {
                                                    buffer = Encoding.Default.GetBytes("typeMes:" + response);
                                                    onlineClients[actualMessage].Send(buffer);
                                                    Thread.Sleep(75);
                                                }
                                                else
                                                {
                                                    responseList[actualMessage].Add(response);
                                                }
                                            }
                                            else // Actual friend request happens here
                                            {
                                                richTextBoxLogs.AppendText(client_name + " sends friendship request to " + actualMessage + ".\n");
                                                requestList[actualMessage].Add(client_name);
                                                buffer = Encoding.Default.GetBytes("typeMes:You have succesfully sent friendship request to " + actualMessage + ".");
                                                thisClient.Send(buffer);
                                                Thread.Sleep(75);
                                                if (onlineClients.Keys.Contains(actualMessage))
                                                {
                                                    buffer = Encoding.Default.GetBytes("typeAdR:" + client_name);
                                                    onlineClients[actualMessage].Send(buffer);
                                                    Thread.Sleep(75);
                                                }
                                            }
                                        }
                                    }
                                }
                                else // cannot send request to itself
                                {
                                    richTextBoxLogs.AppendText("Friendship request of " + client_name + " to " + actualMessage + " is is not valid!\n");
                                    buffer = Encoding.Default.GetBytes("typeMes:You cannot send friendship request to yourself!");
                                    thisClient.Send(buffer);
                                    Thread.Sleep(75);
                                }
                            }
                            else // requested user name not in the database
                            {
                                richTextBoxLogs.AppendText("Friendship request of " + client_name + " to " + actualMessage + " is not valid!\n");
                                buffer = Encoding.Default.GetBytes("typeMes:Your friendship request is not valid because " + actualMessage + " not in the database!");
                                thisClient.Send(buffer);
                                Thread.Sleep(75);
                            }
                        }

                        catch
                        {
                            richTextBoxLogs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            buttonListen.Enabled = true;
                            textBoxPort.Enabled = true;
                            buttonClear.Enabled = false;
                            serverSocket.Close();
                            break;
                        }

                    }
                    else if (incomingMessage.Length > 8 && incomingMessage.Length <= 16384 && incomingMessage.Substring(0, 8) == "typeRjR:") // Rejecting some requests in the request list
                    {
                        try
                        {
                            List<string> result = actualMessage.Split('@').ToList(); // rejected list by username
                            foreach (string dname in result) // for each reject the request and send corresponding responses
                            {
                                requestList[client_name].Remove(dname);
                                richTextBoxLogs.AppendText("Friendship request of " + dname + " is rejected by " + client_name + "!\n");
                                string response = "Your friendship request is rejected by " + client_name + "!";
                                if (onlineClients.Keys.Contains(dname))
                                {
                                    buffer = Encoding.Default.GetBytes("typeMes:" + response);
                                    onlineClients[dname].Send(buffer);
                                    Thread.Sleep(75);
                                }
                                else
                                {
                                    responseList[dname].Add(response);
                                }
                            }
                        }
                        catch
                        {
                            richTextBoxLogs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            buttonListen.Enabled = true;
                            textBoxPort.Enabled = true;
                            buttonClear.Enabled = false;
                            serverSocket.Close();
                            break;
                        }

                    }
                    else if (incomingMessage.Length > 8 && incomingMessage.Length <= 16384 && incomingMessage.Substring(0, 8) == "typeAcR:") // user accepting some requests
                    {
                        try
                        {
                            IDictionary<string, string> onlineMessages = new Dictionary<string, string>();
                            List<string> result = actualMessage.Split('@').ToList(); // get the list of names accepted
                            foreach (string dname in result) // for each accepted requests send the corresponding responses
                            {
                                requestList[client_name].Remove(dname);
                                richTextBoxLogs.AppendText("Friendship request of " + dname + " is accepted by " + client_name + "!\n");
                                string response = "Your friendship request is accepted by " + client_name + "!";
                                friendList[dname].Add(client_name);
                                friendList[client_name].Add(dname);

                                if (onlineClients.Keys.Contains(client_name))
                                {
                                    buffer = Encoding.Default.GetBytes("typeAdF:" + dname);
                                    onlineClients[client_name].Send(buffer);
                                    Thread.Sleep(75);
                                }

                                if (onlineClients.Keys.Contains(dname))
                                {
                                    onlineMessages.Add(dname, response);
                                    
                                    buffer = Encoding.Default.GetBytes("typeAdF:" + client_name);
                                    onlineClients[dname].Send(buffer);
                                    Thread.Sleep(75);
                                }
                                else
                                {
                                    responseList[dname].Add(response);
                                }
                            }
                            foreach (string dname in onlineMessages.Keys)
                            {
                                buffer = Encoding.Default.GetBytes("typeMes:" + onlineMessages[dname]);
                                onlineClients[dname].Send(buffer);
                                Thread.Sleep(75);
                            }
                        }
                        catch
                        {
                            richTextBoxLogs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            buttonListen.Enabled = true;
                            textBoxPort.Enabled = true;
                            buttonClear.Enabled = false;
                            serverSocket.Close();
                            break;
                        }
                    }
                    else if (incomingMessage.Length > 8 && incomingMessage.Length <= 16384 && incomingMessage.Substring(0, 8) == "typeFrR:")
                    {
                        try
                        {
                            IDictionary<string, string> onlineMessages = new Dictionary<string, string>();
                            List<string> result = actualMessage.Split('@').ToList(); // get the list of names accepted
                            foreach (string dname in result) // for each accepted requests send the corresponding responses
                            {
                                richTextBoxLogs.AppendText("Friendship between " + client_name + " and " + dname + " is removed!\n");
                                string response = client_name + " have removed you from his/her friendship list!";
                                friendList[dname].Remove(client_name);
                                friendList[client_name].Remove(dname);

                                if (onlineClients.Keys.Contains(dname))
                                {
                                    onlineMessages.Add(dname, response);
                                    buffer = Encoding.Default.GetBytes("typeFrR:" + client_name);
                                    onlineClients[dname].Send(buffer);
                                    Thread.Sleep(75);
                                }
                                else
                                {
                                    responseList[dname].Add(response);
                                }
                            }
                            foreach (string dname in onlineMessages.Keys)
                            {
                                buffer = Encoding.Default.GetBytes("typeMes:" + onlineMessages[dname]);
                                onlineClients[dname].Send(buffer);
                                Thread.Sleep(75);
                            }
                        }
                        catch
                        {
                            richTextBoxLogs.AppendText("There is a problem! Check the connection...\n");
                            terminating = true;
                            buttonListen.Enabled = true;
                            textBoxPort.Enabled = true;
                            buttonClear.Enabled = false;
                            serverSocket.Close();
                            break;
                        }
                    }
                    else if (incomingMessage.Length > 8 && incomingMessage.Length <= 16384 && incomingMessage.Substring(0, 8) == "typeFrM:")
                    {
                        buffer = Encoding.Default.GetBytes("typeMes:" + "Your friend " + client_name + "'s message: " + actualMessage);
                        richTextBoxLogs.AppendText(client_name + " 's message forwarded to his/her friends: " + actualMessage + " " + "\n");
                        foreach (String key in friendList[client_name]) // forward message to every online user
                        {
                            try
                            {
                                if (onlineClients.Keys.Contains(key))
                                {
                                    onlineClients[key].Send(buffer);
                                    Thread.Sleep(75);
                                }
                                else
                                {
                                    friendMessages[key].Add("Your friend " + client_name + " 's message forwarded: " + actualMessage);
                                }

                            }
                            catch // Some connection error while forwarding the message to the other users
                            {
                                richTextBoxLogs.AppendText("There is a problem! Check the connection...\n");
                                terminating = true;
                                buttonListen.Enabled = true;
                                textBoxPort.Enabled = true;
                                buttonClear.Enabled = false;
                                serverSocket.Close();
                                break;
                                // handle the loop variants, close server Socket and fix buttons.
                            }
                        }
                    }
                }


                catch
                {
                    if (!terminating) // If Server is still working, The user has disconnected
                    {
                        richTextBoxLogs.AppendText(client_name + " has disconnected.\n");
                    }
                    connected = false; // Close the connection
                }
            }
            // These are here because of the catch in line 164 
            // Let's assume that is it is the case, we never reach the catch in line 180 so if we put them in here
            // We never remove the corresponding client but server terminates.
            connected = false;
            thisClient.Close();
            onlineClients.Remove(client_name);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxLogs.Clear();
        }
    }
}
