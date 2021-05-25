using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multimedia
{
    
    public class DataAccessLayer
    {
        TCCMEntities2 context;
        public DataAccessLayer()
        {
            context = new TCCMEntities2();
        }
        public void SaveImage(CustomImage image)
        {
            ImageTable imageTable = image;
            this.context.ImageTables.Add(imageTable);
            this.context.SaveChanges();
            image.id = imageTable.Id;
        }
        public List<CustomImage> Search(string name)
        {
            var list = this.context.ImageTables.Where(c => c.Name.Contains(name)).ToList();
            List<CustomImage> customImages = new List<CustomImage>();
            list.ForEach(c =>
            {
                customImages.Add(c);
            });
            return customImages;
        }
    }
}
