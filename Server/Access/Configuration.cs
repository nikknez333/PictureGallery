using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using Common.Model;

namespace Server.Access
{
    class Configuration : DbMigrationsConfiguration<PhotoGalleryContext>
    { 
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "PhotoGalleryContext";
        }

        protected override void Seed(PhotoGalleryContext context)
        {
            User admin = new User() {Username = "admin", Password = "admin", Name = "Pera", Surname = "Peric", Type = TypeOfUser.ADMIN};

            if(!DBManager.Instance.UserExist("admin"))
            {
                DBManager.Instance.AddUser(admin);
            }
        }
    }
}
