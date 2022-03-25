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
        }
    }
}
