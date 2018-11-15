using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nuitrack;

[Serializable]
public struct SerializableOrientation {

    public float[] Matrix;

    public static implicit operator SerializableOrientation(Orientation o) {
        return new SerializableOrientation {
            Matrix = (float[]) o.Matrix.Clone()
        };
    }

    public static implicit operator Orientation(SerializableOrientation o) {
        return new Orientation {
            Matrix = (float[]) o.Matrix.Clone()
        };
    }
}
