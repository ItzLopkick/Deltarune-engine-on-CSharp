using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class Player
{
    public Image Sprite;
    public double Speed;
    public double X;
    public double Y;
    public double X2;
    public double Y2;
    public double Width;
    public double Height;
    public double SpeedBuff;
    public double hp;

    public double def;
    public double maxhp;
    public bool MoveUpFlag = false;
    public bool MoveRightFlag = false;
    public bool MoveDownFlag = false;
    public bool MoveLeftFlag = false;
    public bool ShiftFlag = false;
    public double SpeedFromStart;
    public double ButtonCount;
    public Player(Image Sprite,double hp,double def,double X,double Y,double SpeedFromStart,double Width, double Height, double SpeedBuff, double ButtonCount)
    {
        this.Sprite = Sprite;
        this.hp = hp;
        this.maxhp = hp;
        this.SpeedFromStart = SpeedFromStart;
        this.Speed = SpeedFromStart;
        this.X = X;
        this.Y = Y;
        this.Width = Width;
        this.Height = Height;
        this.SpeedBuff = SpeedBuff;
        this.MoveUpFlag = false;
        this.MoveLeftFlag = false;
        this.MoveRightFlag = false;
        this.ShiftFlag = false;
        this.ButtonCount = ButtonCount;
        this.X2 = X+Width;
        this.Y2 = Y+Height;
    }
    public void Controls(double ArenaX, double ArenaY, double ArenaWidth, double ArenaHeight)
    {
        double ArenaX2 = ArenaX+ArenaWidth;
        double ArenaY2 = ArenaY+ArenaHeight;
        Console.WriteLine("ShiftFlag : "+this.ShiftFlag);
        if (this.ShiftFlag == true)
        {
            Console.WriteLine("BUFF?");
            this.Speed = this.SpeedBuff;
        }
        else
        {
            Console.WriteLine("NORMAL?");
            this.Speed = this.SpeedFromStart;
        }
        if ((this.MoveRightFlag == true || this.MoveLeftFlag == true) && (this.MoveDownFlag == true || this.MoveUpFlag == true))
        {
            Speed = this.Speed*0.7;
        }
        else
        {
            this.Speed = this.SpeedFromStart;
        }
        if (this.MoveLeftFlag == true)
        {
            // Console.WriteLine("!left");
            if ((this.X - this.Speed) < ArenaX)
            {
                this.X = ArenaX;
                this.X2 = this.X+this.Width;
                // Console.WriteLine("!left_nope");
            }
            else
            {
                this.X = this.X-this.Speed;
                this.X2 = X+this.Width;
            }
        }
        if (MoveRightFlag == true)
        {
            // Console.WriteLine("!right");
            if ((this.X + this.Speed + this.Width) >  ArenaX2)
            {
                this.X = ArenaX2-this.Width;
                this.X2 = X+this.Width;
                // Console.WriteLine("!right_nope");
            }
            else
            {
                this.X = X+this.Speed;  
                this.X2 = X+this.Width;        
            }
        }
        if (this.MoveUpFlag == true)
        {
            // Console.WriteLine("!up");
            if ((this.Y - this.Speed) < ArenaY)
            {
                this.Y = ArenaY;
                this.Y2 = this.Y+this.Height;
                // Console.WriteLine("!up_nope");
            }
            else
            {
                this.Y = this.Y-this.Speed;
                this.Y2 = this.Y+this.Height;
            }
        }
        if (this.MoveDownFlag == true)
        {
            // Console.WriteLine("!down");
            if ((this.Y + this.Speed + this.Height) > ArenaY2)
            {
                this.Y = ArenaY2-this.Height;
                this.Y2 = this.Y+this.Height;
                // Console.WriteLine("!down_nope");
            }
            else
            {
                this.Y = this.Y+this.Speed;
                this.Y2 = this.Y+this.Height;
            }
        }
    }
}