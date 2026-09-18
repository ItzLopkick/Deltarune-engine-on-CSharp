using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Input;
using System.Security;
using System;
using System.Media;
using NAudio;
using NAudio.Wave;
using System.Windows.Documents;
using System.Numerics;

class PlayerStats
{
    public double hp;
    public double maxhp;
    public double def;
    public PlayerStats(double hp,double maxhp,double def)
    {
        this.hp = hp;
        this.maxhp = maxhp;
        this.def = def;
    }
}