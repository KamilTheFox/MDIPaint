using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryForPaint;

namespace FirstAddon
{
    internal class AddonMain : IAddons
    {
        public InfoAddon InfoAddons => new InfoAddon() { Name = "Аддон с фигурами", Description = "Тут простые фигуры"};

        public IAddonDraw[] addons => new IAddonDraw[]
        {
            new RectangleDraw(),
            new CircleDraw(),
            new TextDraw(),
            new FillDraw(),
        };
    }
}
