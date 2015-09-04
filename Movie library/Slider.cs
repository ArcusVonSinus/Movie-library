using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Movie_library
{
    class Index
    {
        public int index;
        public Index(int index)
        {
            this.index = index;
        }
    }
    class Slider
    {
        int animationTime = 300;
        int fps = 30;     
                  
        Database database;
        Image[] obrazky = new Image[100];
        Panel panel;
        Timer timer;
        PictureBox[] pictureBoxes;
        int filmu;        
        int mezera
        {
            get
            {
                return ((int)((4.0 / 47.0) * panel.Height));
            }
        }
        int step = 0;
        Point[] startPoints;
        Size[] startSizes;       

        Point imagePoint(int index)
        {
            if (index == 0)
            {
                int temp = panel.Width / 2;
                temp -= imageSize(0).Width / 2;
                return new Point(temp, (panel.Height - imageSize(0).Height) / 2);
            }
            if(index == 1)
            {
                return new Point(imagePoint(0).X + imageSize(0).Width + mezera,(panel.Height-imageSize(1).Height)/2);
            }
            if(index > 1)
            {
                return new Point(imagePoint(1).X + imageSize(1).Width + mezera + (index-2)*(mezera + imageSize(2).Width), (panel.Height - imageSize(2).Height) / 2);
            }
            if (index == -1)
            {
                return new Point(imagePoint(0).X - imageSize(1).Width - mezera, (panel.Height - imageSize(1).Height) / 2);
            }
            if(index < -1)
            {
                return new Point(imagePoint(-1).X - imageSize(2).Width - mezera - (-index - 2) * (mezera + imageSize(2).Width), (panel.Height - imageSize(2).Height) / 2);
            }
            return new Point(0,0);
        }
        Size imageSize(int index)
        {
            if(index == 0)
            {
                int middleHeight = (int)((40.0 / 47.0) * panel.Height);
                return new Size(((int)((2.0 / 3.0) * middleHeight)), middleHeight);
            }
            if(index == 1)
            {
                int middleHeight = (int)((33.0 / 47.0) * panel.Height);
                return new Size(((int)((2.0 / 3.0) * middleHeight)), middleHeight);
            }
            if (index > 1)
            {
                int middleHeight = (int)((29.0 / 47.0) * panel.Height);
                return new Size(((int)((2.0 / 3.0) * middleHeight)), middleHeight);
            }
            if(index<0)
            {
                return imageSize(-index);
            }

            return new Size(0, 0);
        }
        
        public Slider(Panel panel, Database database)
        {
            this.database = database;
            this.panel = panel;
            filmu = database.pocetFilmu;
            pictureBoxes = new PictureBox[filmu];
            startPoints = new Point[filmu];
            startSizes = new Size[filmu];            
            for(int i = 0; i < filmu; i++)
            {
                pictureBoxes[i] = new PictureBox();
                panel.Controls.Add(pictureBoxes[i]);
                pictureBoxes[i].BackgroundImageLayout = ImageLayout.Zoom;
                //pictureBoxes[i].Image = Image.FromFile(imagesFilepaths[i]);
                pictureBoxes[i].Image = Image.FromFile(database.films[i].posterPath);
                pictureBoxes[i].SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxes[i].Location = imagePoint(i- database.selectedIndex);
                pictureBoxes[i].Size = imageSize(i- database.selectedIndex);
                pictureBoxes[i].Tag = i- database.selectedIndex;
                pictureBoxes[i].MouseClick += Slider_MouseClick;
               // if(i>10)
                 //   pictureBoxes[i].Visible = false;
            }

            timer = new Timer();
            timer.Interval = 1000 / fps;
            timer.Tick += tick;
            /*
            pictureBoxes[0].Visible = true;
            pictureBoxes[1].Visible = true;
            pictureBoxes[2].Visible = true;
            pictureBoxes[3].Visible = true;
            pictureBoxes[4].Visible = true;*/
            

        }

        private void Slider_MouseClick(object sender, MouseEventArgs e)
        {
            int i = (int)(((PictureBox)(sender)).Tag);
            selectIndex(i + database.selectedIndex);
        }

        public void leftButtonPressed()
        {
            if (database.selectedIndex > 0)
            {
                database.selectedIndex--;
                for (int i = 0; i < filmu; i++)
                {
                    startPoints[i] = pictureBoxes[i].Location;
                    startSizes[i] = pictureBoxes[i].Size;

                    PictureBox pb = pictureBoxes[i];
                    int temp = (int)(pb.Tag);
                    temp++;
                    pb.Tag = temp;
                }
                step = 0;
                timer.Enabled = true;
            }
        }
        public void rightButtonPressed()
        {
            if (database.selectedIndex < filmu-1)
            {
                database.selectedIndex++;
                for (int i = 0; i < filmu; i++)
                {
                    startPoints[i] = pictureBoxes[i].Location;
                    startSizes[i] = pictureBoxes[i].Size;

                    PictureBox pb = pictureBoxes[i];
                    int temp = (int)(pb.Tag);
                    temp--;
                    pb.Tag = temp;
                }
                step = 0;
                timer.Enabled = true;
            }
        }
        public void selectIndex(int index)
        {
            if(index>0 && index<filmu)
            {
                database.selectedIndex =index;
                for (int i = 0; i < filmu; i++)
                {
                    startPoints[i] = pictureBoxes[i].Location;
                    startSizes[i] = pictureBoxes[i].Size;

                    PictureBox pb = pictureBoxes[i];
                    pb.Tag = i - database.selectedIndex;
                }
                step = 0;
                timer.Enabled = true;
            }
            else
            {
                throw new System.InvalidOperationException("Index out of bounds");
            }
        }        
        private void tick(object sender, object arguments)
        {
            step++;
            if (step > fps * animationTime / 1000)
            {
                timer.Enabled = false;
                step = 0;
                foreach (PictureBox pb in pictureBoxes)
                {
                    int i = (int)(pb.Tag);
                    pb.Size = imageSize(i);
                    pb.Location = imagePoint(i);
                }
                return;
            }
            foreach (PictureBox pb in pictureBoxes)
            {
                int i = (int)(pb.Tag);
                pb.Size = plynulyPosun(startSizes[i+ database.selectedIndex], imageSize(i), step / (fps * animationTime / 1000.0));
                pb.Location = plynulyPosun(startPoints[i+ database.selectedIndex], imagePoint(i), step / (fps * animationTime / 1000.0));
            }
        }
        private int plynulyPosun(int start, int cil, double progress)
        {
            double factor = -progress * (progress - 2.0);
            int delka = cil - start;
            return((int)(start+delka*factor));
        }
        private Point plynulyPosun(Point start, Point cil, double progress)
        {
            return new Point(plynulyPosun(start.X, cil.X, progress), plynulyPosun(start.Y, cil.Y, progress));
        }
        private Size plynulyPosun(Size start, Size cil, double progress)
        {
            return new Size(plynulyPosun(start.Width, cil.Width, progress), plynulyPosun(start.Height, cil.Height, progress));
        }

        public void resize()
        {
            if(!timer.Enabled)
            {
                foreach(PictureBox pb in pictureBoxes)
                {
                    int i = (int)(pb.Tag);
                    pb.Size = imageSize(i);
                    pb.Location = imagePoint(i);
                }
            }
        }
        public void dispose()
        {
            foreach(PictureBox pb in pictureBoxes)
            {
                pb.Dispose();
            }
            timer.Dispose();            
        }

    }
}
