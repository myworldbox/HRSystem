using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace HRSystem.Domain;

public class Enums
{

    public enum Status
    {
        [Display(Name = "Contract Draft")]
        ContractDraft,
        [Display(Name = "Contract Completed")]
        ContractCompleted,
        [Display(Name = "Payroll Completed")]
        PayrollCompleted,
        Terminated
    }

    public enum Position
    {
        Clerk,
        Teacher,
        Principal,
        [Display(Name = "HR Assistant")]
        HRAssistant
    }

    public enum Role
    {
        Clerk,
        Supervisor,
        Manager,
        Admin
    }

    public enum Dialog
    {
        primary,
        secondary,
        success,
        danger,
        warning,
        info,
        light,
        dark
    }

    public enum Database
    {
        // Relational Databases (SQL)
        SqlServer,
        PostgreSQL,
        MySQL,
        MariaDB,
        Oracle,
        SQLite,
        IBMDb2,
        Firebird,
        SAPHana,
        Teradata,
        Sybase,

        // NoSQL Databases
        MongoDB,
        Cassandra,
        Couchbase,
        Redis,
        Neo4j,
        DynamoDB,
        CosmosDB,
        ArangoDB,
        RavenDB,
        OrientDB,
        InfluxDB,
        FaunaDB,
        Firebase,
        Firestore,

        // Cloud/Hybrid Databases
        AmazonAurora,
        GoogleCloudSpanner,
        AzureSQL,
        AzureCosmosDB,
        PlanetScale,
        YugabyteDB,
        TiDB
    }
}