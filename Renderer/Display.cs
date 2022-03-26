using GUI_20212202_CM7A68.Logic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GUI_20212202_CM7A68.Renderer
{
    public class Display : FrameworkElement
    {
        Size area;
        IGameModel model;
        public void SetupSize(Size area)
        {
            this.area = area;
            model.SetupSize(new System.Drawing.Size((int)area.Width, (int)area.Height));
            InvalidateVisual();
        }
        public void SetupModel(IGameModel model)
        {
            this.model = model;
        }
        bool swapped;
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (area.Width > 0 && area.Height > 0)
            {
                base.OnRender(drawingContext);
                drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Backgrounds", "mkmap1.jpg"),
                    UriKind.RelativeOrAbsolute))), null, new Rect(0, 0, area.Width, area.Height));


                if (model.Robot1.Center.X < model.Robot2.Center.X)
                {
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", "robotpic_stand.png"),
                 UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot1.Center.X - area.Width / 12, model.Robot1.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));

                    drawingContext.PushTransform(new ScaleTransform(-1, 1, model.Robot2.Center.X, model.Robot2.Center.Y));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", "robotpic_stand.png"),
                  UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot2.Center.X - area.Width / 12, model.Robot2.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                    drawingContext.Pop();
                }
                else
                {
                    drawingContext.PushTransform(new ScaleTransform(-1, 1, model.Robot1.Center.X, model.Robot1.Center.Y));
                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", "robotpic_stand.png"),
               UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot1.Center.X - area.Width / 12, model.Robot1.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                    drawingContext.Pop();

                    drawingContext.DrawRectangle(new ImageBrush(new BitmapImage(new Uri(Path.Combine("Renderer", "Images", "Robots", "robotpic_stand.png"),
                  UriKind.RelativeOrAbsolute))), null, new Rect(model.Robot2.Center.X - area.Width / 12, model.Robot2.Center.Y - area.Height / 6, area.Width / 6, area.Height / 3));
                }






            }

        }
    }
}
