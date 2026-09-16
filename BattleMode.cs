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

class BattleMode
{
    bool decline = false;
    BigInteger buttonswipestamp = 0; 
    BigInteger tick = 0;
    double windowWidth;
    double windowHeight;
    double characterscount = 2;
    double wave = 0;
    bool BattleFlag = false;
    bool Attack1Flag = false;
    bool AttackFlag2 = false;
    double AttackTime = 0;
    double meteorsvalue = 10;
    double buttonselected = 1;
    List<Attack> meteors = new List<Attack>{};
    Image playersprite;
    Player player;
    Image AttckBtnSprite;
    BtlButton AttackBtn;
    Image ActBtnSprite;
    BtlButton ActBtn;
    Image SpareBtnSprite;
    BtlButton SpareBtn;
    Image BlockBtnSprite;
    BtlButton BlockBtn;
    List<bool> buttons = new List<bool>{};
    PlayerBtl FirstCharacter;
    EnemyBtl Enemy1;
    Image ArenaSprite;
    Image MaxText;
    Image StephBar;
    TextBox PlayerHp;
    Attack2 CC;
    AttackSettings attackSettings;
    Arena arena;
    bool HealFlag = false;
    bool HitAnimFlag = false;
    bool DefendFlag = false;
    //Timers
    Timer MaxTextTimer = new Timer(2);

    Random random = new Random();



    public async Task<bool> StartBattle(Canvas gcanvas, Sound RMusic, Sound RSound, double windowWidth, double windowHeight)
    {
        wave = 2;
        // Arena!!!
        ArenaSprite = new Image // НАЧАЛО БАТЛА!!!!
        {
            Width = 500,
            Height = 500,
            Source = new BitmapImage(new Uri("assets/btlarena.png",UriKind.Relative))
        };
        arena = new Arena(
            ArenaSprite,
            Math.Round(windowWidth/2-250),
            Math.Round(windowHeight/2-250),
            500,
            500,
            Math.Round(windowWidth/2-235),
            Math.Round(windowHeight/2-235),
            465,
            465
        );
        gcanvas.Children.Add(arena.sprite);
        Canvas.SetLeft(arena.sprite,arena.spriteX);
        Canvas.SetTop(arena.sprite,arena.spriteY);

        // Texts
        MaxText = new Image
        {
            Width = 150,
            Height = 50,
            Source = new BitmapImage(new Uri("assets/Max.png",UriKind.Relative))
        };

        playersprite = new Image
        {
            Width = 50,
            Height = 50,
            Source = new BitmapImage(new Uri("assets/player.png",UriKind.Relative))
        };
        player = new Player(
            Sprite : playersprite, // sprite
            hp : 275, //hp
            def : 5, // defence
            X : windowWidth/2-200, //spawnX
            Y : windowHeight/2, //spawnY
            SpeedFromStart : 10, // speedfromstart
            Width : 50, //width
            Height : 50, // height
            SpeedBuff : 5, // sprintspeed
            ButtonCount : 4 // BUTTON COUNT
        );
        if (characterscount == 1)
        {
            FirstCharacter = new PlayerBtl(50,windowHeight/2-135,300,300);
        }
        else if (characterscount > 1)
        {
            FirstCharacter = new PlayerBtl(50,windowHeight/2-155,300,300);
        }
        Enemy1 = new EnemyBtl(windowWidth+200,windowHeight/2-135,150,150,250);
        Canvas.SetLeft(Enemy1.Sprite,Enemy1.X);
        Canvas.SetLeft(Enemy1.Sprite,Enemy1.Y);
        // GUI buttons / bar
        AttckBtnSprite = new Image
        {
            Width = 75,
            Height = 75,
            Source = new BitmapImage(new Uri("assets/Buttons/AttackBtn/AttackBtn1.png",UriKind.Relative))
        };
        ActBtnSprite = new Image
        {
            Width = 75,
            Height = 75,
            Source = new BitmapImage(new Uri("assets/Buttons/ActBtn/Actbtn1.png",UriKind.Relative))
        };
        SpareBtnSprite = new Image
        {
            Width = 75,
            Height = 75,
            Source = new BitmapImage(new Uri("assets/Buttons/Mercybtn/Mercy1.png",UriKind.Relative))
        };
        BlockBtnSprite = new Image
        {
            Width = 75,
            Height = 75,
            Source = new BitmapImage(new Uri("assets/Buttons/Blockbtn/Defend1.png",UriKind.Relative))
        };
        AttackBtn = new BtlButton(AttckBtnSprite,windowWidth/2-150,windowHeight-150,75,75,false,true);
        ActBtn = new BtlButton(ActBtnSprite,windowWidth/2-75,windowHeight-150,75,75,false,false);
        SpareBtn = new BtlButton(SpareBtnSprite,windowWidth/2,windowHeight-150,75,75,false,false);
        BlockBtn = new BtlButton(BlockBtnSprite,windowWidth/2+75,windowHeight-150,75,75,false,false);



        StephBar = new Image
        {
            Width = 300,
            Height = 300,
            Source = new BitmapImage(new Uri("assets/Stephsbar.png",UriKind.Relative))
        };
        gcanvas.Children.Add(StephBar);
        Canvas.SetLeft(StephBar,windowWidth/2-150);
        Canvas.SetTop(StephBar,windowHeight-300);
        PlayerHp = new TextBox
        {
            Width = 100,
            Height = 25,
            Text = ""+player.hp+"/"+player.maxhp,
            FontFamily = new FontFamily("pack://application:,,,/Fonts/#deltarune HP font"),
            FontSize = 22,

            Background = Brushes.Black,
            BorderBrush = Brushes.Black,
            Foreground = Brushes.LightGreen
            
        };
        PlayerHp.IsHitTestVisible = false;
        TextOptions.SetTextFormattingMode(PlayerHp, TextFormattingMode.Display);
        TextOptions.SetTextRenderingMode(PlayerHp, TextRenderingMode.Aliased);
        gcanvas.Children.Add(PlayerHp);
        Canvas.SetLeft(PlayerHp,windowWidth/2+30);
        Canvas.SetTop(PlayerHp,windowHeight-190);
        // Attack :)
        // Setings :)
        AttackSettings attackSettings = new AttackSettings
        (
            20    // Time
        );
        // Timers
        Timer Exiting = new Timer(5);

        gcanvas.Children.Add(player.Sprite);
        gcanvas.Children.Add(FirstCharacter.Sprite);
        Canvas.SetLeft(player.Sprite, player.X);
        Canvas.SetTop(player.Sprite, player.Y);
        Canvas.SetLeft(FirstCharacter.Sprite, FirstCharacter.X);
        Canvas.SetTop(FirstCharacter.Sprite,FirstCharacter.Y);

        gcanvas.Children.Add(AttackBtn.Sprite);
        Canvas.SetLeft(AttackBtn.Sprite, AttackBtn.X);
        Canvas.SetTop(AttackBtn.Sprite,AttackBtn.Y);
        gcanvas.Children.Add(ActBtn.Sprite);
        Canvas.SetLeft(ActBtn.Sprite, ActBtn.X);
        Canvas.SetTop(ActBtn.Sprite,ActBtn.Y);
        gcanvas.Children.Add(SpareBtn.Sprite);
        Canvas.SetLeft(SpareBtn.Sprite,SpareBtn.X);
        Canvas.SetTop(SpareBtn.Sprite,SpareBtn.Y);
        gcanvas.Children.Add(BlockBtn.Sprite);
        Canvas.SetLeft(BlockBtn.Sprite, BlockBtn.X);
        Canvas.SetTop(BlockBtn.Sprite,BlockBtn.Y);

        Rectangle CoolCubeSprite = new Rectangle
        {
            Width = 100,
            Height = 100,
            Fill = Brushes.White
        };
        CC = new Attack2(CoolCubeSprite,0,9999,100,100);
        gcanvas.Children.Add(CC.Sprite);
        Canvas.SetTop(CC.Sprite,CC.Y);
        CreateMeteors(meteorsvalue);
        foreach (Attack meteor in meteors)
        {
            gcanvas.Children.Add(meteor.Sprite);
            Canvas.SetLeft(meteor.Sprite,meteor.X);
            Canvas.SetTop(meteor.Sprite,meteor.Y);
        }
        buttons.Add(AttackBtn.IsActive);
        buttons.Add(ActBtn.IsActive);
        buttons.Add(SpareBtn.IsActive);
        buttons.Add(BlockBtn.IsActive);

        RSound.PlaySound("assets/sounds/weopenpull.mp3",1F);
        RMusic.PlayLoop("assets/sounds/FM.mp3",0.75F);

        while (1 == 1)
        {
            ProcesingKeysDown();
            ProcesingKeysUp();
            InputSystem.KeyLog();
            player.Controls(arena.physX,arena.physY,arena.physWidth,arena.physHeight); 
            
            Canvas.SetLeft(player.Sprite,player.X);
            Canvas.SetTop(player.Sprite,player.Y);
            // Canvas.SetLeft(FirstCharacter.Sprite, FirstCharacter.X);
            // Canvas.SetTop(FirstCharacter.Sprite,FirstCharacter.Y);

            PlayerHp.Text = ""+player.hp+"/"+player.maxhp;
            if (Enemy1.hp <= 0)
            {
                RMusic.Stop();
                return true;
            }
            if (decline == true)
            {
                RSound.PlaySound("assets/sounds/glue.mp3",1F);
                decline = false;
            }
            if (player.hp <= 0)
            {
                if (characterscount > 2)
                {
                    FirstCharacter.Downed = true;
                }
                else if (characterscount == 1)
                {
                    if (FirstCharacter.DeadAnimTime%10 == 0)
                    {
                        if (FirstCharacter.DeadAnimFrame < FirstCharacter.Animation2Bitmaps.Count-1)
                        {
                            FirstCharacter.DeadAnimFrame = FirstCharacter.DeadAnimFrame+1;
                            FirstCharacter.Sprite.Source = FirstCharacter.DeadAnimBitmaps[FirstCharacter.DeadAnimFrame];
                        }
                        else
                        {
                            FirstCharacter.Sprite.Source = FirstCharacter.DefaultSprite;
                            FirstCharacter.DeadAnimFrame = 0;
                            FirstCharacter.DeadAnimTime = 0;
                            Console.WriteLine("Died");
                            BattleFlag = false;
                            player.hp = 275;
                            if(wave == 2){MoveMeteorsOutOfBounds(meteors);}
                            else{CC.Disapear();}
                        }
                        
                    }
                    FirstCharacter.DeadAnimTime = FirstCharacter.DeadAnimTime+1;
                }
            }
            if (HealFlag == true)
            {
                // Anim
                FirstCharacter.Sprite.Source = FirstCharacter.Animation2Bitmaps[FirstCharacter.Animation2Frame];
                if (FirstCharacter.Animation2Time%10 == 0)
                {
                    if (FirstCharacter.Animation2Frame < FirstCharacter.Animation2Bitmaps.Count-1)
                    {
                        FirstCharacter.Animation2Frame = FirstCharacter.Animation2Frame+1;
                        FirstCharacter.Sprite.Source = FirstCharacter.Animation2Bitmaps[FirstCharacter.Animation2Frame];
                    }
                    else
                    {
                        MaxTextTimer.Activate();
                        FirstCharacter.Sprite.Source = FirstCharacter.DefaultSprite;
                        FirstCharacter.Animation2Frame = 0;
                        FirstCharacter.Animation2Time = 0;
                        // Action
                        if (player.hp+10 >= player.maxhp)
                        {
                            player.hp = player.maxhp;
                            Canvas.SetLeft(MaxText,windowHeight/2-135);
                            Canvas.SetTop(MaxText,50);
                            if (MaxTextTimer.DoAction == true)
                            {
                                Canvas.SetLeft(MaxText,99999999);
                                Canvas.SetTop(MaxText,9999999999);
                            }
                            MaxTextTimer.AddTime();
                        }
                        else
                        {
                            player.hp = player.hp + 10;
                        }
                        // End
                        HealFlag = false;
                        BattleFlag = true;
                    }
                }
                FirstCharacter.Animation2Time = FirstCharacter.Animation2Time+1;
            }
            if (HitAnimFlag == true)
            {
                // Anim
                FirstCharacter.Sprite.Source = FirstCharacter.HirtAnimBitmaps[FirstCharacter.HirtAnimFrame];
                Console.WriteLine("Anim hurt start");
                if (FirstCharacter.HirtAnimTime%10 == 0)
                {
                    Console.WriteLine("Frame = "+FirstCharacter.HirtAnimFrame);
                    if (FirstCharacter.HirtAnimFrame < FirstCharacter.HirtAnimBitmaps.Count-1)
                    {
                        FirstCharacter.HirtAnimFrame = FirstCharacter.HirtAnimFrame+1;
                        FirstCharacter.Sprite.Source = FirstCharacter.HirtAnimBitmaps[FirstCharacter.HirtAnimFrame];
                    }
                    else
                    {
                        Console.WriteLine("Write LINE");
                        FirstCharacter.Sprite.Source = FirstCharacter.DefaultSprite;
                        FirstCharacter.HirtAnimFrame = 0;
                        FirstCharacter.HirtAnimTime = 0;
                        // Action
                        
                        // End
                        HitAnimFlag = false;
                    }
                }
                FirstCharacter.HirtAnimTime = FirstCharacter.HirtAnimTime+1;
            }
            if (BattleFlag == true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Working :)");
                Console.ForegroundColor = ConsoleColor.Black;
                player.Speed = player.SpeedFromStart;
                if (wave == 1)
                {
                    foreach (Attack meteor in meteors)
                    {
                        if (CollisionsS.has_objects_collision(meteor,player) == true && attackSettings.attackTime%10 == 0)
                        {
                            double meteordamage = 10;
                            if (DefendFlag == true)
                            {
                                meteordamage = meteordamage-5;
                            }
                            player.hp = player.hp - meteordamage;
                            HitAnimFlag = true;
                        }
                        meteor.Undertale();
                        Canvas.SetTop(meteor.Sprite,meteor.Y);
                        if (meteor.Y > windowHeight)
                        {
                            meteor.X = random.Next(Convert.ToInt32(arena.spriteX),Convert.ToInt32(arena.spriteX2));
                            meteor.Y = random.Next(-6000,-500);
                            meteor.X2 = meteor.X + meteor.Width;
                            meteor.Y2 = meteor.Y + meteor.Height;

                            Canvas.SetLeft(meteor.Sprite,meteor.X);
                            Canvas.SetTop(meteor.Sprite,meteor.Y);
                        }
                    }
                    // :D
                    attackSettings.attackTime = attackSettings.attackTime + 1;
                    if (attackSettings.attackTime >= attackSettings.attacklength)
                    {
                        BattleFlag = false;
                        Attack1Flag = false;
                        attackSettings.attackTime = 0;
                        MoveMeteorsOutOfBounds(meteors);
                        wave = random.Next(1,2);
                    }
                }
                if (wave == 2)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Working :) WAVE 2 "+attackSettings.attackTime);
                    Console.ForegroundColor = ConsoleColor.Black;
                    if (attackSettings.attackTime == 0)
                    {
                        attackSettings.attacklength = 10*100;
                        Console.WriteLine("Aoo");
                        CC.Apear(arena.physX,arena.physY,arena.physX2,arena.physY2);
                        Canvas.SetLeft(CC.Sprite,CC.X);
                        Canvas.SetTop(CC.Sprite,CC.Y);
                    }
                    if (CC.IsApear == true)
                    {
                        if (CC.Agresive == false)
                        {
                            if (attackSettings.attackTime%20 == 0)
                            {
                                CC.Agresive = true;
                                CC.Sprite.Fill = Brushes.Red;
                            }
                        }
                        if (CC.Agresive == true)
                        {
                            if (CollisionsS.has_objects_collision(CC,player) == true && attackSettings.attackTime%20 == 0)
                            {
                                double ccdamage = 10;
                                if (DefendFlag == true)
                                {
                                    ccdamage = ccdamage-5;
                                }
                                player.hp = player.hp - ccdamage;
                                HitAnimFlag = true;
                            }
                            if (attackSettings.attackTime%60 == 0)
                                {
                                    CC.Sprite.Fill = Brushes.White;
                                    CC.Disapear();
                                    Canvas.SetLeft(CC.Sprite,CC.X);
                                    Canvas.SetTop(CC.Sprite,CC.Y);
                                }
                        }
                    }
                    if (CC.IsApear == false)
                    {
                        if (attackSettings.attackTime%70 == 0)
                        {
                            CC.Apear(arena.physX,arena.physY,arena.physX2,arena.physY2);
                            Canvas.SetLeft(CC.Sprite,CC.X);
                            Canvas.SetTop(CC.Sprite,CC.Y);
                        }
                    }
                    attackSettings.attackTime = attackSettings.attackTime + 1;
                    if (attackSettings.attackTime >= attackSettings.attacklength)
                    {
                        BattleFlag = false;
                        AttackFlag2 = false;
                        attackSettings.attackTime = 0;
                        CC.Disapear();
                        CC.Sprite.Fill = Brushes.White;
                        Canvas.SetLeft(CC.Sprite,CC.X);
                        Canvas.SetTop(CC.Sprite,CC.Y);
                        wave = random.Next(1,2);
                    }
                }
            }
            // Timers
            







            // tick
            tick = tick + 1;
            await Task.Delay(16);
        }
    }
    void CreateMeteors(double value)
    {
        for (int i = 0;i < value; i++)
        {
            double MeteorWidth = random.Next(70,100);
            double MeteorHeight = random.Next(70,100);
            Rectangle meteorsprite = new Rectangle{Width = MeteorWidth,Height = MeteorHeight,Fill = Brushes.Red};
            Attack meteor = new Attack(
                meteorsprite,
                random.Next(Convert.ToInt32(arena.spriteX),Convert.ToInt32(arena.spriteX2)), // X
                3000, // Y
                MeteorWidth,
                MeteorHeight,
                6
            );
            meteors.Add(meteor);
        }
    }
    void MoveMeteorsOutOfBounds(List<Attack> meteors)
    {
        foreach (Attack meteor in meteors)
        {
            meteor.Y = meteor.Y +9999;
            Canvas.SetTop(meteor.Sprite,meteor.Y);
        }
    }
    void ProcesingKeysUp()
    {
        if (!InputSystem.keyboardbuttons.Contains(Key.Right))
        {
            player.MoveRightFlag = false;
        }
        if (!InputSystem.keyboardbuttons.Contains(Key.Up))
        {
            player.MoveUpFlag = false;
        }
        if (!InputSystem.keyboardbuttons.Contains(Key.Left))
        {
            player.MoveLeftFlag = false;
        }
        if (!InputSystem.keyboardbuttons.Contains(Key.Down))
        {
            player.MoveDownFlag = false;
        }
        if (!InputSystem.keyboardbuttons.Contains(Key.X))
        {
            player.ShiftFlag = false;
        }
        if (!InputSystem.keyboardbuttons.Contains(Key.Escape))
        {
            
        }
    }
    void ProcesingKeysDown()
    {
        if (BattleFlag == true)
        {
            if (InputSystem.keyboardbuttons.Contains(Key.Right))
            {
                player.MoveRightFlag = true;
            }
            if (InputSystem.keyboardbuttons.Contains(Key.Up))
            {
                player.MoveUpFlag = true;
            }
            if (InputSystem.keyboardbuttons.Contains(Key.Down))
            {
                player.MoveDownFlag = true;
            }
            if (InputSystem.keyboardbuttons.Contains(Key.Left))
            {
                player.MoveLeftFlag = true;
            }
            if (InputSystem.keyboardbuttons.Contains(Key.X))
            {
                player.ShiftFlag = true;
            }
        }

        
        else if (BattleFlag == false)
        {
            player.MoveLeftFlag = false;
            player.MoveRightFlag = false;
            player.MoveUpFlag = false;
            player.MoveDownFlag = false;
            player.ShiftFlag = false;
            if (!(tick-buttonswipestamp >= 4))
            {
                return;
            }
            if (InputSystem.keyboardbuttons.Contains(Key.Right))
            {
                if (buttonselected >= 3)
                {
                    buttonselected = 0;
                    HandlingBattleButtons();
                }
                else
                {
                    buttonselected = buttonselected+1;
                    HandlingBattleButtons();
                }
            }
            if (InputSystem.keyboardbuttons.Contains(Key.Left))
            {
                if (buttonselected <= 0)
                {
                    buttonselected = 3;
                    HandlingBattleButtons();
                }
                else
                {
                    buttonselected = buttonselected-1;
                    HandlingBattleButtons();
                }
            }
            if (InputSystem.keyboardbuttons.Contains(Key.Enter) && BattleFlag == false)
            {
                if (buttonselected == 0)
                {
                    Enemy1.hp = Enemy1.hp - 20;
                    BattleFlag = true;
                    Console.WriteLine("Attack"+" "+BattleFlag+" "+wave);
                    DefendFlag = false;
                }
                if (buttonselected == 1)
                {
                    HealFlag = true;
                    DefendFlag = false;
                }
                if (buttonselected == 2)
                {
                    decline = true;
                    DefendFlag = false;
                }
                if (buttonselected == 3)
                {
                    BattleFlag = true;
                    DefendFlag = true;
                }
            }
        }
        if (InputSystem.keyboardbuttons.Contains(Key.Escape))
        {
            
        }
    }
    void HandlingBattleButtons()
    {
        if (buttonselected == 0)
        {
            AttackBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Attackbtn/AttackBtn2.png",UriKind.Relative));
        }
        if (buttonselected != 0)
        {
            AttackBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Attackbtn/AttackBtn1.png",UriKind.Relative));
        }
        if (buttonselected == 1)
        {
            ActBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Actbtn/ActBtn2.png",UriKind.Relative));
        }
        if (buttonselected != 1)
        {
            ActBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Actbtn/Actbtn1.png",UriKind.Relative));
        }
        if (buttonselected == 2)
        {
            SpareBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Mercybtn/Mercy1.png",UriKind.Relative));
        }
        if (buttonselected != 2)
        {
            SpareBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Mercybtn/Mercy1.png",UriKind.Relative));
        }
        if (buttonselected == 3)
        {
            BlockBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Blockbtn/Defend2.png",UriKind.Relative));
        }
        if (buttonselected != 3)
        {
            BlockBtn.Sprite.Source = new BitmapImage(new Uri("assets/Buttons/Blockbtn/Defend1.png",UriKind.Relative));
        }
        buttonswipestamp = tick;
        
    }
}