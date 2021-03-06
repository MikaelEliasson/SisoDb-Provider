﻿using System.Collections.Generic;
using System.Data;
using System.Linq;
using SisoDb.Dac;
using SisoDb.DbSchema;
using SisoDb.EnsureThat;
using SisoDb.NCore;
using SisoDb.NCore.Collections;
using SisoDb.PineCone.Structures;
using SisoDb.PineCone.Structures.Schemas;
using SisoDb.SqlServer;

namespace SisoDb.Sql2005
{
    public class Sql2005DbClient : SqlServerDbClient
    {
        protected override int MaxBatchedIdsSize
        {
            get { return 32; }
        }

        public Sql2005DbClient(IAdoDriver driver, ISisoConnectionInfo connectionInfo, IDbConnection connection, IDbTransaction transaction, IConnectionManager connectionManager, ISqlStatements sqlStatements) 
            : base(driver, connectionInfo, connection, transaction, connectionManager, sqlStatements) {}

        public override void DeleteAllExceptIds(IEnumerable<IStructureId> structureIds, IStructureSchema structureSchema)
        {
            Ensure.That(structureSchema, "structureSchema").IsNotNull();
            var sqlFormat = SqlStatements.GetSql("DeleteAllExceptIds").Inject(structureSchema.GetStructureTableName(), "{0}");

            using (var cmd = CreateCommand(string.Empty))
            {
                foreach (var batchedIds in structureIds.Batch<IStructureId, IDacParameter>(MaxBatchedIdsSize, (id, batchCount) => new DacParameter(string.Concat("id", batchCount), id.Value)))
                {
                    cmd.Parameters.Clear();
                    Driver.AddCommandParametersTo(cmd, batchedIds);

                    var paramsString = string.Join(",", batchedIds.Select(p => p.Name));
                    cmd.CommandText = sqlFormat.Inject(paramsString);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public override void DeleteByIds(IEnumerable<IStructureId> ids, IStructureSchema structureSchema)
        {
            Ensure.That(structureSchema, "structureSchema").IsNotNull();
            var sqlFormat = SqlStatements.GetSql("DeleteByIds").Inject(structureSchema.GetStructureTableName(), "{0}");

            using (var cmd = CreateCommand(string.Empty))
            {
                foreach (var batchedIds in ids.Batch<IStructureId, IDacParameter>(MaxBatchedIdsSize, (id, batchCount) => new DacParameter(string.Concat("id", batchCount), id.Value)))
                {
                    cmd.Parameters.Clear();
                    Driver.AddCommandParametersTo(cmd, batchedIds);

                    var paramsString = string.Join(",", batchedIds.Select(p => p.Name));
                    cmd.CommandText = sqlFormat.Inject(paramsString);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public override IEnumerable<string> GetJsonByIds(IEnumerable<IStructureId> ids, IStructureSchema structureSchema)
        {
            Ensure.That(structureSchema, "structureSchema").IsNotNull();
            var sqlFormat = SqlStatements.GetSql("GetJsonByIds").Inject(structureSchema.GetStructureTableName(), "{0}");

            using (var cmd = CreateCommand(string.Empty))
            {
                foreach (var batchedIds in ids.Batch<IStructureId, IDacParameter>(MaxBatchedIdsSize, (id, batchCount) => new DacParameter(string.Concat("id", batchCount), id.Value)))
                {
                    cmd.Parameters.Clear();
                    Driver.AddCommandParametersTo(cmd, batchedIds);

                    var paramsString = string.Join(",", batchedIds.Select(p => p.Name));
                    cmd.CommandText = sqlFormat.Inject(paramsString);

                    using (var reader = cmd.ExecuteReader(CommandBehavior.SingleResult | CommandBehavior.SequentialAccess))
                    {
                        while (reader.Read())
                        {
                            yield return reader.GetString(0);
                        }
                        reader.Close();
                    }
                }
            }
        }
    }
}