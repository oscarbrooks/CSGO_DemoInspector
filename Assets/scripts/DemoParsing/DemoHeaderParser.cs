using System;
using System.IO;
using System.Linq;
using UnityEngine;
using DemoInspector;
using DemoInfo;
using CSharpFunctionalExtensions;

public class DemoHeaderParser {
    
    public static Result<DemoHeader> ParseHeader(string filePath)
    {
        if(!File.Exists(filePath)) return Result.Fail<DemoHeader>("Demo file does not exist");

        using (var filestream = File.OpenRead(filePath))
        {
            using (var parser = new DemoParser(filestream))
            {
                parser.ParseHeader();
                return Result.Ok(parser.Header);
            }
        }
    }

    public static DemoSource GetDemoSource(string name)
    {
        var source = Sources.FirstOrDefault(s => name.Contains(s.Identifier, StringComparison.OrdinalIgnoreCase));

        if (source == null) return new DemoSource() { Identifier = "valve", Name = "MatchMaking" };
        
        return source;
    }

    public static readonly DemoSource[] Sources = new DemoSource[]
    {
        new DemoSource() { Identifier = "valve", Name = "MatchMaking" },
        new DemoSource() { Identifier = "faceit", Name = "FACEIT" },
        new DemoSource() { Identifier = "esea", Name = "ESEA" },
        new DemoSource() { Identifier = "cevo", Name = "Cevo" },
        new DemoSource() { Identifier = "esl", Name = "ESL" },
        new DemoSource() { Identifier = "ebot", Name = "eBot" }
    };

}
