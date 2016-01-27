using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Shmup
{
    public class Ship
    {

        // state

        public int x;
        public int y;
        public Sprite sprite;
        public int direction;
        public int rotation;
        public int speed =7;
        public  int TURNING_SPEED = 12;

        public Boolean forward = false;




        public void moveForward() {
            x -= Convert.ToInt32(speed * Math.Sin((Math.PI * direction / 180.0)));
            y -= Convert.ToInt32(speed * Math.Cos((Math.PI * direction / 180.0)));
            Debug.WriteLine("X:{0} \t Y:{1} Direction:{2} ", x, y,direction);
        }
        // behaviour 

       
    }
}
