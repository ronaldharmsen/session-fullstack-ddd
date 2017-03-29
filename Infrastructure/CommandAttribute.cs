using System;

public class CommandAttribute : Attribute {
    public CommandAttribute() {
        
    }

    public string Title {get;set;}
}