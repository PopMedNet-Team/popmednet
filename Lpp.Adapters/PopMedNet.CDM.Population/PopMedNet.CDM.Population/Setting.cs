namespace PopMedNet.CDM.Population
{
    public class Setting
    {
        public DbConnection Source { get; set; }
        public DbConnection[] Replicas { get; set; }
    }

    public class DbConnection
    {
        public string ClassType { get; set; }
        public string ConnectionString { get; set; }
        public string DbSchema { get; set; }
    }
}
