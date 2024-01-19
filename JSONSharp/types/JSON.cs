using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JSONSharp.Visitor;

namespace JSONSharp.types;

public abstract class JSON
{
    public abstract R Accept<R>(IVisitor<R> visitor);
}


/*
        JSON
       /    \
      /      \
JSONValue   JSONObject
- number
- boolean
- array
- null

*/