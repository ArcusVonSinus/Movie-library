using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Movie_library
{
    public partial class Form1 : Form
    {
        Slider slider;
        Database database;
        public Form1()
        {
            InitializeComponent();
            String[] s = new String[57];
            s[0] = "1.png";
            s[1] = "2.png";
            s[2] = "3.png";
            s[3] = "4.png";
            s[4] = "5.png";
            s[5] = "6.png";
            s[6] = "7.jpg";
            for(int i = 7;i<57;i++)
            {
                s[i] = "1.png";
            }
            database = new Database();
            database.films.Add(new Film("Hooovno", "hovno.avi", "1.png"));
            slider = new Slider(sliderPanel,database);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //slider.selectIndex(20);            
            database.films.Add(new Film("Hovno", "fd", "2.png"));
            filmsChanged();
        }
        public void nactiFilm(int index)
        {
            labelName.Text = database.selectedFilm.name;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left)
            {
                slider.leftButtonPressed();

                return true;
            }
            if(keyData == Keys.Right)
            {
                slider.rightButtonPressed();
                return true;
            }
            // etc..
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void sliderPanel_Resize(object sender, EventArgs e)
        {
            slider.resize();
        }
        private void filmsChanged()
        {
            slider.dispose();
            slider = new Slider(sliderPanel, database);
        }

    }
}
