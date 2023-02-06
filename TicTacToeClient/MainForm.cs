using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TicTacToeClient
{
    public partial class MainForm : Form
    {
        public SimpleTcpClient client;
        GameForm gameForm;


        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            client = new SimpleTcpClient("127.0.0.1:8000");
            client.Events.Connected += Events_Connected;
            client.Events.DataReceived += Events_DataReceived;
            client.Connect();
        }


        
        private void Events_Connected(object sender, ClientConnectedEventArgs e)
        {

        }
        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            switch (Encoding.UTF8.GetString(e.Data).Split(':')[0])
            {
                case Messages.Server.Start:
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.Hide();
                        gameForm = new GameForm(Encoding.UTF8.GetString(e.Data).Split(':')[1], this);
                        gameForm.Show();
                    });
                    break;
                case Messages.Server.Matches:
                    this.Invoke((MethodInvoker)delegate {

                        String[] matches = Encoding.UTF8.GetString(e.Data).Split(':');
                        PlayersListBox.Items.Clear();

                        for (int i = 1; i < matches.Length; i++)
                        {
                            PlayersListBox.Items.Add($"{matches[i]}");
                        }
                    });
                    break;
                case Messages.Server.Logged:
                    MessageBox.Show("player with that name is already logged in");
                    client.Disconnect();
                    break;
                case Messages.Server.StartGame:
                    this.Invoke((MethodInvoker)delegate {
                        this.Hide();
                        gameForm = new GameForm(Encoding.UTF8.GetString(e.Data).Split(':')[1], this);
                        gameForm.Show();
                    });
                    break;
                case Messages.Server.Disconnect:
                    MessageBox.Show("Incorrect login or password");
                    client.Disconnect();
                    break;
                case Messages.Client.Move:
                    this.Invoke((MethodInvoker)delegate {
                        gameForm.TurnLabel.Text = "Your turn";
                        gameForm.EnemyMove(Int32.Parse(Encoding.UTF8.GetString(e.Data).Split(':')[1]));
                    });
                    break;
                case Messages.Server.Win:
                    this.Invoke((MethodInvoker)delegate
                    {
                        gameForm.Close();
                        this.Show();
                        HostButton.Enabled = true;
                        RefreshButton.Enabled = true;
                    });
                    MessageBox.Show("Player " + Encoding.UTF8.GetString(e.Data).Split(':')[3] + " won", "End");
                    break;
                case Messages.Server.Lost:
                    this.Invoke((MethodInvoker)delegate
                    {
                        gameForm.Close();
                        this.Show();
                        HostButton.Enabled = true;
                        RefreshButton.Enabled = true;
                    });
                    MessageBox.Show("You lose, player " + Encoding.UTF8.GetString(e.Data).Split(':')[1] + " won", "End");
                    break;
                default:
                    break;

            }
        }

       

        private void HostButton_Click(object sender, EventArgs e)
        {
            if (HostButton.Enabled)
            {
                HostButton.Enabled = false;
                client.Send(Messages.Client.Host);
                RefreshButton.Enabled = false;
            }
            else
            {
                HostButton.Enabled=true;
                client.Send(Messages.Client.Cancel);
                RefreshButton.Enabled=true;
            }
        }

        private void JoinButton_Click(object sender, EventArgs e)
        {
            if(PlayersListBox.SelectedItem == null)
            {
                return;
            }
            client.Send($"{Messages.Client.Join}:{PlayersListBox.SelectedItem.ToString()}");
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            client.Send(Messages.Server.Matches);
        }

        private void PlayersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // unselected host
            if (PlayersListBox.SelectedItem == null)
            {
                JoinButton.Enabled = false;
            }
            else
            {
                if (RefreshButton.Enabled)
                {
                    JoinButton.Enabled = true;
                }
            }
        }
        private void Login_Click(object sender, EventArgs e)
        {
            if (LoginButton.Text.Equals("Log out"))
            {
                client.Disconnect();
                LoginPartVisible(true);
                return;
            }
            client.Connect();
            client.Send($"{Messages.Client.Login}:{LoginTextBox.Text}:{PasswordTextBox.Text}");
        }

        private void LoginPartVisible(bool v)
        {
            throw new NotImplementedException();
        }

        private void RegisterClick(object sender, EventArgs e)
        {

        }
    }
}
