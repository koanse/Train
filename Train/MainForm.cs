using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Train
{
    public partial class MainForm : Form
    {
        public List<RailwayItem> items;

        public class RailwayItem
        {
            public int PosX, PosY;
            
            public virtual void Draw(Graphics g)
            {
                g.DrawRectangle(Pens.AliceBlue, PosX - 10, PosY - 10, 20, 20);
            }

            public void Move(int deltaX, int deltaY, int max)
            {
                PosX += deltaX;
                PosY += deltaY;
                if (PosX < -200)
                {
                    PosX = max;
                }

                if (PosY < -200)
                {
                    PosY = max;
                }
            }

            public virtual string Name
            {
                get
                {
                    return this.ToString();
                }
            }

            public virtual bool IsLoco
            {
                get
                {
                    return false;
                }
            }

            public virtual List<RailwayItem> Children
            {
                get
                {
                    return null;
                }
            }
        }

        public class Locomotive : RailwayItem
        {
            private List<RailwayItem> children = new List<RailwayItem>();

            public Locomotive(List<RailwayItem> children)
            {
                this.children = children;
            }

            public override void Draw(Graphics g)
            {
                var bmp = Bitmap.FromFile("Locomotive.png");

                g.DrawImage(bmp, PosX + bmp.Width / 2, PosY + bmp.Height / 2);
            }

            public override string Name
            {
                get
                {
                    return "Locomotive";
                }
            }

            public override bool IsLoco
            {
                get
                {
                    return true;
                }
            }

            public override List<RailwayItem> Children
            {
                get
                {
                    return children;
                }
            }
        }

        public class Carriage : RailwayItem
        {
            public override void Draw(Graphics g)
            {
                var bmp = Bitmap.FromFile("Carriage.png");

                g.DrawImage(bmp, PosX + bmp.Width / 2, PosY + bmp.Height / 2);
            }

            public virtual string Name
            {
                get
                {
                    return "Carriage";
                }
            }
        }

        public MainForm()
        {
            InitializeComponent();

            var carriage3 = new Carriage
            {
                PosX = 2 * 106 + 118,
                PosY = 27
            };

            var carriage2 = new Carriage
            {
                PosX = 106 + 118,
                PosY = 27
            };
            var carriage1 = new Carriage
            {
                PosX = 118,
                PosY = 27
            };
            var locomotive = new Locomotive(new [] {carriage1, carriage2, carriage3}.ToList<RailwayItem>());
            items = new RailwayItem[] { carriage1, carriage2, carriage3, locomotive }.ToList<RailwayItem>();

            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var size = 500;
            var bmp = new Bitmap(size, size);

            var g = Graphics.FromImage(bmp);
            
            foreach (var item in items)
            {
                item.Draw(g);
            }

            foreach (var item in items)
            {
                item.Move(-1, 0, size);
            }

            this.BackgroundImage = bmp;
        }
    }
}
