#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;

public class BrokenSubEmitterSetup : EditorWindow {

    ParticleSystem mainParticle;
    ParticleSystem subParticle;
    ArrayList subParticles;

    bool disabled;
    [MenuItem("Tools/Cibbi tools/BrokenSubEmitterSetup",false,-753)]

    static void Init()
    {
        UnityEditor.EditorWindow window = GetWindow(typeof(BrokenSubEmitterSetup),false,"SubEmitter");
        window.minSize=new Vector2(400, 100);
        window.maxSize=new Vector2(400, 100);
        window.position = new Rect(300, 300, 400, 100);
        
        window.Show();
    }


    public static void ShowWindow() 
    {
        EditorWindow.GetWindow(typeof(BrokenSubEmitterSetup));
    }

    void OnSelectionChange() 
    {
        disabled=true;
        if(Selection.transforms != null)
        {
            subParticles=new ArrayList();
            foreach(Transform transform in Selection.transforms)
            {
                ParticleSystem particle = transform.GetComponent<ParticleSystem>();
                if(particle != null && particle != mainParticle)
                {
                    subParticles.Add(particle);
                    if(disabled)
                    {
                        disabled = false; 
                    }
                }
            }
        }
        Repaint();
    }

    void OnGUI()
    {
        EditorGUILayout.Space();
        mainParticle = (ParticleSystem)EditorGUILayout.ObjectField("Main particle", mainParticle, typeof(ParticleSystem),true);
        if(mainParticle== null)
        {
            disabled=true;
        }
        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Select the subparticles you want to put in the main particle in the hierarchy",MessageType.None);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUI.BeginDisabledGroup(disabled);
        if (GUILayout.Button( "apply selected subparticles"))
        {
           ParticleSystem.SubEmittersModule subModule = mainParticle.GetComponent<ParticleSystem>().subEmitters;
            subModule.enabled=true;
            foreach (ParticleSystem subParticle in subParticles.ToArray())
            {
                subModule.AddSubEmitter(subParticle, ParticleSystemSubEmitterType.Death, ParticleSystemSubEmitterProperties.InheritNothing);   
            }  
        }
        EditorGUI.EndDisabledGroup();

    }


}

#endif