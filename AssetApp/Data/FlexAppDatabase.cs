using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetApp.Data
{
    public static class FlexAppDatabase
    {
        private static SQLiteAsyncConnection database;

        public static async Task Initialize()
        {
            var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

            database = new SQLiteAsyncConnection(databasePath);

            if(await TableNotExists("AppAssets"))
                await database.CreateTableAsync<AppAsset>();
            if(await TableNotExists("AppAttributes"))
                await database.CreateTableAsync<AppAttribute>();
            if (await TableNotExists("AppCategories"))
                await database.CreateTableAsync<AppCategory>();
            if (await TableNotExists("AppCategoryAttributes"))
                await database.CreateTableAsync<AppCategoryAttribute>();
            if (await TableNotExists("AppTokens"))
                await database.CreateTableAsync<AppToken>();
            if (await TableNotExists("AppSites"))
                await database.CreateTableAsync<AppSite>();

        }

        internal static AppAsset GetAsset(int asset_id)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                var asset = db.Table<AppAsset>().Where(x => x.Id == asset_id).FirstOrDefault();
                return asset;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static List<AppCategory> GetCategories()
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                var categories = db.Table<AppCategory>().ToList();
                return categories;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static List<AppAttribute> GetAttributes(int id)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                var attrs = db.Table<AppAttribute>().Where(x => x.AssetId == id).ToList();
                return attrs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static List<AppCategoryAttribute> GetCategoryAttributes(int id)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                var cat_attrs = db.Table<AppCategoryAttribute>().Where(x => x.CategoryId == id).ToList();
                return cat_attrs;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static List<AppAsset> GetAssets()
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                var assets = db.Table<AppAsset>().ToList();
                return assets;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static async Task<bool> TableNotExists(string name)
        {

            object[] _params = { };

            var query = "SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name='"+ name +"';";
            //var result = await database.QueryAsync(query, "table");

            var result = await database.ExecuteScalarAsync<int>(query);

            if(result < 1)
                return true;
            else
            {
                return false;
            }
                
        }

        internal static bool SaveOrUpdateAttribute(AppAttribute attr)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);

                if(attr.Id == 0)
                {
                    var ret = db.Insert(attr);
                }
                else
                {
                    var ret = db.Update(attr);
                }
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        internal static int SaveAsset(AppAsset live_asset)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);

                db.Insert(live_asset);
                
                return live_asset.Id;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        private static void CheckDatabase()
        {
            if (database == null)
            {
                string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");
                database = new SQLiteAsyncConnection(dbPath, false);
            }
        }

        public static bool InsertToken(AppToken token)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                db.Insert(token);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        internal static bool IsertAttributeCategories(List<AppCategoryAttribute> categoryAttributes)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);

                foreach (var cat_attr in categoryAttributes)
                {
                    db.Insert(cat_attr);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        internal static bool InsertCategories(List<AppCategory> categories)
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);

                foreach (var category in categories)
                {
                    db.Insert(category);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        internal static bool SaveAssets(List<AppAsset> assets)
        {

            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);

                foreach (var category in assets)
                {
                    db.Insert(category);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static void ClearTokens()
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                var tokens = db.DeleteAll<AppToken>();
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        public static List<AppToken> GetTokens()
        {
            try
            {
                var databasePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "flex.db");

                var db = new SQLiteConnection(databasePath);
                var tokens = db.Table<AppToken>().ToList();
                return tokens;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
    }
}