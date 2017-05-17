using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    [DataContract]
    public class ImageJoinerStatus
    {
        [DataMember]
        public int LastFileNumber { get; set; }

        [DataMember]
        public string ServiceName { get; set; }

        [DataMember]
        public DateTime CurrentDate { get; set; }

        [DataMember]
        public int UpdateStatusTimeout { get; set; }
    }
}
