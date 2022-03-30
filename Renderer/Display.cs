using GUI_20212202_CM7A68.Logic;
using GUI_20212202_CM7A68.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212202_CM7A68.Renderer
{
    public class Display:FrameworkElement
    {
        Size area;
        IGameModel model;
        public void SetupSize(Size area)
        {
            this.area = area;
            InvalidateVisual();
        }
        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Backgrounds", "mkmap1.jpg"), 
                UriKind.RelativeOrAbsolute))), null, new Rect(0,0,area.Width, area.Height));
            Explosion e = new Explosion(area, new Point(area.Width / 2, area.Height / 2), 10, 2, 3);
            Explosion e1 = new Explosion(area, new Point(area.Width / 3, area.Height / 2), 10, 1, 5);
            Explosion e2 = new Explosion(area, new Point(area.Width / 4, area.Height / 3), 10, 3, 7);
            DrawExplosion(drawingContext, e);
            DrawExplosion(drawingContext, e1);
            DrawExplosion(drawingContext, e2);
        }

        public void DrawExplosion(DrawingContext drawingContext, Explosion explosion)           //TODO: 0 range bomb
        {
            double cursorX;
            double cursorY;

            #region vertical_drawing
            if (explosion.Height > 0)
            {
                cursorX = explosion.Center.X - explosion.PartSize.Width / 2;    //set cursor to the bottom left corner of the center part
                cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;   //
                for (int i = 0; i < (explosion.Height * 2) + 2; i++)            //+2 to help place top and bottom parts
                {
                    if (i < explosion.Height)                   //Upwards
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.VertContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))), 
                            null,
                            new Rect(
                            cursorX,
                            cursorY - explosion.PartSize.Height * i,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                    else if (i == explosion.Height)             //Top part
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.TopAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX,
                            cursorY - explosion.PartSize.Height * i,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                        cursorX = explosion.Center.X - explosion.PartSize.Width / 2;    //set cursor to the bottom left corner of the center part
                        cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;
                    }
                    else if(i < (explosion.Height * 2) + 1)                                        //Downwards
                    {
                        
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.VertContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                           null,
                           new Rect(
                           cursorX,
                           cursorY + explosion.PartSize.Height * ((i - 1) % explosion.Height),
                           explosion.PartSize.Width,
                           explosion.PartSize.Height)
                           );
                    }
                    else
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.BottomAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX,
                            cursorY + explosion.PartSize.Height * (explosion.Height),
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                }
            }
            #endregion

            #region horizontal_drawing
            if (explosion.Width > 0)
            {
                cursorX = explosion.Center.X - explosion.PartSize.Width / 2;    
                cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;   
                for (int i = 0; i < (explosion.Width * 2) + 2; i++)            //+2 to help place top and bottom parts
                {
                    if (i < explosion.Width)                   //LeftContinuation
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.SideContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX - explosion.PartSize.Width * i,
                            cursorY,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                    else if (i == explosion.Width)             //Leftmost
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.LeftAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX - explosion.PartSize.Width * i,
                            cursorY,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                        cursorX = explosion.Center.X - explosion.PartSize.Width / 2;    
                        cursorY = explosion.Center.Y - explosion.PartSize.Height / 2;
                    }
                    else if (i < (explosion.Width * 2) + 1)                                        //RightContinuation
                    {

                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.SideContAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                           null,
                           new Rect(
                           cursorX + explosion.PartSize.Width * ((i - 1) % explosion.Width),
                           cursorY,
                           explosion.PartSize.Width,
                           explosion.PartSize.Height)
                           );
                    }
                    else                                                                           //RightMost
                    {
                        drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.RightAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                            null,
                            new Rect(
                            cursorX + explosion.PartSize.Width * (explosion.Width),
                            cursorY,
                            explosion.PartSize.Width,
                            explosion.PartSize.Height)
                            );
                    }
                }
            }
            #endregion

            //Center piece
            drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(explosion.CenterAnim[explosion.FrameCount], UriKind.RelativeOrAbsolute))),
                null,
                new Rect(
                    explosion.Center.X - explosion.PartSize.Width / 2,
                    explosion.Center.Y - explosion.PartSize.Height / 2,
                    explosion.PartSize.Width,
                    explosion.PartSize.Height));

            ++explosion.FrameCount;
        }
    }
}
