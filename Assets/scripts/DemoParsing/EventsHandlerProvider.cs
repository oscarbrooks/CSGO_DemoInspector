using DemoInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventsHandlerProvider {
    public static IDemoEventHandler GetEventsHandler(string filePath, Parser parser)
    {
        var header = DemoHeaderParser.ParseHeader(filePath);

        var source = DemoHeaderParser.GetDemoSource(header.Value.ServerName);

        switch (source.Identifier)
        {
            case "esea":
                Debug.Log("ESEA demo events handler");
                return new EseaEventsHandler(parser);
            case "faceit":
                Debug.Log("FACEIT demo events handler");
                return new FaceItEventsHandler(parser);
            default:
                Debug.Log($"{source.Identifier} ({source.Name}) - default events handler");
                return new ValveDemoEventsHandler(parser);
        }
    }
}
