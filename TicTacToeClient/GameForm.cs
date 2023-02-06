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
    public partial class GameForm : Form
    {
        List<Label> labels = new List<Label>();
        MainForm mainForm;
        string playerType;
        public GameForm(string playerType, MainForm mainForm)
        {
            InitializeComponent();
            labels = new List<Label>();
            this.mainForm = mainForm;
            this.playerType = playerType;

            labels.Add(Label11);
            labels.Add(Label21);
            labels.Add(Label31);
            labels.Add(Label12);
            labels.Add(Label22);
            labels.Add(Label32);
            labels.Add(Label13);
            labels.Add(Label23);
            labels.Add(Label33);

            foreach (Label l in labels)
            {
                l.Enabled = true;
                l.Text = "_";
            }
            if (playerType.Equals("PlayerO"))
            {
                foreach (Label l in labels)
                {
                    l.Enabled=true;
                }
                TurnLabel.Text = "Your turn";
            }
            else
            {
                foreach (Label l in labels)
                {
                    l.Enabled = false;
                }
                TurnLabel.Text = "Opponent turn";
            }
        }

        private void GameForm_Load(object sender, EventArgs e)
        {

        }

      

        private void Label22_Click(object sender, EventArgs e)
        {
             myMove(22);
        }

        private void Label33_Click(object sender, EventArgs e)
        {
            myMove(33);
        }

        private void label23_Click(object sender, EventArgs e)
        {
            myMove(23);
        }

        private void Label13_Click(object sender, EventArgs e)
        {
            myMove(13);
        }
        
        private void Label32_Click(object sender, EventArgs e)
        {
            myMove(32);
        }

        private void Label31_Click(object sender, EventArgs e)
        {
            myMove(31);
        }

        private void Label12_Click(object sender, EventArgs e)
        {
            myMove(12);
        }

        private void Label21_Click(object sender, EventArgs e)
        {
            myMove(21);
        }

        private void Label11_Click(object sender, EventArgs e)
        {
            myMove(11);
        }
        private void myMove(int labelIndex)
        {
            if (TurnLabel.Text.Equals("Your turn"))
            {
                if (labels.Find(x => Int32.Parse(x.Name[5].ToString() + x.Name[6].ToString()) == labelIndex).Text == "_")
                {
                    labels.Find(x => Int32.Parse(x.Name[5].ToString() + x.Name[6].ToString()) == labelIndex).Text = playerType[6].ToString();
                    mainForm.client.Send(Messages.Client.Move + $":{labelIndex}");
                    TurnLabel.Text = "Opponent turn";
                    foreach(Label label in labels)
                    {
                        label.Enabled = false;
                    }
                }
            }

            

            // CHECK IF WON
            if (
                Label11.Text != "_" && Label11.Text == Label12.Text && Label12.Text == Label13.Text ||
                Label21.Text != "_" && Label21.Text == Label22.Text && Label22.Text == Label23.Text ||
                Label31.Text != "_" && Label31.Text == Label32.Text && Label32.Text == Label33.Text ||
                Label11.Text != "_" && Label11.Text == Label21.Text && Label21.Text == Label31.Text ||
                Label12.Text != "_" && Label12.Text == Label22.Text && Label22.Text == Label32.Text ||
                Label13.Text != "_" && Label13.Text == Label23.Text && Label23.Text == Label33.Text ||
                Label11.Text != "_" && Label11.Text == Label22.Text && Label22.Text == Label33.Text ||
                Label13.Text != "_" && Label13.Text == Label22.Text && Label22.Text == Label31.Text)
            {
                mainForm.client.Send(Messages.Client.EndGame + ":"+playerType);
            }

           
        }

        public void EnemyMove(int labelIndex)
        {
            
            if (labels.Find(x => Int32.Parse(x.Name[5].ToString() + x.Name[6].ToString()) == labelIndex).Text == "_")
            {
                if (playerType == "PlayerO")
                {
                    labels.Find(x => Int32.Parse(x.Name[5].ToString() + x.Name[6].ToString()) == labelIndex).Text = "X";
                }
                else
                {
                    labels.Find(x => Int32.Parse(x.Name[5].ToString() + x.Name[6].ToString()) == labelIndex).Text = "O";
                }
                
                foreach(Label label in labels)
                {
                    if(label.Text != "X" && label.Text != "O")
                    {
                        label.Enabled = true;
                    }
                }
            }
        }
       
    }
}
