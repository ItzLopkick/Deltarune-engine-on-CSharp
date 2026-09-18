using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Collections.Generic;
using System.Windows.Input;
using System.Security;
using System;
using System.Media;
using NAudio;
using NAudio.Wave;
using TiledSharp;

class Freemode
{
    PlayerF player;
    Image Room1;
    public async Task<bool> StartFreeMode(Canvas gcanvas,Sound RMusic,Sound RSound,double windowWidth,double windowHeight,PlayerStats FirstCharacterStats)
    {
        int integerl = 0;
        Room1 = new Image
        {
            Width = windowWidth,
            Height = windowHeight,
            Source = new BitmapImage(new Uri("assets/Room1.png",UriKind.Relative))
        };
        // player = new Player(
        //     new Image
        //     {
        //         Width = 350,
        //         Height = 350,
        //         Source = new BitmapImage(new Uri("assets/FreeMode/IdleDown.png",UriKind.Relative))
        //     }, // sprite
        //     275, //hp
        //     5, // defence
        //     windowWidth/2, //spawnX
        //     windowHeight/2, //spawnY
        //     5, // speedfromstart
        //     50, //width
        //     50, // height
        //     15, // sprintspeed
        //     4 // ishowspeed said it is "BUTTON COUNT *HAW HAW"
        // );
        gcanvas.Children.Add(Room1);
        // gcanvas.Children.Add(player.Sprite);

        Canvas.SetLeft(Room1,0);
        Canvas.SetTop(Room1,0);
        while (true)
        {
            Console.WriteLine(FirstCharacterStats.hp+","+FirstCharacterStats.maxhp);
            foreach (Key kbb in InputSystem.keyboardbuttons)
            {
                Console.WriteLine(kbb);
            }
            // player.Controls(0,0,windowWidth,windowHeight); 
            
            // Canvas.SetLeft(player.Sprite,player.X);
            // Canvas.SetTop(player.Sprite,player.Y);

            integerl = integerl + 1;
            if (integerl == 100)
            {
                Console.WriteLine("End");
                gcanvas.Children.Clear();
                return true;
            }
            await Task.Delay(10);
            Console.WriteLine("-----------------");
        }
    }
}