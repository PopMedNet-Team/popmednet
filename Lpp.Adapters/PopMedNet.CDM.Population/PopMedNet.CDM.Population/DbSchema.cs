using System.Data;

namespace PopMedNet.CDM.Population
{
    public record DbSchema
    {
        public string TableName { get; init; }
        public DbColumn[] Columns { get; init; }
        public DataTable Records { get; init; }
    }

    public record DbColumn
    {
        public string Name { get; init; }
    }
}
