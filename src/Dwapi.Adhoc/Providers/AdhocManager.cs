using System;
using System.Data.SqlClient;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using Dwapi.Adhoc.Models;
using MySql.Data.MySqlClient;
using Serilog;

namespace Dwapi.Adhoc.Providers
{
    public class AdhocManager : IAdhocManager
    {
        public void RefreshMetadata(string connectionString, string path, SourceDbType dbType)
        {
            var qb = new QueryBuilder();
            qb.BehaviorOptions.AllowSleepMode = true;
            qb.MetadataLoadingOptions.OfflineMode = true;
            SetupDbProvider(qb,connectionString,dbType);
            
            try
            {
               // qb.MetadataContainer.AddDatabase("portaldev");
                var server= qb.MetadataContainer.AddServer("MYSQL_SERVER");
                MetadataItem database = server.AddDatabase("portaldev");
                
                qb.MetadataContainer.LoadAll(true);
                qb.MetadataStructure.Refresh();
                qb.MetadataContainer.ExportToXML(path);
            }
            catch (Exception e)
            {
                Log.Error($"Refresh metadata error {connectionString}", e);
                throw;
            }
        }

        private void SetupDbProvider(QueryBuilder qb,string connectionString, SourceDbType dbType)
        {
            if (dbType == SourceDbType.MsSqL)
            {
                qb.SyntaxProvider = new MSSQLSyntaxProvider();
                qb.MetadataProvider = new MSSQLMetadataProvider() {Connection = new SqlConnection(connectionString)};
            }
            
            if (dbType == SourceDbType.MySqL)
            {
                qb.SyntaxProvider = new MySQLSyntaxProvider();
                qb.MetadataProvider = new MySQLMetadataProvider() {Connection = new MySqlConnection(connectionString)};
            }
        }
    }
}