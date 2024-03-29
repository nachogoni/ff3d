using UnityEngine;
using System.Collections;

public class FF3d_DistancePoint {

    Vector3 point;
    float distance;

    public FF3d_DistancePoint(Vector3 p, float d)
    {
        point = p;
        distance = d;
    }

    public Vector3 getPoint()
    {
        return point;
    }

    public float getDistance()
    {
        return distance;
    }

    public override string ToString()
    {
        return point + "(" + distance + ")";
    }

    public void setPoint(Vector3 p)
    {
        point = p;
    }

    public void setDistance(float d)
    {
        distance = d;
    }

    public override bool Equals(object obj)
    {
        //hacer bien el chekeo de tipo
        return (point.Equals(((FF3d_DistancePoint)obj).getPoint())) && (distance.Equals(((FF3d_DistancePoint)obj).getDistance()));
    }
}
