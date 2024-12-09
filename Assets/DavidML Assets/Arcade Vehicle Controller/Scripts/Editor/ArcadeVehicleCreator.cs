using UnityEngine;
using UnityEditor;
using System;

namespace AVC
{
    public class ArcadeVehicleCreator : EditorWindow
    {
        GameObject preset;
        Transform vehicleParent;
        Transform wheelFL;
        Transform wheelFR;
        Transform wheelRL;
        Transform wheelRR;

        MeshRenderer bodyMesh;
        MeshRenderer wheelMesh;

        private GameObject NewVehicle;


        [MenuItem("Tools/Arcade Vehicle Creator")]
        static void OpenWindow()
        {
            ArcadeVehicleCreator vehicleCreatorWindow = (ArcadeVehicleCreator)GetWindow(typeof(ArcadeVehicleCreator));
            vehicleCreatorWindow.minSize = new Vector2(400, 375);
            vehicleCreatorWindow.titleContent = new GUIContent("Arcade Vehicle Creator");
            vehicleCreatorWindow.Show();
        }

        private void OnGUI()
        {
            Color primaryColor = new Color(0f, 0.78f, 1f);

            GUIStyle headingStyle = new GUIStyle(EditorStyles.boldLabel);
            headingStyle.normal.textColor = primaryColor;

            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 27;
            headerStyle.alignment = TextAnchor.MiddleCenter;
            headerStyle.normal.textColor = primaryColor;
            headerStyle.padding = new RectOffset(1, 1, 1, 1);

            RectOffset padding = new RectOffset(16, 16, 16, 16);
            Rect area = new Rect(padding.right, padding.top, position.width - (padding.right + padding.left), position.height - (padding.top + padding.bottom));
            GUILayout.BeginArea(area);

            GUILayout.Label("Arcade Vehicle Creator", headerStyle);
            GUILayout.Space(16f);

            preset = EditorGUILayout.ObjectField("Vehicle preset", preset, typeof(GameObject), true) as GameObject;

            GUILayout.Space(8f);
            GUILayout.Label("Your Vehicle", headingStyle);
            vehicleParent = EditorGUILayout.ObjectField("Vehicle Parent", vehicleParent, typeof(Transform), true) as Transform;

            GUILayout.Space(8f);
            GUILayout.Label("Wheels", headingStyle);
            wheelFL = EditorGUILayout.ObjectField("Front Left", wheelFL, typeof(Transform), true) as Transform;
            wheelFR = EditorGUILayout.ObjectField("Front Right", wheelFR, typeof(Transform), true) as Transform;
            wheelRL = EditorGUILayout.ObjectField("Rear Left", wheelRL, typeof(Transform), true) as Transform;
            wheelRR = EditorGUILayout.ObjectField("Rear Right", wheelRR, typeof(Transform), true) as Transform;

            GUILayout.Space(8f);
            GUILayout.Label("Meshes", headingStyle);
            bodyMesh = EditorGUILayout.ObjectField("Body Mesh", bodyMesh, typeof(MeshRenderer), true) as MeshRenderer;
            wheelMesh = EditorGUILayout.ObjectField("Wheel Mesh", wheelMesh, typeof(MeshRenderer), true) as MeshRenderer;

            GUILayout.Space(16f);

            if (GUILayout.Button("Create Vehicle", GUILayout.Height(32f)))
            {
                createVehicle();
            }

            GUILayout.EndArea();
        }

        private void createVehicle()
        {
            // Remove all Colliders
            var AllVehicleColliders = vehicleParent.GetComponentsInChildren<Collider>();
            foreach (var collider in AllVehicleColliders)
            {
                DestroyImmediate(collider);
            }

            // Remove all RigidBodies
            var AllRigidBodies = vehicleParent.GetComponentsInChildren<Rigidbody>();
            foreach (var rb in AllRigidBodies)
            {
                DestroyImmediate(rb);
            }

            NewVehicle = Instantiate(preset, bodyMesh.bounds.center, vehicleParent.rotation);
            NewVehicle.name = "Arcade_" + vehicleParent.name;
            DestroyImmediate(NewVehicle.transform.Find("Mesh").Find("Body").GetChild(0).gameObject);
            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL"))
            {
                DestroyImmediate(NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL").Find("WheelFL Axel").GetChild(0).gameObject);
            }
            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR"))
            {
                DestroyImmediate(NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR").Find("WheelFR Axel").GetChild(0).gameObject);
            }
            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL"))
            {
                DestroyImmediate(NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL").Find("WheelRL Axel").GetChild(0).gameObject);
            }
            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR"))
            {
                DestroyImmediate(NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR").Find("WheelRR Axel").GetChild(0).gameObject);
            }

            vehicleParent.parent = NewVehicle.transform.Find("Mesh").Find("Body");

            NewVehicle.transform.Find("Mesh").transform.Find("Wheels").position = vehicleParent.position;

            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL"))
            {
                NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL").position = wheelFL.position;
                wheelFL.parent = NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFL").Find("WheelFL Axel");
            }
            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR"))
            {
                NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR").position = wheelFR.position;
                wheelFR.parent = NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelFR").Find("WheelFR Axel");
            }
            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL"))
            {
                NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL").position = wheelRL.position;
                wheelRL.parent = NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRL").Find("WheelRL Axel");
            }
            if (NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR"))
            {
                NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR").position = wheelRR.position;
                wheelRR.parent = NewVehicle.transform.Find("Mesh").transform.Find("Wheels").Find("WheelRR").Find("WheelRR Axel");
            }

            // Adjust colliders
            if (NewVehicle.GetComponent<BoxCollider>())
            {
                NewVehicle.GetComponent<BoxCollider>().center = Vector3.zero;
                NewVehicle.GetComponent<BoxCollider>().size = bodyMesh.bounds.size;
            }

            if (NewVehicle.GetComponent<CapsuleCollider>())
            {
                NewVehicle.GetComponent<CapsuleCollider>().center = Vector3.zero;
                NewVehicle.GetComponent<CapsuleCollider>().height = bodyMesh.bounds.size.z;
                NewVehicle.GetComponent<CapsuleCollider>().radius = bodyMesh.bounds.size.x / 2;

            }

            Vector3 SpheareRBOffset = new Vector3(NewVehicle.transform.position.x,
                                                  wheelFL.position.y + bodyMesh.bounds.extents.y - wheelMesh.bounds.size.y / 2,
                                                  NewVehicle.transform.position.z);

            if (NewVehicle.transform.Find("SphereRB"))
            {
                NewVehicle.transform.Find("SphereRB").GetComponent<SphereCollider>().radius = bodyMesh.bounds.extents.y;
                NewVehicle.transform.Find("SphereRB").position = SpheareRBOffset;
            }

            NewVehicle.transform.Find("Effects").transform.Find("RL_SKID").position = wheelRL.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
            NewVehicle.transform.Find("Effects").transform.Find("RR_SKID").position = wheelRR.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
            NewVehicle.transform.Find("Effects").transform.Find("RL_SMOKE").position = wheelRL.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
            NewVehicle.transform.Find("Effects").transform.Find("RR_SMOKE").position = wheelRR.position - Vector3.up * (wheelMesh.bounds.size.y / 2 - 0.02f);
        }
    }
}