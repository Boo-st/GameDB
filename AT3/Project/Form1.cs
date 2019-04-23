using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class frmMain : Form
    {
        String[] nameArray = new string[20];
        String[] platformArray = new string[20];
        String[] genreArray = new string[20];

        public frmMain()
        {
            InitializeComponent();
        }
        //function to save arrays to file
        private void saveData()
        {
            StreamWriter sWriter = new StreamWriter("mygames.txt");
            for(int i = 0; i < nameArray.Length; i++)
            {
                sWriter.WriteLine(nameArray[i] + ',' + platformArray[i] + ',' + genreArray[i]);
            }
            sWriter.Close();
        }
        //Function to empty array, second two arrays are updated upon reading in array
        public void initialize()
        {
            //reset arrays and text file
            StreamWriter sWriter = new StreamWriter("mygames.txt");
            for (int i=0;i<nameArray.Length;i++)
            {
                nameArray[i] = "Empty";
                // String line = nameArray[i].ToString();
                sWriter.WriteLine(nameArray[i] + ',' + ' ' + ','+ ' ');

            }
            sWriter.Close();
            openFile();
            fillLstbox();
        }
        //function to fill lstbox from array
        public void fillLstbox()
        {
            lstBoxGames.Items.Clear();
            lstBoxGames.Items.Add("Name\t\tPlatform\t\tGenre");
            for (int i = 0; i < nameArray.Length; i++)
            {
                lstBoxGames.Items.Add(nameArray[i] + "\t\t" + platformArray[i] + "\t\t" + genreArray[i]);
            }
        }
        //array sort algorthim, sorts by name array, and swaps other two array elements to match
        public void sortArray(String[] myArray)
        {
            string temp;
            for(int i = 0; i < myArray.Length; i++)
            {
                for(int j = i + 1; j < myArray.Length; j++)
                {
                    if(String.Compare(myArray[i], myArray[j]) > 0)
                    {
                        //sort name array, and swap genre and platform arrays to match
                        temp = myArray[i];
                        myArray[i] = myArray[j];
                        myArray[j] = temp;
                        //genre
                        temp = genreArray[i];
                        genreArray[i] = genreArray[j];
                        genreArray[j] = temp;
                        //platform
                        temp = platformArray[i];
                        platformArray[i] = platformArray[j];
                        platformArray[j] = temp;
                    }
                }
            }
            saveData();
        }

        public void openFile()
        {
            //Check to see if file exists
            //if (!System.IO.File.Exists("C:\\Users\\Mitchel King\\Documents\\mygames.txt"))
            //{
            //    System.IO.File.Create("C:\\Users\\Mitchel King\\Documents\\mygames.txt").Close();
            //}
            StreamReader sReader = new StreamReader("mygames.txt");
            String init;
            
            int counter = 0;
            int counter1 = 0;
            //While the line in datafile is not empty
            //while ((init = sReader.ReadLine()) != null)

            while (counter < nameArray.Length)
            {
                init = sReader.ReadLine();
                //if(init !=null)
                //{
                //    continue;
                //}
                //else
                //{
                //    initialize();
                //}
                if ((init.Substring(0, 5) != "Empty"))
                {
                    nameArray[counter] = Regex.Split(init ?? "", ",")[0];
                    platformArray[counter] = Regex.Split(init ?? "", ",")[1];
                    genreArray[counter] = Regex.Split(init ?? "", ",")[2];
                    counter1++;
                }
                else if(init.Substring(0, 5) == null)
                {
                    nameArray[counter] = "Empty";
                    platformArray[counter] = " ";
                    genreArray[counter] = " ";
                }
                counter++;
            }
            //Close reader stream

            sReader.Close();
            txtBoxCount.Text = counter1.ToString();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            //tool tips set for buttons
            toolTip1.SetToolTip(btnAdd, "Add entry to game database if database is not full");
            toolTip1.SetToolTip(btnClear, "Clears text boxes");
            toolTip1.SetToolTip(btnDelete, "Deletes selected entry from database");
            toolTip1.SetToolTip(btnOpen, "Opens game collection file and displays entries");
            toolTip1.SetToolTip(btnReset, "Resets database to empty");
            toolTip1.SetToolTip(btnSave, "Saves current state to file");
            toolTip1.SetToolTip(btnSearch, "Search database for matching game name");
            toolTip1.SetToolTip(btnUpdate, "Updates the selected entry");
            //initialize();
            openFile();
            sortArray(nameArray);
            fillLstbox(); 
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if(lstBoxGames.SelectedIndex == -1)
            {
                MessageBox.Show("Nothing selected to delete");
            }
            else
            {
                nameArray[lstBoxGames.SelectedIndex -1] = "Empty";
                genreArray[lstBoxGames.SelectedIndex -1] = " ";
                platformArray[lstBoxGames.SelectedIndex -1] = " ";
            }
            sortArray(nameArray);
            fillLstbox();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!(txtBoxName.Text == "" | txtBoxGenre.Text == "" || txtBoxPlatform.Text == ""))
            {
                for (int i = 0; i < nameArray.Length; i++)
                {
                    //boolean test to see if string value is considered empty, fill data into next empty
                    bool test = String.Compare(nameArray[i], "Empty") == 0;
                    if (!test)
                    {
                        continue;
                    }
                    else
                    {
                        nameArray[i] = txtBoxName.Text;
                        genreArray[i] = txtBoxGenre.Text;
                        platformArray[i] = txtBoxPlatform.Text;

                        txtBoxName.Clear();
                        txtBoxGenre.Clear();
                        txtBoxPlatform.Clear();

                        lstBoxGames.Items.Clear();
                        sortArray(nameArray);
                        fillLstbox();
                        //break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter all data!");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            for(int i=0; i< nameArray.Length; i++)
            {
                if(txtBoxName.Text == nameArray[i])
                {
                    //i+1 for the header titles at position 0 in listbox
                    lstBoxGames.SelectedIndex = i+1;
                    txtBoxName.Text = nameArray[i];
                    txtBoxGenre.Text = genreArray[i];
                    txtBoxPlatform.Text = platformArray[i];
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //get selected index element and save textbox properties to that array with same element
            if(lstBoxGames.SelectedIndex != -1)
            {
                nameArray[lstBoxGames.SelectedIndex - 1] = txtBoxName.Text;
                genreArray[lstBoxGames.SelectedIndex - 1] = txtBoxGenre.Text;
                platformArray[lstBoxGames.SelectedIndex - 1] = txtBoxPlatform.Text;
            }
            else
            {
                MessageBox.Show("No item selected to update");
            }
            sortArray(nameArray);
            fillLstbox();
        }

        private void lstBoxGames_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBoxName.Text = nameArray[lstBoxGames.SelectedIndex - 1];
            txtBoxGenre.Text = genreArray[lstBoxGames.SelectedIndex - 1];
            txtBoxPlatform.Text = platformArray[lstBoxGames.SelectedIndex - 1];
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            initialize();
            fillLstbox();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtBoxGenre.Clear();
            txtBoxName.Clear();
            txtBoxPlatform.Clear();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFile();
            sortArray(nameArray);
            fillLstbox();
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            sortArray(nameArray);
        }
    }
}
