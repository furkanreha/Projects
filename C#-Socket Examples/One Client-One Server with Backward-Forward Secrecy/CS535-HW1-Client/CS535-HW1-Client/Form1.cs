using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;

namespace CS535_HW1_Client
{
    public partial class Form1 : Form
    {
        bool connected = false;
        bool terminating = false;
        Socket clientSocket;
        string currentPassword;
        tamperProofProcessor processor = new tamperProofProcessor();

        private void Form1_FormClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            connected = false;
            terminating = true;
            Environment.Exit(0);
        }
        public Form1()
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            InitializeComponent();
        }

        private void writeLog(string log)
        {
            richTextBoxLogs.AppendText(log + "\n");
        }

        private void endSpace()
        {
            writeLog("**********************************************************************************");
        }

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public string EncryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        public string DecryptText(string input, string password)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        private string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private Tuple<string, string> unpad(string input)
        {
            int firstStringPosition = input.IndexOf("|");

            string actualMessage = input.Substring(0, firstStringPosition);
            string signature = input.Substring(firstStringPosition + 1, input.Length - firstStringPosition - 1);

            return new Tuple<string, string>(actualMessage, signature);
        }

        private void receiveMessage()
        {
            Byte[] buffer = new Byte[16384];
            clientSocket.Receive(buffer); // Recieve the response of the server

            string ciphertext = Encoding.Default.GetString(buffer);
            ciphertext = ciphertext.Substring(0, ciphertext.IndexOf("\0"));
            
            writeLog("Client revieves the ciphertext:");
            writeLog(ciphertext); 
            
            string plaintext = DecryptText(ciphertext, currentPassword);
            writeLog("Client decrypts ciphertext using current symmetric key:");
            writeLog(plaintext);

            Tuple<string, string> unpaddedPlaintext = unpad(plaintext);
            string actualMessage = unpaddedPlaintext.Item1;
            string receivedSignature = unpaddedPlaintext.Item2;
            writeLog("Client unpads the plaintext (as actualMessage and receivedHash) and checks SHA256(actualMessage) == receivedHash:");
            writeLog("Received hash = " + receivedSignature);
            writeLog("SHA256 (actualMessage) = " + receivedSignature);

            if (ComputeSha256Hash(actualMessage) == receivedSignature)
            {
                writeLog("Received hash matches calculated hash. Authentication succeeds..");
                if (actualMessage == "REKEY")
                {   
                    writeLog("Server recieves REKEY..");
                    endSpace();
                    Tuple<string, string> responseTuple = processor.rekey();
                    writeLog(responseTuple.Item2);
                    currentPassword = responseTuple.Item1;
                }
                else
                {
                    writeLog("Actual received message from server: " + actualMessage);
                    endSpace();
                }


            }
            else
            {
                writeLog("Received hash doesn't match calculated hash. Authentication fails..");
                endSpace();
            }

        }

        private void sendMessage(string message)
        {
            Byte[] buffer = new Byte[16384];
            writeLog("Client wants to send the following message to the Server: " + message);
            string signature = ComputeSha256Hash(message);
            writeLog("Client calculates SHA256 (message) as hash: " + signature);
            string newMessage = message + "|" + signature;
            writeLog("Client pads message with hash as follows (message | hash):");
            writeLog(newMessage);
            string ciphertext = EncryptText(newMessage, currentPassword);
            writeLog("Client encrypts (message | hash) using current symmetric key and sends ciphertext to the server:");
            writeLog(ciphertext);
            buffer = Encoding.Default.GetBytes(ciphertext);
            clientSocket.Send(buffer);
            endSpace();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                string IP = "127.0.0.1";
                int portNum = 8080;
                clientSocket.Connect(IP, portNum);

                writeLog("Client Setup..");
                writeLog("Rekey to get initial password..");
                endSpace();

                Tuple<string, string> responseTuple = processor.rekey();

                currentPassword = responseTuple.Item1;
                writeLog(responseTuple.Item2);

                receiveMessage();

                connected = true;
                buttonConnect.Enabled = false;
                buttonRekey.Enabled = true;
                buttonSendMessage.Enabled = true;

                Thread recieveThread = new Thread(recieve);
                recieveThread.Start();
            }

            catch
            {
                writeLog("Could not connect to the server!");
            }
        }

        private void recieve()
        {
            while (connected && !terminating)
            {
                try
                {
                    receiveMessage();
                }

                catch
                {
                    buttonConnect.Enabled = true;
                    richTextBoxMessage.Clear();
                    richTextBoxLogs.Clear();
                    buttonRekey.Enabled = false;
                    buttonSendMessage.Enabled = false;
                    processor.reset();

                    if (connected)
                    {
                        writeLog("The server has disconnected.");
                    }

                    break;
                }
                
            }
        }

        private void buttonRekey_Click(object sender, EventArgs e)
        {
            Tuple<string, string> responseTuple = processor.rekey();
            writeLog(responseTuple.Item2);
            sendMessage("REKEY");
            currentPassword = responseTuple.Item1;
        }

        private void buttonSendMessage_Click(object sender, EventArgs e)
        {
            string message = richTextBoxMessage.Text;
            if (message != "")
            {
                sendMessage(message);
                richTextBoxMessage.Clear();
            }
        }
    }
}

class tamperProofProcessor
{
    List<string> hashChain1 = new List<string>();
    List<string> hashChain2 = new List<string>();

    int rekeyIndex = -1, hashChainLen = 50;

    public tamperProofProcessor()
    {
        string path = Directory.GetCurrentDirectory();

        System.IO.StreamReader file1 = new System.IO.StreamReader(path + "\\hashChain-1.txt");
        string line;
        while ((line = file1.ReadLine()) != null) // Until the end of file
        {
            hashChain1.Add(line);
        }

        file1.Close();

        System.IO.StreamReader file2 = new System.IO.StreamReader(path + "\\hashChain-2.txt");

        while ((line = file2.ReadLine()) != null) // Until the end of file
        {
            hashChain2.Add(line);
        }
    }

    string xor(string a, string b)
    {


        string resultingString = "";
        for (int i = 0; i < a.Length; i++)
        {
            int hexIntA = int.Parse(a[i].ToString(), System.Globalization.NumberStyles.HexNumber);
            int hexIntB = int.Parse(b[i].ToString(), System.Globalization.NumberStyles.HexNumber);


            int resultingHexInt = hexIntA ^ hexIntB;
            resultingString = resultingString + resultingHexInt.ToString("x");
        }

        return resultingString;
    }

    public Tuple<string, string> rekey()
    {
        rekeyIndex++;
        int h2Index = hashChainLen - rekeyIndex;
        string response = "";
        response += "Rekeying is issued..\n";
        response += "New Password = H1(" + rekeyIndex.ToString() + ") ^ H2(" + h2Index.ToString() + ")\n";
        response += "H1(" + rekeyIndex.ToString() + ") = " + hashChain1[rekeyIndex] + "\n";
        response += "H2(" + h2Index.ToString() + ") = " + hashChain2[h2Index] + "\n";
        string currentPassword = xor(hashChain1[rekeyIndex], hashChain2[h2Index]);
        response += "Current Password = " + currentPassword + "\n";
        response += "**********************************************************************************";

        return new Tuple<string, string>(currentPassword, response);
    }

    public void reset()
    {
        rekeyIndex = -1;
    }
}