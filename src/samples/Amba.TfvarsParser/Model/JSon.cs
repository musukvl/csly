﻿namespace Amba.TfvarsParser.Model
{
    public abstract class JSon
    {
        public virtual bool IsObject { get; set; }
        public virtual bool IsList { get; set; }
        public virtual bool IsValue { get; set; }
        public virtual bool IsNull { get; set; }
        
    }
}