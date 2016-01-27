using System;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Graphics.Primitives;
using SdlDotNet.Core;

using System.Collections.Generic;

namespace Shmup {

    // ===== Data Types =====

    // Sprite Identifiers

    public enum Sprite {
        red
    }

    

    static class Program {

        // STATE
        // Keep the state of the elements of the game here (variables hold state).

        
        static Ship ship = new Ship();

        // This procedure is called (invoked) before the first time onTick is called.
        static void onInit() {
            ship.sprite = Sprite.red;
            ship.x= FRAME_WIDTH / 2;
            ship.y = FRAME_HEIGHT / 2;
            ship.direction = 0;
            ship.rotation = 0;
        }




        // This procedure is called (invoked) for each window refresh
        static void onTick(object sender, TickEventArgs args) {


            if (ship.forward) {
                ship.moveForward();
            }
            // STEP
            // Update the automagic elements and enforce the rules of the game here.

            if (ship.rotation!= 0) { 
                ship.direction +=  ship.rotation % 360;
            }

            // DRAW
            // Draw the new view of the game based on the state of the elements here.

            drawBackground();

            drawSprite(ship.sprite,ship.x,ship.y,ship.direction);

            // ANIMATE 
            // Step the animation frames ready for the next tick
            // ...

            // REFRESH
            // Tranfer the new view to the screen for the user to see.
            video.Update();

        }

        // this procedure is called when the mouse is moved
        static void onMouseMove(object sender, SdlDotNet.Input.MouseMotionEventArgs args) {
        }

        // this procedure is called when a mouse button is pressed or released
        static void onMouseButton(object sender, SdlDotNet.Input.MouseButtonEventArgs args) {
        }

        // this procedure is called when a key is pressed or released
        static void onKeyboard(object sender, SdlDotNet.Input.KeyboardEventArgs args) {

            if (args.Down) { 

                switch (args.Key) {
                    case SdlDotNet.Input.Key.W:
                        ship.forward = !ship.forward;
                        break;
                    case SdlDotNet.Input.Key.D :
                        ship.rotation-= ship.TURNING_SPEED;
                        break;
                    case SdlDotNet.Input.Key.A :
                        ship.rotation+= ship.TURNING_SPEED;
                        break;
                    case SdlDotNet.Input.Key.Escape :
                        Events.QuitApplication();
                        break;
                }

            } else {

                switch (args.Key) {
                    case SdlDotNet.Input.Key.W:
                        ship.forward = !ship.forward;
                        break;
                    case SdlDotNet.Input.Key.D:
                    case SdlDotNet.Input.Key.A:
                        ship.rotation = 0;
                        break;
                }

            }

        }


        // --------------------------
        // ----- GAME Utilities -----  
        // --------------------------

        // draw the background image onto the frame
        static void drawBackground() {
            video.Blit(background);
        }

        // draw the sprite image onto the frame
        // Sprite sprite - which sprite to draw
        // int x, int y - the co-ordinates of where to draw the sprite on the frame.
        static void drawSprite(Sprite sprite, int x, int y, int direction) {
            Surface temp = sprites.CreateSurfaceFromClipRectangle(shipsSpriteSheetCuts[(int)sprite]).CreateRotatedSurface(direction,false).Convert(video,false,false);
            temp.SourceColorKey = temp.GetPixel(new Point(0, 0));
            video.Blit(temp, new Point(x - (temp.Width / 2), y - (temp.Height / 2)));
             
        }

        // -- APPLICATION ENTRY POINT --

        static void Main() {
            //System.Windows.Forms.Cursor.Hide();

            // Create display surface.
            video = Video.SetVideoMode(FRAME_WIDTH, FRAME_HEIGHT, COLOUR_DEPTH, FRAME_RESIZABLE, USE_OPENGL, FRAME_FULLSCREEN, USE_HARDWARE);
            Video.WindowCaption = "Shmup";
            Video.WindowIcon(new Icon(@"images/shmup.ico"));

            // invoke application initialisation subroutine
            setup();

            // invoke secondary initialisation subroutine
            onInit();

            // register event handler subroutines
            Events.Tick += new EventHandler<TickEventArgs>(onTick);
            Events.Quit += new EventHandler<QuitEventArgs>(onQuit);
            Events.KeyboardDown += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(onKeyboard);
            Events.KeyboardUp += new EventHandler<SdlDotNet.Input.KeyboardEventArgs>(onKeyboard);
            Events.MouseButtonDown += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(onMouseButton);
            Events.MouseButtonUp += new EventHandler<SdlDotNet.Input.MouseButtonEventArgs>(onMouseButton);
            Events.MouseMotion += new EventHandler<SdlDotNet.Input.MouseMotionEventArgs>(onMouseMove);

            // while not quit do process events
            Events.TargetFps = 60;
            Events.Run();
        }

        // This procedure is called after the video has been initialised but before any events have been processed.
        static void setup() {

            // Load Art
            sprites = new Surface(@"images/sprites.png");

            backgroundColour = sprites.GetPixel(new Point(0, 0));

            background = video.CreateCompatibleSurface();
            background.Transparent = false;
            background.Fill(backgroundColour);

            // Specify where each sprite is in the sprite-sheet

            shipsSpriteSheetCuts[(int)Sprite.red] = new Rectangle(71,49,44,34);
        }

        // This procedure is called when the event loop receives an exit event (window close button is pressed)
        static void onQuit(object sender, QuitEventArgs args) {
            Events.QuitApplication();
        }

        // -- DATA --

        const int FRAME_WIDTH = 800;
        const int FRAME_HEIGHT = 600;
        const int COLOUR_DEPTH = 32;
        const bool FRAME_RESIZABLE = false;
        const bool FRAME_FULLSCREEN = false;
        const bool USE_OPENGL = false;
        const bool USE_HARDWARE = true;

        static Surface video;  // the window on the display

        static Surface background;
        static Surface sprites;

        static Color backgroundColour;

        // All the sprites come from one large image, this is called a 
        // sprite-sheet. For each sprite, we need to know which part (Rectangle)
        // of the larger sheet to use.  We store these rectangles in a variable
        // called sprite_sheet_cut for later use.
        // It is easier and more efficient to store one big image rather than lots
        // of little ones, especially if they are stored in the graphics memory.
        // (You can find the sprite-sheet for many games online - be careful of copyright!)
        // *The Rectangles are stored in the same sequence as the Sprite enum.
        static Rectangle[] shipsSpriteSheetCuts = new Rectangle[1];

    }
}
