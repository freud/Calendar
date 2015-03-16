using Simple.Data;

namespace Calendar.Data
{
    public static class DbHelper
    {
        public static InMemoryAdapter CreateStructures()
        {
            var adapter = new InMemoryAdapter();
            adapter.SetAutoIncrementKeyColumn("Events", "Id");
            return adapter;
        }

        //public static void InitDb()
        //{
        //    var adapter = CreateStructures();
        //    Database.UseMockAdapter(adapter);

        //    var db = Database.Open();
        //    db.Test.Insert(Id: 1, Name: "Alice");
        //}
    }
}
