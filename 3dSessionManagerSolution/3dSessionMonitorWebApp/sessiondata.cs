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
    
    public partial class sessiondata
    {
        public int id { get; set; }
        public int sessionId { get; set; }
        public int instanceId { get; set; }
        public string dataType { get; set; }
        public string data { get; set; }
        public byte[] dataBlob { get; set; }
        public System.DateTime timeStamp { get; set; }
        public byte[] processedData { get; set; }
    
        public virtual instance instance { get; set; }
        public virtual session session { get; set; }
    }
}