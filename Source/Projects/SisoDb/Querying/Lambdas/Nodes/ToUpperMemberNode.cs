using System;
using SisoDb.PineCone.Structures;

namespace SisoDb.Querying.Lambdas.Nodes
{
    [Serializable]
    public class ToUpperMemberNode : MemberNode, IStringOperationMemberNode
    {
        public ToUpperMemberNode(string memberPath, Type dataType, DataTypeCode dataTypeCode)
            : base(memberPath, dataType, dataTypeCode) { }
    }
}