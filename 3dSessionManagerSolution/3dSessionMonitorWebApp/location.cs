//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace _3dSessionMonitorWebApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class location
    {
        public int id { get; set; }
        public int setupId { get; set; }
        public int instanceId { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public System.DateTime creationTimestamp { get; set; }
    
        public virtual instance instance { get; set; }
        public virtual setup setup { get; set; }
    }
}
