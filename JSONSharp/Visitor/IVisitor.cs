using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSharp.types;

namespace JSONSharp.Visitor;

public interface IVisitor<R>
{
    R VisitJSONObject(JSONObject json);
    R VisitJSONArray(JSONArray json);
    R VisitJSONNumber(JSONNumber json);
    R VisitJSONString(JSONString json);
    R VisitJSONBool(JSONBool json);
    R VisitJSONNUll(JSONNull json);
}