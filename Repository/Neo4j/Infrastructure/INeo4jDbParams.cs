using Repository.Neo4j.Infrastructure.Model;

namespace Repository.Neo4j.Infrastructure
{
    public interface INeo4jDbParams
    {
        Neo4jConnectionModel GetNeo4jConnection();
    }
}
