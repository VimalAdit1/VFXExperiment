using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    public LineRenderer line;
    public ParticleSystem ps;
    public int emissionsPerFrame = 5;

    void Update()
    {
        var emitParams = new ParticleSystem.EmitParams();

        for (int i = 0; i < emissionsPerFrame; i++)
        {
            float t = Random.value;

            Vector3 point = GetPointOnLine(t);
            Vector3 dir = GetLineDirection(t);

            emitParams.position = point;

            // Add slight outward motion (like sparks flying off)
            emitParams.velocity = dir * 0.5f + Random.insideUnitSphere * 0.3f;

            ps.Emit(emitParams, 1);
        }
    }

    Vector3 GetPointOnLine(float t)
    {
        int count = line.positionCount - 1;
        float scaled = t * count;

        int i = Mathf.FloorToInt(scaled);
        float localT = scaled - i;

        Vector3 a = line.GetPosition(i);
        Vector3 b = line.GetPosition(i + 1);

        return Vector3.Lerp(a, b, localT);
    }

    Vector3 GetLineDirection(float t)
    {
        int count = line.positionCount - 1;
        float scaled = t * count;

        int i = Mathf.Clamp(Mathf.FloorToInt(scaled), 0, count - 1);

        Vector3 a = line.GetPosition(i);
        Vector3 b = line.GetPosition(i + 1);

        return (b - a).normalized;
    }
}


