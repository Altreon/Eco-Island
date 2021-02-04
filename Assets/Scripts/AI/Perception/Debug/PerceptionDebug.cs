#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomEditor (typeof(Agent))]
public class PerceptionDebug : Editor
{
    public void OnSceneGUI()
    {
        Agent fow = (Agent)target;
        if(fow.Debug)
        {
            // View
            Handles.color = Color.white;
            Handles.DrawWireArc(fow.transform.position, Vector3.up,Vector3.forward,360,fow.PerceptionConfig.viewRadius);
            Vector3 viewAngleA = DirFromAngle(fow,-fow.PerceptionConfig.viewAngle / 2, false);
            Vector3 viewAngleB = DirFromAngle(fow,fow.PerceptionConfig.viewAngle / 2, false);

            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.PerceptionConfig.viewRadius);
            Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.PerceptionConfig.viewRadius);

            //Sound
            Handles.color = Color.red;

            if(fow.SoundEmission != null)
            {
                Handles.DrawWireArc(fow.transform.position, Vector3.up,Vector3.forward,360,fow.SoundEmission.CurrentSoundEmissionRadius);
            }
            else
            {
                Handles.DrawWireArc(fow.transform.position, Vector3.up,Vector3.forward,360,fow.PerceptionConfig.MaxSoundEmissionRadius);
            }
        }
    }

    public Vector3 DirFromAngle(Agent fow, float angleInDegress, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegress += fow.transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegress * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegress * Mathf.Deg2Rad));
    }
}
#endif