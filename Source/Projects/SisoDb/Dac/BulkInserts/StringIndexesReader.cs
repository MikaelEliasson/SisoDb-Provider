using System;
using System.Collections.Generic;
using SisoDb.DbSchema;
using SisoDb.PineCone.Structures;

namespace SisoDb.Dac.BulkInserts
{
	public class StringIndexesReader : IndexesReader
	{
		public StringIndexesReader(IndexStorageSchema storageSchema, IEnumerable<IStructureIndex> items)
			: base(storageSchema, items)
		{ }

		public override object GetValue(int ordinal)
		{
            if (ordinal == IndexStorageSchema.Fields.RowId.Ordinal)
                return DBNull.Value;

			if (ordinal == IndexStorageSchema.Fields.StructureId.Ordinal)
				return Enumerator.Current.StructureId.Value;

			if (ordinal == IndexStorageSchema.Fields.MemberPath.Ordinal)
				return Enumerator.Current.Path;

			if (ordinal == IndexStorageSchema.Fields.Value.Ordinal)
				return Enumerator.Current.Value;

			throw new NotSupportedException();
		}
	}
}