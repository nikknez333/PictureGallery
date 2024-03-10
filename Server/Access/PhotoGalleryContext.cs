using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using Common.Model;

namespace Server.Access
{
    class PhotoGalleryContext : DbContext
    {
        public PhotoGalleryContext() : base("dbConnection2015")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<PhotoGalleryContext, Configuration>());
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<User> Users {get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Event> Events { get; set; }
    }
}
