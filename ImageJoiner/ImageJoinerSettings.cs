using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessing
{
    [DataContract]
    public class ImageJoinerSettings
    {
        [DataMember]
        public int UpdateStatusTimeout { get; set; }
    }
}
