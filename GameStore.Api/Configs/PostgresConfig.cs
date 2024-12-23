namespace GameStore.Api.Configs;

public class PostgresConfig
{
    public const string SectionName = "Postgres";
    public string Hostname { get; set; } = string.Empty;
    public string Port { get; set; } = "5432";
    public string Database { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string ConnectionString =>
        $"Server={Hostname};Port={Port};Database={Database};User ID={Username};Password={Password};";
}
